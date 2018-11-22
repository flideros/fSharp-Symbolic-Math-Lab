namespace Math.Pure.Objects

type Function = 
    | Equals | GreaterThan | LessThan //Binary -> Relational
    | Sum | Plus | Positive // n-Ary, Binary, Unary -> Algebraic
    | Minus | Negative // Binary, Unary -> Algebraic
    | Product | Times // n-Ary, Binary -> Algebraic
    | Factorial
    | ToThePowerOf 
    | ExplicitToThePowerOf   
    | Sin | Cos | Tan
    | Sec | Csc | Cot
    | Sinh | Cosh | Tanh
    | Sech | Csch | Coth
    | ArcSin | ArcCos | ArcTan
    | ArcSec | ArcCsc | ArcCot
    | ArcSinh | ArcCosh | ArcTanh
    | ArcSech | ArcCsch | ArcCoth
    | Ln
    | Derivative
    | AntiDerivative
    | Abs
    | Exp
    | DividedBy
    

type Function with

    member this.Symbol =
        match this with 
        | Equals -> " = "
        | GreaterThan -> " > "
        | LessThan -> " < "
        | Sum -> " + "
        | Plus -> "+"
        | Positive -> "+"
        | Minus -> "-"
        | Negative -> "-"
        | Product -> "*"
        | Times -> "*"
        | Factorial -> "!"
        | ToThePowerOf -> "^"
        | Sin -> "Sin"
        | Cos -> "Cos"
        | Tan -> "Tan"
        | ArcTan -> "ArcTan"
        | _ -> ""  

