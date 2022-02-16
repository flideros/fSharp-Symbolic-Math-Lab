namespace Math.Pure.Structure

open Math.Pure.Objects
open Math.Pure.Quantity

module Polynomial =
    open ExpressionStructure
    open ExpressionType

//Active patterns
    module Patterns =

        let (|Monomial|_|) (u : Expression) =
            match u with
            | Number (Integer i) -> Some u //MON-1
            | Number (Rational r) -> Some u //MON-1
            | Symbol v -> Some u //MON-2
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) when //MON-3
                b > 1I -> Some u 
            | NaryOp (Product, [a;b]) when //MON-4            
                    match a, b with                 
                    | Number n, Symbol s -> true                
                    | Number n, BinaryOp (Symbol s,ToThePowerOf,(Number (Integer b))) when b > 1I -> true
                    | _ -> false
                    -> Some u
            | _-> None

        let (|Polynomial|_|) (u : Expression) =
            match u with
            | Monomial n -> Some u //POLY-1
            | NaryOp (Sum, mList) when //POLY-2
                List.forall (fun x -> match x with | Monomial m -> true | _ -> false) mList &&
                Seq.length (Seq.distinct((List.collect (fun x -> variables x) mList))) = 1 -> Some u 
            | _-> None
    
        let rec (|GeneralMonomial|_|) (xList : Expression list) = function        
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) as B when //GME-3
                b > 1I && List.exists (fun x -> a = x) xList -> Some B
            | NaryOp (Product, a) -> //GME-4
                let test = List.forall (fun x -> (match x with | GeneralMonomial xList x -> true
                                                               | _ -> false) = true) a
                match test with
                | true -> Some (NaryOp (Product, a))
                | false -> None
            | a when List.forall (fun x -> freeOf (a) x ) xList || //GME-1
                     List.exists (fun x -> a = x) xList -> Some (a) //GME-2        
            | _-> None

        let (|GeneralPolynomial|_|) (xList : Expression list) = function
            | NaryOp (Sum, a) -> //GPE-2
                match List.forall (fun x -> (match x with | GeneralMonomial xList x -> true                                                     
                                                          | _ -> false) = true) a with
                | true -> Some (NaryOp (Sum, a))
                | false -> None
            | x when (match x with | GeneralMonomial xList x -> true //GPE-1
                                   | _ -> false) = true -> Some x
            | _-> None

        let rec (|AlternateGeneralMonomial|_|) (x : Expression) = function        
            | BinaryOp (a,ToThePowerOf,(Number (Integer b))) when b > 1I -> //GMEALT-2
                 Some (BinaryOp (a,ToThePowerOf,(Number (Integer b))))
            | NaryOp (Product, a) -> //GMEALT-3
                let test = List.forall (fun a ->
                    (match x with 
                     | AlternateGeneralMonomial x a -> true
                     | NaryOp(Sum,s) -> true
                     | _ -> false) = true) a
                match test with
                | true -> Some (NaryOp (Product, a))
                | false -> None        
            | a when a = x -> Some a //GMEALT-1        
            | NaryOp(Sum,s) -> None //GMEALT-4
            | _ -> Some x //GMEALT-4
 
        let (|AlternateGeneralPolynomial|_|) (x : Expression) = function
            | NaryOp (Sum, a) -> //GPEALT-2
                match List.forall (fun a' -> (match a' with | AlternateGeneralMonomial x a' -> true                                                     
                                                            | _ -> false) = true) a with
                | true -> Some (NaryOp (Sum, a))
                | false -> None
            | a when (match a with | AlternateGeneralMonomial x a -> true //GPEALT-1
                                   | _ -> false) = true -> Some x
            | _-> None

        let (|GeneralRationalExpression|_|) (S : Expression list) (x : Expression) = 
            match numerator x, denominator x with
            | GeneralPolynomial S n, GeneralPolynomial S d -> Some x
            | _-> None

        let rec (|GExpression|_|) (alpha : Expression) = function
            | Number (Integer i) as u -> Some u //G-1
            | Number (Rational r) as u -> Some u //G-2
            | Symbol s as S when S = alpha -> Some S //G-3
            | NaryOp (op, a) as naryOp when op = Sum || op = Product -> //G-4 and G-5
                let test = List.forall (fun a' ->
                    (match a' with 
                     | GExpression alpha a' -> true                     
                     | _ -> false) = true) a
                match test with
                | true -> Some naryOp
                | false -> None                  
            | BinaryOp (GExpression alpha a,ToThePowerOf,(Number (Integer b))) as B -> Some B //G-6
            | _-> None

        let rec (|RSIN|_|) = function
            | Number (Integer i) as u -> Some u //RS-IN-1
            | Number (Rational r) as u -> Some u //RS-IN-2
            | Symbol s as S -> Some S //RS-IN-3
            | NaryOp (op, a) as naryOp when op = Sum || op = Product -> //RS-IN-4 and 5
                let test = List.forall (fun a' ->
                    (match a' with 
                     | RSIN a' -> true                     
                     | _ -> false) = true) a
                match test with
                | true -> Some naryOp
                | false -> None                  
            | BinaryOp (RSIN a,ToThePowerOf,(Number (Integer b))) as B -> Some B //RS-IN-6
            | _-> None

//Check functions
    [<RequireQualifiedAccess>]
    module Check =
        open Patterns

        let isMonomialSV x =
            match x with
            | Monomial m -> true
            | _ -> false 

        let isPolynomialSV x =
            match x with
            | Polynomial p -> true
            | _ -> false  
    
        let isMonomialGPE x xList =
            match x with
            | GeneralMonomial xList m -> true
            | _ -> false 

        let isPolynomialGPE x xList =
            match x with
            | GeneralPolynomial xList p -> true
            | _ -> false

        let isAltMonomialGPE u x =
            match u with
            | AlternateGeneralMonomial x m -> true
            | _ -> false 

        let isAltPolynomialGPE u x =
            match u with
            | AlternateGeneralPolynomial x p -> true
            | _ -> false

        let isGRE u sList=
            match u with
            | GeneralRationalExpression sList x -> true
            | _ -> false

        let isGExpression x u =
            match u with
            | GExpression x u -> true
            | _ -> false
        
        let isRSIN u =
            match u with
            | RSIN a -> true
            | _ -> false                    
        
        let expressionListContainsSum expList = 
            List.exists (fun x -> 
            match x with 
            | NaryOp(Sum, xList) -> true
            | _ -> false ) expList

        let expressionListContainsNegativePower expList = 
            List.exists (fun x -> 
            match x with 
            | BinaryOp(a,ToThePowerOf,Number(Integer b)) when b < 0I -> true
            | _ -> false ) expList

        let expressionContainsVariable v (x:Expression)  = 
            let accOut acc = match acc with | true -> true | false -> false
            let eNumber acc (n:Expression) = accOut acc
            let eComplexNumber acc x = accOut acc
            let eSymbol acc (v':Expression) = match v = v' with | true -> true | false -> accOut acc
            let eBinaryOp acc x = accOut acc
            let eUnaryOp acc x = accOut acc
            let eNaryOp acc x = accOut acc
            let acc = false
            Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x

        let sumContainsConstantTerm x = 
            match x with
            | NaryOp(Sum,xList) -> 
                List.exists (fun x -> 
                match x with 
                | Number n -> true
                | _ -> false ) xList
            | _ -> false

//Variables of a polynomial  
    [<RequireQualifiedAccess>]
    module Variables =
        open Math.Foundations.Logic
              
        let rec private _variablesOf acc u = 
            match u with
            | Number n -> acc //Var-1
            | BinaryOp(a,ToThePowerOf,Number(Integer b)) when b > 1I -> a::acc //Var-2
            | NaryOp(Product,aList) -> List.collect (fun x -> //Var-4
                match x with 
                | NaryOp(Sum,bList) -> NaryOp(Sum,bList)::acc
                | _ -> (_variablesOf acc x)) aList
            | NaryOp(Sum,aList) -> List.collect (fun x -> _variablesOf acc x) aList //Var-3
            | _ -> u::acc //Var-5
            |> Seq.distinct
            |> Seq.toList
            |> List.sortWith (fun x -> ExpressionType.compareExpressions x)

        let ofExpression u = _variablesOf [] u
        
        let ofRationalExpression u = Set.Union.oFList (ofExpression (numerator u)) (ofExpression (denominator u))
                                     |> List.sortWith (fun x -> ExpressionType.compareExpressions x) 

//Degree of a polynomial
    [<RequireQualifiedAccess>]
    module Degree =


        let ofPolynomialSV x = 
            match Check.isPolynomialSV x with
            | false -> Undefined
            | true -> 
                let powers = 
                    let eNumber acc (n:Expression) = match n = Number Number.Zero with | false -> (Integer 0I)::acc | true -> NegativeInfinity ::acc
                    let eComplexNumber acc x = acc//Need to look further into this
                    let eSymbol acc (v:Expression) = (Integer 1I)::acc
                    let eBinaryOp acc x = match x with | BinaryOp (Symbol(Variable a),ToThePowerOf,Number(Integer b)) -> (Integer b) :: acc | _ -> acc
                    let eUnaryOp acc x = acc
                    let eNaryOp acc x = acc
                    let acc = []
                    Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
                (List.rev(List.sortWith Number.compare powers)).Head

        let rec private _GPE x xList = 
            match Check.isPolynomialGPE x xList with
            | false -> Undefined
            | true ->
                match x with
                | Number n when n = Number.Zero -> NegativeInfinity
                | NaryOp (Sum, aList) -> (List.rev (List.sortWith Number.compare (List.map (fun x -> _GPE x xList) aList))).Head
                | _ -> 
                    let powers = 
                        let eNumber acc (n:Expression) = match n = Number Number.Zero with | false -> (Integer 0I,Base n)::acc | true -> (NegativeInfinity,Base n)::acc
                        let eComplexNumber acc x = acc//Need to look further into this
                        let eSymbol acc (v:Expression) = (Integer 1I,Base v)::acc
                        let eBinaryOp acc x = match x with | BinaryOp (a,ToThePowerOf,Number(Integer b)) -> (Integer b,Base x) :: acc | _ -> acc
                        let eUnaryOp acc x = acc
                        let eNaryOp acc x = acc
                        let acc = []                
                        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
                    let compare x' y' = Number.compare (fst x') (fst y')
                    let out = List.rev(List.sortWith compare powers)                           
                    List.collect (fun x ->  
                        let x' = 
                            match List.exists (fun (a,b) -> x = b) out with
                            | true -> List.filter (fun (a,b) -> b = x) out
                            | false -> [Number.Zero,x]
                        [(List.rev(List.sortWith compare x')).Head]) xList
                    |> List.fold (fun acc (a,b) -> acc + a) Number.Zero

        let ofGPE x xList = _GPE x xList            

        let ofTotalGPE u = 
            let var = Variables.ofExpression u
            ofGPE u var
            
        let rec ofAltGME u x = 
            match Check.isAltMonomialGPE u x with
            | false -> Undefined
            | true -> 
                match u with
                | a when a = x -> Integer 1I
                | a when a.Base = x -> match a.Exponent with | Number (Integer n ) -> Integer n | _ -> Undefined
                | NaryOp(Product, xList) -> 
                    let x'List = List.map(fun a -> ofAltGME a x) xList 
                                 |> List.sortWith Number.compare
                                 |> List.rev
                    x'List.Head
                | _ -> Integer 0I

        let ofAltGPE u x = 
            match Check.isAltPolynomialGPE u x with
            | false -> Undefined
            | true -> 
                match u with
                | NaryOp(Sum,xList) when 
                    (List.exists (fun a -> Check.isAltPolynomialGPE a x = false) xList) = false -> 
                        let x'List = List.map(fun a -> ofAltGME a x) xList 
                                     |> List.sortWith Number.compare
                                     |> List.rev
                        x'List.Head
                | _ -> ofAltGME u x

        let sort x u v  =
            let u' = (Number (ofGPE u [x]))
            let v' = (Number (ofGPE v [x]))
            Expression.compareExpressions u' v'

//
    [<RequireQualifiedAccess>]
    module Order =

        let isLexicographical u v L =
            match (Check.isMonomialGPE u L && Check.isMonomialGPE v L) || u = v with
            | false -> false
            | true -> 
                let compare x = ExpressionType.compareExpressions (Number(Degree.ofGPE u [x])) (Number(Degree.ofGPE v [x])) 
                let check = List.tryPick (fun x -> 
                    match compare x with
                    | 1 -> Some false
                    | 0 -> None
                    | -1 -> Some true
                    | _ -> None) L
                match check with
                | Some x -> x
                | None -> true  
                
        let lexicographicalMonomialList u L =
            let compare x' y' = 
                match isLexicographical x' y' L with
                | true -> -1
                | false -> 1
            match u with
            | NaryOp(Sum,xList) -> List.sortWith compare xList |> List.rev
            | x -> [x]
    
//Coefficients of a polynomial
    [<RequireQualifiedAccess>]
    module Coefficients =

        let ofMonomialSV m =
            match Check.isMonomialSV m with 
            | false -> Undefined
            | true -> 
                match m with
                | Number n -> n
                | Symbol v -> (Integer 1I)
                | BinaryOp (a,ToThePowerOf,b) -> (Integer 1I)
                | NaryOp (Product, [Number a;b]) -> a
                | _-> Undefined

        let ofPolynomialSV u j = 
            match Check.isPolynomialSV u with
            | false -> Undefined
            | true ->
                match Check.isMonomialSV u with
                | true -> match Degree.ofPolynomialSV u = j with | true ->  ofMonomialSV u | false -> Number.Zero
                | false -> 
                    match u with
                    | NaryOp(Sum, a) -> 
                        match List.tryPick (fun x -> match (Degree.ofPolynomialSV x) = j with | true -> Some x | false -> None) a with
                        | Some m -> ofMonomialSV m
                        | None -> Number.Zero
                    | _ -> Undefined

        let leadingSV x = ofPolynomialSV x (Degree.ofPolynomialSV x)

        let rec private _monomialGPE u x = 
            match u with
            | a when a = x -> (Number (Integer 1I), Integer 1I)
            | BinaryOp (a,ToThePowerOf,Number (Integer b)) when a = x && b > 1I -> (Number (Integer 1I), Integer b)        
            | NaryOp (Product, aList) -> 
                let f = List.map (fun a -> _monomialGPE a x ) aList
                let fTest = List.exists (fun (a,b) -> a = Number (Integer 1I)) f
                match fTest with             
                | true -> 
                    let x' = List.find (fun a -> fst (_monomialGPE a x) = Number (Integer 1I)) aList
                    (u/x',Degree.ofPolynomialSV x')
                | false -> (u,Number.Zero)
            | _ -> match freeOf u x with
                   | true -> (u,Number.Zero)
                   | false -> (Number Undefined,Undefined)

        let private _GPE u x j = 
            match u with 
            | NaryOp(Sum,aList) as u' when u' = x -> 
                match j = Integer 1I with 
                | true -> Number (Integer 1I)
                | false -> Number (Integer 0I)
            | NaryOp (Sum, aList) -> 
                List.fold (fun c a -> 
                    let f = _monomialGPE a x 
                    match f with 
                    | (Number Undefined,Undefined) -> Number Undefined
                    | (y,z) when z = j -> c + y
                    | _ -> c
                    ) (Number (Integer 0I)) aList
            | _ -> 
                let f = _monomialGPE u x
                match f with 
                | (Number Undefined,Undefined) -> Number Undefined
                | (y,z) when z = j -> y
                | _ -> Number (Integer 0I)     
        
        let ofMonomialGPE u x = 
            match Check.isPolynomialGPE u [x] with
            | false -> (Number Undefined,Undefined)
            | true -> _monomialGPE u x 

        let ofGPE u x j = 
            match Check.isPolynomialGPE u [x] with
            | false -> Number Undefined
            | true -> _GPE u x j        

        let ofAltMonomialGPE u x = 
            match Check.isAltPolynomialGPE u x with
            | false -> (Number Undefined,Undefined)
            | true -> _monomialGPE u x

        let ofAltGPE u x j = 
            match Check.isAltPolynomialGPE u x with
            | false -> Number Undefined
            | true -> _GPE u x j

        let leadingGPE u x =
            match Check.isPolynomialGPE u [x] with 
            | false -> Number Undefined
            | true -> ofGPE u x (Degree.ofGPE u [x])

        ///Let L be a list of symbols and let u be a multivariate polynomial the symbols of L with rational number coefficients
        let rec leadingNumerical u L = 
            match Check.isPolynomialGPE u L with 
            | true -> match L with
                      | [] -> u //LNC-1
                      | _ -> let x = L.Head
                             let l = leadingGPE u x
                             leadingNumerical l L.Tail //LNC-2
            | false -> Number Undefined
        
        ///Let L be a list of symbols and let u be a multivariate polynomial the symbols of L with rational number coefficients
        let signOfGPE u L =
            let s = leadingNumerical u L
            match s = Number Number.Zero with
            | true -> 0I
            | false -> match s with 
                       | Number (Integer i) when i > 0I ->  1I
                       | Number (Integer i) when i < 0I ->  -1I
                       | Number (Rational r) when r > Fraction.Zero ->  1I
                       | Number (Rational r) when r < Fraction.Zero ->  -1I
                       | _  -> 0I
        
        ///Let u be a multivariate polynomial
        let signVarOfGPE u =
            let T = Variables.ofExpression u
            let sub1 = List.mapi (fun i x -> (x,Symbol(Variable ("Temp" + i.ToString())))) T
            let tempExp = List.fold (fun acc (y,t) -> ExpressionStructure.substitute (y,t) acc) u sub1
            let T' = Variables.ofExpression tempExp
            signOfGPE tempExp T'

        let listOfGPE u x =
            let deg = Degree.ofGPE u [x]
            let rec coeffList deg list =
                match deg with 
                | Integer i as int when i = 0I -> (ofGPE u x int) :: list
                | Integer i as int -> 
                    let deg' = match deg with | Integer i when i > 0I -> Integer (i - 1I) | _ -> Integer 0I
                    let list' = (ofGPE u x int) :: list
                    coeffList deg' list'
                | _ -> list
            coeffList deg []

        let rec leadingMonomial u L =
            match L with
            | [] -> u
            | _ -> 
                let x = L.Head
                let m = Degree.ofGPE u [x]
                let c = ofGPE u x m
                x**(Number m) * (leadingMonomial c L.Tail)

//The collection of coefficients of like terms in a polynomial
    [<RequireQualifiedAccess>]
    module Collect = 

        ///Coeff var monomial(u, s) returns a two element tuple with the coefficient part and variable part of u.
        let private coeffVarMonomialGPE u s =
            List.fold (fun acc x ->         
                match Check.isMonomialGPE (fst acc) [x] with 
                | false -> (Number Undefined,Number Undefined)
                | true -> (fst(Coefficients.ofMonomialGPE (fst acc) x),((fst acc)/(fst(Coefficients.ofMonomialGPE (fst acc) x))*(snd acc)))
            ) (u,Number (Integer 1I)) s
     
        let private coeffVarMonomialAltGPE u s =
            List.fold (fun acc x ->         
                match Check.isAltMonomialGPE (fst acc) x with 
                | false -> (Number Undefined,Number Undefined)
                | true -> (fst(Coefficients.ofAltMonomialGPE (fst acc) x),((fst acc)/(fst(Coefficients.ofAltMonomialGPE (fst acc) x))*(snd acc)))
            ) (u,Number (Integer 1I)) s        
        
        let private terms coeffVarMonomial u s = 
            match u with
            | NaryOp(Sum, xList) -> 
                match List.exists (fun x -> x = u) s with 
                | true -> u
                | false ->                     
                    let pack xs =
                        let xs' = List.sortWith (fun (x : (Expression*Expression)) (y : (Expression*Expression)) -> 
                            (snd x) |> ExpressionType.compareExpressions <| (snd y)) xs
                        let collect x = function
                            | (y::xs')::xss when (snd x) = (snd y) -> (x::y::xs')::xss
                            | xss -> [x]::xss
                        List.foldBack collect xs' []
                    let t = pack (List.map (fun x -> coeffVarMonomial x s) xList)
                    let t' = [for i in 0..(t.Length - 1) -> (List.fold (fun acc (x,y) -> (acc + x)) (Number (Integer 0I)) t.[i]) * (snd (t.[i].Head))]
                             |> List.choose (fun elem ->
                                match elem with
                                | elem when elem <>  Number Undefined -> Some elem
                                | _ -> None)                     
                    match t'.Length with 
                    | 0 -> Number Undefined
                    | 1 -> t'.[0]
                    | _ -> NaryOp(Sum,t')

            | _ -> match coeffVarMonomial u s with
                   | (Number Undefined,Number Undefined) -> Number Undefined
                   | _ -> u

        let termsOfGPE u s = terms (coeffVarMonomialGPE) u s

        let termsOfAltGPE u s = terms (coeffVarMonomialAltGPE) u s

//Apply the distributive transformations to an expression
    [<RequireQualifiedAccess>]
    module Distribute =
        
        let expression u =
            match u with
            | NaryOp(Product,xList) -> 
                match Check.expressionListContainsSum xList with
                | false -> u
                | true -> 
                    let sum1 = List.find (fun x -> match x with | NaryOp(sum,aList) -> true | _ -> false) xList
                    let sum1List = match sum1 with | NaryOp(sum,aList) -> aList | _ -> []
                    match sum1List with
                    | [] -> u
                    | _ -> NaryOp(Sum,List.map (fun x -> (u/sum1)*x) sum1List)
            | _ -> u

//a limited version of factorization
    [<RequireQualifiedAccess>]
    module Factor =
        open System.Numerics
        open ExpressionStructure
        open Math.Pure.Structure.NumberTheory

        ///finds some factors that are common to u and v
        let rec common u v = 
            match u, v with
            | Number (Integer iu), Number (Integer iv) -> //CF-1
                Integer.gcd u v
            | NaryOp(Product,xList),_ -> //CF-2
                let f = xList.Head 
                let r = common f v
                r*(common (u/f) (v/r))
            | _, NaryOp(Product,xList) -> common v u //CF-3
            | _ -> //CF-4
                let baseU, exponentU = ExpressionType.Base u, ExpressionType.Exponent u
                let baseV, exponentV= ExpressionType.Base v, ExpressionType.Exponent v
                match baseU = baseV, exponentU, exponentV with
                | true, Number (Integer nu), Number (Integer nv) when nu >= 0I && nv >= 0I -> baseU**(Number (Number.min (Integer nu) (Integer nv)))
                | true, Number (Rational nu), Number (Rational nv) when nu >= Fraction.Zero && nv >= Fraction.Zero -> baseU**(Number (Number.min (Rational nu) (Rational nv)))
                | true, Number (Integer nu), Number (Rational nv) when nu >= 0I && nv >= Fraction.Zero -> baseU**(Number (Number.min (Integer nu) (Rational nv)))
                | true, Number (Rational nu), Number (Integer nv) when nu >= Fraction.Zero && nv >= 0I -> baseU**(Number (Number.min (Rational nu) (Integer nv)))
                | _ -> Number (Integer 1I)
        
        ///performs a limited version of factorization.
        let rec out u = 
            match u with
            | NaryOp(Product,xList) -> NaryOp(Product,List.map out xList) |> ExpressionType.simplifyExpression //FO-1
            | BinaryOp(x,ToThePowerOf,y) -> (out x)**y //FO-2
            | NaryOp(Sum,xList) -> //FO-3
                let s = NaryOp(Sum,List.map out xList) |> ExpressionType.simplifyExpression
                match s with
                | NaryOp(Sum,sList) ->  
                    let c = List.fold (fun acc x -> common acc x) sList.[0] sList
                    c * (NaryOp(Sum,(List.map (fun x -> x/c) sList))) |> ExpressionType.simplifyExpression
                | _ -> s
            | _ -> u //FO-4

        let seperate u x = 
            match u with 
            | NaryOp(Product,uList) -> 
                let freeOfPart, dependantPart = List.partition (fun a -> freeOf a x) uList
                let ifEmpty l = match l with |[] -> Number(Integer 1I) | _ -> List.fold (fun acc w -> w*acc) (Number(Integer 1I)) l
                (ifEmpty freeOfPart,ifEmpty dependantPart)
            | _ -> 
                match freeOf u x with
                | true -> (u,Number(Integer 1I))
                | false -> (Number(Integer 1I),u)

        let candidates n' d' =
            let abs num = 
                match num with 
                | Number n as N -> ExpressionFunction.abs N
                | _ -> num
            let n, d = abs n', abs d'
            let expand l =                
                let rec comb accLst elemLst =
                    match elemLst with
                    | h::t ->
                        let next = [h]::List.map (fun el -> h::el) accLst @ accLst
                        comb next t
                    | _ -> accLst
                comb [] l |> Seq.distinct|> Seq.toList   
            let rawCandidatesN = expand (NumberTheory.Integer.factorByTrialDivision n)
            let rawCandidatesD = expand (NumberTheory.Integer.factorByTrialDivision d)
            let positiveCandidatesN = (Number(Integer 1I))::(List.map (fun x -> List.fold (fun x' acc -> x' * acc ) (Number(Integer 1I)) x) rawCandidatesN)                                       
                                      |> Seq.distinct  
                                      |> Seq.toList
            let negativeCandidatesN = List.map (fun x -> -x) positiveCandidatesN
            let positiveCandidatesD = (Number(Integer 1I))::(List.map (fun x -> List.fold (fun x' acc -> x' * acc ) (Number(Integer 1I)) x) rawCandidatesD)                                      
                                      |> Seq.distinct  
                                      |> Seq.toList
            let negativeCandidatesD = List.map (fun x -> -x) positiveCandidatesD
            let nCandidates = List.concat [positiveCandidatesN; negativeCandidatesN]
            let dCandidates = List.concat [positiveCandidatesD; negativeCandidatesD]
            (List.map (fun x -> List.map (fun y -> x / y) dCandidates) nCandidates) |> List.concat |> Seq.distinct|> Seq.toList

//Rational Expressions
    [<RequireQualifiedAccess>]
    module RationalExpression =
        
        let rec private _Numerator u = ExpressionStructure.numerator u 
        let rec private _Denominator u = ExpressionStructure.denominator u
 
        let rec transformSum u v =
            let m, r, n, s = _Numerator u, _Denominator u, _Numerator v, _Denominator v
            match r = Number (Integer 1I) && s = Number (Integer 1I) with
            | true -> u + v
            | false -> (transformSum (m*s) (n*r))/(r*s)

        let rec transform u = 
            match u with 
            | BinaryOp(x,ToThePowerOf,n) -> (transform x)**n
            | NaryOp(Product,xList) -> 
                let f = xList.[0]
                (transform f)*(transform (u/f))
            | NaryOp(Sum, xList) -> 
                let f = xList.[0]
                let g = transform f
                let r = transform (u-f)
                transformSum g r
            | _ -> u

        let numerator u = 
            match u with
            | NaryOp(Sum,xList) -> transform u |> _Numerator
            | _ -> _Numerator u

        let denominator u = 
            match u with
            | NaryOp(Sum,xList) -> transform u |> _Denominator
            | _ -> _Denominator u
            
        let formCheck u = 
            match u with 
            | Number n -> true
            | Symbol s -> true
            | UnaryOp(op,x) -> true
            | _  ->                 
                let S = (Variables.ofRationalExpression u)
                let checkA = List.forall (fun x -> (denominator x = Number(Integer 1I))) S
                match checkA with
                | true ->
                    let checkB = 
                        let rec checkCoeff x = 
                            let xVars = Variables.ofExpression x
                            let x' = match xVars.Length = 1 with
                                     | true -> xVars.[0]
                                     | false -> (NaryOp(Product,xVars))
                            match x with 
                            | NaryOp(Sum,xList) -> List.forall (fun a -> checkCoeff a) xList
                            | _ -> match Coefficients.ofMonomialGPE x x' with
                                   | (_,Undefined) -> false
                                   | (Number(Integer i),_) -> true
                                   | _ -> false                               
                        checkCoeff u
                    checkB
                | false -> false

        let cancel u = (Factor.out (numerator u)) / (Factor.out (denominator u))

//Apply the two distributive transformations to an expression
    [<RequireQualifiedAccess>]
    module Expand = 
                
        let rec product r s = 
            match r with 
            | NaryOp(Sum, a::b) when b.Length > 1 -> (product a s) + (product ((NaryOp(Sum,b))) s)
            | NaryOp(Sum, a::b::[]) -> (product a s) + (product b s)
            | _ ->
                match s with
                | NaryOp(Sum, xList) -> product s r 
                | _ -> r*s

        let rec productP r s p = 
            match r with 
            | NaryOp(Sum, a::b) when b.Length > 1 -> (productP a s p) + (productP ((NaryOp(Sum,b))) s p)
            | NaryOp(Sum, a::b::[]) -> (productP a s p) + (productP b s p)
            | _ ->
                match s with
                | NaryOp(Sum, xList) -> productP s r p 
                | _ -> 
                    match r*s with                    
                    | Number(Integer i) when i = 0I -> Number(Integer 0I)
                    | Number (Integer i) as a -> NumberTheory.Integer.remainder a p
                    | Number (Rational r)  -> NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                    | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                    | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                    | _ -> r*s

        let rec power u (n:NumberType) =             
            let bExp (k : NumberType) = (Number(!*n))/(Number((!*k)*(!*(n + -k))))            
            let n' = match n with | Integer a -> a | _ -> 0I
            match u with
            | NaryOp(Sum, xList) -> 
                let f = match xList with | a::[] -> a | a::b -> a | _ ->  Number Undefined
                let r =  match xList with | a::[] -> a | a::b::[] -> b | a::b when b.Length > 1 -> NaryOp(Sum, b) | _ ->  Number Undefined
                let kList = [for i in 0I..n' -> Number(Integer i)]
                List.sumBy (fun k -> 
                    let k' = match k with | Number (Integer a) -> Integer a | _ -> Integer 0I
                    product ((bExp k')*(f**(Number(n+ -k')))) (power r k')) kList
            | _ -> u**(Number n)

        let rec powerP u (n:NumberType) p =             
            let bExp (k : NumberType) = (Number(!*n))/(Number((!*k)*(!*(n + -k))))            
            let n' = match n with | Integer a -> a | _ -> 0I
            match u with
            | NaryOp(Sum, xList) -> 
                let f = match xList with | a::[] -> a | a::b -> a | _ ->  Number Undefined
                let r =  match xList with | a::[] -> a | a::b::[] -> b | a::b when b.Length > 1 -> NaryOp(Sum, b) | _ ->  Number Undefined
                let kList = [for i in 0I..n' -> Number(Integer i)]
                let out = 
                    List.sumBy (fun k -> 
                        let k' = match k with | Number (Integer a) -> Integer a | _ -> Integer 0I
                        product ((bExp k')*(f**(Number(n+ -k')))) (power r k')
                        ) kList
                match out with
                    | NaryOp(Sum, yList) -> 
                        let yList' = 
                            List.map (fun y -> 
                            match y with
                            | Number (Integer i) -> NumberTheory.Integer.remainder y p
                            | Number (Rational r)  -> NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                            | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                            | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                            | _ -> y
                            ) yList
                        NaryOp(Sum, yList')
                    | _ -> out
            | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
            | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
            | Number (Integer i) -> NumberTheory.Integer.remainder u p
            | Number (Rational r) -> NumberTheory.Field.divisionP ( NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) ( NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
            | _ -> u**(Number n)
 
        let algebraicExpression uIn =  
            let rec expandExp u' =
                let u = ExpressionType.simplifyExpression u'
                
                let check u' = 
                    match Check.expressionListContainsSum (Variables.ofExpression u') && u' <> u with 
                    | true -> expandExp u'
                    | false -> u'
                
                match u with
                | NaryOp(Sum, xList) ->
                    let v = xList.Head
                    match xList.Tail with
                    | [] -> (expandExp v)
                    | _ -> (expandExp v) + (expandExp (NaryOp(Sum,xList.Tail))) |> check            
                | NaryOp(Product, xList) when Check.expressionListContainsNegativePower xList ->
                    let denominator, numerator = 
                        List.partition (fun x ->     
                        match x with
                        | BinaryOp(a,ToThePowerOf,Number(Integer b)) when b < 0I -> true
                        | _ -> false ) xList
                    let numerator' = expandExp (ExpressionType.simplifyExpression (NaryOp(Product,numerator)))
                    let denominator' = expandExp (ExpressionType.simplifyExpression (NaryOp(Product,(List.map (fun (x:Expression) -> 
                        (x.Base** (Number(Integer -1I) * x.Exponent))) denominator))))
                    numerator'*(denominator'**(Number(Integer -1I))) |> check 
                | NaryOp(Product, xList) ->
                    match xList.Tail with
                    | [] -> expandExp xList.Head                    
                    | _ -> product (expandExp xList.Head) (expandExp (NaryOp(Product,xList.Tail))) |> check 
                | BinaryOp(x,ToThePowerOf,n) -> 
                    match n with 
                    | Number (Integer i) when i >= 2I -> power x (Integer i) |> check
                    | Number (Integer i) when i < 0I -> let x' = (power x (Integer -i)) |> check 
                                                        x'**Number(Integer -1I)
                    | Number (Rational r) when r.numerator > r.denominator && r.numerator > 0I -> 
                        (power x (Integer r.Floor)) * ((expandExp x)**
                            (Number (Rational {r with numerator = (r.numerator-(r.Floor*r.denominator))}))) //|> check 
                    | _ -> u //check u
                | BinaryOp(x,op,n) -> BinaryOp(expandExp x,op,expandExp n) //|> check 
                | UnaryOp(op,x) -> UnaryOp(op,expandExp x) |> check 
                | _ -> check u                            
            Collect.termsOfGPE (expandExp uIn) (Variables.ofExpression (expandExp uIn)) |> ExpressionType.simplifyExpression

        let algebraicExpressionP uIn p =  
            let rec expandExp u' =
                let u = ExpressionType.simplifyExpression u'                
                let check u'' = 
                    match Check.expressionListContainsSum (Variables.ofExpression u'') && u'' <> u with 
                    | true -> expandExp u''
                    | false -> u''                
                match u with
                | NaryOp(Sum, xList) ->
                    match xList with
                    | a::b when b.Length > 1 -> (expandExp a) + (expandExp (NaryOp(Sum,b))) 
                    | a::b::[] -> (expandExp a) + (expandExp b) 
                    | a::[] -> (expandExp a)
                    | _ -> Number Undefined
                | NaryOp(Product, xList) ->
                    match xList with
                    | a::b when b.Length > 1 -> productP (expandExp a) (expandExp (NaryOp(Product,b))) p 
                    | a::b::[] -> productP (expandExp a) (expandExp b) p
                    | a::[] -> (expandExp a)  
                    | _ -> Number Undefined
                | BinaryOp(x,ToThePowerOf,n) -> 
                    match n with 
                    | Number (Integer i) when i >= 2I -> powerP x (Integer i) p                     
                    | _ -> u 
                | BinaryOp(x,op,n) -> BinaryOp(expandExp x,op,expandExp n)  
                | UnaryOp(op,x) -> UnaryOp(op,expandExp x)  
                | Number (Integer i) as n -> NumberTheory.Integer.remainder n p
                | Number (Rational r) -> NumberTheory.Integer.remainder (Number (Integer r.numerator) * (NumberTheory.Field.multiplicativeInverseP (Number (Integer r.denominator)) p)) p
                | _ -> u                           
            Collect.termsOfGPE (expandExp uIn) (Variables.ofExpression (expandExp uIn)) 
      
        let mainOpOf u = 
            match u with
            | NaryOp(Product, xList) ->
                let v = xList.Head
                match xList.Tail with
                | [] -> v
                | _ -> product (v) (NaryOp(Product,xList.Tail))
            | BinaryOp(x,ToThePowerOf,n) -> 
                match n with 
                | Number (Integer i) when i >= 2I -> power x (Integer i)
                | Number (Integer i) when i < 0I -> let x' = (power x (Integer -i))
                                                    x'**Number(Integer -1I)
                | Number (Rational r) when r.numerator > r.denominator && r.numerator > 0I -> 
                    (power (x.Base) (Integer r.Floor)) * (x.Base)**
                        (Number (Rational {r with numerator = r.numerator-(r.Floor*r.denominator)})) 
                | _ -> u
            | _ -> u
       
        let restricted u (T : Expression list)=
            let sub1 = List.mapi (fun i x -> (x,Symbol(Variable ("Temp" + i.ToString())))) T
            let sub2 = List.mapi (fun i x -> (Symbol(Variable ("Temp" + i.ToString())),x)) T
            let tempExp = List.fold (fun acc (y,t) -> ExpressionStructure.substitute (y,t) acc) u sub1 |> algebraicExpression
            List.fold (fun acc (y,t) -> ExpressionStructure.substitute (y,t) acc) tempExp sub2

        let rec rational u = 
            match RationalExpression.formCheck u with
            | true ->
                let uD = algebraicExpression (RationalExpression.denominator u)
                let uN = algebraicExpression (RationalExpression.numerator u)
                let out = uN/uD
                match RationalExpression.formCheck out with
                | true -> out
                | false -> rational out
            | false -> rational (RationalExpression.transform u)

        let binomialCoeff (n:Expression) (j:Expression) = (!*n)/((!*j) * (!*(n - j)))

//Polynomial Division
    [<RequireQualifiedAccess>]
    module Divide =
        open NumberTheory
        
        /// Divide algebraic expressions
        let algebraicExpressions u v x = 
            match Check.isPolynomialGPE u [x] && Check.isPolynomialGPE v [x] && v <> Number (Integer 0I) with
            | true ->
                let n = Degree.ofGPE v [x]
                let lcv = Coefficients.leadingGPE v x
                let rec loop q r =      
                    let m = Degree.ofGPE r [x]              
                    let compareMN = Number.compare m n 
                    match compareMN = 1 || compareMN = 0 with
                    | false -> (q, r)
                    | true -> 
                        let lcr = Coefficients.leadingGPE r x
                        let s = lcr/lcv
                        let q' = q + (s * x**(Number m - Number n))
                        let r' = ((r - (lcr * x**(Number m))) - ((v - (lcv * x**(Number n))) * s * (x**(Number m - Number n)))) |> Expand.algebraicExpression 
                        loop q' r'
                loop (Number (Integer 0I)) u
            | false -> (Number Undefined,Number Undefined)

        let getQuotient u v x = fst (algebraicExpressions u v x)

        let getRemainder u v x = snd (algebraicExpressions u v x)
        
        /// Recursive division
        let rec recursiveMV u v L K = 
            match Check.isPolynomialGPE u L && Check.isPolynomialGPE v L && v <> Number (Integer 0I) with
            | true ->
                match L with
                | [] -> 
                    match K with
                    | Constant.IntegralNumbers ->
                        match u/v with 
                        | Number (Integer i) -> (u/v,Number(Integer 0I))
                        | _ -> (Number(Integer 0I),u)
                    | _ -> (u/v,Number(Integer 0I))
                | _ ->
                    let x = L.Head
                    let r = u
                    let m = Number (Degree.ofGPE r [x])
                    let n = Number (Degree.ofGPE v [x])
                    let q = Number(Integer 0I)
                    let lcv = Coefficients.leadingGPE v x
                    let rec loop q r m = 
                        let lcr = Coefficients.leadingGPE r x
                        let d = recursiveMV lcr lcv L.Tail K
                        match snd d <> Number(Integer 0I) with
                        | true -> (Expand.algebraicExpression q,r)
                        | false -> 
                            let c = fst d
                            let q' = q + c*(x**(m-n))
                            let r' = Expand.algebraicExpression (r-c*v*(x**(m-n)))
                            let m' = Number (Degree.ofGPE r' [x])                            
                            match Expression.compareExpressions m n = -1 with
                            | false -> loop q' r' m'
                            | true -> (Expand.algebraicExpression q,r)
                    loop q r m
            | false -> (Number Undefined,Number Undefined)
        
        let getRecQuotient u v L K = fst (recursiveMV u v L K)

        let getRecRemainder u v L K = snd (recursiveMV u v L K)

        let G u v =    
            match Check.isMonomialGPE v (Variables.ofExpression v) && v <> Number(Integer 0I) with
            | false -> Number(Integer 0I)
            | true -> 
                let L = Order.lexicographicalMonomialList u (Variables.ofExpression v)
                let rec loop out (L': Expression list) =
                    match L' with
                    | [] -> out
                    | _ -> 
                        let check = Expand.algebraicExpression (L'.Head / v)            
                        match RationalExpression.denominator check  with 
                        | Number n when n <> Integer 0I ->             
                            let out' = out + check
                            loop out' L'.Tail
                        | _ -> loop out L'.Tail                
                loop (Number(Integer 0I)) L
        
        /// Monomial based division
        let monomialBased u v L =
            let q = Number(Integer 0I)
            let vT = Coefficients.leadingMonomial v L
            let f = G u vT
            let rec loop q' r' f' =
                match f' = Number(Integer 0I) with
                | true -> (q', r')
                | false -> 
                    let q'' = q' + f'
                    let r'' = Expand.algebraicExpression (r' - f' * v)
                    let f'' =  G r'' vT
                    loop q'' r'' f''
            loop q u f

        let getMBQuotient u v L = fst (monomialBased u v L)

        let getMBRemainder u v L = snd (monomialBased u v L)
        
        /// Algebraic number field division
        let algebraicExpressionsP u v x p = 
            match Check.isPolynomialGPE u [x] && Check.isPolynomialGPE v [x] && v <> Number (Integer 0I) with  
            | true ->
                let n = Degree.ofGPE v [x]
                let lcv = Coefficients.leadingGPE v x
                let pAdd x' y = Integer.remainder (x' + y) p
                let pSub x' y = Integer.remainder (x' + y) (-p)
                let pMult x' y = Integer.remainder (x' * y) p
                let convertP out = 
                    match out with
                        | NaryOp(Sum, yList) -> 
                            let yList' = 
                                List.map (fun y -> 
                                match y with
                                | Number (Integer i) -> NumberTheory.Integer.remainder y p
                                | Number (Rational r)  -> NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                                | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                                | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                                | _ -> y
                                ) yList
                            NaryOp(Sum, yList')
                        | Number (Integer i) as y -> NumberTheory.Integer.remainder y p
                        | Number (Rational r)  -> NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                        | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                        | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                        | _ -> out
                let rec loop q r =      
                    let m = Degree.ofGPE r [x]              
                    let compareMN = Number.compare m n 
                    match compareMN = 1 || compareMN = 0, n = Integer 0I with
                    | _, true -> ((Expand.algebraicExpressionP ((NumberTheory.Field.multiplicativeInverseP v p)*u) p), Number(Integer 0I))
                    | false, false-> (q, r)
                    | true, false -> 
                        let lcr = Coefficients.leadingGPE r x
                        let s = Field.divisionP lcr lcv p
                        let q' = q + (s * x**(Number m - Number n))
                        let r' = ((r - (lcr * x**(Number m))) - ((v - (lcv * x**(Number n))) * s * (x**(Number m - Number n)))) |> Expand.algebraicExpression |> convertP
                        loop q' r'
                let out = loop (Number (Integer 0I)) u
                (ExpressionType.simplifyExpression (fst out),ExpressionType.simplifyExpression (snd out))
            | false -> (Number Undefined,Number Undefined)

        let getQuotientP u v x p = fst (algebraicExpressionsP u v x p)

        let getRemainderP u v x p = snd (algebraicExpressionsP u v x p)
        
        /// Pseudo division
        let pseudo u v x = 
            let m = Degree.ofGPE u [x]
            let n = Degree.ofGPE v [x]
            let delta =
                match ExpressionType.compareExpressions (Number m - Number n + Number(Integer 1I)) (Number(Integer 0I)) with
                | 1 -> (Number m - Number n + Number(Integer 1I))
                | _ -> Number(Integer 0I)
            let lcv = Coefficients.ofGPE v x n
            let rec loop s p sigma m' =
                match ExpressionType.compareExpressions (Number m') (Number n) with
                | -1 -> (Expand.algebraicExpression (p * lcv**(delta - Number sigma)),Expand.algebraicExpression (s * lcv**(delta - Number sigma)))
                | _ -> 
                    let lcs = Coefficients.ofGPE s x m'
                    let p' = lcv * p + lcs * x**(Number m' - Number n)
                    let s' = Expand.algebraicExpression (lcv * s - lcs * v * x**(Number m' - Number n))
                    let sigma' = sigma + Integer 1I
                    let m'' = Degree.ofGPE s' [x]
                    loop s' p' sigma' m''
            loop u (Number(Integer 0I)) (Integer 0I) m

        let pseudoQuotient u v x = fst (pseudo u v x)

        let pseudoRemainder u v x = snd (pseudo u v x)

    [<RequireQualifiedAccess>]
    module Reduce =
        
        /// Multiple division
        let byMultipleDivision u F L = 
            let F' = List.map (fun x -> Coefficients.leadingMonomial x L) F
            let C = [for i in 1..(F.Length) -> Number(Integer 0I)]
            let checkNotReduced a = List.exists (fun x -> Divide.getMBQuotient a x L <> Number(Integer 0I) ) F'
            let QR c r =
                List.fold2(fun (acc, r) f c -> 
                    let q = Divide.getMBQuotient r f L
                    let r = Divide.getMBRemainder r f L
                    List.concat [acc; [(c + q)]],r
                    ) ([], r) F c            
            let rec loop (c, r) =
                match checkNotReduced r with
                | true -> loop (QR c r)
                | false -> (c , r)
            loop (C, u)

        let getQuotientMD u F L = fst (byMultipleDivision u F L)

        let getRemainderMD u F L = snd (byMultipleDivision u F L)
        
        let getMultipleResult u F L =
            let c = getQuotientMD u F L
            let r = getRemainderMD u F L            
            //(List.fold2 (fun acc c f -> acc + c*f) (Number(Integer 0I)) c F) + r
            NaryOp(Sum,List.concat [(List.map2 (fun c f -> c*f) c F); (match r = Number(Integer 0I) with |true -> [] |false -> [r])])

        /// Reduction Algorithm
        let byReductionAlgorithm u (F:Expression list) L =
            let C'' = [for i in 1..(F.Length) -> Number(Integer 0I)]
            let rec loop R r i (C:Expression list) =
                match R = Number(Integer 0I) with
                | true -> (C,r)
                | false ->                     
                    let m = Coefficients.leadingMonomial R L
                    let f = F.[i]
                    let lmf = Coefficients.leadingMonomial f L
                    let Q = m/lmf |> Expand.algebraicExpression
                    match RationalExpression.denominator Q with
                    | Number(Integer int) -> 
                        let R' = Collect.termsOfGPE (R - Q*f) [] |> Expand.algebraicExpression
                        let C' = List.mapi (fun i' x -> match i' = i with |true -> x + Q |false-> x) C
                        loop R' r 0 C'
                    | _ -> 
                        let i' = i + 1
                        match i' = F.Length with
                        | true ->                         
                            let R' = Collect.termsOfGPE (R - m) []
                            let r' = Collect.termsOfGPE (r + m) []
                            loop R' r' 0 C
                        | false -> loop R r i' C 
            loop u (Number(Integer 0I)) 0 C''

        let getQuotientRA u F L = fst (byReductionAlgorithm u F L)

        let getRemainderRA u F L = snd (byReductionAlgorithm u F L)
        
        let getReductionResult u F L =
            let c = getQuotientRA u F L
            let r = getRemainderRA u F L            
            //(List.fold2 (fun acc c f -> acc + c*f) (Number(Integer 0I)) c F) + r
            NaryOp(Sum,List.concat [(List.map2 (fun c f -> c*f) c F); (match r = Number(Integer 0I) with |true -> [] |false -> [r])])

//Polynomial Expansion
    [<RequireQualifiedAccess>]
    module Expansion =
        
        let rec ofGPE u v x t =
            match Check.isPolynomialGPE u [x] && Check.isPolynomialGPE v [x] with
            | true when u = Number(Integer 0I) && Number.compare (Degree.ofGPE v [x]) Number.Zero = 1 -> Number(Integer 0I)
            | true when Number.compare (Degree.ofGPE v [x]) Number.Zero = 1 -> 
                let d = Divide.algebraicExpressions u v x
                let q = fst d
                let r = snd d
                Expand.algebraicExpression (t* (ofGPE q v x t) + r)
            | _ -> Number Undefined
            
        ///True if u is a polynomial in terms of v, false otherwise
        let isGPE u v x = 
            match Check.isPolynomialGPE u [x] && Check.isPolynomialGPE v [x] with
            | true -> 
                let symbol = Symbol (Variable "checkExpansionOfUandV")
                let check = ofGPE u v x symbol
                let deg = Degree.ofGPE check [symbol]
                let rec checkQ deg' = 
                    match deg' with
                    | Integer i when i < 1I -> false
                    | Integer i when i = 1I ->  
                        match Coefficients.ofGPE check symbol deg' with
                        | Number (Integer j) -> true
                        | Number (Rational r) -> true
                        | _ -> false
                    | Integer i when i > 1I ->  
                        match Coefficients.ofGPE check symbol deg' with
                        | Number (Integer j) -> checkQ (Integer (i - 1I))
                        | Number (Rational r) -> checkQ (Integer (i - 1I))
                        | _ -> false
                    | _ -> false
                checkQ deg
            | false -> false
        
        ///Returns Degree of u in terms of v 
        let degreeOfGPE u v x = 
            match isGPE u v x with
            | true -> 
                let symbol = Symbol (Variable "checkExpansionOfUandV ")
                let exp = ofGPE u v x symbol
                Degree.ofGPE exp [symbol]
            | false -> Undefined


        let coefficientOfGPE u v x i = 
            let i' = match i with | Number n -> n | _ -> Undefined
            match isGPE u v x with
            | true -> 
                let symbol = Symbol (Variable "checkExpansionOfUandV ")
                let exp = ofGPE u v x symbol
                Coefficients.ofGPE exp symbol i'
            | false -> Number Undefined

        let rec monomialBased u v L t =
            match u = Number (Number.Zero) with
            | true -> Number (Number.Zero)
            | false -> 
                let q, r = Divide.getMBQuotient u v L, Divide.getMBRemainder u v L
                t * (monomialBased q v L t ) + r |> Expand.algebraicExpression

        
        let degreeOfPolynomialMV u v L =
            match Check.isPolynomialGPE u (v::L) with
            | true ->
                let globalT = Symbol(Variable "GlobalT")
                let w = monomialBased u v L globalT
                Degree.ofGPE w [globalT]
            | false -> Undefined

        let coefficientsOfPolynomialMV u v L j =
            match Check.isPolynomialGPE u (v::L) with
            | true ->
                let globalT = Symbol(Variable "GlobalT")
                let w = monomialBased u v L globalT
                Coefficients.ofGPE w globalT j            
            | false -> Number Undefined

 //Euclidean Algorithm
    [<RequireQualifiedAccess>]
    module Euclidean = 
        
        ///Returns (GCD(u,v),A,B)
        let extAlgorithm u v x = 
            let zero = Number (Integer 0I)
            let one = Number (Integer 1I)
            match u = zero && v = zero with
            | true -> (zero,zero,zero)
            | false -> 
                let rec loop u' v' app ap bpp bp =
                    let c = Coefficients.leadingGPE u' x
                    match v' = zero with
                    | false -> 
                        let q = Divide.getQuotient u' v' x
                        let r = Divide.getRemainder u' v' x
                        let ap' = app - q*ap
                        let bp' = bpp - q*bp
                        loop v' r ap ap' bp bp'
                    | true -> ((u'/c) |> Expand.algebraicExpression, (app/c) |> Expand.algebraicExpression, (bpp/c) |> Expand.algebraicExpression )
                loop u v one zero zero one
        
        ///Returns (A,B,GCD(u,v))
        let extAlgorithmP u v x p = 
            let convertP out = 
                match out with
                    | NaryOp(Sum, yList) -> 
                        let yList' = 
                            List.map (fun y -> 
                            match y with
                            | Number (Integer i) -> NumberTheory.Integer.remainder y p
                            | Number (Rational r)  -> NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                            | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                            | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                            | _ -> y
                            ) yList
                        NaryOp(Sum, yList')
                    | Number (Integer i) as y -> NumberTheory.Integer.remainder y p
                    | Number (Rational r)  -> NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p
                    | NaryOp(Product, Number (Integer i)::b) -> NaryOp(Product, (NumberTheory.Integer.remainder (Number (Integer i)) p)::b)
                    | NaryOp(Product, Number (Rational r)::b) -> NaryOp(Product, (NumberTheory.Field.divisionP (NumberTheory.Integer.remainder (Number (Integer r.numerator)) p) (NumberTheory.Integer.remainder (Number (Integer r.denominator)) p) p)::b)
                    | _ -> out            
            let zero = Number (Integer 0I)
            let one = Number (Integer 1I)
            match u = zero && v = zero with
            | true -> (zero,zero,zero)
            | false -> 
                let rec loop u' v' app ap bpp bp =
                    let c = Coefficients.leadingGPE u' x
                    match v' = zero with
                    | false -> 
                        let q = Divide.getQuotientP u' v' x p
                        let r = Divide.getRemainderP u' v' x p
                        let ap' = app - q*ap |> convertP
                        let bp' = bpp - q*bp |> convertP
                        loop v' r ap ap' bp bp'
                    | true -> ((app/c) |> Expand.algebraicExpression |> convertP, (bpp/c) |> Expand.algebraicExpression |> convertP, (u'/c) |> Expand.algebraicExpression |> convertP)
                loop u v one zero zero one

//Polynomial Divisors
    [<RequireQualifiedAccess>]
    module Divisors =
        open Math.Foundations.Logic
        
        let areAssosiateInQ u v x =
            match u <> Number(Integer 0I) && v <> Number(Integer 0I) with
            | true ->
                let r = Divide.getRemainder u v x
                let q = Divide.getQuotient u v x 
                match r, q with
                | Number R, Number Q when R = Integer 0I && Q <> Integer 0I -> (true,q)
                | _ -> (false,Number Undefined)
            | false -> (false,Number Undefined)
        
        let makeMonic u x =
            match Coefficients.leadingGPE u x = Number(Integer 1I) with 
            | false -> Divide.getQuotient u (Coefficients.leadingGPE u x ) x
            | true -> u
             
        //Returns the set of monic divisors of u with positive degree.        
        let rec ofPolynomial u x = 
            match Check.isPolynomialGPE u [x] with
            | true -> 
                let coeffList = Coefficients.listOfGPE u x
                let firstCoeffIndex = List.findIndex (fun x -> x <> Number(Integer 0I)) coeffList
                let candidates = Factor.candidates (coeffList.[firstCoeffIndex]) (coeffList.[(coeffList.Length - 1)])                    
                let check n = ExpressionStructure.substitute (x,n) u |> ExpressionType.simplifyExpression
                let rootlist = 
                    let rl = List.choose (fun x' -> match check x' with | Number (Integer i) when i = 0I -> Some (x - x') | _ -> None) candidates
                    match Check.sumContainsConstantTerm u = true with
                    | true -> rl
                    | false -> x :: rl
                let deg = match Degree.ofGPE u [x] with | Integer i -> i | _ -> 0I
                let rec list degree roots acc =
                    match degree with 
                    | i when i = 1I -> List.concat [roots; acc] |> Seq.distinct|> Seq.toList 
                    | _ -> 
                        let roots' =
                            match roots with
                            | [] -> List.map (fun x' -> x**(Number(Integer(degree-1I))) - x') candidates
                            | _ -> roots
                        let newRoots = List.choose (fun v -> 
                            match Divide.getRemainder (Expand.algebraicExpression u) (Expand.algebraicExpression v) x = Number (Integer 0I) with
                            | true -> Some (makeMonic (Divide.getQuotient (Expand.algebraicExpression u) (Expand.algebraicExpression v) x) x)
                            | false -> None) roots'
                        list (degree - 1I) newRoots (List.concat [acc; roots])                
                let out1 = list deg rootlist []
                let out2 = List.map (fun x' -> 
                    match Degree.ofGPE x' [x] = Integer 1I with 
                    | true -> []
                    | false -> ofPolynomial x' x) out1 |> List.concat
                let out3 = List.concat [out1; out2] |> Seq.distinct |> Seq.toList
                let out4 = List.collect (fun x' -> [for i in out3 -> makeMonic (Divide.getQuotient u x' x) x]) out3 //|> List.map (fun x' -> makeMonic x' x) |> Seq.distinct |> Seq.toList
                let out5 = [makeMonic u x]
                List.concat [out3;out4;out5] |> Seq.distinct |> Seq.toList                
            | false -> [] 
            //|> List.sortWith (fun x' -> Degree.sort x x')

        let gcd u v x = 
            match Check.isPolynomialGPE u [x] with
            | false -> Number Undefined
            | true -> 
                match u, v with
                | Number (Integer a), Number (Integer b) when a = 0I && b = 0I -> Number (Integer 0I)
                | _ -> 
                    let rec recurse U V = 
                        match V = Number (Integer 0I) with
                        | true -> Expand.algebraicExpression ((Number (Integer 1I) / (Coefficients.leadingGPE U x))*U)
                        | false -> 
                            let R = Divide.getRemainder U V x
                            recurse V R
                    recurse u v

        let gcdP u v x p = 
            match Check.isPolynomialGPE u [x] with
            | false -> Number Undefined
            | true -> 
                match u, v with
                | Number (Integer a), Number (Integer b) when a = 0I && b = 0I -> Number (Integer 0I)
                | _ -> 
                    let rec recurse U V = 
                        match V = Number (Integer 0I) with
                        | true -> Expand.algebraicExpressionP ((NumberTheory.Field.multiplicativeInverseP (Coefficients.leadingGPE U x) p)*U) p
                        | false -> 
                            let R = Divide.getRemainderP U V x p/////
                            match R = Number Undefined with 
                            | true -> Number Undefined
                            | false -> recurse V R
                    recurse u v
 
        let rec checkPairwiseRelativelyPrime (nList : Expression list) x =
            let h, t = nList.Head, nList.Tail
            let check = List.forall (fun x' -> gcd h x' x = (Number (Integer 1I))) t
            match check with
            | true -> 
                match t.Length > 1 with
                | true -> checkPairwiseRelativelyPrime t x
                | false -> true
            | false -> false

        //Chinese remainder theorem procedure that obtains the solution to the system of remainser equations
        let chineseRemainder (M : Expression list) (X : Expression list) x =
            let second (_,b,_) = b
            let third (_,_,c) = c
            let checkX =
                match M.Length = X.Length with
                | false -> false
                | true ->  List.forall2 (fun m x' -> ExpressionType.compareExpressions (Number(Degree.ofGPE m [x])) (Number(Degree.ofGPE x' [x])) = 1) M X
            match checkPairwiseRelativelyPrime M x && checkX with
            | false -> Number Undefined
            | true -> 
                let rec cRem n s i =
                    let x' = X.[i]
                    let m = M.[i]
                    let e = Euclidean.extAlgorithm n m x
                    let c = second e
                    let d = third e
                    let s' = Expand.algebraicExpression (c*n*x') + Expand.algebraicExpression (d*m*s)
                    let n' = Expand.algebraicExpression (n*m)
                    match i + 1 = M.Length with 
                    | true -> Divide.getRemainder s' n' x
                    | false -> cRem n' s' (i+1)
                cRem M.Head X.Head 1

        let normalize w L K =     
            let rec loop u L =
                match L with
                | [] -> u
                | x::xs -> loop (Coefficients.leadingGPE u x) xs
            let c = loop w L 
            match K with 
            | IntegralNumbers -> 
                match ExpressionType.compareExpressions c (Number(Integer 0I)) with
                | 1 -> w
                | -1 -> -w |> Expand.algebraicExpression
                | _ -> Number(Integer 0I)
            | RationalNumbers -> w/c |> Expand.algebraicExpression
            | _ -> Number Undefined
        
        let rec mVPolyGCD u v L K =
            let one = Number(Integer 1I)
            let zero = Number(Integer 0I)
            let content u x R = 
                match u = zero with
                | true -> zero
                | false -> 
                    let cList = 
                        Coefficients.listOfGPE u x 
                        |> List.choose (fun x -> 
                        match x <> zero with 
                        | true -> Some x 
                        | _ -> None)
                    normalize (List.reduce (fun acc x' -> mVPolyGCD acc x' R K) cList) L K
            match u,v with
            | a, b when a = zero -> normalize v L K
            | a, b when b = zero -> normalize u L K
            | _ -> normalize (
                    let rec mVGCD u v L =                    
                        match L with
                        | [] -> 
                            match K with
                            | IntegralNumbers -> NumberTheory.Integer.gcd u v
                            | RationalNumbers -> one
                            | _ -> Number Undefined
                        | x::R ->
                            let contU = content u x R
                            let contV = content v x R
                            let d = mVGCD contU contV R
                            let ppU = Divide.getRecQuotient u contU L K
                            let ppV = Divide.getRecQuotient v contV L K
                            let rec polyRemainderSeq  ppV' ppU' =
                                match ppV' = zero with
                                | true -> ppU'*d |> Expand.algebraicExpression
                                | false -> 
                                    let r = Divide.pseudoRemainder ppU' ppV' x
                                    match r = zero with
                                    | true -> polyRemainderSeq zero ppV'
                                    | false -> 
                                        let contR = content r x R
                                        let ppR = Divide.getRecQuotient r contR L K
                                        polyRemainderSeq ppR ppV'
                            polyRemainderSeq ppV ppU
                    mVGCD u v L
                   ) L K

        let rec SubResultantGCD u' v' (L:Expression list) K = //slower than mVPolyGCD
            let u, v = 
                match L with
                | [] -> 
                    match u', v' with
                    | Number n, _ -> v', u'
                    | _ -> u', v'
                | h::t -> 
                    match Degree.ofGPE u' [L.Head], Degree.ofGPE v' [L.Head] with
                    | a, b when ExpressionType.compareExpressions (Number a) (Number b) = -1 -> v', u'
                    | _ -> u',v'
            let one = Number(Integer 1I)
            let zero = Number(Integer 0I)
            let content u x R = 
                match u = zero with
                | true -> zero
                | false -> 
                    let cList = 
                        Coefficients.listOfGPE u x 
                        |> List.choose (fun x -> 
                        match x <> zero with 
                        | true -> Some x 
                        | _ -> None)
                    normalize (List.reduce (fun acc x' -> SubResultantGCD acc x' R K) cList) L K
            match u,v with
            | a, b when a = zero -> normalize v L K
            | a, b when b = zero -> normalize u L K
            | _ -> normalize (
                    let rec sRGCD u v L =                    
                        match L with
                        | [] -> 
                            match K with
                            | IntegralNumbers -> NumberTheory.Integer.gcd u v
                            | RationalNumbers -> one
                            | _ -> Number Undefined
                        | x::R ->
                            let contU = content u x R
                            let contV = content v x R
                            let d = sRGCD contU contV R
                            let U = Divide.getRecQuotient u contU L K
                            let V = Divide.getRecQuotient v contV L K
                            let g = SubResultantGCD (Coefficients.leadingGPE U x) (Coefficients.leadingGPE V x) R K
                            let rec polyRemainderSeq  V' U' i delta' psi' =
                                match V' = zero with
                                | true -> 
                                    let s = Divide.getRecQuotient (Coefficients.leadingGPE U' x) g R K
                                    let W = Divide.getRecQuotient U' s L K
                                    let contW = content W x R 
                                    let ppW = Divide.getRecQuotient W contW L K
                                    ppW*d |> Expand.algebraicExpression
                                | false -> 
                                    let r = Divide.pseudoRemainder U' V' x
                                    match r = zero, i with
                                    | true, _ -> polyRemainderSeq zero V' i delta' psi' 
                                    | false, i' when i' = one -> 
                                        let delta = Number(Degree.ofGPE U' [x]) - Number(Degree.ofGPE V' [x]) + one
                                        let psi = -one
                                        let beta = Expand.algebraicExpression ((-one)**delta)
                                        polyRemainderSeq (Divide.getRecQuotient r beta L K) V' (i + one) delta psi
                                    | false, i' when ExpressionType.compareExpressions i' one = 1 -> 
                                        let deltaP = delta'
                                        let delta = Number(Degree.ofGPE U' [x]) - Number(Degree.ofGPE V' [x]) + one
                                        let f = Coefficients.leadingGPE U' x
                                        let psi = 
                                            Divide.getRecQuotient 
                                                (Expand.algebraicExpression ((-f)**(deltaP - one))) 
                                                (Expand.algebraicExpression ((psi')**(deltaP - (Number(Integer 2I))))) 
                                                R K
                                        let beta = Expand.algebraicExpression ((-f)*psi**(delta - one))
                                        polyRemainderSeq (Divide.getRecQuotient r beta L K) V' (i + one) delta psi
                                    | _ -> Number Undefined
                            polyRemainderSeq V U one zero zero
                    sRGCD u v L
                   ) L K

//
    module Simplification =
        open Math.Foundations.Logic

        let monomialLCM u v L =
            match u = Number(Integer 0I) 
               || v = Number(Integer 0I) 
               || Check.isMonomialGPE u L = false 
               || Check.isMonomialGPE v L = false 
               with
            | true -> Number Undefined
            | false -> Divisors.normalize (u*v/(Divisors.mVPolyGCD u v L RationalNumbers) |> Expand.algebraicExpression) L RationalNumbers

        let S u v L = 
            let u',v' = Coefficients.leadingMonomial u L, Coefficients.leadingMonomial v L
            let d = monomialLCM u' v' L
            (d/u')*u - (d/v')*v |> Expand.algebraicExpression

        let GBasis (F:Expression list) L = 
            let G = F
            let P = List.concat [for i in 0..(G.Length-1) -> [for j in (i+1)..(G.Length-1) -> G.[i],G.[j]]]
            let rec loop (P:(Expression*Expression) list) G =
                match P.IsEmpty with
                | true -> G
                | false -> 
                    let t = P.Head
                    let P' = P.Tail
                    let s = S (fst t) (snd t) L
                    let r = Reduce.getRemainderRA s G L
                    match r = Number(Integer 0I) with
                    | true -> loop P' G
                    | false -> 
                        let P'' = List.concat [P'; [for i in 0..(G.Length-1) -> G.[i],r]]
                        let G' = List.concat [G;[r]]
                        loop P'' G'
            loop P G
                            
        let elimPoly G L =
            let rec loop (G':Expression list) i =
                match i = (G'.Length-1) with 
                | true -> G'
                | false ->
                    let gI = G'.[i]
                    let g = 
                        match i with
                        | 0 -> G'.Tail
                        | n when n > 0 && n < (G'.Length-1) -> List.concat [[for j in 0..(n-1) -> G'.[j]];[for k in (n+1)..(G'.Length-1) -> G'.[k]]]
                        | _ -> []
                    let r = Reduce.getRemainderRA gI g L
                    match r = Number(Integer 0I) with
                    | true -> loop g i
                    | false -> loop G' (i+1)
            loop G 0

        //Reduction of u with respect to F or the GBassis
        let withSideRelations u F L =
            let G = GBasis F L
            let H = elimPoly G L
            match H.Head, H.Length = 1 with
            | Number n, true -> Symbol Inconsistent
            | _ -> Reduce.getRemainderRA u H L

        let representBasis u F L =
            
            let rec permutations (A : 'a list) =
                match List.isEmpty A with 
                | true -> [[]]
                | false -> 
                    [for a in A do
                        yield! A |> List.filter (fun x -> x <> a) 
                                 |> permutations
                                 |> List.map (fun xs -> a::xs)]            
            
            let L' = permutations L
            let G = GBasis F L
            let H = elimPoly G L
                        
            let rec loop i g = 
                match i = L'.Length with
                | true -> g
                | false -> 
                    let g' = Reduce.getQuotientRA g F L'.[i]
                    match List.forall (fun x -> x = Number(Integer 0I)) g' with
                    | true -> loop (i+1) g
                    | false -> Reduce.getReductionResult g F L'.[i]

            let G' = List.mapi (fun i x -> loop i x) H

            match H.Head, H.Length = 1 with
            | Number n, true -> Symbol Inconsistent
            | _ -> 
                let c = Reduce.getQuotientRA u H L
                let r = Reduce.getRemainderRA u H L
                NaryOp(Sum,List.concat [(List.map2 (fun c f -> c*f) c G'); (match r = Number(Integer 0I) with |true -> [] |false -> [r])])
        
        
    [<RequireQualifiedAccess>]
    module RationalExpressionMV =

        let numerator' u L = 
            match u with
            | BinaryOp (x, ToThePowerOf, y) when Coefficients.signOfGPE y L < 0I -> Number (Integer 1I) //ND-2'
            | _ -> numerator u

        let denominator' u L = 
            match u with
            | BinaryOp (x, ToThePowerOf, y) when Coefficients.signOfGPE y L < 0I -> x**((Number (Integer -1I))*(Expand.algebraicExpression y)) //ND-2'
            | _ -> denominator u

        let simplify u L = 
            let K = RationalNumbers

            let u' = Expand.algebraicExpression (u)
    
            let nnn = RationalExpression.numerator u' |> RationalExpression.numerator
            let nnd = RationalExpression.numerator u' |> RationalExpression.denominator
            let ddn = RationalExpression.denominator u' |> RationalExpression.numerator
            let ddd = RationalExpression.denominator u' |> RationalExpression.denominator

            let nn = (Expand.algebraicExpression (nnn*ddd))
            let nn' = Divisors.normalize (Expand.algebraicExpression (nnn*ddd)) L K
            let dd = (Expand.algebraicExpression (ddn*nnd))
            let dd' = Divisors.normalize (Expand.algebraicExpression (ddn*nnd)) L K
            let sn = Divide.getRecQuotient nn nn' L K  
            let sd = Divide.getRecQuotient dd dd' L K
            let c = sn/sd
    
            let gcd = Divisors.mVPolyGCD nn' dd' L K

            let n = Divide.getRecQuotient nn' gcd L K
            let d = Divide.getRecQuotient dd' gcd L K

            c*(n/d)      

    [<RequireQualifiedAccess>]
    module Resultant =
        ///Uses the determinat defenition --Fastest--
        let viaDeterminant u v x =    
            let uL = Coefficients.listOfGPE u x
            let vL = Coefficients.listOfGPE v x
            let m = match Degree.ofGPE u [x] with | Integer i -> i | _ -> 0I
            let n = match Degree.ofGPE v [x] with | Integer i -> i | _ -> 0I
            let rows = n + m
    
            let drop i L =
                let rec drop' i l1 p l2 =
                    match List.length l2 = List.length l1 - 1 with
                    | true -> l2
                    | false ->  
                        let p = match p = i with | true-> p+1  | false -> p 
                        drop' i l1 (p+1) (List.append l2 [(List.item p l1)])
                drop' i L 0 []

            let addZero l t (L:Expression list) = 
                match l, t with
                | z, n when z = 0I -> List.concat[List.rev L;[for i in 1I..n -> Number(Integer 0I)]]
                | m, z when z = 0I -> List.concat[[for i in 1I..m -> Number(Integer 0I)]; List.rev L]
                | m, n -> List.concat[[for i in 1I..m -> Number(Integer 0I)]; List.rev L;[for i in 1I..n -> Number(Integer 0I)]]
    
            let sylT = 
                List.concat [
                    [for i in 0I..(n-1I) -> 
                        let n' = (rows-bigint(uL.Length)) - i 
                        addZero i n' uL];
                    [for i in 0I..(m-1I) -> 
                        let m' = (rows-bigint(vL.Length)) - i 
                        addZero i m' vL]]
    
            let rec det (m:Expression list list) = 
                match m.Length with
                | 2 -> m.[0].[0] * m.[1].[1] - m.[0].[1] * m.[1].[0]
                | j when j > 2 -> 
                    let LHead = m.Head
                    let LTail = m.Tail
                    let out = List.mapi (fun indx x -> 
                        let LOut = List.map (fun x -> drop indx x) LTail
                        match indx%2 = 0 with
                        | true -> LHead.[indx]*(det LOut)
                        | false -> -LHead.[indx]*(det LOut)) LHead
                    List.fold (fun acc x -> acc + x) (Number (Integer 0I)) out
                | _ -> Number(Integer 0I)

            det sylT |> Expand.algebraicExpression

        let rec viaEuclidAlgorithmSV u v x =    
            let m = Degree.ofGPE u [x] 
            let n = Degree.ofGPE v [x] 
            match n = Integer 0I with
            | true -> v**(Number m)
            | false -> 
                let r = Divide.getRemainder u v x
                match r = Number(Integer 0I) with
                | true -> Number(Integer 0I)
                | false -> 
                    let s = Degree.ofGPE r [x]
                    let l = Coefficients.ofGPE v x (n)
                    ((Number(Integer -1I))**(Number m * Number n)) * 
                    (l**(Number m - Number s)) * 
                    viaEuclidAlgorithmSV v r x 
                    |> Expand.algebraicExpression

        let rec viaEuclidAlgorithmMV u v (L : Expression list) K =
            let x = L.Head
            let m = Degree.ofGPE u [x] 
            let n = Degree.ofGPE v [x]
            match Expression.compareExpressions (Number m) (Number n) = -1 with
            | true -> ((Number(Integer -1I))**(Number m * Number n)) * 
                      viaEuclidAlgorithmMV v u L K
            | false -> 
                match n = Integer 0I with
                | true -> v**(Number m)
                | false -> 
                    let sigma = Number m - Number n + Number(Integer 1I)
                    let r = Divide.pseudoRemainder u v x
                    match r = Number(Integer 0I) with
                    | true -> Number(Integer 0I)
                    | false -> 
                        let s = Degree.ofGPE r [x]
                        let w = ((Number(Integer -1I))**(Number m * Number n)) * 
                                viaEuclidAlgorithmMV v r L K
                                |> Expand.algebraicExpression
                        let l = Coefficients.leadingGPE v x 
                        let k = sigma * (Number n) - (Number m) + (Number s)
                        let f = l**k |> Expand.algebraicExpression
                        Divide.getRecQuotient w f L K

        let viaSubResultant u v (L : Expression list) K =
            let zero = Number(Integer 0I)
            let x = L.Head
            let R = L.Tail
            let m = Degree.ofGPE u [x] 
            let n = Degree.ofGPE v [x]
            
            let content u x R = 
                match u = zero with
                | true -> zero
                | false -> 
                    let cList = 
                        Coefficients.listOfGPE u x 
                        |> List.choose (fun x -> 
                        match x <> zero with 
                        | true -> Some x 
                        | _ -> None)
                    Divisors.normalize (List.reduce (fun acc x' -> Divisors.SubResultantGCD acc x' R K) cList) L K
            
            let contU = content u x R
            let ppU = Divide.getRecQuotient u contU L K
            let contV = content v x R
            let ppV = Divide.getRecQuotient v contV L K
            
            let rec SubResultant u v (L : Expression list) K i sigP psiP =
                let x = L.Head
                let m = Degree.ofGPE u [x] 
                let n = Degree.ofGPE v [x]
                match Expression.compareExpressions (Number m) (Number n) = -1 with
                | true -> ((Number(Integer -1I))**(Number m * Number n)) * 
                          SubResultant u v L K i sigP psiP
                | false -> 
                    match n = Integer 0I with
                    | true -> v**(Number m)
                    | false -> 
                        let r = Divide.pseudoRemainder u v x
                        match r = zero with
                        | true -> zero
                        | false -> 
                            let sigma = Number m - Number n + Number(Integer 1I)
                            let R = L.Tail                            
                            let psi = 
                                match i = 1 with 
                                | true -> Number(Integer -1I) 
                                | false -> 
                                    let f = Coefficients.leadingGPE u x
                                    Divide.getRecQuotient ((-f)**(sigP - Number(Integer 1I)) |> Expand.algebraicExpression) ((psiP)**(sigP - Number(Integer 2I)) |> Expand.algebraicExpression) R K
                            let beta = 
                                match i = 1 with 
                                | true -> (Number(Integer -1I))**sigma
                                | false -> 
                                    let f = Coefficients.leadingGPE u x
                                    (-f)*(psi**(sigma - Number(Integer 1I))) |> Expand.algebraicExpression
                            let r' = Divide.getRecQuotient r beta L K
                            let w = Number(Integer -1I) * (beta**(Number n)) * SubResultant v r' L K (i+1) sigma psi |> Expand.algebraicExpression
                            let l = Coefficients.ofGPE v x n
                            let s = Degree.ofGPE r [x]
                            let k = Number n * sigma - Number m + Number s
                            let f'' = Expand.algebraicExpression l**k
                            Divide.getRecQuotient w f'' L K
            
            let s = SubResultant ppU ppV L K 1 zero zero
            contU**(Number n) * contV**(Number m) * s

//
    [<RequireQualifiedAccess>]
    module AlgebraicNumberField =
 
        let add u v = u + v

        let addativeInverse u = Expand.algebraicExpression (-u)
 
        let multiply u v p a = Divide.getRemainder (Expand.algebraicExpression (u*v)) p a

        let multiplicativeInverse u p a = match Euclidean.extAlgorithm u p a with | (a,b,c) -> b

        let divide u v p a = 
            match v <> Number(Integer 0I) with
            | false -> Number Undefined
            | true -> 
                let v' = multiplicativeInverse v p a
                multiply u v' p a


        let coefficientSimplify r x p a = snd (Divide.algebraicExpressions r p a)

        let generalSimplify u p a =     
            match Check.isGExpression a u with 
            | true ->        
                let eNumber (n:Expression) = n
                let eComplexNumber (a,b) = ComplexNumber (a,b)
                let eSymbol (v:Expression) = v
                let eBinaryOp (a,op,b) = 
                    match b = Number(Integer -1I) with
                    | true -> BinaryOp (a,op,b)
                    | false -> 
                        let n, d = RationalExpression.numerator (BinaryOp (a,op,b)), RationalExpression.denominator (BinaryOp (a,op,b))
                        divide n d p a
                let eUnaryOp (op,a) = UnaryOp (op,a)
                let eNaryOp (op,aList) = 
                    let test = List.forall (fun a' ->
                            (match a' with 
                             | BinaryOp (a,op,b) when b = Number(Integer -1I) -> false                    
                             | _ -> true) = true) aList                 
                    match op with 
                    | Product -> 
                        let n, d = RationalExpression.numerator (NaryOp (op,aList)), RationalExpression.denominator (NaryOp (op,aList))
                        divide n d p a
                    | _ -> NaryOp (op,aList)
                let gen = fun a -> ExpressionType.simplifyExpression a
                Cata.foldbackExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp gen u
            | false -> Number Undefined

        let polynomialDivide u v x p a = 
            let lcv = Coefficients.leadingGPE v x
            let n = Degree.ofGPE v [x]
            let rec loop q r m = 
                let lcr = Coefficients.leadingGPE r x
                let s = divide lcr lcv p a
                let q' = q + (s*x**(Number (m + -n)))
                let r' = coefficientSimplify (Expand.algebraicExpression ((r - lcr*x**Number m) - (v - lcv*x**Number n) * s * x**(Number(m + -n)))) x p a
                let m' = Degree.ofGPE r' [x]
                match Number.compare m n = -1 with 
                | true -> (q,r)
                | false -> loop q' r' m'
            loop (Number (Integer 0I)) u (Degree.ofGPE u [x])

        let getQuotient u v x p a = fst (polynomialDivide u v x p a)

        let getRemainder u v x p a = snd (polynomialDivide u v x p a)

        let makeMonic u x p a = 
            //let p = ExpressionStructure.substitute (x,a) p'
            divide u (Coefficients.leadingGPE u x) p a

        let polynomialGCD u v x p a =
            //let p = ExpressionStructure.substitute (x,a) p'
            let rec loop u' v' =
                match v' <> Number(Integer 0I) with
                | false -> makeMonic u' x p a
                | true -> loop v' (getRemainder u' v' x p a )
            loop u v

        let rec expansion u v x t p a = 
            match Check.isPolynomialGPE u [x] && Check.isPolynomialGPE v [x] with
            | true when u = Number(Integer 0I) && Number.compare (Degree.ofGPE v [x]) Number.Zero = 1 -> Number(Integer 0I)
            | true when Number.compare (Degree.ofGPE v [x]) Number.Zero = 1 -> 
                let q = getQuotient u v x p a
                let r = getRemainder u v x p a 
                makeMonic (Expand.algebraicExpression (t * (expansion q v x t p a) + r)) x p a
            | _ -> Number Undefined

        let extEuclideanAlgorithm u v x p a = 
            let zero = Number (Integer 0I)
            let one = Number (Integer 1I)
            match u = zero && v = zero with
            | true -> (zero,zero,zero)
            | false -> 
                let rec loop u' v' app ap bpp bp =
                    let c = Coefficients.leadingGPE u' x
                    match v' = zero with
                    | false -> 
                        let q = getQuotient u' v' x p a
                        let r = getRemainder u' v' x p a
                        let ap' = (app - (multiply q ap p a))
                        let bp' = (bpp - (multiply q bp p a))
                        loop v' r ap ap' bp bp'
                    | true -> (divide (Expand.algebraicExpression (u')) c p a, divide (Expand.algebraicExpression (app)) c p a, divide (Expand.algebraicExpression (bpp)) c p a)
                loop u v one zero zero one

        /// Input: a - an explicit algebraic number, x - a symbol
        /// Output: a polynomial in Q[x]
        let rec findPolynomial a x =
            let y = Symbol(Variable "globalY")
            match a with 
            | Number (Integer i) -> x - a
            | Number (Rational r) -> x - a
            | BinaryOp(b,ToThePowerOf,e) -> 
                let n = RationalExpression.numerator e
                let d = RationalExpression.denominator e
                let p = ExpressionStructure.substitute (x,y) (findPolynomial b x) 
                match ExpressionType.compareExpressions e (Number(Integer 0I)) with
                | 1 -> Resultant.viaDeterminant p (x**d - y**n) y
                | _ -> 
                    let w = Resultant.viaDeterminant p (x**d - y**(ExpressionFunction.abs n)) y
                            |> ExpressionStructure.substitute (x,y)
                    Resultant.viaDeterminant (x*y - Number(Integer 1I)) w y
            | BinaryOp(b,ExplicitToThePowerOf,e) -> 
                let n = RationalExpression.numerator e
                let d = RationalExpression.denominator e
                let p = ExpressionStructure.substitute (x,y) (findPolynomial b x) 
                match ExpressionType.compareExpressions e (Number(Integer 0I)) with
                | 1 -> Resultant.viaDeterminant p (x**d - y**n) y
                | _ -> 
                    let w = Resultant.viaDeterminant p (x**d - y**(ExpressionFunction.abs n)) y
                            |> ExpressionStructure.substitute (x,y)
                    Resultant.viaDeterminant (x*y - Number(Integer 1I)) w y
            | NaryOp(Sum,xList) -> 
                let w = findPolynomial xList.Head x
                List.fold (fun w' x' -> 
                    let p = findPolynomial x' x
                    let q = ExpressionStructure.substitute (x,x-y) p 
                            |> Expand.algebraicExpression
                    Resultant.viaDeterminant q (ExpressionStructure.substitute (x,y) w') y
                    ) w xList.Tail
            | NaryOp(Product,xList) -> 
                let w = findPolynomial xList.Head x
                List.fold (fun w' x' -> 
                    let p = findPolynomial x' x
                    let m = Degree.ofGPE p [x]
                    let q = (y**Number m)*(ExpressionStructure.substitute (x,x/y) p) 
                            |> Expand.algebraicExpression
                    Resultant.viaDeterminant q (ExpressionStructure.substitute (x,y) w') y
                    ) w xList.Tail
            | _ -> Number Undefined
//    

    [<RequireQualifiedAccess>]
    module Decomposition =

        let ofPolynomial2 u x = 
            let u0 = Coefficients.ofGPE u x (Integer 0I)
            let U = u - u0
            let S = Divisors.ofPolynomial U x
            let globalT = Symbol(Variable "globalT")
            let out = 
                List.tryPick (fun w ->
                    match 
                        Degree.ofGPE w [x] <> Degree.ofGPE u [x] &&
                        Expression.compareExpressions (Number (Degree.ofGPE w [x])) (Number (Integer 1I)) = 1 &&
                        NumberTheory.Integer.remainder (Number(Degree.ofGPE u [x])) (Number(Degree.ofGPE w [x])) = Number(Integer 0I) 
                        with
                    | false -> None
                    | true -> 
                        let v = Expansion.ofGPE u w x globalT
                        match freeOf v x with
                        | false -> None
                        | true -> Some ((ExpressionStructure.substitute (globalT,x) v),w)
                    ) S
            match out with
            | Some e -> e
            | None -> (x,u)

        let rec ofPolynomialComplete u x = 
            let compare a b = ExpressionType.compareExpressions (Number (Degree.ofGPE a [x])) (Number (Degree.ofGPE b [x]))
            let u0 = Coefficients.ofGPE u x (Integer 0I)
            let U = u - u0
            let S = List.sortWith compare (Divisors.ofPolynomial U x)
            let decomposition = []
            let globalT = Symbol(Variable "globalT")
            let C = x            
            let rec out s c decomp finalComponent = 
                match s with
                | [] -> 
                    match decomp with
                    | [] -> [u]
                    | _ -> List.concat [(ofPolynomialComplete(ExpressionStructure.substitute (globalT,x) finalComponent)x); decomp]
                | _ ->
                    let w = s.Head
                    let s' = s.Tail
                    match 
                        compare c w = -1 &&
                        compare w u = -1 &&
                        NumberTheory.Integer.remainder (Number(Degree.ofGPE u [x])) (Number(Degree.ofGPE w [x])) = Number(Integer 0I) 
                        with
                        | false -> out s' c decomp finalComponent
                        | true -> 
                            let g = Expansion.ofGPE w c x globalT
                            let R = Expansion.ofGPE u w x globalT
                            match freeOf g x && freeOf R x with
                            | false -> out s' c decomp finalComponent
                            | true -> 
                                let decomp' = (ExpressionStructure.substitute (globalT,x) g)::decomp
                                out s' w decomp' R
            out S C decomposition u

//Operators related to the form of a polynomial
    module Form =
        open ExpressionStructure

        let rationalSimplify u x = 
            let u' = RationalExpression.transform u
            let n, d = Expand.algebraicExpression (RationalExpression.numerator u'), Expand.algebraicExpression (RationalExpression.denominator u')
            let gcd = Divisors.gcd n d x
            let n', d' = fst (Divide.algebraicExpressions n gcd x), fst (Divide.algebraicExpressions d gcd x)
            let coeffN, coeffD = (Coefficients.leadingGPE n' x), (Coefficients.leadingGPE d' x)
            (Expand.algebraicExpression(n'/coeffN))/(Expand.algebraicExpression(d'/coeffD))*(coeffN/coeffD)

        let rec linear u x =
            match u = x with
            | true -> Pass (Number(Integer 1I),Number(Integer 0I))
            | false -> 
                match u with
                | Symbol s -> Pass (Number(Integer 0I),u)
                | Number n -> Pass (Number(Integer 0I),u)
                | NaryOp(Product, xList) -> 
                    match freeOf u x with
                    | false -> 
                        match freeOf (u/x) x with
                        | true -> Pass ((u/x), Number(Integer 0I))
                        | false -> Fail (Symbol(Error OtherError),(Symbol(Error OtherError)))
                    | true -> Pass (Number(Integer 0I),u)
                | NaryOp(Sum, xList) ->
                    let f = linear xList.Head x
                    match f with 
                    | Fail e -> Fail e
                    | Pass f' -> let r = linear (u-(xList.Head)) x
                                 match r with 
                                 | Fail e -> Fail e
                                 | Pass r' -> Pass (fst f' + fst r', snd f' + snd r')
                | _ -> match freeOf u x with
                       | true -> Pass (Number(Integer 0I),u)
                       | false -> Fail (Symbol(Error OtherError),(Symbol(Error OtherError)))
     
        ///Returns true when an algebraic expression u has the form ax+by+c, where x and y are symbols and a, b, and c are free of x and y
        let isBilinear u x y =
            let freeOfXY aList = List.choose (fun a -> 
                match ExpressionStructure.freeOf a x && ExpressionStructure.freeOf a y with
                | false -> None
                | true -> Some a ) aList        
            match x, y with
            | Symbol (Variable a), Symbol (Variable b) -> 
                match Degree.ofGPE u [x] = Integer 1I && Degree.ofGPE u [y] = Integer 1I with
                | true -> 
                    match u with 
                    | NaryOp(Sum,aList) -> 
                        let eList = List.concat [(freeOfXY aList); [Coefficients.ofGPE u x Number.One]; [Coefficients.ofGPE u y Number.One]]
                        let checkX = List.forall (fun a -> ExpressionStructure.freeOf a x) eList
                        let checkY = List.forall (fun a -> ExpressionStructure.freeOf a y) eList
                        checkX && checkY                 
                    | _ -> false
                | false -> false
            | _ -> false

        let GMEtoMultivariate u =
            let freeOfSort s =  ExpressionStructure.freeOfSort s
            let tList = Variables.ofExpression u |> freeOfSort
            let rList = [for i in 1 .. tList.Length  -> Symbol (Variable("x" + i.ToString()))]
            let yList = List.zip tList rList
            substituteSequential yList u

        
            