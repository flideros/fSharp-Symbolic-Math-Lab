namespace Math.Pure.Structure

module NumberTheory = 

    module Patterns =
        open Math.Pure.Quantity
        open Math.Pure.Structure
        open Math.Pure.Objects

        let rec (|RationalNumberExpression|_|) (u : Expression) =
            match u with
            | Number (Integer i) -> Some u //RNE-1
            | Number (Rational r) -> Some u //RNE-2
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) when //RNE-7
                (match a with | RationalNumberExpression a -> true | _ -> false) -> Some u 
            | BinaryOp (a,Plus,b) when //RNE-3
                (match a with | RationalNumberExpression a -> true | _ -> false) &&
                (match b with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,Minus,b) when //RNE-4
                (match a with | RationalNumberExpression a -> true | _ -> false) &&
                (match b with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,Times,b) when //RNE-6
                (match a with | RationalNumberExpression a -> true | _ -> false) &&
                (match b with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,DividedBy,b) when //RNE-6
                (match a with | RationalNumberExpression a -> true | _ -> false) &&
                (match b with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | UnaryOp (Negative,a) when //RNE-4
                (match a with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | UnaryOp (Positive,a) when //RNE-3
                (match a with | RationalNumberExpression a -> true | _ -> false) -> Some u
            | _-> None

        let rec (|GaussianRationalNumberExpression|_|) (u : Expression) =
            match u with
            | Number (Integer i) -> Some u //GRNE-1
            | Number (Rational r) -> Some u //GRNE-2
            | Symbol (Constant I) -> Some u //GRNE-3
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) when //GRNE-8
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u 
            | BinaryOp (a,Plus,b) when //GRNE-4
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) &&
                (match b with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,Minus,b) when //GRNE-5
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) &&
                (match b with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,Times,b) when //GRNE-6
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) &&
                (match b with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | BinaryOp (a,DividedBy,b) when //GRNE-7
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) &&
                (match b with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | UnaryOp (Negative,a) when //GRNE-5
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | UnaryOp (Positive,a) when //GRNE-4
                (match a with | GaussianRationalNumberExpression a -> true | _ -> false) -> Some u
            | _-> None
        
        let rec (|ExplicitAlgebraicNumber|_|) (u : Expression) =
            match u with
            | Number (Integer i) -> Some u //EAN-1
            | Number (Rational i) -> Some u //EAN-1
            | BinaryOp(ExplicitAlgebraicNumber a,ToThePowerOf,Number(Integer i)) -> Some u //EAN-2
            | BinaryOp(ExplicitAlgebraicNumber a,ToThePowerOf,Number(Rational r)) -> Some u //EAN-2
            | NaryOp(Product,aList) 
                when List.forall (fun x -> match x with | ExplicitAlgebraicNumber a -> true | _ -> false) aList 
                    -> Some u //EAN-3
            | NaryOp(Sum,aList) 
               when List.forall (fun x -> match x with | ExplicitAlgebraicNumber a -> true | _ -> false) aList 
                    -> Some u //EAN-3
            | _ -> None

    [<RequireQualifiedAccess>]
    module Check =
        open Patterns
        open Math.Pure.Quantity

        let isExplicitAlgebraicNumber u = 
            match u with
            | ExplicitAlgebraicNumber a -> true
            | _ -> false

        let isPrimeNaive n' =
            match n' with
            | Number (Integer n) ->
                match n with
                | _ when n > 3I && (n % 2I = 0I || n % 3I = 0I) -> false
                | _ ->
                    let maxDiv = System.Numerics.BigInteger(System.Math.Sqrt(float n)) + 1I
                    let rec f d i = 
                        match d > maxDiv with 
                        | true -> true
                        | false -> 
                            match n % d = 0I with
                            | true -> false
                            | false -> f (d + i) (6I - i)     
                    f 5I 2I
            | _ -> false

        let isPrime n' =
                    let cores = bigint (System.Environment.ProcessorCount * 16)
                    match n' with            
                    | Number (Integer n) ->
                        let maxDiv = (System.Numerics.BigInteger(System.Math.Sqrt(float n)) + 1I) / 3I
                        let rec test (i : System.Numerics.BigInteger) max = 
                                let oe = match i.IsEven with | true -> 1I | false-> 2I
                                let oe' = match i.IsEven with | true -> 2I | false-> 1I
                                let out1,out2,out3 = n % (oe + (3I * i)),n % (oe' + (3I * (i + 1I))),n % (oe + (3I * (i+2I)))
                                match out1,out2,out3 with
                                | _ when i > max -> None
                                | _ when out1 = 0I || out2 = 0I || out3 = 0I -> Some false
                                | _ -> test (i + 3I) max                        
                        let out = 
                            match n with
                            | _ when n = 5I || n = 7I || n = 11I -> None
                            | _ when n > 3I && (n % 2I = 0I || n % 3I = 0I || n % 5I = 0I) -> Some false                
                            | _ when n < 5000I -> test 1I maxDiv
                            //| _ -> Async.Choice [async {return test 1I (maxDiv/2I)}; async {return test (maxDiv/2I) (maxDiv)}] |> Async.RunSynchronously 
                            | _ -> Async.Choice [for i in 0I..(cores-1I) -> async {return test ((i*maxDiv/cores)+1I) ((i+1I)*maxDiv/cores)}] |> Async.RunSynchronously 
                        match out with
                        | Some false -> false
                        | _ -> true                
                    | _ -> false

    [<RequireQualifiedAccess>]
    module Integer =
        open Math.Pure.Quantity

        let quotient a b =
            match a, b with 
            | Number (Integer x), Number (Integer y) when y <> 0I && x = 0I -> Number (Integer 0I) // added 1/11/17
            | Number (Integer x), Number (Integer y) when y <> 0I ->
                let out = x/y
                let out' = 
                    match (x >= y && x >= 0I) with
                    | true -> out
                    | false -> 
                        match y < 0I || ((abs x) - (abs (out * y))) = 0I with 
                        | true -> out + 1I
                        | false -> out - 1I
                match 0I <= (x - out' * y) && (x - out' * y) <= (abs y) - 1I with
                | true -> Number (Integer out')
                | false -> 
                    match x < 0I with 
                    | true -> Number (Integer (out' - 1I))
                    | false -> Number (Integer (out' + 1I))
            | _ -> Number Undefined
 
        let remainder a b =
            match a, b with
            //| Number (Integer x), Number (Integer y) when x = 0I && y <> 0I -> Number (Integer 0I) //added 11-23-16, removed 11-28-16
            | Number (Integer x), Number (Integer y) when y <> 0I -> 
                match quotient a b with
                | Number (Integer q) -> Number (Integer (x - q*y))
                | _ -> Number Undefined
            | _ -> Number Undefined

        let posDivisors number' = 
            let number = match number' with | Number (Integer i) -> i | _ -> 1I
            let out = 
                [
                 for divisor in 1I .. (System.Numerics.BigInteger ((float >> sqrt >> int) number)) do
                 if number % divisor = 0I then
                     yield Number (Integer divisor)
                     if number <> 1I then yield Number (Integer (number / divisor)) //special case condition: when number=1 then divisor=(number/divisor), so don't repeat it
                ]
            out |> List.sortWith Expression.compareExpressions

        let divisors n = 
            let pos = (posDivisors n) 
            pos @ (List.map (fun x -> -x) pos)
    
        //Euclid’s Greatest Common Divisor Algorithm
        let rec gcd a b =
            match b with
            | Number (Integer b') when b' = 0I -> match a with | Number (Integer a') -> Number (Integer (abs a')) | _ -> Number Undefined 
            | Number (Integer b') -> gcd (Number (Integer (abs b'))) (remainder a b)
            | _ -> Number Undefined
 
        //The Extended Euclidean Algorithm
        let extEuclideanAlgorithm a b = 
            let mpp, mp, npp, np = 1I, 0I, 0I, 1I
            let rec eea a b mpp mp npp np =
                let A = match a with | Number (Integer i) -> i | _ -> 0I 
                let B = match b with | Number (Integer i) -> i | _ -> 0I
                match B = 0I with 
                | false -> 
                    let Q = match quotient a b with Number (Integer i) -> i | _ -> 1I
                    let R = remainder a b
                    let m = mpp - Q*mp
                    let n = npp - Q*np
                    eea (Number (Integer B)) R mp m np n
                | true -> 
                    match A >= 0I with
                    | true -> (Number (Integer A),Number (Integer mpp),Number (Integer npp))
                    | false -> (Number (Integer -A),Number (Integer -mpp),Number (Integer -npp))
            eea a b mpp mp npp np

        let rec checkPairwiseRelativelyPrime (nList : Expression list) =
            let h, t = nList.Head, nList.Tail
            let check = List.forall (fun x -> gcd h x = (Number (Integer 1I))) t
            match check with
            | true -> 
                match t.Length > 1 with
                | true -> checkPairwiseRelativelyPrime t
                | false -> true
            | false -> false

        //Chinese remainder theorem procedure that obtains the solution to the system of remainser equations
        let chineseRemainder (M : Expression list) (X : Expression list) =
            let third (_, _, c) = c
            let second (_,b,_) = b
            let checkPositive l = List.forall (fun x  -> match x with | Number (Integer i) when i > 0I -> true | _ -> false) l
            let checkX =
                match M.Length = X.Length with
                | false -> false
                | true ->  List.forall2 (fun m x -> ExpressionType.compareExpressions m x = 1) M X
            match checkPairwiseRelativelyPrime M && checkPositive M && checkPositive X && checkX with
            | false -> Number Undefined
            | true -> 
                let rec cRem n s i =
                    let x = X.[i]
                    let m = M.[i]
                    let e = extEuclideanAlgorithm n m
                    let c = second e
                    let d = third e
                    let s' = c*n*x + d*m*s
                    let n' = n*m
                    match i + 1 = M.Length with 
                    | true -> remainder s' n'
                    | false -> cRem n' s' (i+1)
                cRem M.Head X.Head 1

        let floor u = 
            match u with
            | Number n -> Number (Number.floor n)
            | _ -> u

        let ceiling u = 
            match u with
            | Number n -> Number (Number.ceiling n)
            | _ -> u

        let numberOfDigits n =
            match n with
            | Number (Integer i) -> System.Numerics.BigInteger (i.ToString().Length)
            | _ -> 0I

        // The integer n has a unique representation
        // in the base b given by n = b0 + b1 b +...+ bk b^k
        let baseRepresentation n b =
            match n, b with
            | Number (Integer n'), Number (Integer R') when n' >= 0I && R' >= 2I ->
                let r = match b with | Number (Integer i) -> i | _ -> 1I
                let d0 = remainder n b
                let q0 = quotient n b
                let rec recurse q d i lst =
                    let d' = remainder q b
                    let q' = quotient q b 
                    match q' = Number (Integer 0I) with
                    | true -> d'::lst
                    | false when i = 0I -> recurse q d (i+1I) (d::lst)
                    | false when i > 0I -> recurse q' d' (i+1I) (d'::lst)
                    | _ -> []
                recurse q0 d0 0I []
            | _ -> []

        let lcm a b = 
            match a, b with
            | Number (Integer a'), Number (Integer b') -> (Number (Integer (abs (a'*b'))))/(gcd a b)
            | _ -> Number (Integer 1I)

        let primes =
            let rec next x = seq{
                let test =
                   match x with
                   | Number (Integer x') when x' < 700I ->  Check.isPrimeNaive
                   | _ -> Check.isPrime
                match test x with
                | true when x = Number(Integer 2I) ->
                    yield Number(Integer 2I)
                    yield! next (Number(Integer 3I))
                | true -> yield x 
                          yield! next (x + Number(Integer 2I))
                | false -> yield! next (x + Number(Integer 2I))} 
            next (Number (Integer 2I)) |> Seq.cache

        let primesUpTo max = Seq.takeWhile (fun x -> ExpressionType.compareExpressions x max < 1) primes

        let nextPrime p = 
            let rec next x = 
                match Check.isPrime x with
                | true -> Some x                
                | false -> next (x + Number(Integer 2I))
            match p with 
            | Number (Integer x) when x <= 2I -> None
            | Number (Integer x) when x.IsEven = true -> next (p + Number(Integer 1I))
            | Number (Integer x) when x.IsEven = false -> next (p + Number(Integer 2I))
            | _ -> None
 
        let previousPrime p = 
            let rec next x = 
                match Check.isPrime x with
                | true -> Some x                
                | false -> next (x - Number(Integer 2I))
            match p with 
            | Number (Integer x) when x <= 2I -> None
            | Number (Integer x) when x.IsEven = true -> next (p - Number(Integer 1I))
            | Number (Integer x) when x.IsEven = false -> next (p - Number(Integer 2I))
            | _ -> None


        //let listPrimesUpTo max = primesUpTo max |> Seq.toList

        let factorCandidates n' =
            let n = match ExpressionFunction.abs n' with | Number(Integer i) -> i | _ -> 0I
            let expand l =                
                let rec comb accLst elemLst =
                    match elemLst with
                    | h::t ->
                        let next = [h]::List.map (fun el -> h::el) accLst @ accLst
                        comb next t
                    | _ -> accLst
                comb [] l 
                |> Seq.distinct
                |> Seq.toList   
            let rawCandidatesN = 
                let cand = Seq.choose (fun x -> match remainder n' x = Number Number.Zero with | true -> Some x | _ -> None) (primesUpTo (Number(Integer(System.Numerics.BigInteger(System.Math.Sqrt(float n)) + 1I)))) |> Seq.toList
                expand cand   
            (List.map (fun x -> List.fold (fun x' acc -> x' * acc ) (Number(Integer 1I)) x) rawCandidatesN)                                       
            |> Seq.distinct  
            |> Seq.toList

        let factorByTrialDivision num =
            match Check.isPrime num with
            | false ->
                let n' = match num with | Number(Integer i) -> i | _ -> 0I
                let rec factorByTrialDivision' n primes' =        
                    match n with
                    | Number(Integer i) when i = 1I -> [Number(Integer 1I)]
                    | _ when Seq.isEmpty primes' -> [n]                
                    | _-> 
                        let p = Seq.head primes'         
                        match ExpressionType.compareExpressions (p*p) n = 1 with
                        | true -> [n]
                        | false ->
                            let n'' = match n with | Number(Integer i) -> i | _ -> 0I
                            let p' = match p with | Number(Integer i) -> i | _ -> 0I
                            match n'' % p' = 0I with
                            | true -> p :: factorByTrialDivision' (Number(Integer(n''/p'))) primes'
                            | false -> factorByTrialDivision' n (Seq.skip 1 primes')
                let candidates = 
                    (List.choose (fun x -> match (ExpressionType.compareExpressions x (Number(Integer 0I))), (Check.isPrime (ExpressionFunction.abs x)) with | 1, true -> Some x | _ -> None) (factorCandidates num))
                    |> List.sortWith (ExpressionType.compareExpressions)  
                    |> List.toSeq
                factorByTrialDivision' num candidates
            | true -> [num]

    //do Async.StartAsTask (Integer.primeWF) |> ignore
    [<RequireQualifiedAccess>]
    module Rational = 
        open Math.Pure.Quantity
        open Math.Pure.Objects
        open Patterns

        let numeratorRNE u =
            match u with 
            | Number (Integer i) -> u
            | Number (Rational r) -> Number(Integer r.numerator)
            | BinaryOp(Number (Integer a),DividedBy,Number (Integer b)) -> Number (Integer a)
            | _ -> Number Undefined

        let denominatorRNE u =
            match u with
            | Number (Integer i) -> Number(Integer 1I)
            | Number (Rational r) -> Number(Integer r.denominator)
            | BinaryOp(Number (Integer a),DividedBy,Number (Integer b)) -> Number (Integer b)
            | _ -> Number Undefined

        let private evalQuotient v w =
            match numeratorRNE w = Number (Integer 0I) with
            | false -> BinaryOp((numeratorRNE v)*(denominatorRNE w),DividedBy,(numeratorRNE w)*(denominatorRNE v))
            | true -> Number Undefined

        let private evalProduct v u = 
            match v, u with
            | Number (Integer a), Number (Integer b) when a = 0I || b = 0I -> Number (Integer 0I)
            | Number (Integer a), b when a = 1I -> b
            | a, Number (Integer b) when b = 1I -> a
            | _ -> BinaryOp((numeratorRNE v)*(numeratorRNE u),DividedBy,(denominatorRNE u)*(denominatorRNE v))

        let private evalSum v u = 
            match v, u with
            | Number (Integer a), b when a = 0I -> b
            | a, Number (Integer b) when b = 0I -> a
            | _ -> BinaryOp(((numeratorRNE v)*(denominatorRNE u))+((numeratorRNE u)*(denominatorRNE v)),DividedBy,(denominatorRNE u)*(denominatorRNE v))
    
        let private evalDifference v u = 
            match v, u with
            | Number (Integer a), b when a = 0I -> b
            | a, Number (Integer b) when b = 0I -> a
            | _ -> BinaryOp((numeratorRNE v)*(denominatorRNE u)-(numeratorRNE u)*(denominatorRNE v),DividedBy,(denominatorRNE u)*(denominatorRNE v))
    
        let rec private evalPower v n =
            match numeratorRNE v <> Number (Integer 0I), n with
            | true, Number (Integer i) -> 
                match i with
                | a when a > 0I -> 
                    let s = evalPower v (n - Number (Integer 1I))
                    evalProduct s v
                | b when b = 0I -> Number (Integer 1I)
                | c when c = -1I -> 
                    match v with 
                    | BinaryOp (a,DividedBy,b) -> BinaryOp (evalQuotient a b,DividedBy,numeratorRNE v)
                    | _ -> Number Undefined
                | _ -> let s = 
                           match v with 
                           | BinaryOp (a,DividedBy,b) -> BinaryOp (evalQuotient a b,DividedBy,numeratorRNE v)
                           | _ -> Number Undefined
                       evalPower s (-n)
            | false, Number (Integer i) -> 
                match i <= 0I with
                | true -> Number (Integer 0I)
                | false -> Number Undefined
            | _ -> Number Undefined
    
        let simplify u =
            match u with 
            | Number (Integer i) -> u
            | BinaryOp(a,DividedBy,b) when b = Number (Integer 0I) -> Number Undefined
            | BinaryOp(a,DividedBy,b) when a = Number (Integer 0I) && b <> Number (Integer 0I) -> Number (Integer 0I)
            | BinaryOp(a,DividedBy,b) when b <> Number (Integer 0I) -> 
                match Integer.remainder a b = Number (Integer 0I) with
                | true -> Integer.quotient a b
                | false -> 
                    match b with
                    | Number (Integer i) when i > 0I -> BinaryOp(Integer.quotient a (Integer.gcd a b),DividedBy,Integer.quotient b (Integer.gcd a b))
                    | Number (Integer i) when i < 0I -> BinaryOp(Integer.quotient (-a) (Integer.gcd a b),DividedBy,Integer.quotient (-b) (Integer.gcd a b))
                    | _ -> u
            | _ -> u

        let rec private _simplifyRNE u = 
            match u with
            | Number (Integer i) -> u
            | Number (Rational r) when r.denominator <> 0I -> BinaryOp (Number (Integer r.numerator),DividedBy,Number (Integer r.denominator))
            | BinaryOp (a,DividedBy,b) when 
                _simplifyRNE a <> Number Undefined &&
                _simplifyRNE b <> Number Undefined -> evalQuotient (_simplifyRNE a) (_simplifyRNE b) 
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) when 
                _simplifyRNE a <> Number Undefined -> evalPower (_simplifyRNE a) (Number (Integer b))
            | BinaryOp (a,Plus,b) when 
                _simplifyRNE a <> Number Undefined &&
                _simplifyRNE b <> Number Undefined -> evalSum (_simplifyRNE a) (_simplifyRNE b)
            | BinaryOp (a,Minus,b) when 
                _simplifyRNE a <> Number Undefined &&
                _simplifyRNE b <> Number Undefined -> evalDifference (_simplifyRNE a) (_simplifyRNE b)
            | BinaryOp (a,Times,b) when 
                _simplifyRNE a <> Number Undefined &&
                _simplifyRNE b <> Number Undefined -> evalProduct (_simplifyRNE a) (_simplifyRNE b)
            | UnaryOp (Negative,a) when _simplifyRNE a <> Number Undefined -> -(_simplifyRNE a)
            | UnaryOp (Positive,a) when _simplifyRNE a <> Number Undefined -> _simplifyRNE a 
            | _ -> Number Undefined
            
        let simplifyRNE u = 
            match u with
            | RationalNumberExpression rne -> simplify (_simplifyRNE rne)
            | _ -> Number Undefined

        let partialFractionExpansion number =
            match number with
            | Number(Rational ({numerator = x; denominator = y})) -> //when x < y && x/y >= 0I -> 
                let qi = Integer.quotient (numeratorRNE number) (denominatorRNE number)
                let formatPower (p : Expression list) = 
                    match p.Length with
                    | 1 -> p.Head
                    | _ -> BinaryOp(p.Head,ToThePowerOf,Number(Integer(System.Numerics.BigInteger p.Length)))
                let yFactors = Integer.factorByTrialDivision (Number(Integer y)) 
                               |> List.sortWith ExpressionType.compareExpressions
                let rec loop acc bcc numerator xList yList = 
                    let x' = List.fold (fun acc x -> acc * x) (Number(Integer 1I)) xList
                    let y' = List.fold (fun acc y -> acc * y) (Number(Integer 1I)) yList
                    let extEuc = Integer.extEuclideanAlgorithm x' y'
                    let A = match extEuc with | (g,a,b) -> a
                    let B = match extEuc with | (g,a,b) -> b
                    let numX = (Integer.remainder (numerator*B) x') 
                    let numY = (Integer.remainder (numerator*A) y')            
                    match Seq.length (Seq.distinct (List.toSeq yList)) > 1 with
                    | true -> 
                        let newxList, newyList = 
                            let h = List.head yList
                            List.partition (fun x -> x = h) yList
                        loop (acc + (BinaryOp(numX,DividedBy,formatPower xList))) (bcc+numX/(formatPower xList)) numY newxList newyList
                    | false -> 
                        let bcc' = bcc + numX/(formatPower xList) + numY/(formatPower yList)
                        let q = Integer.quotient (numeratorRNE bcc') (denominatorRNE bcc')
                        acc 
                        + (BinaryOp(numX,DividedBy,formatPower xList)) 
                        + (BinaryOp(numY,DividedBy,formatPower yList))
                        - q + qi
                let xList, yList = 
                    let h = List.head yFactors
                    List.partition (fun x -> x = h) yFactors
                match yList with
                | [] -> BinaryOp(Number(Integer x),DividedBy,formatPower xList) //|> ExpressionType.simplifyExpression
                | _ -> loop (Number(Integer 0I)) (Number(Integer 0I)) (Number(Integer x)) xList yList 
            | _ -> number

    [<RequireQualifiedAccess>]
    module Field = 
        open Math.Pure.Quantity
        open Math.Pure.Objects
        open Patterns

        //Let p be prime, and let s, t <> 0 be in Zp,  
        //obtains the multiplicative inverse t^−1.
        let multiplicativeInverseP t p = 
            match t, p with
            | Number(Integer it), Number(Integer ip) when it > 0I && it <= ip - 1I && Check.isPrime p = true -> 
                let eeu = (Integer.extEuclideanAlgorithm t p)
                let m = match eeu with | (g,m,n) -> m
                Integer.remainder m p
            | _ -> Number Undefined

        //obtains s/t in Zp.
        let divisionP s t p = 
            match s, t, p with
            | Number(Integer is), Number(Integer it), Number(Integer ip) when
                abs is >= 0I && abs is <= ip - 1I && it > 0I && it <= ip - 1I && Check.isPrime p = true -> //added 11/23/16
                    let t' = multiplicativeInverseP t p
                    Integer.remainder (s * t') p
            | _ -> Number Undefined

        let rec nestDepth u =
            match u with 
            | ExplicitAlgebraicNumber a -> 
                match a with
                | Number (Integer i) -> Number (Integer 0I)
                | Number (Rational r) -> Number (Integer 0I)
                | BinaryOp (a,ToThePowerOf,Number (Rational r)) -> (nestDepth a) + Number (Integer 1I)
                | BinaryOp (a,op,b) when op = Plus || op = Times || op = DividedBy || op = Minus ->
                    match (nestDepth a), (nestDepth b) with
                    | Number (Integer i), Number (Integer j) when i >= j-> Number (Integer i)
                    | Number (Integer i), Number (Integer j) when i < j-> Number (Integer j)
                    | _ -> Number Undefined
                | NaryOp(op,aList) when op = Product || op = Sum -> 
                    List.maxBy (fun x -> 
                    match (nestDepth x) with
                    | Number (Integer i) -> i
                    | _ -> 0I
                    ) aList
                | _ -> Number Undefined
            | _ -> Number Undefined

    [<RequireQualifiedAccess>]
    module Gaussian = 
        open Math.Pure.Quantity
        open Math.Pure.Objects
        open Math.Pure.Structure.ExpressionStructure
        open Patterns

        let private containsI (x:Expression) = 
            let eNumber acc (n:Expression) = acc
            let eComplexNumber acc x = acc
            let eSymbol acc (v:Expression) = 
                match v with
                | Symbol (Constant c) when c = I -> acc + 1
                | _ -> acc
            let eBinaryOp acc x = acc
            let eUnaryOp acc x = acc
            let eNaryOp acc x = acc
            let acc = 0
            let i = Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
            match i > 0 with
            | true -> true
            | false -> false

        let rec private normalize u =
            match u with
            | BinaryOp(a,Plus,b) when containsI a = true && containsI b = false 
                -> BinaryOp(b,Plus,normalize a)
            | BinaryOp(a,Times,b) when containsI a = true && containsI b = false 
                -> BinaryOp(b,Times,normalize a)
            | BinaryOp(a,Minus,b) when containsI a = true && containsI b = false 
                -> BinaryOp(BinaryOp(Number(Integer -1I),Times,b),Plus,normalize a)
            | BinaryOp(a,Plus,b) when containsI a = false && containsI b = true
                -> BinaryOp(a,Plus,normalize b)
            | BinaryOp(a,Times,b) when containsI a = false && containsI b = true
                -> BinaryOp(a,Times,normalize b)
            | BinaryOp(a,Minus,b) when containsI a = false && containsI b = true 
                -> BinaryOp(a,Minus,normalize b)
            | BinaryOp(a,DividedBy,b) -> BinaryOp(normalize a,DividedBy,normalize b)
            | BinaryOp(a,ToThePowerOf,Number (Integer b)) -> BinaryOp(normalize a,ToThePowerOf, Number (Integer b))
            | _ -> u

        let conjugate u =
            match u with
            | BinaryOp(x, Plus, y) -> normalize (BinaryOp(x, Minus, y))
            | BinaryOp(x, Minus, y) -> normalize (BinaryOp(x, Plus, y))
            | _ -> u
        
        let rec simplify u' = 
            let zero, one, negOne, two, i = Number(Integer 0I), Number(Integer 1I),Number(Integer -1I), Number(Integer 2I), Symbol (Constant I)
            let add x y = BinaryOp(simplify x, Plus, simplify y)
            let subtract x y = BinaryOp(simplify x, Minus, simplify y)
            let multiply x y = BinaryOp(simplify x, Times, simplify y)
            let divide x y = BinaryOp(simplify x, DividedBy, simplify y)
            let power x y = BinaryOp(simplify x, ToThePowerOf, simplify y)
            let u = normalize u'
            match u with 
            | GaussianRationalNumberExpression u -> 
                match u with 
                // ***Standard Form***
                | RationalNumberExpression r -> Rational.simplifyRNE u
                // ***Standard Form***
                | Symbol(Constant I) -> u

                //xxxx Multiply xxxx
                //i*i
                | BinaryOp (j,Times,k) when j = i && k = i-> negOne
                //i*ai
                | BinaryOp (j,Times,BinaryOp(RationalNumberExpression a,Times,k)) when j = i && k = i -> 
                    multiply a negOne |> simplify
                //i*(b+i)
                | BinaryOp (j,Times,BinaryOp (RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add negOne (multiply b i) |> simplify
                //i*(b+ci)
                | BinaryOp (j,Times,BinaryOp (RationalNumberExpression b,Plus,BinaryOp(RationalNumberExpression c,Times,k))) when j = i && k = i -> 
                    add (multiply c negOne) (multiply b i) |> simplify
                //ai*i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Times,k) when j = i && k = i -> 
                    multiply a negOne |> simplify
                //ai*bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Times,BinaryOp(RationalNumberExpression b,Times,k)) when j = i && k = i -> 
                    multiply (multiply a b) negOne |> simplify
                //ai*(b+i)
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Times,BinaryOp (RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add (multiply negOne a) (multiply (multiply a b) i) |> simplify
                //ai*(b+ci)
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Times,BinaryOp (RationalNumberExpression b,Plus,BinaryOp(RationalNumberExpression c,Times,k))) when j = i && k = i -> 
                    add (multiply negOne (multiply c a)) (multiply (multiply a b) i) |> simplify
                //a*i  ***Standard Form***
                | BinaryOp (RationalNumberExpression a,Times,j) when j = i && a <> zero && a <> one -> 
                    multiply a i
                //a*i, a = 0
                | BinaryOp (RationalNumberExpression a,Times,j) when j = i && a = zero -> 
                    zero
                //a*i, a = 1
                | BinaryOp (RationalNumberExpression a,Times,j) when j = i && a = one -> 
                    i
                //a*bi
                | BinaryOp (RationalNumberExpression a,Times,BinaryOp (RationalNumberExpression b,Times,j)) when j = i -> 
                    multiply (multiply a b) i |> simplify
                //a*(b+i)
                | BinaryOp (RationalNumberExpression a,Times,BinaryOp (RationalNumberExpression b,Plus,j)) when j = i -> 
                    add (multiply a b) (multiply a i) |> simplify
                //a*(b+ci)
                | BinaryOp (RationalNumberExpression a,Times,BinaryOp (RationalNumberExpression b,Plus,BinaryOp (RationalNumberExpression c,Times,j))) when j = i -> 
                    add (multiply a b) (multiply (multiply a c) i) |> simplify
                //(a+i)*i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Times,k) when j = i && k = i -> 
                    add negOne (multiply a i) |> simplify
                //(a+i)*bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Times,(BinaryOp(b,Times,k))) when j = i && k = i -> 
                    add (multiply negOne b) (multiply (multiply a b) i) |> simplify
                //(a+i)*(b+i)               
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Times,BinaryOp(RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add (add (multiply a b) negOne) (multiply (add a b) i) |> simplify
                //(a+i)*(b+di)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Times,BinaryOp(RationalNumberExpression b,Plus,(BinaryOp(RationalNumberExpression d,Times,k)))) when j = i && k = i -> 
                    add (subtract (multiply a b) d) (multiply (add (multiply a d) b) i) |> simplify
                //(a+ci)*i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,Symbol (Constant i)))),Times,Symbol (Constant j)) when i = I && j = I -> 
                    add (negOne*c) (multiply a (Symbol (Constant I))) |> simplify
                //(a+ci)*bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Times,BinaryOp(RationalNumberExpression b,Times,k)) when j = i && k = i -> 
                    add (multiply negOne (multiply c b)) (multiply b (multiply a i)) |> simplify
                //(a+ci)*(b+i)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Times,BinaryOp(RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add (subtract (multiply a b) c) (multiply (multiply (add a b) c) i) |> simplify
                //(a+ci)*(b+di)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Times,BinaryOp(RationalNumberExpression b,Plus,(BinaryOp(RationalNumberExpression d,Times,k)))) when j = i && k = i -> 
                    add (subtract (multiply a b) (multiply c d)) (multiply (add (multiply a d) (multiply b c)) i) |> simplify
                 
                //++++ Add ++++
                //i+i
                | BinaryOp (j,Plus,k) when j = i && k = i -> 
                    multiply  two i |> simplify
                //i+ai
                | BinaryOp (j,Plus,BinaryOp(RationalNumberExpression a,Times,k)) when j = i && k = i -> 
                    multiply (add a one) i |> simplify
                //i+(b+i)
                | BinaryOp (j,Plus,BinaryOp (RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add b (multiply two (Symbol (Constant I))) |> simplify
                //i+(b+ci)
                | BinaryOp (j,Plus,BinaryOp (RationalNumberExpression b,Plus,BinaryOp(RationalNumberExpression c,Times,k))) when j = i && k = i -> 
                    add b (multiply (add c one) i) |> simplify
                //ai+i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Plus,k) when j = i && k = i -> 
                    multiply (add a one) i |> simplify
                //ai+bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Plus,BinaryOp(RationalNumberExpression b,Times,k)) when j = i && k = i -> 
                    multiply (add a b) i |> simplify
                //ai+(b+i)
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Plus,BinaryOp (RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add b (multiply (add a one) i) |> simplify
                //ai+(b+ci)
                | BinaryOp (BinaryOp(RationalNumberExpression a,Times,j),Plus,BinaryOp (RationalNumberExpression b,Plus,BinaryOp(RationalNumberExpression c,Times,k))) when j = i && k = i -> 
                    add b (multiply (add a c) i) |> simplify
                //a+(b+i)
                | BinaryOp (RationalNumberExpression a,Plus,BinaryOp (RationalNumberExpression b,Plus,j)) when j = i -> 
                    add (add a b) i |> simplify
                //a+(b+ci)
                | BinaryOp (RationalNumberExpression a,Plus,BinaryOp (RationalNumberExpression b,Plus,BinaryOp(RationalNumberExpression c,Times,j))) when j = i -> 
                    add (add a b) (multiply c i) |> simplify
                //a+bi ***Standard Form***
                | BinaryOp (RationalNumberExpression a,Plus,BinaryOp (RationalNumberExpression b,Times,j)) when j = i && b <> zero && b <> one && a <> zero -> 
                    add a (multiply b i)                
                //a+bi, a = 0
                | BinaryOp (RationalNumberExpression a,Plus,BinaryOp (RationalNumberExpression b,Times,j)) when j = i && b <> zero && b <> one && a <> zero -> 
                    multiply b i                
                //a+i ***Standard Form***
                | BinaryOp(RationalNumberExpression a,Plus,j) when a <> zero && j = i -> 
                    add a i                
                //a+i, a = 0
                | BinaryOp(RationalNumberExpression a,Plus,j) when a <> zero && j = i -> 
                    i 
                //(a+i)+i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Plus,k) when j = i && k = i -> 
                    add a (multiply two i) |> simplify
                //(a+i)+bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Plus,(BinaryOp(b,Times,k))) when j = i && k = i -> 
                    add a (multiply (add b one) i) |> simplify
                //(a+i)+(b+i)               
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Plus,BinaryOp(RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add (add a b) (multiply two i) |> simplify
                //(a+i)+(b+di)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,j),Plus,BinaryOp(RationalNumberExpression b,Plus,(BinaryOp(RationalNumberExpression d,Times,k)))) when j = i && k = i -> 
                    add (add a b) (multiply (add d one) i) |> simplify
                //(a+ci)+i
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Plus,k) when j = i && k = i -> 
                    add a (multiply (add c one) i) |> simplify
                //(a+ci)+bi
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Plus,BinaryOp(RationalNumberExpression b,Times,k)) when j = i && k = i -> 
                    add a (multiply (add c b) i) |> simplify
                //(a+ci)+(b+i)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Plus,BinaryOp(RationalNumberExpression b,Plus,k)) when j = i && k = i -> 
                    add (add a b) (multiply (add c one) i) |> simplify
                //(a+ci)+(b+di)                
                | BinaryOp (BinaryOp(RationalNumberExpression a,Plus,(BinaryOp(RationalNumberExpression c,Times,j))),Plus,BinaryOp(RationalNumberExpression b,Plus,(BinaryOp(RationalNumberExpression d,Times,k)))) when j = i && k = i -> 
                    add (add a b) (multiply (add c d) i) |> simplify

                //General cases                
                | BinaryOp(a,DividedBy,b) -> 
                    let out = (divide (multiply a (simplify (conjugate b))) (multiply b (simplify (conjugate b)))) 
                    match out with
                    | BinaryOp(a,DividedBy,RationalNumberExpression b) -> multiply (divide one b) a |> simplify
                    | _ -> out
                | BinaryOp(a,Times,b) -> (multiply a b) 
                | BinaryOp(a,Minus,b) -> (add a (multiply negOne b)) 
                | BinaryOp(a,Plus,b) -> (add a b) 
                
                //a**n
                | BinaryOp(a,ToThePowerOf,Number(Integer b)) when b >= 2I-> 
                    match a with
                    | Symbol(Constant I) -> 
                        match b%4I with
                        | bmod4 when bmod4 = 0I -> one
                        | bmod4 when bmod4 = 1I -> i
                        | bmod4 when bmod4 = 2I -> negOne
                        | bmod4 when bmod4 = 3I -> multiply negOne i
                        | _ -> Number Undefined
                    | BinaryOp(x,Plus,y) when b = 2I -> (add (multiply x x) (add (multiply two (multiply x y)) (multiply y y))) |> simplify
                    | BinaryOp(x,Plus,y) when b > 2I -> (multiply a (power a (Number(Integer (b-1I))))) |> simplify
                    | BinaryOp(x,Times,y) -> (multiply (power x (Number(Integer b))) (power y (Number(Integer b)))) |> simplify
                    | _ -> u
                | _ -> Number Undefined
            | _ -> Number Undefined

    [<RequireQualifiedAccess>]
    module Test = 
        open Math.Pure.Quantity
        open Math.Pure.Objects
        open System

        let isPrime5 n' =
                    let cores = bigint (Environment.ProcessorCount * 16)
                    match n' with            
                    | Number (Integer n) ->
                        let maxDiv = (System.Numerics.BigInteger(System.Math.Sqrt(float n)) + 1I) / 3I
                        let rec test (i : System.Numerics.BigInteger) max = 
                                let oe = match i.IsEven with | true -> 1I | false-> 2I
                                let oe' = match i.IsEven with | true -> 2I | false-> 1I
                                let out1,out2,out3 = n % (oe + (3I * i)),n % (oe' + (3I * (i + 1I))),n % (oe + (3I * (i+2I)))
                                match out1,out2,out3 with
                                | _ when i > max -> None
                                | _ when out1 = 0I || out2 = 0I || out3 = 0I -> Some false
                                | _ -> test (i + 3I) max                        
                        let out = 
                            match n with
                            | _ when n = 5I || n = 7I || n = 11I -> None
                            | _ when n > 3I && (n % 2I = 0I || n % 3I = 0I || n % 5I = 0I) -> Some false                
                            | _ when n < 5000I -> test 1I maxDiv
                            //| _ -> Async.Choice [async {return test 1I (maxDiv/2I)}; async {return test (maxDiv/2I) (maxDiv)}] |> Async.RunSynchronously 
                            | _ -> Async.Choice [for i in 0I..(cores-1I) -> async {return test ((i*maxDiv/cores)+1I) ((i+1I)*maxDiv/cores)}] |> Async.RunSynchronously 
                        match out with
                        | Some false -> false
                        | _ -> true                
                    | _ -> false
             
