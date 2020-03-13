namespace Math.Presentation

open MathML
open System

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type Position = {x:float;y:float}
type Size = {scaleX :float; scaleY :float}

type Font = 
    {emSquare : float<MathML.em>
     typeFace : Typeface
     size : float<MathML.px>               
    }

type Glyph = 
    {path:Path;
     overHangBefore:float;
     extent:float;
     baseline:float;
     overHangAfter:float;
     height:float;
     leftBearing:float;
     rightBearing:float;
     width:float;
     string:string
     font:Font
     }

type ControlSequence =
    | ControlSymbol 
    | ControlWord

type Token =
    | Char of TokenElement * Glyph
    | ControlSequence of ControlSequence

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
    let STIX2Math_Typeface =           Typeface(STIX2Math_FontFamily,System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2MathItalic_Typeface =     Typeface(STIX2Math_FontFamily,System.Windows.FontStyles.Italic,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    
    
    let STIX2TextBold_Typeface =       Typeface(STIX2TextBold_FontFamily,      System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextBoldItalic_Typeface = Typeface(STIX2TextBoldItalic_FontFamily,System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextItalic_Typeface =     Typeface(STIX2TextItalic_FontFamily,    System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextRegular_Typeface =    Typeface(STIX2TextRegular_FontFamily,   System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    
    // Font Sizes
    let math100px = {emSquare = 1000.<MathML.em>; typeFace = STIX2Math_Typeface; size = 100.<MathML.px>}
    let math200px = {emSquare = 1000.<MathML.em>; typeFace = STIX2Math_Typeface; size = 200.<MathML.px>}
    let mathItalic200px = {emSquare = 1000.<MathML.em>; typeFace = STIX2MathItalic_Typeface; size = 200.<MathML.px>}

    
    let drawText t = 
        let drawingGroup = DrawingGroup()
        let drawingContext = drawingGroup.Open()
        let textLine = Text.format t
        do  textLine.Draw(drawingContext,Point(0.,0.),TextFormatting.InvertAxes.None)
            drawingContext.Close()            
        drawingGroup
    
    
    
    let formatTextWithFont = 
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
            let ft = formatTextWithFont t font //Text.format t //
            let p = Path(Stroke = Brushes.Black, Fill = Brushes.Black)
            // move glyph right when it hangs over the left side
            let x = match ft.OverhangLeading > 0. with | true -> 0. | false -> Math.Abs ft.OverhangLeading
            let geometry = ft.BuildGeometry(Point(x,0.)) //drawText t //
            do  p.Data <- geometry.GetFlattenedPathGeometry()            
            {path=p;
             leftBearing = ft.OverhangLeading; 
             extent = ft.Extent;
             overHangAfter = ft.OverhangAfter;
             overHangBefore = ft.Extent - ft.OverhangAfter - ft.Height;
             rightBearing = ft.OverhangTrailing; 
             baseline = ft.Baseline; 
             width = ft.Width; 
             height = ft.Height;
             font = font;
             string = t}

    let getHorizontalKern leftGlyph rightGlyph = 
        let pairWidth = formatTextWithFont (leftGlyph.string + rightGlyph.string) leftGlyph.font        
        pairWidth.Width - leftGlyph.width - rightGlyph.width

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

    let scaleGlyphBox (glyphBox :GlyphBox) (s:Size) = 
        do glyphBox.RenderTransform <- ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY)

    let placeGlyphBox (glyphBox :GlyphBox) (p:Position) = 
        do glyphBox.RenderTransform <- TranslateTransform(X = p.x, Y = p.y)

    let transformGlyphBox (glyphBox :GlyphBox) (p:Position) (s:Size) = 
        let tranforms = TransformGroup()
        do  tranforms.Children.Add(TranslateTransform(X = p.x, Y = p.y))
            tranforms.Children.Add(ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY))            
            glyphBox.RenderTransform <- tranforms

    (*Test Area*)
    type TestCanvas() as this  =  
        inherit UserControl()
        
        let textBlock =                    
                   let tb = TextBlock()
                   tb.Text <-  "\u212c"//"\U0001D49C"//"\ue0f2" 
                   tb.FontStyle <- FontStyles.Normal
                   tb.FontSize <- 100.
                   tb.FontFamily <- STIX2Math_FontFamily
                   tb.Typography.StylisticSet1 <- true
                   tb

        
        let testDrawing = 
            let d = DrawingBrush()
            do  d.Drawing <- drawText "\u212c"//"\U0001D49C"
            let rectangle = Rectangle (Width=200., Height=200., Stroke=Brushes.Black, StrokeThickness=0.)
            do  rectangle.Fill <- d
            let g = Grid()
            do  g.Children.Add(rectangle) |> ignore
            g

        let getGlyphBox glyph (p:Position) = 
            let x,y = p.x, p.y
            //let glyph = getGlyph text font
            let y' = y - (match glyph.overHangBefore > 0. with | true -> glyph.overHangBefore | false -> 0.)
            let gb = GlyphBox(glyph)
            do  gb.Width <- gb.Width * (glyph.font.size / 960.<MathML.px>)
                gb.Height <- gb.Height * (glyph.font.size / 960.<MathML.px>)
            do  gb.Loaded.AddHandler(
                    RoutedEventHandler(
                        fun _ _ -> 
                            transformGlyphBox 
                                gb // glyph box
                                {x = x; y = y'} // position
                                {scaleX = glyph.font.size / 960.<MathML.px>; scaleY = glyph.font.size / 960.<MathML.px>} )) // font size          
            gb
        
        let operator = getGlyph (getOperatorString mathematicalLeftFlattenedParenthesisPrefix) math200px
        let unicode =  getGlyph (getStringAtUnicode 0x221c) math200px
        
        let a = getGlyph MathematicalAlphanumericSymbols.LatinScript.Bold.A math200px
        let plus = getGlyph (getOperatorString plusSignInfix) math200px
        let two = getGlyph "2" math200px
        let closeParen = getGlyph (getOperatorString mathematicalRightFlattenedParenthesisPostfix) math200px

        let unicode_GlyphBox = getGlyphBox unicode {x=0.;y=0.}
        let operator_GlyphBox = getGlyphBox operator {x=unicode.width - (getHorizontalKern unicode operator);y=0.}
        let a_GlyphBox = getGlyphBox a {x = operator.width + unicode.width;y=0.}
        let plus_GlyphBox = getGlyphBox plus {x = operator.width + unicode.width + a.width + (getHorizontalKern a plus);y=0.}
        let two_GlyphBox =  getGlyphBox two  {x = operator.width + unicode.width + a.width + plus.width + (getHorizontalKern a plus) + (getHorizontalKern plus two);y=0.}
        let closeParen_GlyphBox =  getGlyphBox closeParen  {x = operator.width + unicode.width + a.width + plus.width + two.width + (getHorizontalKern a plus) + (getHorizontalKern plus two);y=0.}
        
        let line = 
            let g = Grid()
            let row0 = RowDefinition(Height = GridLength.Auto)
            let row1 = RowDefinition(Height = GridLength.Auto)
            do  g.RowDefinitions.Add(row0)
                g.RowDefinitions.Add(row1)
            g
        
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
            let handleValueChanged (s) = 
                line.RenderTransform <- 
                    let tranforms = TransformGroup()
                    do  tranforms.Children.Add(TranslateTransform(X = 100., Y = 100.))
                        tranforms.Children.Add(ScaleTransform(ScaleX = 5.0/s,ScaleY = 5.0/s))            
                    tranforms
                    //
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


        do  line.Children.Add(unicode_GlyphBox) |> ignore 
            line.Children.Add(operator_GlyphBox) |> ignore            
            line.Children.Add(a_GlyphBox) |> ignore
            line.Children.Add(plus_GlyphBox) |> ignore
            line.Children.Add(two_GlyphBox) |> ignore
            line.Children.Add(closeParen_GlyphBox) |> ignore

            line.RenderTransform <- TranslateTransform(X = 100., Y = 100.)
            //canvas.Children.Add(line) |> ignore
            //canvas.Children.Add(textBlock) |> ignore
            canvas.Children.Add(testDrawing) |> ignore
            this.Content <- screen_Grid