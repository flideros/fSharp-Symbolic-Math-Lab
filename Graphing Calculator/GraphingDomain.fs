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
        | UnknownError
        | SomeOtherErrorsAsIThinOfThem
       
    type DrawOperationResult =  
        | Trace of Trace 
        | DrawError of DrawError

    type ExpressionInput =
        | Symbol of Symbol
        | Number of NumberType
        | Function of Function

    // data associated with each state    
    type ExpressionStateData = {expression:Expression; pending:Expression; digits:ConventionalDomain.DigitAccumulator}
    type DrawStateData =       {expression:Expression; trace:Trace}    
    type ErrorStateData =      {error:DrawError}

    type CalculatorInput =
        | ExpressionOp of ExpressionInput
        | ExpressionInput of ConventionalDomain.CalculatorInput
        | Draw

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
            match (variables e).Length = 1 ||
                  e.isNumber with
            | true -> Symbol (variables e).Head
            //I need to throw an error here, but I'll get back to this later.
            | false -> Symbol (Variable "x") 
        substitute ( x, Number n ) e
