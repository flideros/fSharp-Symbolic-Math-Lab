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

Type NamedSpace = 
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
    | KeyWord of string

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
type _CharSpacing = Length of Length | Loose | Medium | Tight
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
    | SupScriptShift of Length
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
            let defaultAttributes = [Display Inline;
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
                                     
                                     (* TODO *
                                        accent
                                        accentunder
                                        align
                                        alignmentscope
                                        bevelled
                                        charalign
                                        charspacing
                                        close
                                        columnalign
                                        columnlines
                                        columnspacing
                                        columnspan
                                        columnwidth
                                        crossout
                                        denomalign
                                        depth
                                        edge
                                        equalcolumns
                                        equalrows
                                        fence
                                        form
                                        frame
                                        framespacing
                                        groupalign
                                        height
                                        indentalign
                                        indentalignfirst
                                        indentalignlast
                                        indentshift
                                        indentshiftfirst
                                        indentshiftlast
                                        indenttarget
                                        largeop
                                        leftoverhang
                                        length
                                        linebreak
                                        linebreakmultchar
                                        linebreakstyle
                                        lineleading
                                        linethickness
                                        location
                                        longdivstyle
                                        lquote
                                        lspace
                                        maxsize
                                        minlabelspacing
                                        minsize
                                        movablelimits
                                        mslinethickness
                                        notation
                                        numalign
                                        open
                                        position
                                        rightoverhang
                                        rowalign
                                        rowlines
                                        rowspacing
                                        rowspan
                                        rquote
                                        rspace
                                        selection
                                        separator
                                        separators
                                        shift
                                        side
                                        stackalign
                                        stretchy
                                        subscriptshift
                                        superscriptshift
                                        symmetric
                                        valign
                                        width
                                     *)
                                     ]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)

        | Token Mi -> 
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mn ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mo ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mtext ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mspace ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Ms ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Token Mglyph ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mrow ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mfrac ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Msqrt ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mroot ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mstyle ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Merror ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mpadded ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mphantom ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Mfenced ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | GeneralLayout Menclose ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msub ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msup ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Msubsup ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Munde ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Mover ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Munderover ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Script Mmultiscripts ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtable ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mlabeledtr ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtr ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Mtd ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Maligngroup ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Table Malignmark ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mstack ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mlongdiv ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msgroup ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msrow ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mscarries ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Mscarry ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | MathLayout Msline ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
            let attr' = scrubAttributes attr defaultAttributes
            (elem, attr', args)
        
        | Enlivening Maction ->
            let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
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
