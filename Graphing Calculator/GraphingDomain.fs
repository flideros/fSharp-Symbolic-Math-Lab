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
    type T = T of NumberType
    type U = U of NumberType
    type V = V of NumberType
    type Tstep = Tstep of NumberType
    type Ustep = Ustep of NumberType
    type Vstep = Vstep of NumberType
    type Size = {height:NumberType; width:NumberType}
    type SweepDirection =
            /// positive-angle direction
        | Clockwise
            /// negative-angle direction
        | CounterClockwise
    type DrawingOptions = 
        { upperX : X; 
          lowerX : X; 
          upperY : Y; 
          lowerY : Y;
          upperT : T; 
          lowerT : T;
          Tstep  : Tstep;
          upperU : U; 
          lowerU : U; 
          upperV : V; 
          lowerV : V;
          uStep  : Ustep;
          vStep  : Vstep}        
    
    // Geometry    
    type Point = 
        | Point of X * Y 
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
    
    //type MeshGroup = 
    type Mesh = 
        | Surface of System.Windows.Media.Media3D.GeometryModel3D
        | Curve of System.Windows.Media.Media3D.Model3DGroup
    
    // types to describe results
    type DrawError = 
        | Error of Error
        | FailedToCreateTrace
        | LazyCoder  
        | InputError
        | UnexpectedModelType
        | ParameterError
       
    type DrawOperationResult =  
        | Traces of Trace list        
        | Mesh of Mesh
        | DrawError of DrawError

    type ExpressionOperationResult =  
        | ExpressionSuccess of Expression 
        | ExpressionError of Error

    type ExpressionInput =
        | Symbol of Symbol
        | Function of Function        

    type ActiveExpression = 
        | X_
        | Y_
        | Z_

    type PendingFunction = (Expression * Function)

    type Parenthetical = Parenthetical of (Expression * PendingFunction option * Parenthetical option)

    // data associated with each state        
    type ExpressionStateData = 
        { expression : Expression;
          pendingFunction : PendingFunction option;
          parenthetical : Parenthetical option;
          digits : ConventionalDomain.DigitAccumulator;
          drawingOptions : DrawingOptions }
    type EvaluatedStateData =
        { evaluatedExpression : Expression
          pendingFunction : PendingFunction option;
          drawingOptions : DrawingOptions}
    type ParentheticalStateData = 
        { evaluatedExpression : Expression
          parenthetical : Parenthetical;
          pendingFunction : PendingFunction Option;
          drawingOptions : DrawingOptions }
    type DrawStateData =  
        { traceExpression : Expression; 
          trace:Trace list; // this doesn't need to be an option, just return [] when in 3D Mode
          drawingOptions : DrawingOptions;
          mesh : Mesh Option}    
    type ErrorStateData = 
        { lastExpression : Expression; 
          error : DrawError }

    type Dim3StateData = {x:Expression;y:Expression;z:Expression;activeExpression:ActiveExpression}
    
    // Drawing
    type DrawOp = Expression * DrawingOptions
    type DrawOp3D = Dim3StateData * DrawingOptions
    
    type CalculatorInput =
        | ExpressionInput of ExpressionInput
        | CalcInput of ConventionalDomain.CalculatorInput
        | Stack of RpnDomain.CalculatorInput
        | Draw
        | Draw2DParametric
        | Draw3DParametric
        | OpenParentheses
        | CloseParentheses
        | GraphOptionSave of DrawingOptions
        | GraphOptionReset
        | ExpressionSquared
        | ExpressionToThePowerOf
        | SpiralExample
        | EllipseExample
        | HelixExample
        | SphereExample
        | ConeExample
        | TorusExample

    // States
    type CalculatorState =         
        | EvaluatedState of EvaluatedStateData
        | DrawState of DrawStateData
        | ParentheticalState of ParentheticalStateData
        | ExpressionDigitAccumulatorState of ExpressionStateData
        | ExpressionDecimalAccumulatorState of ExpressionStateData
        | DrawErrorState of ErrorStateData
        | ExpressionErrorState of ErrorStateData
        // 2D Parametric
        | EvaluatedState2DParametric of EvaluatedStateData * CalculatorState
        | DrawState2DParametric of DrawStateData * CalculatorState
        | ParentheticalState2DParametric of ParentheticalStateData * CalculatorState
        | ExpressionDigitAccumulatorState2DParametric of ExpressionStateData * CalculatorState
        | ExpressionDecimalAccumulatorState2DParametric of ExpressionStateData * CalculatorState
        // 3D Parametric
        | EvaluatedState3DParametric of EvaluatedStateData * Dim3StateData
        | DrawState3DParametric of DrawStateData * Dim3StateData
        | ParentheticalState3DParametric of ParentheticalStateData * Dim3StateData
        | ExpressionDigitAccumulatorState3DParametric of ExpressionStateData * Dim3StateData
        | ExpressionDecimalAccumulatorState3DParametric of ExpressionStateData * Dim3StateData
      
    type Evaluate = CalculatorInput * CalculatorState -> CalculatorState

    // Graph Services
    type AccumulateSymbol = ExpressionStateData -> Symbol -> Expression //StateData
    type DoDrawOperation = DrawOp-> DrawOperationResult
    type DoDraw2DParametricOperation = DrawOp * DrawOp -> DrawOperationResult
    type DoDraw3DParametricOperation = DrawOp3D -> DrawOperationResult
    type DoExpressionOperation = Function * Expression * Expression -> ExpressionOperationResult
    type GetDisplayFromExpression = Expression -> string
    type GetNumberFromAccumulator = ExpressionStateData -> NumberType
    type GetDisplayFromGraphState = CalculatorState -> string
    type GetErrorFromExpression = Expression -> Error
    type GetDrawingOptionsFromState = CalculatorState -> DrawingOptions
    type GetDim3DStateDataFromState = CalculatorState -> Dim3StateData
    type SetDrawingOptions = CalculatorState * DrawingOptions -> CalculatorState
    type GetDisplayFromPendingFunction = PendingFunction option -> string
    type GetExpressionFromParenthetical = Parenthetical option -> Expression
    type GetParentheticalFromExpression = Expression -> Parenthetical
    type GetParentheticalFromCalculatorState = CalculatorState -> Parenthetical
    type SetExpressionToParenthetical = Expression * (Parenthetical option) -> Parenthetical
    type SetActivate3DStateExpression = CalculatorState * ActiveExpression -> CalculatorState
    type CloseParenthetical = ParentheticalStateData -> CalculatorState
    type Toggle2DParametricState = CalculatorState -> CalculatorState

        
    type GraphServices = {        
        doDrawOperation :DoDrawOperation
        doDraw2DParametricOperation :DoDraw2DParametricOperation
        doDraw3DParametricOperation :DoDraw3DParametricOperation
        doExpressionOperation :DoExpressionOperation
        accumulateSymbol :AccumulateSymbol
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        getNumberFromAccumulator :GetNumberFromAccumulator
        getDisplayFromExpression :GetDisplayFromExpression
        getDisplayFromGraphState :GetDisplayFromGraphState
        getDrawingOptionsFromState :GetDrawingOptionsFromState
        getDim3DStateDataFromState :GetDim3DStateDataFromState
        setDrawingOptions :SetDrawingOptions
        getDisplayFromPendingFunction :GetDisplayFromPendingFunction
        getExpressionFromParenthetical :GetExpressionFromParenthetical
        getParentheticalFromCalculatorState :GetParentheticalFromCalculatorState
        setExpressionToParenthetical :SetExpressionToParenthetical
        setActivate3DStateExpression :SetActivate3DStateExpression
        closeParenthetical :CloseParenthetical
        toggle2DParametricState :Toggle2DParametricState
        }
 
module GraphingImplementation =    
    open GraphingDomain
    open Utilities

    let defaultOptions = 
        { upperX = X (Math.Pure.Quantity.Real 150.); 
          lowerX = X (Math.Pure.Quantity.Real -150.); 
          upperY = Y (Math.Pure.Quantity.Real 150.); 
          lowerY = Y (Math.Pure.Quantity.Real -150.);
          upperT = T (Math.Pure.Quantity.Real 10.); 
          lowerT = T (Math.Pure.Quantity.Real -10.);
          Tstep  = Tstep (Math.Pure.Quantity.Real 0.1) 
          upperU = U (Math.Pure.Quantity.Real (2.0 * System.Math.PI));
          lowerU = U (Math.Pure.Quantity.Real 0.);
          upperV = V (Math.Pure.Quantity.Real (2.0 * System.Math.PI));
          lowerV = V (Math.Pure.Quantity.Real 0.);
          uStep  = Ustep (Math.Pure.Quantity.Real (0.1*System.Math.PI)) ;
          vStep  = Vstep (Math.Pure.Quantity.Real (0.1*System.Math.PI)) }

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
        // check expressionStateData for parenthetical
        let returnEvaluatedState =
            match expressionStateData.parenthetical with
            | None -> true
            | Some (Parenthetical(_a,None,None)) -> true
            | _ -> false         
        
        // helper to create a new EvaluatedState from a given displayExpression 
        // and the nextOp parameter
        let getNewState displayExpression =
            let options = services.getDrawingOptionsFromState (ExpressionDigitAccumulatorState expressionStateData)
            let newPendingFunc = 
                nextFunc 
                |> Option.map ( fun func -> displayExpression, func )
            match returnEvaluatedState with
            | true -> 
                {evaluatedExpression = displayExpression; 
                 pendingFunction = newPendingFunc;
                 drawingOptions = options }
                |> EvaluatedState
            | false ->
                let newParenthetical = 
                    match expressionStateData.parenthetical.Value with
                    | Parenthetical(_,po,p) -> Parenthetical(Expression.Zero,po,p)                
                {evaluatedExpression = displayExpression; 
                 pendingFunction = newPendingFunc;
                 drawingOptions = options; 
                 parenthetical = newParenthetical}
                |> ParentheticalState

        let currentExpression = 
            let number = services.getNumberFromAccumulator expressionStateData |> Number
            match expressionStateData.parenthetical with
            | None -> number
            | Some (Parenthetical(x,_,_)) when x <> Expression.Zero -> x
            | _ -> number

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
        let expression, options = drawOp
        match result with        
        | Traces t -> DrawState {traceExpression = expression; trace = t; drawingOptions = options; mesh = None}        
        | Mesh _m -> 
            {lastExpression = expression; 
             error = UnexpectedModelType} 
            |> DrawErrorState
        | DrawError x ->  
            {lastExpression = expression; 
             error = x} 
            |> DrawErrorState 

    let doDraw2DParametricOperation services stateData =        
        match stateData with 
        | EvaluatedState2DParametric (xT, EvaluatedState yT) ->
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},EvaluatedState yT)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | EvaluatedState2DParametric (xT, ParentheticalState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.evaluatedExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | EvaluatedState2DParametric (xT, ExpressionDigitAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | EvaluatedState2DParametric (xT, ExpressionDecimalAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState        
        | EvaluatedState2DParametric (xT, DrawState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.traceExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.traceExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        
        | ParentheticalState2DParametric (xT, EvaluatedState yT) ->
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},EvaluatedState yT)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | ParentheticalState2DParametric (xT, ParentheticalState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.evaluatedExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | ParentheticalState2DParametric (xT, ExpressionDigitAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | ParentheticalState2DParametric (xT, ExpressionDecimalAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState        
        | ParentheticalState2DParametric (xT, DrawState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.traceExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.evaluatedExpression,xT.drawingOptions)
            let drawOpYt = (yT.traceExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.evaluatedExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState

        | ExpressionDigitAccumulatorState2DParametric (xT, EvaluatedState yT) ->
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> 
                DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},EvaluatedState yT)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDigitAccumulatorState2DParametric (xT, ParentheticalState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.evaluatedExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDigitAccumulatorState2DParametric (xT, ExpressionDigitAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> 
                DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDigitAccumulatorState2DParametric (xT, ExpressionDecimalAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState        
        | ExpressionDigitAccumulatorState2DParametric (xT, DrawState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.traceExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.traceExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState

        | ExpressionDecimalAccumulatorState2DParametric (xT, EvaluatedState yT) ->
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},EvaluatedState yT)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDecimalAccumulatorState2DParametric (xT, ParentheticalState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.evaluatedExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDecimalAccumulatorState2DParametric (xT, ExpressionDigitAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDecimalAccumulatorState2DParametric (xT, ExpressionDecimalAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState        
        | ExpressionDecimalAccumulatorState2DParametric (xT, DrawState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.traceExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.expression,xT.drawingOptions)
            let drawOpYt = (yT.traceExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.expression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.expression; 
                 error = x} 
                |> DrawErrorState

        | DrawState2DParametric (xT, EvaluatedState yT) ->
            let drawOpXt = (xT.traceExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t ->  DrawState2DParametric ({traceExpression = xT.traceExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},EvaluatedState yT)
            | Mesh _m -> 
                {lastExpression = xT.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.traceExpression; 
                 error = x} 
                |> DrawErrorState
        | DrawState2DParametric (xT, ParentheticalState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.evaluatedExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.traceExpression,xT.drawingOptions)
            let drawOpYt = (yT.evaluatedExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.traceExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.traceExpression; 
                 error = x} 
                |> DrawErrorState
        | DrawState2DParametric (xT, ExpressionDigitAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.traceExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.traceExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.traceExpression; 
                 error = x} 
                |> DrawErrorState
        | DrawState2DParametric (xT, ExpressionDecimalAccumulatorState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.expression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.traceExpression,xT.drawingOptions)
            let drawOpYt = (yT.expression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.traceExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.traceExpression; 
                 error = x} 
                |> DrawErrorState        
        | DrawState2DParametric (xT, DrawState yT) ->
            let newEvaluatedStateYt = {evaluatedExpression=yT.traceExpression; pendingFunction = None; drawingOptions=yT.drawingOptions} |> EvaluatedState
            let drawOpXt = (xT.traceExpression,xT.drawingOptions)
            let drawOpYt = (yT.traceExpression,yT.drawingOptions)
            let result = services.doDraw2DParametricOperation (drawOpXt, drawOpYt)            
            match result with
            | Traces t -> DrawState2DParametric ({traceExpression = xT.traceExpression; trace = t; drawingOptions = xT.drawingOptions; mesh = None},newEvaluatedStateYt)
            | Mesh _m -> 
                {lastExpression = xT.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = xT.traceExpression; 
                 error = x} 
                |> DrawErrorState
        
        | _ -> {lastExpression = Expression.Zero; error = FailedToCreateTrace} |> DrawErrorState

    let doDraw3DParametricOperation services stateData = 
        match stateData with
        | EvaluatedState3DParametric (sd, d3d) ->
            let drawOp = (d3d,services.getDrawingOptionsFromState stateData)
            let result = services.doDraw3DParametricOperation drawOp 
            match result with
            | Mesh m -> DrawState3DParametric ({traceExpression = sd.evaluatedExpression; trace = []; drawingOptions = sd.drawingOptions; mesh = Some m},d3d)
            | Traces _t -> 
                {lastExpression = sd.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = sd.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState 
        | ParentheticalState3DParametric (sd, d3d) ->
            let drawOp = (d3d,services.getDrawingOptionsFromState ( ParentheticalState sd))
            let result = services.doDraw3DParametricOperation drawOp 
            match result with
            | Mesh m -> DrawState3DParametric ({traceExpression = sd.evaluatedExpression; trace = []; drawingOptions = sd.drawingOptions; mesh = Some m},d3d)
            | Traces _t -> 
                {lastExpression = sd.evaluatedExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = sd.evaluatedExpression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDigitAccumulatorState3DParametric (sd, d3d) -> 
            let drawOp = (d3d,services.getDrawingOptionsFromState ( ExpressionDigitAccumulatorState sd))
            let result = services.doDraw3DParametricOperation drawOp 
            match result with
            | Mesh m -> DrawState3DParametric ({traceExpression = sd.expression; trace = []; drawingOptions = sd.drawingOptions; mesh = Some m},d3d)
            | Traces _t -> 
                {lastExpression = sd.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = sd.expression; 
                 error = x} 
                |> DrawErrorState
        | ExpressionDecimalAccumulatorState3DParametric (sd, d3d) -> 
            let drawOp = (d3d,services.getDrawingOptionsFromState ( ExpressionDecimalAccumulatorState sd))
            let result = services.doDraw3DParametricOperation drawOp 
            match result with
            | Mesh m -> DrawState3DParametric ({traceExpression = sd.expression; trace = []; drawingOptions = sd.drawingOptions; mesh = Some m},d3d)
            | Traces _t -> 
                {lastExpression = sd.expression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = sd.expression; 
                 error = x} 
                |> DrawErrorState
        | DrawState3DParametric (sd, d3d) -> 
            let drawOp = (d3d,services.getDrawingOptionsFromState stateData)
            let result = services.doDraw3DParametricOperation drawOp 
            match result with
            | Mesh m -> DrawState3DParametric ({traceExpression = sd.traceExpression; trace = []; drawingOptions = sd.drawingOptions; mesh = Some m},d3d)
            | Traces _t -> 
                {lastExpression = sd.traceExpression; 
                 error = UnexpectedModelType} 
                |> DrawErrorState
            | DrawError x ->  
                {lastExpression = sd.traceExpression; 
                 error = x} 
                |> DrawErrorState 
        | _ -> {lastExpression = Expression.Zero; error = FailedToCreateTrace} |> DrawErrorState

    let doExpressionOperation services stateData = services.doExpressionOperation stateData
        
    let replacePendingFunction (evaluatedStateData:EvaluatedStateData) nextFun =         
        let newPending = maybe {
            let! expression, _existingFunc = evaluatedStateData.pendingFunction
            let! next = nextFun
            return expression,next }
        match newPending, nextFun with 
        | Some _, _ -> {evaluatedStateData with pendingFunction = newPending}
        | None, Some func -> {evaluatedStateData with pendingFunction = Some (evaluatedStateData.evaluatedExpression,func)}
        | _ -> evaluatedStateData
        |> EvaluatedState 

    let replacePendingFunctionParenthetical (evaluatedStateData:ParentheticalStateData) nextFun =         
        let newPending = maybe {
            let! expression, _existingFunc = evaluatedStateData.pendingFunction
            let! next = nextFun
            return expression,next }
        match newPending, nextFun with 
        | Some _, _ -> {evaluatedStateData with pendingFunction = newPending}
        | None, Some func -> {evaluatedStateData with pendingFunction = Some (evaluatedStateData.evaluatedExpression,func)}
        | _ -> evaluatedStateData
        |> ParentheticalState
    
    let spiral_ExampleState =
        let xT = NaryOp (Product,[UnaryOp (Sin,Expression.Symbol (Variable "t")); Expression.Symbol (Variable "t")])
        let yT = NaryOp (Product,[UnaryOp (Cos,Expression.Symbol (Variable "t")); Expression.Symbol (Variable "t")])
        let options = {defaultOptions with 
                        upperT = T (Real (10.*System.Math.PI))
                        lowerT = T (Real 0.0)}
        ({ evaluatedExpression = xT
           pendingFunction = None;
           drawingOptions = options}, 
           EvaluatedState 
            { evaluatedExpression = yT
              pendingFunction = None;
              drawingOptions = options }) |> EvaluatedState2DParametric

    let ellipse_ExampleState =
        let xT = NaryOp (Product,[UnaryOp (Sin,Expression.Symbol (Variable "t")); Expression.Number (Real 20.0)])
        let yT = NaryOp (Product,[UnaryOp (Cos,Expression.Symbol (Variable "t")); Expression.Number (Real 40.0)])
        let options = {defaultOptions with 
                        upperT = T (Real 31.415926535897932384626433832795)
                        lowerT = T (Real 0.0)}
        ({ evaluatedExpression = xT
           pendingFunction = None;
           drawingOptions = options}, 
           EvaluatedState 
            { evaluatedExpression = yT
              pendingFunction = None;
              drawingOptions = options }) |> EvaluatedState2DParametric       
    
    let helix_ExampleState =
        let xT = UnaryOp (Sin,Expression.Symbol (Variable "t"))
        let yT = UnaryOp (Cos,Expression.Symbol (Variable "t"))
        let zT = NaryOp (Product,[Number(Real -1.0);Expression.Symbol (Variable "t")])
        let options = {defaultOptions with 
                        upperT = T (Real 25.0)
                        lowerT = T (Real 0.0)
                        Tstep = Tstep (Real 0.1)}
        
        ({ evaluatedExpression = xT
           pendingFunction = None;
           drawingOptions = options}, 
           {x = xT; y = yT; z = zT;activeExpression = X_}) |> EvaluatedState3DParametric

    let sphere_ExampleState =
        let xUV = (UnaryOp (Cos,Expression.Symbol (Variable "u"))) * (UnaryOp (Sin,Expression.Symbol (Variable "v")))
        let yUV = (UnaryOp (Cos,Expression.Symbol (Variable "v"))) * Number (Real -1.)
        let zUV = (UnaryOp (Sin,Expression.Symbol (Variable "u") * Number (Real -1.))) * (UnaryOp (Sin,Expression.Symbol (Variable "v")))
        let options = {defaultOptions with 
                        upperU = U (Real (2.0 * System.Math.PI))
                        lowerU = U (Real 0.0)
                        uStep = Ustep (Real (0.05 * System.Math.PI))
                        upperV = V (Real (2.0 * System.Math.PI))
                        lowerV = V (Real 0.0)
                        vStep = Vstep (Real (0.05 * System.Math.PI))}
        
        ({ evaluatedExpression = xUV
           pendingFunction = None;
           drawingOptions = options}, 
           {x = xUV; y = yUV; z = zUV;activeExpression = X_}) |> EvaluatedState3DParametric

    let torus_ExampleState =
        let xUV = (UnaryOp (Sin,Expression.Symbol (Variable "v")))
        let yUV = (Number (Real 2.) + UnaryOp (Cos,Expression.Symbol (Variable "v"))) * (UnaryOp (Sin,Expression.Symbol (Variable "u")))
        let zUV = (Number (Real 2.) + UnaryOp (Cos,Expression.Symbol (Variable "v"))) * (UnaryOp (Cos,Expression.Symbol (Variable "u")))
        let options = {defaultOptions with 
                        upperU = U (Real (2.0 * System.Math.PI))
                        lowerU = U (Real 0.0)
                        uStep = Ustep (Real (0.05 * System.Math.PI))
                        upperV = V (Real (2.0 * System.Math.PI))
                        lowerV = V (Real 0.0)
                        vStep = Vstep (Real (0.05 * System.Math.PI))}
        
        ({ evaluatedExpression = xUV
           pendingFunction = None;
           drawingOptions = options}, 
           {x = xUV; y = yUV; z = zUV;activeExpression = X_}) |> EvaluatedState3DParametric

    let cone_ExampleState =
        let xUV = Number (Real 0.6) * (Number (Real 1.5) - Expression.Symbol (Variable "v")) * (UnaryOp (Cos,Expression.Symbol (Variable "u"))) 
        let yUV = Expression.Symbol (Variable "v")
        let zUV = Number (Real 0.6) * (Number (Real 1.5) - Expression.Symbol (Variable "v")) * (UnaryOp (Sin,Expression.Symbol (Variable "u")* (Number (Real -1.))))
        let options = {defaultOptions with 
                        upperU = U (Real (System.Math.PI))
                        lowerU = U (Real (-System.Math.PI))
                        uStep = Ustep (Real (0.05 * System.Math.PI))
                        upperV = V (Real (System.Math.PI))
                        lowerV = V (Real (-System.Math.PI))
                        vStep = Vstep (Real (0.05 * System.Math.PI))}        
        ({ evaluatedExpression = xUV
           pendingFunction = None;
           drawingOptions = options}, 
           {x = xUV; y = yUV; z = zUV;activeExpression = X_}) |> EvaluatedState3DParametric

    let handleDrawState services stateData input = //Only the'Back' function implemented here.
        let options = services.getDrawingOptionsFromState (DrawState stateData)
        let expr = stateData.traceExpression        
        let newEvaluatedStateData =  
                {evaluatedExpression=expr;  
                 pendingFunction=None;
                 drawingOptions = options }
        match input with         
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
        | GraphOptionSave options -> services.setDrawingOptions (DrawState stateData,options)
        | GraphOptionReset -> services.setDrawingOptions (DrawState stateData,defaultOptions)
        | Stack _input -> DrawState stateData
        | ExpressionInput _input -> DrawState stateData
        | Draw 
        | Draw2DParametric
        | Draw3DParametric -> DrawState stateData
        | OpenParentheses 
        | CloseParentheses         
        | ExpressionSquared
        | ExpressionToThePowerOf -> DrawState stateData
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState

    let handleEvaluatedState services stateData input =
        let options = services.getDrawingOptionsFromState (EvaluatedState stateData)
        let zero = Number (Integer 0I)
        let expr = stateData.evaluatedExpression        
        let newExpressionStateData =  
                {expression = expr;  
                 pendingFunction = stateData.pendingFunction;
                 digits = "";
                 drawingOptions = options;
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
                              drawingOptions = options;
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
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}                            
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _ 
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | Percent ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Quotient);
                              digits = "100";
                              drawingOptions = options;
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
                                  expression = ev.evaluatedExpression;
                                  drawingOptions = options;
                                  parenthetical = None 
                                } |> ExpressionDigitAccumulatorState 
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | CalculatorMathOp.Root ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Root); 
                              digits = "0.5";
                              drawingOptions = options;
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
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _
                            | DrawState2DParametric _
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | ChangeSign ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Times); 
                              digits = "-1";
                              drawingOptions = options;
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
                                      drawingOptions = options;
                                      parenthetical = None 
                                    }
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _
                            | DrawState2DParametric _
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | MemoryAdd 
                | MemorySubtract -> EvaluatedState stateData         
            | CalculatorInput.Zero -> stateData |> EvaluatedState                     
            | DecimalSeparator -> 
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
                 drawingOptions=stateData.drawingOptions}
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
                      drawingOptions = options;
                      parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s),None)|> Some 
                    } 
                    |> ExpressionDigitAccumulatorState
                | Some pendingfunc ->                    
                    {expression = expr; 
                     pendingFunction = Some pendingfunc;
                     digits = "";
                     parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s),None)|> Some;
                     drawingOptions = stateData.drawingOptions}
                    |> ExpressionDigitAccumulatorState
            | Function f -> 
                match f with
                | Derivative ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Derivative); 
                              digits = "";
                              drawingOptions = options;
                              parenthetical =                                 
                                Some (Parenthetical(Expression.Symbol (Variable "x"),None,None))
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
                                      drawingOptions = options;
                                      parenthetical = None 
                                    }
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _
                            | DrawState2DParametric _
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | Tan
                | Sin 
                | Cos as op->    
                    let nextOp = None//Some op
                    let newEvaluationState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,op); 
                              digits = "0";
                              drawingOptions = options;
                              parenthetical = None
                            } nextOp 
                    let oldParentheticalState = (services.getParentheticalFromCalculatorState (stateData |> EvaluatedState))
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newEvaluationState
                        | false -> 
                            match newEvaluationState with                            
                            | EvaluatedState _ev ->
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState                            
                            | ParentheticalState _p -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | EvaluatedState2DParametric (ev,_) ->                                 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ParentheticalState2DParametric (p,_) -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | EvaluatedState3DParametric (ev,_) ->                                 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ParentheticalState3DParametric (p,_) -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newEvaluationState
                    finalState                    
                | _ -> replacePendingFunction stateData (Some f)
        | Draw -> 
            (stateData.evaluatedExpression,stateData.drawingOptions)
            |> doDrawOperation services         
        | Draw2DParametric
        | Draw3DParametric -> EvaluatedState stateData
        | OpenParentheses -> 
            { evaluatedExpression = Expression.Zero
              pendingFunction = None //stateData.pendingFunction
              parenthetical = (services.getParentheticalFromCalculatorState (EvaluatedState stateData)) 
              drawingOptions = stateData.drawingOptions } 
            |> ParentheticalState
        | CloseParentheses -> EvaluatedState stateData
        | GraphOptionSave options -> services.setDrawingOptions (EvaluatedState stateData,options)
        | GraphOptionReset -> services.setDrawingOptions (EvaluatedState stateData,defaultOptions)
        | ExpressionSquared ->
            let nextOp = None//Some op
            let newState = 
                getEvaluationState services 
                    { expression = expr;
                      pendingFunction = Some (expr,ToThePowerOf); 
                      digits = "2";
                      drawingOptions = options;
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
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState p -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | EvaluatedState2DParametric (ev,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=ev.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState2DParametric (p,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | EvaluatedState3DParametric (ev,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=ev.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState3DParametric (p,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | ExpressionDigitAccumulatorState _ 
                    | ExpressionDecimalAccumulatorState _                             
                    | DrawState _
                    | DrawErrorState _
                    | ExpressionErrorState _
                    | DrawState2DParametric _
                    | ExpressionDigitAccumulatorState2DParametric _
                    | ExpressionDecimalAccumulatorState2DParametric _ 
                    | DrawState3DParametric _
                    | ExpressionDigitAccumulatorState3DParametric _
                    | ExpressionDecimalAccumulatorState3DParametric _ -> newState
            finalState
        | ExpressionToThePowerOf -> replacePendingFunction stateData (Some ToThePowerOf)
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState
        
    let handleParentheticalState services stateData input =
        let options = services.getDrawingOptionsFromState (ParentheticalState stateData)
        let zero = Number (Integer 0I)
        let expr = stateData.evaluatedExpression        
        let newExpressionStateData =  
                {expression = expr;  
                 pendingFunction = stateData.pendingFunction;
                 digits = "";
                 drawingOptions = options;
                 parenthetical = services.setExpressionToParenthetical (zero, Some stateData.parenthetical) |> Some  }
        match input with
        | Stack _ -> ParentheticalState stateData
        | CalcInput op -> 
            match op with
            | MathOp m -> 
                match m with
                | Add -> replacePendingFunctionParenthetical stateData (Some Plus)                    
                | Subtract -> replacePendingFunctionParenthetical stateData (Some Minus)                    
                | Multiply -> replacePendingFunctionParenthetical stateData (Some Times)                    
                | Divide -> replacePendingFunctionParenthetical stateData (Some DividedBy)                    
                | CalculatorMathOp.Inverse ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Inverse); 
                              digits = "1";
                              drawingOptions = options;
                              parenthetical = Some stateData.parenthetical
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
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | Percent ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Quotient);
                              digits = "100";
                              drawingOptions = options;
                              parenthetical = Some stateData.parenthetical
                            } nextOp
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newState
                        | false -> 
                            match newState with
                            | EvaluatedState ev ->                                 
                                { digits = ""; 
                                  pendingFunction = stateData.pendingFunction; 
                                  expression = ev.evaluatedExpression;
                                  drawingOptions = options;
                                  parenthetical = None 
                                } |> ExpressionDigitAccumulatorState 
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | CalculatorMathOp.Root ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Root); 
                              digits = "0.5";
                              drawingOptions = options;
                              parenthetical = Some stateData.parenthetical 
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
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | ChangeSign ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Function.Times); 
                              digits = "-1";
                              drawingOptions = options;
                              parenthetical = Some stateData.parenthetical
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
                                      drawingOptions = options;
                                      parenthetical = None 
                                    }
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | MemoryAdd 
                | MemorySubtract -> stateData |> ParentheticalState         
            | CalculatorInput.Zero -> stateData |> ParentheticalState                      
            | DecimalSeparator -> 
                newExpressionStateData 
                |> accumulateSeparator services
                |> ExpressionDecimalAccumulatorState 
            | CalculatorInput.Equals -> replacePendingFunctionParenthetical stateData None                
            | ClearEntry ->                 
                    match stateData.pendingFunction with
                    | None -> stateData |> ParentheticalState 
                    | Some po -> 
                        let _oldExpression, oldPendingFunction = po
                        let zero = Number (Integer 0I)
                        {stateData with pendingFunction = Some (zero,oldPendingFunction)}
                        |> ParentheticalState
            | Clear -> 
                {evaluatedExpression = zero; 
                 pendingFunction=None;
                 drawingOptions=stateData.drawingOptions}
                |> EvaluatedState
            | MemoryRecall -> ParentheticalState stateData //not used
            | MemoryClear -> ParentheticalState stateData //not used
            | MemoryStore -> ParentheticalState stateData //not used
            | Back -> ParentheticalState stateData 
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
                      drawingOptions = options;
                      parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s),Some stateData.parenthetical)|> Some
                    } 
                    |> ExpressionDigitAccumulatorState
                | Some pendingfunc ->                    
                    {expression = expr; 
                     pendingFunction = Some pendingfunc;
                     digits = "";
                     parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s),Some stateData.parenthetical) |> Some;
                     drawingOptions = stateData.drawingOptions}
                    |> ExpressionDigitAccumulatorState
            | Function f -> 
                match f with
                | Derivative ->
                    let nextOp = None//Some op
                    let newState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,Derivative); 
                              digits = "";
                              drawingOptions = options;
                              parenthetical =                                 
                                Some (Parenthetical(Expression.Symbol (Variable "x"),None,None))
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
                                      drawingOptions = options;
                                      parenthetical = None 
                                    }
                            | ParentheticalState p -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression = p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState2DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState2DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | EvaluatedState3DParametric (ev,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=ev.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = None}
                            | ParentheticalState3DParametric (p,_) -> 
                                ExpressionDigitAccumulatorState 
                                    { digits = ""; 
                                      pendingFunction = stateData.pendingFunction; 
                                      expression=p.evaluatedExpression;
                                      drawingOptions = options;
                                      parenthetical = Some p.parenthetical}
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _
                            | DrawState2DParametric _
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newState
                    finalState
                | Tan
                | Sin 
                | Cos as op->    
                    let nextOp = None//Some op
                    let newEvaluationState = 
                        getEvaluationState services 
                            { expression = expr;
                              pendingFunction = Some (expr,op); 
                              digits = "0";
                              drawingOptions = options;
                              parenthetical = Some stateData.parenthetical
                            } nextOp 
                    let oldParentheticalState = (services.getParentheticalFromCalculatorState (stateData |> ParentheticalState))
                    let finalState =
                        match stateData.pendingFunction = None with
                        | true -> newEvaluationState
                        | false -> 
                            match newEvaluationState with                            
                            | EvaluatedState _ev ->
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState                            
                            | ParentheticalState _p -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | EvaluatedState2DParametric (ev,_) ->                                 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ParentheticalState2DParametric (p,_) -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | EvaluatedState3DParametric (ev,_) ->                                 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ParentheticalState3DParametric (p,_) -> 
                                { evaluatedExpression = Expression.Zero
                                  pendingFunction = None //stateData.pendingFunction
                                  drawingOptions = stateData.drawingOptions
                                  parenthetical = (Expression.Zero,Some (Expression.Zero,op),Some oldParentheticalState) |> Parenthetical
                                } |> ParentheticalState
                            | ExpressionDigitAccumulatorState _ 
                            | ExpressionDecimalAccumulatorState _                             
                            | DrawState _
                            | DrawErrorState _
                            | ExpressionErrorState _                             
                            | DrawState2DParametric _                            
                            | ExpressionDigitAccumulatorState2DParametric _
                            | ExpressionDecimalAccumulatorState2DParametric _ 
                            | DrawState3DParametric _                            
                            | ExpressionDigitAccumulatorState3DParametric _
                            | ExpressionDecimalAccumulatorState3DParametric _ -> newEvaluationState
                    finalState
                | _ -> replacePendingFunctionParenthetical stateData (Some f)
        | Draw -> 
            (stateData.evaluatedExpression,stateData.drawingOptions)
            |> doDrawOperation services 
        | Draw2DParametric
        | Draw3DParametric -> ParentheticalState stateData
        | OpenParentheses -> 
            { evaluatedExpression = Expression.Zero
              pendingFunction = None //stateData.pendingFunction
              parenthetical = (services.getParentheticalFromCalculatorState (ParentheticalState stateData)) 
              drawingOptions = stateData.drawingOptions } 
            |> ParentheticalState
        | CloseParentheses -> services.closeParenthetical stateData
        | GraphOptionSave options -> services.setDrawingOptions (ParentheticalState stateData,options)
        | GraphOptionReset -> services.setDrawingOptions (ParentheticalState stateData,defaultOptions)
        | ExpressionSquared ->
            let nextOp = None//Some op
            let newState = 
                getEvaluationState services 
                    { expression = expr;
                      pendingFunction = Some (expr,ToThePowerOf); 
                      digits = "2";
                      drawingOptions = options;
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
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState p -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | EvaluatedState2DParametric (ev,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=ev.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState2DParametric (p,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | EvaluatedState3DParametric (ev,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=ev.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = None}
                    | ParentheticalState3DParametric (p,_) -> 
                        ExpressionDigitAccumulatorState 
                            { digits = ""; 
                              pendingFunction = stateData.pendingFunction; 
                              expression=p.evaluatedExpression;
                              drawingOptions = options;
                              parenthetical = Some p.parenthetical}
                    | ExpressionDigitAccumulatorState _ 
                    | ExpressionDecimalAccumulatorState _                             
                    | DrawState _
                    | DrawErrorState _
                    | ExpressionErrorState _
                    | DrawState2DParametric _
                    | ExpressionDigitAccumulatorState2DParametric _
                    | ExpressionDecimalAccumulatorState2DParametric _ 
                    | DrawState3DParametric _
                    | ExpressionDigitAccumulatorState3DParametric _
                    | ExpressionDecimalAccumulatorState3DParametric _ -> newState
            finalState
        | ExpressionToThePowerOf -> replacePendingFunctionParenthetical stateData (Some ToThePowerOf)
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState
        
    let handleExpressionDigitAccumulatorState services stateData input =           
           let options = services.getDrawingOptionsFromState (ExpressionDigitAccumulatorState stateData)           
           let zero = Number (Integer 0I)  
           let newExpressionStateData =  
                   {expression = stateData.expression;  
                    pendingFunction = stateData.pendingFunction;
                    digits = "";
                    drawingOptions = options;
                    parenthetical = 
                        match stateData.parenthetical with 
                        | Some _ -> services.setExpressionToParenthetical (zero, stateData.parenthetical) |> Some;
                        | None -> None}           
           let getFinalStateFrom = fun newState ->
               match stateData.pendingFunction = None with
               | true -> newState
               | false -> 
                   match newState with                            
                   | EvaluatedState ev -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = ev.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                   | ParentheticalState p -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = p.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (p.evaluatedExpression, stateData.parenthetical) |> Some }
                   | EvaluatedState2DParametric (ev,_) -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = ev.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                   | ParentheticalState2DParametric (p,_) -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = p.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (p.evaluatedExpression, stateData.parenthetical) |> Some }
                   | EvaluatedState3DParametric (ev,_) -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = ev.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                   | ParentheticalState3DParametric (p,_) -> 
                       ExpressionDigitAccumulatorState 
                           { newExpressionStateData with 
                               expression = p.evaluatedExpression;
                               parenthetical = services.setExpressionToParenthetical (p.evaluatedExpression, stateData.parenthetical) |> Some }
                   | ExpressionDigitAccumulatorState _ 
                   | ExpressionDecimalAccumulatorState _                             
                   | DrawState _
                   | DrawErrorState _
                   | ExpressionErrorState _                             
                   | DrawState2DParametric _                            
                   | ExpressionDigitAccumulatorState2DParametric _
                   | ExpressionDecimalAccumulatorState2DParametric _ 
                   | DrawState3DParametric _                            
                   | ExpressionDigitAccumulatorState3DParametric _
                   | ExpressionDecimalAccumulatorState3DParametric _ -> newState 
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
                       let expr = 
                           match stateData.digits, stateData.parenthetical with
                           | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                           | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                           | _, _ -> stateData.expression
                       let newState = 
                           getEvaluationState services 
                               { newExpressionStateData with
                                     pendingFunction = Some (expr,Inverse); 
                                     digits = "1";} nextOp 
                       getFinalStateFrom newState
                   | Percent ->
                       let nextOp = None//Some op
                       let expr = 
                           match stateData.digits, stateData.parenthetical with
                           | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                           | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                           | _, _ -> stateData.expression
                       let newState = 
                           getEvaluationState services 
                               { newExpressionStateData with
                                     pendingFunction = Some (expr,Function.Quotient); 
                                     digits = "100"} nextOp 
                       getFinalStateFrom newState
                   | CalculatorMathOp.Root ->
                       let nextOp = None//Some op
                       let expr = 
                           match stateData.digits, stateData.parenthetical with
                           | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                           | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                           | _, _ -> stateData.expression
                       let newState = 
                           getEvaluationState services 
                               { newExpressionStateData with
                                   pendingFunction = Some (expr,Function.ToThePowerOf); 
                                   digits = "0.5"} nextOp 
                       getFinalStateFrom newState
                   | ChangeSign ->
                       let nextOp = None//Some op
                       let expr = 
                           match stateData.digits, stateData.parenthetical with
                           | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                           | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                           | _, _ -> stateData.expression
                       let newState = 
                           getEvaluationState services 
                               { newExpressionStateData with
                                   pendingFunction = Some (expr,Function.Times); 
                                   digits = "-1"} nextOp 
                       getFinalStateFrom newState
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
                   drawingOptions = options } 
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
                   match stateData.digits with
                   | "" -> 
                       { newExpressionStateData with
                           parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) 
                           |> Some;
                       } 
                       |> ExpressionDigitAccumulatorState
                   | _ ->                       
                       let symbol = (Expression.Symbol s)
                       let newState = 
                           getEvaluationState services 
                               { newExpressionStateData with
                                   pendingFunction = Some (symbol,Times); 
                                   digits = stateData.digits;} None                       
                       getFinalStateFrom newState
               | Function f -> //getEvaluationState services stateData (Some f)
                   match stateData.digits with
                   | "" -> 
                       let expression = services.getExpressionFromParenthetical stateData.parenthetical
                       getEvaluationState services 
                           { newExpressionStateData with
                               pendingFunction = Some (expression,f); 
                               digits = stateData.digits;} None 
                   | _acc ->                       
                       let expression = Number (services.getNumberFromAccumulator stateData)
                       getEvaluationState services 
                           { newExpressionStateData with
                               pendingFunction = Some (expression,f); 
                               digits = stateData.digits;} None


           | Draw -> doDrawOperation services (DrawOp (stateData.expression, options))
           | Draw2DParametric
           | Draw3DParametric -> ExpressionDigitAccumulatorState stateData
           | OpenParentheses -> 
                {stateData with 
                    parenthetical = 
                        (services.getParentheticalFromCalculatorState (ExpressionDigitAccumulatorState stateData)) 
                        |> Some} 
                |> ExpressionDigitAccumulatorState
           | CloseParentheses -> stateData |> ExpressionDigitAccumulatorState
           | GraphOptionSave options -> services.setDrawingOptions (ExpressionDigitAccumulatorState stateData,options)
           | GraphOptionReset -> services.setDrawingOptions (ExpressionDigitAccumulatorState stateData,defaultOptions)
           | ExpressionSquared ->
               let nextOp = None
               let expr = 
                   match stateData.digits, stateData.parenthetical with
                   | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                   | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                   | _, _ -> stateData.expression
               let newState = 
                   getEvaluationState services 
                       { newExpressionStateData with                             
                             pendingFunction = Some (expr,ToThePowerOf); 
                             digits = "2";} nextOp 
               getFinalStateFrom newState
           | ExpressionToThePowerOf -> getEvaluationState services stateData (Some ToThePowerOf)
           | SpiralExample -> spiral_ExampleState
           | EllipseExample -> ellipse_ExampleState
           | HelixExample -> helix_ExampleState
           | SphereExample -> sphere_ExampleState
           | ConeExample -> cone_ExampleState
           | TorusExample -> torus_ExampleState
           
    let handleExpressionDecimalAccumulatorState services stateData input =
        let options = services.getDrawingOptionsFromState (ExpressionDecimalAccumulatorState stateData)        
        let zero = Number (Integer 0I)  
        let newExpressionStateData =  
                {expression = stateData.expression;  
                 pendingFunction = stateData.pendingFunction;
                 digits = "";
                 drawingOptions = options;
                 parenthetical = 
                     match stateData.parenthetical with 
                     | Some _ -> services.setExpressionToParenthetical (zero, stateData.parenthetical) |> Some;
                     | None -> None}           
        let getFinalStateFrom = fun newState ->
            match stateData.pendingFunction = None with
            | true -> newState
            | false -> 
                match newState with                            
                | EvaluatedState ev -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = ev.evaluatedExpression;
                            parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                | ParentheticalState p -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = p.evaluatedExpression}
                | EvaluatedState2DParametric (ev,_) -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = ev.evaluatedExpression;
                            parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                | ParentheticalState2DParametric (p,_) -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = p.evaluatedExpression;
                            parenthetical = services.setExpressionToParenthetical (p.evaluatedExpression, stateData.parenthetical) |> Some }
                | EvaluatedState3DParametric (ev,_) -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = ev.evaluatedExpression;
                            parenthetical = services.setExpressionToParenthetical (ev.evaluatedExpression, stateData.parenthetical) |> Some}
                | ParentheticalState3DParametric (p,_) -> 
                    ExpressionDigitAccumulatorState 
                        { newExpressionStateData with 
                            expression = p.evaluatedExpression;
                            parenthetical = services.setExpressionToParenthetical (p.evaluatedExpression, stateData.parenthetical) |> Some }
                | ExpressionDigitAccumulatorState _ 
                | ExpressionDecimalAccumulatorState _                             
                | DrawState _
                | DrawErrorState _
                | ExpressionErrorState _
                | DrawState2DParametric _
                | ExpressionDigitAccumulatorState2DParametric _
                | ExpressionDecimalAccumulatorState2DParametric _ 
                | DrawState3DParametric _
                | ExpressionDigitAccumulatorState3DParametric _
                | ExpressionDecimalAccumulatorState3DParametric _ -> newState 
        match input with
        | Stack _ -> ExpressionDecimalAccumulatorState stateData
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
                    let expr = 
                        match stateData.digits, stateData.parenthetical with
                        | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                        | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                        | _, _ -> stateData.expression
                    let newState = 
                        getEvaluationState services 
                            { newExpressionStateData with
                                  pendingFunction = Some (expr,Inverse); 
                                  digits = "1";} nextOp 
                    getFinalStateFrom newState
                | Percent ->
                    let nextOp = None//Some op
                    let expr = 
                        match stateData.digits, stateData.parenthetical with
                        | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                        | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                        | _, _ -> stateData.expression
                    let newState = 
                        getEvaluationState services 
                            { newExpressionStateData with
                                  pendingFunction = Some (expr,Function.Quotient); 
                                  digits = "100"} nextOp 
                    getFinalStateFrom newState
                | CalculatorMathOp.Root ->
                    let nextOp = None//Some op
                    let expr = 
                        match stateData.digits, stateData.parenthetical with
                        | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                        | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                        | _, _ -> stateData.expression
                    let newState = 
                        getEvaluationState services 
                            { newExpressionStateData with
                                pendingFunction = Some (expr,Function.ToThePowerOf); 
                                digits = "0.5"} nextOp 
                    getFinalStateFrom newState
                | ChangeSign ->
                    let nextOp = None//Some op
                    let expr = 
                        match stateData.digits, stateData.parenthetical with
                        | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                        | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                        | _, _ -> stateData.expression
                    let newState = 
                        getEvaluationState services 
                            { newExpressionStateData with
                                pendingFunction = Some (expr,Function.Times); 
                                digits = "-1"} nextOp 
                    getFinalStateFrom newState
                | MemoryAdd 
                | MemorySubtract -> ExpressionDecimalAccumulatorState stateData         
            | CalculatorInput.Zero -> 
                stateData
                |> accumulateZero services 
                |> ExpressionDecimalAccumulatorState                     
            | DecimalSeparator -> 
                stateData |> ExpressionDecimalAccumulatorState 
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
                drawingOptions = options } 
               |> EvaluatedState
            | MemoryRecall -> ExpressionDecimalAccumulatorState stateData //not used
            | MemoryClear -> ExpressionDecimalAccumulatorState stateData //not used
            | MemoryStore -> ExpressionDecimalAccumulatorState stateData //not used
            | Back -> // I need to come back to this function to incorporate expression type
                 match stateData.digits.Length with
                 | x when x <= 2 -> newExpressionStateData |> ExpressionDigitAccumulatorState
                 | x -> {newExpressionStateData with digits = stateData.digits.Remove(x-1)}
                        |> ExpressionDigitAccumulatorState  
            | Digit d -> 
                stateData
                |> accumulateNonZeroDigit services d
                |> ExpressionDigitAccumulatorState 
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> 
                match stateData.digits with
                | "" -> 
                    { newExpressionStateData with
                        parenthetical = services.setExpressionToParenthetical ((Expression.Symbol s), stateData.parenthetical) 
                        |> Some;
                    } 
                    |> ExpressionDigitAccumulatorState
                | _ ->                    
                    let symbol = (Expression.Symbol s)
                    let newState = 
                        getEvaluationState services 
                            { newExpressionStateData with
                                pendingFunction = Some (symbol,Times); 
                                digits = stateData.digits;} None                       
                    getFinalStateFrom newState
            | Function f -> getEvaluationState services stateData (Some f)
        | Draw -> doDrawOperation services (DrawOp (stateData.expression, options))
        | Draw2DParametric
        | Draw3DParametric -> ExpressionDecimalAccumulatorState stateData
        | OpenParentheses -> 
             {stateData with 
                 parenthetical = 
                     (services.getParentheticalFromCalculatorState (ExpressionDigitAccumulatorState stateData)) 
                     |> Some} 
             |> ExpressionDigitAccumulatorState
        | CloseParentheses -> stateData |> ExpressionDigitAccumulatorState
        | GraphOptionSave options -> services.setDrawingOptions (ExpressionDecimalAccumulatorState stateData,options)
        | GraphOptionReset -> services.setDrawingOptions (ExpressionDecimalAccumulatorState stateData,defaultOptions)
        | ExpressionSquared ->
            let nextOp = None//Some op
            let expr = 
                match stateData.digits, stateData.parenthetical with
                | s, _ when s.Length > 0 -> services.getNumberFromAccumulator (stateData) |> Expression.Number 
                | _, Some (Parenthetical(e,_,_)) when e <> Expression.Zero -> e                
                | _, _ -> stateData.expression
            let newState = 
                getEvaluationState services 
                    { newExpressionStateData with
                          pendingFunction = Some (expr,ToThePowerOf); 
                          digits = "2";} nextOp 
            getFinalStateFrom newState
        | ExpressionToThePowerOf -> getEvaluationState services stateData (Some ToThePowerOf)
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState
        
    let handleDrawErrorState services stateData input =           
        let options = services.getDrawingOptionsFromState (DrawErrorState stateData)
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
                drawingOptions = options}
                |> EvaluatedState
        | ExpressionInput i -> DrawErrorState stateData
        | Draw -> DrawErrorState stateData
        | Draw2DParametric
        | Draw3DParametric -> DrawErrorState stateData
        | OpenParentheses -> DrawErrorState stateData
        | CloseParentheses -> DrawErrorState stateData
        | GraphOptionSave options -> DrawErrorState stateData
        | ExpressionSquared
        | ExpressionToThePowerOf
        | GraphOptionReset -> DrawErrorState stateData
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState
        
    let handleExpressionErrorState services stateData input =
        let options = services.getDrawingOptionsFromState (ExpressionErrorState stateData)
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
                drawingOptions = options}
                |> EvaluatedState
        | ExpressionInput i -> DrawErrorState stateData
        | Draw -> DrawErrorState stateData
        | Draw2DParametric
        | Draw3DParametric -> DrawErrorState stateData
        | OpenParentheses -> DrawErrorState stateData
        | CloseParentheses -> DrawErrorState stateData
        | GraphOptionSave _options -> DrawErrorState stateData
        | ExpressionSquared
        | ExpressionToThePowerOf
        | GraphOptionReset -> DrawErrorState stateData
        | SpiralExample -> spiral_ExampleState
        | EllipseExample -> ellipse_ExampleState
        | HelixExample -> helix_ExampleState
        | SphereExample -> sphere_ExampleState
        | ConeExample -> cone_ExampleState
        | TorusExample -> torus_ExampleState
        
    let createEvaluate (services:GraphServices) : Evaluate = 
         // create some local functions with partially applied services
         let handleDrawState = handleDrawState services
         let handleEvaluatedState = handleEvaluatedState services
         let handleParentheticalState = handleParentheticalState services
         let handleExpressionDigitAccumulatorState = handleExpressionDigitAccumulatorState services
         let handleExpressionDecimalAccumulatorState = handleExpressionDecimalAccumulatorState services
         let handleDrawErrorState = handleDrawErrorState services
         let handleExpressionErrorState = handleExpressionErrorState services
         let doDraw2DParametricOperation = doDraw2DParametricOperation services
         let doDraw3DParametricOperation = doDraw3DParametricOperation services
         // helper to wrap graph state data into a 2DParametric state
         let wrapStateData2D state newState = 
            match newState with
            | EvaluatedState s -> (s,state) |> EvaluatedState2DParametric
            | DrawState s -> (s,state) |> DrawState2DParametric
            | ParentheticalState s -> (s,state) |> ParentheticalState2DParametric
            | ExpressionDigitAccumulatorState s -> (s,state) |> ExpressionDigitAccumulatorState2DParametric
            | ExpressionDecimalAccumulatorState s -> (s,state) |> ExpressionDecimalAccumulatorState2DParametric
            | DrawErrorState _
            | ExpressionErrorState _
            | EvaluatedState2DParametric _ 
            | DrawState2DParametric _
            | ParentheticalState2DParametric _
            | ExpressionDigitAccumulatorState2DParametric _
            | ExpressionDecimalAccumulatorState2DParametric _ 
            | EvaluatedState3DParametric _ 
            | DrawState3DParametric _
            | ParentheticalState3DParametric _
            | ExpressionDigitAccumulatorState3DParametric _
            | ExpressionDecimalAccumulatorState3DParametric _ -> newState

         // helper to wrap graph state data into a 3DParametric state
         let wrapStateData3D dim3DStateData newState = 
            match newState with
            | EvaluatedState s -> (s,dim3DStateData) |> EvaluatedState3DParametric
            | DrawState s -> (s,dim3DStateData) |> DrawState3DParametric
            | ParentheticalState s -> (s,dim3DStateData) |> ParentheticalState3DParametric
            | ExpressionDigitAccumulatorState s -> (s,dim3DStateData) |> ExpressionDigitAccumulatorState3DParametric
            | ExpressionDecimalAccumulatorState s -> (s,dim3DStateData) |> ExpressionDecimalAccumulatorState3DParametric
            | DrawErrorState _
            | ExpressionErrorState _
            | EvaluatedState2DParametric _ 
            | DrawState2DParametric _
            | ParentheticalState2DParametric _
            | ExpressionDigitAccumulatorState2DParametric _
            | ExpressionDecimalAccumulatorState2DParametric _ 
            | EvaluatedState3DParametric _ 
            | DrawState3DParametric _
            | ParentheticalState3DParametric _
            | ExpressionDigitAccumulatorState3DParametric _
            | ExpressionDecimalAccumulatorState3DParametric _ -> newState

         fun (input,calcState) -> 
             match calcState with
             | DrawState stateData -> 
                 handleDrawState stateData input 
             | EvaluatedState stateData -> 
                 handleEvaluatedState stateData input 
             | ParentheticalState stateData -> 
                 handleParentheticalState stateData input
             | ExpressionDigitAccumulatorState stateData -> 
                 handleExpressionDigitAccumulatorState stateData input
             | ExpressionDecimalAccumulatorState stateData -> 
                 handleExpressionDecimalAccumulatorState stateData input
             | DrawErrorState stateData -> 
                 handleDrawErrorState stateData input
             | ExpressionErrorState stateData -> 
                 handleExpressionErrorState stateData input             
             | EvaluatedState2DParametric (stateData,state) ->
                 match input  with
                 | Draw2DParametric -> doDraw2DParametricOperation calcState
                 | _ -> handleEvaluatedState stateData input |> wrapStateData2D state                 
             | DrawState2DParametric (stateData,state) ->
                match input  with
                | Draw2DParametric -> doDraw2DParametricOperation calcState
                | _ -> handleDrawState stateData input |> wrapStateData2D state
             | ParentheticalState2DParametric (stateData,state) ->
                match input  with
                | Draw2DParametric -> doDraw2DParametricOperation calcState
                | _ -> handleParentheticalState stateData input |> wrapStateData2D state
             | ExpressionDigitAccumulatorState2DParametric (stateData,state) ->
                match input  with
                | Draw2DParametric -> doDraw2DParametricOperation calcState
                | _ -> handleExpressionDigitAccumulatorState stateData input |> wrapStateData2D state
             | ExpressionDecimalAccumulatorState2DParametric (stateData,state) ->
                match input  with
                | Draw2DParametric -> doDraw2DParametricOperation calcState
                | _ -> handleExpressionDecimalAccumulatorState stateData input |> wrapStateData2D state
             | EvaluatedState3DParametric (stateData,state) ->
                 match input  with
                 | Draw3DParametric -> doDraw3DParametricOperation calcState
                 | _ -> handleEvaluatedState stateData input |> wrapStateData3D state                 
             | DrawState3DParametric (stateData,state) ->
                match input  with
                //| Draw3DParametric -> doDraw3DParametricOperation calcState
                | _ -> handleDrawState stateData input |> wrapStateData3D state
             | ParentheticalState3DParametric (stateData,state) ->
                match input  with
                //| Draw3DParametric -> doDraw3DParametricOperation calcState
                | _ -> handleParentheticalState stateData input |> wrapStateData3D state
             | ExpressionDigitAccumulatorState3DParametric (stateData,state) ->
                match input  with
                //| Draw3DParametric -> doDraw3DParametricOperation calcState
                | _ -> handleExpressionDigitAccumulatorState stateData input |> wrapStateData3D state
             | ExpressionDecimalAccumulatorState3DParametric (stateData,state) ->
                match input  with
                //| Draw3DParametric -> doDraw3DParametricOperation calcState
                | _ -> handleExpressionDecimalAccumulatorState stateData input |> wrapStateData3D state

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
            | ParentheticalState p -> p.evaluatedExpression.ToString()
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
            | DrawErrorState de -> "derror" + de.ToString()
            | ExpressionErrorState ee -> "eerror" + ee.ToString()
            | EvaluatedState2DParametric (e,_) -> e.evaluatedExpression.ToString()
            | DrawState2DParametric (d,_) -> d.trace.ToString()
            | ParentheticalState2DParametric (p,_) -> p.evaluatedExpression.ToString()
            | ExpressionDigitAccumulatorState2DParametric (d,_) -> 
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
            | ExpressionDecimalAccumulatorState2DParametric (d,_) -> 
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
            | EvaluatedState3DParametric (e,_) -> e.evaluatedExpression.ToString()
            | DrawState3DParametric (d,_) -> d.mesh.Value.ToString()
            | ParentheticalState3DParametric (p,_) -> p.evaluatedExpression.ToString()
            | ExpressionDigitAccumulatorState3DParametric (d,_) -> 
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
            | ExpressionDecimalAccumulatorState3DParametric (d,_) -> 
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

    let getDim3DStateDataFromState :GetDim3DStateDataFromState = 
        let zero = 
            Math.Pure.Quantity.Number.Zero 
            |> Math.Pure.Structure.Number
        fun g -> 
            match g with 
            | EvaluatedState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ParentheticalState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | DrawState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ExpressionDigitAccumulatorState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ExpressionDecimalAccumulatorState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | DrawErrorState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ExpressionErrorState _ -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | EvaluatedState2DParametric (_,_) -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | DrawState2DParametric (_,_) -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ParentheticalState2DParametric (_p,_) -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ExpressionDigitAccumulatorState2DParametric (_,_) -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | ExpressionDecimalAccumulatorState2DParametric (_,_) -> {x=zero;y=zero;z=zero;activeExpression=X_}
            | EvaluatedState3DParametric (_,d3) -> d3
            | DrawState3DParametric (_d,d3) -> d3
            | ParentheticalState3DParametric (_,d3) -> d3
            | ExpressionDigitAccumulatorState3DParametric (_,d3) -> d3
            | ExpressionDecimalAccumulatorState3DParametric (_,d3) -> d3

    let getDisplayFromPendingFunction (pendingFunction : PendingFunction option) =
        match pendingFunction with
        | Some x -> x.ToString()
        | None -> ""

    let getDrawingOptionsFromState :GetDrawingOptionsFromState = 
        fun g -> 
            match g with 
            | EvaluatedState e -> e.drawingOptions
            | DrawState d -> d.drawingOptions
            | ExpressionDecimalAccumulatorState e -> e.drawingOptions
            | ExpressionDigitAccumulatorState e -> e.drawingOptions
            | ParentheticalState p -> p.drawingOptions            
            | EvaluatedState2DParametric (stateData,_state) -> stateData.drawingOptions
            | DrawState2DParametric (stateData,_state) -> stateData.drawingOptions
            | ParentheticalState2DParametric (stateData,_state) -> stateData.drawingOptions
            | ExpressionDigitAccumulatorState2DParametric (stateData,_tate) -> stateData.drawingOptions
            | ExpressionDecimalAccumulatorState2DParametric (stateData,_state) -> stateData.drawingOptions            
            | EvaluatedState3DParametric (stateData,_state) -> stateData.drawingOptions
            | DrawState3DParametric (stateData,_state) -> stateData.drawingOptions
            | ParentheticalState3DParametric (stateData,_state) -> stateData.drawingOptions
            | ExpressionDigitAccumulatorState3DParametric (stateData,_tate) -> stateData.drawingOptions
            | ExpressionDecimalAccumulatorState3DParametric (stateData,_state) -> stateData.drawingOptions
            | _ -> GraphingImplementation.defaultOptions

    let getValueFrom numberType =
        match numberType with
        | Real r -> r
        | Integer i -> float i
        | _ -> System.Double.NaN

    let setDrawingOptions :SetDrawingOptions = 
        fun g ->             
            let state, options = g
            match state with 
            | EvaluatedState e -> {e with drawingOptions = options} |> EvaluatedState
            | DrawState d -> {d with drawingOptions = options} |> DrawState
            | ExpressionDecimalAccumulatorState e -> {e with drawingOptions = options} |> ExpressionDecimalAccumulatorState
            | ExpressionDigitAccumulatorState e -> {e with drawingOptions = options} |> ExpressionDigitAccumulatorState
            | ParentheticalState p -> {p with drawingOptions = options} |> ParentheticalState            
            | EvaluatedState2DParametric (stateData,state) -> ({stateData with drawingOptions = options},state) |> EvaluatedState2DParametric 
            | DrawState2DParametric (stateData,state) -> ({stateData with drawingOptions = options},state) |> DrawState2DParametric
            | ParentheticalState2DParametric (stateData,state) -> ({stateData with drawingOptions = options},state) |> ParentheticalState2DParametric
            | ExpressionDigitAccumulatorState2DParametric (stateData,state) -> ({stateData with drawingOptions = options},state) |> ExpressionDigitAccumulatorState2DParametric
            | ExpressionDecimalAccumulatorState2DParametric (stateData,state) -> ({stateData with drawingOptions = options},state) |> ExpressionDecimalAccumulatorState2DParametric            
            | _ -> state
    
    let doDrawOperation resolution (drawOp:DrawOp):DrawOperationResult = 
        let expression, drawBounds = drawOp
        let getValueFrom numberType =
            match numberType with
            | Real r -> r
            | Integer i -> float i
            | _ -> System.Double.NaN
        let valueOfX coordinate = match coordinate with X x -> getValueFrom x
        let valueOfY coordinate = match coordinate with Y y -> getValueFrom y
        let xMin, xMax, yMin, yMax = 
            valueOfX drawBounds.lowerX, 
            valueOfX drawBounds.upperX, 
            valueOfY drawBounds.lowerY, 
            valueOfY drawBounds.upperY                 
        
        let makePoint xExpression yExpression = 
            let xValue =
                match xExpression with
                | Number n -> n
                | _ -> Undefined
            let yValue =
                match yExpression with 
                | Number n -> n
                | _ -> Undefined
            Point(X xValue,Y yValue)
        
        let evaluate expression xValue = 
            expression
            |> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
            |> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
            |> ExpressionStructure.substitute (Expression.Symbol (Variable "x"), xValue)            
            |> ExpressionFunction.evaluateRealPowersOfExpression            
            
        let partitionInfinity = 
            let rec loop acc lcc = function
                | (Number(Real x), Number(Real y))::pl when y <> infinity && y <> -infinity -> loop ((Number(Real x),Number(Real y))::acc) lcc pl
                | [] -> acc::lcc //, []
                | pl -> 
                    let infinityPoint = 
                        match pl,acc with
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 =  infinity && y1 <= 0. -> (x0,Number(Real yMin))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 <= 0. -> (x0,Number(Real yMin))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 =  infinity && y1 >= 0. -> (x0,Number(Real yMax))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 >= 0. -> (x0,Number(Real yMax))                     
                        | [],acc -> (fst acc.Head, Number(Real yMax))                        
                        | _      -> (fst  pl.Head, Number(Real yMax))
                    let p =
                          match pl with
                          | (x0,Number(Real _y0))::(_x1,Number(Real y1))::_t when y1 <= 0. -> (x0,Number(Real yMin))
                          | (x0,Number(Real _y0))::(_x1,Number(Real y1))::_t when y1 >  0. -> (x0,Number(Real yMax))
                          | _ -> (fst pl.Head, Number(Real 150.))
                    loop [p] ((List.rev (infinityPoint::acc))::lcc) pl.Tail             
            loop [] []
            
        let xCoordinates = seq {for x in xMin .. resolution .. xMax -> Number(Real x)}
        let yCoordinates = Seq.map (fun x -> evaluate expression x ) xCoordinates        
        
        let coordinatePairs = 
            Seq.zip xCoordinates yCoordinates 
            |> Seq.filter (fun (_x,y) -> match y with | Number(Real r) when System.Double.IsNaN(r) = true -> false | _ -> true)
            |> Seq.toList
            |> partitionInfinity

        let points = List.map (fun pl -> seq { for (x,y) in pl do yield makePoint x y }) coordinatePairs 
                    
        let checkForUndefinedPoints = 
            points 
            |> Seq.concat
            |> Seq.exists (fun x -> 
                match x with 
                | Point(X x,Y y) when x = Undefined || y = Undefined -> true 
                | _ -> false)  
        
        let createTrace ps =
            { startPoint = Seq.head ps;
              traceSegments = 
                  Seq.tail ps 
                  |> Seq.toList 
                  |> List.map (fun x -> LineSegment x) }  
       
        match checkForUndefinedPoints with
        | true -> DrawError FailedToCreateTrace
        | false -> 
            let pointLists = points          
            let out = List.map (fun x -> createTrace x) pointLists 
            out |> Traces
    
    let doDraw2DParametricOperation (drawOpXt:DrawOp, drawOpYt:DrawOp):DrawOperationResult =         
        let expressionXt, drawOptionsX = drawOpXt
        let expressionYt, drawOptionsY = drawOpYt
        
        let valueOfT coordinate = match coordinate with T t -> getValueFrom t
        let valueOfY coordinate = match coordinate with Y y -> getValueFrom y
        let valueOfX coordinate = match coordinate with X x -> getValueFrom x
        let tMin, tMax, yMin, yMax, xMin, xMax = 
            valueOfT drawOptionsX.lowerT, 
            valueOfT drawOptionsX.upperT, 
            valueOfY drawOptionsY.lowerY, 
            valueOfY drawOptionsY.upperY, 
            valueOfX drawOptionsX.lowerX, 
            valueOfX drawOptionsX.upperX
        
        let tStep = match drawOptionsX.Tstep with | Tstep (Real x) -> x | _ -> System.Double.NaN

        let makePoint xExpression yExpression = 
            let xValue =
                match xExpression with
                | Number n -> n
                | _ -> Undefined
            let yValue =
                match yExpression with 
                | Number n -> n
                | _ -> Undefined
            Point(X xValue,Y yValue)
        
        let evaluate expression tValue = 
            expression
            |> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
            |> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
            |> ExpressionStructure.substitute (Expression.Symbol (Variable "t"), tValue)            
            |> ExpressionFunction.evaluateRealPowersOfExpression            
            
        let partitionInfinityY = 
            let rec loop acc lcc = function
                | (Number(Real x), Number(Real y))::pl when y <> infinity && y <> -infinity -> loop ((Number(Real x),Number(Real y))::acc) lcc pl
                | [] -> acc::lcc //, []
                | pl -> 
                    let infinityPoint = 
                        match pl,acc with
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 =  infinity && y1 <= 0. -> (x0,Number(Real yMin))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 <= 0. -> (x0,Number(Real yMin))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 =  infinity && y1 >= 0. -> (x0,Number(Real yMax))
                        | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 >= 0. -> (x0,Number(Real yMax))                     
                        | [],acc -> (fst acc.Head, Number(Real yMax))                        
                        | _      -> (fst  pl.Head, Number(Real yMax))
                    let p =
                          match pl with
                          | (x0,Number(Real _y0))::(_x1,Number(Real y1))::_t when y1 <= 0. -> (x0,Number(Real yMin))
                          | (x0,Number(Real _y0))::(_x1,Number(Real y1))::_t when y1 >  0. -> (x0,Number(Real yMax))
                          | _ -> (fst pl.Head, Number(Real 150.))
                    loop [p] ((List.rev (infinityPoint::acc))::lcc) pl.Tail             
            loop [] []

        let partitionInfinityX inputList = 
            let partition = 
                let rec loop acc lcc = function
                    | (Number(Real x), Number(Real y))::pl when x <> infinity && x <> -infinity -> loop ((Number(Real x),Number(Real y))::acc) lcc pl
                    | [] -> acc::lcc //, []
                    | pl -> 
                        let infinityPoint = 
                            match pl,acc with
                            | (Number(Real x0),Number(Real y0))::_plTail,(Number(Real x1),Number(Real y1))::_accTail when x0 =  infinity && x1 <= 0. -> (Number(Real xMin),Number(Real y0))
                            | (Number(Real x0),Number(Real y0))::_plTail,(Number(Real x1),Number(Real y1))::_accTail when x0 = -infinity && x1 <= 0. -> (Number(Real xMin),Number(Real y0))
                            | (Number(Real x0),Number(Real y0))::_plTail,(Number(Real x1),Number(Real y1))::_accTail when x0 =  infinity && x1 >= 0. -> (Number(Real xMax),Number(Real y0))
                            | (Number(Real x0),Number(Real y0))::_plTail,(Number(Real x1),Number(Real y1))::_accTail when x0 = -infinity && x1 >= 0. -> (Number(Real xMax),Number(Real y0))                    
                            | [],acc -> (Number(Real xMax), snd acc.Head)                        
                            | _      -> (Number(Real xMax), snd  pl.Head)
                        let p =
                              match pl with
                              | (Number(Real x0),Number(Real y0))::(Number(Real x1),Number(Real y1))::_t when x1 <= 0. -> (Number(Real xMin),Number(Real  y0))
                              | (Number(Real x0),Number(Real y0))::(Number(Real x1),Number(Real y1))::_t when x1 >  0. -> (Number(Real xMax),Number(Real  y0))
                              | _ -> (Number(Real 150.), snd pl.Head)//pl.Head//
                        loop [p] ((List.rev (infinityPoint::acc))::lcc) pl.Tail             
                loop [] []
            List.collect (fun x -> partition x) inputList

        let t = seq {for x in tMin .. tStep .. tMax -> Number(Real x)}
        let xCoordinates = Seq.map (fun x -> evaluate expressionXt x ) t
        let yCoordinates = Seq.map (fun y -> evaluate expressionYt y ) t
        
        let coordinatePairs = 
            Seq.zip xCoordinates yCoordinates 
            |> Seq.filter (fun (x,_y) -> match x with | Number(Real r) when System.Double.IsNaN(r) = true -> false | _ -> true)
            |> Seq.filter (fun (_x,y) -> match y with | Number(Real r) when System.Double.IsNaN(r) = true -> false | _ -> true)
            |> Seq.toList
            |> partitionInfinityY
            |> partitionInfinityX
        
        let points = List.map (fun pl -> seq { for (x,y) in pl do yield makePoint x y }) coordinatePairs 
                    
        let checkForUndefinedPoints = 
            points 
            |> Seq.concat
            |> Seq.exists (fun x -> 
                match x with 
                | Point(X x,Y y) when x = Undefined || y = Undefined -> true 
                | _ -> false)  
        
        let createTrace ps =
            { startPoint = Seq.head ps;
              traceSegments = 
                  Seq.tail ps 
                  |> Seq.toList 
                  |> List.map (fun x -> LineSegment x) }  
       
        match checkForUndefinedPoints with
        | true -> DrawError FailedToCreateTrace
        | false -> 
            let pointLists = points          
            let out = List.map (fun x -> createTrace x) pointLists 
            out |> Traces
    
    let doDraw3DParametricOperation (drawOp3D :DrawOp3D) :DrawOperationResult  = 
        let expressions,options = drawOp3D 
        let symbols =
            let xParameter = ExpressionStructure.variables expressions.x
            let yParameter = ExpressionStructure.variables expressions.y 
            let zParameter = ExpressionStructure.variables expressions.z 
            List.concat [xParameter;yParameter;zParameter]
        let hasT = symbols |> List.contains (Symbol.Variable "t")  
        let hasU = symbols |> List.contains (Symbol.Variable "u")
        let hasV = symbols |> List.contains (Symbol.Variable "v") 
        
        let valueOfT coordinate = match coordinate with T t -> getValueFrom t
        let valueOfU coordinate = match coordinate with U u -> getValueFrom u
        let valueOfV coordinate = match coordinate with V v -> getValueFrom v
        let valueOfX coordinate = match coordinate with X x -> getValueFrom x
        let valueOfY coordinate = match coordinate with Y y -> getValueFrom y
        let valueOfZ coordinate = match coordinate with Z z -> getValueFrom z
        let tMin, tMax, uMin, uMax, vMin, vMax = 
            valueOfT options.lowerT, 
            valueOfT options.upperT, 
            valueOfU options.lowerU, 
            valueOfU options.upperU, 
            valueOfV options.lowerV, 
            valueOfV options.upperV
        
        let tStep = match options.Tstep with | Tstep (Real x) -> x | _ -> System.Double.NaN
        let uStep = match options.uStep with | Ustep (Real x) -> x | _ -> System.Double.NaN
        let vStep = match options.vStep with | Vstep (Real x) -> x | _ -> System.Double.NaN
        
        let makePoint xExpression yExpression zExpression = 
            let xValue =
                match xExpression with
                | Number n -> n
                | _ -> Undefined
            let yValue =
                match yExpression with 
                | Number n -> n
                | _ -> Undefined
            let zValue =
                match zExpression with 
                | Number n -> n
                | _ -> Undefined
            Point3D(X xValue,Y yValue,Z zValue)
        let getVectorfrom point =
                match point with
                | Point (x,y) -> System.Windows.Media.Media3D.Vector3D(valueOfX x, valueOfY y,0.)
                | Point3D (x,y,z) -> System.Windows.Media.Media3D.Vector3D(valueOfX x, valueOfY y, valueOfZ z)
        let getPoint3Dfrom point =
            match point with
            | Point (x,y) -> System.Windows.Media.Media3D.Point3D(valueOfX x, valueOfY y,0.)
            | Point3D (x,y,z) -> System.Windows.Media.Media3D.Point3D(valueOfX x, valueOfY y, valueOfZ z)
        let transformModel (model :System.Windows.Media.Media3D.GeometryModel3D) (v1 :System.Windows.Media.Media3D.Vector3D)  (p :Point) =             
            let v2 =             
                let v = System.Windows.Media.Media3D.Vector3D(0.,0.,1.)
                do  v.Normalize()
                v
            
            let transforms = System.Windows.Media.Media3D.Transform3DGroup()
            let translation = System.Windows.Media.Media3D.TranslateTransform3D(getVectorfrom p)
            
            let axis = 
                let dot =  System.Windows.Media.Media3D.Vector3D.DotProduct (v1, v2)
                match dot < 0. with
                | true -> System.Windows.Media.Media3D.Vector3D.CrossProduct (v1, v2)        
                | false -> System.Windows.Media.Media3D.Vector3D.CrossProduct (v2, v1)

            let angle = axis.Length * (180./System.Math.PI)
            
            let quaternion = 

                let q = (System.Windows.Media.Media3D.Quaternion(axis, angle))
                do q.Normalize()
                q
            
            let rotation = System.Windows.Media.Media3D.QuaternionRotation3D(quaternion) |> System.Windows.Media.Media3D.RotateTransform3D
            do  transforms.Children.Add(rotation)
                transforms.Children.Add(translation)
                model.Transform <- transforms
            model

        let evaluateT expression value = 
            expression
            |> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
            |> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
            |> ExpressionStructure.substitute (Expression.Symbol (Variable "t"), value)            
            |> ExpressionFunction.evaluateRealPowersOfExpression

        let evaluateU expression value = 
            expression
            |> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
            |> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
            |> ExpressionStructure.substitute (Expression.Symbol (Variable "u"), value)

        let evaluateV expression value = 
            expression
            |> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
            |> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
            |> ExpressionStructure.substitute (Expression.Symbol (Variable "v"), value)
            |> ExpressionFunction.evaluateRealPowersOfExpression
        
        match hasT, hasU, hasV with
        | true, false, false ->
            let x(t) = evaluateT expressions.x (Number(Real t))
            let y(t) = evaluateT expressions.y (Number(Real t))
            let z(t) = evaluateT expressions.z (Number(Real t))            
            let points0 = seq{for t in tMin..tStep..tMax -> makePoint (x (t)) (y(t)) (z(t))}
            let points1 = seq{for t in (tMin + tStep)..tStep..(tMax + tStep) -> makePoint (x (t)) (y(t)) (z(t))}
            let points = Seq.zip points0 points1
            let normals = 
                seq{for p0, p1 in points 
                    -> let vector = (getVectorfrom p1) - (getVectorfrom p0)
                       do vector.Normalize()
                       vector}        
            let pointsAndNormals = Seq.zip points0 normals        
            let models = seq{for p,n in pointsAndNormals -> 
                                let pixel = Pixel.Box ()
                                transformModel pixel n p}
            let model3DGroup = System.Windows.Media.Media3D.Model3DGroup()        
            do Seq.iter (fun m -> model3DGroup.Children.Add(m)) models
            Curve model3DGroup |> Mesh
        | false, _, _ -> 
            let x(u,v) = 
                let v' = evaluateU expressions.x (Number(Real u))
                evaluateV v' (Number(Real v))  
            let y(u,v) = 
                let v' = evaluateU expressions.y (Number(Real u))
                evaluateV v' (Number(Real v))  
            let z(u,v) = 
                let v' = evaluateU expressions.z (Number(Real u))
                evaluateV v' (Number(Real v))  
            let points0 = 
                seq{for u in uMin..uStep..uMax do 
                      for v in vMin..vStep..vMax -> makePoint (x(u, v)) (y(u, v)) (z(u, v))}
            let points1 = 
                seq{for u in uMin..uStep..uMax do 
                      for v in vMin..vStep..vMax -> makePoint (x(u, v + vStep))  (y(u, v + vStep)) (z(u, v + vStep))}
            let points2 = 
                seq{for u in uMin..uStep..uMax do 
                      for v in vMin..vStep..vMax -> makePoint (x(u + uStep, v + vStep)) (y(u + uStep, v + vStep)) (z(u + uStep, v + vStep))}
            let points3 = 
                seq{for u in uMin..uStep..uMax do 
                      for v in vMin..vStep..vMax -> makePoint (x(u + uStep, v)) (y(u + uStep, v)) (z(u + uStep, v))}            
            let points_1 = Seq.zip3 points0 points1 points2
            let points_2 = Seq.zip3 points2 points3 points0
            let meshGeometry = System.Windows.Media.Media3D.MeshGeometry3D()
            do  Seq.iter (fun n -> 
                let p1,p2,p3 = n 
                // Front
                meshGeometry.Positions.Add(getPoint3Dfrom p3)
                meshGeometry.Positions.Add(getPoint3Dfrom p2)
                meshGeometry.Positions.Add(getPoint3Dfrom p1)               
                // back
                meshGeometry.Positions.Add(getPoint3Dfrom p1)
                meshGeometry.Positions.Add(getPoint3Dfrom p2)
                meshGeometry.Positions.Add(getPoint3Dfrom p3) ) points_1                
                
                Seq.iter (fun n -> 
                let p1,p2,p3 = n 
                // Front
                meshGeometry.Positions.Add(getPoint3Dfrom p3)
                meshGeometry.Positions.Add(getPoint3Dfrom p2)
                meshGeometry.Positions.Add(getPoint3Dfrom p1)               
                // Back
                meshGeometry.Positions.Add(getPoint3Dfrom p1)
                meshGeometry.Positions.Add(getPoint3Dfrom p2)
                meshGeometry.Positions.Add(getPoint3Dfrom p3)) points_2                                
            
            let model3D = System.Windows.Media.Media3D.GeometryModel3D(meshGeometry, Style.genericMaterial)
            Surface model3D |> Mesh 
        | _ -> DrawError ParameterError

    let doExpressionOperation opData :ExpressionOperationResult = 
        
        let func, expression_1, expression_2 = opData
        
        let checkResult expression = 
            match expression with
            | Expression.Symbol (Symbol.Error e) -> ExpressionError e
            | BinaryOp(Number(Real x),ToThePowerOf,Number(Real p)) -> Number(Real (x**p)) |> ExpressionSuccess
            | _ -> expression |> ExpressionSuccess
        
        match func with
        | Plus ->         expression_1 + expression_2 |> checkResult
        | Minus ->        expression_1 - expression_2 |> checkResult
        | Times ->        expression_1 * expression_2 |> checkResult
        | DividedBy 
        | Quotient ->     expression_1 / expression_2 |> checkResult
        | Inverse ->      expression_2 / expression_1 |> checkResult
        | ToThePowerOf -> expression_1** expression_2 |> checkResult
        | Root ->         expression_1** expression_2 |> checkResult
        | Sin ->             
            match expression_1 <> Expression.Zero && expression_2 <> Expression.Zero with
            | true -> UnaryOp(Sin,expression_1) |> checkResult 
            | false -> UnaryOp(Sin,expression_2) |> checkResult
        | Cos -> 
            match expression_1 <> Expression.Zero && expression_2 <> Expression.Zero with
            | true -> UnaryOp(Cos,expression_1) |> checkResult 
            | false -> UnaryOp(Cos,expression_2) |> checkResult
        | Tan -> 
            match expression_1 <> Expression.Zero && expression_2 <> Expression.Zero with
            | true -> UnaryOp(Tan,expression_1) |> checkResult 
            | false -> UnaryOp(Tan,expression_2) |> checkResult
        | Derivative -> Math.Pure.Change.Calculus.Differential.derivativeOf expression_1 expression_2 |> ExpressionType.simplifyRealExpression |> checkResult             
        | _ -> expression_1 |> checkResult

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
       
    let setExpressionToParenthetical :SetExpressionToParenthetical = 
        fun (expression, parenthetical) -> 
            match parenthetical with 
            | Some (Parenthetical(_,po,p)) -> Parenthetical(expression, po, p)
            | None -> Parenthetical(expression, None, None)

    let getExpressionFromParenthetical :GetExpressionFromParenthetical = 
        fun parenthetical ->
            match parenthetical with 
            | None -> Expression.Zero
            | Some p -> match p with Parenthetical (x, _, _) -> x   
            
    let getParentheticalFromCalculatorState :GetParentheticalFromCalculatorState = 
        fun state ->
            match state with 
            | EvaluatedState ev -> (Expression.Zero, ev.pendingFunction, None) |> Parenthetical
            | ParentheticalState p -> (Expression.Zero, p.pendingFunction, Some p.parenthetical) |> Parenthetical
            | ExpressionDigitAccumulatorState es
            | ExpressionDecimalAccumulatorState es -> 
                let lastParenthetical = (Expression.Zero, es.pendingFunction, es.parenthetical) |> Parenthetical |> Some
                (Expression.Zero, es.pendingFunction, lastParenthetical ) |> Parenthetical
            
            | EvaluatedState2DParametric (ev,_s) -> (Expression.Zero, ev.pendingFunction, None) |> Parenthetical
            | ParentheticalState2DParametric (p,_s) -> (Expression.Zero, p.pendingFunction, Some p.parenthetical) |> Parenthetical
            | ExpressionDigitAccumulatorState2DParametric (es,_s)
            | ExpressionDecimalAccumulatorState2DParametric (es,_s) -> 
                let lastParenthetical = (Expression.Zero, es.pendingFunction, es.parenthetical) |> Parenthetical |> Some
                (Expression.Zero, es.pendingFunction, lastParenthetical ) |> Parenthetical

            | EvaluatedState3DParametric (ev,_s) -> (Expression.Zero, ev.pendingFunction, None) |> Parenthetical
            | ParentheticalState3DParametric (p,_s) -> (Expression.Zero, p.pendingFunction, Some p.parenthetical) |> Parenthetical
            | ExpressionDigitAccumulatorState3DParametric (es,_s)
            | ExpressionDecimalAccumulatorState3DParametric (es,_s) -> 
                let lastParenthetical = (Expression.Zero, es.pendingFunction, es.parenthetical) |> Parenthetical |> Some
                (Expression.Zero, es.pendingFunction, lastParenthetical ) |> Parenthetical

            | _ -> (Expression.Zero, None, None) |> Parenthetical    

    let closeParenthetical :CloseParenthetical =
        fun parentheticalStateData ->             
            match parentheticalStateData.parenthetical with
            // 
            | Parenthetical (x, Some (e,po), Some p) -> 
                let result = doExpressionOperation (po,e,parentheticalStateData.evaluatedExpression)
                match result with
                | ExpressionError e ->  
                    {error=Error e;lastExpression = Expression.Zero} 
                    |> ExpressionErrorState
                | ExpressionSuccess ex ->
                    {parentheticalStateData with evaluatedExpression = ex; parenthetical = p} 
                    |> ParentheticalState  
            
            | Parenthetical (x, Some (e,po), None  ) ->  
                let result = doExpressionOperation (po,e,parentheticalStateData.evaluatedExpression)
                match result with
                | ExpressionError e ->  
                    {error=Error e;lastExpression = Expression.Zero} 
                    |> ExpressionErrorState
                | ExpressionSuccess ex ->
                    { evaluatedExpression = ex
                      pendingFunction = parentheticalStateData.pendingFunction
                      drawingOptions = parentheticalStateData.drawingOptions }
                    |> EvaluatedState
            //
            | Parenthetical (_, None, Some (Parenthetical(_,Some (e,po),None))) -> parentheticalStateData |> ParentheticalState
            | Parenthetical (x, None, Some (Parenthetical(_x,None       ,None  ))) -> parentheticalStateData |> ParentheticalState
            | Parenthetical (x, None, Some (Parenthetical(_x,None       ,Some p))) -> parentheticalStateData |> ParentheticalState
            | Parenthetical (_, None, Some (Parenthetical(_,Some (e,po),Some p)))  -> parentheticalStateData |> ParentheticalState 
            | Parenthetical (x, None,        None  ) -> parentheticalStateData |> ParentheticalState  

    let toggle2DParametricState :Toggle2DParametricState = 
        fun calculatorState ->
            match calculatorState with 
            | EvaluatedState2DParametric (sd, EvaluatedState s) -> (s, EvaluatedState sd) |> EvaluatedState2DParametric
            | EvaluatedState2DParametric (sd, ParentheticalState s) -> (s, EvaluatedState sd) |> ParentheticalState2DParametric
            | EvaluatedState2DParametric (sd, DrawState s) -> (s, EvaluatedState sd) |> DrawState2DParametric
            | EvaluatedState2DParametric (sd, ExpressionDigitAccumulatorState s) -> (s, EvaluatedState sd) |> ExpressionDigitAccumulatorState2DParametric
            | EvaluatedState2DParametric (sd, ExpressionDecimalAccumulatorState s) -> (s, EvaluatedState sd) |> ExpressionDecimalAccumulatorState2DParametric
            
            | ParentheticalState2DParametric (sd, EvaluatedState s) -> (s, ParentheticalState sd) |> EvaluatedState2DParametric
            | ParentheticalState2DParametric (sd, ParentheticalState s) -> (s, ParentheticalState sd) |> ParentheticalState2DParametric
            | ParentheticalState2DParametric (sd, DrawState s) -> (s, ParentheticalState sd) |> DrawState2DParametric
            | ParentheticalState2DParametric (sd, ExpressionDigitAccumulatorState s) -> (s, ParentheticalState sd) |> ExpressionDigitAccumulatorState2DParametric
            | ParentheticalState2DParametric (sd, ExpressionDecimalAccumulatorState s) -> (s, ParentheticalState sd) |> ExpressionDecimalAccumulatorState2DParametric
            
            | DrawState2DParametric (sd, EvaluatedState s) -> (s, DrawState sd) |> EvaluatedState2DParametric
            | DrawState2DParametric (sd, ParentheticalState s) -> (s, DrawState sd) |> ParentheticalState2DParametric
            | DrawState2DParametric (sd, DrawState s) -> (s, DrawState sd) |> DrawState2DParametric
            | DrawState2DParametric (sd, ExpressionDigitAccumulatorState s) -> (s, DrawState sd) |> ExpressionDigitAccumulatorState2DParametric
            | DrawState2DParametric (sd, ExpressionDecimalAccumulatorState s) -> (s, DrawState sd) |> ExpressionDecimalAccumulatorState2DParametric
            
            | ExpressionDigitAccumulatorState2DParametric (sd, EvaluatedState s) -> (s, ExpressionDigitAccumulatorState sd) |> EvaluatedState2DParametric
            | ExpressionDigitAccumulatorState2DParametric (sd, ParentheticalState s) -> (s, ExpressionDigitAccumulatorState sd) |> ParentheticalState2DParametric
            | ExpressionDigitAccumulatorState2DParametric (sd, DrawState s) -> (s, ExpressionDigitAccumulatorState sd) |> DrawState2DParametric
            | ExpressionDigitAccumulatorState2DParametric (sd, ExpressionDigitAccumulatorState s) -> (s, ExpressionDigitAccumulatorState sd) |> ExpressionDigitAccumulatorState2DParametric
            | ExpressionDigitAccumulatorState2DParametric (sd, ExpressionDecimalAccumulatorState s) -> (s, ExpressionDigitAccumulatorState sd) |> ExpressionDecimalAccumulatorState2DParametric          
            
            | ExpressionDecimalAccumulatorState2DParametric (sd, EvaluatedState s) -> (s, ExpressionDecimalAccumulatorState sd) |> EvaluatedState2DParametric
            | ExpressionDecimalAccumulatorState2DParametric (sd, ParentheticalState s) -> (s, ExpressionDecimalAccumulatorState sd) |> ParentheticalState2DParametric
            | ExpressionDecimalAccumulatorState2DParametric (sd, DrawState s) -> (s, ExpressionDecimalAccumulatorState sd) |> DrawState2DParametric
            | ExpressionDecimalAccumulatorState2DParametric (sd, ExpressionDigitAccumulatorState s) -> (s, ExpressionDecimalAccumulatorState sd) |> ExpressionDigitAccumulatorState2DParametric
            | ExpressionDecimalAccumulatorState2DParametric (sd, ExpressionDecimalAccumulatorState s) -> (s, ExpressionDecimalAccumulatorState sd) |> ExpressionDecimalAccumulatorState2DParametric
            | _ -> calculatorState

    let setActivate3DStateExpression :SetActivate3DStateExpression = 
        fun (calcState, activeExpression) -> 
            let update d3 exp =
                match d3.activeExpression with
                | X_ -> {d3 with x = exp}
                | Y_ -> {d3 with y = exp}
                | Z_ -> {d3 with z = exp}
            match calcState with 
            | EvaluatedState3DParametric (sd, d3) ->
                match activeExpression = d3.activeExpression with
                | true -> EvaluatedState3DParametric (sd, (update d3 sd.evaluatedExpression))
                | false -> 
                    let newD3 = {d3 with activeExpression = activeExpression}
                    let newSD = match newD3.activeExpression with
                                | X_ -> {sd with evaluatedExpression = newD3.x}
                                | Y_ -> {sd with evaluatedExpression = newD3.y}
                                | Z_ -> {sd with evaluatedExpression = newD3.z}
                    EvaluatedState3DParametric (newSD, (update newD3 sd.evaluatedExpression))            
            | _ -> calcState
    
    let createGraphServices () = {
        doDrawOperation =  doDrawOperation (0.1)
        doDraw2DParametricOperation =  doDraw2DParametricOperation
        doDraw3DParametricOperation =  doDraw3DParametricOperation
        doExpressionOperation =  doExpressionOperation
        accumulateSymbol =  accumulateSymbol
        accumulateZero = accumulateZero (15)
        accumulateNonZeroDigit = accumulateNonZeroDigit (10)
        accumulateSeparator = accumulateSeparator (15)
        getNumberFromAccumulator = getNumberFromAccumulator
        getDisplayFromExpression = getDisplayFromExpression
        getDisplayFromGraphState = getDisplayFromGraphState
        getDrawingOptionsFromState = getDrawingOptionsFromState
        setDrawingOptions = setDrawingOptions
        getDisplayFromPendingFunction = getDisplayFromPendingFunction
        getExpressionFromParenthetical = getExpressionFromParenthetical
        getParentheticalFromCalculatorState = getParentheticalFromCalculatorState
        getDim3DStateDataFromState = getDim3DStateDataFromState
        setExpressionToParenthetical = setExpressionToParenthetical
        setActivate3DStateExpression = setActivate3DStateExpression
        closeParenthetical = closeParenthetical 
        toggle2DParametricState = toggle2DParametricState
        }