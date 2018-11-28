namespace Math.Pure.Objects

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
        | E -> GET.definitionEntry (GET.definitions "nums1") "e"
        | I -> GET.definitionEntry (GET.definitions "nums1") "i"
        | Pi -> GET.definitionEntry (GET.definitions "nums1") "pi"
        | Gamma -> GET.definitionEntry (GET.definitions "nums1") "gamma"
        | Infinity -> GET.definitionEntry (GET.definitions "nums1") "infinity"
        | NaN -> GET.definitionEntry (GET.definitions "nums1") "NaN"
        
        // setname1
        | RealNumbers -> GET.definitionEntry (GET.definitions "setname1") "R"
        | NaturalNumbers -> GET.definitionEntry (GET.definitions "setname1") "N"
        | RationalNumbers -> GET.definitionEntry (GET.definitions "setname1") "Q"
        | IntegralNumbers -> GET.definitionEntry (GET.definitions "setname1") "Z"
        | ComplexNumbers -> GET.definitionEntry (GET.definitions "setname1") "C"
        | PositivePrimes -> GET.definitionEntry (GET.definitions "setname1") "P"
        
        // set1
        | EmptySet -> GET.definitionEntry (GET.definitions "set1") "emptyset"
        
        // alg1
        | Zero -> GET.definitionEntry (GET.definitions "alg1") "zero"
        | One -> GET.definitionEntry (GET.definitions "alg1") "one"
        
        // limit1
        | Null -> GET.definitionEntry (GET.definitions "limit1") "null"
        | BothSides -> GET.definitionEntry (GET.definitions "limit1") "both_sides"
        | Above -> GET.definitionEntry (GET.definitions "limit1") "above"
        | Below -> GET.definitionEntry (GET.definitions "limit1") "below"
        
        // logic1
        | False -> GET.definitionEntry (GET.definitions "logic1") "false"
        | True -> GET.definitionEntry (GET.definitions "logic1") "true"
        
        // mathmltypes
        | ComplexCartesianType -> GET.definitionEntry (GET.definitions "mathmltypes") "complex_cartesian_type"
        | ComplexPolarType -> GET.definitionEntry (GET.definitions "mathmltypes") "complex_polar_type"
        | FnType -> GET.definitionEntry (GET.definitions "mathmltypes") "fn_type"
        | RationalType -> GET.definitionEntry (GET.definitions "mathmltypes") "rational_type"
        | SetType -> GET.definitionEntry (GET.definitions "mathmltypes") "set_type"
        | VectorType -> GET.definitionEntry (GET.definitions "mathmltypes") "vector_type"
        | IntegerType -> GET.definitionEntry (GET.definitions "mathmltypes") "integer_type"
        | ConstantType -> GET.definitionEntry (GET.definitions "mathmltypes") "constant_type"
        | ListType -> GET.definitionEntry (GET.definitions "mathmltypes") "list_type"
        | MatrixType -> GET.definitionEntry (GET.definitions "mathmltypes") "matrix_type"
        | RealType -> GET.definitionEntry (GET.definitions "mathmltypes") "real_type"
        
        //multiset1
        | EmptyMultiSet -> GET.definitionEntry (GET.definitions "multiset1") "emptyset"
        
        // sts
        | Object -> GET.definitionEntry (GET.definitions "sts") "Object"
        | SetNumericalValue -> GET.definitionEntry (GET.definitions "sts") "SetNumericalValue"
        | Error -> GET.definitionEntry (GET.definitions "sts") "error"
        | Attribution -> GET.definitionEntry (GET.definitions "sts") "attribution"
        | Binder -> GET.definitionEntry (GET.definitions "sts") "binder"
        | NumericalValue -> GET.definitionEntry (GET.definitions "sts") "NumericalValue"
                   
    member this.symbol =         
        match this with         
        // nums1
        | E -> (GET.definitionEntry (GET.definitions "nums1") "e").Value.Name
        | I -> (GET.definitionEntry (GET.definitions "nums1") "i").Value.Name
        | Pi -> (GET.definitionEntry (GET.definitions "nums1") "pi").Value.Name
        | Gamma -> (GET.definitionEntry (GET.definitions "nums1") "gamma").Value.Name
        | Infinity -> (GET.definitionEntry (GET.definitions "nums1") "infinity").Value.Name
        | NaN -> (GET.definitionEntry (GET.definitions "nums1") "NaN").Value.Name
        
        // setname1
        | RealNumbers -> (GET.definitionEntry (GET.definitions "setname1") "R").Value.Name
        | NaturalNumbers -> (GET.definitionEntry (GET.definitions "setname1") "N").Value.Name
        | RationalNumbers -> (GET.definitionEntry (GET.definitions "setname1") "Q").Value.Name
        | IntegralNumbers -> (GET.definitionEntry (GET.definitions "setname1") "Z").Value.Name
        | ComplexNumbers -> (GET.definitionEntry (GET.definitions "setname1") "C").Value.Name
        | PositivePrimes -> (GET.definitionEntry (GET.definitions "setname1") "P").Value.Name
        
        // set
        | EmptySet -> (GET.definitionEntry (GET.definitions "set1") "emptyset").Value.Name
        
        // alg1
        | Zero -> (GET.definitionEntry (GET.definitions "alg1") "zero").Value.Name
        | One -> (GET.definitionEntry (GET.definitions "alg1") "one").Value.Name
        
        // limit1
        | Null -> (GET.definitionEntry (GET.definitions "limit1") "null").Value.Name
        | BothSides -> (GET.definitionEntry (GET.definitions "limit1") "both_sides").Value.Name
        | Above -> (GET.definitionEntry (GET.definitions "limit1") "above").Value.Name
        | Below -> (GET.definitionEntry (GET.definitions "limit1") "below").Value.Name
        
        // logical1
        | False -> (GET.definitionEntry (GET.definitions "logic1") "false").Value.Name
        | True -> (GET.definitionEntry (GET.definitions "logic1") "true").Value.Name
        
        // mathmltypes
        | ComplexCartesianType -> (GET.definitionEntry (GET.definitions "mathmltypes") "complex_cartesian_type").Value.Name
        | ComplexPolarType -> (GET.definitionEntry (GET.definitions "mathmltypes") "complex_polar_type").Value.Name
        | FnType -> (GET.definitionEntry (GET.definitions "mathmltypes") "fn_type").Value.Name
        | RationalType -> (GET.definitionEntry (GET.definitions "mathmltypes") "rational_type").Value.Name
        | SetType -> (GET.definitionEntry (GET.definitions "mathmltypes") "set_type").Value.Name
        | VectorType -> (GET.definitionEntry (GET.definitions "mathmltypes") "vector_type").Value.Name
        | IntegerType -> (GET.definitionEntry (GET.definitions "mathmltypes") "integer_type").Value.Name
        | ConstantType -> (GET.definitionEntry (GET.definitions "mathmltypes") "constant_type").Value.Name
        | ListType -> (GET.definitionEntry (GET.definitions "mathmltypes") "list_type").Value.Name
        | MatrixType -> (GET.definitionEntry (GET.definitions "mathmltypes") "matrix_type").Value.Name
        | RealType -> (GET.definitionEntry (GET.definitions "mathmltypes") "real_type").Value.Name
        
        // multiset1
        | EmptyMultiSet -> (GET.definitionEntry (GET.definitions "multiset1") "emptyset").Value.Name
        
        // sts
        | Object -> (GET.definitionEntry (GET.definitions "sts") "Object").Value.Name
        | SetNumericalValue -> (GET.definitionEntry (GET.definitions "sts") "SetNumericalValue").Value.Name
        | Error -> (GET.definitionEntry (GET.definitions "sts") "error").Value.Name
        | Attribution -> (GET.definitionEntry (GET.definitions "sts") "attribution").Value.Name
        | Binder -> (GET.definitionEntry (GET.definitions "sts") "binder").Value.Name
        | NumericalValue -> (GET.definitionEntry (GET.definitions "sts") "NumericalValue").Value.Name

type Symbol = 
    | Constant of Constant
    | Variable of string
    | Inconsistent
    with
    member this.definition = 
        match this with
        | Constant _ -> ConstantType.definition
        | Variable _ -> None //GET.definitionEntry (GET.definitions "prog1") "local_var"
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
