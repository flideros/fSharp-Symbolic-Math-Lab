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

    // Font Sizes
    let mathX00px = {emSquare = 1000.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = 400.<MathML.px>}
    let math200px = {emSquare = 1000.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = 200.<MathML.px>}
    
    let formatTextWithFont = fun t font   ->  Text.format t font.typeFace font.emSquare
    
    let getGlyph :GlyphBuilder = 
        fun t font -> 
            let drawText t = 
                let drawingGroup = DrawingGroup()
                let drawingContext = drawingGroup.Open()
                let textLine = Text.format t font.typeFace font.emSquare
                do  textLine.Draw(drawingContext,Point(0.,0.),TextFormatting.InvertAxes.None)
                    drawingContext.Close()             
                drawingGroup
            let rec createGeometry (dg :DrawingGroup) = 
                let items = dg.Children.Count
                let gg = GeometryGroup()
                let rec getDrawing i = 
                    match i > 0 with
                    | false -> gg
                    | true -> 
                        match dg.Children.Item(i-1) with
                        | :? GeometryDrawing as gd -> 
                            //do  gg.Children.Add(gd.Geometry)
                            getDrawing (i-1)
                        | :? GlyphRunDrawing as gr -> 
                            do  gg.Children.Add(gr.GlyphRun.BuildGeometry())
                            getDrawing (i-1)
                        | :? DrawingGroup as dg -> 
                            do  gg.Children.Add(createGeometry dg)
                            getDrawing (i-1)
                        | _ -> gg
                getDrawing items            
            let ft = Text.format t font.typeFace font.emSquare
            let p = Path(Stroke = Brushes.Black, Fill = Brushes.Black)            
            let geometry = drawText t 
            do  p.Data <- (createGeometry geometry).GetFlattenedPathGeometry()
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
        let typeSetPair = formatTextWithFont (leftGlyph.string + rightGlyph.string) leftGlyph.font        
        typeSetPair.Width - leftGlyph.width - rightGlyph.width

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

    let getGlyphBox glyph (p:Position) = 
            let x,y = p.x, p.y
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
    
    let makeLineFrom (glyphs:Glyph list) =
            let g = Grid()
            let row0 = RowDefinition(Height = GridLength.Auto)
            let row1 = RowDefinition(Height = GridLength.Auto)
            do  g.RowDefinitions.Add(row0)
                g.RowDefinitions.Add(row1)
            
            let kerns = 
            // compare adjacent glyph widths to the typeset width of the pair.
                let leftGlyphs = List.truncate (glyphs.Length-1) glyphs
                let rightGlyphs = glyphs.Tail
                let glyphPairs = List.zip leftGlyphs rightGlyphs
                let kerns = List.scan (fun acc (l,r) -> acc + (getHorizontalKern l r)) 0. glyphPairs
                kerns
            let positions = 
                let initialPosition = {x=0.;y=0.}
                
                let _positions = 
                    let p = List.scan (fun acc (x : Glyph) -> {x = (acc.x + x.width); y = 0.}) initialPosition glyphs
                    List.truncate glyphs.Length p
                
                let positionsWithKernApplied = 
                    List.mapi (fun i (p:Position) -> {x = p.x + kerns.[i]; y = p.y}) _positions

                positionsWithKernApplied
            let glyphBoxes = List.map2 (fun g p -> getGlyphBox g p) glyphs positions
            do  List.iter (fun x -> g.Children.Add(x) |> ignore) glyphBoxes
            g
    
    (*Test Area*)
    type TestCanvas() as this  =  
        inherit UserControl()
        
        let c0=  getGlyph (getOperatorString OperatorDictionary.cubeRootPrefix) mathX00px
        let c1 = getGlyph (getOperatorString mathematicalLeftFlattenedParenthesisPrefix) mathX00px
        let c2 = getGlyph MathematicalStandardizedVariants.emptySet mathX00px
        let c3 = getGlyph (MathematicalAlphanumericSymbols.LatinSerif.Normal.W) mathX00px
        let c4 = getGlyph (MathematicalAlphanumericSymbols.LatinSerif.Normal.A) mathX00px
        let c5 = getGlyph (getOperatorString mathematicalRightFlattenedParenthesisPostfix) mathX00px

        let glyphs = [c0;c1;c2;c3;c4;c5]
        //let glyphs = [c3;c4]  

        let line = makeLineFrom glyphs
        
        let kerns = 
            // compare adjacent glyph widths to the typeset width of the pair.
                let leftGlyphs = List.truncate (glyphs.Length-1) glyphs
                let rightGlyphs = glyphs.Tail
                let glyphPairs = List.zip leftGlyphs rightGlyphs
                let kerns = List.scan (fun acc (l,r) -> acc + (getHorizontalKern l r)) 0. glyphPairs
                kerns

        let textBlock =                    
            let tb = TextBlock()
            tb.Text <- kerns.ToString()//(MathematicalAlphanumericSymbols.LatinSerif.Normal.W) //"FA"//"\U0001D49C"//"\ue0f2" 
            tb.FontStyle <- FontStyles.Normal
            tb.FontSize <- 60.
            tb.FontFamily <- Text.STIX2Math_FontFamily
            tb.Typography.StylisticSet1 <- true
            tb

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

        do  line.RenderTransform <- TranslateTransform(X = 0., Y = 200.)
            canvas.Children.Add(line) |> ignore
            canvas.Children.Add(textBlock) |> ignore
            
            this.Content <- screen_Grid