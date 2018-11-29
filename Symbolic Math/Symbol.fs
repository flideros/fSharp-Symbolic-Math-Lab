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
        | E -> GET.definitionEntry (FROM.cD "nums1") "e"
        | I -> GET.definitionEntry (FROM.cD "nums1") "i"
        | Pi -> GET.definitionEntry (FROM.cD "nums1") "pi"
        | Gamma -> GET.definitionEntry (FROM.cD "nums1") "gamma"
        | Infinity -> GET.definitionEntry (FROM.cD "nums1") "infinity"
        | NaN -> GET.definitionEntry (FROM.cD "nums1") "NaN"
        
        // setname1
        | RealNumbers -> GET.definitionEntry (FROM.cD "setname1") "R"
        | NaturalNumbers -> GET.definitionEntry (FROM.cD "setname1") "N"
        | RationalNumbers -> GET.definitionEntry (FROM.cD "setname1") "Q"
        | IntegralNumbers -> GET.definitionEntry (FROM.cD "setname1") "Z"
        | ComplexNumbers -> GET.definitionEntry (FROM.cD "setname1") "C"
        | PositivePrimes -> GET.definitionEntry (FROM.cD "setname1") "P"
        
        // set1
        | EmptySet -> GET.definitionEntry (FROM.cD "set1") "emptyset"
        
        // alg1
        | Zero -> GET.definitionEntry (FROM.cD "alg1") "zero"
        | One -> GET.definitionEntry (FROM.cD "alg1") "one"
        
        // limit1
        | Null -> GET.definitionEntry (FROM.cD "limit1") "null"
        | BothSides -> GET.definitionEntry (FROM.cD "limit1") "both_sides"
        | Above -> GET.definitionEntry (FROM.cD "limit1") "above"
        | Below -> GET.definitionEntry (FROM.cD "limit1") "below"
        
        // logic1
        | False -> GET.definitionEntry (FROM.cD "logic1") "false"
        | True -> GET.definitionEntry (FROM.cD "logic1") "true"
        
        // mathmltypes
        | ComplexCartesianType -> GET.definitionEntry (FROM.cD "mathmltypes") "complex_cartesian_type"
        | ComplexPolarType -> GET.definitionEntry (FROM.cD "mathmltypes") "complex_polar_type"
        | FnType -> GET.definitionEntry (FROM.cD "mathmltypes") "fn_type"
        | RationalType -> GET.definitionEntry (FROM.cD "mathmltypes") "rational_type"
        | SetType -> GET.definitionEntry (FROM.cD "mathmltypes") "set_type"
        | VectorType -> GET.definitionEntry (FROM.cD "mathmltypes") "vector_type"
        | IntegerType -> GET.definitionEntry (FROM.cD "mathmltypes") "integer_type"
        | ConstantType -> GET.definitionEntry (FROM.cD "mathmltypes") "constant_type"
        | ListType -> GET.definitionEntry (FROM.cD "mathmltypes") "list_type"
        | MatrixType -> GET.definitionEntry (FROM.cD "mathmltypes") "matrix_type"
        | RealType -> GET.definitionEntry (FROM.cD "mathmltypes") "real_type"
        
        //multiset1
        | EmptyMultiSet -> GET.definitionEntry (FROM.cD "multiset1") "emptyset"
        
        // sts
        | Object -> GET.definitionEntry (FROM.cD "sts") "Object"
        | SetNumericalValue -> GET.definitionEntry (FROM.cD "sts") "SetNumericalValue"
        | Error -> GET.definitionEntry (FROM.cD "sts") "error"
        | Attribution -> GET.definitionEntry (FROM.cD "sts") "attribution"
        | Binder -> GET.definitionEntry (FROM.cD "sts") "binder"
        | NumericalValue -> GET.definitionEntry (FROM.cD "sts") "NumericalValue"
                   
    member this.symbol =         
        match this with         
        // nums1

        | E -> (GET.definitionEntry (FROM.cD "nums1") "e").Value.Name
        | I -> (GET.definitionEntry (FROM.cD "nums1") "i").Value.Name
        | Pi -> (GET.definitionEntry (FROM.cD "nums1") "pi").Value.Name
        | Gamma -> (GET.definitionEntry (FROM.cD "nums1") "gamma").Value.Name
        | Infinity -> (GET.definitionEntry (FROM.cD "nums1") "infinity").Value.Name
        | NaN -> (GET.definitionEntry (FROM.cD "nums1") "NaN").Value.Name
        
        // setname1
        | RealNumbers -> (GET.definitionEntry (FROM.cD "setname1") "R").Value.Name
        | NaturalNumbers -> (GET.definitionEntry (FROM.cD "setname1") "N").Value.Name
        | RationalNumbers -> (GET.definitionEntry (FROM.cD "setname1") "Q").Value.Name
        | IntegralNumbers -> (GET.definitionEntry (FROM.cD "setname1") "Z").Value.Name
        | ComplexNumbers -> (GET.definitionEntry (FROM.cD "setname1") "C").Value.Name
        | PositivePrimes -> (GET.definitionEntry (FROM.cD "setname1") "P").Value.Name
        
        // set
        | EmptySet -> (GET.definitionEntry (FROM.cD "set1") "emptyset").Value.Name
        
        // alg1
        | Zero -> (GET.definitionEntry (FROM.cD "alg1") "zero").Value.Name
        | One -> (GET.definitionEntry (FROM.cD "alg1") "one").Value.Name
        
        // limit1
        | Null -> (GET.definitionEntry (FROM.cD "limit1") "null").Value.Name
        | BothSides -> (GET.definitionEntry (FROM.cD "limit1") "both_sides").Value.Name
        | Above -> (GET.definitionEntry (FROM.cD "limit1") "above").Value.Name
        | Below -> (GET.definitionEntry (FROM.cD "limit1") "below").Value.Name
        
        // logical1
        | False -> (GET.definitionEntry (FROM.cD "logic1") "false").Value.Name
        | True -> (GET.definitionEntry (FROM.cD "logic1") "true").Value.Name
        
        // mathmltypes
        | ComplexCartesianType -> (GET.definitionEntry (FROM.cD "mathmltypes") "complex_cartesian_type").Value.Name
        | ComplexPolarType -> (GET.definitionEntry (FROM.cD "mathmltypes") "complex_polar_type").Value.Name
        | FnType -> (GET.definitionEntry (FROM.cD "mathmltypes") "fn_type").Value.Name
        | RationalType -> (GET.definitionEntry (FROM.cD "mathmltypes") "rational_type").Value.Name
        | SetType -> (GET.definitionEntry (FROM.cD "mathmltypes") "set_type").Value.Name
        | VectorType -> (GET.definitionEntry (FROM.cD "mathmltypes") "vector_type").Value.Name
        | IntegerType -> (GET.definitionEntry (FROM.cD "mathmltypes") "integer_type").Value.Name
        | ConstantType -> (GET.definitionEntry (FROM.cD "mathmltypes") "constant_type").Value.Name
        | ListType -> (GET.definitionEntry (FROM.cD "mathmltypes") "list_type").Value.Name
        | MatrixType -> (GET.definitionEntry (FROM.cD "mathmltypes") "matrix_type").Value.Name
        | RealType -> (GET.definitionEntry (FROM.cD "mathmltypes") "real_type").Value.Name
        
        // multiset1
        | EmptyMultiSet -> (GET.definitionEntry (FROM.cD "multiset1") "emptyset").Value.Name
        
        // sts
        | Object -> (GET.definitionEntry (FROM.cD "sts") "Object").Value.Name
        | SetNumericalValue -> (GET.definitionEntry (FROM.cD "sts") "SetNumericalValue").Value.Name
        | Error -> (GET.definitionEntry (FROM.cD "sts") "error").Value.Name
        | Attribution -> (GET.definitionEntry (FROM.cD "sts") "attribution").Value.Name
        | Binder -> (GET.definitionEntry (FROM.cD "sts") "binder").Value.Name
        | NumericalValue -> (GET.definitionEntry (FROM.cD "sts") "NumericalValue").Value.Name

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




    