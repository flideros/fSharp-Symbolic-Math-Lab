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
     rowWidth:float;
     rowHeight:float;
     leftBearing:float;
     rightBearing:float
     }

type GlyphBuilder = Font -> Element -> Glyph 
type GlyphBox (glyph) as glyphBox =
    inherit Border(BorderThickness=Thickness(1.5),BorderBrush=Brushes.Red)
    let g = Grid()
    
    let mLine = 
        let p = Path(Stroke = Brushes.Cyan, StrokeThickness = 4.)

        let pf = PathFigure(StartPoint = Point(0., glyph.baseline - (MathPositioningConstants.axisHeight * (glyph.font.emSquare / 1000.<MathML.em>))))        
        do  pf.Segments.Add( LineSegment( Point(glyph.width, glyph.baseline - (MathPositioningConstants.axisHeight * (glyph.font.emSquare / 1000.<MathML.em>))), true ))
        let pg = PathGeometry() 
        do  pg.Figures.Add(pf)
            p.Data <- pg
            p.SetValue(Grid.ColumnSpanProperty,3)
        p
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
        mLine.SetValue(Grid.RowProperty,1)
        bLine.SetValue(Grid.RowProperty,1)
        g.RowDefinitions.Add(row0)
        g.RowDefinitions.Add(row1)
        
        g.Children.Add(glyph.path) |> ignore
        g.Children.Add(mLine) |> ignore
        g.Children.Add(bLine) |> ignore 
        
           
    do  //glyph.path.
        glyphBox.SetValue(Grid.RowProperty,1)
        glyphBox.SetValue(Grid.RowSpanProperty,3)
        glyphBox.Child <- g

////(**)

type TypeObject =
    | Glyph of Glyph
    | GlyphRow of GlyphRow

module TypeSetting = 
    open MathML
    // Constants
    let basisSize = 100.<MathML.px>
    let basisEmSquare = 10.<MathML.em>
    let textSizeScaleFactor = float (basisEmSquare / basisSize)
    let textBaseline = 762.

    //  Font Sizes
    let textSizeFont = {emSquare = 1000.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    let scriptSizeFont = {emSquare = 700.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    let scriptScriptSizeFont = {emSquare = 500.<MathML.em>; typeFace = Text.STIX2Math_Typeface; size = basisSize}
    
    //  Checks
    let areAllGlyphs (tList:TypeObject list) = 
        List.forall (fun x -> 
            match x with
            | Glyph _ -> true
            | _ -> false) tList
    
    //  Format Text
    let formatTextWithFont = fun t font -> Text.format t font.typeFace font.emSquare    
    
    //  Transfomations
    let scaleGlyphBox (glyphBox :GlyphBox) (s:Size) = 
        do glyphBox.RenderTransform <- ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY)
    let placeGlyphBox (glyphBox :GlyphBox) (p:Position) = 
        do glyphBox.RenderTransform <- TranslateTransform(X = p.x, Y = p.y)
    let transformGlyphBox (glyphBox :GlyphBox) (p:Position) (s:Size) = 
        let tranforms = TransformGroup()
        do  tranforms.Children.Add(TranslateTransform(X = p.x, Y = p.y))
            tranforms.Children.Add(ScaleTransform(ScaleX = s.scaleX,ScaleY = s.scaleY))            
            glyphBox.RenderTransform <- tranforms
    
    //  Getters
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
    let getFontFromTokenElement (el:Element) = 
        let mathSize = 
            List.tryFind (fun x -> 
                match x with
                | MathSize _ -> true 
                | _ -> false) el.attributes
        match mathSize with 
        | Some (MathSize (EM s)) -> {emSquare = s * 1000.; typeFace = Text.STIX2Math_Typeface; size = basisSize}
        | _ -> textSizeFont
    let getGridFromTypeObject t = match t with | GlyphRow gr -> gr.grid | Glyph g -> new Grid() 
    let getWidthFromTypeObject t = match t with | GlyphRow gr -> gr.rowWidth | Glyph g -> g.width
    let getHeightFromTypeObject t = match t with | GlyphRow gr -> gr.rowHeight | Glyph g -> g.height    

    // Builders
    let makeGlyphBox glyph (p:Position) = 
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
    let makeGlyph :GlyphBuilder = 
        fun font el -> 
        
            let symbol = 
                match el.operator with 
                | Some o -> getOperatorString o
                | Option.None -> el.symbol
            let lSpace = 
                let space = 
                    List.tryFind (fun x -> 
                        match x with
                        | LSpace _ -> true 
                        | _ -> false) el.attributes
                match space with 
                | Some (LSpace s) -> Operator.getValueFromLength font.emSquare s
                | _ -> 0.
            let rSpace = 
                let space = 
                    List.tryFind (fun x -> 
                        match x with
                        | RSpace _ -> true 
                        | _ -> false) el.attributes
                match space with 
                | Some (RSpace s) -> Operator.getValueFromLength font.emSquare s
                | _ -> match el.element = (Token Mo) with
                        | true -> (Operator.getValueFromLength font.emSquare (NamedLength ThickMathSpace))
                        | false -> 0.
                
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
             string = text;
             lSpace = lSpace;
             rSpace = rSpace}
    let makeRowFromGlyphs (glyphs:TypeObject list) =
        let errorGlyph = makeGlyph textSizeFont (Element.build (GeneralLayout Merror) [] [] "\ufffd" Option.None)
        let glyphs = List.map (fun x -> match x with | Glyph g -> g | GlyphRow gr -> errorGlyph ) glyphs
        
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
        let leftBearing = 
            match glyphs.Head.lSpace = 0. with
            | false -> glyphs.Head.lSpace - glyphs.Head.leftBearing
            | true -> glyphs.Head.leftBearing
        let rightBearing = 
            match glyphs.Head.rSpace = 0. with
            | false -> (List.rev glyphs).Head.rSpace - (List.rev glyphs).Head.rightBearing 
            | true -> (List.rev glyphs).Head.rightBearing
        let width = 
             List.fold (fun acc x -> x.width + acc) 0. glyphs + // glyphWidths
             List.fold (fun acc x -> x + acc) 0. kerns        + // kerns
             List.fold (fun acc x -> x + acc) 0. mathSpaces     // math space
        let height = (List.maxBy (fun x -> x.height) glyphs).height
        let glyphBoxes = List.map2 (fun g p -> makeGlyphBox g p) glyphs positions

        do  List.iter (fun x -> g.Children.Add(x) |> ignore) glyphBoxes
        {grid = g;
         rowWidth = width;
         rowHeight = height;
         leftBearing = leftBearing;
         rightBearing = rightBearing} |> GlyphRow    
    let makeRowFromTypeObjects (typeObjects:TypeObject list) =
        let g = Grid()
        let row0 = RowDefinition(Height = GridLength.Auto)
        let row1 = RowDefinition(Height = GridLength.Auto)
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
               
        let rec getRows (typeObjects:TypeObject list) acc = 
            match typeObjects with
            | [] -> acc
            //| gs when areAllGlyphs gs -> [makeRowFromGlyphs gs]
            | GlyphRow gr :: tail -> getRows tail (List.concat [acc; [GlyphRow gr]])
            | Glyph g :: tail -> 
                let tail' = Seq.toList(Seq.skipWhile (fun x -> match x with | Glyph _ -> true | GlyphRow _ -> false) tail)
                let r0 = 
                    let r = Seq.toList(Seq.takeWhile (fun x -> match x with | Glyph _ -> true | GlyphRow _ -> false) tail)
                    makeRowFromGlyphs ((Glyph g)::r)
                getRows tail' (List.concat [acc; [r0]])

        let rows = 
            getRows typeObjects [] 
            |> List.map (fun x -> 
                match x with 
                | GlyphRow gr -> gr 
                | Glyph g -> 
                    {grid=Grid();
                     rowWidth=0.;
                     rowHeight=0.;
                     leftBearing=0.;
                     rightBearing=0.}) 

        let positions = 
            let initialPosition = {x=0.;y=0.} 
                                           // Need to revisit this --\/
            let p = List.scan (fun acc x -> {x = (acc.x + (x.rowWidth + x.rightBearing + x.leftBearing) * 0.1); y = 0.}) initialPosition rows
            List.truncate rows.Length p  
            
        let mappedRows = 
            List.map2 (fun gr p -> 
                let gOut = gr.grid
                do  gOut.RenderTransform <- TranslateTransform(p.x + (gr.leftBearing/10.),p.y)
                gOut) rows positions

        let leftBearing = rows.Head.leftBearing
        let rightBearing = (List.rev rows).Head.rightBearing 
        let width = List.fold (fun acc x ->  x.rowWidth + acc) 0. rows
        let height = (List.maxBy (fun x -> x.rowHeight) rows).rowHeight
        
        do  List.iter (fun x -> g.Children.Add(x) |> ignore) mappedRows
        
        {grid = g;
         rowWidth = width;
         rowHeight = height;
         leftBearing = leftBearing;
         rightBearing = rightBearing}|> GlyphRow
    let makeSuperScriptFromTypeObjects (target:TypeObject) (script:TypeObject) superscriptShiftUp =        
        let g = Grid()

        let targetGrid =
            match target with
            | GlyphRow gr -> gr.grid
            | Glyph gl -> 
                let grid = Grid()
                let gb = makeGlyphBox gl {x=0.;y=0.}
                do grid.Children.Add(gb) |> ignore
                grid
        let targetRBearing =
            match target with
            | GlyphRow gr -> gr.rightBearing
            | Glyph gl -> gl.rightBearing

        let mathAxisCorrectionHeight = 
                MathPositioningConstants.axisHeight * 
                ((MathPositioningConstants.scriptPercentScaleDown / 100.) * 
                 (1. + textSizeScaleFactor))
        
        let scriptGrid = 
            
            let position = 
                let x = (getWidthFromTypeObject target - targetRBearing) * textSizeScaleFactor
                let y = (mathAxisCorrectionHeight - superscriptShiftUp) * textSizeScaleFactor 
                {x = x; y = y}
            match script with
            | GlyphRow gr -> 
                let g = gr.grid
                do g.Loaded.AddHandler(
                    RoutedEventHandler(
                        fun _ _ -> g.RenderTransform <- TranslateTransform(X = position.x, Y = position.y)))
                g
            | Glyph gl -> 
                let grid = Grid()
                let gb = makeGlyphBox gl position
                do grid.Children.Add(gb) |> ignore
                grid
        
        let leftBearing = 0.
        let rightBearing = MathPositioningConstants.spaceAfterScript
        let width = (getWidthFromTypeObject target) + (getWidthFromTypeObject script)
        let height = (getHeightFromTypeObject target) + ((mathAxisCorrectionHeight - superscriptShiftUp) * textSizeScaleFactor)
        
        do  List.iter (fun x -> g.Children.Add(x) |> ignore) [targetGrid; scriptGrid]
        
        {grid = g;
         rowWidth = width;
         rowHeight = height;
         leftBearing = leftBearing;
         rightBearing = rightBearing}|> GlyphRow

    let makeFractionFromTypeObjects 
        (numerator:TypeObject) 
        (denominator:TypeObject) =        
        //lineThickness 
        //numAlign
        //denomAlign
        //bevelled =
        
        let fractionWidth = 
            let n,d = (getWidthFromTypeObject numerator), (getWidthFromTypeObject denominator)           
            match n > d with
            | true -> n / 10.
            | false -> d / 10.
        
        let numeratorShift = 
            match (getWidthFromTypeObject numerator) * 0.1 >= fractionWidth with
            | true -> 0.
            | false -> ((getWidthFromTypeObject denominator) - (getWidthFromTypeObject numerator)) * 0.5
        let denominatorShift = 
            match (getWidthFromTypeObject denominator) * 0.1 >= fractionWidth with
            | true -> 0.
            | false -> ((getWidthFromTypeObject numerator) - (getWidthFromTypeObject denominator)) * 0.5

        let mathLine = (MathPositioningConstants.mathLeading + textBaseline - MathPositioningConstants.axisHeight) * textSizeScaleFactor

        let mathAxisCorrectionHeight = MathPositioningConstants.axisHeight * (MathPositioningConstants.scriptPercentScaleDown / 100.)

        let mLine = 
            let p = Path(Stroke = Brushes.Black, StrokeThickness = MathPositioningConstants.fractionRuleThickness * (100./960.))
            let pf = PathFigure(StartPoint = Point(0., mathLine))        
            do  pf.Segments.Add( LineSegment(Point(fractionWidth, mathLine), true ))
            let pg = PathGeometry() 
            do  pg.Figures.Add(pf)
                p.Data <- pg
                p.SetValue(Grid.RowProperty,1)
            p

        let numeratorGrid =
            match numerator with
            | GlyphRow gr -> gr.grid
            | Glyph gl -> 
                let grid = Grid()
                let gb = 
                    makeGlyphBox gl 
                        {x = numeratorShift;
                         y = mathAxisCorrectionHeight  
                             - (MathPositioningConstants.fractionNumeratorDisplayStyleShiftUp)
                             //- MathPositioningConstants.fractionNumeratorShiftUp
                             * (MathPositioningConstants.scriptPercentScaleDown / 100.)}                
                do grid.Children.Add(gb) |> ignore
                grid

        let denominatorGrid =              
            match denominator with
            | GlyphRow gr -> gr.grid
            | Glyph gl -> 
                let grid = Grid()
                let gb = 
                    makeGlyphBox gl 
                        {x=denominatorShift;
                         y = mathAxisCorrectionHeight 
                             + MathPositioningConstants.fractionDenominatorDisplayStyleShiftDown 
                             //+ MathPositioningConstants.fractionDenominatorShiftDown
                             * (MathPositioningConstants.scriptPercentScaleDown / 100.)}
                do grid.Children.Add(gb) |> ignore
                grid
        
        let leftBearing = 0.
        let rightBearing = 0.
        let width = fractionWidth * 10.
        let height = (getHeightFromTypeObject numerator) + (getHeightFromTypeObject denominator) // + some others
        
        let g = Grid()
                
        do g.Children.Add(mLine) |> ignore
           List.iter (fun x -> g.Children.Add(x) |> ignore) [numeratorGrid; denominatorGrid]
        
        {grid = g;
         rowWidth = width;
         rowHeight = height;
         leftBearing = leftBearing;
         rightBearing = rightBearing}|> GlyphRow


    // Typesetter    
    let typesetElement (el:Element) =        
        let math = 
            match el.element with
            | Math -> el
            | _ -> Element.build (Math) [] [] "" Option.None
        
        let typeset_Token (el:Element) = 
            match el.element = Token Mi ||
                  el.element = Token Mn ||
                  el.element = Token Mo ||
                  el.element = Token Ms ||
                  el.element = Token Mspace ||
                  el.element = Token Mtext with            
            | false -> makeGlyph (getFontFromTokenElement el) (Element.build (GeneralLayout Merror) [] [] "\ufffd" Option.None) |> Glyph
            | true -> makeGlyph (getFontFromTokenElement el) el |> Glyph
            
        let typeset_Row (el:TypeObject list) = makeRowFromTypeObjects el
      
        let typeset_Superscript ((target:TypeObject),(script:TypeObject), attributes) = 
            
            let superscriptShift =
                match List.tryFind (fun x -> 
                        match x with
                        | SuperScriptShift _ -> true 
                        | _ -> false) attributes with
                | Some (SuperScriptShift (Numb n)) -> n
                | _ -> 0.

            let display = 
                match List.tryFind (fun x -> 
                    match x with
                    | Display _ -> true 
                    | _ -> false) math.attributes with
                | Some (Display n) -> n
                | _ -> Inline
            let superscriptShiftUp = 
                match display with
                | Inline -> MathPositioningConstants.superscriptShiftUpCramped + superscriptShift
                | Block -> MathPositioningConstants.superscriptShiftUp + superscriptShift
            makeSuperScriptFromTypeObjects target script superscriptShiftUp
        
        let typeset_Fraction ((numerator:TypeObject),(denominator:TypeObject), attributes) = 
            makeFractionFromTypeObjects numerator denominator
        
        Element.recurseElement typeset_Token 
                               typeset_Row 
                               typeset_Superscript 
                               typeset_Fraction el

    (*Test Area*)
    type TestCanvas() as this  =  
        inherit UserControl()

        let t0 = (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.cubeRootPrefix))
        let t1 = (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let t2 = (Element.build (Token Mn) [] [] "8" Option.None)
        let t3 = (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.plusSignInfix))
        let t4 = (Element.build (Token Mi) [] [] "v" Option.None)
        let t5 = (Element.build (Token Mo) [] [] "" (Some OperatorDictionary.doubleStruckItalicSmallDPrefix))
        
        let s0=  (Element.build (Token Mo) [MathSize (EM 0.7<em>)] [] "" (Some OperatorDictionary.cubeRootPrefix)) 
        let s1 = (Element.build (Token Mo) [MathSize (EM 0.7<em>)] [] "" (Some OperatorDictionary.mathematicalLeftFlattenedParenthesisPrefix))
        let s2 = (Element.build (Token Mi) [MathSize (EM 0.7<em>)] [] "w" Option.None)
        let s3 = (Element.build (Token Mo) [MathSize (EM 0.7<em>); MathColor Brushes.BlueViolet] [] "" (Some OperatorDictionary.plusSignPrefix))
        let s4 = (Element.build (Token Mn) [MathSize (EM 0.7<em>)] [] "2" Option.None)
        let s5 = (Element.build (Token Mo) [MathSize (EM 0.7<em>)] [] "" (Some OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix))

        let r0 = (Element.build (GeneralLayout Mrow) [] [t2;t3;t4] "" Option.None)
        let r1 = (Element.build (GeneralLayout Mrow) [] [s1;s2;s3;s4;s5] "" Option.None)

        let r = typesetElement (Element.build (GeneralLayout Mrow) [] [r0;r1] "" Option.None)
        let s = typesetElement (Element.build (Script Msup) [] [r0;r1] "" Option.None)
        

        let ms = (Element.build (Script Msup) [] [r0;r1] "" Option.None)
        let m = typesetElement(Element.build (Math) [Display Block] [ms] "" Option.None)

        let f0 = (Element.build (GeneralLayout Mfrac) [] [s2;s4] "" Option.None)
        let f1 = (Element.build (GeneralLayout Mrow) [] [f0;t3;t4] "" Option.None)
        let f = typesetElement f1

        let line3 = getGridFromTypeObject f
       
        let textBlock =                    
            let tb = TextBlock()
            tb.Text <- (getWidthFromTypeObject r) .ToString() 
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
                line3.RenderTransform <- 
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

        do  canvas.Children.Add(line3) |> ignore
            //canvas.Children.Add(textBlock) |> ignore
            
            this.Content <- screen_Grid