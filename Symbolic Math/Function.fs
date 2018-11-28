namespace Math.Pure.Objects

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
        | Abs -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "abs"
        | And -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "and"
        | ApplyToList-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "apply_to_list"
        | Approx-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "approx"
        | ArcCos-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccos"
        | ArcCosh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccosh"
        | ArcCot-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccot"
        | ArcCoth-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccoth"
        | ArcCsc-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccsc"
        | ArcCsch-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccsch"
        | ArcSec-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsec"
        | ArcSech-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsech"
        | ArcSin-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsin"
        | ArcSinh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsinh"
        | ArcTan-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arctan"
        | ArcTanh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arctanh"
        | Argument-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "argument"
        | BasedFloat-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "based_float"
        | BasedInteger-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "based_integer"
        | BigFloat-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "bigfloat1") "bigfloat"
        | BigFloatPrec-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "bigfloat1") "bigfloatprec"
        | CartesianProduct-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "cartesian_product"
        | MultiCartesianProduct-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "cartesian_product"
        | Ceiling-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "ceiling"
        | ComplexCartesian-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "complex_cartesian"
        | ComplexPolar-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "complex_polar"
        | Conjugate-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "conjugate"
        | Cos-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cos"
        | Cosh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cosh"
        | Cot-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cot"
        | Coth-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "coth"
        | Csc-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "csc"
        | Csch-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "csch"
        | Curl-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "curl"
        | Defint-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "defint"
        | Determinant-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "determinant"
        | Diff-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "diff"
        | Divergence-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "divergence"
        | DividedBy-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "divide"
        | Domain-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "domain"
        | DomainOfApplication-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "domainofapplication"
        | Equals-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "eq"
        | Equivalent-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "equivalent"
        | Exp-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "exp"
        | Factorial-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "factorial"
        | Factorof-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "factorof"
        | Floor-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "floor"
        | Gcd-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "gcd"
        | Geq-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "geq"
        | Grad-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "grad"
        | GreaterThan-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "gt"
        | Identity-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "identity"
        | Image-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "image"
        | Imaginary-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "imaginary"
        | Implies-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "implies"
        | In-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "in"
        | MultiIn-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "in"
        | Int-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "int"
        | IntegerInterval-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "integer_interval"
        | Intersect-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "intersect"
        | MultiIntersect-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "intersect"
        | Interval-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval"
        | IntervalCC-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_cc"
        | IntervalCO-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_co"
        | IntervalOC-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_oc"
        | IntervalOO-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_oo"
        | Inverse-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "inverse"
        | Kernel-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "kernel"
        | Laplacian-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "Laplacian"
        | Lcm-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "lcm"
        | LeftCompose-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "left_compose"
        | LeftInverse-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "left_inverse"
        | Leq-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "leq"
        | Limit-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "limit"
        | List-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "list"
        | Ln-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "ln"
        | Log-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "log"
        | LessThan-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "lt"
        | ListMap-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "map"
        | Map-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "map"
        | Matrix-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "matrix"
        | MatrixSelector-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "matrix_selector"
        | Matrixrow-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "matrixrow"
        | Max-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "minmax1") "max"
        | DataMean-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "mean"
        | Mean-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "mean"
        | Median-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "median"
        | Min-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "minmax1") "min"
        | Minus-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "minus"
        | Mode-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "mode"
        | DataMoment-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "moment"
        | Moment-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "moment"
        | Multiset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "multiset"
        | Nand-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "nand"
        | Neq-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "neq"
        | Nor-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "nor"
        | Not-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "not"
        | MultiNotin-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notin"
        | Notin-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notin"
        | MultiNotProperSubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notprsubset"
        | Notprsubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notprsubset"
        | MultiNotSubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notsubset"
        | Notsubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notsubset"
        | NthDiff-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "nthdiff"
        | Or-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "or"
        | OrientedInterval-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "oriented_interval"
        | Otherwise-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "otherwise"
        | Outerproduct-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "outerproduct"
        | PartialDiff-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "partialdiff"
        | PartialDiffDegree-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "partialdiffdegree"
        | Piece-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "piece"
        | Piecewise-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "piecewise"
        | Plus-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "plus"
        | ExplicitToThePowerOf-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "power"
        | ToThePowerOf-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "power"
        | PredicateOnList-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "predicate_on_list"
        | Product-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "product"
        | MultiProperSubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "prsubset"
        | Prsubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "prsubset"
        | Quotient-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "quotient"
        | Range-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "range"
        | RealPart-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "real"
        | Remainder-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "remainder"
        | Restriction-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "restriction"
        | RightCompose-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "right_compose"
        | RightIinverse-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "right_inverse"
        | Root-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "root"
        | Round-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "round"
        | Scalarproduct-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "scalarproduct"
        | DataSdev-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "sdev"
        | Sdev-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "sdev"
        | Sec-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sec"
        | Sech-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sech"
        | Set-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "set"
        | MultiSetdiff-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "setdiff"
        | Setdiff-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "setdiff"
        | Sin-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sin"
        | Sinh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sinh"
        | MultiSize-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "size"
        | Size-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "size"
        | MultiSubset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "subset"
        | Subset-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "subset"
        | ListSuchthat-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "suchthat"
        | SuchThat-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "suchthat"
        | Sum-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "sum"
        | Tan-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "tan"
        | Tanh-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "tanh"
        | Times-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "times"
        | Transpose-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "transpose"
        | Trunc-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "trunc"
        | UnaryMinus-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "unary_minus"
        | MultiUnion-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "union"
        | Union-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "union"
        | DataVariance-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "variance"
        | Variance-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "variance"
        | Vector-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "vector"
        | Vectorselector-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "vector_selector"
        | Vectorproduct-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "vectorproduct"
        | Xnor-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "xnor"
        | Xor-> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "xor"
        | _ -> None

    member this.symbol =
        match this with 
        | Abs -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "abs").Value.Name
        | And -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "and").Value.Name
        | ApplyToList -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "apply_to_list").Value.Name
        | Approx -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "approx").Value.Name
        | ArcCos -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccos").Value.Name
        | ArcCosh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccosh").Value.Name
        | ArcCot -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccot").Value.Name
        | ArcCoth -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccoth").Value.Name
        | ArcCsc -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccsc").Value.Name
        | ArcCsch -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arccsch").Value.Name
        | ArcSec -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsec").Value.Name
        | ArcSech -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsech").Value.Name
        | ArcSin -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsin").Value.Name
        | ArcSinh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arcsinh").Value.Name
        | ArcTan -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arctan").Value.Name
        | ArcTanh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "arctanh").Value.Name
        | Argument -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "argument").Value.Name
        | BasedFloat -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "based_float").Value.Name
        | BasedInteger -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "based_integer").Value.Name
        | BigFloat -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "bigfloat1") "bigfloat").Value.Name
        | BigFloatPrec -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "bigfloat1") "bigfloatprec").Value.Name
        | CartesianProduct -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "cartesian_product").Value.Name
        | MultiCartesianProduct -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "cartesian_product").Value.Name
        | Ceiling -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "ceiling").Value.Name
        | ComplexCartesian -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "complex_cartesian").Value.Name
        | ComplexPolar -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "complex_polar").Value.Name
        | Conjugate -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "conjugate").Value.Name
        | Cos -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cos").Value.Name
        | Cosh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cosh").Value.Name
        | Cot -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "cot").Value.Name
        | Coth -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "coth").Value.Name
        | Csc -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "csc").Value.Name
        | Csch -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "csch").Value.Name
        | Curl -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "curl").Value.Name
        | Defint -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "defint").Value.Name
        | Determinant -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "determinant").Value.Name
        | Diff -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "diff").Value.Name
        | Divergence -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "divergence").Value.Name
        | DividedBy -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "divide").Value.Name
        | Domain -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "domain").Value.Name
        | DomainOfApplication -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "domainofapplication").Value.Name
        | Equals -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "eq").Value.Name
        | Equivalent -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "equivalent").Value.Name
        | Exp -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "exp").Value.Name
        | Factorial -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "factorial").Value.Name
        | Factorof -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "factorof").Value.Name
        | Floor -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "floor").Value.Name
        | Gcd -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "gcd").Value.Name
        | Geq -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "geq").Value.Name
        | Grad -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "grad").Value.Name
        | GreaterThan -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "gt").Value.Name
        | Identity -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "identity").Value.Name
        | Image -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "image").Value.Name
        | Imaginary -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "imaginary").Value.Name
        | Implies -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "implies").Value.Name
        | In -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "in").Value.Name
        | MultiIn -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "in").Value.Name
        | Int -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "int").Value.Name
        | IntegerInterval -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "integer_interval").Value.Name
        | Intersect -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "intersect").Value.Name
        | MultiIntersect -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "intersect").Value.Name
        | Interval -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval").Value.Name
        | IntervalCC -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_cc").Value.Name
        | IntervalCO -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_co").Value.Name
        | IntervalOC -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_oc").Value.Name
        | IntervalOO -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "interval_oo").Value.Name
        | Inverse -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "inverse").Value.Name
        | Kernel -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "kernel").Value.Name
        | Laplacian -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "veccalc1") "Laplacian").Value.Name
        | Lcm -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "lcm").Value.Name
        | LeftCompose -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "left_compose").Value.Name
        | LeftInverse -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "left_inverse").Value.Name
        | Leq -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "leq").Value.Name
        | Limit -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "limit").Value.Name
        | List -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "list").Value.Name
        | Ln -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "ln").Value.Name
        | Log -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "log").Value.Name
        | LessThan -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "lt").Value.Name
        | ListMap -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "map").Value.Name
        | Map -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "map").Value.Name
        | Matrix -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "matrix").Value.Name
        | MatrixSelector -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "matrix_selector").Value.Name
        | Matrixrow -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "matrixrow").Value.Name
        | Max -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "minmax1") "max").Value.Name
        | DataMean -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "mean").Value.Name
        | Mean -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "mean").Value.Name
        | Median -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "median").Value.Name
        | Min -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "minmax1") "min").Value.Name
        | Minus -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "minus").Value.Name
        | Mode -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "mode").Value.Name
        | DataMoment -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "moment").Value.Name
        | Moment -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "moment").Value.Name
        | Multiset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "multiset").Value.Name
        | Nand -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "nand").Value.Name
        | Neq -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "relation1") "neq").Value.Name
        | Nor -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "nor").Value.Name
        | Not -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "not").Value.Name
        | MultiNotin -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notin").Value.Name
        | Notin -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notin").Value.Name
        | MultiNotProperSubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notprsubset").Value.Name
        | Notprsubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notprsubset").Value.Name
        | MultiNotSubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "notsubset").Value.Name
        | Notsubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "notsubset").Value.Name
        | NthDiff -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "nthdiff").Value.Name
        | Or -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "or").Value.Name
        | OrientedInterval -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "interval1") "oriented_interval").Value.Name
        | Otherwise -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "otherwise").Value.Name
        | Outerproduct -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "outerproduct").Value.Name
        | PartialDiff -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "partialdiff").Value.Name
        | PartialDiffDegree -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "calculus1") "partialdiffdegree").Value.Name
        | Piece -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "piece").Value.Name
        | Piecewise -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "piece1") "piecewise").Value.Name
        | Plus -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "plus").Value.Name
        | ExplicitToThePowerOf -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "power").Value.Name
        | ToThePowerOf -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "power").Value.Name
        | PredicateOnList -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "predicate_on_list").Value.Name
        | Product -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "product").Value.Name
        | MultiProperSubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "prsubset").Value.Name
        | Prsubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "prsubset").Value.Name
        | Quotient -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "quotient").Value.Name
        | Range -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "range").Value.Name
        | RealPart -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "complex1") "real").Value.Name
        | Remainder -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "integer1") "remainder").Value.Name
        | Restriction -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "restriction").Value.Name
        | RightCompose -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns2") "right_compose").Value.Name
        | RightIinverse -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "fns1") "right_inverse").Value.Name
        | Root -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "root").Value.Name
        | Round -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "round").Value.Name
        | Scalarproduct -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "scalarproduct").Value.Name
        | DataSdev -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "sdev").Value.Name
        | Sdev -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "sdev").Value.Name
        | Sec -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sec").Value.Name
        | Sech -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sech").Value.Name
        | Set -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "set").Value.Name
        | MultiSetdiff -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "setdiff").Value.Name
        | Setdiff -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "setdiff").Value.Name
        | Sin -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sin").Value.Name
        | Sinh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "sinh").Value.Name
        | MultiSize -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "size").Value.Name
        | Size -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "size").Value.Name
        | MultiSubset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "subset").Value.Name
        | Subset -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "subset").Value.Name
        | ListSuchthat -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "list1") "suchthat").Value.Name
        | SuchThat -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "suchthat").Value.Name
        | Sum -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "sum").Value.Name
        | Tan -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "tan").Value.Name
        | Tanh -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "transc1") "tanh").Value.Name
        | Times -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "times").Value.Name
        | Transpose -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "transpose").Value.Name
        | Trunc -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "rounding1") "trunc").Value.Name
        | UnaryMinus -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "arith1") "unary_minus").Value.Name
        | MultiUnion -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "union").Value.Name
        | Union -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "union").Value.Name
        | DataVariance -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_data1") "variance").Value.Name
        | Variance -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "s_dist1") "variance").Value.Name
        | Vector -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg2") "vector").Value.Name
        | Vectorselector -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "vector_selector").Value.Name
        | Vectorproduct -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "linalg1") "vectorproduct").Value.Name
        | Xnor -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "xnor").Value.Name
        | Xor -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "xor").Value.Name
        | _ -> ""  
