namespace MathML

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type TestCanvas(testGlyph:Path) as this  =  
    inherit UserControl()
    
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
            testGlyph.RenderTransform <- ScaleTransform(ScaleX = 5./s,ScaleY=5./s)
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
 
    do  canvas.Children.Add(testGlyph) |> ignore

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
