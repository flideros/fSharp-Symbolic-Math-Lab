namespace Math.Pure.Objects

open OpenMath

open OpenMath

type Function = 

    | Abs // Implemented
    | And
    | ApplyToList
    | Approx
    | ArcCos // Implemented
    | ArcCosh
    | ArcCot // Implemented
    | ArcCoth
    | ArcCsc // Implemented
    | ArcCsch
    | ArcSec // Implemented
    | ArcSech
    | ArcSin // Implemented
    | ArcSinh
    | ArcTan // Implemented
    | ArcTanh
    | Argument
    | BasedFloat
    | BasedInteger
    | BigFloat
    | BigFloatPrec
    | CartesianProduct
    | MultiCartesianProduct
    | Ceiling
    | ComplexCartesian
    | ComplexPolar
    | Conjugate
    | Cos // Implemented
    | Cosh
    | Cot // Implemented
    | Coth
    | Csc // Implemented
    | Csch
    | Curl
    | Defint
    | Determinant
    | Diff
    | Divergence
    | DividedBy // Implemented
    | Domain
    | DomainOfApplication
    | Equals
    | Equivalent
    | Exp  // Implemented
    | Factorial
    | Factorof
    | Floor
    | Gcd
    | Geq
    | Grad
    | GreaterThan
    | Identity
    | Image
    | Imaginary
    | Implies
    | In
    | MultiIn
    | Int
    | IntegerInterval
    | Intersect // Implemented
    | MultiIntersect // Implemented
    | Interval
    | IntervalCC
    | IntervalCO
    | IntervalOC
    | IntervalOO
    | Inverse
    | Kernel
    | Laplacian
    | Lcm  
    | LeftCompose
    | LeftInverse
    | Leq
    | Limit
    | List
    | Ln // Implemented
    | Log
    | LessThan
    | ListMap
    | Map
    | Matrix
    | MatrixSelector
    | Matrixrow
    | Max
    | DataMean
    | Mean
    | Median
    | Min
    | Minus // Implemented
    | Mode
    | DataMoment
    | Moment
    | Multiset
    | Nand
    | Neq
    | Nor
    | Not
    | MultiNotin
    | Notin
    | MultiNotProperSubset
    | Notprsubset
    | MultiNotSubset
    | Notsubset
    | NthDiff
    | Or
    | OrientedInterval
    | Otherwise
    | Outerproduct
    | PartialDiff
    | PartialDiffDegree
    | Piece
    | Piecewise
    | Plus // Implemented
    | ExplicitToThePowerOf
    | ToThePowerOf // Implemented
    | PredicateOnList
    | Product  // Implemented
    | MultiProperSubset
    | Prsubset
    | Quotient
    | Range
    | RealPart
    | Remainder
    | Restriction
    | RightCompose
    | RightIinverse
    | Root
    | Round
    | Scalarproduct
    | DataSdev
    | Sdev
    | Sec
    | Sech
    | Set
    | MultiSetdiff
    | Setdiff // Implemented
    | Sin // Implemented
    | Sinh
    | MultiSize
    | Size
    | MultiSubset
    | Subset
    | ListSuchthat
    | SuchThat
    | Sum // Implemented
    | Tan // Implemented
    | Tanh
    | Times // Implemented
    | Transpose
    | Trunc
    | UnaryMinus
    | MultiUnion // Implemented
    | Union // Implemented
    | DataVariance
    | Variance
    | Vector
    | Vectorselector
    | Vectorproduct
    | Xnor
    | Xor
    | AntiDerivative 
    | Derivative // Implemented
    | FractionType // Implemented

    with
    member this.definition =
        match this with
        | Abs -> GET.definitionEntry "abs" (FROM.cD "arith1")
        | And -> GET.definitionEntry "and" (FROM.cD "logic1")
        | ApplyToList -> GET.definitionEntry "apply_to_list" (FROM.cD "fns2")
        | Approx -> GET.definitionEntry "approx" (FROM.cD "relation1")
        | ArcCos -> GET.definitionEntry "arccos" (FROM.cD "transc1")
        | ArcCosh -> GET.definitionEntry "arccosh" (FROM.cD "transc1")
        | ArcCot -> GET.definitionEntry "arccot" (FROM.cD "transc1")
        | ArcCoth -> GET.definitionEntry "arccoth" (FROM.cD "transc1")
        | ArcCsc -> GET.definitionEntry "arccsc" (FROM.cD "transc1")
        | ArcCsch -> GET.definitionEntry "arccsch" (FROM.cD "transc1")
        | ArcSec -> GET.definitionEntry "arcsec" (FROM.cD "transc1")
        | ArcSech -> GET.definitionEntry "arcsech" (FROM.cD "transc1")
        | ArcSin -> GET.definitionEntry "arcsin" (FROM.cD "transc1")
        | ArcSinh -> GET.definitionEntry "arcsinh" (FROM.cD "transc1")
        | ArcTan -> GET.definitionEntry "arctan" (FROM.cD "transc1")
        | ArcTanh -> GET.definitionEntry "arctanh" (FROM.cD "transc1")
        | Argument -> GET.definitionEntry "argument" (FROM.cD "complex1")
        | BasedFloat -> GET.definitionEntry "based_float" (FROM.cD "nums1")
        | BasedInteger -> GET.definitionEntry "based_integer" (FROM.cD "nums1")
        | BigFloat -> GET.definitionEntry "bigfloat" (FROM.cD "bigfloat1")
        | BigFloatPrec -> GET.definitionEntry "bigfloatprec" (FROM.cD "bigfloat1")
        | CartesianProduct -> GET.definitionEntry "cartesian_product" (FROM.cD "set1")
        | MultiCartesianProduct -> GET.definitionEntry "cartesian_product" (FROM.cD "multiset1")
        | Ceiling -> GET.definitionEntry "ceiling" (FROM.cD "rounding1")
        | ComplexCartesian -> GET.definitionEntry "complex_cartesian" (FROM.cD "complex1")
        | ComplexPolar -> GET.definitionEntry "complex_polar" (FROM.cD "complex1")
        | Conjugate -> GET.definitionEntry "conjugate" (FROM.cD "complex1")
        | Cos -> GET.definitionEntry "cos" (FROM.cD "transc1")
        | Cosh -> GET.definitionEntry "cosh" (FROM.cD "transc1")
        | Cot -> GET.definitionEntry "cot" (FROM.cD "transc1")
        | Coth -> GET.definitionEntry "coth" (FROM.cD "transc1")
        | Csc -> GET.definitionEntry "csc" (FROM.cD "transc1")
        | Csch -> GET.definitionEntry "csch" (FROM.cD "transc1")
        | Curl -> GET.definitionEntry "curl" (FROM.cD "veccalc1")
        | Defint -> GET.definitionEntry "defint" (FROM.cD "calculus1")
        | Determinant -> GET.definitionEntry "determinant" (FROM.cD "linalg1")
        | Diff -> GET.definitionEntry "diff" (FROM.cD "calculus1")
        | Divergence -> GET.definitionEntry "divergence" (FROM.cD "veccalc1")
        | DividedBy -> GET.definitionEntry "divide" (FROM.cD "arith1")
        | Domain -> GET.definitionEntry "domain" (FROM.cD "fns1")
        | DomainOfApplication -> GET.definitionEntry "domainofapplication" (FROM.cD "fns1")
        | Equals -> GET.definitionEntry "eq" (FROM.cD "relation1")
        | Equivalent -> GET.definitionEntry "equivalent" (FROM.cD "logic1")
        | Exp -> GET.definitionEntry "exp" (FROM.cD "transc1")
        | Factorial -> GET.definitionEntry "factorial" (FROM.cD "integer1")
        | Factorof -> GET.definitionEntry "factorof" (FROM.cD "integer1")
        | Floor -> GET.definitionEntry "floor" (FROM.cD "rounding1")
        | Gcd -> GET.definitionEntry "gcd" (FROM.cD "arith1")
        | Geq -> GET.definitionEntry "geq" (FROM.cD "relation1")
        | Grad -> GET.definitionEntry "grad" (FROM.cD "veccalc1")
        | GreaterThan -> GET.definitionEntry "gt" (FROM.cD "relation1")
        | Identity -> GET.definitionEntry "identity" (FROM.cD "fns1")
        | Image -> GET.definitionEntry "image" (FROM.cD "fns1")
        | Imaginary -> GET.definitionEntry "imaginary" (FROM.cD "complex1")
        | Implies -> GET.definitionEntry "implies" (FROM.cD "logic1")
        | In -> GET.definitionEntry "in" (FROM.cD "set1")
        | MultiIn -> GET.definitionEntry "in" (FROM.cD "multiset1")
        | Int -> GET.definitionEntry "int" (FROM.cD "calculus1")
        | IntegerInterval -> GET.definitionEntry "integer_interval" (FROM.cD "interval1")
        | Intersect -> GET.definitionEntry "intersect" (FROM.cD "set1")
        | MultiIntersect -> GET.definitionEntry "intersect" (FROM.cD "multiset1")
        | Interval -> GET.definitionEntry "interval" (FROM.cD "interval1")
        | IntervalCC -> GET.definitionEntry "interval_cc" (FROM.cD "interval1")
        | IntervalCO -> GET.definitionEntry "interval_co" (FROM.cD "interval1")
        | IntervalOC -> GET.definitionEntry "interval_oc" (FROM.cD "interval1")
        | IntervalOO -> GET.definitionEntry "interval_oo" (FROM.cD "interval1")
        | Inverse -> GET.definitionEntry "inverse" (FROM.cD "fns1")
        | Kernel -> GET.definitionEntry "kernel" (FROM.cD "fns2")
        | Laplacian -> GET.definitionEntry "Laplacian" (FROM.cD "veccalc1")
        | Lcm -> GET.definitionEntry "lcm" (FROM.cD "arith1")
        | LeftCompose -> GET.definitionEntry "left_compose" (FROM.cD "fns1")
        | LeftInverse -> GET.definitionEntry "left_inverse" (FROM.cD "fns1")
        | Leq -> GET.definitionEntry "leq" (FROM.cD "relation1")
        | Limit -> GET.definitionEntry "limit" (FROM.cD "limit1")
        | List -> GET.definitionEntry "list" (FROM.cD "list1")
        | Ln -> GET.definitionEntry "ln" (FROM.cD "transc1")
        | Log -> GET.definitionEntry "log" (FROM.cD "transc1")
        | LessThan -> GET.definitionEntry "lt" (FROM.cD "relation1")
        | ListMap -> GET.definitionEntry "map" (FROM.cD "list1")
        | Map -> GET.definitionEntry "map" (FROM.cD "set1")
        | Matrix -> GET.definitionEntry "matrix" (FROM.cD "linalg2")
        | MatrixSelector -> GET.definitionEntry "matrix_selector" (FROM.cD "linalg1")
        | Matrixrow -> GET.definitionEntry "matrixrow" (FROM.cD "linalg2")
        | Max -> GET.definitionEntry "max" (FROM.cD "minmax1")
        | DataMean -> GET.definitionEntry "mean" (FROM.cD "s_data1")
        | Mean -> GET.definitionEntry "mean" (FROM.cD "s_dist1")
        | Median -> GET.definitionEntry "median" (FROM.cD "s_data1")
        | Min -> GET.definitionEntry "min" (FROM.cD "minmax1")
        | Minus -> GET.definitionEntry "minus" (FROM.cD "arith1")
        | Mode -> GET.definitionEntry "mode" (FROM.cD "s_data1")
        | DataMoment -> GET.definitionEntry "moment" (FROM.cD "s_data1")
        | Moment -> GET.definitionEntry "moment" (FROM.cD "s_dist1")
        | Multiset -> GET.definitionEntry "multiset" (FROM.cD "multiset1")
        | Nand -> GET.definitionEntry "nand" (FROM.cD "logic1")
        | Neq -> GET.definitionEntry "neq" (FROM.cD "relation1")
        | Nor -> GET.definitionEntry "nor" (FROM.cD "logic1")
        | Not -> GET.definitionEntry "not" (FROM.cD "logic1")
        | MultiNotin -> GET.definitionEntry "notin" (FROM.cD "multiset1")
        | Notin -> GET.definitionEntry "notin" (FROM.cD "set1")
        | MultiNotProperSubset -> GET.definitionEntry "notprsubset" (FROM.cD "multiset1")
        | Notprsubset -> GET.definitionEntry "notprsubset" (FROM.cD "set1")
        | MultiNotSubset -> GET.definitionEntry "notsubset" (FROM.cD "multiset1")
        | Notsubset -> GET.definitionEntry "notsubset" (FROM.cD "set1")
        | NthDiff -> GET.definitionEntry "nthdiff" (FROM.cD "calculus1")
        | Or -> GET.definitionEntry "or" (FROM.cD "logic1")
        | OrientedInterval -> GET.definitionEntry "oriented_interval" (FROM.cD "interval1")
        | Otherwise -> GET.definitionEntry "otherwise" (FROM.cD "piece1")
        | Outerproduct -> GET.definitionEntry "outerproduct" (FROM.cD "linalg1")
        | PartialDiff -> GET.definitionEntry "partialdiff" (FROM.cD "calculus1")
        | PartialDiffDegree -> GET.definitionEntry "partialdiffdegree" (FROM.cD "calculus1")
        | Piece -> GET.definitionEntry "piece" (FROM.cD "piece1")
        | Piecewise -> GET.definitionEntry "piecewise" (FROM.cD "piece1")
        | Plus -> GET.definitionEntry "plus" (FROM.cD "arith1")
        | ExplicitToThePowerOf -> GET.definitionEntry "power" (FROM.cD "arith1")
        | ToThePowerOf -> GET.definitionEntry "power" (FROM.cD "arith1")
        | PredicateOnList -> GET.definitionEntry "predicate_on_list" (FROM.cD "fns2")
        | Product -> GET.definitionEntry "product" (FROM.cD "arith1")
        | MultiProperSubset -> GET.definitionEntry "prsubset" (FROM.cD "multiset1")
        | Prsubset -> GET.definitionEntry "prsubset" (FROM.cD "set1")
        | Quotient -> GET.definitionEntry "quotient" (FROM.cD "integer1")
        | Range -> GET.definitionEntry "range" (FROM.cD "fns1")
        | RealPart -> GET.definitionEntry "real" (FROM.cD "complex1")
        | Remainder -> GET.definitionEntry "remainder" (FROM.cD "integer1")
        | Restriction -> GET.definitionEntry "restriction" (FROM.cD "fns1")
        | RightCompose -> GET.definitionEntry "right_compose" (FROM.cD "fns2")
        | RightIinverse -> GET.definitionEntry "right_inverse" (FROM.cD "fns1")
        | Root -> GET.definitionEntry "root" (FROM.cD "arith1")
        | Round -> GET.definitionEntry "round" (FROM.cD "rounding1")
        | Scalarproduct -> GET.definitionEntry "scalarproduct" (FROM.cD "linalg1")
        | DataSdev -> GET.definitionEntry "sdev" (FROM.cD "s_data1")
        | Sdev -> GET.definitionEntry "sdev" (FROM.cD "s_dist1")
        | Sec -> GET.definitionEntry "sec" (FROM.cD "transc1")
        | Sech -> GET.definitionEntry "sech" (FROM.cD "transc1")
        | Set -> GET.definitionEntry "set" (FROM.cD "set1")
        | MultiSetdiff -> GET.definitionEntry "setdiff" (FROM.cD "multiset1")
        | Setdiff -> GET.definitionEntry "setdiff" (FROM.cD "set1")
        | Sin -> GET.definitionEntry "sin" (FROM.cD "transc1")
        | Sinh -> GET.definitionEntry "sinh" (FROM.cD "transc1")
        | MultiSize -> GET.definitionEntry "size" (FROM.cD "multiset1")
        | Size -> GET.definitionEntry "size" (FROM.cD "set1")
        | MultiSubset -> GET.definitionEntry "subset" (FROM.cD "multiset1")
        | Subset -> GET.definitionEntry "subset" (FROM.cD "set1")
        | ListSuchthat -> GET.definitionEntry "suchthat" (FROM.cD "list1")
        | SuchThat -> GET.definitionEntry "suchthat" (FROM.cD "set1")
        | Sum -> GET.definitionEntry "sum" (FROM.cD "arith1")
        | Tan -> GET.definitionEntry "tan" (FROM.cD "transc1")
        | Tanh -> GET.definitionEntry "tanh" (FROM.cD "transc1")
        | Times -> GET.definitionEntry "times" (FROM.cD "arith1")
        | Transpose -> GET.definitionEntry "transpose" (FROM.cD "linalg1")
        | Trunc -> GET.definitionEntry "trunc" (FROM.cD "rounding1")
        | UnaryMinus -> GET.definitionEntry "unary_minus" (FROM.cD "arith1")
        | MultiUnion -> GET.definitionEntry "union" (FROM.cD "multiset1")
        | Union -> GET.definitionEntry "union" (FROM.cD "set1")
        | DataVariance -> GET.definitionEntry "variance" (FROM.cD "s_data1")
        | Variance -> GET.definitionEntry "variance" (FROM.cD "s_dist1")
        | Vector -> GET.definitionEntry "vector" (FROM.cD "linalg2")
        | Vectorselector -> GET.definitionEntry "vector_selector" (FROM.cD "linalg1")
        | Vectorproduct -> GET.definitionEntry "vectorproduct" (FROM.cD "linalg1")
        | Xnor -> GET.definitionEntry "xnor" (FROM.cD "logic1")
        | Xor -> GET.definitionEntry "xor" (FROM.cD "logic1")
        | FractionType -> GET.definitionEntry "rational)" (FROM.cD "nums1")
        | _ -> None
 
module Function =
    
    let defenition = "TODO"

