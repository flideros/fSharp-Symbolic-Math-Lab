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
        
    with
    member this.definition = 
        // Content Dictionaries
        match this with 
        | E -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "e"
        | I -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "i"
        | Pi -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "pi"
        | RealNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "R"
        | NaturalNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "N"
        | RationalNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Q"
        | IntegralNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Z"
        | ComplexNumbers -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "C"
        | PositivePrimes -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "P"
        | EmptySet -> OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "emptyset"
    
    member this.symbol = 
        // Content Dictionaries
        match this with 
        | E -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "e").Value.Name
        | I -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "i").Value.Name
        | Pi -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "pi").Value.Name
        | RealNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "R").Value.Name
        | NaturalNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "N").Value.Name
        | RationalNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Q").Value.Name
        | IntegralNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "Z").Value.Name
        | ComplexNumbers -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "C").Value.Name
        | PositivePrimes -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "setname1") "P").Value.Name
        | EmptySet -> (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "set1") "emptyset").Value.Name

type Symbol = 
    | Constant of Constant
    | Variable of string
    | Inconsistent
    
type Result<'T> =
    | Pass of 'T
    | Fail
    with
    member this.value = 
        let v = match this with
                | Pass t -> Some t
                | Fail -> None
        v.Value




    