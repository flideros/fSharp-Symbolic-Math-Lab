namespace Math.Presentation
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media


type Glyph = Path
type TestCanvas(testGlyph:Glyph) as this  =  
    inherit UserControl()
    
    do  testGlyph.SetValue(Grid.ColumnProperty,1)
        
    let glyph_Grid = 
        let g = Grid()//Width = 546., Height = 657.)
            
        let column0 = ColumnDefinition(Width = GridLength(30.))
        let column1 = ColumnDefinition(Width = GridLength(546.))
        let column2 = ColumnDefinition(Width = GridLength(28.))
            
        do  g.ColumnDefinitions.Add(column0)
            g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
        do  g.Children.Add(testGlyph) |> ignore
        g
    let glyph_Border = 
        let b = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Red,Child=glyph_Grid)
        b.SetValue(Grid.RowProperty,1)
        b
    
    let em_Grid =
        let g = Grid(Height = 1000.)
        let row0 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(238.))
            
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.Children.Add(glyph_Border) |> ignore
        g
    let em_Border = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Green,Child=em_Grid)

    let canvasGridLine_Slider =
        let s = 
            Slider(
                Margin = Thickness(left = 0., top = 20., right = 0., bottom = 0.),
                Minimum = 5.,
                Maximum = 100.,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                TickFrequency = 5.,
                IsSnapToTickEnabled = true,
                IsEnabled = true)        
        do s.SetValue(Grid.RowProperty, 0)
        let handleValueChanged (s)= 
            em_Border.RenderTransform <- ScaleTransform(ScaleX = 5./s,ScaleY=5./s)
        s.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ e -> handleValueChanged (e.NewValue)))

        s
    let canvas_DockPanel_Grid =
        let g = Grid()
        do 
            g.SetValue(DockPanel.DockProperty,Dock.Top)
            g.Children.Add(canvasGridLine_Slider) |> ignore
        g    
    let canvas = Canvas(ClipToBounds = true)

    let canvas_DockPanel =
        let d = DockPanel()
        do d.Children.Add(canvas_DockPanel_Grid) |> ignore
        do d.Children.Add(canvas) |> ignore
        d
    let screen_Canvas =
        let g = Grid()
        do g.SetValue(Grid.RowProperty, 1)        
        do g.Children.Add(canvas_DockPanel) |> ignore
        
        g
 
    do  canvas.Children.Add(em_Border) |> ignore

        this.Content <- screen_Canvas

module Glyph =

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