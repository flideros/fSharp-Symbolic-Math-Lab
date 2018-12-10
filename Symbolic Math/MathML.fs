namespace MathML

[<Measure>] type em // an em (font-relative unit traditionally used for horizontal lengths)
[<Measure>] type ex // an ex (font-relative unit traditionally used for vertical lengths)
[<Measure>] type px // pixels, or size of a pixel in the current display
[<Measure>] type inch // inches (1 inch = 2.54 centimeters)
[<Measure>] type cm // centimeters
[<Measure>] type mm // millimeters
[<Measure>] type pt // points (1 point = 1/72 inch)
[<Measure>] type pc // picas (1 pica = 12 points)
[<Measure>] type pct // percentage of the default value

type NamedSpace = 
    | VeryVeryThinMathSpace // 1/18em
    | VeryThinMathSpace // 2/18em
    | ThinMathSpace // 3/18em
    | MediumMathSpace // 4/18em
    | ThickMathSpace // 5/18em
    | VeryThickMathSpace // 6/18em
    | VeryVeryThickMathSpace // 7/18em
    | NegativeVeryVeryThinMathSpace // -1/18em
    | NegativeVeryThinMathSpace // -2/18em
    | NegativeThinMathSpace // -3/18em
    | NegativeMediumMathSpace // -4/18em
    | NegativeThickMathSpace // -5/18em
    | NegativeVeryThickMathSpace // -6/18em
    | NegativeVeryVeryThickMathSpace // -7/18em

type Length<[<Measure>] 'u> =    
    | Numer of float
    | EM of float<em>
    | EX of float<ex>
    | PX of float<px>
    | IN of float<inch>
    | CM of float<cm>
    | MM of float<mm>
    | PT of float<pt>
    | PC of float<pc>
    | Pct of float<pct>
    | NamedSpace of NamedSpace

type Color = 
    | RGB of int*int*int 
    | RRGGBB of string*string*string
    | Html of string
