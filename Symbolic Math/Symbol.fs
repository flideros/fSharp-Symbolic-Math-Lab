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

    with
    member this.definition =         
        match this with 
        //nums1
        | E -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "e"
        | I -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "i"
        | Pi -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "pi"
        | Gamma -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "gamma"
        | Infinity -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "infinity"
        | NaN -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "NaN"
        
        // setname1    
        | RealNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "R"
        | NaturalNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "N"
        | RationalNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Q"
        | IntegralNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Z"
        | ComplexNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "C"
        | PositivePrimes -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "P"

        
        // set1
        | EmptySet -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "emptyset"
        
        // alg1
        | Zero -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "alg1") "zero"
        | One -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "alg1") "one"
        
        // limit1
        | Null -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "null"
        | BothSides -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "both_sides"
        | Above -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "above"
        | Below -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "below"
        
        // logic1
        | False -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "false"
        | True -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "true"
        
        // mathmltypes
        | ComplexCartesianType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "complex_cartesian_type"
        | ComplexPolarType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "complex_polar_type"
        | FnType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "fn_type"
        | RationalType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "rational_type"
        | SetType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "set_type"
        | VectorType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "vector_type"
        | IntegerType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "integer_type"
        | ConstantType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "constant_type"
        | ListType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "list_type"
        | MatrixType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "matrix_type"
        | RealType -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "real_type"
        
        //multiset1
        | EmptyMultiSet -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "emptyset"
        
        // sts
        | Object -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "Object"
        | SetNumericalValue -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "SetNumericalValue"
        | Error -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "error"
        | Attribution -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "attribution"
        | Binder -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "binder"
        | NumericalValue -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "NumericalValue"
                   
    member this.symbol =         
        match this with         
        // nums1
        | E -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "e").Value.Name
        | I -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "i").Value.Name
        | Pi -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "pi").Value.Name
        | Gamma -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "gamma").Value.Name
        | Infinity -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "infinity").Value.Name
        | NaN -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "NaN").Value.Name
        
        // setname1
        | RealNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "R").Value.Name
        | NaturalNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "N").Value.Name
        | RationalNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Q").Value.Name
        | IntegralNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Z").Value.Name
        | ComplexNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "C").Value.Name
        | PositivePrimes -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "P").Value.Name

        
        // set
        | EmptySet -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "emptyset").Value.Name
        
        // alg1
        | Zero -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "alg1") "zero").Value.Name
        | One -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "alg1") "one").Value.Name
        
        // limit1
        | Null -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "null").Value.Name
        | BothSides -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "both_sides").Value.Name
        | Above -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "above").Value.Name
        | Below -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "limit1") "below").Value.Name
        
        // logical1
        | False -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "false").Value.Name
        | True -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "logic1") "true").Value.Name
        
        // mathmltypes
        | ComplexCartesianType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "complex_cartesian_type").Value.Name
        | ComplexPolarType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "complex_polar_type").Value.Name
        | FnType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "fn_type").Value.Name
        | RationalType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "rational_type").Value.Name
        | SetType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "set_type").Value.Name
        | VectorType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "vector_type").Value.Name
        | IntegerType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "integer_type").Value.Name
        | ConstantType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "constant_type").Value.Name
        | ListType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "list_type").Value.Name
        | MatrixType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "matrix_type").Value.Name
        | RealType -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "mathmltypes") "real_type").Value.Name
        
        // multiset1
        | EmptyMultiSet -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "multiset1") "emptyset").Value.Name
        
        // sts
        | Object -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "Object").Value.Name
        | SetNumericalValue -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "SetNumericalValue").Value.Name
        | Error -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "error").Value.Name
        | Attribution -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "attribution").Value.Name
        | Binder -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "binder").Value.Name
        | NumericalValue -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "sts") "NumericalValue").Value.Name

type Symbol = 
    | Constant of Constant
    | Variable of string
    | Inconsistent
    with
    member this.definition = 
        match this with
        | Constant _ -> ConstantType.definition
        | Variable _ -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "prog1") "local_var"
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




    