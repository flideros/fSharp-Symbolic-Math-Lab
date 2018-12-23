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
[<Measure>] type boolean /// no measure
[<Measure>] type text /// no measure

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

type Length<[<Measure>] 'u> =    
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

type ActionType = | Toggle | Statusline | Tooltip | Input | Highlight
type Align = | Left | Right | Top | Bottom | Center | Baseline | Axis
type CharAlign = | Left | Right | Center
type CharSpacing<[<Measure>] 'u> = Length of Length<'u> | Left | Right | Center
type ColumnAlign = | Left | Right | Center
type ColumnLines = | None | Solid | Dashed
type ColumnWidth<[<Measure>] 'u> = | Length of Length<'u> | Auto | Fit
type Crossout = | None | Updiagonalstrike | Downdiagonalstrike | Verticalstrike | Horizontalstrike
type DenomAlign = | Left | Right | Center
type Dir = | Ltr | Rt
type Display = | Block | Inline
type Edge = | Left | Right
type Form = | Prefix | Infix | Postfix
type Frame = | None | Solid | Dashed
type GroupAlign = | Left | Center | Right | Decimalpoint
type IndentAlignFirst = | Left | Center | Right | Auto | Id | Indentalign
type IndentAlignLast = | Left | Center | Right | Auto | Id | Indentalign
type IndentAlign = | Left | Center | Right | Auto | Id
type InfixLineBreakStyle = | Before | After | Duplicate
type LineBreak = | Auto | Newline | Nobreak | Goodbreak | Badbreak
type LineBreakStyle = | Before | After | Duplicate | Infixlinebreakstyle
type Location = | W | NW | N | NE | E | SE | S | SW
type LongDivStyle = | LeftTop | StackedRightRight | MediumStackedRightRight | ShortStackedRightRight | RightTop | LeftRight1  | LeftRight2 | RightRight | StackedLeftLeft | StackedLeftLineTop
type MathVariant = | Normal | Bold | Italic | BoldItalic | DoubleStruck | BoldFraktur | Script | BoldScript | Fraktur | SansSerif | BoldSansSerif | SansSerifItalic | SansSerifBoldItalic | MonoSpace | Initial | Tailed | Looped | Stretched
type Notation = | LongDiv | Actuarial | PhasOrAngle | Radical | Box | RoundedBox | Circle | Left | Right | Top | Bottom | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike | NorthEastArrow | Madruwb | Text
type NumAlign = | Left | Right | Center
type Overflow = | Linebreak | Scroll | Elide | Truncate | Scale
type RowAlign = | Top | Bottom | Center | Baseline | Axis
type RowLines = | None | Solid | Dashed
type Side = | Left | Right | LeftOverlap | Rightoverlap
type StackAlign = | Left | Center | Right | Decimalpoint

type Attribute<[<Measure>] 'u> =
    private
    | Accent of bool
    | AccentUnder of bool
    | ActionType of ActionType
    | Align of Align
    | AlignmentScope of bool
    | Alt of Uri
    | AltImg of Uri
    | AltImgHeight of Length<'u>
    | AltImgValign of Length<'u>
    | AltImgWidth of Length<'u>
    | AltText of string // Alternate string
    | Bevelled of bool
    | CdGroup of Uri
    | CharAlign of CharAlign
    | CharSpacing of Length<'u>
    | Close of string //Specifies the closing delimiter.
    | ColumnAlign of ColumnAlign
    | ColumnLines of ColumnLines
    | ColumnSpacing of Length<'u>
    | ColumnSpan of uint32
    | ColumnWidth of Length<'u>
    | Crossout of Crossout
    | DecimalPoint of char
    | DenomAlign of DenomAlign
    | Depth of Length<'u>
    | Dir of Dir
    | Display of Display
    | DisplayStyle of bool
    | Edge of Edge
    | EqualColumns of bool
    | EqualRows of bool
    | Fence of bool
    | Form of Form
    | Frame of Frame
    | FrameSpacing of Length<'u>*Length<'u>
    | GroupAlign of GroupAlign
    | Height of Length<'u>
    | Href of Uri
    | Id of Id
    | IndentAlignFirst of IndentAlignFirst
    | IndentAlignLast of IndentAlignLast
    | IndentAlign of IndentAlign
    | IndentShift of Length<'u>
    | IndentShiftFirst of Length<'u>
    | IndentShiftLast of Length<'u>
    | IndentTarget of Idref
    | InfixLineBreakStyle of InfixLineBreakStyle
    | LargeOp of bool
    | LeftOverhang of Length<'u>
    | Length of uint32
    | LineBreak of LineBreak
    | LineBreakMultChar of LineBreakMultChar
    | LineBreakStyle of LineBreakStyle
    | LineLeading of Length<'u>
    | LineThickness of Length<'u>
    | Location of Location
    | LongDivStyle of LongDivStyle
    | LQuote of LQuote
    | LSpace of Length<'u>
    | MathBackground of Color
    | MathColor of Color
    | MathSize of Length<'u>
    | MathVariant of MathVariant
    | MaxSize of Length<'u>
    | MaxWidth of Length<'u>
    | MinLabelSpacing of Length<'u>
    | MinSize of Length<'u>
    | MovableLimits of bool
    | MsLineThickness of Length<'u>
    | Notation of Notation
    | NumAlign of NumAlign
    | Open of string // Specifies the opening delimiter.
    | Overflow of Overflow
    | Position of int
    | RightOverhang of Length<'u>
    | RowAlign of RowAlign
    | RowLines of RowLines
    | RowSpacing of Length<'u>
    | RowSpan of uint32
    | RQuote of RQuote
    | RSpace of Length<'u>
    | ScriptLevel of char*uint32
    | ScriptMinSize of Length<'u>
    | ScriptSizeMultiplier of float
    | Selection of uint32
    | Separator of bool
    | Separators of string //Specifies a sequence of zero or more separator characters, optionally separated by whitespace.
    | Shift of int
    | Side of Side
    | Src of Uri
    | StackAlign of StackAlign
    | Stretchy of bool
    | SubScriptShift of Length<'u>
    | SupScriptShift of Length<'u>
    | Symmetric of bool
    | VAlign of Length<'u>
    | VOffset of Length<'u>
    | Width of Length<'u>
    | Xmlns of Uri

module Mrow =
    
    type G = string
