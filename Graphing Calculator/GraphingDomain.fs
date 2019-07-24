namespace GraphingCalculator

// Using types and functions from Conventional Domain
// that are common to the Graphing data Domain.
open GraphingCalculator.ConventionalDomain
open Math.Pure.Objects
open Math.Pure.Quantity 
open Math.Pure.Structure


module GraphingDomain =
    
    // Parameters

    type X = X of NumberType
    type Y = Y of NumberType
    type Z = Z of NumberType
    type Size = {height:NumberType; width:NumberType}
    type SweepDirection =
            /// positive-angle direction
        | Clockwise
            /// negative-angle direction
        | CounterClockwise
    type Drawing2DBounds = {upperX : X; lowerX : X; upperY : Y; lowerY : Y}

    // Geometry
    
    type Point = 
        | Point of X * Y  //-> System.Windows.Point //{x:Coordinate; y:Coordinate}
        | Point3D of X * Y * Z

    type ArcSegment =
        { Point : Point
          Size : Size
          RotationAngle : NumberType
          IsLargeArc : bool
          SweepDirection : SweepDirection }    
    type BezierSegment =
        { Point1 : Point
          Point2 : Point
          Point3 : Point }
    type QuadraticBezierSegment =
        { Point1 : Point
          Point2 : Point }
     
    type PathGeometry =
        /// Creates a line between two points.
        | LineSegment of Point        
        /// Creates an elliptical arc between two points.
        | ArcSegment of ArcSegment
        /// Creates a cubic Bezier curve between two points.
        | BezierSegment of BezierSegment
        /// Creates a quadratic Bezier curve.
        | QuadraticBezierSegment of QuadraticBezierSegment
        /// Creates a series of lines.
        | PolyLineSegment of seq<Point>        
        /// Creates a series of cubic Bezier curves.
        | PolyBezierSegment of seq<Point>        
        /// Creates a series of quadratic Bezier curves.
        | PolyQuadraticBezierSegment  of seq<Point>         

    type Trace = {startPoint:Point; traceSegments:PathGeometry list}
    
    // Drawing

    type DrawOp = 
        Draw2D of Trace * Drawing2DBounds
     
    // types to describe errors
    type DrawError = 
        | Error of Error
        | FailedToCreateTrace
        | LazyCoder        
       
    type DrawOperationResult =  
        | Trace of Trace 
        | DrawError of DrawError

    type ExpressionOperationResult =  
        | ExpressionSuccess of Expression 
        | ExpressionError of Error

    type ExpressionInput =
        | Symbol of Symbol
        | Function of Function        

    type PendingFunction = (Expression * Function)

    // data associated with each state    
    
    type ExpressionStateData = 
        { expression : Expression
          pendingFunction:PendingFunction option;
          digits:ConventionalDomain.DigitAccumulator }
    type EvaluatedStateData =
        { evaluatedExpression:Expression; 
          pendingFunction:PendingFunction option }
    type DrawStateData =  {traceExpression:Expression; trace:Trace; pendingFunction:PendingFunction option;}    
    type ErrorStateData = {lastExpression:Expression; error:DrawError}

    type CalculatorInput =
        | ExpressionInput of ExpressionInput
        | CalcInput of ConventionalDomain.CalculatorInput
        | Stack of RpnDomain.CalculatorInput
        | Draw
        | EnterExpression

    // States
    type CalculatorState =         
        | EvaluatedState of EvaluatedStateData
        | DrawState of DrawStateData
        | ExpressionDigitAccumulatorState of ExpressionStateData
        | ExpressionDecimalAccumulatorState of ExpressionStateData
        | DrawErrorState of ErrorStateData
        | ExpressionErrorState of ErrorStateData
      
    type Evaluate = CalculatorInput * CalculatorState -> CalculatorState

    // Services used by the calculator itself
    type DoDrawOperation = Expression -> DrawOperationResult
    type DoExpressionOperation = Function*Expression*Expression -> Result<Expression>
    type GetDisplayFromExpression = Expression -> string
    type GetNumberFromAccumulator = ExpressionStateData -> NumberType
    type GetDisplayFromGraphState = CalculatorState -> string
    
    type GraphServices = {        
        doDrawOperation :DoDrawOperation
        doExpressionOperation :DoExpressionOperation
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        getNumberFromAccumulator :GetNumberFromAccumulator
        getDisplayFromExpression :GetDisplayFromExpression
        getDisplayFromGraphState :GetDisplayFromGraphState
        }

module EvaluateExpression = 
    open ExpressionStructure
    let for_X = fun (e:Expression) (n:NumberType) ->        
        let x = 
            match (variables e).Length = 1 with
            | true -> substitute (Symbol (variables e).Head, Number n ) e
            | false -> 
                match e.isNumber with
                | true -> e
                //I need to throw an error here, but I'll get back to this later.
                | false ->  substitute ( Symbol (Variable "x"), Number n ) e 
        x |> ExpressionType.simplifyExpression

    let getErrorFrom = fun e -> 
        match e with
        | Symbol (Error e) -> e | _ -> OtherError

module GraphingImplementation =    
    open GraphingDomain
    open Utilities

    let accumulateNonZeroDigit services digit accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateNonZeroDigit (digit, digits)
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let accumulateZero services accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateZero digits
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let accumulateSeparator services accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateSeparator digits
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let getEvaluationState services (expressionStateData:ExpressionStateData) nextFunc = 

        // helper to create a new EvaluatedState from a given displayExpression 
        // and the nextOp parameter
        let getNewState displayExpression =
            let newPendingFunc = 
                nextFunc |> Option.map (fun func -> displayExpression, func )
            {evaluatedExpression = displayExpression; pendingFunction=newPendingFunc}
            |> EvaluatedState

        let currentNumber = 
            services.getNumberFromAccumulator expressionStateData 

        // If there is no pending function, create a new ExpressionState using the currentNumber
        let computeStateWithNoPendingOp = getNewState (Number currentNumber)

        maybe {            
            let! (previousExpression,func) = expressionStateData.pendingFunction
            let result = services.doExpressionOperation(func,previousExpression,Number currentNumber)
            let newState =
                match result with
                | Pass resultExpression ->
                    // If there was a pending op, create a new ExpressionState using the result
                    getNewState resultExpression 
                | Fail error -> ExpressionErrorState {lastExpression = previousExpression; error = Error (EvaluateExpression.getErrorFrom error)}
            return newState
            } |> ifNone computeStateWithNoPendingOp 

    let doDrawOperation services expression =        
        let result = services.doDrawOperation expression 
        match result with
        | Trace t -> DrawState {traceExpression = expression; trace = t; pendingFunction = None}
        | DrawError x -> DrawErrorState {lastExpression = expression; error = x}  

    let doExpressionOperation services stateData = 
        let result = services.doExpressionOperation stateData
        match result with
        | Pass r -> ExpressionSuccess r
        | Fail e -> ExpressionError (EvaluateExpression.getErrorFrom e)

    let replacePendingFunction (expressionStateData:ExpressionStateData) nextFun = 
        let newPending = maybe {
            let! expression,existingFunc  = expressionStateData.pendingFunction
            let! next = nextFun
            return expression,next
            }
        {expressionStateData with pendingFunction=newPending}
        |> ExpressionDigitAccumulatorState

    let handleGrahphState services stateData input =
        let expr = stateData.traceExpression        
        let newExpressionStateData =  
                {expression=expr;  
                 pendingFunction=None;
                 digits=""}
        match input with
        | Stack _ -> DrawState stateData
        | CalcInput op -> 
            match op with
            | MathOp m -> 
                match m with
                | Add ->      replacePendingFunction newExpressionStateData (Some Plus)
                | Subtract -> replacePendingFunction newExpressionStateData (Some Minus)
                | Multiply -> replacePendingFunction newExpressionStateData (Some Times)
                | Divide ->   replacePendingFunction newExpressionStateData (Some DividedBy)
                | CalculatorMathOp.Inverse ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Inverse); 
                              digits = "1" } nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression }
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | Percent 
                | CalculatorMathOp.Root 
                | ChangeSign                    
                | MemoryAdd 
                | MemorySubtract -> DrawState stateData         
            | CalculatorInput.Zero -> ExpressionDigitAccumulatorState newExpressionStateData                    
            | DecimalSeparator -> 
                newExpressionStateData 
                |> accumulateSeparator services
                |> ExpressionDigitAccumulatorState 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> ExpressionDigitAccumulatorState newExpressionStateData
            | Digit d -> 
                newExpressionStateData
                |> accumulateNonZeroDigit services d
                |> ExpressionDigitAccumulatorState 
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> DrawState stateData
            | Function f -> DrawState stateData
        | Draw -> DrawState stateData
        | EnterExpression -> DrawState stateData

    let handleExpressionDigitAccumulatorState services stateData input =
        match input with
        | CalcInput op -> 
            match op with            
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | CalculatorMathOp.Inverse 
                | Percent 
                | CalculatorMathOp.Root 
                | ChangeSign                    
                | MemoryAdd 
                | MemorySubtract -> {lastExpression = stateData.expression; error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {lastExpression = stateData.expression; error = LazyCoder}
            | Digit d -> {lastExpression = stateData.expression; error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {lastExpression = stateData.expression; error = LazyCoder}
            | Function f -> {lastExpression = stateData.expression; error = LazyCoder}
        | Draw -> {lastExpression = stateData.expression; error = LazyCoder}
        | EnterExpression -> {lastExpression = stateData.expression; error = LazyCoder}

    let handleExpressionDecimalAccumulatorState services stateData input =
        match input with
        | CalcInput op -> 
            match op with            
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | CalculatorMathOp.Inverse 
                | Percent 
                | CalculatorMathOp.Root 
                | ChangeSign                    
                | MemoryAdd 
                | MemorySubtract -> {lastExpression = stateData.expression; error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {lastExpression = stateData.expression; error = LazyCoder}
            | Digit d -> {lastExpression = stateData.expression; error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {lastExpression = stateData.expression; error = LazyCoder}
            | Function f -> {lastExpression = stateData.expression; error = LazyCoder}
        | Draw -> {lastExpression = stateData.expression; error = LazyCoder}
        | EnterExpression -> {lastExpression = stateData.expression; error = LazyCoder}

    let handleDrawErrorState services stateData input =
        match input with
        | CalcInput op -> 
            match op with            
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | CalculatorMathOp.Inverse 
                | Percent 
                | CalculatorMathOp.Root 
                | ChangeSign                    
                | MemoryAdd 
                | MemorySubtract -> {lastExpression = stateData.expression; error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {lastExpression = stateData.expression; error = LazyCoder}
            | Digit d -> {lastExpression = stateData.expression; error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {lastExpression = stateData.expression; error = LazyCoder}
            | Function f -> {lastExpression = stateData.expression; error = LazyCoder}
        | Draw -> {lastExpression = stateData.expression; error = LazyCoder}
        | EnterExpression -> {lastExpression = stateData.expression; error = LazyCoder}



             
