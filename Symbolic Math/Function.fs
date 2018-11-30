namespace Math.Pure.Objects

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
    | Minus
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

    member this.symbol =
        match this with 
        | Abs -> (GET.definitionEntry "abs" (FROM.cD "arith1")).Value.Name
        | And -> (GET.definitionEntry "and" (FROM.cD "logic1")).Value.Name
        | ApplyToList -> (GET.definitionEntry "apply_to_list" (FROM.cD "fns2")).Value.Name
        | Approx -> (GET.definitionEntry "approx" (FROM.cD "relation1")).Value.Name
        | ArcCos -> (GET.definitionEntry "arccos" (FROM.cD "transc1")).Value.Name
        | ArcCosh -> (GET.definitionEntry "arccosh" (FROM.cD "transc1")).Value.Name
        | ArcCot -> (GET.definitionEntry "arccot" (FROM.cD "transc1")).Value.Name
        | ArcCoth -> (GET.definitionEntry "arccoth" (FROM.cD "transc1")).Value.Name
        | ArcCsc -> (GET.definitionEntry "arccsc" (FROM.cD "transc1")).Value.Name
        | ArcCsch -> (GET.definitionEntry "arccsch" (FROM.cD "transc1")).Value.Name
        | ArcSec -> (GET.definitionEntry "arcsec" (FROM.cD "transc1")).Value.Name
        | ArcSech -> (GET.definitionEntry "arcsech" (FROM.cD "transc1")).Value.Name
        | ArcSin -> (GET.definitionEntry "arcsin" (FROM.cD "transc1")).Value.Name
        | ArcSinh -> (GET.definitionEntry "arcsinh" (FROM.cD "transc1")).Value.Name
        | ArcTan -> (GET.definitionEntry "arctan" (FROM.cD "transc1")).Value.Name
        | ArcTanh -> (GET.definitionEntry "arctanh" (FROM.cD "transc1")).Value.Name
        | Argument -> (GET.definitionEntry "argument" (FROM.cD "complex1")).Value.Name
        | BasedFloat -> (GET.definitionEntry "based_float" (FROM.cD "nums1")).Value.Name
        | BasedInteger -> (GET.definitionEntry "based_integer" (FROM.cD "nums1")).Value.Name
        | BigFloat -> (GET.definitionEntry "bigfloat" (FROM.cD "bigfloat1")).Value.Name
        | BigFloatPrec -> (GET.definitionEntry "bigfloatprec" (FROM.cD "bigfloat1")).Value.Name
        | CartesianProduct -> (GET.definitionEntry "cartesian_product" (FROM.cD "set1")).Value.Name
        | MultiCartesianProduct -> (GET.definitionEntry "cartesian_product" (FROM.cD "multiset1")).Value.Name
        | Ceiling -> (GET.definitionEntry "ceiling" (FROM.cD "rounding1")).Value.Name
        | ComplexCartesian -> (GET.definitionEntry "complex_cartesian" (FROM.cD "complex1")).Value.Name
        | ComplexPolar -> (GET.definitionEntry "complex_polar" (FROM.cD "complex1")).Value.Name
        | Conjugate -> (GET.definitionEntry "conjugate" (FROM.cD "complex1")).Value.Name
        | Cos -> (GET.definitionEntry "cos" (FROM.cD "transc1")).Value.Name
        | Cosh -> (GET.definitionEntry "cosh" (FROM.cD "transc1")).Value.Name
        | Cot -> (GET.definitionEntry "cot" (FROM.cD "transc1")).Value.Name
        | Coth -> (GET.definitionEntry "coth" (FROM.cD "transc1")).Value.Name
        | Csc -> (GET.definitionEntry "csc" (FROM.cD "transc1")).Value.Name
        | Csch -> (GET.definitionEntry "csch" (FROM.cD "transc1")).Value.Name
        | Curl -> (GET.definitionEntry "curl" (FROM.cD "veccalc1")).Value.Name
        | Defint -> (GET.definitionEntry "defint" (FROM.cD "calculus1")).Value.Name
        | Determinant -> (GET.definitionEntry "determinant" (FROM.cD "linalg1")).Value.Name
        | Diff -> (GET.definitionEntry "diff" (FROM.cD "calculus1")).Value.Name
        | Divergence -> (GET.definitionEntry "divergence" (FROM.cD "veccalc1")).Value.Name
        | DividedBy -> (GET.definitionEntry "divide" (FROM.cD "arith1")).Value.Name
        | Domain -> (GET.definitionEntry "domain" (FROM.cD "fns1")).Value.Name
        | DomainOfApplication -> (GET.definitionEntry "domainofapplication" (FROM.cD "fns1")).Value.Name
        | Equals -> (GET.definitionEntry "eq" (FROM.cD "relation1")).Value.Name
        | Equivalent -> (GET.definitionEntry "equivalent" (FROM.cD "logic1")).Value.Name
        | Exp -> (GET.definitionEntry "exp" (FROM.cD "transc1")).Value.Name
        | Factorial -> (GET.definitionEntry "factorial" (FROM.cD "integer1")).Value.Name
        | Factorof -> (GET.definitionEntry "factorof" (FROM.cD "integer1")).Value.Name
        | Floor -> (GET.definitionEntry "floor" (FROM.cD "rounding1")).Value.Name
        | Gcd -> (GET.definitionEntry "gcd" (FROM.cD "arith1")).Value.Name
        | Geq -> (GET.definitionEntry "geq" (FROM.cD "relation1")).Value.Name
        | Grad -> (GET.definitionEntry "grad" (FROM.cD "veccalc1")).Value.Name
        | GreaterThan -> (GET.definitionEntry "gt" (FROM.cD "relation1")).Value.Name
        | Identity -> (GET.definitionEntry "identity" (FROM.cD "fns1")).Value.Name
        | Image -> (GET.definitionEntry "image" (FROM.cD "fns1")).Value.Name
        | Imaginary -> (GET.definitionEntry "imaginary" (FROM.cD "complex1")).Value.Name
        | Implies -> (GET.definitionEntry "implies" (FROM.cD "logic1")).Value.Name
        | In -> (GET.definitionEntry "in" (FROM.cD "set1")).Value.Name
        | MultiIn -> (GET.definitionEntry "in" (FROM.cD "multiset1")).Value.Name
        | Int -> (GET.definitionEntry "int" (FROM.cD "calculus1")).Value.Name
        | IntegerInterval -> (GET.definitionEntry "integer_interval" (FROM.cD "interval1")).Value.Name
        | Intersect -> (GET.definitionEntry "intersect" (FROM.cD "set1")).Value.Name
        | MultiIntersect -> (GET.definitionEntry "intersect" (FROM.cD "multiset1")).Value.Name
        | Interval -> (GET.definitionEntry "interval" (FROM.cD "interval1")).Value.Name
        | IntervalCC -> (GET.definitionEntry "interval_cc" (FROM.cD "interval1")).Value.Name
        | IntervalCO -> (GET.definitionEntry "interval_co" (FROM.cD "interval1")).Value.Name
        | IntervalOC -> (GET.definitionEntry "interval_oc" (FROM.cD "interval1")).Value.Name
        | IntervalOO -> (GET.definitionEntry "interval_oo" (FROM.cD "interval1")).Value.Name
        | Inverse -> (GET.definitionEntry "inverse" (FROM.cD "fns1")).Value.Name
        | Kernel -> (GET.definitionEntry "kernel" (FROM.cD "fns2")).Value.Name
        | Laplacian -> (GET.definitionEntry "Laplacian" (FROM.cD "veccalc1")).Value.Name
        | Lcm -> (GET.definitionEntry "lcm" (FROM.cD "arith1")).Value.Name
        | LeftCompose -> (GET.definitionEntry "left_compose" (FROM.cD "fns1")).Value.Name
        | LeftInverse -> (GET.definitionEntry "left_inverse" (FROM.cD "fns1")).Value.Name
        | Leq -> (GET.definitionEntry "leq" (FROM.cD "relation1")).Value.Name
        | Limit -> (GET.definitionEntry "limit" (FROM.cD "limit1")).Value.Name
        | List -> (GET.definitionEntry "list" (FROM.cD "list1")).Value.Name
        | Ln -> (GET.definitionEntry "ln" (FROM.cD "transc1")).Value.Name
        | Log -> (GET.definitionEntry "log" (FROM.cD "transc1")).Value.Name
        | LessThan -> (GET.definitionEntry "lt" (FROM.cD "relation1")).Value.Name
        | ListMap -> (GET.definitionEntry "map" (FROM.cD "list1")).Value.Name
        | Map -> (GET.definitionEntry "map" (FROM.cD "set1")).Value.Name
        | Matrix -> (GET.definitionEntry "matrix" (FROM.cD "linalg2")).Value.Name
        | MatrixSelector -> (GET.definitionEntry "matrix_selector" (FROM.cD "linalg1")).Value.Name
        | Matrixrow -> (GET.definitionEntry "matrixrow" (FROM.cD "linalg2")).Value.Name
        | Max -> (GET.definitionEntry "max" (FROM.cD "minmax1")).Value.Name
        | DataMean -> (GET.definitionEntry "mean" (FROM.cD "s_data1")).Value.Name
        | Mean -> (GET.definitionEntry "mean" (FROM.cD "s_dist1")).Value.Name
        | Median -> (GET.definitionEntry "median" (FROM.cD "s_data1")).Value.Name
        | Min -> (GET.definitionEntry "min" (FROM.cD "minmax1")).Value.Name
        | Minus -> (GET.definitionEntry "minus" (FROM.cD "arith1")).Value.Name
        | Mode -> (GET.definitionEntry "mode" (FROM.cD "s_data1")).Value.Name
        | DataMoment -> (GET.definitionEntry "moment" (FROM.cD "s_data1")).Value.Name
        | Moment -> (GET.definitionEntry "moment" (FROM.cD "s_dist1")).Value.Name
        | Multiset -> (GET.definitionEntry "multiset" (FROM.cD "multiset1")).Value.Name
        | Nand -> (GET.definitionEntry "nand" (FROM.cD "logic1")).Value.Name
        | Neq -> (GET.definitionEntry "neq" (FROM.cD "relation1")).Value.Name
        | Nor -> (GET.definitionEntry "nor" (FROM.cD "logic1")).Value.Name
        | Not -> (GET.definitionEntry "not" (FROM.cD "logic1")).Value.Name
        | MultiNotin -> (GET.definitionEntry "notin" (FROM.cD "multiset1")).Value.Name
        | Notin -> (GET.definitionEntry "notin" (FROM.cD "set1")).Value.Name
        | MultiNotProperSubset -> (GET.definitionEntry "notprsubset" (FROM.cD "multiset1")).Value.Name
        | Notprsubset -> (GET.definitionEntry "notprsubset" (FROM.cD "set1")).Value.Name
        | MultiNotSubset -> (GET.definitionEntry "notsubset" (FROM.cD "multiset1")).Value.Name
        | Notsubset -> (GET.definitionEntry "notsubset" (FROM.cD "set1")).Value.Name
        | NthDiff -> (GET.definitionEntry "nthdiff" (FROM.cD "calculus1")).Value.Name
        | Or -> (GET.definitionEntry "or" (FROM.cD "logic1")).Value.Name
        | OrientedInterval -> (GET.definitionEntry "oriented_interval" (FROM.cD "interval1")).Value.Name
        | Otherwise -> (GET.definitionEntry "otherwise" (FROM.cD "piece1")).Value.Name
        | Outerproduct -> (GET.definitionEntry "outerproduct" (FROM.cD "linalg1")).Value.Name
        | PartialDiff -> (GET.definitionEntry "partialdiff" (FROM.cD "calculus1")).Value.Name
        | PartialDiffDegree -> (GET.definitionEntry "partialdiffdegree" (FROM.cD "calculus1")).Value.Name
        | Piece -> (GET.definitionEntry "piece" (FROM.cD "piece1")).Value.Name
        | Piecewise -> (GET.definitionEntry "piecewise" (FROM.cD "piece1")).Value.Name
        | Plus -> (GET.definitionEntry "plus" (FROM.cD "arith1")).Value.Name
        | ExplicitToThePowerOf -> (GET.definitionEntry "power" (FROM.cD "arith1")).Value.Name
        | ToThePowerOf -> (GET.definitionEntry "power" (FROM.cD "arith1")).Value.Name
        | PredicateOnList -> (GET.definitionEntry "predicate_on_list" (FROM.cD "fns2")).Value.Name
        | Product -> (GET.definitionEntry "product" (FROM.cD "arith1")).Value.Name
        | MultiProperSubset -> (GET.definitionEntry "prsubset" (FROM.cD "multiset1")).Value.Name
        | Prsubset -> (GET.definitionEntry "prsubset" (FROM.cD "set1")).Value.Name
        | Quotient -> (GET.definitionEntry "quotient" (FROM.cD "integer1")).Value.Name
        | Range -> (GET.definitionEntry "range" (FROM.cD "fns1")).Value.Name
        | RealPart -> (GET.definitionEntry "real" (FROM.cD "complex1")).Value.Name
        | Remainder -> (GET.definitionEntry "remainder" (FROM.cD "integer1")).Value.Name
        | Restriction -> (GET.definitionEntry "restriction" (FROM.cD "fns1")).Value.Name
        | RightCompose -> (GET.definitionEntry "right_compose" (FROM.cD "fns2")).Value.Name
        | RightIinverse -> (GET.definitionEntry "right_inverse" (FROM.cD "fns1")).Value.Name
        | Root -> (GET.definitionEntry "root" (FROM.cD "arith1")).Value.Name
        | Round -> (GET.definitionEntry "round" (FROM.cD "rounding1")).Value.Name
        | Scalarproduct -> (GET.definitionEntry "scalarproduct" (FROM.cD "linalg1")).Value.Name
        | DataSdev -> (GET.definitionEntry "sdev" (FROM.cD "s_data1")).Value.Name
        | Sdev -> (GET.definitionEntry "sdev" (FROM.cD "s_dist1")).Value.Name
        | Sec -> (GET.definitionEntry "sec" (FROM.cD "transc1")).Value.Name
        | Sech -> (GET.definitionEntry "sech" (FROM.cD "transc1")).Value.Name
        | Set -> (GET.definitionEntry "set" (FROM.cD "set1")).Value.Name
        | MultiSetdiff -> (GET.definitionEntry "setdiff" (FROM.cD "multiset1")).Value.Name
        | Setdiff -> (GET.definitionEntry "setdiff" (FROM.cD "set1")).Value.Name
        | Sin -> (GET.definitionEntry "sin" (FROM.cD "transc1")).Value.Name
        | Sinh -> (GET.definitionEntry "sinh" (FROM.cD "transc1")).Value.Name
        | MultiSize -> (GET.definitionEntry "size" (FROM.cD "multiset1")).Value.Name
        | Size -> (GET.definitionEntry "size" (FROM.cD "set1")).Value.Name
        | MultiSubset -> (GET.definitionEntry "subset" (FROM.cD "multiset1")).Value.Name
        | Subset -> (GET.definitionEntry "subset" (FROM.cD "set1")).Value.Name
        | ListSuchthat -> (GET.definitionEntry "suchthat" (FROM.cD "list1")).Value.Name
        | SuchThat -> (GET.definitionEntry "suchthat" (FROM.cD "set1")).Value.Name
        | Sum -> (GET.definitionEntry "sum" (FROM.cD "arith1")).Value.Name
        | Tan -> (GET.definitionEntry "tan" (FROM.cD "transc1")).Value.Name
        | Tanh -> (GET.definitionEntry "tanh" (FROM.cD "transc1")).Value.Name
        | Times -> (GET.definitionEntry "times" (FROM.cD "arith1")).Value.Name
        | Transpose -> (GET.definitionEntry "transpose" (FROM.cD "linalg1")).Value.Name
        | Trunc -> (GET.definitionEntry "trunc" (FROM.cD "rounding1")).Value.Name
        | UnaryMinus -> (GET.definitionEntry "unary_minus" (FROM.cD "arith1")).Value.Name
        | MultiUnion -> (GET.definitionEntry "union" (FROM.cD "multiset1")).Value.Name
        | Union -> (GET.definitionEntry "union" (FROM.cD "set1")).Value.Name
        | DataVariance -> (GET.definitionEntry "variance" (FROM.cD "s_data1")).Value.Name
        | Variance -> (GET.definitionEntry "variance" (FROM.cD "s_dist1")).Value.Name
        | Vector -> (GET.definitionEntry "vector" (FROM.cD "linalg2")).Value.Name
        | Vectorselector -> (GET.definitionEntry "vector_selector" (FROM.cD "linalg1")).Value.Name
        | Vectorproduct -> (GET.definitionEntry "vectorproduct" (FROM.cD "linalg1")).Value.Name
        | Xnor -> (GET.definitionEntry "xnor" (FROM.cD "logic1")).Value.Name
        | Xor -> (GET.definitionEntry "xor" (FROM.cD "logic1")).Value.Name
        | FractionType -> (GET.definitionEntry "rational)" (FROM.cD "nums1")).Value.Name
        | _ -> ""  
 
module Function =
    
    let defenition = "TODO"

