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
     lSpace:float;
     rSpace:float
     }
type GlyphRow = 
    {grid:Grid;
     inkWidth:float;
     leftBearing:float;
     rightBearing:float}

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
            fun font el -> 
                
                let symbol = 
                    match el.operator with 
                    | Some o -> getOperatorString o
                    | Option.None -> el.symbol
                let lSpace = 
                    match el.operator with 
                    | Some o -> Operator.getValueFromLength font.emSquare o.lspace
                    | Option.None -> 0.
                let rSpace = 
                    match el.operator with 
                    | Some o -> Operator.getValueFromLength font.emSquare o.rspace
                    | Option.None -> 0.
                
                let text = 
                    let variant = 
                        List.tryFind (fun x -> 
                            match x with
                            | MathVariant _ -> true 
                            | _ -> false) el.attributes
                    match variant with
                    | Some (MathVariant v) -> v 
                    | _ -> Normal
                    |> MathematicalAlphanumericSymbolMap.stringToVariant symbol
                let formattedText = Text.format text font.typeFace font.emSquare

                let mathColor = 
                    let color = 
                        List.tryFind (fun x -> match x with
                                               | MathColor _ -> true 
                                               | _ -> false) el.attributes
                    match color with
                    | Some (MathColor m) -> m 
                    | _ -> Brushes.Black :> Brush
                let p = Path(Stroke = mathColor, Fill = mathColor)            
                
                let geometry = 
                    let drawingGroup = DrawingGroup()
                    let drawingContext = drawingGroup.Open()
                    let textLine = Text.format text font.typeFace font.emSquare
                    do  textLine.Draw(drawingContext,Point(0.,0.),TextFormatting.InvertAxes.None)
                        drawingContext.Close()             
                    drawingGroup
                let rec draw (dg :DrawingGroup) = 
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
                                do  gg.Children.Add(draw dg)
                                getDrawing (i-1)
                            | _ -> gg
                    getDrawing items            
                
                do  p.Data <- (draw geometry).GetFlattenedPathGeometry()
                {path=p;
                 leftBearing = formattedText.OverhangLeading; 
                 extent = formattedText.Extent;
                 overHangAfter = formattedText.OverhangAfter;
                 overHangBefore = formattedText.Extent - formattedText.OverhangAfter - formattedText.Height;
                 rightBearing = formattedText.OverhangTrailing; 
                 baseline = formattedText.Baseline; 
                 width = formattedText.Width; 
                 height = formattedText.Height;
                 font = font;
                 string = text
                 lSpace = lSpace;
                 rSpace = rSpace}
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
        
        let mathSpaces = 
            // Apply operator lSpace and rSpace.
            let leftGlyphs = List.truncate (glyphs.Length-1) glyphs
            let rightGlyphs = glyphs.Tail
            let glyphPairs = List.zip leftGlyphs rightGlyphs
            let spaces = List.scan (fun acc (l,r) -> acc + l.rSpace + r.lSpace - l.rightBearing - r.leftBearing) 0. glyphPairs
            // Specify space based on em size
            match glyphs.Head.font.emSquare = 1000.<MathML.em> with
            | true -> spaces
            | false -> List.map (fun x -> 0. * x) spaces // In this case, do not apply to script sizes

        let positions = 
            let initialPosition = {x=0.;y=0.}
            // positions = 
            let p = List.scan (fun acc (x : Glyph) -> {x = (acc.x + x.width); y = 0.}) initialPosition glyphs
            List.truncate glyphs.Length p  
            // apply kerns
            |> List.mapi (fun i (p:Position) -> {x = p.x + kerns.[i]; y = p.y})
            // apply math spacees
            |> List.mapi (fun i (p:Position) -> {x = p.x + mathSpaces.[i]; y = p.y})

        let glyphBoxes = List.map2 (fun g p -> getGlyphBox g p) glyphs positions
        do  List.iter (fun x -> g.Children.Add(x) |> ignore) glyphBoxes
        g
    
    (*Test Area*)
    type TestCanvas() as this  =  
        inherit UserControl()

        let mathematicalRightFlattenedParenthesisPostfix = OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix
        let invisibleTimesInfix = OperatorDictionary.invisibleTimesInfix

        let c0=  makeGlyph textSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.cubeRootPrefix))
        let c1 = makeGlyph textSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let c2 = makeGlyph textSizeFont (Element.build (Token Mn) [] [] "4" Option.None)
        let c3 = makeGlyph textSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.plusSignInfix))
        let c4 = makeGlyph textSizeFont (Element.build (Token Mi) [] [] "x" Option.None)
        let c5 = makeGlyph textSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix))

        let glyphs1 = [c2;c3;c4]//c4;c2]//
        let line1 = makeRowFrom glyphs1 
        let line1_Width = List.fold (fun acc x -> x.width + acc) 0. glyphs1

        let s0=  makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.cubeRootPrefix)) 
        let s1 = makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let s2 = makeGlyph scriptSizeFont (Element.build (Token Mi) [] [] "z" Option.None)
        let s3 = makeGlyph scriptSizeFont (Element.build (Token Mo) [MathColor Brushes.Crimson] [] "" (Some OperatorDictionary.plusSignInfix))
        let s4 = makeGlyph scriptSizeFont (Element.build (Token Mn) [] [] "32" Option.None)
        let s5 = makeGlyph scriptSizeFont (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix))

        let glyphs2 = [s1;s2;s3;s4;s5]//s2]//s0;
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
            line2.RenderTransform <- 
                TranslateTransform(
                    X = (line1_Width  
                        //+ c2.leftBearing
                        - c2.rightBearing                        
                        - c4.leftBearing 
                        //+ c4.rightBearing 
                        + MathPositioningConstants.spaceAfterScript 
                        + 444. 
                        ) *0.1, Y = 27.8 - 36.)
            line2.SetValue(Grid.RowProperty,1)

            line1.Children.Add(line2) |> ignore

            canvas.Children.Add(line1) |> ignore
            //canvas.Children.Add(line2) |> ignore
            //canvas.Children.Add(textBlock) |> ignore
            
            this.Content <- screen_Grid
