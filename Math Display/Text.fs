namespace Math.Presentation

open MathML
open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.TextFormatting

module Text =

    //  Font Families
    let STIX2Math_FontFamily =           FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"),            "./#STIX Two Math")
    let STIX2TextBold_FontFamily =       FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Bold"),       "./#STIX Two Text Bold")
    let STIX2TextBoldItalic_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-BoldItalic"), "./#STIX Two Text Bold Italic")
    let STIX2TextItalic_FontFamily =     FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Italic"),     "./#STIX Two Text Italic")
    let STIX2TextRegular_FontFamily =    FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Regular"),    "./#STIX Two Text")
    
    //  Typefaces
    let STIX2Math_Typeface =           Typeface(STIX2Math_FontFamily,System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextBold_Typeface =       Typeface(STIX2TextBold_FontFamily,      System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextBoldItalic_Typeface = Typeface(STIX2TextBoldItalic_FontFamily,System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextItalic_Typeface =     Typeface(STIX2TextItalic_FontFamily,    System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    let STIX2TextRegular_Typeface =    Typeface(STIX2TextRegular_FontFamily,   System.Windows.FontStyles.Normal,System.Windows.FontWeights.Normal,System.Windows.FontStretches.Normal)
    
    type TypographyPropertiesRecord =     
        {   AnnotationAlternates : int;
            Capitals : FontCapitals;
            CapitalSpacing : bool;
            CaseSensitiveForms : bool;
            ContextualAlternates : bool;
            ContextualLigatures : bool;
            ContextualSwashes : int;
            DiscretionaryLigatures : bool;
            EastAsianExpertForms : bool;
            EastAsianLanguage : FontEastAsianLanguage;
            EastAsianWidths : FontEastAsianWidths;
            Fraction : FontFraction;
            HistoricalForms : bool;
            HistoricalLigatures : bool;
            Kerning : bool;
            MathematicalGreek : bool;
            NumeralAlignment : FontNumeralAlignment;
            NumeralStyle : FontNumeralStyle;
            SlashedZero : bool;
            StandardLigatures : bool;
            StandardSwashes : int;
            StylisticAlternates : int;
            StylisticSet1  : bool;
            StylisticSet10 : bool;
            StylisticSet11 : bool;
            StylisticSet12 : bool;
            StylisticSet13 : bool;
            StylisticSet14 : bool;
            StylisticSet15 : bool;
            StylisticSet16 : bool;
            StylisticSet17 : bool;
            StylisticSet18 : bool;
            StylisticSet19 : bool;
            StylisticSet2  : bool;
            StylisticSet20 : bool;
            StylisticSet3  : bool;
            StylisticSet4  : bool;
            StylisticSet5  : bool;
            StylisticSet6  : bool;
            StylisticSet7  : bool;
            StylisticSet8  : bool;
            StylisticSet9  : bool;
            Variants : FontVariants} 
    type TypographyProperties(typographyPropertiesRecord) = 
        inherit TextFormatting.TextRunTypographyProperties()

        override properties.AnnotationAlternates = typographyPropertiesRecord.AnnotationAlternates
        override properties.Capitals = typographyPropertiesRecord.Capitals
        override properties.CapitalSpacing = typographyPropertiesRecord.CapitalSpacing
        override properties.CaseSensitiveForms = typographyPropertiesRecord.CaseSensitiveForms
        override properties.ContextualAlternates = typographyPropertiesRecord.ContextualAlternates
        override properties.ContextualLigatures = typographyPropertiesRecord.ContextualLigatures
        override properties.ContextualSwashes = typographyPropertiesRecord.ContextualSwashes
        override properties.DiscretionaryLigatures = typographyPropertiesRecord.DiscretionaryLigatures
        override properties.EastAsianExpertForms = typographyPropertiesRecord.EastAsianExpertForms
        override properties.EastAsianLanguage = typographyPropertiesRecord.EastAsianLanguage
        override properties.EastAsianWidths = typographyPropertiesRecord.EastAsianWidths
        override properties.Fraction = typographyPropertiesRecord.Fraction
        override properties.HistoricalForms = typographyPropertiesRecord.HistoricalForms
        override properties.HistoricalLigatures = typographyPropertiesRecord.HistoricalLigatures
        override properties.Kerning = typographyPropertiesRecord.Kerning
        override properties.MathematicalGreek = typographyPropertiesRecord.MathematicalGreek
        override properties.NumeralAlignment = typographyPropertiesRecord.NumeralAlignment
        override properties.NumeralStyle = typographyPropertiesRecord.NumeralStyle
        override properties.SlashedZero = typographyPropertiesRecord.SlashedZero
        override properties.StandardLigatures = typographyPropertiesRecord.StandardLigatures
        override properties.StandardSwashes = typographyPropertiesRecord.StandardSwashes
        override properties.StylisticAlternates = typographyPropertiesRecord.StylisticAlternates
        override properties.StylisticSet1 = typographyPropertiesRecord.StylisticSet1
        override properties.StylisticSet10 = typographyPropertiesRecord.StylisticSet10
        override properties.StylisticSet11 = typographyPropertiesRecord.StylisticSet11
        override properties.StylisticSet12 = typographyPropertiesRecord.StylisticSet12
        override properties.StylisticSet13 = typographyPropertiesRecord.StylisticSet13
        override properties.StylisticSet14 = typographyPropertiesRecord.StylisticSet14
        override properties.StylisticSet15 = typographyPropertiesRecord.StylisticSet15
        override properties.StylisticSet16 = typographyPropertiesRecord.StylisticSet16
        override properties.StylisticSet17 = typographyPropertiesRecord.StylisticSet17
        override properties.StylisticSet18 = typographyPropertiesRecord.StylisticSet18
        override properties.StylisticSet19 = typographyPropertiesRecord.StylisticSet19
        override properties.StylisticSet2 = typographyPropertiesRecord.StylisticSet2
        override properties.StylisticSet20 = typographyPropertiesRecord.StylisticSet20
        override properties.StylisticSet3 = typographyPropertiesRecord.StylisticSet3
        override properties.StylisticSet4 = typographyPropertiesRecord.StylisticSet4
        override properties.StylisticSet5 = typographyPropertiesRecord.StylisticSet5
        override properties.StylisticSet6 = typographyPropertiesRecord.StylisticSet6
        override properties.StylisticSet7 = typographyPropertiesRecord.StylisticSet7
        override properties.StylisticSet8 = typographyPropertiesRecord.StylisticSet8
        override properties.StylisticSet9 = typographyPropertiesRecord.StylisticSet9 
        override properties.Variants = typographyPropertiesRecord.Variants
    let  defaultTypographyProperties =
        {AnnotationAlternates = 0;
         Capitals = FontCapitals.Normal;
         CapitalSpacing = false;
         CaseSensitiveForms = false;
         ContextualAlternates = true;
         ContextualLigatures = true;
         ContextualSwashes = 0;
         DiscretionaryLigatures = false;
         EastAsianExpertForms = false;
         EastAsianLanguage = FontEastAsianLanguage.Normal;
         EastAsianWidths = FontEastAsianWidths.Normal;
         Fraction = FontFraction.Normal;
         HistoricalForms = false;
         HistoricalLigatures = false;
         Kerning = true;
         MathematicalGreek = false;
         NumeralAlignment = FontNumeralAlignment.Normal;
         NumeralStyle = FontNumeralStyle.Normal;
         SlashedZero = false;
         StandardLigatures = true;
         StandardSwashes = 0;
         StylisticAlternates = 0;
         StylisticSet1 = true;
         StylisticSet10 = false;
         StylisticSet11 = false;
         StylisticSet12 = false;
         StylisticSet13 = false;
         StylisticSet14 = false;
         StylisticSet15 = false;
         StylisticSet16 = false;
         StylisticSet17 = false;
         StylisticSet18 = false;
         StylisticSet19 = false;
         StylisticSet2 = false;
         StylisticSet20 = false;
         StylisticSet3 = false;
         StylisticSet4 = false;
         StylisticSet5 = false;
         StylisticSet6 = false;
         StylisticSet7 = false;
         StylisticSet8 = false;
         StylisticSet9 = false;
         Variants = FontVariants.Normal}

    type RunPropertiesRecord = 
        {
         BackgroundBrush : Brush;
         BaselineAlignment : BaselineAlignment;
         CultureInfo : System.Globalization.CultureInfo;
         FontHintingEmSize : float;
         FontRenderingEmSize : float;
         ForegroundBrush : Brush;
         NumberSubstitution : NumberSubstitution;
         TextDecorations : TextDecorationCollection;
         TextEffects : TextEffectCollection;
         Typeface : Typeface;
         TypographyProperties : TypographyProperties       
        }    
    type RunProperties(runPropertiesRecord) = 
        inherit TextRunProperties()
        
        override properties.BackgroundBrush = runPropertiesRecord.BackgroundBrush
        override properties.BaselineAlignment = runPropertiesRecord.BaselineAlignment
        override properties.CultureInfo = runPropertiesRecord.CultureInfo
        override properties.FontHintingEmSize = runPropertiesRecord.FontHintingEmSize
        override properties.FontRenderingEmSize = runPropertiesRecord.FontRenderingEmSize
        override properties.ForegroundBrush = runPropertiesRecord.ForegroundBrush
        override properties.NumberSubstitution = runPropertiesRecord.NumberSubstitution
        override properties.TextDecorations = runPropertiesRecord.TextDecorations
        override properties.TextEffects = runPropertiesRecord.TextEffects
        override properties.Typeface = runPropertiesRecord.Typeface
        override properties.TypographyProperties = upcast runPropertiesRecord.TypographyProperties
    let  defaultRunProperties =         
        {BackgroundBrush  =  Brushes.Transparent;
         BaselineAlignment  =  BaselineAlignment.Baseline;
         CultureInfo  =  System.Globalization.CultureInfo("es-ES", false);
         FontHintingEmSize  = 1000.;
         FontRenderingEmSize  = 1000.;
         ForegroundBrush  =  Brushes.Black;
         NumberSubstitution  =  NumberSubstitution();
         TextDecorations  =  TextDecorationCollection();
         TextEffects  =  TextEffectCollection();
         Typeface  =  STIX2Math_Typeface;
         TypographyProperties  =  TypographyProperties(defaultTypographyProperties);
        }
 
    type ParagraphPropertiesRecord = 
        {
         AlwaysCollapsible : bool; 
         DefaultIncrementalTab : float;
         DefaultTextRunProperties : TextRunProperties;
         FirstLineInParagraph : bool;
         FlowDirection : FlowDirection;
         Indent : float;
         LineHeight : float;
         ParagraphIndent  : float;
         TextAlignment : TextAlignment;
         TextDecorations :TextDecorationCollection;
         TextMarkerProperties : TextMarkerProperties;
         TextWrapping : TextWrapping
        }
    type ParagraphProperties(paragraphPropertiesRecord) = 
        inherit TextParagraphProperties()

        override properties.AlwaysCollapsible = paragraphPropertiesRecord.AlwaysCollapsible
        override properties.DefaultIncrementalTab = paragraphPropertiesRecord.DefaultIncrementalTab
        override properties.DefaultTextRunProperties = paragraphPropertiesRecord.DefaultTextRunProperties
        override properties.FirstLineInParagraph = paragraphPropertiesRecord.FirstLineInParagraph
        override properties.FlowDirection = paragraphPropertiesRecord.FlowDirection
        override properties.Indent = paragraphPropertiesRecord.Indent
        override properties.LineHeight = paragraphPropertiesRecord.LineHeight
        override properties.ParagraphIndent = paragraphPropertiesRecord.ParagraphIndent
        override properties.TextAlignment = paragraphPropertiesRecord.TextAlignment
        override properties.TextDecorations = paragraphPropertiesRecord.TextDecorations
        override properties.TextMarkerProperties = paragraphPropertiesRecord.TextMarkerProperties
        override properties.TextWrapping = paragraphPropertiesRecord.TextWrapping
    let  defaultParagraphProperties =
        {
         AlwaysCollapsible  =  false;
         DefaultIncrementalTab  = 0.;
         DefaultTextRunProperties  = RunProperties(defaultRunProperties);
         FirstLineInParagraph  = true;
         FlowDirection = FlowDirection.LeftToRight;
         Indent = 0.;
         LineHeight = 0.;
         ParagraphIndent = 0.;
         TextAlignment = TextAlignment.Left;
         TextDecorations = TextDecorationCollection();
         TextMarkerProperties = null;
         TextWrapping = TextWrapping.NoWrap
        }

    type Store(text:string,typeface:Typeface,emSize:float<MathML.em>) = 
        inherit TextSource()
        
        let runProperties = 
            {defaultRunProperties with 
                FontHintingEmSize = (emSize/1.<MathML.em>);
                FontRenderingEmSize = (emSize/1.<MathML.em>);
                Typeface  = typeface}

        override store.GetTextRun(textSourceCharacterIndex) : TextRun = 
            match textSourceCharacterIndex with
            | i when i < 0 -> failwith "textSourceCharacterIndex, Value must be greater than 0."
            | i when i >= text.Length -> upcast TextEndOfParagraph(1)
            | i when i < text.Length -> 
                upcast TextFormatting.TextCharacters
                    (text,textSourceCharacterIndex,
                     text.Length - textSourceCharacterIndex,
                     RunProperties(runProperties))
            | _ -> upcast TextEndOfParagraph(1)
        override store.GetPrecedingText(textSourceCharacterIndexLimit) : TextSpan<CultureSpecificCharacterBufferRange> =
            let characterBufferRange = 
                CharacterBufferRange(text, 0, textSourceCharacterIndexLimit)
            TextSpan<CultureSpecificCharacterBufferRange>
                (textSourceCharacterIndexLimit,
                 CultureSpecificCharacterBufferRange(System.Globalization.CultureInfo.CurrentUICulture, characterBufferRange))
        override store.GetTextEffectCharacterIndexFromTextSourceCharacterIndex(_textSourceCharacterIndex) = 0

    let format (text:string) (typeface:Typeface) (emSize:float<MathML.em>)  =         
        
        let runProperties = 
            {defaultRunProperties with 
                FontHintingEmSize = (emSize/1.<MathML.em>);
                FontRenderingEmSize = (emSize/1.<MathML.em>);
                Typeface  = typeface}
        let paragraphProperties = 
            {defaultParagraphProperties with
                DefaultTextRunProperties = RunProperties(runProperties)}
        
        let textStore = Store(text,typeface,emSize)              
        let textFormatter = TextFormatting.TextFormatter.Create()
        let textLine =             
            textFormatter.FormatLine
                (textStore,
                    0,
                    96.*6.,
                    ParagraphProperties(paragraphProperties),
                    null)
        textLine

    