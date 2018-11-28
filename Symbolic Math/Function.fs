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

    with
    member this.definition =
        match this with
        | Abs -> GET.definitionEntry (GET.definitions "arith1") "abs"
        | And -> GET.definitionEntry (GET.definitions "logic1") "and"
        | ApplyToList-> GET.definitionEntry (GET.definitions "fns2") "apply_to_list"
        | Approx-> GET.definitionEntry (GET.definitions "relation1") "approx"
        | ArcCos-> GET.definitionEntry (GET.definitions "transc1") "arccos"
        | ArcCosh-> GET.definitionEntry (GET.definitions "transc1") "arccosh"
        | ArcCot-> GET.definitionEntry (GET.definitions "transc1") "arccot"
        | ArcCoth-> GET.definitionEntry (GET.definitions "transc1") "arccoth"
        | ArcCsc-> GET.definitionEntry (GET.definitions "transc1") "arccsc"
        | ArcCsch-> GET.definitionEntry (GET.definitions "transc1") "arccsch"
        | ArcSec-> GET.definitionEntry (GET.definitions "transc1") "arcsec"
        | ArcSech-> GET.definitionEntry (GET.definitions "transc1") "arcsech"
        | ArcSin-> GET.definitionEntry (GET.definitions "transc1") "arcsin"
        | ArcSinh-> GET.definitionEntry (GET.definitions "transc1") "arcsinh"
        | ArcTan-> GET.definitionEntry (GET.definitions "transc1") "arctan"
        | ArcTanh-> GET.definitionEntry (GET.definitions "transc1") "arctanh"
        | Argument-> GET.definitionEntry (GET.definitions "complex1") "argument"
        | BasedFloat-> GET.definitionEntry (GET.definitions "nums1") "based_float"
        | BasedInteger-> GET.definitionEntry (GET.definitions "nums1") "based_integer"
        | BigFloat-> GET.definitionEntry (GET.definitions "bigfloat1") "bigfloat"
        | BigFloatPrec-> GET.definitionEntry (GET.definitions "bigfloat1") "bigfloatprec"
        | CartesianProduct-> GET.definitionEntry (GET.definitions "set1") "cartesian_product"
        | MultiCartesianProduct-> GET.definitionEntry (GET.definitions "multiset1") "cartesian_product"
        | Ceiling-> GET.definitionEntry (GET.definitions "rounding1") "ceiling"
        | ComplexCartesian-> GET.definitionEntry (GET.definitions "complex1") "complex_cartesian"
        | ComplexPolar-> GET.definitionEntry (GET.definitions "complex1") "complex_polar"
        | Conjugate-> GET.definitionEntry (GET.definitions "complex1") "conjugate"
        | Cos-> GET.definitionEntry (GET.definitions "transc1") "cos"
        | Cosh-> GET.definitionEntry (GET.definitions "transc1") "cosh"
        | Cot-> GET.definitionEntry (GET.definitions "transc1") "cot"
        | Coth-> GET.definitionEntry (GET.definitions "transc1") "coth"
        | Csc-> GET.definitionEntry (GET.definitions "transc1") "csc"
        | Csch-> GET.definitionEntry (GET.definitions "transc1") "csch"
        | Curl-> GET.definitionEntry (GET.definitions "veccalc1") "curl"
        | Defint-> GET.definitionEntry (GET.definitions "calculus1") "defint"
        | Determinant-> GET.definitionEntry (GET.definitions "linalg1") "determinant"
        | Diff-> GET.definitionEntry (GET.definitions "calculus1") "diff"
        | Divergence-> GET.definitionEntry (GET.definitions "veccalc1") "divergence"
        | DividedBy-> GET.definitionEntry (GET.definitions "arith1") "divide"
        | Domain-> GET.definitionEntry (GET.definitions "fns1") "domain"
        | DomainOfApplication-> GET.definitionEntry (GET.definitions "fns1") "domainofapplication"
        | Equals-> GET.definitionEntry (GET.definitions "relation1") "eq"
        | Equivalent-> GET.definitionEntry (GET.definitions "logic1") "equivalent"
        | Exp-> GET.definitionEntry (GET.definitions "transc1") "exp"
        | Factorial-> GET.definitionEntry (GET.definitions "integer1") "factorial"
        | Factorof-> GET.definitionEntry (GET.definitions "integer1") "factorof"
        | Floor-> GET.definitionEntry (GET.definitions "rounding1") "floor"
        | Gcd-> GET.definitionEntry (GET.definitions "arith1") "gcd"
        | Geq-> GET.definitionEntry (GET.definitions "relation1") "geq"
        | Grad-> GET.definitionEntry (GET.definitions "veccalc1") "grad"
        | GreaterThan-> GET.definitionEntry (GET.definitions "relation1") "gt"
        | Identity-> GET.definitionEntry (GET.definitions "fns1") "identity"
        | Image-> GET.definitionEntry (GET.definitions "fns1") "image"
        | Imaginary-> GET.definitionEntry (GET.definitions "complex1") "imaginary"
        | Implies-> GET.definitionEntry (GET.definitions "logic1") "implies"
        | In-> GET.definitionEntry (GET.definitions "set1") "in"
        | MultiIn-> GET.definitionEntry (GET.definitions "multiset1") "in"
        | Int-> GET.definitionEntry (GET.definitions "calculus1") "int"
        | IntegerInterval-> GET.definitionEntry (GET.definitions "interval1") "integer_interval"
        | Intersect-> GET.definitionEntry (GET.definitions "set1") "intersect"
        | MultiIntersect-> GET.definitionEntry (GET.definitions "multiset1") "intersect"
        | Interval-> GET.definitionEntry (GET.definitions "interval1") "interval"
        | IntervalCC-> GET.definitionEntry (GET.definitions "interval1") "interval_cc"
        | IntervalCO-> GET.definitionEntry (GET.definitions "interval1") "interval_co"
        | IntervalOC-> GET.definitionEntry (GET.definitions "interval1") "interval_oc"
        | IntervalOO-> GET.definitionEntry (GET.definitions "interval1") "interval_oo"
        | Inverse-> GET.definitionEntry (GET.definitions "fns1") "inverse"
        | Kernel-> GET.definitionEntry (GET.definitions "fns2") "kernel"
        | Laplacian-> GET.definitionEntry (GET.definitions "veccalc1") "Laplacian"
        | Lcm-> GET.definitionEntry (GET.definitions "arith1") "lcm"
        | LeftCompose-> GET.definitionEntry (GET.definitions "fns1") "left_compose"
        | LeftInverse-> GET.definitionEntry (GET.definitions "fns1") "left_inverse"
        | Leq-> GET.definitionEntry (GET.definitions "relation1") "leq"
        | Limit-> GET.definitionEntry (GET.definitions "limit1") "limit"
        | List-> GET.definitionEntry (GET.definitions "list1") "list"
        | Ln-> GET.definitionEntry (GET.definitions "transc1") "ln"
        | Log-> GET.definitionEntry (GET.definitions "transc1") "log"
        | LessThan-> GET.definitionEntry (GET.definitions "relation1") "lt"
        | ListMap-> GET.definitionEntry (GET.definitions "list1") "map"
        | Map-> GET.definitionEntry (GET.definitions "set1") "map"
        | Matrix-> GET.definitionEntry (GET.definitions "linalg2") "matrix"
        | MatrixSelector-> GET.definitionEntry (GET.definitions "linalg1") "matrix_selector"
        | Matrixrow-> GET.definitionEntry (GET.definitions "linalg2") "matrixrow"
        | Max-> GET.definitionEntry (GET.definitions "minmax1") "max"
        | DataMean-> GET.definitionEntry (GET.definitions "s_data1") "mean"
        | Mean-> GET.definitionEntry (GET.definitions "s_dist1") "mean"
        | Median-> GET.definitionEntry (GET.definitions "s_data1") "median"
        | Min-> GET.definitionEntry (GET.definitions "minmax1") "min"
        | Minus-> GET.definitionEntry (GET.definitions "arith1") "minus"
        | Mode-> GET.definitionEntry (GET.definitions "s_data1") "mode"
        | DataMoment-> GET.definitionEntry (GET.definitions "s_data1") "moment"
        | Moment-> GET.definitionEntry (GET.definitions "s_dist1") "moment"
        | Multiset-> GET.definitionEntry (GET.definitions "multiset1") "multiset"
        | Nand-> GET.definitionEntry (GET.definitions "logic1") "nand"
        | Neq-> GET.definitionEntry (GET.definitions "relation1") "neq"
        | Nor-> GET.definitionEntry (GET.definitions "logic1") "nor"
        | Not-> GET.definitionEntry (GET.definitions "logic1") "not"
        | MultiNotin-> GET.definitionEntry (GET.definitions "multiset1") "notin"
        | Notin-> GET.definitionEntry (GET.definitions "set1") "notin"
        | MultiNotProperSubset-> GET.definitionEntry (GET.definitions "multiset1") "notprsubset"
        | Notprsubset-> GET.definitionEntry (GET.definitions "set1") "notprsubset"
        | MultiNotSubset-> GET.definitionEntry (GET.definitions "multiset1") "notsubset"
        | Notsubset-> GET.definitionEntry (GET.definitions "set1") "notsubset"
        | NthDiff-> GET.definitionEntry (GET.definitions "calculus1") "nthdiff"
        | Or-> GET.definitionEntry (GET.definitions "logic1") "or"
        | OrientedInterval-> GET.definitionEntry (GET.definitions "interval1") "oriented_interval"
        | Otherwise-> GET.definitionEntry (GET.definitions "piece1") "otherwise"
        | Outerproduct-> GET.definitionEntry (GET.definitions "linalg1") "outerproduct"
        | PartialDiff-> GET.definitionEntry (GET.definitions "calculus1") "partialdiff"
        | PartialDiffDegree-> GET.definitionEntry (GET.definitions "calculus1") "partialdiffdegree"
        | Piece-> GET.definitionEntry (GET.definitions "piece1") "piece"
        | Piecewise-> GET.definitionEntry (GET.definitions "piece1") "piecewise"
        | Plus-> GET.definitionEntry (GET.definitions "arith1") "plus"
        | ExplicitToThePowerOf-> GET.definitionEntry (GET.definitions "arith1") "power"
        | ToThePowerOf-> GET.definitionEntry (GET.definitions "arith1") "power"
        | PredicateOnList-> GET.definitionEntry (GET.definitions "fns2") "predicate_on_list"
        | Product-> GET.definitionEntry (GET.definitions "arith1") "product"
        | MultiProperSubset-> GET.definitionEntry (GET.definitions "multiset1") "prsubset"
        | Prsubset-> GET.definitionEntry (GET.definitions "set1") "prsubset"
        | Quotient-> GET.definitionEntry (GET.definitions "integer1") "quotient"
        | Range-> GET.definitionEntry (GET.definitions "fns1") "range"
        | RealPart-> GET.definitionEntry (GET.definitions "complex1") "real"
        | Remainder-> GET.definitionEntry (GET.definitions "integer1") "remainder"
        | Restriction-> GET.definitionEntry (GET.definitions "fns1") "restriction"
        | RightCompose-> GET.definitionEntry (GET.definitions "fns2") "right_compose"
        | RightIinverse-> GET.definitionEntry (GET.definitions "fns1") "right_inverse"
        | Root-> GET.definitionEntry (GET.definitions "arith1") "root"
        | Round-> GET.definitionEntry (GET.definitions "rounding1") "round"
        | Scalarproduct-> GET.definitionEntry (GET.definitions "linalg1") "scalarproduct"
        | DataSdev-> GET.definitionEntry (GET.definitions "s_data1") "sdev"
        | Sdev-> GET.definitionEntry (GET.definitions "s_dist1") "sdev"
        | Sec-> GET.definitionEntry (GET.definitions "transc1") "sec"
        | Sech-> GET.definitionEntry (GET.definitions "transc1") "sech"
        | Set-> GET.definitionEntry (GET.definitions "set1") "set"
        | MultiSetdiff-> GET.definitionEntry (GET.definitions "multiset1") "setdiff"
        | Setdiff-> GET.definitionEntry (GET.definitions "set1") "setdiff"
        | Sin-> GET.definitionEntry (GET.definitions "transc1") "sin"
        | Sinh-> GET.definitionEntry (GET.definitions "transc1") "sinh"
        | MultiSize-> GET.definitionEntry (GET.definitions "multiset1") "size"
        | Size-> GET.definitionEntry (GET.definitions "set1") "size"
        | MultiSubset-> GET.definitionEntry (GET.definitions "multiset1") "subset"
        | Subset-> GET.definitionEntry (GET.definitions "set1") "subset"
        | ListSuchthat-> GET.definitionEntry (GET.definitions "list1") "suchthat"
        | SuchThat-> GET.definitionEntry (GET.definitions "set1") "suchthat"
        | Sum-> GET.definitionEntry (GET.definitions "arith1") "sum"
        | Tan-> GET.definitionEntry (GET.definitions "transc1") "tan"
        | Tanh-> GET.definitionEntry (GET.definitions "transc1") "tanh"
        | Times-> GET.definitionEntry (GET.definitions "arith1") "times"
        | Transpose-> GET.definitionEntry (GET.definitions "linalg1") "transpose"
        | Trunc-> GET.definitionEntry (GET.definitions "rounding1") "trunc"
        | UnaryMinus-> GET.definitionEntry (GET.definitions "arith1") "unary_minus"
        | MultiUnion-> GET.definitionEntry (GET.definitions "multiset1") "union"
        | Union-> GET.definitionEntry (GET.definitions "set1") "union"
        | DataVariance-> GET.definitionEntry (GET.definitions "s_data1") "variance"
        | Variance-> GET.definitionEntry (GET.definitions "s_dist1") "variance"
        | Vector-> GET.definitionEntry (GET.definitions "linalg2") "vector"
        | Vectorselector-> GET.definitionEntry (GET.definitions "linalg1") "vector_selector"
        | Vectorproduct-> GET.definitionEntry (GET.definitions "linalg1") "vectorproduct"
        | Xnor-> GET.definitionEntry (GET.definitions "logic1") "xnor"
        | Xor-> GET.definitionEntry (GET.definitions "logic1") "xor"
        | _ -> None

    member this.symbol =
        match this with 
        | Abs -> (GET.definitionEntry (GET.definitions "arith1") "abs").Value.Name
        | And -> (GET.definitionEntry (GET.definitions "logic1") "and").Value.Name
        | ApplyToList -> (GET.definitionEntry (GET.definitions "fns2") "apply_to_list").Value.Name
        | Approx -> (GET.definitionEntry (GET.definitions "relation1") "approx").Value.Name
        | ArcCos -> (GET.definitionEntry (GET.definitions "transc1") "arccos").Value.Name
        | ArcCosh -> (GET.definitionEntry (GET.definitions "transc1") "arccosh").Value.Name
        | ArcCot -> (GET.definitionEntry (GET.definitions "transc1") "arccot").Value.Name
        | ArcCoth -> (GET.definitionEntry (GET.definitions "transc1") "arccoth").Value.Name
        | ArcCsc -> (GET.definitionEntry (GET.definitions "transc1") "arccsc").Value.Name
        | ArcCsch -> (GET.definitionEntry (GET.definitions "transc1") "arccsch").Value.Name
        | ArcSec -> (GET.definitionEntry (GET.definitions "transc1") "arcsec").Value.Name
        | ArcSech -> (GET.definitionEntry (GET.definitions "transc1") "arcsech").Value.Name
        | ArcSin -> (GET.definitionEntry (GET.definitions "transc1") "arcsin").Value.Name
        | ArcSinh -> (GET.definitionEntry (GET.definitions "transc1") "arcsinh").Value.Name
        | ArcTan -> (GET.definitionEntry (GET.definitions "transc1") "arctan").Value.Name
        | ArcTanh -> (GET.definitionEntry (GET.definitions "transc1") "arctanh").Value.Name
        | Argument -> (GET.definitionEntry (GET.definitions "complex1") "argument").Value.Name
        | BasedFloat -> (GET.definitionEntry (GET.definitions "nums1") "based_float").Value.Name
        | BasedInteger -> (GET.definitionEntry (GET.definitions "nums1") "based_integer").Value.Name
        | BigFloat -> (GET.definitionEntry (GET.definitions "bigfloat1") "bigfloat").Value.Name
        | BigFloatPrec -> (GET.definitionEntry (GET.definitions "bigfloat1") "bigfloatprec").Value.Name
        | CartesianProduct -> (GET.definitionEntry (GET.definitions "set1") "cartesian_product").Value.Name
        | MultiCartesianProduct -> (GET.definitionEntry (GET.definitions "multiset1") "cartesian_product").Value.Name
        | Ceiling -> (GET.definitionEntry (GET.definitions "rounding1") "ceiling").Value.Name
        | ComplexCartesian -> (GET.definitionEntry (GET.definitions "complex1") "complex_cartesian").Value.Name
        | ComplexPolar -> (GET.definitionEntry (GET.definitions "complex1") "complex_polar").Value.Name
        | Conjugate -> (GET.definitionEntry (GET.definitions "complex1") "conjugate").Value.Name
        | Cos -> (GET.definitionEntry (GET.definitions "transc1") "cos").Value.Name
        | Cosh -> (GET.definitionEntry (GET.definitions "transc1") "cosh").Value.Name
        | Cot -> (GET.definitionEntry (GET.definitions "transc1") "cot").Value.Name
        | Coth -> (GET.definitionEntry (GET.definitions "transc1") "coth").Value.Name
        | Csc -> (GET.definitionEntry (GET.definitions "transc1") "csc").Value.Name
        | Csch -> (GET.definitionEntry (GET.definitions "transc1") "csch").Value.Name
        | Curl -> (GET.definitionEntry (GET.definitions "veccalc1") "curl").Value.Name
        | Defint -> (GET.definitionEntry (GET.definitions "calculus1") "defint").Value.Name
        | Determinant -> (GET.definitionEntry (GET.definitions "linalg1") "determinant").Value.Name
        | Diff -> (GET.definitionEntry (GET.definitions "calculus1") "diff").Value.Name
        | Divergence -> (GET.definitionEntry (GET.definitions "veccalc1") "divergence").Value.Name
        | DividedBy -> (GET.definitionEntry (GET.definitions "arith1") "divide").Value.Name
        | Domain -> (GET.definitionEntry (GET.definitions "fns1") "domain").Value.Name
        | DomainOfApplication -> (GET.definitionEntry (GET.definitions "fns1") "domainofapplication").Value.Name
        | Equals -> (GET.definitionEntry (GET.definitions "relation1") "eq").Value.Name
        | Equivalent -> (GET.definitionEntry (GET.definitions "logic1") "equivalent").Value.Name
        | Exp -> (GET.definitionEntry (GET.definitions "transc1") "exp").Value.Name
        | Factorial -> (GET.definitionEntry (GET.definitions "integer1") "factorial").Value.Name
        | Factorof -> (GET.definitionEntry (GET.definitions "integer1") "factorof").Value.Name
        | Floor -> (GET.definitionEntry (GET.definitions "rounding1") "floor").Value.Name
        | Gcd -> (GET.definitionEntry (GET.definitions "arith1") "gcd").Value.Name
        | Geq -> (GET.definitionEntry (GET.definitions "relation1") "geq").Value.Name
        | Grad -> (GET.definitionEntry (GET.definitions "veccalc1") "grad").Value.Name
        | GreaterThan -> (GET.definitionEntry (GET.definitions "relation1") "gt").Value.Name
        | Identity -> (GET.definitionEntry (GET.definitions "fns1") "identity").Value.Name
        | Image -> (GET.definitionEntry (GET.definitions "fns1") "image").Value.Name
        | Imaginary -> (GET.definitionEntry (GET.definitions "complex1") "imaginary").Value.Name
        | Implies -> (GET.definitionEntry (GET.definitions "logic1") "implies").Value.Name
        | In -> (GET.definitionEntry (GET.definitions "set1") "in").Value.Name
        | MultiIn -> (GET.definitionEntry (GET.definitions "multiset1") "in").Value.Name
        | Int -> (GET.definitionEntry (GET.definitions "calculus1") "int").Value.Name
        | IntegerInterval -> (GET.definitionEntry (GET.definitions "interval1") "integer_interval").Value.Name
        | Intersect -> (GET.definitionEntry (GET.definitions "set1") "intersect").Value.Name
        | MultiIntersect -> (GET.definitionEntry (GET.definitions "multiset1") "intersect").Value.Name
        | Interval -> (GET.definitionEntry (GET.definitions "interval1") "interval").Value.Name
        | IntervalCC -> (GET.definitionEntry (GET.definitions "interval1") "interval_cc").Value.Name
        | IntervalCO -> (GET.definitionEntry (GET.definitions "interval1") "interval_co").Value.Name
        | IntervalOC -> (GET.definitionEntry (GET.definitions "interval1") "interval_oc").Value.Name
        | IntervalOO -> (GET.definitionEntry (GET.definitions "interval1") "interval_oo").Value.Name
        | Inverse -> (GET.definitionEntry (GET.definitions "fns1") "inverse").Value.Name
        | Kernel -> (GET.definitionEntry (GET.definitions "fns2") "kernel").Value.Name
        | Laplacian -> (GET.definitionEntry (GET.definitions "veccalc1") "Laplacian").Value.Name
        | Lcm -> (GET.definitionEntry (GET.definitions "arith1") "lcm").Value.Name
        | LeftCompose -> (GET.definitionEntry (GET.definitions "fns1") "left_compose").Value.Name
        | LeftInverse -> (GET.definitionEntry (GET.definitions "fns1") "left_inverse").Value.Name
        | Leq -> (GET.definitionEntry (GET.definitions "relation1") "leq").Value.Name
        | Limit -> (GET.definitionEntry (GET.definitions "limit1") "limit").Value.Name
        | List -> (GET.definitionEntry (GET.definitions "list1") "list").Value.Name
        | Ln -> (GET.definitionEntry (GET.definitions "transc1") "ln").Value.Name
        | Log -> (GET.definitionEntry (GET.definitions "transc1") "log").Value.Name
        | LessThan -> (GET.definitionEntry (GET.definitions "relation1") "lt").Value.Name
        | ListMap -> (GET.definitionEntry (GET.definitions "list1") "map").Value.Name
        | Map -> (GET.definitionEntry (GET.definitions "set1") "map").Value.Name
        | Matrix -> (GET.definitionEntry (GET.definitions "linalg2") "matrix").Value.Name
        | MatrixSelector -> (GET.definitionEntry (GET.definitions "linalg1") "matrix_selector").Value.Name
        | Matrixrow -> (GET.definitionEntry (GET.definitions "linalg2") "matrixrow").Value.Name
        | Max -> (GET.definitionEntry (GET.definitions "minmax1") "max").Value.Name
        | DataMean -> (GET.definitionEntry (GET.definitions "s_data1") "mean").Value.Name
        | Mean -> (GET.definitionEntry (GET.definitions "s_dist1") "mean").Value.Name
        | Median -> (GET.definitionEntry (GET.definitions "s_data1") "median").Value.Name
        | Min -> (GET.definitionEntry (GET.definitions "minmax1") "min").Value.Name
        | Minus -> (GET.definitionEntry (GET.definitions "arith1") "minus").Value.Name
        | Mode -> (GET.definitionEntry (GET.definitions "s_data1") "mode").Value.Name
        | DataMoment -> (GET.definitionEntry (GET.definitions "s_data1") "moment").Value.Name
        | Moment -> (GET.definitionEntry (GET.definitions "s_dist1") "moment").Value.Name
        | Multiset -> (GET.definitionEntry (GET.definitions "multiset1") "multiset").Value.Name
        | Nand -> (GET.definitionEntry (GET.definitions "logic1") "nand").Value.Name
        | Neq -> (GET.definitionEntry (GET.definitions "relation1") "neq").Value.Name
        | Nor -> (GET.definitionEntry (GET.definitions "logic1") "nor").Value.Name
        | Not -> (GET.definitionEntry (GET.definitions "logic1") "not").Value.Name
        | MultiNotin -> (GET.definitionEntry (GET.definitions "multiset1") "notin").Value.Name
        | Notin -> (GET.definitionEntry (GET.definitions "set1") "notin").Value.Name
        | MultiNotProperSubset -> (GET.definitionEntry (GET.definitions "multiset1") "notprsubset").Value.Name
        | Notprsubset -> (GET.definitionEntry (GET.definitions "set1") "notprsubset").Value.Name
        | MultiNotSubset -> (GET.definitionEntry (GET.definitions "multiset1") "notsubset").Value.Name
        | Notsubset -> (GET.definitionEntry (GET.definitions "set1") "notsubset").Value.Name
        | NthDiff -> (GET.definitionEntry (GET.definitions "calculus1") "nthdiff").Value.Name
        | Or -> (GET.definitionEntry (GET.definitions "logic1") "or").Value.Name
        | OrientedInterval -> (GET.definitionEntry (GET.definitions "interval1") "oriented_interval").Value.Name
        | Otherwise -> (GET.definitionEntry (GET.definitions "piece1") "otherwise").Value.Name
        | Outerproduct -> (GET.definitionEntry (GET.definitions "linalg1") "outerproduct").Value.Name
        | PartialDiff -> (GET.definitionEntry (GET.definitions "calculus1") "partialdiff").Value.Name
        | PartialDiffDegree -> (GET.definitionEntry (GET.definitions "calculus1") "partialdiffdegree").Value.Name
        | Piece -> (GET.definitionEntry (GET.definitions "piece1") "piece").Value.Name
        | Piecewise -> (GET.definitionEntry (GET.definitions "piece1") "piecewise").Value.Name
        | Plus -> (GET.definitionEntry (GET.definitions "arith1") "plus").Value.Name
        | ExplicitToThePowerOf -> (GET.definitionEntry (GET.definitions "arith1") "power").Value.Name
        | ToThePowerOf -> (GET.definitionEntry (GET.definitions "arith1") "power").Value.Name
        | PredicateOnList -> (GET.definitionEntry (GET.definitions "fns2") "predicate_on_list").Value.Name
        | Product -> (GET.definitionEntry (GET.definitions "arith1") "product").Value.Name
        | MultiProperSubset -> (GET.definitionEntry (GET.definitions "multiset1") "prsubset").Value.Name
        | Prsubset -> (GET.definitionEntry (GET.definitions "set1") "prsubset").Value.Name
        | Quotient -> (GET.definitionEntry (GET.definitions "integer1") "quotient").Value.Name
        | Range -> (GET.definitionEntry (GET.definitions "fns1") "range").Value.Name
        | RealPart -> (GET.definitionEntry (GET.definitions "complex1") "real").Value.Name
        | Remainder -> (GET.definitionEntry (GET.definitions "integer1") "remainder").Value.Name
        | Restriction -> (GET.definitionEntry (GET.definitions "fns1") "restriction").Value.Name
        | RightCompose -> (GET.definitionEntry (GET.definitions "fns2") "right_compose").Value.Name
        | RightIinverse -> (GET.definitionEntry (GET.definitions "fns1") "right_inverse").Value.Name
        | Root -> (GET.definitionEntry (GET.definitions "arith1") "root").Value.Name
        | Round -> (GET.definitionEntry (GET.definitions "rounding1") "round").Value.Name
        | Scalarproduct -> (GET.definitionEntry (GET.definitions "linalg1") "scalarproduct").Value.Name
        | DataSdev -> (GET.definitionEntry (GET.definitions "s_data1") "sdev").Value.Name
        | Sdev -> (GET.definitionEntry (GET.definitions "s_dist1") "sdev").Value.Name
        | Sec -> (GET.definitionEntry (GET.definitions "transc1") "sec").Value.Name
        | Sech -> (GET.definitionEntry (GET.definitions "transc1") "sech").Value.Name
        | Set -> (GET.definitionEntry (GET.definitions "set1") "set").Value.Name
        | MultiSetdiff -> (GET.definitionEntry (GET.definitions "multiset1") "setdiff").Value.Name
        | Setdiff -> (GET.definitionEntry (GET.definitions "set1") "setdiff").Value.Name
        | Sin -> (GET.definitionEntry (GET.definitions "transc1") "sin").Value.Name
        | Sinh -> (GET.definitionEntry (GET.definitions "transc1") "sinh").Value.Name
        | MultiSize -> (GET.definitionEntry (GET.definitions "multiset1") "size").Value.Name
        | Size -> (GET.definitionEntry (GET.definitions "set1") "size").Value.Name
        | MultiSubset -> (GET.definitionEntry (GET.definitions "multiset1") "subset").Value.Name
        | Subset -> (GET.definitionEntry (GET.definitions "set1") "subset").Value.Name
        | ListSuchthat -> (GET.definitionEntry (GET.definitions "list1") "suchthat").Value.Name
        | SuchThat -> (GET.definitionEntry (GET.definitions "set1") "suchthat").Value.Name
        | Sum -> (GET.definitionEntry (GET.definitions "arith1") "sum").Value.Name
        | Tan -> (GET.definitionEntry (GET.definitions "transc1") "tan").Value.Name
        | Tanh -> (GET.definitionEntry (GET.definitions "transc1") "tanh").Value.Name
        | Times -> (GET.definitionEntry (GET.definitions "arith1") "times").Value.Name
        | Transpose -> (GET.definitionEntry (GET.definitions "linalg1") "transpose").Value.Name
        | Trunc -> (GET.definitionEntry (GET.definitions "rounding1") "trunc").Value.Name
        | UnaryMinus -> (GET.definitionEntry (GET.definitions "arith1") "unary_minus").Value.Name
        | MultiUnion -> (GET.definitionEntry (GET.definitions "multiset1") "union").Value.Name
        | Union -> (GET.definitionEntry (GET.definitions "set1") "union").Value.Name
        | DataVariance -> (GET.definitionEntry (GET.definitions "s_data1") "variance").Value.Name
        | Variance -> (GET.definitionEntry (GET.definitions "s_dist1") "variance").Value.Name
        | Vector -> (GET.definitionEntry (GET.definitions "linalg2") "vector").Value.Name
        | Vectorselector -> (GET.definitionEntry (GET.definitions "linalg1") "vector_selector").Value.Name
        | Vectorproduct -> (GET.definitionEntry (GET.definitions "linalg1") "vectorproduct").Value.Name
        | Xnor -> (GET.definitionEntry (GET.definitions "logic1") "xnor").Value.Name
        | Xor -> (GET.definitionEntry (GET.definitions "logic1") "xor").Value.Name
        | _ -> ""  

 
module Function =
    
    let defenition = "TODO"

