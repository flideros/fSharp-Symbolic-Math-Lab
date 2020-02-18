namespace Math.Presentation

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type Font = 
    {emSquare : float<MathML.em>
     typeFace : Typeface
     size : float<MathML.px>
    }

type Glyph = 
    {path:Path;
     leftBearing:float;
     rightBearing:float}
type GlyphBuilder = string -> Font -> Glyph


module TypeSetting = 
    
    //  Font Families
    let STIX2Math_FontFamily =           FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"),            "./#STIX Two Math")
    let STIX2TextBold_FontFamily =       FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Bold"),       "./#STIX Two Text Bold")
    let STIX2TextBoldItalic_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-BoldItalic"), "./#STIX Two Text Bold Italic")
    let STIX2TextItalic_FontFamily =     FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Italic"),     "./#STIX Two Text Italic")
    let STIX2TextRegular_FontFamily =    FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Regular"),    "./#STIX Two Text")
    
    //  Typefaces
    let STIX2Math_Typeface =           Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextBold_Typeface =       Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextBoldItalic_Typeface = Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextItalic_Typeface =     Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextRegular_Typeface =    Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    
    let formatText = 
        fun t font   -> 
            FormattedText(
                textToFormat = t,
                culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                flowDirection = FlowDirection.LeftToRight,
                typeface = font.typeFace,
                emSize = float font.emSquare,
                foreground = Brushes.Black,
                pixelsPerDip = 1.25)

    let getGlyph :GlyphBuilder = 
        fun t font -> 
            let ft = formatText t font 
            let p = Path(Stroke = Brushes.Black, Fill = Brushes.Black)
            let geometry = ft.BuildGeometry(Point(0.,-ft.Baseline))//1.*(formattedText.Extent-formattedText.Baseline))) 
            do  p.Data <- geometry.GetFlattenedPathGeometry()
            {path=p;leftBearing=(System.Math.Abs(ft.OverhangLeading));rightBearing=(ft.OverhangTrailing)}

    // Type setting controls
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
            glyphBox.SetValue(Grid.RowSpanProperty,3)
            glyphBox.Child <- g
    
    type TestCanvas(testGlyphs:Path list) as this  =  
        inherit UserControl()       
        
        let font = 
            {emSquare = 1000.<MathML.em>;
             typeFace = STIX2Math_Typeface;
             size = 100.<MathML.px>
            }
        let getGlyphFromFont text = GlyphBox(getGlyph text font)

        let sigma_GlyphBox = GlyphBox({path=testGlyphs.[0];rightBearing=30.;leftBearing=28.})
        let p_GlyphBox = GlyphBox({path=testGlyphs.[1];rightBearing=22.;leftBearing=35.})
    
        let em0_Grid =
            let g = Grid(Height = 1000.)
            let row0 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            let row1 = RowDefinition(Height = GridLength.Auto)
            let row2 = RowDefinition(Height = GridLength.Auto)
            
            do  g.RowDefinitions.Add(row0)
                g.RowDefinitions.Add(row1)
                g.RowDefinitions.Add(row2)
                g.Children.Add(getGlyphFromFont "Testing 1 2") |> ignore
            g
        let em0_Border = 
            let b = Border(BorderThickness=Thickness(1.),BorderBrush=Brushes.Green,Child=em0_Grid)
            do  b.SetValue(Grid.RowProperty,1)
            b

        let em_Grid =
            let g = Grid(Height = 1000.)
            let row0 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            let row1 = RowDefinition(Height = GridLength.Auto)
            let row2 = RowDefinition(Height = GridLength.Auto)
            
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
    
        let canvas = Canvas(ClipToBounds = true)
    
    
        let scale_Slider =
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
        let scaleSlider_Grid =
            let g = Grid()
            do 
                g.SetValue(DockPanel.DockProperty,Dock.Top)
                g.Children.Add(scale_Slider) |> ignore
            g
    
        let canvas_DockPanel =
            let d = DockPanel()
            do d.Children.Add(scaleSlider_Grid) |> ignore
            do d.Children.Add(canvas) |> ignore
            d
        let screen_Grid =
            let g = Grid()
            do g.SetValue(Grid.RowProperty, 1)        
            do g.Children.Add(canvas_DockPanel) |> ignore        
            g
 
        do  canvas.Children.Add(font_Border) |> ignore //font_Border) |> ignore

            this.Content <- screen_Grid