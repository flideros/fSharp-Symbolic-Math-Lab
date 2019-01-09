namespace MathML

[<Measure>] type em /// an em (font-relative unit traditionally used for horizontal lengths)
[<Measure>] type ex /// an ex (font-relative unit traditionally used for vertical lengths)
[<Measure>] type px /// pixels, or size of a pixel in the current display
[<Measure>] type inch /// inches (1 inch = 2.54 centimeters)
[<Measure>] type cm /// centimeters
[<Measure>] type mm /// millimeters
[<Measure>] type pt /// points (1 point = 1/72 inch)
[<Measure>] type pc /// picas (1 pica = 12 points)
[<Measure>] type pct /// percentage of the default value


type NamedSpace = 
    | VeryVeryThinMathSpace /// 1/18em
    | VeryThinMathSpace /// 2/18em
    | ThinMathSpace /// 3/18em
    | MediumMathSpace /// 4/18em
    | ThickMathSpace /// 5/18em
    | VeryThickMathSpace /// 6/18em
    | VeryVeryThickMathSpace /// 7/18em
    | NegativeVeryVeryThinMathSpace /// -1/18em
    | NegativeVeryThinMathSpace /// -2/18em
    | NegativeThinMathSpace /// -3/18em
    | NegativeMediumMathSpace /// -4/18em
    | NegativeThickMathSpace /// -5/18em
    | NegativeVeryThickMathSpace /// -6/18em
    | NegativeVeryVeryThickMathSpace /// -7/18em

type Length =  
    | Number of float
    | EM of float<em>
    | EX of float<ex>
    | PX of float<px>
    | IN of float<inch>
    | CM of float<cm>
    | MM of float<mm>
    | PT of float<pt>
    | PC of float<pc>
    | Pct of float<pct>
    | NamedLength of NamedSpace
    | KeyWord of obj

type Color = string
type Idref = string
type Uri = string

type _Id = string
type _LineBreakMultChar = string /// hex character code
type _LQuote = string /// character code
type _RQuote = string /// character code
type _ActionType = | Toggle | StatusLine | ToolTip | Input | Highlight
type _Align = | Left | Right | Top | Bottom | Center | Baseline | Axis
type _CharAlign = | Left | Right | Center
type _CharSpacing = | Loose | Medium | Tight | Length of Length 
type _ColumnAlign = | Left | Right | Center
type _ColumnLines = | None | Solid | Dashed
type _ColumnWidth = | Length of Length | Auto | Fit
type _Crossout = | None | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike
type _DenomAlign = | Left | Right | Center
type _Dir = | Ltr | Rtl
type _Display = | Block | Inline
type _Edge = | Left | Right
type _Form = | Prefix | Infix | Postfix
type _Frame = | None | Solid | Dashed
type _GroupAlign = | Left | Center | Right | DecimalPoint
type _IndentAlign = | Left | Center | Right | Auto | Id
type _IndentAlignFirst = | Left | Center | Right | Auto | Id | IndentAlign
type _IndentAlignLast = | Left | Center | Right | Auto | Id | IndentAlign
type _LineBreak = | Auto | NewLine | NoBreak | GoodBreak | BadBreak
type _LineBreakStyle = | Before | After | Duplicate | InfixLinebBreakStyle
type _Location = | W | NW | N | NE | E | SE | S | SW
type _LongDivStyle = | LeftTop | StackedRightRight | MediumStackedRightRight | ShortStackedRightRight | RightTop | LeftRight1  | LeftRight2 | RightRight | StackedLeftLeft | StackedLeftLineTop
type _MathVariant = | Normal | Bold | Italic | BoldItalic | DoubleStruck | BoldFraktur | Script | BoldScript | Fraktur | SansSerif | BoldSansSerif | SansSerifItalic | SansSerifBoldItalic | MonoSpace | Initial | Tailed | Looped | Stretched
type _Notation = | LongDiv | Actuarial | PhasOrAngle | Radical | Box | RoundedBox | Circle | Left | Right | Top | Bottom | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike | NorthEastArrow | Madruwb | Text
type _NumAlign = | Left | Right | Center
type _Overflow = | Linebreak | Scroll | Elide | Truncate | Scale
type _RowAlign = | Top | Bottom | Center | Baseline | Axis
type _RowLines = | None | Solid | Dashed
type _Side = | Left | Right | LeftOverlap | RightOerlap
type _StackAlign = | Left | Center | Right | DecimalPoint

type MathMLAttribute =
    //private
    | Accent of bool
    | AccentUnder of bool
    | ActionType of _ActionType
    | Align of _Align
    | AlignmentScope of bool
    | Alt of Uri
    | AltImg of Uri
    | AltImgHeight of Length
    | AltImgValign of Length
    | AltImgWidth of Length
    | AltText of string // Alternate string
    | Bevelled of bool
    | CdGroup of Uri
    | CharAlign of _CharAlign
    | CharSpacing of Length
    | Class of string //Associates the element with a set of style classes for use with [XSLT] and [CSS21].
    | Close of string //Specifies the closing delimiter.
    | ColumnAlign of _ColumnAlign
    | ColumnLines of _ColumnLines
    | ColumnSpacing of Length
    | ColumnSpan of uint32
    | ColumnWidth of Length
    | Crossout of _Crossout
    | DecimalPoint of char
    | DenomAlign of _DenomAlign
    | Depth of Length
    | Dir of _Dir
    | Display of _Display
    | DisplayStyle of bool
    | Edge of _Edge
    | EqualColumns of bool
    | EqualRows of bool
    | Fence of bool
    | Form of _Form
    | Frame of _Frame
    | FrameSpacing of Length*Length
    | GroupAlign of _GroupAlign
    | Height of Length
    | Href of Uri
    | Id of _Id
    | IndentAlignFirst of _IndentAlignFirst
    | IndentAlignLast of _IndentAlignLast
    | IndentAlign of _IndentAlign
    | IndentShift of Length
    | IndentShiftFirst of Length
    | IndentShiftLast of Length
    | IndentTarget of Idref
    | InfixLineBreakStyle of _LineBreakStyle
    | LargeOp of bool
    | LeftOverhang of Length
    | Length of uint32
    | LineBreak of _LineBreak
    | LineBreakMultChar of _LineBreakMultChar
    | LineBreakStyle of _LineBreakStyle
    | LineLeading of Length
    | LineThickness of Length
    | Location of _Location
    | LongDivStyle of _LongDivStyle
    | LQuote of _LQuote
    | LSpace of Length
    | MathBackground of Color
    | MathColor of Color
    | MathSize of Length
    | MathVariant of _MathVariant
    | MaxSize of Length
    | MaxWidth of Length
    | MinLabelSpacing of Length
    | MinSize of Length
    | MovableLimits of bool
    | MsLineThickness of Length
    | Notation of _Notation
    | NumAlign of _NumAlign
    | Open of string // Specifies the opening delimiter.
    | Overflow of _Overflow
    | Position of int
    | RightOverhang of Length
    | RowAlign of _RowAlign
    | RowLines of _RowLines
    | RowSpacing of Length
    | RowSpan of uint32
    | RQuote of _RQuote
    | RSpace of Length
    | ScriptLevel of char*uint32
    | ScriptMinSize of Length
    | ScriptSizeMultiplier of float
    | Selection of uint32
    | Separator of bool
    | Separators of string //Specifies a sequence of zero or more separator characters, optionally separated by whitespace.
    | Shift of int
    | Side of _Side
    | Src of Uri
    | StackAlign of _StackAlign
    | Stretchy of bool
    | Style of string //Associates style information with the element for use with [XSLT] and [CSS21].
    | SubScriptShift of Length
    | SuperScriptShift of Length
    | Symmetric of bool
    | VAlign of Length
    | VOffset of Length
    | Width of Length
    | Xmlns of Uri
    | Xref of Idref

type TokenElement = | Mi | Mn | Mo | Mtext | Mspace | Ms | Mglyph
type GeneralLayoutElement = | Mrow | Mfrac | Msqrt | Mroot | Mstyle | Merror | Mpadded | Mphantom | Mfenced | Menclose
type ScriptElement = | Msub | Msup | Msubsup | Munde | Mover | Munderover | Mmultiscripts
type TableElement = | Mtable | Mlabeledtr | Mtr | Mtd | Maligngroup | Malignmark
type MathLayoutElement = | Mstack | Mlongdiv | Msgroup | Msrow | Mscarries  | Mscarry | Msline
type EnliveningExpressionElement = | Maction

type Element = | Math | Token of TokenElement | GeneralLayout of GeneralLayoutElement | Script of ScriptElement | Table of TableElement | MathLayout of MathLayoutElement | Enlivening of EnliveningExpressionElement

module Element =
    let private isValidElementAttributeOf defaultAttrs attr = List.exists (fun elem -> elem.GetType() = attr.GetType()) defaultAttrs
    let private scrubAttributes attrList defaultAttributes = 
        List.choose (fun elem ->
        match elem with
        | elem when isValidElementAttributeOf defaultAttributes elem -> Option.Some elem
        | _ -> Option.None) attrList
    
    let element (elem : Element) (attr : MathMLAttribute list) args = 
        
        match elem with
        | Math ->                    //2.2 Top-Level <math> Element 
            let defaultAttributes = 
                                    [Display Inline;
                                     MaxWidth (KeyWord "available width");
                                     Overflow Linebreak; 
                                     AltImg "none";                                      
                                     AltImgWidth (KeyWord "altimg width"); 
                                     AltImgHeight (KeyWord "altimg height");
                                     AltImgValign (EM 0.0<em>); 
                                     AltText ""; 
                                     CdGroup ""; 
                                     
                                     //3.3.4 Style Change <mstyle>
                                     ScriptLevel ('+',0u)
                                     DisplayStyle false ///When display="block", displaystyle is initialized to "true" when display="inline", displaystyle is initialized to "false"
                                     ScriptSizeMultiplier 0.71
                                     ScriptMinSize (PT 8.0<pt>)
                                     InfixLineBreakStyle Before;
                                     DecimalPoint '.'
                                     
                                     //2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;
                                     
                                     //The rest after I compared this to the MathML schema
                                     Accent false;
                                     AccentUnder false;
                                     Align _Align.Center;
                                     AlignmentScope true;
                                     Bevelled false;
                                     CharAlign _CharAlign.Center;
                                     CharSpacing (KeyWord _CharSpacing.Medium);
                                     Close ")";
                                     ColumnAlign _ColumnAlign.Center;
                                     ColumnLines _ColumnLines.None;
                                     ColumnSpacing (EM 0.8<em>);
                                     ColumnSpan 1u;
                                     ColumnWidth (KeyWord _ColumnWidth.Auto);
                                     Crossout _Crossout.None;
                                     DenomAlign _DenomAlign.Center;
                                     Depth (EX 0.0<ex>);
                                     Edge _Edge.Left;
                                     EqualColumns false;
                                     EqualRows false;
                                     Fence false;
                                     Form _Form.Infix;
                                     Frame _Frame.None;
                                     FrameSpacing (EM 0.4<em>,EX 0.5<ex>);
                                     GroupAlign _GroupAlign.Left;
                                     Height (KeyWord "fromimage");
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Number 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";
                                     LargeOp false;
                                     LeftOverhang (Number 0.0);
                                     Length 0u; //<msline/> Specifies the the number of columns that should be spanned by the line.
                                     LineBreak _LineBreak.Auto;
                                     LineBreakMultChar "02062";//&InvisibleTimes;
                                     LineBreakStyle _LineBreakStyle.Before;
                                     LineLeading (Pct 100.0<pct>);
                                     LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                                     Location N;
                                     LongDivStyle LeftTop;
                                     LQuote "&quot;";
                                     LSpace (NamedLength ThickMathSpace);
                                     MaxSize (KeyWord "infinity");
                                     MinLabelSpacing (EM 0.8<em>);
                                     MinSize (Pct 100.0<pct>);
                                     MovableLimits false;
                                     MsLineThickness (KeyWord "medium");
                                     Notation LongDiv;
                                     NumAlign _NumAlign.Center;
                                     Open ")";
                                     Position 0;
                                     RightOverhang (Number 0.0);
                                     RowAlign _RowAlign.Baseline;
                                     RowLines _RowLines.None;
                                     RowSpacing (EX 1.0<ex>);
                                     RowSpan 1u;
                                     RQuote "&quot;";
                                     RSpace (NamedLength ThickMathSpace);
                                     Selection 1u;
                                     Separator false;
                                     Separators ",";
                                     Shift 0;
                                     Side _Side.Right;
                                     StackAlign _StackAlign.DecimalPoint;
                                     Stretchy false;
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     Symmetric false;
                                     VAlign (EX 0.0<ex>);
                                     Width (KeyWord "automatic");
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)

        | Token Mi -> 
            let defaultAttributes = 
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;                                     
                                     ]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mn ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;                                     
                                     ]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mo ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;                                     
                                     
                                     //3.2.5.2.1 Dictionary-based attributes 
                                     Fence false;
                                     Form _Form.Infix;
                                     Separator false;
                                     LSpace (NamedLength ThickMathSpace);
                                     RSpace (NamedLength ThickMathSpace);
                                     Stretchy false
                                     Symmetric false
                                     MaxSize (KeyWord "infinity");                                     
                                     MinSize (Pct 100.0<pct>);
                                     LargeOp false;
                                     MovableLimits false;
                                     Accent false;
                                     
                                     //3.2.5.2.2 Linebreaking attributes 
                                     LineBreak _LineBreak.Auto;
                                     LineBreakMultChar "02062"; //&InvisibleTimes;
                                     LineBreakStyle _LineBreakStyle.Before;
                                     LineLeading (Pct 100.0<pct>);
                                     
                                     //3.2.5.2.3 Indentation attributes
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Number 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";                                     
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mtext ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;  
                                     ]
                                     
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mspace ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;
                                     
                                     //3.2.7.2 Attributes 
                                     Width (EM 0.0<em>);
                                     Height (EX 0.0<ex>);
                                     Depth (EX 0.0<ex>);
                                     LineBreak _LineBreak.Auto
                                     
                                     //3.2.5.2.3 Indentation attributes 
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Number 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";                                     
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Ms ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;
                                     
                                     //3.2.8.2 Attributes 
                                     RQuote "&quot;";
                                     LQuote "&quot;";
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mglyph ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent"; 
                                     //3.2.1.2.2 Attributes 
                                     Src "required"
                                     Width (KeyWord "fromimage");
                                     Height (KeyWord "fromimage");                                     
                                     VAlign (EX 0.0<ex>);
                                     Alt "required"
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mrow ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.1.2 Attributes
                                     Dir Ltr
                                    ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mfrac ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.2.2 Attributes
                                     LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                                     NumAlign _NumAlign.Center;
                                     DenomAlign _DenomAlign.Center;
                                     Bevelled false;
                                    ]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Msqrt ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mroot ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mstyle ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.4.2 Attributes 
                                     ScriptLevel ('+',0u)
                                     DisplayStyle false ///When display="block", displaystyle is initialized to "true" when display="inline", displaystyle is initialized to "false"
                                     ScriptSizeMultiplier 0.71
                                     ScriptMinSize (PT 8.0<pt>)
                                     InfixLineBreakStyle Before;
                                     DecimalPoint '.'

                                     //The rest after I compared this to the MathML schema
                                     Accent false;
                                     AccentUnder false;
                                     Align _Align.Center;
                                     AlignmentScope true;
                                     Bevelled false;
                                     CharAlign _CharAlign.Center;
                                     CharSpacing (KeyWord _CharSpacing.Medium);
                                     Close ")";
                                     ColumnAlign _ColumnAlign.Center;
                                     ColumnLines _ColumnLines.None;
                                     ColumnSpacing (EM 0.8<em>);
                                     ColumnSpan 1u;
                                     ColumnWidth (KeyWord _ColumnWidth.Auto);
                                     Crossout _Crossout.None;
                                     DenomAlign _DenomAlign.Center;
                                     Depth (EX 0.0<ex>);
                                     Edge _Edge.Left;
                                     EqualColumns false;
                                     EqualRows false;
                                     Fence false;
                                     Form _Form.Infix;
                                     Frame _Frame.None;
                                     FrameSpacing (EM 0.4<em>,EX 0.5<ex>);
                                     GroupAlign _GroupAlign.Left;
                                     Height (KeyWord "fromimage");
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Number 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";
                                     LargeOp false;
                                     LeftOverhang (Number 0.0);
                                     Length 0u; //<msline/> Specifies the the number of columns that should be spanned by the line.
                                     LineBreak _LineBreak.Auto;
                                     LineBreakMultChar "02062";//&InvisibleTimes;
                                     LineBreakStyle _LineBreakStyle.Before;
                                     LineLeading (Pct 100.0<pct>);
                                     LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                                     Location N;
                                     LongDivStyle LeftTop;
                                     LQuote "&quot;";
                                     LSpace (NamedLength ThickMathSpace);
                                     MaxSize (KeyWord "infinity");
                                     MinLabelSpacing (EM 0.8<em>);
                                     MinSize (Pct 100.0<pct>);
                                     MovableLimits false;
                                     MsLineThickness (KeyWord "medium");
                                     Notation LongDiv;
                                     NumAlign _NumAlign.Center;
                                     Open ")";
                                     Position 0;
                                     RightOverhang (Number 0.0);
                                     RowAlign _RowAlign.Baseline;
                                     RowLines _RowLines.None;
                                     RowSpacing (EX 1.0<ex>);
                                     RowSpan 1u;
                                     RQuote "&quot;";
                                     RSpace (NamedLength ThickMathSpace);
                                     Selection 1u;
                                     Separator false;
                                     Separators ",";
                                     Shift 0;
                                     Side _Side.Right;
                                     StackAlign _StackAlign.DecimalPoint;
                                     Stretchy false;
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     Symmetric false;
                                     VAlign (EX 0.0<ex>);
                                     Width (KeyWord "automatic");
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Merror ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mpadded ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.6.2 Attributes
                                     Depth (KeyWord "same as content");                                   
                                     Height (KeyWord "same as content");
                                     Width (KeyWord "same as content");
                                     LSpace (EM 0.8<em>);
                                     VOffset (EM 0.8<em>);
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mphantom ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mfenced ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.8.2 Attributes
                                     Open ")";
                                     Close ")";
                                     Separators ",";
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Menclose ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.3.9.2 Attributes
                                     Notation LongDiv;
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msub ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.1.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.2.2 Attributes 
                                     SuperScriptShift (KeyWord "automatic");
                                     ]
        
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msubsup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.3.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Munde ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.4.2 Attributes
                                     AccentUnder false; //automatic
                                     Align _Align.Center;
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Mover ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.5.2 Attributes
                                     Accent false; //automatic
                                     Align _Align.Center;
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Munderover ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.6.2 Attributes
                                     Accent false; //automatic
                                     AccentUnder false; //automatic
                                     Align _Align.Center;
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Mmultiscripts ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //3.4.7.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtable ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mlabeledtr ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtr ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtd ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Maligngroup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Malignmark ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mstack ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mlongdiv ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msgroup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msrow ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mscarries ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mscarry ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msline ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Enlivening Maction ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor "black"; 
                                     MathBackground "transparent";

                                     //
                                     ]

            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)


        




(*module Attribute = 

    let accent d = match d with 
                   | true -> Accent true 
                   | false -> Accent false 
                   /// Default 
                   | _ -> Accent false 
    
    let dir d = match d with 
                | "ltr" -> Dir Ltr
                | "rtl" ->  Dir Rtl 
                /// Default 
                | _ -> Dir Ltr*)   
