namespace Math.Pure.Objects

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
