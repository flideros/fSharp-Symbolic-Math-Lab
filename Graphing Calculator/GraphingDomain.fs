namespace GraphingCalculator

module GraphingDomain =
    
    type X = X of float
    type Y = Y of float    
    
    type Point = X * Y  //{x:Coordinate; y:Coordinate}
        
    type PathGeometry =
        | ArcSegment /// Creates an elliptical arc between two points.
        | BezierSegment /// Creates a cubic Bezier curve between two points.
        | LineSegment of Point/// Creates a line between two points.
        | PolyBezierSegment /// Creates a series of cubic Bezier curves.
        | PolyLineSegment /// Creates a series of lines.
        | PolyQuadraticBezierSegment /// Creates a series of quadratic Bezier curves.
        | QuadraticBezierSegment /// Creates a quadratic Bezier curve.

    type Trace = {startPoint:Point; traceSegments:PathGeometry list}

module GraphingImplementation =
    open System.Windows.Media
    open System.Windows.Shapes
    open GraphingDomain

    let t = {startPoint = (X 10.,Y 20.); traceSegments = [LineSegment (X 50.,Y 30.);LineSegment (X 100.,Y 200.);LineSegment (X 178.,Y 66.)]}
    
   
   
    let testTrace trace = 
        let convertPoint = fun point -> match point with | (X x,Y y) -> System.Windows.Point(x,y)
        let convertSegment = fun segment -> 
            match segment with 
            | LineSegment(X x,Y y) -> System.Windows.Media.LineSegment( System.Windows.Point(x,y),true )
            | _ -> System.Windows.Media.LineSegment( System.Windows.Point(0.,0.),true )
        let segments = List.map (fun segment -> convertSegment segment)trace.traceSegments
        let pg = PathGeometry()
        let pf = PathFigure()
        let path = Path(Stroke = Brushes.Black, StrokeThickness= 2.) //, Fill = Brushes.Blue)
        do  
            pf.StartPoint <-  convertPoint trace.startPoint 
            List.iter (fun s -> pf.Segments.Add(s)) segments
            pg.Figures.Add(pf)
            path.Data <- pg
        path