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
        | FailedToCreateTrace
        | LazyCoder
        | SomeOtherErrorsAsIThinOfThem
       
    type DrawOperationResult =  
        | Trace of Trace 
        | DrawError of DrawError

    type ExpressionInput =
        | Symbol of Symbol
        | Function of Function
        

    // data associated with each state    
    
    type ExpressionStateData = {expression:Expression; pending:Expression; digits:ConventionalDomain.DigitAccumulator}
    type DrawStateData =       {expression:Expression; trace:Trace}    
    type ErrorStateData =      {error:DrawError}

    type CalculatorInput =
        | ExpressionInput of ExpressionInput
        | CalcInput of ConventionalDomain.CalculatorInput
        | Draw
        | Enter

    // States
    type CalculatorState =         
        | DrawState of DrawStateData
        | ExpressionDigitAccumulator of ExpressionStateData
        | ExpressionDecimalAccumulatorState of ExpressionStateData
        | DrawErrorState of ErrorStateData
      
    type Calculate = CalculatorInput * CalculatorState -> CalculatorState

    // Services used by the calculator itself
    type DoDrawOperation = Expression -> DrawOperationResult
    type GetNumberFromAccumulator = ExpressionStateData -> NumberType
    type GetDisplayFromGraphState = CalculatorState -> string
    type RpnServices = {        
        doDrawOperation :DoDrawOperation
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        getNumberFromAccumulator :GetNumberFromAccumulator
        GetDisplayFromGraphState :GetDisplayFromGraphState
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

module GraphingImplementation =    
    open GraphingDomain

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

    let doDrawOperation services expression =        
        let result = services.doDrawOperation expression 
        match result with
        | Trace t -> DrawState {expression = expression; trace = t}
        | DrawError x -> DrawErrorState {error = x}  

    let handleGrahphState services stateData input =
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
                | MemorySubtract -> {error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {error = LazyCoder}
            | Digit d -> {error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {error = LazyCoder}
            | Function f -> {error = LazyCoder}
        | Draw -> {error = LazyCoder}
        | Enter -> {error = LazyCoder}

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
                | MemorySubtract -> {error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {error = LazyCoder}
            | Digit d -> {error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {error = LazyCoder}
            | Function f -> {error = LazyCoder}
        | Draw -> {error = LazyCoder}
        | Enter -> {error = LazyCoder}

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
                | MemorySubtract -> {error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {error = LazyCoder}
            | Digit d -> {error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {error = LazyCoder}
            | Function f -> {error = LazyCoder}
        | Draw -> {error = LazyCoder}
        | Enter -> {error = LazyCoder}

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
                | MemorySubtract -> {error = LazyCoder}         
            | CalculatorInput.Zero                    
            | DecimalSeparator 
            | CalculatorInput.Equals 
            | Clear 
            | ClearEntry            
            | MemoryRecall
            | MemoryClear
            | MemoryStore 
            | CalculatorInput.Equals
            | Back -> {error = LazyCoder}
            | Digit d -> {error = LazyCoder}
        | ExpressionInput input -> 
            match input with 
            | Symbol s -> {error = LazyCoder}
            | Function f -> {error = LazyCoder}
        | Draw -> {error = LazyCoder}
        | Enter -> {error = LazyCoder}



             