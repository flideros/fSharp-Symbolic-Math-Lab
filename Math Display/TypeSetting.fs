namespace Math.Presentation

open MathML
open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type Glyph = 
    {path:Path;
     overHangBefore:float;
     extent:float;
     baseline:float;
     overHangAfter:float;
     height:float;
     leftBearing:float;
     rightBearing:float;
     width:float
     }

type ControlSequence =
    | ControlSymbol 
    | ControlWord

type Token =
    | Char of TokenElement * Glyph
    | ControlSequence of ControlSequence

type Font = 
    {emSquare : float<MathML.em>
     typeFace : Typeface
     size : float<MathML.px>                                                                                                                                                                                                                                                                                                                                                                                                                                    
    }

type GlyphBuilder = string -> Font -> Glyph

type GlyphBox (glyph) as glyphBox =
    inherit Border(BorderThickness=Thickness(1.5),BorderBrush=Brushes.Red)
    let g = Grid()
    
    let bLine = 
        let p = Path(Stroke = Brushes.White, StrokeThickness = 4.)
        let pf = PathFigure(StartPoint = Point(0., glyph.baseline))        
        do  pf.Segments.Add( LineSegment( Point(glyph.width, glyph.baseline), true ))
        let pg = PathGeometry() 
        do  pg.Figures.Add(pf)
            p.Data <- pg
            p.SetValue(Grid.ColumnSpanProperty,3)
        p
    
    let column0 = 
        match glyph.leftBearing < 0. with
        | true -> ColumnDefinition(Width = GridLength(glyph.width))
        | false -> ColumnDefinition(Width = GridLength.Auto)    
    let column1 = 
        match glyph.rightBearing < 0. with
        | true -> ColumnDefinition(Width =  GridLength.Auto)
        | false -> ColumnDefinition(Width = GridLength(glyph.rightBearing))
      
    do  glyph.path.SetValue(Grid.ColumnProperty,0)
        glyph.path.SetValue(Grid.ColumnSpanProperty,2)
        g.ColumnDefinitions.Add(column0)
        g.ColumnDefinitions.Add(column1)
    
    let row0 = 
        RowDefinition(
            Height = GridLength(match glyph.overHangBefore > 0. with 
                                | false -> 0.
                                | true -> glyph.overHangBefore))
    let row1 = RowDefinition(Height = GridLength.Auto)
           
    do  glyph.path.SetValue(Grid.RowProperty,1)
        bLine.SetValue(Grid.RowProperty,1)
        g.RowDefinitions.Add(row0)
        g.RowDefinitions.Add(row1)
        
        g.Children.Add(glyph.path) |> ignore
        g.Children.Add(bLine) |> ignore 
        
           
    do  //glyph.path.
        glyphBox.SetValue(Grid.RowProperty,1)
        glyphBox.SetValue(Grid.RowSpanProperty,3)
        glyphBox.Child <- g

module TypeSetting = 
    open MathML
    open MathML.OperatorDictionary

    //  Font Families
    let STIX2Math_FontFamily =           FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"),            "./#STIX Two Math")
    let STIX2TextBold_FontFamily =       FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Bold"),       "./#STIX Two Text Bold")
    let STIX2TextBoldItalic_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-BoldItalic"), "./#STIX Two Text Bold Italic")
    let STIX2TextItalic_FontFamily =     FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Italic"),     "./#STIX Two Text Italic")
    let STIX2TextRegular_FontFamily =    FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Regular"),    "./#STIX Two Text")
    
    //  Typefaces
    let STIX2Math_Typeface =           Typeface(STIX2Math_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextBold_Typeface =       Typeface(STIX2TextBold_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextBoldItalic_Typeface = Typeface(STIX2TextBoldItalic_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextItalic_Typeface =     Typeface(STIX2TextItalic_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    let STIX2TextRegular_Typeface =    Typeface(STIX2TextRegular_FontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())
    
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
            // move glyph right when it hangs over the left side
            let x = match ft.OverhangLeading > 0. with |true -> 0. | false -> Math.Abs ft.OverhangLeading
            let geometry = ft.BuildGeometry(Point(x,0.)) 
            do  p.Data <- geometry.GetFlattenedPathGeometry()            
            {path=p;
             leftBearing = ft.OverhangLeading; 
             extent = ft.Extent;
             overHangAfter = ft.OverhangAfter;
             overHangBefore = ft.Extent - ft.OverhangAfter - ft.Height;
             rightBearing = ft.OverhangTrailing; 
             baseline = ft.Baseline; 
             width = ft.Width; 
             height = ft.Height}

    let getOperatorString (operator : Operator) = 
        let rec loop oc =
            match oc with
            | Unicode u -> (char u).ToString()
            | Char c -> c.ToString()
            | UnicodeArray a -> 
                let chars = Array.map (fun oc -> (char)(loop oc)) a
                new string (chars)
        loop operator.character

    let getStringAtUnicode u = (char u).ToString()

    let scaleGlyphBox (glyphBox :GlyphBox) (scaleX, scaleY) = do glyphBox.RenderTransform <- ScaleTransform(ScaleX = scaleX,ScaleY = scaleY)

    let placeGlyphBox (glyphBox :GlyphBox) (x, y) = do glyphBox.RenderTransform <- TranslateTransform(X = x,Y = y)

    let transformGlyphBox (glyphBox :GlyphBox) (x, y) (scaleX, scaleY) = 
        let tranforms = TransformGroup()
        do  tranforms.Children.Add(TranslateTransform(X = x,Y = y))
            tranforms.Children.Add(ScaleTransform(ScaleX = scaleX,ScaleY = scaleY))
            glyphBox.RenderTransform <- tranforms
        

    (*Test Area*)
    type TestCanvas(testGlyphs:Path list) as this  =  
        inherit UserControl()
        
        let font = 
            {emSquare = 1000.<MathML.em>;
             typeFace = STIX2Math_Typeface;
             size = 200.<MathML.px>
            }
        let getGlyphBox text font (position:float*float) = 
            let gb = GlyphBox(getGlyph text font)
            do  gb.Width <- gb.Width * (font.size / 960.<MathML.px>)
                gb.Height <- gb.Height * (font.size / 960.<MathML.px>)
            do  gb.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> transformGlyphBox gb position (font.size / 960.<MathML.px>, font.size / 960.<MathML.px>)))
          
            gb

        let operator_GlyphBox = getGlyphBox (getOperatorString mathematicalRightFlattenedParenthesisPostfix) font (1000., 1000.) 
        
        let unicode_GlyphBox = getGlyphBox (getStringAtUnicode 0x20db) font (500., 300.)
  
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
                ()//operator_GlyphBox.RenderTransform <- ScaleTransform(ScaleX = 50./s,ScaleY = 50./s)
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
 
        do  canvas.Children.Add(operator_GlyphBox) |> ignore 
            canvas.Children.Add(unicode_GlyphBox) |> ignore

            this.Content <- screen_Grid