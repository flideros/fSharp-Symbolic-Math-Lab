namespace Math.Pure.Objects

type Constant =
    | E
    | I
    | Pi
    | RealNumbers
    | NaturalNumbers
    | RationalNumbers
    | IntegralNumbers
    | ComplexNumbers
    | PositivePrimes
    | EmptySet
    | Zero
    | One
    | Null
    | BothSides
    | Above
    | Below
    | False
    | True
    | ComplexCartesianType
    | ComplexPolarType
    | FnType
    | RationalType
    | SetType
    | VectorType
    | IntegerType
    | ConstantType
    | ListType
    | MatrixType
    | RealType
    | EmptyMultiSet
    | Gamma
    | Infinity
    | NaN
    | Object
    | SetNumericalValue
    | Error
    | Attribution
    | Binder
    | NumericalValue
    
type Error =
    | DivideByZeroError
    | OtherError
    | LazyCoder

type Symbol = 
    | Constant of Constant
    | Variable of string
    | Error of Error
    | Inconsistent
    

type Result<'T> =
    | Pass of 'T
    | Fail of 'T
    with
    member this.value = 
        let v = match this with
                | Pass t -> Some t
                | Fail f -> Some f
        v.Value
    


    