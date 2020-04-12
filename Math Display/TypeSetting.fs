namespace Math.Presentation

open MathML
open System

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open MathematicalAlphanumericSymbols
open MathML

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
     string:string;
     font:Font;
     //mathElement:Element
     }
type GlyphBuilder = Font -> Element -> Glyph 
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

type RowData = {element:MathMLElement;inkWidth:float;leftBearing:float;rightBearing:float}
type MathRow = {grid:Grid;rowData:RowData}

module TypeSetting = 
    open MathML
    
    let basisSize = 100.<MathML.px> 

    // Font Sizes
    let mathX00px = {emSquare = 1000.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    let mathX0px = {emSquare = 700.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}        
    let textSizeFont = {emSquare = 1000.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    let scriptSizeFont = {emSquare = 700.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    let scriptScriptSizeFont = {emSquare = 500.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    
    // Format Text
    let formatTextWithFont = fun t font   ->  Text.format t font.typeFace font.emSquare    
    
    // Transfomations
    let scaleGlyphBox (glyphBox :GlyphBox) (s:Size) = 
        do glyphBox.RenderTransform <- ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY)
    let placeGlyphBox (glyphBox :GlyphBox) (p:Position) = 
        do glyphBox.RenderTransform <- TranslateTransform(X = p.x, Y = p.y)
    let transformGlyphBox (glyphBox :GlyphBox) (p:Position) (s:Size) = 
        let tranforms = TransformGroup()
        do  tranforms.Children.Add(TranslateTransform(X = p.x, Y = p.y))
            tranforms.Children.Add(ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY))            
            glyphBox.RenderTransform <- tranforms

    // Getters
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
    


    // Builders
    let makeGlyph :GlyphBuilder = 
            fun font elem -> 
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
                let text = 
                    let mathVariant = 
                        let variant = 
                            List.tryFind (fun x -> match x with
                                                   | MathVariant _ -> true 
                                                   | _ -> false) elem.attributes
                        match variant with
                        | Some (MathVariant v) -> v 
                        | _ -> Normal
                    MathematicalAlphanumericSymbolMap.stringToVariant elem.symbol mathVariant
                let ft = Text.format text font.typeFace font.emSquare
                let mathColor = 
                    let color = 
                        List.tryFind (fun x -> match x with
                                               | MathColor _ -> true 
                                               | _ -> false) elem.attributes
                    match color with
                    | Some (MathColor m) -> m 
                    | _ -> Brushes.Black :> Brush
                let p = Path(Stroke = mathColor, Fill = mathColor)            
                let geometry = drawText text                  
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
                 string = text}    
    let makeRowFrom (glyphs:Glyph list) =
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
        
        let c0=  makeGlyph textSizeFont (Element.build (GeneralLayout Mroot) [] [] (getOperatorString OperatorDictionary .cubeRootPrefix) )
        let c1 = makeGlyph textSizeFont (Element.build (Token Mo) [] [] (getOperatorString OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let c2 = makeGlyph textSizeFont (Element.build (Token Mi) [] [] (LatinSerif.Italic.C))
        let c3 = makeGlyph textSizeFont (Element.build (Token Mo) [MathColor Brushes.Blue] [] (getOperatorString OperatorDictionary.plusSignInfix))
        let c4 = makeGlyph textSizeFont (Element.build (Token Mi) [] [] ("x"))
        let c5 = makeGlyph textSizeFont (Element.build (Token Mo) [] [] (getOperatorString OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix))

        let glyphs1 = [c2;c3;c4]//c4;c2]//
        let line1 = makeRowFrom glyphs1 
        let line1_Width = List.fold (fun acc x -> x.width + acc) 0. glyphs1

        let s0=  makeGlyph scriptSizeFont (Element.build (GeneralLayout Mroot) [] [] (getOperatorString OperatorDictionary.cubeRootPrefix))
        let s1 = makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] (getOperatorString OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let s2 = makeGlyph scriptSizeFont (Element.build (Token Mi) [] [] (LatinSerif.Italic.x))
        let s3 = makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] (getOperatorString OperatorDictionary.plusSignInfix))
        let s4 = makeGlyph scriptSizeFont (Element.build (Token Mi) [] [] (LatinSerif.Italic.x))
        let s5 = makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] (getOperatorString OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix))

        let glyphs2 = [s2]//s0;s1;s2;s3;s4;s5]
        let line2 = makeRowFrom glyphs2
        
        
       
        let textBlock =                    
            let tb = TextBlock()
            tb.Text <- (line1_Width  + c4.rightBearing + c2.leftBearing).ToString() 
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
                line1.RenderTransform <- 
                    let tranforms = TransformGroup()
                    do  tranforms.Children.Add(TranslateTransform(X = 100., Y = 100.))
                        tranforms.Children.Add(ScaleTransform(ScaleX = 15.0/s,ScaleY = 15.0/s))            
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

        do  line1.RenderTransform <- TranslateTransform(X = 0., Y = 200.)
            line2.RenderTransform <- TranslateTransform(X = 201.2, Y = 27.8 - 36.)
            line2.SetValue(Grid.RowProperty,1)

            line1.Children.Add(line2) |> ignore

            canvas.Children.Add(line1) |> ignore
            //canvas.Children.Add(line2) |> ignore
            canvas.Children.Add(textBlock) |> ignore
            
            this.Content <- screen_Grid
