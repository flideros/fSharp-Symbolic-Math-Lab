namespace Math.Pure.Structure
open Math.Pure.Objects
open Math.Pure.Quantity

type Expression =
    | Number of NumberType
    | ComplexNumber of Expression*Expression //still working on this. Might remove and create a module...
    | Symbol of Symbol
    | BinaryOp of Expression * Function * Expression
    | UnaryOp of Function * Expression
    | NaryOp of Function * (Expression list)
    

module Cata = 
    
    ///bottom-up recursion
    let rec recurseExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp exp : 'r =
        let recurse = recurseExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp
        match exp with 
        | Number n -> eNumber (Number n)
        | ComplexNumber (r,i) -> eComplexNumber ((recurse r), (recurse i))
        | Symbol v -> eSymbol (Symbol v)
        | BinaryOp (a,op,b) -> eBinaryOp (recurse a,op,recurse b)
        | UnaryOp (op,a) -> eUnaryOp (op,recurse a)
        | NaryOp (op,aList) -> eNaryOp (op,(List.map recurse aList))

    ///top-down iteration
    let rec foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc exp : 'r =
        let recurse = foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp
        match exp with 
        | Number n -> 
            let finalAcc = eNumber acc (Number n)
            finalAcc
        | ComplexNumber (r,i) -> 
            let newAcc = eComplexNumber acc (ComplexNumber (r,i))
            [r;i] |> List.fold recurse newAcc
        | Symbol v -> 
            let finalAcc = eSymbol acc (Symbol v)
            finalAcc
        | BinaryOp (a,op,b) ->                        
            let newAcc = eBinaryOp acc (BinaryOp (a,op,b))
            [a;b] |> List.fold recurse newAcc
        | UnaryOp (op,a) -> 
            let newAcc = eUnaryOp acc (UnaryOp (op,a))
            recurse newAcc a
        | NaryOp (op,aList) -> 
            let newAcc = eNaryOp acc (NaryOp (op,aList))
            aList |> List.fold recurse newAcc  

    ///Bottom-up iteration
    let rec foldbackExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp generator exp :'r =
        let recurse = foldbackExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp 
        match exp with 
        | Number n -> generator (eNumber (Number n))
        | ComplexNumber (r,i) -> generator (eComplexNumber (r,i))           
        | Symbol v -> generator (eSymbol (Symbol v))
        | BinaryOp (a,op,b) -> generator (eBinaryOp (recurse generator a,op,recurse generator b))
        | UnaryOp (op,a) ->  generator (eUnaryOp (op,recurse generator a))
        | NaryOp (op,aList) -> generator (eNaryOp(op,List.map (fun x -> recurse generator x) aList))

module ExpressionType =

//Property-based Operators
    let isNumber x = 
        match x with 
        | Number n -> true
        | _ -> false

    let isNegativeNumber x =
        match x with 
        | Number n when n.isNegative -> true
        | _ -> false

    let Base x = 
        match x with
        | Number n -> Number Undefined
        | BinaryOp (a,ToThePowerOf,b) -> a
        | _ -> x

    let Exponent x =
        match x with
        | Number n -> Number Undefined
        | BinaryOp (a,ToThePowerOf,b) -> b
        | _ -> Number Number.One

    let Term x =
        match x with
        | Number n -> Number Undefined
        | NaryOp(Product,p) when isNumber p.[0] -> 
            match p.Length with
            | 1 -> Number Undefined
            | 2 -> p.[1]
            | _ -> NaryOp(Product,p.Tail)
        | NaryOp(Product,x) when isNumber x.[0] = false -> NaryOp(Product,x)
        | a -> a

    let Const x =
        match x with
        | Number n -> Number Undefined
        | NaryOp(op,a) when isNumber a.[0] -> a.[0]
        | NaryOp(op,a) when isNumber a.[0] = false -> Number Number.One
        | _ -> Number Number.One

//Comparison
    let rec compareExpressions u v =
        match u, v with
        | Number x, Number y -> Number.compare x y //O-1
        | Symbol x, Symbol y when x > y -> 1 //O-2
        | Symbol x, Symbol y when x < y -> -1 //O-2
        | Symbol x, Symbol y when x = y -> 0 //O-2
        | NaryOp(op1, x), NaryOp(op2, y) when //O-3.1 & O-6.2.(a)
            op1 = op2 && 
            (List.rev x).Head <> (List.rev y).Head ->
            compareExpressions ((List.rev x).Head) ((List.rev y).Head) 
        | NaryOp(op1, x), NaryOp(op2, y) when //O-3 & O-6.2
            op1 = op2 && 
            (List.rev x).Head = (List.rev y).Head ->
            match x.Tail.IsEmpty , y.Tail.IsEmpty with
            | false, false -> compareExpressions (NaryOp(op1, (List.rev((List.rev x).Tail)))) (NaryOp(op1, (List.rev((List.rev y).Tail)))) //O-3.2 & O-6.2.(b)
            | true, false -> 1 //O-3.3 & O-6.2.(c)
            | false, true -> -1 //O-3.3 & O-6.2.(c)
            | true, true -> 0        
        | BinaryOp(x1, op1, y1), BinaryOp(x2, op2, y2) when op1 = op2 && x1 <> x2 -> compareExpressions x1 x2 //O-4.1
        | BinaryOp(x1, op1, y1), BinaryOp(x2, op2, y2) when op1 = op2 && x1 = x2 -> compareExpressions y1 y2 //O-4.2        
        | UnaryOp(op1, x), UnaryOp(op2, y) when op1 = op2 -> compareExpressions x y //O-5
        | BinaryOp(x1, op1, y1), BinaryOp(x2, op2, y2) when op1 < op2 -> -1 //O-6.1
        | BinaryOp(x1, op1, y1), BinaryOp(x2, op2, y2) when op1 > op2 -> 1 //O-6.1
        | BinaryOp(x1, op1, y1), NaryOp(op2, y) when op1 < op2 -> -1 //O-6.1
        | BinaryOp(x1, op1, y1), NaryOp(op2, y) when op1 > op2 -> 1 //O-6.1
        | BinaryOp(x1, op1, y1), UnaryOp(op2, y) when op1 < op2 -> -1 //O-6.1
        | BinaryOp(x1, op1, y1), UnaryOp(op2, y) when op1 > op2 -> 1 //O-6.1
        | NaryOp(op1, x), NaryOp(op2, y) when op1 < op2 -> -1 //O-6.1
        | NaryOp(op1, x), NaryOp(op2, y) when op1 > op2 -> 1 //O-6.1
        | NaryOp(op1, x), BinaryOp(x2, op2, y2) when op1 < op2 -> -1 //O-6.1
        | NaryOp(op1, x), BinaryOp(x2, op2, y2) when op1 > op2 -> 1 //O-6.1
        | NaryOp(op1, x), UnaryOp(op2, y) when op1 < op2 -> -1 //O-6.1
        | NaryOp(op1, x), UnaryOp(op2, y) when op1 > op2 -> 1 //O-6.1        
        | UnaryOp(op1, x), UnaryOp(op2, y) when op1 > op2 -> -1 //O-6.1
        | UnaryOp(op1, x), UnaryOp(op2, y) when op1 < op2 -> 1 //O-6.1
        | UnaryOp(op1, x), BinaryOp(x2, op2, y2) when op1 > op2 -> -1 //O-6.1
        | UnaryOp(op1, x), BinaryOp(x2, op2, y2) when op1 < op2 -> 1 //O-6.1
        | UnaryOp(op1, x), NaryOp(op2, y) when op1 > op2 -> -1 //O-6.1
        | UnaryOp(op1, x), NaryOp(op2, y) when op1 < op2 -> 1 //O-6.1        
        | _, Number _ -> 1 //O-7
        | Number _, _ -> -1 //O-7 
        | NaryOp(Product, x), y -> compareExpressions (NaryOp(Product, x)) (NaryOp(Product, [y])) //O-8        
        | BinaryOp(base', ToThePowerOf, power'), b when base' <> Base b -> compareExpressions base' (Base b) //O-9
        | BinaryOp(base', ToThePowerOf, power'), b when base' = Base b -> compareExpressions power' (Exponent b) //O-9        
        | NaryOp(Sum, s), b -> compareExpressions (NaryOp(Sum, s)) (NaryOp(Sum, [b])) //O-10        
        | UnaryOp(Factorial, x), b when x = b -> -1 //O-11.1
        | UnaryOp(Factorial, x), b when x <> b -> compareExpressions (UnaryOp(Factorial, x)) (UnaryOp(Factorial, b)) //O-11.2        
        | NaryOp(op, x), Symbol v when x = [Symbol v] -> -1 //O-12.1
        | NaryOp(op, x), Symbol v when x <> [Symbol v] -> compareExpressions (NaryOp(op, x)) (NaryOp(op, [Symbol v])) //O-12.2
        | BinaryOp(x,op, y), Symbol v when x = Symbol v -> -1 //O-12.1
        | BinaryOp(x,op, y), Symbol v when x <> Symbol v -> compareExpressions (BinaryOp(x,op, y)) (BinaryOp(Symbol v,op, y)) //O-12.2
        | UnaryOp(op, x), Symbol v when x = Symbol v -> -1 //O-12.1
        | UnaryOp(op, x), Symbol v when x <> Symbol v -> compareExpressions (UnaryOp(op, x)) (UnaryOp(op, Symbol v)) //O-12.2        
        | _ -> -1 * (compareExpressions v u) //O-13
 
// Simplification Operators
    let rec simplifyPower x =
        let rec simplifyIntegerPower x =        
            match x with
            | BinaryOp(Number base',ToThePowerOf,Number(Integer i)) when base' <> Number.Zero -> Number (base'**(Integer i)) //SINTPOW-1
            | BinaryOp(base',ToThePowerOf,Number(Integer i)) when base' <> Number Number.Zero && i = 0I -> Number (Integer 1I) //SINTPOW-2
            | BinaryOp(base',ToThePowerOf,Number(Integer i)) when base' <> Number Number.Zero && i = 1I-> base' //SINTPOW-3
            | BinaryOp((BinaryOp(base',ToThePowerOf,power')),ToThePowerOf,Number(Integer i)) -> //SINTPOW-4
                 let p =  simplifyProduct (NaryOp(Product,[power'; Number(Integer i)])) 
                 match p with 
                 | Number (Integer ii) -> simplifyIntegerPower (BinaryOp(base',ToThePowerOf,p))
                 | _ -> (BinaryOp(base',ToThePowerOf,p)) 
            | BinaryOp((NaryOp(Product,eList)),ToThePowerOf,Number(Integer i)) -> //SINTPOW-5
                let eList' = List.map (fun x -> simplifyIntegerPower (BinaryOp(x,ToThePowerOf,Number(Integer i)))) eList
                simplifyProduct (NaryOp(Product,eList'))
            | _ -> x //SINTPOW-6        
        
        
        match x with
        | BinaryOp(base',ToThePowerOf,power') when base' = Number Undefined || power' = Number Undefined -> Number Undefined //SPOW-1
        | BinaryOp(base',ToThePowerOf,Number n) when base' = Number Number.Zero && n.isNegative = false -> Number Number.Zero //SPOW-2
        | BinaryOp(base',ToThePowerOf,_) when base' = Number Number.Zero -> Number Undefined //SPOW-2
        | BinaryOp(base',ToThePowerOf,_) when base' = Number Number.One -> Number Number.One //SPOW-3
        | BinaryOp(_,ToThePowerOf,Number (Integer n)) -> simplifyIntegerPower x //SPOW-4        
        | _ -> x //SPOW-5        
 
    and simplifyProduct p =      
        let rec simplifyProductRec p =
            match p with
            | [BinaryOp(Number (Integer i1),ToThePowerOf,Number n1);BinaryOp(Number (Integer i2),ToThePowerOf,Number n2)] 
                when n1=n2 -> [BinaryOp(Number (Integer (i1*i2)),ToThePowerOf,Number n1)]// TEST POWER OF INTEGERS
            | [NaryOp(Product, x); NaryOp(Product, y)] -> mergeProducts x y //SPRDREC-2.1
            | [NaryOp(Product, x); a] -> mergeProducts x [a] //SPRDREC-2.2
            | [a; NaryOp(Product, x)] -> mergeProducts [a] x //SPRDREC-2.3
            | [a; b] ->
                match a, b with            
                | Number a, Number b -> //SPRDREC-1
                    let n = Number (a * b)
                    match n with
                    | Number x when x = Number.One -> [] //SPRDREC-1.1
                    | _ -> [n] //SPRDREC-1.1
                | Number a, b when a = Number.One -> [b] //SPRDREC-1.2.a
                | a, Number b when b = Number.One -> [a] //SPRDREC-1.2.b
                | a, b when Base b = Base a -> //SPRDREC-1.3
                    let s = simplifySum (NaryOp(Sum,[Exponent a; Exponent b]))
                    let p = simplifyPower (BinaryOp(Base a, ToThePowerOf, s))
                    match p with
                    | Number n when n = Number.One -> [] //SPRDREC-1.3
                    | _ -> [p] //SPRDREC-1.3
                | a, b when compareExpressions a b = 1 -> [b; a] //SPRDREC-1.4
                | _ -> [a; b] //SPRDREC-1.5
            | l when List.length l > 2 -> //SPRDREC-3
                let w = simplifyProductRec l.Tail
                match l.Head with
                | NaryOp(Product, x) -> mergeProducts x w //SPRDREC-3.1
                | _ -> mergeProducts [l.Head] w //SPRDREC-3.2
            | _ -> p        
        and mergeProducts a b = 
            let sort = List.sortWith (fun x -> compareExpressions x)
            let a' = sort a
            let b' = sort b
            match a', b' with
            | x, [] -> x //MPRD-1
            | [], y -> y //MPRD-2
            | x, y -> //MPRD-3
                let h = simplifyProductRec [x.Head; y.Head]
                match h with
                | [] -> mergeProducts x.Tail y.Tail //MPRD-3.1
                | [h'] -> h'::(mergeProducts x.Tail y.Tail) //MPRD-3.2
                | [a; b] when compareExpressions x.Head y.Head = -1 -> x.Head::(mergeProducts x.Tail y) //MPRD-3.3
                | _ -> y.Head::(mergeProducts x y.Tail) //MPRD-3.4
        match p with
        | NaryOp(Product,x) when List.exists (fun elem -> elem = Number Undefined) x || x.IsEmpty -> Number Undefined //SPRD-1
        | NaryOp(Product,x) when List.exists (fun elem -> elem = Number Number.Zero) x -> Number Number.Zero //SPRD-2
        | NaryOp(Product,x) when List.length x = 1 -> x.[0] //SPRD-3
        | NaryOp(Product,x) -> //SPRD-4
            let x' : Expression list = simplifyProductRec x
            match x' with
            | []-> Number Number.One //SPRD-4.3
            | [x1] -> x1 //SPRD-4.1
            | _ -> NaryOp(Product,x') //SPRD-4.2
        | _ -> Number Undefined
    
    and simplifySum p = 
        let rec simplifySumRec s =
            match s with
            | [NaryOp(Sum, x); NaryOp(Sum, y)] -> mergeSums x y |> simplifySumRec
            | [NaryOp(Sum, x); a] -> mergeSums x [a] |> simplifySumRec
            | [a; NaryOp(Sum, x)] -> mergeSums [a] x |> simplifySumRec
            | [a'; b'] ->
                match a', b' with            
                | Number a, Number b -> 
                    let n = Number (a + b)
                    match n with
                    | Number x when x = Number.Zero -> [] 
                    | _ -> [n] 
                | Number a, b when a = Number.Zero -> [b] 
                | a, Number b when b = Number.Zero -> [a] 
                | a, b when Term a = Term b -> [(simplifyProduct (NaryOp(Product,[(simplifySum (NaryOp(Sum, [(Const a); (Const b)]))); Term a])))]
                | a, b when compareExpressions a b = 1 -> [b; a] 
                | _ -> [a'; b'] 
            | l when List.length l > 2 -> 
                let w = simplifySumRec l.Tail
                match l.Head with
                | NaryOp(Sum, x) -> mergeSums x w 
                | _ -> mergeSums [l.Head] w 
            | _ -> s
        and mergeSums a b = 
            let sort = List.sortWith (fun x -> compareExpressions x)
            let a' = sort a
            let b' = sort b
            match a', b' with
            | x, [] -> x //MPRD-1
            | [], y -> y //MPRD-2
            | x, y -> //MPRD-3
                let h = simplifySumRec [x.Head; y.Head]
                match h with
                | [] -> mergeSums x.Tail y.Tail //MPRD-3.1
                | [h'] -> h'::(mergeSums x.Tail y.Tail) //MPRD-3.2
                | [a; b] when compareExpressions x.Head y.Head = -1 -> x.Head::(mergeSums x.Tail y) //MPRD-3.3
                | _ -> y.Head::(mergeSums x y.Tail) //MPRD-3.4
        match p with
        | NaryOp(Sum,x) when List.exists (fun elem -> elem = Number Undefined) x || x.IsEmpty -> Number Undefined     
        | NaryOp(Sum,x) when List.length x = 1 -> x.[0] 
        | NaryOp(Sum,x) ->
            let x' : Expression list = simplifySumRec x
            match x' with
            | []-> Number Number.Zero 
            | [x1] -> x1 
            | _ -> NaryOp(Sum,x') 
        | _ -> Number Undefined

    //Simplify function form quotient (use inline operator '/' for simplfication of all other expressions)
    let simplifyQuotient u =
        match u with 
        | BinaryOp(a,DividedBy,b) -> BinaryOp(a,Times,BinaryOp(b,ToThePowerOf,Number(Integer -1I)))
        | _ -> u
    
    //Simplify function form difference (use inline operator '-' for simplfication of all other expressions)
    let simplifyDifference u =
        match u with 
        | BinaryOp(a,Minus,b) -> BinaryOp(a,Plus,BinaryOp(Number(Integer -1I),Times,b))
        | _ -> u

    let simplifyFactorial x = 
        match x with
        | UnaryOp(Factorial, a) when isNegativeNumber a -> UnaryOp(Factorial,a)
        | UnaryOp(Factorial, Number x) -> Number !*x
        | _ -> x

    let negate x = simplifyProduct (NaryOp(Product, [Number (Integer -1I); x]))

    let simplifyExpression x = 
        let rec simplify a' =
            match a' with 
            | Number (Rational r) when r.denominator = 1I -> Number (Integer r.numerator)
            | Number (Rational r) ->
                let gcd = System.Numerics.BigInteger.GreatestCommonDivisor (r.numerator, r.denominator)
                match gcd with
                | n when n = 1I -> Number (Rational r)
                | _ -> simplify (Number (Rational {numerator = r.numerator/gcd; denominator = r.denominator/gcd}))
            | NaryOp(Sum,a) -> simplifySum (NaryOp(Sum,(List.map simplify a)))            
            | NaryOp(Product,a) -> simplifyProduct (NaryOp(Product,(List.map simplify a)))
            | BinaryOp(a,ToThePowerOf,b) -> simplifyPower (BinaryOp(simplify a,ToThePowerOf,simplify b))
            | UnaryOp(Factorial,a) -> simplifyFactorial (simplify a)
            //Trig Functions            
            //Sine
            | UnaryOp (Sin,NaryOp(Product,a)) when isNegativeNumber a.[0] -> simplify (NaryOp(Product,[Number (Integer -1I);UnaryOp (Sin,NaryOp(Product,(negate a.[0])::a.Tail))]))
            | UnaryOp (Sin,NaryOp(Sum,(Number n)::(NaryOp(Product,a))::t)) when isNegativeNumber a.[0] -> (NaryOp(Product,[Number (Integer -1I);UnaryOp (Sin,NaryOp(Sum,List.map negate ((Number n)::(NaryOp(Product,a))::t)))]))
            | UnaryOp (Sin,NaryOp(Sum,xh::Symbol s::xt)) when s = Constant Pi-> simplify (NaryOp(Product,[Number (Integer -1I);UnaryOp (Sin,NaryOp(Sum,(xh::xt)))]))
            | UnaryOp (Sin,NaryOp(Sum,Symbol s::xn::xt)) when s = Constant Pi-> simplify (NaryOp(Product,[Number (Integer -1I);UnaryOp (Sin,NaryOp(Sum,(xn::xt)))]))            
            | UnaryOp (Sin,NaryOp(Sum,xh::(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xt)) when pi = Constant Pi && r.denominator = 2I -> 
                match r.numerator > 0I, r.Floor%2I = 0I with
                | true, true -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))
                | false, true -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))]))
                | false, false -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))
                | true, false -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))]))
            | UnaryOp (Sin,NaryOp(Sum,(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && r.denominator = 2I ->
                match r.numerator > 0I, r.Floor%2I = 0I with
                | true, true -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))
                | false, true -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))]))
                | false, false -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))
                | true, false -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))]))
            | UnaryOp (Sin,NaryOp(Sum,xh::(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xt)) when pi = Constant Pi && r.denominator = 3I -> 
                match (r.numerator-(r.numerator - (r.Floor*r.denominator)))%2I=0I, abs (r.numerator - (r.Floor*r.denominator)) with
                | true, n when n = 1I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,(negate xh)::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(List.map negate xt)))))
                | true, n when n = 2I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xh::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xt))))
                | false, n when n = 1I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,(negate xh)::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(List.map negate xt)))))]))
                | false, n when n = 2I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xh::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xt))))]))
                | _ -> a'
            | UnaryOp (Sin,NaryOp(Sum,(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && r.denominator = 3I ->
                match (r.numerator-(r.numerator - (r.Floor*r.denominator)))%2I=0I, abs (r.numerator - (r.Floor*r.denominator))  with
                | true, n when n = 1I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(negate xn)::(List.map negate xt)))))
                | true, n when n = 2I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xn::xt))))
                | false, n when n = 1I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(negate xn)::(List.map negate xt)))))]))
                | false, n when n = 2I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::xn::Symbol pi::t))::xt))))]))
                | _ -> a'            
            | UnaryOp (Sin,NaryOp(Sum,xh::(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xt)) when pi = Constant Pi && n%2I = 0I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))
            | UnaryOp (Sin,NaryOp(Sum,(NaryOp(Product,Number(Integer n)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && n%2I = 0I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))
            | UnaryOp (Sin,NaryOp(Sum,xh::(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xt)) when pi = Constant Pi && n%2I <> 0I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))]))
            | UnaryOp (Sin,NaryOp(Sum,(NaryOp(Product,Number(Integer n)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && n%2I <> 0I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))]))
            | UnaryOp (Sin,NaryOp(Product,a)) when isNumber a.[0] && a.[1] = Symbol(Constant Pi) && a.Length = 2 -> 
                    match simplify a.[0] with
                    | Number (Integer i) -> Number (Integer 0I)
                    | Number (Rational r) -> 
                        match r.numerator, r.denominator with
                        | n, d when d = 6I ->
                            match r.Floor%2I = 0I with 
                            | true ->  Number (Rational {numerator = 1I; denominator = 2I})
                            | false -> Number (Rational {numerator = -1I; denominator = 2I})
                        | n, d when d = 3I && (n/3I)%2I = 0I -> NaryOp(Product,[Number (Rational {numerator = 1I;denominator = 2I;});BinaryOp(Number (Integer 3I),ToThePowerOf,Number (Rational {numerator = 1I;denominator = 2I;}))])
                        | n, d when d = 3I && (n/3I)%2I <> 0I -> NaryOp(Product,[Number (Rational {numerator = -1I;denominator = 2I;});BinaryOp(Number (Integer 3I),ToThePowerOf,Number (Rational {numerator = 1I;denominator = 2I;}))])
                        | n, d when d = 4I && (n/4I)%2I = 0I -> BinaryOp(Number (Integer 2I),ToThePowerOf,Number (Rational {numerator = -1I;denominator = 2I;}))
                        | n, d when d = 4I && (n/4I)%2I <> 0I -> NaryOp(Product,[Number (Integer -1I);BinaryOp(Number (Integer 2I),ToThePowerOf,Number (Rational {numerator = -1I;denominator = 2I;}))])
                        | n, d when d = 2I && n%4I = 1I -> Number (Integer 1I)
                        | n, d when d = 2I && n%4I = 3I -> Number (Integer -1I)
                        | n, d when r.compareTo {numerator = 1I; denominator = 2I} = -1 -> a'
                        | n, d when r.compareTo {numerator = 1I; denominator = 1I} = -1 -> simplify (UnaryOp (Sin,NaryOp(Product,(Number (Rational{r with numerator = (r.denominator - r.numerator)}))::a.Tail)))
                        | n, d when r.compareTo {numerator = 3I; denominator = 2I} = -1 -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,NaryOp(Product,(Number (Rational{r with numerator = (abs(r.denominator - r.numerator))}))::a.Tail)))]))
                        | n, d when r.compareTo {numerator = 2I; denominator = 1I} = -1 -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,NaryOp(Product,(Number (Rational{r with numerator = (abs(2I*r.denominator - r.numerator))}))::a.Tail)))]))
                        | n, d when r.compareTo {numerator = 2I; denominator = 1I} = 1 -> 
                            match r.Floor%2I = 0I with 
                            | true -> simplify (UnaryOp (Sin,NaryOp(Product,(Number (Rational{r with numerator = (r.numerator - ((r.Floor)*r.denominator))}))::a.Tail)))
                            | false -> simplify (UnaryOp (Sin,NaryOp(Product,(Number (Rational{r with numerator = abs(r.numerator - ((r.Floor)*r.denominator) + r.denominator)}))::a.Tail)))
                        | _ -> a'
                    | _ -> a'
            | UnaryOp (Sin,a) when a = Number (Integer 0I) -> Number (Integer 0I)            
            //Cosine
            | UnaryOp (Cos,NaryOp(Product,a)) when isNegativeNumber a.[0] -> simplify (UnaryOp (Cos,NaryOp(Product,(negate a.[0])::a.Tail)))
            | UnaryOp (Cos,NaryOp(Sum,(Number n)::(NaryOp(Product,a))::t)) when isNegativeNumber a.[0] -> (UnaryOp (Cos,NaryOp(Product,List.map negate ((Number n)::(NaryOp(Product,a))::t))))
            | UnaryOp (Cos,NaryOp(Sum,xh::Symbol s::xt)) when s = Constant Pi-> simplify (NaryOp(Product,[Number (Integer -1I);UnaryOp (Cos,NaryOp(Sum,(xh::xt)))]))
            | UnaryOp (Cos,NaryOp(Sum,Symbol s::xn::xt)) when s = Constant Pi-> simplify (NaryOp(Product,[Number (Integer -1I);UnaryOp (Cos,NaryOp(Sum,(xn::xt)))]))            
            | UnaryOp (Cos,NaryOp(Sum,xh::(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xt)) when pi = Constant Pi && r.denominator = 2I -> 
                match r.numerator > 0I, r.Floor%2I = 0I  with
                | true,true -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))]))
                | false,true -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))
                | false, false -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))]))
                | true, false -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xh::xt))))
            | UnaryOp (cos,NaryOp(Sum,(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && r.denominator = 2I ->  
                match r.numerator > 0I, r.Floor%2I = 0I  with
                | true,true -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))]))
                | false,true -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))
                | false, false -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))]))
                | true, false -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xn::xt))))
            | UnaryOp (Cos,NaryOp(Sum,xh::(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xt)) when pi = Constant Pi && r.denominator = 3I -> 
                match (r.numerator-(r.numerator - (r.Floor*r.denominator)))%2I=0I, abs (r.numerator - (r.Floor*r.denominator)) with
                | true, n when n = 1I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,(negate xh)::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(List.map negate xt)))))
                | true, n when n = 2I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,xh::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xt))))]))
                | false, n when n = 1I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,(negate xh)::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(List.map negate xt)))))]))
                | false, n when n = 2I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,xh::(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xt))))
                | _ -> a'
            | UnaryOp (Cos,NaryOp(Sum,(NaryOp(Product,Number (Rational r)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && r.denominator = 3I ->
                match (r.numerator-(r.numerator - (r.Floor*r.denominator)))%2I=0I, abs (r.numerator - (r.Floor*r.denominator))  with
                | true, n when n = 1I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(negate xn)::(List.map negate xt)))))
                | true, n when n = 2I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::xn::Symbol pi::t))::xt))))]))
                | false, n when n = 1I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Sin,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::(negate xn)::(List.map negate xt)))))]))
                | false, n when n = 2I -> simplify (UnaryOp (Sin,simplify (NaryOp(Sum,(NaryOp(Product,Number (Rational {numerator = 1I; denominator = 6I})::Symbol pi::t))::xn::xt))))
                | _ -> a'
            | UnaryOp (Cos,NaryOp(Sum,xh::(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xt)) when pi = Constant Pi && n%2I = 0I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))
            | UnaryOp (Cos,NaryOp(Sum,(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && n%2I = 0I -> simplify (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))
            | UnaryOp (Cos,NaryOp(Sum,xh::(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xt)) when pi = Constant Pi && n%2I <> 0I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xh::xt))))]))
            | UnaryOp (Cos,NaryOp(Sum,(NaryOp(Product,Number (Integer n)::Symbol pi::t))::xn::xt)) when pi = Constant Pi && n%2I <> 0I -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,simplify (NaryOp(Sum,xn::xt))))]))
            | UnaryOp (Cos,NaryOp(Product,a)) when isNumber a.[0] && a.[1] = Symbol(Constant Pi) && a.Length = 2 -> 
                    match simplify a.[0] with
                    | Number (Integer i) -> 
                        match i%2I = 0I with
                        | true -> Number (Integer 1I)
                        | false -> Number (Integer -1I)
                    | Number (Rational r) -> 
                        match r.numerator, r.denominator with
                        | n, d when d = 3I -> 
                            match (r.Floor + (n-d*r.Floor%2I))%2I = 0I with 
                                | true ->  Number (Rational {numerator = -1I; denominator = 2I})
                                | false -> Number (Rational {numerator = 1I; denominator = 2I})
                        | n, d when d = 2I -> Number (Integer 0I)
                        | n, d when d = 6I && ({numerator = n;denominator = 4I}.Floor)%2I = 0I -> NaryOp(Product,[Number (Rational {numerator = 1I;denominator = 2I});BinaryOp(Number (Integer 3I),ToThePowerOf,Number (Rational {numerator = 1I;denominator = 2I;}))])
                        | n, d when d = 6I && ({numerator = n;denominator = 4I}.Floor)%2I <> 0I -> NaryOp(Product,[Number (Rational {numerator = -1I;denominator = 2I});BinaryOp(Number (Integer 3I),ToThePowerOf,Number (Rational {numerator = 1I;denominator = 2I;}))])
                        | n, d when d = 4I && ({numerator = n;denominator = 3I}.Floor)%2I = 0I -> BinaryOp(Number (Integer 2I),ToThePowerOf,Number (Rational {numerator = -1I;denominator = 2I;}))
                        | n, d when d = 4I && ({numerator = n;denominator = 3I}.Floor)%2I = 0I -> NaryOp(Product,[Number (Integer -1I);BinaryOp(Number (Integer 2I),ToThePowerOf,Number (Rational {numerator = -1I;denominator = 2I;}))])                        
                        | n, d when r.compareTo {numerator = 1I; denominator = 2I} = -1 -> a'
                        | n, d when r.compareTo {numerator = 1I; denominator = 1I} = -1 -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,NaryOp(Product,(Number (Rational{r with numerator = (r.denominator - r.numerator)}))::a.Tail)))]))
                        | n, d when r.compareTo {numerator = 3I; denominator = 2I} = -1 -> simplify (NaryOp(Product,[Number (Integer -1I); (UnaryOp (Cos,NaryOp(Product,(Number (Rational{r with numerator = (abs(r.denominator - r.numerator))}))::a.Tail)))]))
                        | n, d when r.compareTo {numerator = 2I; denominator = 1I} = -1 -> simplify (UnaryOp (Cos,NaryOp(Product,(Number (Rational{r with numerator = (abs(2I*r.denominator - r.numerator))}))::a.Tail)))
                        | n, d when r.compareTo {numerator = 2I; denominator = 1I} = 1 -> 
                            match r.Floor%2I = 0I with 
                            | true -> simplify (UnaryOp (Cos,NaryOp(Product,(Number (Rational{r with numerator = (r.numerator - ((r.Floor) * r.denominator))}))::a.Tail)))
                            | false -> simplify (UnaryOp (Cos,NaryOp(Product,(Number (Rational{r with numerator = abs(r.numerator - ((r.Floor) * r.denominator) + r.denominator)}))::a.Tail)))
                        | _ -> a'
                    | _ -> a'            
            | UnaryOp (Cos,a) when a = Number (Integer 0I) -> Number (Integer 1I)
            | _ -> a'
            //Trig Functions 
        simplify x

    let treeSize x =
        let eNumber acc (n:Expression) = acc + 1
        let eComplexNumber acc x = acc + 1
        let eSymbol acc (v:Expression) = acc + 1
        let eBinaryOp acc x = acc + 1
        let eUnaryOp acc x = acc + 1
        let eNaryOp acc x = acc + 1
        let acc = 0
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
     
type Expression with
    member this.isNumber = ExpressionType.isNumber this    
    member this.isNegativeNumber = ExpressionType.isNegativeNumber this
    member this.Base = ExpressionType.Base this
    member this.Exponent = ExpressionType.Exponent this
    member this.Term = ExpressionType.Term this
    member this.Const = ExpressionType.Const this
    member this.TreeSize = ExpressionType.treeSize this
    
    static member compareExpressions x y = ExpressionType.compareExpressions x y
    static member Zero = Number (Integer 0I)
    static member (~+) (x:Expression) = x    
    static member (~-) (x:Expression) = ExpressionType.negate x
    static member (+) (x, y) = NaryOp(Sum,[x; y]) |> ExpressionType.simplifySum    
    static member (*) (x, y) = NaryOp(Product,[x; y]) |> ExpressionType.simplifyProduct
    static member (-) (x, y) = NaryOp(Sum,[x; -y]) |> ExpressionType.simplifySum
    static member Pow (x, y) = BinaryOp (x, ToThePowerOf, y) |> ExpressionType.simplifyPower       
    static member (/) (x, y) = NaryOp(Product,[x; (y**(-(Number Number.One)))]) |> ExpressionType.simplifyProduct
    static member (!*) x = UnaryOp(Factorial,x) |> ExpressionType.simplifyFactorial
    static member (^^) (x,y) = BinaryOp (x, ExplicitToThePowerOf, y)

module ExpressionStructure =
    
//Primitive Structure Operators
    let kind x = 
        match x with
        | Number (Integer i) -> "Integer"
        | Number (Rational r) -> "Rational"
        | Number (Real r) -> "Real"
        | _ -> "Undefined"
        // etc.
    let numberOfOperands x = 
        match x with
        | Number n -> Undefined
        | ComplexNumber (a,b) -> Integer 2I
        | Symbol v -> Undefined
        | BinaryOp (a,op,b) -> Integer 2I
        | UnaryOp (op,a) -> Integer 1I
        | NaryOp (op,aList) -> Integer (System.Numerics.BigInteger aList.Length)

    let operand a b =
        match a, b with
        | UnaryOp (op,u), 1 -> u
        | BinaryOp (x,op,y), 1 -> x
        | BinaryOp (x,op,y), 2 -> y
        | NaryOp (op,u), n when n > 0 && n <= u.Length -> u.[n-1]
        | _ -> Number Undefined
        
    let numberOfCompoundExpressions (x:Expression) = 
        let eNumber acc (n:Expression) = acc
        let eComplexNumber acc x = acc + 1
        let eSymbol acc (v:Expression) = acc
        let eBinaryOp acc x = acc + 1
        let eUnaryOp acc x = acc + 1
        let eNaryOp acc x = acc + 1
        let acc = 0
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x

    let numberOfAtomicExpressions (x:Expression) = 
        let eNumber acc (n:Expression) = 1 + acc
        let eComplexNumber acc x = acc
        let eSymbol acc (v:Expression) = 1 + acc
        let eBinaryOp acc x = acc
        let eUnaryOp acc x = acc
        let eNaryOp acc x = acc
        let acc = 0
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x

    let subExpressions (x:Expression) = 
        let eNumber acc (n:Expression) = n::acc
        let eComplexNumber acc x = x::acc
        let eSymbol acc (v:Expression) = v::acc
        let eBinaryOp acc x = x::acc
        let eUnaryOp acc x = x::acc
        let eNaryOp acc x = x::acc
        let acc = []
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x

    let variables (x:Expression) = 
        let eNumber acc (n:Expression) = acc
        let eComplexNumber acc x = acc
        let eSymbol acc (v:Expression) = match v with | Symbol (Variable v) -> Variable v::acc | Symbol (Constant c) -> acc | _ -> acc
        let eBinaryOp acc x = acc
        let eUnaryOp acc x = acc
        let eNaryOp acc x = acc
        let acc = []
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
        |> Seq.distinct
        |> Seq.toList

// Structure-based Operators  
    let freeOf u t =
        let completeSubExpressions = subExpressions u
        not (List.exists (fun x -> x = t) completeSubExpressions)

    let freeOfSort s = 
        let compare x y = 
            match freeOf x y with
            | true -> 1
            | false -> -1
        List.sortWith (fun x y -> compare x y) s
 
    let substitute (y, t) u =
        let eNumber (n:Expression) = (match n = y with | true -> t | false -> n)
        let eComplexNumber (a,b) = (match ComplexNumber (a,b) = y with | true -> t | false -> ComplexNumber (a,b))
        let eSymbol (v:Expression) = (match v = y with | true -> t | false -> v)
        let eBinaryOp (a,op,b) = (match BinaryOp (a,op,b) = y with | true -> t | false -> BinaryOp (a,op,b))
        let eUnaryOp (op,a) = (match UnaryOp (op,a) = y with | true -> t | false -> UnaryOp (op,a))
        let eNaryOp (op,aList) = (match NaryOp (op,aList) = y with | true -> t | false -> NaryOp (op,aList))
        Cata.recurseExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp u
  
    let substituteSequential (yList : (Expression*Expression) list) u =        
        List.fold (fun u' (x,y) -> substitute (x, y) u') u yList

    let rec substituteConcurrent (yList : (Expression*Expression) list) = function
        | Number n ->
            let x = List.exists (fun x -> fst x = Number n) yList            
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = Number n) yList)
                (snd y)
            | false -> Number n
        | ComplexNumber (a,b) ->
            let x = List.exists (fun x -> fst x = ComplexNumber (a,b)) yList
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = ComplexNumber (a,b)) yList)
                (snd y)
            | false -> (ComplexNumber (substituteConcurrent yList a,substituteConcurrent yList b))
        | Symbol v ->
            let x = List.exists (fun x -> fst x = Symbol v) yList
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = Symbol v) yList)
                (snd y)
            | false -> (Symbol v)
        | BinaryOp (a,op,b) ->
            let x = List.exists (fun x -> fst x = BinaryOp (a,op,b)) yList
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = BinaryOp (a,op,b)) yList)
                (snd y)
            | false -> (BinaryOp (substituteConcurrent yList a,op,substituteConcurrent yList b))
        | UnaryOp (op,a) ->
            let x = List.exists (fun x -> fst x = UnaryOp (op,a)) yList
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = UnaryOp (op,a)) yList)
                (snd y)
            | false -> (UnaryOp (op,substituteConcurrent yList a))
        | NaryOp (op,aList) ->
            let x = List.exists (fun x -> fst x = NaryOp (op,aList)) yList
            match x with
            | true -> 
                let y = (List.find (fun x -> fst x = NaryOp (op,aList)) yList)
                (snd y)
            | false -> (NaryOp (op,List.map (fun x -> substituteConcurrent yList x) aList))

    let rec numerator (u : Expression) = 
        match u with
        | Number (Rational {numerator = n; denominator = d}) -> (Number (Integer n)) //ND-1
        | BinaryOp (x, ToThePowerOf, Number (Integer n)) when n < 0I -> (Number (Integer 1I)) //ND-2
        | BinaryOp (x, ToThePowerOf, Number (Rational n)) when n < Fraction.Zero -> (Number (Integer 1I)) //ND-2
        | NaryOp(Product,xList) -> //ND-3
            let v = operand u 1
            (numerator v)*(numerator (u/v))
        | _ -> u //ND-4
 
    let rec denominator (u : Expression) = 
        match u with
        | Number (Rational {numerator = n; denominator = d}) -> (Number (Integer d)) //ND-1
        | BinaryOp (x, ToThePowerOf, Number (Integer n)) when n < 0I -> (x**Number (Integer -n)) //ND-2
        | BinaryOp (x, ToThePowerOf, Number (Rational n)) when n < Fraction.Zero -> (x**Number -(Rational n)) //ND-2
        | NaryOp(Product,xList) -> //ND-3
            let v = operand u 1
            (denominator v) * (denominator (u/v))
        | _ -> (Number (Integer 1I)) //ND-4

module ExpressionFunction =

    let rec abs x =
        match x with
        | Number n -> Number (Number.abs n)
        | NaryOp(Product,xList) -> NaryOp(Product,(List.map (fun x -> abs x) xList))
        | BinaryOp(a,ToThePowerOf,Number(Integer i)) -> (abs a)**Number(Integer i)
        | ComplexNumber(a,b) -> ((a**Number(Integer 2I))+(b**Number(Integer 2I)))**(Number(Integer 1I)/Number(Integer 2I))        
        | _ -> UnaryOp(Abs,x)
        

    let floor u =
        match u with
        | Number n -> Number (Number.floor n)
        | _ -> u
    let evaluateRealPowersOfExpression (u:Expression) =
        let eNumber (n:Expression) = n
        let eComplexNumber (a,b) = ComplexNumber(a,b)
        let eSymbol (v:Expression) = v
        let eBinaryOp (a,op,b) = 
            match op with
            | ToThePowerOf -> a**b
            | _ -> BinaryOp(a,op,b)
        let eUnaryOp (op,a) = UnaryOp (op,a)
        let eNaryOp (op,aList) = NaryOp (op,aList)
        Cata.recurseExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp u
