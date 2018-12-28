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
//[<Measure>] type boolean /// no measure
//[<Measure>] type text /// no measure

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

type Length = //<[<Measure>] 'u> =    
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
type Id = string
type Uri = string
type LineBreakMultChar = string /// hex character code
type LQuote = string /// character code
type RQuote = string /// character code

type ActionType = | Toggle | StatusLine | ToolTip | Input | Highlight
type Align = | Left | Right | Top | Bottom | Center | Baseline | Axis
type CharAlign = | Left | Right | Center
type CharSpacing = Length of Length | Loose | Medium | Tight
type _ColumnAlign = | Left | Right | Center
type ColumnLines = | None | Solid | Dashed
type ColumnWidth = | Length of Length | Auto | Fit
type Crossout = | None | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike
type DenomAlign = | Left | Right | Center
type _Dir = | Ltr | Rtl
type Display = | Block | Inline
type Edge = | Left | Right
type Form = | Prefix | Infix | Postfix
type Frame = | None | Solid | Dashed
type GroupAlign = | Left | Center | Right | DecimalPoint
type IndentAlign = | Left | Center | Right | Auto | Id
type IndentAlignFirst = | Left | Center | Right | Auto | Id | IndentAlign
type IndentAlignLast = | Left | Center | Right | Auto | Id | IndentAlign
type InfixLineBreakStyle = | Before | After | Duplicate
type LineBreak = | Auto | NewLine | NoBreak | GoodBreak | BadBreak
type LineBreakStyle = | Before | After | Duplicate | InfixLinebBreakStyle
type Location = | W | NW | N | NE | E | SE | S | SW
type LongDivStyle = | LeftTop | StackedRightRight | MediumStackedRightRight | ShortStackedRightRight | RightTop | LeftRight1  | LeftRight2 | RightRight | StackedLeftLeft | StackedLeftLineTop
type MathVariant = | Normal | Bold | Italic | BoldItalic | DoubleStruck | BoldFraktur | Script | BoldScript | Fraktur | SansSerif | BoldSansSerif | SansSerifItalic | SansSerifBoldItalic | MonoSpace | Initial | Tailed | Looped | Stretched
type Notation = | LongDiv | Actuarial | PhasOrAngle | Radical | Box | RoundedBox | Circle | Left | Right | Top | Bottom | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike | NorthEastArrow | Madruwb | Text
type NumAlign = | Left | Right | Center
type Overflow = | Linebreak | Scroll | Elide | Truncate | Scale
type RowAlign = | Top | Bottom | Center | Baseline | Axis
type RowLines = | None | Solid | Dashed
type Side = | Left | Right | LeftOverlap | RightOerlap
type StackAlign = | Left | Center | Right | DecimalPoint

type MLAttribute = //<[<Measure>] 'u> =
    //private
    | Accent of bool
    | AccentUnder of bool
    | ActionType of ActionType
    | Align of Align
    | AlignmentScope of bool
    | Alt of Uri
    | AltImg of Uri
    | AltImgHeight of Length
    | AltImgValign of Length
    | AltImgWidth of Length
    | AltText of string // Alternate string
    | Bevelled of bool
    | CdGroup of Uri
    | CharAlign of CharAlign
    | CharSpacing of Length
    | Close of string //Specifies the closing delimiter.
    | ColumnAlign of _ColumnAlign
    | ColumnLines of ColumnLines
    | ColumnSpacing of Length
    | ColumnSpan of uint32
    | ColumnWidth of Length
    | Crossout of Crossout
    | DecimalPoint of char
    | DenomAlign of DenomAlign
    | Depth of Length
    | Dir of _Dir
    | Display of Display
    | DisplayStyle of bool
    | Edge of Edge
    | EqualColumns of bool
    | EqualRows of bool
    | Fence of bool
    | Form of Form
    | Frame of Frame
    | FrameSpacing of Length*Length
    | GroupAlign of GroupAlign
    | Height of Length
    | Href of Uri
    | Id of Id
    | IndentAlignFirst of IndentAlignFirst
    | IndentAlignLast of IndentAlignLast
    | IndentAlign of IndentAlign
    | IndentShift of Length
    | IndentShiftFirst of Length
    | IndentShiftLast of Length
    | IndentTarget of Idref
    | InfixLineBreakStyle of InfixLineBreakStyle
    | LargeOp of bool
    | LeftOverhang of Length
    | LineBreak of LineBreak
    | LineBreakMultChar of LineBreakMultChar
    | LineBreakStyle of LineBreakStyle
    | LineLeading of Length
    | LineThickness of Length
    | Location of Location
    | LongDivStyle of LongDivStyle
    | LQuote of LQuote
    | LSpace of Length
    | MathBackground of Color
    | MathColor of Color
    | MathSize of Length
    | MathVariant of MathVariant
    | MaxSize of Length
    | MaxWidth of Length
    | MinLabelSpacing of Length
    | MinSize of Length
    | MovableLimits of bool
    | MsLineThickness of Length
    | Notation of Notation
    | NumAlign of NumAlign
    | Open of string // Specifies the opening delimiter.
    | Overflow of Overflow
    | Position of int
    | RightOverhang of Length
    | RowAlign of RowAlign
    | RowLines of RowLines
    | RowSpacing of Length
    | RowSpan of uint32
    | RQuote of RQuote
    | RSpace of Length
    | ScriptLevel of char*uint32
    | ScriptMinSize of Length
    | ScriptSizeMultiplier of float
    | Selection of uint32
    | Separator of bool
    | Separators of string //Specifies a sequence of zero or more separator characters, optionally separated by whitespace.
    | Shift of int
    | Side of Side
    | Src of Uri
    | StackAlign of StackAlign
    | Stretchy of bool
    | SubScriptShift of Length
    | SupScriptShift of Length
    | Symmetric of bool
    | VAlign of Length
    | VOffset of Length
    | Width of Length
    | Xmlns of Uri

type TokenElement = | Mi | Mn | Mo | Mtext | Mspace | Ms | Mglyph
type GeneralLayoutElement = | Mrow | Mfrac | Msqrt | Mroot | Mstyle | Merror | Mpadded | Mphantom | Mfenced | Menclose
type ScriptElement = | Msub | Msup| Msubsup | Munde | Mover | Munderover | Mmultiscripts
type TableElement = | Mtable | Mlabeledtr | Mtr | Mtd | Maligngroup | Malignmark
type MathLayoutElement = | Mstack | Mlongdiv | Msgroup | Msrow | Mscarries  | Mscarry | Msline
type EnliveningExpressionElement = | Maction

type Element = | Token of TokenElement | General of GeneralLayoutElement | Script of ScriptElement | Table of TableElement | Math of MathLayoutElement | Enlivening of EnliveningExpressionElement

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

module Element =

    let element (attr : MLAttribute list) (args : Element list) = (attr, args)




//module mi =
//module mn =
//module mo =
//module mtext =
//module mspace =
//module ms =
//module mglyph =

//module mrow =
//module mfrac =
//module msqrt =
//module mroot =
//module mstyle =
//module merror =
//module mpadded =
//module mphantom =
//module mfenced =
//module menclose =

//module msub =
//module msup =
//module msubsup =
//module munder =
//module mover =
//module munderover =
//module mmultiscripts =

//module mtable =
//module mlabeledtr =
//module mtr =
//module mtd =
//module maligngroup =
//module malignmark =

//module mstack =
//module mlongdiv =
//module msgroup =
//module msrow =
//module mscarries =
//module mscarry =
//module msline =

//module maction =

//module math =

    