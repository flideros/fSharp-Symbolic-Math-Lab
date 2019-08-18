﻿namespace GraphingCalculator

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

    // data associated with each state    
    
    type ExpressionStateData = 
        { expression : Expression;
          pendingFunction:PendingFunction option;
          digits:ConventionalDomain.DigitAccumulator; 
          drawing2DBounds:Drawing2DBounds}
    type EvaluatedStateData =
        { evaluatedExpression:Expression; 
          pendingFunction:PendingFunction option; 
          drawing2DBounds:Drawing2DBounds}
    type DrawStateData =  
        { traceExpression:Expression; 
          trace:Trace; 
          pendingFunction:PendingFunction option;
          drawing2DBounds:Drawing2DBounds}    
    type ErrorStateData = 
        { lastExpression:Expression; 
          error:DrawError;
          drawing2DBounds:Drawing2DBounds}

    type CalculatorInput =
        | ExpressionInput of ExpressionInput
        | CalcInput of ConventionalDomain.CalculatorInput
        | Stack of RpnDomain.CalculatorInput
        | Draw

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
            let newPendingFunc = 
                nextFunc |> Option.map (fun func -> displayExpression, func )
            {evaluatedExpression = displayExpression; 
             pendingFunction=newPendingFunc;
             drawing2DBounds=expressionStateData.drawing2DBounds}
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
                | ExpressionSuccess resultExpression ->
                    // If there was a pending op, create a new ExpressionState using the result
                    getNewState resultExpression 
                | ExpressionError error -> 
                    {lastExpression = previousExpression; 
                     error = error |> Error; 
                     drawing2DBounds=expressionStateData.drawing2DBounds} 
                    |> ExpressionErrorState
            return newState
            } |> ifNone computeStateWithNoPendingOp 
    
    let doDrawOperation services drawOp =        
        let result = services.doDrawOperation drawOp 
        let expression, bounds = drawOp
        match result with
        | Trace t -> DrawState {traceExpression = expression; trace = t; pendingFunction = None; drawing2DBounds=bounds}
        | DrawError x ->  
            {lastExpression = expression; 
             error = x; 
             drawing2DBounds=bounds} 
            |> DrawErrorState 

    let doExpressionOperation services stateData = services.doExpressionOperation stateData
        
    let replacePendingFunction (evaluatedStateData:EvaluatedStateData) nextFun = 
        let newPending = maybe {
            let! expression, _existingFunc = evaluatedStateData.pendingFunction
            let! next = nextFun
            return expression,next }
        {evaluatedStateData with pendingFunction=newPending}
        |> EvaluatedState

    let handleDrawState stateData input = //Only the'Back' function implimented here.
        let expr = stateData.traceExpression        
        let newEvaluatedStateData =  
                {evaluatedExpression=expr;  
                 pendingFunction=None;
                 drawing2DBounds=stateData.drawing2DBounds 
                }
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
        
    let handleEvaluatedState services stateData input =
        let zero = Number (Integer 0I)
        let expr = stateData.evaluatedExpression        
        let newExpressionStateData =  
                {expression=expr;  
                 pendingFunction=stateData.pendingFunction;
                 digits="";
                 drawing2DBounds=stateData.drawing2DBounds 
                }
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
                              digits = "1" ;
                              drawing2DBounds=stateData.drawing2DBounds 
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
                                      drawing2DBounds=stateData.drawing2DBounds 
                                    }
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
                              drawing2DBounds=stateData.drawing2DBounds 
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
                                  drawing2DBounds=stateData.drawing2DBounds 
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
                              drawing2DBounds=stateData.drawing2DBounds 
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
                                      drawing2DBounds=stateData.drawing2DBounds}
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
                              drawing2DBounds=stateData.drawing2DBounds 
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
                                      drawing2DBounds=stateData.drawing2DBounds 
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
                | None -> stateData |> EvaluatedState
                | Some _ ->                    
                    newExpressionStateData
                    |> accumulateNonZeroDigit services d
                    |> ExpressionDigitAccumulatorState 
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> 
                match stateData.pendingFunction with
                | None -> stateData |> EvaluatedState
                | Some _ ->
                    let newExpression = accumulateSymbol services s newExpressionStateData
                    {evaluatedExpression = newExpression; 
                     pendingFunction = newExpressionStateData.pendingFunction;
                     drawing2DBounds=stateData.drawing2DBounds}
                    |> EvaluatedState
            | Function f -> replacePendingFunction stateData (Some f)
        | Draw -> 
            (stateData.evaluatedExpression,stateData.drawing2DBounds)
            |> doDrawOperation services 
        
    let handleExpressionDigitAccumulatorState services stateData input=
           let zero = Number (Integer 0I)
           let expr = stateData.expression        
           let newExpressionStateData =  
                   {expression=expr;  
                    pendingFunction=stateData.pendingFunction;
                    digits="";
                    drawing2DBounds=stateData.drawing2DBounds}
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
                                 drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                         drawing2DBounds=stateData.drawing2DBounds}
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
                                 drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                         drawing2DBounds=stateData.drawing2DBounds}
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
                                   drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                         drawing2DBounds=stateData.drawing2DBounds}
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
                                   drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                           drawing2DBounds=stateData.drawing2DBounds}
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
                   newExpressionStateData 
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
                   pendingFunction=None;
                   drawing2DBounds=stateData.drawing2DBounds} 
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
                   let newExpression = accumulateSymbol services s newExpressionStateData
                   {evaluatedExpression = newExpression; 
                    pendingFunction = newExpressionStateData.pendingFunction;
                    drawing2DBounds=stateData.drawing2DBounds}
                   |> EvaluatedState
               | Function f -> getEvaluationState services stateData (Some f)
           | Draw -> doDrawOperation services (DrawOp (stateData.expression,stateData.drawing2DBounds))
           
    let handleExpressionDecimalAccumulatorState services stateData input =
        let zero = Number (Integer 0I)
        let expr = stateData.expression        
        let newExpressionStateData =  
                {expression=expr;  
                 pendingFunction=stateData.pendingFunction;
                 digits="";
                 drawing2DBounds=stateData.drawing2DBounds}
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
                              drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                      drawing2DBounds=stateData.drawing2DBounds}
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
                              drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                      drawing2DBounds=stateData.drawing2DBounds}
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
                                drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                        drawing2DBounds=stateData.drawing2DBounds}
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
                                drawing2DBounds=stateData.drawing2DBounds} nextOp 
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
                                        drawing2DBounds=stateData.drawing2DBounds}
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
                 pendingFunction=None;
                 drawing2DBounds=stateData.drawing2DBounds}
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
                let newExpression = accumulateSymbol services s newExpressionStateData
                {evaluatedExpression = newExpression; 
                 pendingFunction = newExpressionStateData.pendingFunction;
                 drawing2DBounds=stateData.drawing2DBounds}
                |> EvaluatedState
            | Function f -> getEvaluationState services stateData (Some f)
        | Draw -> doDrawOperation services (DrawOp (stateData.expression,stateData.drawing2DBounds))
        
    let handleDrawErrorState stateData input =           
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
                    drawing2DBounds=stateData.drawing2DBounds}
                   |> EvaluatedState
           | ExpressionInput i -> DrawErrorState stateData
           | Draw -> DrawErrorState stateData
          
    let handleExpressionErrorState stateData input =
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
                    drawing2DBounds=stateData.drawing2DBounds}
                   |> EvaluatedState
           | ExpressionInput i -> DrawErrorState stateData
           | Draw -> DrawErrorState stateData
    
    let createEvaluate (services:GraphServices) : Evaluate = 
         // create some local functions with partially applied services
         let handleDrawState = handleDrawState 
         let handleEvaluatedState = handleEvaluatedState services
         let handleExpressionDigitAccumulatorState = handleExpressionDigitAccumulatorState services
         let handleExpressionDecimalAccumulatorState = handleExpressionDecimalAccumulatorState services
         let handleDrawErrorState = handleDrawErrorState 
         let handleExpressionErrorState = handleExpressionErrorState

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
    open Utilities
    
    let getNumberFromAccumulator :GetNumberFromAccumulator =
        fun accumulatorStateData ->
            let digits = accumulatorStateData.digits
            match System.Double.TryParse digits with
            | true, d -> Real d
            | false, _ -> Real 0.0
    
    let getDisplayFromExpression :GetDisplayFromExpression = 
        fun e -> e.ToString()

    let getDisplayFromGraphState :GetDisplayFromGraphState = 
        fun g -> g.ToString()

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
                | Number (Real r) when r <= yMax && r >= yMin -> Real r
                | Number (Real r) when r > yMax -> Real yMax
                | Number (Real r) when r < yMin -> Real yMin
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
                | Point(X x,Y y) when x = Undefined || x = Undefined-> true 
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
        let expression, pendingOp, digits = 
            expressionStateData.expression, 
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
            | _ -> expression //symbol <-- Toggle between expression and symbol for desired behavior
        
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
        }
