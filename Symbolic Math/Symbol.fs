namespace Math.Pure.Objects

open OpenMath

open OpenMath

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
    with
    member this.definition =         
        match this with 
        //nums1
        | E -> GET.definitionEntry "e" (FROM.cD "nums1")
        | I -> GET.definitionEntry "i" (FROM.cD "nums1")
        | Pi -> GET.definitionEntry "pi" (FROM.cD "nums1")
        | Gamma -> GET.definitionEntry "gamma" (FROM.cD "nums1")
        | Infinity -> GET.definitionEntry "infinity" (FROM.cD "nums1")
        | NaN -> GET.definitionEntry "NaN" (FROM.cD "nums1")

        //setname1
        | RealNumbers -> GET.definitionEntry "R" (FROM.cD "setname1")
        | NaturalNumbers -> GET.definitionEntry "N" (FROM.cD "setname1")
        | RationalNumbers -> GET.definitionEntry "Q" (FROM.cD "setname1")
        | IntegralNumbers -> GET.definitionEntry "Z" (FROM.cD "setname1")
        | ComplexNumbers -> GET.definitionEntry "C" (FROM.cD "setname1")
        | PositivePrimes -> GET.definitionEntry "P" (FROM.cD "setname1")

        //set1
        | EmptySet -> GET.definitionEntry "emptyset" (FROM.cD "set1")

        //alg1
        | Zero -> GET.definitionEntry "zero" (FROM.cD "alg1")
        | One -> GET.definitionEntry "one" (FROM.cD "alg1")

        //limit1
        | Null -> GET.definitionEntry "null" (FROM.cD "limit1")
        | BothSides -> GET.definitionEntry "both_sides" (FROM.cD "limit1")
        | Above -> GET.definitionEntry "above" (FROM.cD "limit1")
        | Below -> GET.definitionEntry "below" (FROM.cD "limit1")

        //logic1
        | False -> GET.definitionEntry "False" (FROM.cD "logic1")
        | True -> GET.definitionEntry "True" (FROM.cD "logic1")

        //mathmltypes
        | ComplexCartesianType -> GET.definitionEntry "complex_cartesian_type" (FROM.cD "mathmltypes")
        | ComplexPolarType -> GET.definitionEntry "complex_polar_type" (FROM.cD "mathmltypes")
        | FnType -> GET.definitionEntry "fn_type" (FROM.cD "mathmltypes")
        | RationalType -> GET.definitionEntry "rational_type" (FROM.cD "mathmltypes")
        | SetType -> GET.definitionEntry "set_type" (FROM.cD "mathmltypes")
        | VectorType -> GET.definitionEntry "vector_type" (FROM.cD "mathmltypes")
        | IntegerType -> GET.definitionEntry "integer_type" (FROM.cD "mathmltypes")
        | ConstantType -> GET.definitionEntry "constant_type" (FROM.cD "mathmltypes")
        | ListType -> GET.definitionEntry "list_type" (FROM.cD "mathmltypes")
        | MatrixType -> GET.definitionEntry "matrix_type" (FROM.cD "mathmltypes")
        | RealType -> GET.definitionEntry "real_type" (FROM.cD "mathmltypes")

        //multiset1
        | EmptyMultiSet -> GET.definitionEntry "emptyset" (FROM.cD "multiset1")

        //sts
        | Object -> GET.definitionEntry "Object" (FROM.cD "sts")
        | SetNumericalValue -> GET.definitionEntry "SetNumericalValue" (FROM.cD "sts")
        | Error -> GET.definitionEntry "error" (FROM.cD "sts")
        | Attribution -> GET.definitionEntry "attribution" (FROM.cD "sts")
        | Binder -> GET.definitionEntry "binder" (FROM.cD "sts")
        | NumericalValue -> GET.definitionEntry "NumericalValue" (FROM.cD "sts")

type Symbol = 
    | Constant of Constant
    | Variable of string
    | Inconsistent
    with
    member this.definition = 
        match this with
        | Constant _ -> ConstantType.definition
        | Variable _ -> None //GET.definitionEntry (FROM.cD "prog1") "local_var"
        | Inconsistent -> Error.definition

type Result<'T> =
    | Pass of 'T
    | Fail
    with
    member this.value = 
        let v = match this with
                | Pass t -> Some t
                | Fail -> None
        v.Value




    