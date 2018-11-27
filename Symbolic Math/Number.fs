namespace Math.Pure.Quantity

type Fraction = 
    {numerator:System.Numerics.BigInteger; denominator:System.Numerics.BigInteger}
    with
    member this.compareTo that = 
        match (this.numerator*that.denominator) > (this.denominator*that.numerator) with
        | true -> 1
        | false -> match (this.numerator*that.denominator) = (this.denominator*that.numerator) with
                   | true -> 0
                   | false -> -1
    member this.Floor = 
        match this.numerator > 0I with
        | true -> this.numerator / this.denominator
        | false -> (this.numerator / this.denominator) - 1I   

    member this.definition = (OpenMath.GET.definitionEntry (OpenMath.GET.definitions "nums1") "rational")
    
    member this.Ceiling = 
        match this.numerator > 0I with
        | true -> match snd (System.Numerics.BigInteger.DivRem (this.numerator, this.denominator)) <> 0I with
                  | true -> (this.numerator / this.denominator) + 1I
                  | false -> this.numerator / this.denominator
        | false -> match snd (System.Numerics.BigInteger.DivRem (this.numerator, this.denominator)) <> 0I with
                   | true -> (this.numerator / this.denominator) 
                   | false -> (this.numerator / this.denominator) - 1I

    static member Zero = {numerator = 0I; denominator = 1I}


[<StructuralEquality;NoComparison>]
type NumberType =
    | Complex of System.Numerics.Complex
    | Rational of Fraction 
    | Integer of System.Numerics.BigInteger 
    | Real of float
    | PositiveInfinity
    | NegativeInfinity
    | ComplexInfinity
    | Undefined

module Number =

    let rec hcf x y = System.Numerics.BigInteger.GreatestCommonDivisor (x, y)

    let compare x y =
        match x,y with
        | Integer x, NegativeInfinity -> 1
        | Rational x, NegativeInfinity -> 1
        | NegativeInfinity, Integer x  -> -1
        | NegativeInfinity, Rational x  -> -1
        | NegativeInfinity, NegativeInfinity -> 0

        | Integer x, PositiveInfinity -> -1
        | Rational x, PositiveInfinity -> -1
        | PositiveInfinity, Integer x  -> 1
        | PositiveInfinity, Rational x  -> 1
        | PositiveInfinity, PositiveInfinity -> 0

        | Integer x, Integer y when x > y -> 1
        | Integer x, Integer y when x = y -> 0
        | Integer x, Integer y when x < y -> -1

        | Rational x, Rational y when y.numerator * x.denominator > x.numerator * y.denominator -> -1
        | Rational x, Rational y when y.numerator * x.denominator < x.numerator * y.denominator -> 1
        | Rational x, Rational y when y.numerator * x.denominator = x.numerator * y.denominator -> 0

        | Integer x, Rational y when y.numerator > x * y.denominator -> -1
        | Integer x, Rational y when y.numerator < x * y.denominator -> 1
        | Integer x, Rational y when y.numerator = x * y.denominator -> 0
        | Rational x, Integer y when x.numerator > y * x.denominator -> 1
        | Rational x, Integer y when x.numerator < y * x.denominator -> -1
        | Rational x, Integer y when x.numerator = y * x.denominator -> 0
        | _ -> 0
    
    let abs x = 
        match x with
        | Integer x -> Integer (abs x)
        | Complex x -> Real (System.Numerics.Complex.Abs x)
        | Rational x -> Rational {numerator = abs x.numerator; denominator = abs x.denominator}
        | Real x -> Real (abs x)
        | PositiveInfinity -> PositiveInfinity
        | NegativeInfinity -> PositiveInfinity
        | _ -> Undefined    
    
    let floor x = 
        match x with
        | Integer i -> x
        | Rational r -> Integer (r.Floor)
        | Real r -> Real (floor r)
        | _ -> Undefined    

    let ceiling x = 
        match x with
        | Integer i -> x
        | Rational r -> Integer (r.Ceiling)
        | Real r -> Real (ceil r)
        | _ -> Undefined      

    let min x y =
        match compare x y with
        | 1 -> y
        | -1 -> x
        | _ -> x

    let max x y =
        match compare x y with
        | 1 -> x
        | -1 -> y
        | _ -> x

    let One = Integer 1I
   
    let Zero = Integer 0I

type NumberType with    
    member this.isNegative = 
        match this with
        | Complex x when x.Real < 0.0 -> true
        | Rational x when x.numerator < 0I -> true
        | Integer x when x < 0I -> true
        | Real x when x < 0.0 -> true
        | NegativeInfinity -> true
        | _ -> false    
    static member (~-) x = 
        match x with
        | Complex x -> Complex (-x) 
        | Integer x -> Integer (-x)
        | Rational f -> Rational {f with numerator = -f.numerator}
        | PositiveInfinity -> NegativeInfinity
        | NegativeInfinity -> PositiveInfinity
        | _ -> Undefined //Use until all defenitions are made    
    static member (+) (x, y) = 
        match x, y with
        | Complex x, Complex y -> Complex (x + y) 
        | Integer x, Integer y -> Integer (x + y)
        | Rational f1, Rational f2 -> 
            let nTemp = f1.numerator * f2.denominator + f2.numerator * f1.denominator
            let dTemp = f1.denominator * f2.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | Rational f1, Integer i2 -> 
            let nTemp = f1.numerator + i2 * f1.denominator
            let dTemp = f1.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | Integer i2, Rational f1 -> 
            let nTemp = f1.numerator + i2 * f1.denominator
            let dTemp = f1.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | PositiveInfinity, PositiveInfinity -> PositiveInfinity
        | NegativeInfinity, NegativeInfinity -> NegativeInfinity
        | NegativeInfinity, PositiveInfinity -> Undefined
        | PositiveInfinity, NegativeInfinity -> Undefined
        | _ -> Undefined //Use until all defenitions are made    
    static member (*) (x, y) = 
        match x, y with
        | Complex x, Complex y -> Complex (x * y) 
        | Integer x, Integer y -> Integer (x * y)
        | Rational f1, Rational f2 -> 
            let nTemp = f1.numerator * f2.numerator 
            let dTemp = f1.denominator * f2.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | Rational f1, Integer i2 -> 
            let nTemp = f1.numerator * i2 * f1.denominator
            let dTemp = f1.denominator * f1.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | Integer i2, Rational f1 -> 
            let nTemp = f1.numerator * i2 * f1.denominator
            let dTemp = f1.denominator * f1.denominator
            let hcfTemp = Number.hcf nTemp dTemp
            match dTemp / hcfTemp = 1I with
            | true -> Integer (nTemp / hcfTemp)
            | false -> Rational { numerator = nTemp / hcfTemp; denominator = dTemp / hcfTemp }
        | PositiveInfinity, PositiveInfinity -> PositiveInfinity
        | NegativeInfinity, NegativeInfinity -> NegativeInfinity
        | NegativeInfinity, PositiveInfinity -> Undefined
        | PositiveInfinity, NegativeInfinity -> Undefined
        | _ -> Undefined //Use until all defenitions are made    
    static member Pow (x, y) = 
        match x, y with
        | Complex x, Complex y -> Complex (x ** y) 
        | Integer x, Integer y when y >= 0I-> Integer (x ** int y)
        | Integer x, Integer y when y < 0I-> 
            Rational {numerator = 1I * System.Numerics.BigInteger x.Sign; denominator = ((abs x) ** int (abs y))}
        | Rational r, Integer i when i >= 0I -> 
            Rational {numerator = r.numerator ** int i; denominator = r.denominator ** int i}
        | Rational r, Integer i when i < 0I -> 
            Rational {numerator = ((System.Numerics.BigInteger r.numerator.Sign) * r.denominator) ** 
                                  int (abs i); denominator = (abs r.numerator) ** int (abs i)}
        | PositiveInfinity, PositiveInfinity -> PositiveInfinity
        | NegativeInfinity, NegativeInfinity -> NegativeInfinity
        | NegativeInfinity, PositiveInfinity -> Undefined
        | PositiveInfinity, NegativeInfinity -> Undefined
        | _ -> Undefined //Use until all defenitions are made     
    static member (!*) x = 
        let rec factorial n =
            match n with
            | Integer x when x = 0I || x = 1I -> Integer 1I
            | Integer x when x > 1I -> n * factorial (n + -(Integer 1I))
            | Integer x when x < 0I -> n * factorial (-n + -(Integer 1I))
            | _ -> Undefined
        factorial x


