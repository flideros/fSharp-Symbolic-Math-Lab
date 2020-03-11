namespace Math.Presentation

open MathML
open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

module Text =

    type TypographyProperties() = 
        inherit TextFormatting.TextRunTypographyProperties()

        default properties.AnnotationAlternates = 0
        default properties.Capitals = FontCapitals.Normal
        default properties.CapitalSpacing = false
        default properties.CaseSensitiveForms = false
        default properties.ContextualAlternates = true
        default properties.ContextualLigatures = true
        default properties.ContextualSwashes = 0
        default properties.DiscretionaryLigatures = false
        default properties.EastAsianExpertForms = false
        default properties.EastAsianLanguage = FontEastAsianLanguage.Normal
        default properties.EastAsianWidths = FontEastAsianWidths.Normal
        default properties.Fraction = FontFraction.Normal
        default properties.HistoricalForms = false
        default properties.HistoricalLigatures = false
        default properties.Kerning = true
        default properties.MathematicalGreek = false
        default properties.NumeralAlignment = FontNumeralAlignment.Normal
        default properties.NumeralStyle = FontNumeralStyle.Normal
        default properties.SlashedZero = false
        default properties.StandardLigatures = true
        default properties.StandardSwashes = 0
        default properties.StylisticAlternates = 0
        default properties.StylisticSet1 = true
        default properties.StylisticSet10 = false
        default properties.StylisticSet11 = false
        default properties.StylisticSet12 = false
        default properties.StylisticSet13 = false
        default properties.StylisticSet14 = false
        default properties.StylisticSet15 = false
        default properties.StylisticSet16 = false
        default properties.StylisticSet17 = false
        default properties.StylisticSet18 = false
        default properties.StylisticSet19 = false
        default properties.StylisticSet2 = false
        default properties.StylisticSet20 = false
        default properties.StylisticSet3 = false
        default properties.StylisticSet4 = false
        default properties.StylisticSet5 = false
        default properties.StylisticSet6 = false
        default properties.StylisticSet7 = false
        default properties.StylisticSet8 = false
        default properties.StylisticSet9 = false
        default properties.Variants = FontVariants.Normal

    type RunProperties() = 
        inherit TextFormatting.TextRunProperties()
        
        let STIX2Math_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"),"./#STIX Two Math")
        let STIX2Math_Typeface = Typeface(STIX2Math_FontFamily,System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)

        default properties.BackgroundBrush = upcast Brushes.Black
        default properties.BaselineAlignment = BaselineAlignment.Baseline
        default properties.CultureInfo = System.Globalization.CultureInfo("es-ES", false)
        default properties.FontHintingEmSize = 12.
        default properties.FontRenderingEmSize = 12.
        default properties.ForegroundBrush = upcast Brushes.Black
        default properties.NumberSubstitution = NumberSubstitution()
        default properties.TextDecorations = TextDecorationCollection()
        default properties.TextEffects = TextEffectCollection()
        default properties.Typeface = STIX2Math_Typeface
        default properties.TypographyProperties = upcast TypographyProperties()

    type ParagraphProperties() as properties = 
        inherit TextFormatting.TextParagraphProperties()

        default properties.AlwaysCollapsible = false
        default properties.DefaultIncrementalTab = 0.
        default properties.DefaultTextRunProperties = upcast RunProperties()
        default properties.FirstLineInParagraph = true
        default properties.FlowDirection = FlowDirection.LeftToRight
        default properties.Indent = 0.
        default properties.LineHeight = 0.
        default properties.ParagraphIndent = 0.        
        default properties.TextAlignment = TextAlignment.Left
        default properties.TextDecorations = TextDecorationCollection()
        default properties.TextMarkerProperties = 
            upcast TextFormatting.TextSimpleMarkerProperties(TextMarkerStyle(),0.,1,properties)
        default properties.TextWrapping = TextWrapping.NoWrap
        
        
    
    
    
    type Store = {ToDo:int}



    let drawingGroup = DrawingGroup()
    let drawingContext = drawingGroup.Open()
    let tf = TextFormatting.TextFormatter.Create()
    
    
