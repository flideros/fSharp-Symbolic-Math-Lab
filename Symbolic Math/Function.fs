namespace Math.Pure.Objects

open OpenMath

open OpenMath

type Function = 

// Common Arithmetic Functions
    | Lcm
    | Gcd
    | Plus          // binary op
    | UnaryMinus    // Unary op
    | Minus         // binary op
    | Times         // binary op
    | Product       // n-ary op
    | DividedBy     // binary op
    | ToThePowerOf  // binary op
    | ExplicitToThePowerOf 
    | Abs
    | Root
    | Sum           // n-ary op
// Transcendental functions    
    | Ln
    | Log
    | Exp     
    | Sin 
    | Cos
    | Tan    
    | Sec
    | Csc
    | Cot
    | Sinh
    | Cosh
    | Tanh
    | Sech
    | Csch 
    | Coth    
    | ArcCos 
    | ArcCosh
    | ArcCot 
    | ArcCoth
    | ArcCsc 
    | ArcCsch
    | ArcSec 
    | ArcSech
    | ArcSin 
    | ArcSinh
    | ArcTan 
    | ArcTanh 
// Common arithmetic relations
    | Equals
    | LessThan
    | GreaterThan
    | NotEqual
    | LessThanOrEqualTo
    | GreaterThanOrEqualTo
    | ApproximateEquality
// Set functions for basic set theory.   
    | CartesianProduct
    | Map
    | Size
    | SuchThat
    | Set
    | Intersect
    | Union 
    | SetDiff
    | SubSet
    | In
    | NotIn
    | ProperSubset
    | NotSubSet
    | NotProperSubSet
    | Multiset
// Calculus operations
    | AntiDerivative 
    | Derivative 
    | Differentiation
    | NthDifferentiation
    | PartialDifferentiation
    | PartialDifferentiationDegree
    | IndefiniteIntegration
    | DefiniteIntegration
// Complex number operations
    | RealPart
    | ImaginaryPart
    | Argument
    | Conjugate
// Set of functions concerning functions themselves
    | RightCompose
    | RightIinverse
    | LeftCompose
    | LeftInverse
    | Identity
    | Range
    | Restriction
    | Domain
    | DomainOfApplication
    | Image
    | Inverse
    | Kernel
    | ApplyToList
    | PredicateOnList
// Basic integer functions
    | FactorOf
    | Factorial
    | Quotient
    | Remainder
// Discrete and continuous 1-dimensional intervals (with open/closed end points)
    | IntegerInterval     
    | OrientedInterval
    | Interval
    | IntervalCC
    | IntervalCO
    | IntervalOC
    | IntervalOO 
//  Basic notion of the limits of unary functions whilst its variable tend (either from above, below or both sides) to a particular value
    | Limit
// Operations on Matrices (independent of the matrix representation)
    | VectorProduct
    | ScalarProduct
    | OuterProduct
    | MatrixSelector    
    | VectorSelector
    | Transpose
    | Determinant    
    | Vector
    | Matrix
    | MatrixRow
// Basic logic functions
    | Equivalent
    | Not
    | And
    | Nand
    | Nor
    | Xnor
    | Xor
    | Or
    | Implies
// Basic statistical functions used on random variables
    | Min 
    | Max
    | Mean
    | Median
    | Sdev
    | Variance
    | Mode
    | Moment
// Basic statistical functions used on used on sample data
    | DataMean
    | DataMoment
    | DataVariance
    | DataSdev
// Basic rounding concepts 
    | Ceiling
    | Round
    | Trunc
    | Floor 
// Functions which are concerned with vector calculus
    | Curl
    | Divergence
    | Grad
    | Laplacian
// Set of operators for piece-wise defined expressions
    | Piece
    | Piecewise
    | Otherwise

    with // I'm going to do this differently...got to think about it more though...
    member this.definition =
        match this with
        | Abs -> GET.definitionEntry "abs" (FROM.cD "arith1")
        | And -> GET.definitionEntry "and" (FROM.cD "logic1")
        | ApplyToList -> GET.definitionEntry "apply_to_list" (FROM.cD "fns2")
        | ApproximateEquality -> GET.definitionEntry "approx" (FROM.cD "relation1")
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
        | CartesianProduct -> GET.definitionEntry "cartesian_product" (FROM.cD "set1")
        | Ceiling -> GET.definitionEntry "ceiling" (FROM.cD "rounding1")
        | Conjugate -> GET.definitionEntry "conjugate" (FROM.cD "complex1")
        | Cos -> GET.definitionEntry "cos" (FROM.cD "transc1")
        | Cosh -> GET.definitionEntry "cosh" (FROM.cD "transc1")
        | Cot -> GET.definitionEntry "cot" (FROM.cD "transc1")
        | Coth -> GET.definitionEntry "coth" (FROM.cD "transc1")
        | Csc -> GET.definitionEntry "csc" (FROM.cD "transc1")
        | Csch -> GET.definitionEntry "csch" (FROM.cD "transc1")
        | Curl -> GET.definitionEntry "curl" (FROM.cD "veccalc1")
        | DefiniteIntegration -> GET.definitionEntry "defint" (FROM.cD "calculus1")
        | Determinant -> GET.definitionEntry "determinant" (FROM.cD "linalg1")
        | Differentiation -> GET.definitionEntry "diff" (FROM.cD "calculus1")
        | Divergence -> GET.definitionEntry "divergence" (FROM.cD "veccalc1")
        | DividedBy -> GET.definitionEntry "divide" (FROM.cD "arith1")
        | Domain -> GET.definitionEntry "domain" (FROM.cD "fns1")
        | DomainOfApplication -> GET.definitionEntry "domainofapplication" (FROM.cD "fns1")
        | Equals -> GET.definitionEntry "eq" (FROM.cD "relation1")
        | Equivalent -> GET.definitionEntry "equivalent" (FROM.cD "logic1")
        | Exp -> GET.definitionEntry "exp" (FROM.cD "transc1")
        | Factorial -> GET.definitionEntry "factorial" (FROM.cD "integer1")
        | FactorOf -> GET.definitionEntry "factorof" (FROM.cD "integer1")
        | Floor -> GET.definitionEntry "floor" (FROM.cD "rounding1")
        | Gcd -> GET.definitionEntry "gcd" (FROM.cD "arith1")
        | GreaterThanOrEqualTo -> GET.definitionEntry "geq" (FROM.cD "relation1")
        | Grad -> GET.definitionEntry "grad" (FROM.cD "veccalc1")
        | GreaterThan -> GET.definitionEntry "gt" (FROM.cD "relation1")
        | Identity -> GET.definitionEntry "identity" (FROM.cD "fns1")
        | Image -> GET.definitionEntry "image" (FROM.cD "fns1")
        | ImaginaryPart -> GET.definitionEntry "imaginary" (FROM.cD "complex1")
        | Implies -> GET.definitionEntry "implies" (FROM.cD "logic1")
        | In -> GET.definitionEntry "in" (FROM.cD "set1")
        | IndefiniteIntegration -> GET.definitionEntry "int" (FROM.cD "calculus1")
        | IntegerInterval -> GET.definitionEntry "integer_interval" (FROM.cD "interval1")
        | Intersect -> GET.definitionEntry "intersect" (FROM.cD "set1")
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
        | LessThanOrEqualTo -> GET.definitionEntry "leq" (FROM.cD "relation1")
        | Limit -> GET.definitionEntry "limit" (FROM.cD "limit1")
        | Ln -> GET.definitionEntry "ln" (FROM.cD "transc1")
        | Log -> GET.definitionEntry "log" (FROM.cD "transc1")
        | LessThan -> GET.definitionEntry "lt" (FROM.cD "relation1")
        | Map -> GET.definitionEntry "map" (FROM.cD "set1")
        | Matrix -> GET.definitionEntry "matrix" (FROM.cD "linalg2")
        | MatrixSelector -> GET.definitionEntry "matrix_selector" (FROM.cD "linalg1")
        | MatrixRow -> GET.definitionEntry "matrixrow" (FROM.cD "linalg2")
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
        | NotEqual -> GET.definitionEntry "neq" (FROM.cD "relation1")
        | Nor -> GET.definitionEntry "nor" (FROM.cD "logic1")
        | Not -> GET.definitionEntry "not" (FROM.cD "logic1")
        | NotIn -> GET.definitionEntry "notin" (FROM.cD "set1")
        | NotProperSubSet -> GET.definitionEntry "notprsubset" (FROM.cD "set1")
        | NotSubSet -> GET.definitionEntry "notsubset" (FROM.cD "set1")
        | NthDifferentiation -> GET.definitionEntry "nthdiff" (FROM.cD "calculus1")
        | Or -> GET.definitionEntry "or" (FROM.cD "logic1")
        | OrientedInterval -> GET.definitionEntry "oriented_interval" (FROM.cD "interval1")
        | Otherwise -> GET.definitionEntry "otherwise" (FROM.cD "piece1")
        | OuterProduct -> GET.definitionEntry "outerproduct" (FROM.cD "linalg1")
        | PartialDifferentiation -> GET.definitionEntry "partialdiff" (FROM.cD "calculus1")
        | PartialDifferentiationDegree -> GET.definitionEntry "partialdiffdegree" (FROM.cD "calculus1")
        | Piece -> GET.definitionEntry "piece" (FROM.cD "piece1")
        | Piecewise -> GET.definitionEntry "piecewise" (FROM.cD "piece1")
        | Plus -> GET.definitionEntry "plus" (FROM.cD "arith1")
        | ExplicitToThePowerOf -> GET.definitionEntry "power" (FROM.cD "arith1")
        | ToThePowerOf -> GET.definitionEntry "power" (FROM.cD "arith1")
        | PredicateOnList -> GET.definitionEntry "predicate_on_list" (FROM.cD "fns2")
        | Product -> GET.definitionEntry "product" (FROM.cD "arith1")
        | ProperSubset -> GET.definitionEntry "prsubset" (FROM.cD "set1")
        | Quotient -> GET.definitionEntry "quotient" (FROM.cD "integer1")
        | Range -> GET.definitionEntry "range" (FROM.cD "fns1")
        | RealPart -> GET.definitionEntry "real" (FROM.cD "complex1")
        | Remainder -> GET.definitionEntry "remainder" (FROM.cD "integer1")
        | Restriction -> GET.definitionEntry "restriction" (FROM.cD "fns1")
        | RightCompose -> GET.definitionEntry "right_compose" (FROM.cD "fns2")
        | RightIinverse -> GET.definitionEntry "right_inverse" (FROM.cD "fns1")
        | Root -> GET.definitionEntry "root" (FROM.cD "arith1")
        | Round -> GET.definitionEntry "round" (FROM.cD "rounding1")
        | ScalarProduct -> GET.definitionEntry "scalarproduct" (FROM.cD "linalg1")
        | DataSdev -> GET.definitionEntry "sdev" (FROM.cD "s_data1")
        | Sdev -> GET.definitionEntry "sdev" (FROM.cD "s_dist1")
        | Sec -> GET.definitionEntry "sec" (FROM.cD "transc1")
        | Sech -> GET.definitionEntry "sech" (FROM.cD "transc1")
        | Set -> GET.definitionEntry "set" (FROM.cD "set1")
        | SetDiff -> GET.definitionEntry "setdiff" (FROM.cD "set1")
        | Sin -> GET.definitionEntry "sin" (FROM.cD "transc1")
        | Sinh -> GET.definitionEntry "sinh" (FROM.cD "transc1")
        | Size -> GET.definitionEntry "size" (FROM.cD "set1")
        | SubSet -> GET.definitionEntry "subset" (FROM.cD "set1")
        | SuchThat -> GET.definitionEntry "suchthat" (FROM.cD "set1")
        | Sum -> GET.definitionEntry "sum" (FROM.cD "arith1")
        | Tan -> GET.definitionEntry "tan" (FROM.cD "transc1")
        | Tanh -> GET.definitionEntry "tanh" (FROM.cD "transc1")
        | Times -> GET.definitionEntry "times" (FROM.cD "arith1")
        | Transpose -> GET.definitionEntry "transpose" (FROM.cD "linalg1")
        | Trunc -> GET.definitionEntry "trunc" (FROM.cD "rounding1")
        | UnaryMinus -> GET.definitionEntry "unary_minus" (FROM.cD "arith1")
        | Union -> GET.definitionEntry "union" (FROM.cD "set1")
        | DataVariance -> GET.definitionEntry "variance" (FROM.cD "s_data1")
        | Variance -> GET.definitionEntry "variance" (FROM.cD "s_dist1")
        | Vector -> GET.definitionEntry "vector" (FROM.cD "linalg2")
        | VectorSelector -> GET.definitionEntry "vector_selector" (FROM.cD "linalg1")
        | VectorProduct -> GET.definitionEntry "vectorproduct" (FROM.cD "linalg1")
        | Xnor -> GET.definitionEntry "xnor" (FROM.cD "logic1")
        | Xor -> GET.definitionEntry "xor" (FROM.cD "logic1")
        | _ -> None
 
module Function =
    
    let defenition = "TODO"

