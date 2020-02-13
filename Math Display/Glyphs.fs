namespace Math.Presentation
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

module Glyphs =

    let testGlyph = 
        let p = Path(Stroke = Brushes.Black, StrokeThickness = 1.,Fill=Brushes.Black)
        let pf = PathFigure(StartPoint = Point(487., 516.))
        do  pf.Segments.Add( LineSegment( Point(140., 945.), true ))
            pf.Segments.Add( LineSegment( Point(565., 945.) , true ))
            pf.Segments.Add( BezierSegment(Point(704., 945.), Point(704., 945.), Point(797., 916.), true))
            pf.Segments.Add( BezierSegment(Point(905., 879.), Point(905., 879.), Point(956., 791.), true))
            pf.Segments.Add( BezierSegment(Point(967., 772.), Point(967., 772.), Point(974., 751.), true))
            pf.Segments.Add( LineSegment( Point(999., 751.), true ))
            pf.Segments.Add( LineSegment( Point(913., 1000.), true ))
            pf.Segments.Add( LineSegment( Point(84., 1000.), true ))
            pf.Segments.Add( BezierSegment(Point(59., 1000.), Point(59., 1000.), Point(57., 989.), true))
            pf.Segments.Add( BezierSegment(Point(57., 984.), Point(57., 984.), Point(66., 973.), true))
            pf.Segments.Add( LineSegment( Point(421., 534.),true))
            pf.Segments.Add( LineSegment( Point(56., 32.),true))
            pf.Segments.Add( BezierSegment(Point(56., 06.), Point(56., 6.), Point(62., 2.), true))
            pf.Segments.Add( BezierSegment(Point(67., 0.0), Point(67., 0.0), Point(84., 0.0), true))
            pf.Segments.Add( LineSegment( Point(913., 0.0),true))
            pf.Segments.Add( LineSegment( Point(999., 234.),true))
            pf.Segments.Add( LineSegment( Point(974., 234.),true))
            pf.Segments.Add( BezierSegment(Point(903., 44.), Point(903., 44.), Point(583., 40.), true))
            pf.Segments.Add( LineSegment( Point(560., 40.0),true))
            pf.Segments.Add( LineSegment( Point(164., 40.0),true))
            pf.Segments.Add( LineSegment( Point(489., 486.0),true))
            pf.Segments.Add( BezierSegment(Point(496., 496.), Point(496., 496.), Point(496., 500.), true))
            pf.Segments.Add( BezierSegment(Point(496., 504.), Point(496., 504.), Point(487., 516.), true))
                
        let pg = PathGeometry() 
        do  pg.Figures.Add(pf)
            p.Data <- pg
        p

    let sigma = 
        let p = Path(Stroke = Brushes.Black, StrokeThickness = 1.,Fill=Brushes.Black,Stretch = Stretch.Uniform)
        let pf = PathFigure(StartPoint = Point  (576., -204.))
        do  pf.Segments.Add( LineSegment(Point  (543., 0.), true ))
            pf.Segments.Add( LineSegment(Point  (30., 0.), true ))
            pf.Segments.Add( LineSegment(Point  (30., -31.), true ))
            pf.Segments.Add( LineSegment(Point  (283., -335.), true ))
            pf.Segments.Add( LineSegment(Point  (31., -633.), true ))
            pf.Segments.Add( LineSegment(Point  (31., -657.), true ))
            pf.Segments.Add( LineSegment(Point  (538., -657.), true ))
            pf.Segments.Add( LineSegment(Point  (544., -488.), true ))
            pf.Segments.Add( LineSegment(Point  (511., -488.), true ))
            pf.Segments.Add( BezierSegment(Point(495., -598.),Point(460., -612.),Point(365., -612.), true ))
            pf.Segments.Add( LineSegment(Point  (173., -612.), true ))
            pf.Segments.Add( LineSegment(Point  (173., -605.), true ))
            pf.Segments.Add( LineSegment(Point  (362., -375.), true ))
            pf.Segments.Add( LineSegment(Point  (362., -357.), true ))
            pf.Segments.Add( LineSegment(Point  (138., -85.), true ))
            pf.Segments.Add( LineSegment(Point  (138., -80.), true ))
            pf.Segments.Add( LineSegment(Point  (422., -80.), true ))
            pf.Segments.Add( BezierSegment(Point(498., -80.),Point(523., -126.),Point(543., -204.), true ))
            pf.Segments.Add( LineSegment(Point  (576., -204.), true ))
            
        let pg = PathGeometry() 
        do  pg.Figures.Add(pf)
            p.Data <- pg            
        p

    let p = 
        let p = Path(Stroke = Brushes.Black, StrokeThickness = 1.,Fill=Brushes.Black,Stretch = Stretch.Uniform)
        let pf = PathFigure(StartPoint = Point  (176., -482.))
        do  pf.Segments.Add( LineSegment(Point  (176., -398.), true ))
            pf.Segments.Add( LineSegment(Point  (179., -398.), true ))
            pf.Segments.Add( BezierSegment(Point(227., -454.),Point(275., -485.),Point(334., -485.), true ))
            pf.Segments.Add( BezierSegment(Point(433., -485.),Point(502., -392.),Point(502., -261.), true ))
            pf.Segments.Add( BezierSegment(Point(502., -91.),Point(403., 10.),Point(283., 10.), true ))
            pf.Segments.Add( BezierSegment(Point(245., 10.),Point(211., 2.),Point(182., -18.), true ))
            pf.Segments.Add( LineSegment(Point  (178., -18.), true ))
            pf.Segments.Add( LineSegment(Point  (178., 113.), true ))
            pf.Segments.Add( BezierSegment(Point(178., 178.),Point(197., 191.),Point(275., 191.), true ))
            pf.Segments.Add( LineSegment(Point  (275., 220.), true ))
            pf.Segments.Add( LineSegment(Point  (22., 220.), true ))
            pf.Segments.Add( LineSegment(Point  (22., 192.), true ))
            pf.Segments.Add( BezierSegment(Point(85., 192.),Point(95., 175.),Point(95., 119.), true ))
            pf.Segments.Add( LineSegment(Point  (95., -376.), true ))
            pf.Segments.Add( BezierSegment(Point(95., -414.),Point(90., -427.),Point(57., -427.), true ))
            pf.Segments.Add( BezierSegment(Point(44., -427.),Point(25., -425.),Point(25., -425.), true ))
            pf.Segments.Add( LineSegment(Point  (25., -454.), true ))
            pf.Segments.Add( LineSegment(Point  (151., -482.), true ))
            pf.Segments.Add( LineSegment(Point  (176., -482.), true ))
            
            pf.Segments.Add( LineSegment(Point  (178., -356.), false ))
                        
            pf.Segments.Add( LineSegment(Point  (178., -95.), true ))
            pf.Segments.Add( BezierSegment(Point(197., -74.),Point(232., -29.),Point(288., -29.), true ))
            pf.Segments.Add( BezierSegment(Point(358., -29.),Point(411., -94.),Point(411., -233.), true ))
            pf.Segments.Add( BezierSegment(Point(411., -359.),Point(367., -417.),Point(293., -417.), true ))
            pf.Segments.Add( BezierSegment(Point(240., -417.),Point(197., -381.),Point(178., -356.), true ))
         
        let pg = PathGeometry() 
        do  pg.Figures.Add(pf)
            p.Data <- pg            
        p