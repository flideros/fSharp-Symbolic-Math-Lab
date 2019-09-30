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
    type DrawOp = Expression * Drawing2DBounds
     
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

    type Parenthetical = Parenthetical of (Expression * PendingFunction option * Parenthetical option)

    // data associated with each state        
    type ExpressionStateData = 
        { expression : Expression;
          pendingFunction : PendingFunction option;
          parenthetical : Parenthetical option;
          digits : ConventionalDomain.DigitAccumulator;
          drawing2DBounds : Drawing2DBounds }
    type EvaluatedStateData =
        { evaluatedExpression : Expression
          //parenthetical : Expression option;
          pendingFunction : PendingFunction option;
          drawing2DBounds : Drawing2DBounds}
    type DrawStateData =  
        { traceExpression : Expression; 
          trace:Trace; 
          drawing2DBounds : Drawing2DBounds}    
    type ErrorStateData = 
        { lastExpression : Expression; 
          error : DrawError }

    type CalculatorInput =
        | ExpressionInput of ExpressionInput
        | CalcInput of ConventionalDomain.CalculatorInput
        | Stack of RpnDomain.CalculatorInput
        | Draw
        | OpenParentheses

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
    type AccumulateSymbol = ExpressionStateData -> Symbol -> Expression //StateData
    type DoDrawOperation = DrawOp-> DrawOperationResult
    type DoExpressionOperation = Function * Expression * Expression -> ExpressionOperationResult
    type GetDisplayFromExpression = Expression -> string
    type GetNumberFromAccumulator = ExpressionStateData -> NumberType
    type GetDisplayFromGraphState = CalculatorState -> string
    type GetErrorFromExpression = Expression -> Error
    type GetDrawing2DBounds = CalculatorState -> Drawing2DBounds
    type GetDisplayFromPendingFunction = PendingFunction option -> string
    
    type GetExpressionFromParenthetical = Parenthetical option -> Expression
    type GetParentheticalFromExpression = Expression -> Parenthetical
    type GetParentheticalFromCalculatorState = CalculatorState -> Parenthetical
    type SetExpressionToParenthetical = Expression * (Parenthetical option) -> Parenthetical

    type GraphServices = {        
        doDrawOperation :DoDrawOperation
        doExpressionOperation :DoExpressionOperation
        accumulateSymbol :AccumulateSymbol
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        getNumberFromAccumulator :GetNumberFromAccumulator
        getDisplayFromExpression :GetDisplayFromExpression
        getDisplayFromGraphState :GetDisplayFromGraphState
        getDrawing2DBounds :GetDrawing2DBounds
        getDisplayFromPendingFunction :GetDisplayFromPendingFunction
        getExpressionFromParenthetical :GetExpressionFromParenthetical
        getParentheticalFromExpression :GetParentheticalFromExpression
        getParentheticalFromCalculatorState :GetParentheticalFromCalculatorState
        setExpressionToParenthetical :SetExpressionToParenthetical
        }

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

    let accumulateSymbol services symbol expressionStateData  =
        let newAccumulatorData = services.accumulateSymbol expressionStateData symbol
        newAccumulatorData // return

    let getEvaluationState services (expressionStateData:ExpressionStateData) nextFunc = 

        // helper to create a new EvaluatedState from a given displayExpression 
        // and the nextOp parameter
        let getNewState displayExpression =
            let bounds = services.getDrawing2DBounds (ExpressionDigitAccumulatorState expressionStateData)
            let newPendingFunc = 
                nextFunc 
                |> Option.map ( fun func -> displayExpression, func )
            {evaluatedExpression = displayExpression; 
             //parenthetical = None;
             pendingFunction = newPendingFunc;
             drawing2DBounds = bounds }
            |> EvaluatedState

        let currentExpression = 
            let number = services.getNumberFromAccumulator expressionStateData |> Number
            match expressionStateData.parenthetical with
            | None -> number
            | Some x -> services.getExpressionFromParenthetical expressionStateData.parenthetical

        // If there is no pending function, create a new ExpressionState using the currentNumber                                                                                                              
        let computeStateWithNoPendingOp = getNewState currentExpression

        maybe {            
            let! (previousExpression,func) = expressionStateData.pendingFunction
            let result = services.doExpressionOperation (func,previousExpression, currentExpression)
            let newState =
                match result with
                | ExpressionSuccess resultExpression ->
                    // If there was a pending op, create a new ExpressionState using the result
                    getNewState resultExpression 
                | ExpressionError error -> 
                    {lastExpression = previousExpression; 
                     error = error |> Error} 
                    |> ExpressionErrorState
            return newState
            } |> ifNone computeStateWithNoPendingOp 
    
    let doDrawOperation services drawOp =        
        let result = services.doDrawOperation drawOp 
        let expression, bounds = drawOp
        match result with
        | Trace t -> DrawState {traceExpression = expression; trace = t; drawing2DBounds = bounds}
        | DrawError x ->  
            {lastExpression = expression; 
             error = x} 
            |> DrawErrorState 

    let doExpressionOperation services stateData = services.doExpressionOperation stateData
        
    let replacePendingFunction (evaluatedStateData:EvaluatedStateData) nextFun =         
        let newPending = maybe {
            let! expression, _existingFunc = evaluatedStateData.pendingFunction
            let! next = nextFun
            return expression,next }
        match newPending, nextFun with 
        | Some _, _ -> {evaluatedStateData with pendingFunction=newPending}
        | None, Some func -> {evaluatedStateData with pendingFunction=Some (evaluatedStateData.evaluatedExpression,func)}
        | _ -> evaluatedStateData
        |> EvaluatedState 

    let handleDrawState services stateData input = //Only the'Back' function implimented here.
        let bounds = services.getDrawing2DBounds (DrawState stateData)
        let expr = stateData.traceExpression        
        let newEvaluatedStateData =  
                {evaluatedExpression=expr;  
                 pendingFunction=None;
                 drawing2DBounds = bounds }
        match input with
        | Stack _ -> DrawState stateData 
        | CalcInput op -> 
            match op with
            | MathOp m -> DrawState stateData    
            | CalculatorInput.Zero -> DrawState stateData               
            | DecimalSeparator -> DrawState stateData 
            | CalculatorInput.Equals -> DrawState stateData
            | ClearEntry
            | Clear                         
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back ->
                newEvaluatedStateData
                |> EvaluatedState 
            | Digit d -> DrawState stateData
        | ExpressionInput input -> DrawState stateData
        | Draw -> DrawState stateData
        | OpenParentheses -> DrawState stateData
        
    let handleEvaluatedState services stateData input =
        let bounds = services.getDrawing2DBounds (EvaluatedState stateData)
        let zero = Number (Integer 0I)
        let expr = stateData.evaluatedExpression        
        let newExpressionStateData =  
                {expression = expr;  
                 pendingFunction = stateData.pendingFunction;
                 digits = "";
                 drawing2DBounds = bounds;
                 parenthetical = None}
        match input with
        | Stack _ -> EvaluatedState stateData
        | CalcInput op -> 
            match op with
            | MathOp m -> 
                match m with
                | Add -> replacePendingFunction stateData (Some Plus)                    
                | Subtract -> replacePendingFunction stateData (Some Minus)                    
                | Multiply -> replacePendingFunction stateData (Some Times)                    
                | Divide -> replacePendingFunction stateData (Some DividedBy)                    
                | CalculatorMathOp.Inverse ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Inverse); 
                              digits = "1";
                              drawing2DBounds = bounds;
                              parenthetical = None
                            } nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawing2DBounds = bounds;
                                      parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | Percent ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Quotient);
                              digits = "100";
                              drawing2DBounds = bounds;
                              parenthetical = None
                            } nextOp
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev ->                                 
                                { digits = ""; 
                                  pendingFunction = stateData.pendingFunction; 
                                  expression=ev.evaluatedExpression;
                                  drawing2DBounds = bounds;
                                  parenthetical = None 
                                } |> ExpressionDigitAccumulatorState 
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | CalculatorMathOp.Root ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Root); 
                              digits = "0.5";
                              drawing2DBounds = bounds;
                              parenthetical = None 
                            } nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawing2DBounds = bounds;
                                      parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | ChangeSign ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Times); 
                              digits = "-1";
                              drawing2DBounds = bounds;
                              parenthetical = None
                            } nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawing2DBounds = bounds;
                                      parenthetical = None 
                                    }
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | MemoryAdd 
                | MemorySubtract -> EvaluatedState stateData         
            | CalculatorInput.Zero -> stateData |> EvaluatedState                     
            | DecimalSeparator -> 
                match stateData.pendingFunction with
                | None -> stateData |> EvaluatedState
                | Some _ ->
                    newExpressionStateData 
                    |> accumulateSeparator services
                    |> ExpressionDecimalAccumulatorState 
            | CalculatorInput.Equals -> replacePendingFunction stateData None                
            | ClearEntry ->                 
                    match stateData.pendingFunction with
                    | None -> stateData |> EvaluatedState
                    | Some po -> 
                        let _oldExpression, oldPendingFunction = po
                        let zero = Number (Integer 0I)
                        {stateData with pendingFunction = Some (zero,oldPendingFunction)}
                        |> EvaluatedState
            | Clear -> 
                {evaluatedExpression = zero; 
                 pendingFunction=None;
                 drawing2DBounds=stateData.drawing2DBounds}
                |> EvaluatedState
            | MemoryRecall -> EvaluatedState stateData //not used
            | MemoryClear -> EvaluatedState stateData //not used
            | MemoryStore -> EvaluatedState stateData //not used
            | Back -> EvaluatedState stateData 
            | Digit d -> 
                match stateData.pendingFunction with
                | None ->                    
                    newExpressionStateData
                    |> accumulateNonZeroDigit services d
                    |> ExpressionDigitAccumulatorState
                | Some _ ->                    
                    newExpressionStateData
                    |> accumulateNonZeroDigit services d
                    |> ExpressionDigitAccumulatorState 
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> 
                match stateData.pendingFunction with
                | None -> 
                    { expression = expr;
                      pendingFunction = None; 
                      digits = "";
                      drawing2DBounds = bounds;
                      parenthetical = services.getParentheticalFromExpression (Expression.Symbol s) |> Some 
                    } 
                    |> ExpressionDigitAccumulatorState
                | Some pendingfunc ->                    
                    {expression = expr; 
                     pendingFunction = Some pendingfunc;
                     digits = "";
                     parenthetical = services.getParentheticalFromExpression (Expression.Symbol s) |> Some;
                     drawing2DBounds = stateData.drawing2DBounds}
                    |> ExpressionDigitAccumulatorState
            | Function f -> replacePendingFunction stateData (Some f)
        | Draw -> 
            (stateData.evaluatedExpression,stateData.drawing2DBounds)
            |> doDrawOperation services 
        | OpenParentheses -> 
            {newExpressionStateData with 
                parenthetical = 
                    (services.getParentheticalFromCalculatorState (ExpressionDigitAccumulatorState newExpressionStateData)) 
                    |> Some} 
            |> ExpressionDigitAccumulatorState
        
    let handleExpressionDigitAccumulatorState services stateData input =           
           let bounds = services.getDrawing2DBounds (ExpressionDigitAccumulatorState stateData)
           let zero = Number (Integer 0I)
           let expr = stateData.expression        
           let newExpressionStateData =  
                   {expression=expr;  
                    pendingFunction=stateData.pendingFunction;
                    digits="";
                    drawing2DBounds = bounds;
                    parenthetical = None}           
           match input with
           | Stack _ -> ExpressionDigitAccumulatorState stateData
           | CalcInput op -> 
               match op with
               | MathOp m -> 
                   match m with
                   | Add ->      getEvaluationState services stateData (Some Plus)                    
                   | Subtract -> getEvaluationState services stateData (Some Minus)                    
                   | Multiply -> getEvaluationState services stateData (Some Times)                    
                   | Divide ->   getEvaluationState services stateData (Some DividedBy)                    
                   | CalculatorMathOp.Inverse ->
                       let nextOp = None//Some op
                       let newState = 
                           getEvaluationState services 
                               { expression = expr;
                                 pendingFunction = Some (expr,Inverse); 
                                 digits = "1";
                                 drawing2DBounds = bounds;
                                 parenthetical = None} nextOp 
                       let finalState =
                           match stateData.pendingFunction = None with
                           | true -> newState
                           | false -> 
                               match newState with                            
                               | EvaluatedState ev -> 
                                   ExpressionDigitAccumulatorState 
                                       { digits = ""; 
                                         pendingFunction = stateData.pendingFunction; 
                                         expression=ev.evaluatedExpression;
                                         drawing2DBounds = bounds;
                                         parenthetical = None}
                               | ExpressionDigitAccumulatorState _ 
                               | ExpressionDecimalAccumulatorState _                             
                               | DrawState _
                               | DrawErrorState _
                               | ExpressionErrorState _ -> newState
                       finalState
                   | Percent ->
                       let nextOp = None//Some op
                       let newState = 
                           getEvaluationState services 
                               { expression = expr;
                                 pendingFunction = Some (expr,Function.Quotient); 
                                 digits = "100";
                                 drawing2DBounds = bounds;
                                 parenthetical = None} nextOp 
                       let finalState =
                           match stateData.pendingFunction = None with
                           | true -> newState
                           | false -> 
                               match newState with                            
                               | EvaluatedState ev -> 
                                   ExpressionDigitAccumulatorState 
                                       { digits = ""; 
                                         pendingFunction = stateData.pendingFunction; 
                                         expression=ev.evaluatedExpression;
                                         drawing2DBounds = bounds;
                                         parenthetical = None}
                               | ExpressionDigitAccumulatorState _ 
                               | ExpressionDecimalAccumulatorState _                             
                               | DrawState _
                               | DrawErrorState _
                               | ExpressionErrorState _ -> newState
                       finalState
                   | CalculatorMathOp.Root ->
                       let nextOp = None//Some op
                       let newState = 
                           getEvaluationState services 
                               { expression = expr;
                                   pendingFunction = Some (expr,Function.ToThePowerOf); 
                                   digits = "0.5";
                                   drawing2DBounds = bounds;
                                   parenthetical = None} nextOp 
                       let finalState =
                           match stateData.pendingFunction = None with
                           | true -> newState
                           | false -> 
                               match newState with                            
                               | EvaluatedState ev -> 
                                   ExpressionDigitAccumulatorState 
                                       { digits = ""; 
                                         pendingFunction = stateData.pendingFunction; 
                                         expression=ev.evaluatedExpression;
                                         drawing2DBounds = bounds;
                                         parenthetical = None}
                               | ExpressionDigitAccumulatorState _ 
                               | ExpressionDecimalAccumulatorState _                             
                               | DrawState _
                               | DrawErrorState _
                               | ExpressionErrorState _ -> newState
                       finalState
                   | ChangeSign ->
                       let nextOp = None//Some op
                       let newState = 
                           getEvaluationState services 
                               { expression = expr;
                                   pendingFunction = Some (expr,Function.Times); 
                                   digits = "-1";
                                   drawing2DBounds = bounds;
                                   parenthetical = None} nextOp 
                       let finalState =
                           match stateData.pendingFunction = None with
                           | true -> newState
                           | false -> 
                               match newState with                            
                               | EvaluatedState ev -> 
                                   ExpressionDigitAccumulatorState 
                                       { digits = ""; 
                                           pendingFunction = stateData.pendingFunction; 
                                           expression=ev.evaluatedExpression;
                                           drawing2DBounds = bounds;
                                           parenthetical = None}
                               | ExpressionDigitAccumulatorState _ 
                               | ExpressionDecimalAccumulatorState _                             
                               | DrawState _
                               | DrawErrorState _
                               | ExpressionErrorState _ -> newState
                       finalState
                   | MemoryAdd 
                   | MemorySubtract -> ExpressionDigitAccumulatorState stateData         
               | CalculatorInput.Zero -> 
                   stateData
                   |> accumulateZero services 
                   |> ExpressionDigitAccumulatorState                     
               | DecimalSeparator -> 
                   stateData 
                   |> accumulateSeparator services
                   |> ExpressionDecimalAccumulatorState 
               | CalculatorInput.Equals -> getEvaluationState services stateData None
               | ClearEntry ->                 
                       match stateData.pendingFunction with
                       | None -> stateData |> ExpressionDigitAccumulatorState
                       | Some po -> 
                           let _oldExpression, oldPendingFunction = po
                           let zero = Number (Integer 0I)
                           {stateData with pendingFunction = Some (zero,oldPendingFunction)}
                           |> ExpressionDigitAccumulatorState
               | Clear -> 
                  {evaluatedExpression = zero; 
                   pendingFunction = None;
                   drawing2DBounds = bounds } 
                  |> EvaluatedState
               | MemoryRecall -> ExpressionDigitAccumulatorState stateData //not used
               | MemoryClear -> ExpressionDigitAccumulatorState stateData //not used
               | MemoryStore -> ExpressionDigitAccumulatorState stateData //not used
               | Back -> // I need to come back to this function to incorporate expression type
                    match stateData.digits.Length with
                    | x when x <= 1 -> newExpressionStateData |> ExpressionDigitAccumulatorState
                    | x -> {newExpressionStateData with digits = stateData.digits.Remove(x-1)}
                           |> ExpressionDigitAccumulatorState  
               | Digit d -> 
                   stateData
                   |> accumulateNonZeroDigit services d
                   |> ExpressionDigitAccumulatorState 
           | ExpressionInput input -> 
               match input with 
               | Symbol s -> 
                   match stateData.pendingFunction with
                   | None -> 
                       { expression = expr;
                         pendingFunction = None; 
                         digits = "";
                         parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) |> Some;
                         drawing2DBounds = bounds} 
                       |> ExpressionDigitAccumulatorState
                   | Some pendingfunc ->                    
                       { expression = expr; 
                         pendingFunction = Some pendingfunc;
                         digits = "";
                         parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) |> Some;
                         drawing2DBounds = stateData.drawing2DBounds}
                       |> ExpressionDigitAccumulatorState
               | Function f -> getEvaluationState services stateData (Some f)
           | Draw -> doDrawOperation services (DrawOp (stateData.expression, bounds))
           | OpenParentheses -> 
                {stateData with 
                    parenthetical = 
                        (services.getParentheticalFromCalculatorState (ExpressionDigitAccumulatorState stateData)) 
                        |> Some} 
                |> ExpressionDigitAccumulatorState

    let handleExpressionDecimalAccumulatorState services stateData input =
        let bounds = services.getDrawing2DBounds (ExpressionDecimalAccumulatorState stateData)
        let zero = Number (Integer 0I)
        let expr = stateData.expression        
        let newExpressionStateData =  
                {expression=expr;  
                 pendingFunction=stateData.pendingFunction;
                 digits="";
                 drawing2DBounds = bounds;
                 parenthetical = None}
        match input with
        | Stack _ -> ExpressionDecimalAccumulatorState stateData
        | CalcInput op -> 
            match op with
            | MathOp m -> 
                match m with
                | Add -> getEvaluationState services stateData (Some Plus)                    
                | Subtract -> getEvaluationState services stateData (Some Minus)                    
                | Multiply -> getEvaluationState services stateData (Some Times)                    
                | Divide -> getEvaluationState services stateData (Some DividedBy)                    
                | CalculatorMathOp.Inverse ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Inverse); 
                              digits = "1";
                              drawing2DBounds = bounds;
                              parenthetical = None} nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDecimalAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawing2DBounds = bounds;
                                      parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | Percent ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Quotient); 
                              digits = "100";
                              drawing2DBounds = bounds;
                              parenthetical = None} nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDecimalAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawing2DBounds = bounds;
                                      parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | CalculatorMathOp.Root ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                                pendingFunction = Some (expr,Function.Root); 
                                digits = "0.5";
                                drawing2DBounds = bounds;
                                parenthetical = None} nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDecimalAccumulatorState 
                                    { digits = ""; 
                                        pendingFunction = stateData.pendingFunction; 
                                        expression=ev.evaluatedExpression;
                                        drawing2DBounds = bounds;
                                        parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | ChangeSign ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                                pendingFunction = Some (expr,Function.Times); 
                                digits = "-1";
                                drawing2DBounds = bounds;
                                parenthetical = None} nextOp 
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with                            
                            | EvaluatedState ev -> 
                                ExpressionDecimalAccumulatorState 
                                    { digits = ""; 
                                        pendingFunction = stateData.pendingFunction; 
                                        expression=ev.evaluatedExpression;
                                        drawing2DBounds = bounds;
                                        parenthetical = None}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _ -> newState
                    finalState
                | MemoryAdd 
                | MemorySubtract -> ExpressionDecimalAccumulatorState stateData         
            | CalculatorInput.Zero -> 
                stateData
                |> accumulateZero services 
                |> ExpressionDecimalAccumulatorState                     
            | DecimalSeparator -> 
                newExpressionStateData 
                |> accumulateSeparator services
                |> ExpressionDecimalAccumulatorState 
            | CalculatorInput.Equals -> getEvaluationState services stateData None
            | ClearEntry ->                 
                    match stateData.pendingFunction with
                    | None -> stateData |> ExpressionDecimalAccumulatorState
                    | Some po -> 
                        let _oldExpression, oldPendingFunction = po
                        let zero = Number (Integer 0I)
                        {stateData with pendingFunction = Some (zero,oldPendingFunction)}
                        |> ExpressionDecimalAccumulatorState
            | Clear ->  
                {evaluatedExpression = zero; 
                 pendingFunction = None;
                 drawing2DBounds = bounds}
                |> EvaluatedState
            | MemoryRecall -> ExpressionDecimalAccumulatorState stateData //not used
            | MemoryClear -> ExpressionDecimalAccumulatorState stateData //not used
            | MemoryStore -> ExpressionDecimalAccumulatorState stateData //not used
            | Back -> // I need to come back to this function to incorporate expression type
                 match stateData.digits.Length with
                 | x when x <= 1 -> newExpressionStateData |> ExpressionDecimalAccumulatorState
                 | x -> let revisedDigits = stateData.digits.Remove(x-1)
                        match revisedDigits.EndsWith(".") with //come back to this and refactor to use a decimal separator type
                        | false -> {newExpressionStateData with digits = revisedDigits.Remove(x-2)} |> ExpressionDigitAccumulatorState
                        | true -> {newExpressionStateData with digits = revisedDigits} |> ExpressionDecimalAccumulatorState
            | Digit d -> 
                stateData
                |> accumulateNonZeroDigit services d
                |> ExpressionDecimalAccumulatorState 
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> 
                match stateData.pendingFunction with
                | None -> 
                    { expression = expr;
                      pendingFunction = None; 
                      digits = "";
                      drawing2DBounds = bounds;
                      parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) |> Some 
                    } 
                    |> ExpressionDecimalAccumulatorState
                | Some pendingfunc ->                    
                    {expression = expr; 
                     pendingFunction = Some pendingfunc;
                     digits = "";
                     parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) |> Some;
                     drawing2DBounds = stateData.drawing2DBounds}
                    |> ExpressionDecimalAccumulatorState
            | Function f -> getEvaluationState services stateData (Some f)
        | Draw -> doDrawOperation services (DrawOp (stateData.expression, bounds))
        | OpenParentheses -> 
             {stateData with 
                 parenthetical = 
                     (services.getParentheticalFromCalculatorState (ExpressionDecimalAccumulatorState stateData)) 
                     |> Some} 
             |> ExpressionDecimalAccumulatorState 
            

    let handleDrawErrorState services stateData input =           
        let bounds = services.getDrawing2DBounds (DrawErrorState stateData)
        match input with
        | Stack _ -> DrawErrorState stateData
        | CalcInput op -> 
            match op with
            | MathOp _
            | CalculatorInput.Zero _
            | DecimalSeparator _
            | CalculatorInput.Equals
            | ClearEntry
            | MemoryRecall
            | MemoryClear
            | MemoryStore               
            | Digit _
            | Back -> DrawErrorState stateData
            | Clear ->   
                {evaluatedExpression = stateData.lastExpression;  
                pendingFunction = None;
                drawing2DBounds = bounds}
                |> EvaluatedState
        | ExpressionInput i -> DrawErrorState stateData
        | Draw -> DrawErrorState stateData
        | OpenParentheses -> DrawErrorState stateData
        
    let handleExpressionErrorState services stateData input =
        let bounds = services.getDrawing2DBounds (ExpressionErrorState stateData)
        match input with
        | Stack _ -> DrawErrorState stateData
        | CalcInput op -> 
            match op with
            | MathOp _
            | CalculatorInput.Zero _
            | DecimalSeparator _
            | CalculatorInput.Equals
            | ClearEntry
            | MemoryRecall
            | MemoryClear
            | MemoryStore               
            | Digit _
            | Back -> DrawErrorState stateData
            | Clear ->   
                {evaluatedExpression = stateData.lastExpression;  
                pendingFunction = None;
                drawing2DBounds = bounds}
                |> EvaluatedState
        | ExpressionInput i -> DrawErrorState stateData
        | Draw -> DrawErrorState stateData
        | OpenParentheses -> DrawErrorState stateData
    
    let createEvaluate (services:GraphServices) : Evaluate = 
         // create some local functions with partially applied services
         let handleDrawState = handleDrawState services
         let handleEvaluatedState = handleEvaluatedState services
         let handleExpressionDigitAccumulatorState = handleExpressionDigitAccumulatorState services
         let handleExpressionDecimalAccumulatorState = handleExpressionDecimalAccumulatorState services
         let handleDrawErrorState = handleDrawErrorState services
         let handleExpressionErrorState = handleExpressionErrorState services

         fun (input,state) -> 
             match state with
             | DrawState stateData -> 
                 handleDrawState stateData input 
             | EvaluatedState stateData -> 
                 handleEvaluatedState stateData input 
             | ExpressionDigitAccumulatorState stateData -> 
                 handleExpressionDigitAccumulatorState stateData input
             | ExpressionDecimalAccumulatorState stateData -> 
                 handleExpressionDecimalAccumulatorState stateData input
             | DrawErrorState stateData -> 
                 handleDrawErrorState stateData input
             | ExpressionErrorState stateData -> 
                 handleExpressionErrorState stateData input

module GraphServices =
    open GraphingDomain
    //open Utilities
    
    let getNumberFromAccumulator :GetNumberFromAccumulator =
        fun accumulatorStateData ->
            //let x = accumulatorStateData.expression
            let digits = accumulatorStateData.digits
            match System.Double.TryParse digits with
            | true, d -> Real d
            | false, _ -> Real 0.0
    
    let getDisplayFromExpression :GetDisplayFromExpression = 
        fun e -> e.ToString()

    let getDisplayFromGraphState :GetDisplayFromGraphState = 
        fun g -> 
            match g with 
            | EvaluatedState e -> e.evaluatedExpression.ToString()
            | DrawState d -> d.trace.ToString()
            | ExpressionDigitAccumulatorState d -> 
                let func = 
                    match d.pendingFunction with
                    | None -> ""
                    | Some f -> f.ToString()
                match d.digits.Length = 0 with
                | false -> func + " " + d.digits 
                | true -> 
                    match d.parenthetical with
                    | Some expression -> func + " " + expression.ToString()
                    | None -> ""
            | ExpressionDecimalAccumulatorState d -> 
                let func = 
                    match d.pendingFunction with
                    | None -> ""
                    | Some f -> f.ToString()
                match d.digits.Length = 0 with
                | false -> func + " " + d.digits 
                | true -> 
                    match d.parenthetical with
                    | Some expression -> func + " " + expression.ToString()
                    | None -> ""
            | DrawErrorState _de -> "derror"
            | ExpressionErrorState _ee -> "eerror"

    let getDisplayFromPendingFunction (pendingFunction : PendingFunction option) =
        match pendingFunction with
        | Some x -> x.ToString()
        | None -> ""
    
    let getDrawing2DBounds :GetDrawing2DBounds = 
        fun g -> 
            match g with 
            | EvaluatedState e -> e.drawing2DBounds
            | DrawState d -> d.drawing2DBounds
            | ExpressionDecimalAccumulatorState e -> e.drawing2DBounds
            | ExpressionDigitAccumulatorState e -> e.drawing2DBounds
            | _ -> 
                { upperX = X (Math.Pure.Quantity.Real 15.); 
                  lowerX = X (Math.Pure.Quantity.Real -15.); 
                  upperY = Y (Math.Pure.Quantity.Real 15.); 
                  lowerY = Y (Math.Pure.Quantity.Real -15.) }
   
    let doDrawOperation resolution (drawOp:DrawOp):DrawOperationResult = 
        let expression, drawBounds = drawOp
        let getValueFrom numberType =            
            match numberType with
            | Real r -> r
            | Integer i -> float i
            | _ -> System.Double.NaN
        let valueOfX coordinate = match coordinate with X x -> getValueFrom x
        let valueOfY coordinate = match coordinate with Y y -> getValueFrom y

        let xMin, xMax, yMin, yMax = valueOfX drawBounds.lowerX, valueOfX drawBounds.upperX, valueOfY drawBounds.lowerY, valueOfY drawBounds.upperY

        let xCoordinates = seq {for x in xMin .. resolution .. xMax -> Number(Real x)}
        
        let x = 
            match ExpressionFunction.getSymbolsFrom expression with
            | [] -> Expression.Symbol (Constant Null)
            | [x] -> x
            | x::t as vList -> x
        
        let makePoint xExpression yExpression = 
            let xValue =
                match xExpression with
                | Number n -> n
                | Expression.Symbol (Constant Pi) -> Real (System.Math.PI)
                | Expression.Symbol (Constant E ) -> Real (System.Math.E )
                | _ -> Undefined
            let yValue =
                match yExpression with                
                | Number n -> n
                | Expression.Symbol (Constant Pi) -> Real (System.Math.PI)
                | Expression.Symbol (Constant E ) -> Real (System.Math.E )
                | _ -> Undefined
            Point(X xValue,Y yValue)
        
        let evaluate expression xValue = 
            ExpressionStructure.substitute (x, xValue) expression 
            |> ExpressionFunction.evaluateRealPowersOfExpression 
            |> ExpressionType.simplifyExpression
            |> makePoint xValue
            
        let pointSequence = seq { for x in xCoordinates do yield evaluate expression x } 
        
        let checkForUndefinedPoints = 
            pointSequence |>
            Seq.exists (fun x -> 
                match x with 
                | Point(X x,Y y) when x = Undefined || y = Undefined-> true 
                | _ -> false)  

        match checkForUndefinedPoints with
        | true -> DrawError FailedToCreateTrace
        | false -> Trace { startPoint = Seq.head pointSequence; 
                           traceSegments = 
                               Seq.tail pointSequence 
                               |> Seq.toList 
                               |> List.map (fun x -> LineSegment x) }

    let doExpressionOperation opData :ExpressionOperationResult = 
        
        let func, (expression_1 :Expression), expression_2 = opData
        
        let checkResult expression = 
            match expression with
            | Expression.Symbol (Symbol.Error e) -> ExpressionError e
            | _ -> expression |> ExpressionSuccess
        
        match func with
        | Plus ->         expression_1 + expression_2 |> checkResult
        | Minus ->        expression_1 - expression_2 |> checkResult
        | Times ->        expression_1 * expression_2 |> checkResult
        | DividedBy 
        | Quotient ->     expression_1 / expression_2 |> checkResult
        | Inverse ->      expression_2 / expression_1 |> checkResult
        | ToThePowerOf -> expression_1** expression_2 |> checkResult
        | _ ->            expression_1 |> checkResult

    let accumulateSymbol (expressionStateData :ExpressionStateData) input = 
        let expression = 
            match expressionStateData.parenthetical with
            | Some (Parenthetical(p,_,_)) -> p
            | None -> expressionStateData.expression
        let pendingOp, digits =              
            expressionStateData.pendingFunction,
            expressionStateData.digits            
        let symbol = Expression.Symbol input
        let getResult =
            fun opResult ->
                match opResult with
                | ExpressionSuccess e -> e
                | ExpressionError e -> Expression.Symbol (Symbol.Error e)

        match pendingOp, digits = "" with 
        | None, true -> 
            match expression with
            | Number z when z = Integer 0I -> symbol //Zero State
            | _ -> expression //symbol// <-- Toggle between expression and symbol for desired behavior
        
        | None, false -> 
            let number = 
                getNumberFromAccumulator expressionStateData
                |> Number            
            doExpressionOperation (Times,number,symbol)
            |> getResult
        
        | Some (expression,op), true -> 
            doExpressionOperation (op,expression,symbol)
            |> getResult        
        
        | Some (expression,op), false -> 
            let number = 
                getNumberFromAccumulator expressionStateData
                |> Number            
            let numberXsymbol = 
                doExpressionOperation (Times,number,symbol)
                |> getResult
            doExpressionOperation (op,expression,numberXsymbol)
            |> getResult

    let accumulateNonZeroDigit maxLen :AccumulateNonZeroDigit = 
        fun (digit, accumulator) ->
    
        // determine what character should be appended to the display
        let appendCh= 
            match digit with
            | ConventionalDomain.One -> "1"
            | Two -> "2"
            | Three-> "3"
            | Four -> "4"
            | Five -> "5"
            | Six-> "6"
            | Seven-> "7"
            | Eight-> "8"
            | Nine-> "9"
        CalculatorServices.appendToAccumulator maxLen accumulator appendCh
    
    let accumulateZero maxLen :AccumulateZero = 
        fun accumulator -> CalculatorServices.appendToAccumulator maxLen accumulator "0"
    
    let accumulateSeparator maxLen :AccumulateSeparator = 
        fun accumulator ->
            let appendCh = 
                if accumulator = "" then "0." else "."
            CalculatorServices.appendToAccumulator maxLen accumulator appendCh
    
    let getParentheticalFromExpression :GetParentheticalFromExpression = 
        fun expression -> Parenthetical(expression, None, None)
    
    let setExpressionToParenthetical = 
        fun (expression, parenthetical) -> 
            match parenthetical with 
            | Some (Parenthetical(_,po,p)) -> Parenthetical(expression, po, p)
            | None -> getParentheticalFromExpression expression

    let getExpressionFromParenthetical :GetExpressionFromParenthetical = 
        fun parenthetical ->
            match parenthetical with 
            | None -> Expression.Zero
            | Some p -> match p with Parenthetical (x, _, _) -> x   
            
    let getParentheticalFromCalculatorState = 
        fun state ->
            match state with 
            | EvaluatedState ev -> (Expression.Zero, ev.pendingFunction, None) |> Parenthetical
            | ExpressionDigitAccumulatorState es
            | ExpressionDecimalAccumulatorState es -> 
                let lastParenthetical = (Expression.Zero, es.pendingFunction, es.parenthetical) |> Parenthetical |> Some
                (Expression.Zero, es.pendingFunction, lastParenthetical ) |> Parenthetical
            | _ -> (Expression.Zero, None, None) |> Parenthetical

    let createGraphServices () = {        
        doDrawOperation =  doDrawOperation (0.1)
        doExpressionOperation =  doExpressionOperation
        accumulateSymbol =  accumulateSymbol
        accumulateZero = accumulateZero (15)
        accumulateNonZeroDigit = accumulateNonZeroDigit (10)
        accumulateSeparator = accumulateSeparator (15)
        getNumberFromAccumulator = getNumberFromAccumulator
        getDisplayFromExpression = getDisplayFromExpression
        getDisplayFromGraphState = getDisplayFromGraphState
        getDrawing2DBounds = getDrawing2DBounds 
        getDisplayFromPendingFunction = getDisplayFromPendingFunction
        getExpressionFromParenthetical = getExpressionFromParenthetical
        getParentheticalFromExpression = getParentheticalFromExpression
        getParentheticalFromCalculatorState = getParentheticalFromCalculatorState
        setExpressionToParenthetical = setExpressionToParenthetical
        }
