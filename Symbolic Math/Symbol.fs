namespace Math.Pure.Objects

open OpenMath
open FSharp.Data

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

    member this.symbol =         
        match this with 
        //nums1
        | E -> (GET.definitionEntry "e" (FROM.cD "nums1")).Value.Name
        | I -> (GET.definitionEntry "i" (FROM.cD "nums1")).Value.Name
        | Pi -> (GET.definitionEntry "pi" (FROM.cD "nums1")).Value.Name
        | Gamma -> (GET.definitionEntry "gamma" (FROM.cD "nums1")).Value.Name
        | Infinity -> (GET.definitionEntry "infinity" (FROM.cD "nums1")).Value.Name
        | NaN -> (GET.definitionEntry "NaN" (FROM.cD "nums1")).Value.Name

        //setname1
        | RealNumbers -> (GET.definitionEntry "R" (FROM.cD "setname1")).Value.Name
        | NaturalNumbers -> (GET.definitionEntry "N" (FROM.cD "setname1")).Value.Name
        | RationalNumbers -> (GET.definitionEntry "Q" (FROM.cD "setname1")).Value.Name
        | IntegralNumbers -> (GET.definitionEntry "Z" (FROM.cD "setname1")).Value.Name
        | ComplexNumbers -> (GET.definitionEntry "C" (FROM.cD "setname1")).Value.Name
        | PositivePrimes -> (GET.definitionEntry "P" (FROM.cD "setname1")).Value.Name

        //set1
        | EmptySet -> (GET.definitionEntry "emptyset" (FROM.cD "set1")).Value.Name

        //alg1
        | Zero -> (GET.definitionEntry "zero" (FROM.cD "alg1")).Value.Name
        | One -> (GET.definitionEntry "one" (FROM.cD "alg1")).Value.Name

        //limit1
        | Null -> (GET.definitionEntry "null" (FROM.cD "limit1")).Value.Name
        | BothSides -> (GET.definitionEntry "both_sides" (FROM.cD "limit1")).Value.Name
        | Above -> (GET.definitionEntry "above" (FROM.cD "limit1")).Value.Name
        | Below -> (GET.definitionEntry "below" (FROM.cD "limit1")).Value.Name

        //logic1
        | False -> (GET.definitionEntry "False" (FROM.cD "logic1")).Value.Name
        | True -> (GET.definitionEntry "True" (FROM.cD "logic1")).Value.Name

        //mathmltypes
        | ComplexCartesianType -> (GET.definitionEntry "complex_cartesian_type" (FROM.cD "mathmltypes")).Value.Name
        | ComplexPolarType -> (GET.definitionEntry "complex_polar_type" (FROM.cD "mathmltypes")).Value.Name
        | FnType -> (GET.definitionEntry "fn_type" (FROM.cD "mathmltypes")).Value.Name
        | RationalType -> (GET.definitionEntry "rational_type" (FROM.cD "mathmltypes")).Value.Name
        | SetType -> (GET.definitionEntry "set_type" (FROM.cD "mathmltypes")).Value.Name
        | VectorType -> (GET.definitionEntry "vector_type" (FROM.cD "mathmltypes")).Value.Name
        | IntegerType -> (GET.definitionEntry "integer_type" (FROM.cD "mathmltypes")).Value.Name
        | ConstantType -> (GET.definitionEntry "constant_type" (FROM.cD "mathmltypes")).Value.Name
        | ListType -> (GET.definitionEntry "list_type" (FROM.cD "mathmltypes")).Value.Name
        | MatrixType -> (GET.definitionEntry "matrix_type" (FROM.cD "mathmltypes")).Value.Name
        | RealType -> (GET.definitionEntry "real_type" (FROM.cD "mathmltypes")).Value.Name

        //multiset1
        | EmptyMultiSet -> (GET.definitionEntry "emptyset" (FROM.cD "multiset1")).Value.Name

        //sts
        | Object -> (GET.definitionEntry "Object" (FROM.cD "sts")).Value.Name
        | SetNumericalValue -> (GET.definitionEntry "SetNumericalValue" (FROM.cD "sts")).Value.Name
        | Error -> (GET.definitionEntry "error" (FROM.cD "sts")).Value.Name
        | Attribution -> (GET.definitionEntry "attribution" (FROM.cD "sts")).Value.Name
        | Binder -> (GET.definitionEntry "binder" (FROM.cD "sts")).Value.Name
        | NumericalValue -> (GET.definitionEntry "NumericalValue" (FROM.cD "sts")).Value.Name

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




    