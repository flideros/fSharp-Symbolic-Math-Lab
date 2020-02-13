namespace Math.Presentation

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type Glyph = 
    {path:Path;
     leftBearing:float;
     rightBearing:float}
type GlyphBox (glyph) as glyphBox =
    inherit Border(BorderThickness=Thickness(1.5),BorderBrush=Brushes.Red)
    let g = Grid()
    let column0 = ColumnDefinition(Width = GridLength(glyph.leftBearing))
    let column1 = ColumnDefinition(Width = GridLength.Auto)
    let column2 = ColumnDefinition(Width = GridLength(glyph.rightBearing))
        
    do  glyph.path.SetValue(Grid.ColumnProperty,1)
        g.ColumnDefinitions.Add(column0)
        g.ColumnDefinitions.Add(column1)
        g.ColumnDefinitions.Add(column2)
        g.Children.Add(glyph.path) |> ignore
        glyphBox.SetValue(Grid.RowProperty,1)
        glyphBox.Child <- g

type FontMetrics = 
    {ascender:float; 
     descender:float; 
     capital:float; 
     height:float;
     xHeight:float;     
    }

type Font = 
    {emSquare : float<MathML.em>
     metrics : FontMetrics
     size : float<MathML.px>
    }

type TestCanvas(testGlyphs:Path list) as this  =  
    inherit UserControl()
    
    let sigma_GlyphBox = GlyphBox({path=testGlyphs.[0];rightBearing=30.;leftBearing=28.})
    let p_GlyphBox = GlyphBox({path=testGlyphs.[1];rightBearing=22.;leftBearing=35.})
    
    let em0_Grid =
        let g = Grid(Height = 1000.)
        let row0 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(235.))
            
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.Children.Add(sigma_GlyphBox) |> ignore
        g
    let em0_Border = 
        let b = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Green,Child=em0_Grid)
        do  b.SetValue(Grid.RowProperty,1)
        b

    let em_Grid =
        let g = Grid(Height = 1000.)
        let row0 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(18.))
            
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.Children.Add(p_GlyphBox) |> ignore
        g
    let em_Border = 
        let b = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Green,Child=em_Grid)
        do  b.SetValue(Grid.RowProperty,1)
        b
    
    let font_Grid = 
        let g = Grid(MaxHeight = 3900.)

        let sp = StackPanel(Orientation=Orientation.Horizontal)

        let row0 = RowDefinition(Height = GridLength(0., GridUnitType.Star))
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(250.))
            
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            sp.SetValue(Grid.RowProperty,1)
            sp.Children.Add(em0_Border) |> ignore
            sp.Children.Add(em_Border) |> ignore
            g.Children.Add(sp) |> ignore
        g
    let font_Border = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Black,Child=font_Grid)
    
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
            font_Border.RenderTransform <- ScaleTransform(ScaleX = 5./s,ScaleY=5./s)
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
 
    do  canvas.Children.Add(font_Border) |> ignore

        this.Content <- screen_Canvas