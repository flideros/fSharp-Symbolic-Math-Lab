﻿namespace Math.Presentation

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
            let p = Path(Stroke = Brushes.Black, Fill = Brushes.Purple)
            let geometry = ft.BuildGeometry(Point(0.,0.)) 
            do  p.Data <- geometry.GetFlattenedPathGeometry()
            {path=p;leftBearing = 0.; rightBearing = ft.OverhangTrailing}

    let getOperatorString (operator : Operator) = 
        let rec loop oc =
            match oc with
            | Unicode u -> (char u).ToString()
            | Char c -> c.ToString()
            | UnicodeArray a -> 
                let chars = Array.map (fun oc -> (char)(loop oc)) a
                new string (chars)
        loop operator.character

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

        let operator_GlyphBox = getGlyphFromFont(getOperatorString blackLeftPointingSmallTriangleInfix)
            
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
                operator_GlyphBox.RenderTransform <- ScaleTransform(ScaleX = 5./s,ScaleY=5./s)
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
 
        do  canvas.Children.Add(operator_GlyphBox) |> ignore //font_Border) |> ignore

            this.Content <- screen_Grid