namespace Math.Pure.Space

open Math.Pure.Structure
open Math.Pure.Structure.ExpressionStructure
open Math.Pure.Objects
open Math.Pure.Quantity

module Trigonometry =
    
    [<RequireQualifiedAccess>]
    module Substitute = 
        let rec forTrigFunction u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b)-> ComplexNumber (forTrigFunction a, forTrigFunction b)
            | BinaryOp (a,op,b) -> BinaryOp (forTrigFunction a,op, forTrigFunction b)
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> forTrigFunction x) aList)
            | UnaryOp(Tan,a) -> UnaryOp(Sin,a)/UnaryOp(Cos,a)
            | UnaryOp(Cot,a) -> UnaryOp(Cos,a)/UnaryOp(Sin,a)
            | UnaryOp(Sec,a) -> Number(Integer 1I)/UnaryOp(Cos,a)
            | UnaryOp(Csc,a) -> Number(Integer 1I)/UnaryOp(Sin,a)
            | UnaryOp(op,a) -> UnaryOp(op,forTrigFunction a)
            |> ExpressionType.simplifyExpression

    [<RequireQualifiedAccess>]
    module Seperate =
        open ExpressionType

        let sinCos u =
            match u with
            | NaryOp(Product,aList) -> 
                let s, r = List.partition (fun x ->
                    match x with
                    | UnaryOp(Sin,s) -> true
                    | UnaryOp(Cos,s) -> true
                    | BinaryOp(UnaryOp(Sin,s),ToThePowerOf,Number(Integer i)) when i > 0I -> true
                    | BinaryOp(UnaryOp(Cos,s),ToThePowerOf,Number(Integer i)) when i > 0I -> true
                    | _ -> false) aList
                match r = [], s = [] with 
                | false, true -> (simplifyExpression (NaryOp(Product,r)), Number(Integer 1I))
                | false, false -> (simplifyExpression (NaryOp(Product,r)), simplifyExpression (NaryOp(Product,s)))
                | true, true -> (Number(Integer 1I), Number(Integer 1I))
                | true, false -> (Number(Integer 1I), simplifyExpression (NaryOp(Product,s)))
            | UnaryOp(Sin,s) -> (Number(Integer 1I),u)
            | UnaryOp(Cos,s) -> (Number(Integer 1I),u)
            | BinaryOp(UnaryOp(Sin,s),ToThePowerOf,Number(Integer i)) when i > 0I -> (Number(Integer 1I),u)
            | BinaryOp(UnaryOp(Cos,s),ToThePowerOf,Number(Integer i)) when i > 0I -> (Number(Integer 1I),u)
            | _ -> (u, Number(Integer 1I))

    [<RequireQualifiedAccess>]
    module Expand =

        let private check u =
            let u' = subExpressions u
            let check1 x = 
                match x with
                | NaryOp(Sum,aList) -> false
                | NaryOp(Product,aList) when (match aList.[0] with | Number(Integer i) -> false | _ -> true) = false -> false
                | _ -> true
            let check2 x =
                match x with
                | UnaryOp(Sin, a) when check1 a = false -> false
                | UnaryOp(Cos, a) when check1 a = false -> false
                | _ -> true
            List.forall (fun x -> check2 x) u'

        let multAngCos n u =      
            let n' = match n with | Number (Integer i) -> i | _ -> 1I
            let n'' = abs n'
            let cList = [for j in 0I..2I..n'' -> ((Number(Integer -1I)**(Number (Integer j)/(Number(Integer 2I)))) *
                                                  (Polynomial.Expand.binomialCoeff (Number(Integer n'')) (Number (Integer j))) *
                                                  ((UnaryOp(Cos,u))**((Number(Integer n'')) - Number (Integer j))) *
                                                  ((UnaryOp(Sin,u))**(Number (Integer j))))]        
            match ExpressionType.compareExpressions n (Number (Integer 0I)) with
            | 1 -> NaryOp(Sum,cList)
            | _ -> -(NaryOp(Sum,cList)) |> Polynomial.Distribute.expression

        let multAngSin n u =         
            let n' = match n with | Number (Integer i) -> i | _ -> 1I
            let n'' = abs n'
            let cList = [for j in 1I..2I..n'' -> ((Number(Integer -1I)**((Number (Integer j)-(Number(Integer 1I)))/(Number(Integer 2I)))) *
                                                  (Polynomial.Expand.binomialCoeff (Number(Integer n'')) (Number (Integer j))) *
                                                  ((UnaryOp(Cos,u))**((Number(Integer n'')) - Number (Integer j))) *
                                                  ((UnaryOp(Sin,u))**(Number (Integer j))))]         
            match ExpressionType.compareExpressions n (Number (Integer 0I)) with
            | 1 -> NaryOp(Sum,cList)
            | _ -> -(NaryOp(Sum,cList)) |> Polynomial.Distribute.expression
    
        let expandMultAngCosh n u =      
            let n' = match n with | Number (Integer i) -> i | _ -> 1I
            let n'' = abs n'
            let cList = [for j in 0I..2I..n'' ->  (Polynomial.Expand.binomialCoeff (Number(Integer n'')) (Number (Integer j))) *
                                                  ((UnaryOp(Cosh,u))**((Number(Integer n'')) - Number (Integer j))) *
                                                  ((UnaryOp(Sinh,u))**(Number (Integer j)))]            
            match ExpressionType.compareExpressions n (Number (Integer 0I)) with
            | 1 -> NaryOp(Sum,cList)
            | _ -> -(NaryOp(Sum,cList)) |> Polynomial.Distribute.expression

        let expandMultAngSinh n u =         
            let n' = match n with | Number (Integer i) -> i | _ -> 1I
            let n'' = abs n'
            let cList = [for j in 1I..2I..n'' ->  (Polynomial.Expand.binomialCoeff (Number(Integer n'')) (Number (Integer j))) *
                                                  ((UnaryOp(Cosh,u))**((Number(Integer n'')) - Number (Integer j))) *
                                                  ((UnaryOp(Sinh,u))**(Number (Integer j)))]         
            match ExpressionType.compareExpressions n (Number (Integer 0I)) with
            | 1 -> NaryOp(Sum,cList)
            | _ -> -(NaryOp(Sum,cList)) |> Polynomial.Distribute.expression
    
        let rec private trigRules A' =
            let A = Polynomial.Expand.algebraicExpression A'
            match A with
            | NaryOp(Sum,aList) -> 
                let f = trigRules (aList.[0])
                let r = trigRules (A - (aList.[0]))
                let s = Polynomial.Expand.algebraicExpression ((fst f)*(snd r)) + Polynomial.Expand.algebraicExpression ((snd f)*(fst r))
                let c = Polynomial.Expand.algebraicExpression ((snd f)*(snd r)) - Polynomial.Expand.algebraicExpression ((fst f)*(fst r))
                (s,c)
            | NaryOp(Product,aList) -> 
                let f = aList.[0]            
                match f with
                | Number (Integer i) when i > 0I -> ((multAngSin f (A/f)),(multAngCos f (A/f)))
                | Number (Integer i) when i < 0I -> ((multAngSin f (A/f)),(multAngCos (Number (Number.abs (Integer i))) (A/f)))
                | _ -> (UnaryOp(Sin,A),UnaryOp(Cos,A))
            | _ -> (UnaryOp(Sin,A),UnaryOp(Cos,A))
    
        let rec private hyperbolicRules A' =
            let A = Polynomial.Expand.algebraicExpression A'
            match A with
            | NaryOp(Sum,aList) -> 
                let f = hyperbolicRules (aList.[0])
                let r = hyperbolicRules (A - (aList.[0]))
                let s = Polynomial.Expand.algebraicExpression ((fst f)*(snd r)) + Polynomial.Expand.algebraicExpression ((snd f)*(fst r))
                let c = Polynomial.Expand.algebraicExpression ((snd f)*(snd r)) + Polynomial.Expand.algebraicExpression ((fst f)*(fst r))
                (s,c)
            | NaryOp(Product,aList) -> 
                let f = aList.[0]            
                match f with
                | Number (Integer i) when i > 0I -> ((expandMultAngSinh f (A/f)),(expandMultAngCosh f (A/f)))
                | Number (Integer i) when i < 0I -> (((expandMultAngSinh f (A/f))),(expandMultAngCosh (Number (Number.abs (Integer i))) (A/f)))
                | _ -> (UnaryOp(Sinh,A),UnaryOp(Cosh,A))
            | _ -> (UnaryOp(Sinh,A),UnaryOp(Cosh,A))

        let trigFunction u' = 
            let rec trigFunction' u =
                let uOut = 
                    match u with
                    | Number n -> u
                    | Symbol s -> u
                    | ComplexNumber (a,b)-> ComplexNumber (trigFunction' a, trigFunction' b)
                    | BinaryOp (a,op,b) -> BinaryOp (trigFunction' a,op, trigFunction' b)
                    | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> trigFunction' x) aList)
                    | UnaryOp(Sin,a) -> fst (trigRules a)
                    | UnaryOp(Cos,a) -> snd (trigRules a)
                    | UnaryOp(Tan,a) -> (trigFunction' (UnaryOp(Sin,a)))/(trigFunction' (UnaryOp(Cos,a)))
                    | UnaryOp(Sinh,a) -> fst (hyperbolicRules a)
                    | UnaryOp(Cosh,a) -> snd (hyperbolicRules a)
                    | UnaryOp(op,a) -> UnaryOp(op,trigFunction' a)        
                match check uOut with
                | true -> uOut
                | false -> trigFunction' uOut
                |> ExpressionType.simplifyExpression
            trigFunction' u'
            |> Polynomial.Expand.algebraicExpression

    [<RequireQualifiedAccess>]
    module Contract =

        let trigPower u =
            match u with
            | BinaryOp (b,ToThePowerOf,Number (Integer i)) when i > 0I -> 
                let n, one, two = Number (Integer i), Number (Integer 1I), Number (Integer 2I)
                let binomCoeff n j = Polynomial.Expand.binomialCoeff n j
                match b, i%2I = 0I with
                | UnaryOp(Sin,a), true -> 
                    ((((-one))**n) * (binomCoeff n (n / two)) / (two**n)) +
                    (((-one)**(n/two)) / (two**(n-one))) * Polynomial.Expand.mainOpOf (ExpressionType.simplifyExpression
                        (NaryOp(Sum,
                            [for j in 0I..((i/2I) - 1I) -> 
                              ((-one)**(Number (Integer j))) * 
                              (binomCoeff n (Number (Integer j))) *
                              UnaryOp(Cos, (n-(two*(Number (Integer j))))*a)])))
                | UnaryOp(Sin,a), false -> 
                    ((((-one))**((n-one) / two)) / (two**(n-one))) * (ExpressionType.simplifyExpression
                        (NaryOp(Sum,
                            [for j in 0I..(i/2I) ->  
                              ((-one)**(Number (Integer j))) * 
                              (binomCoeff n (Number (Integer j))) *
                              UnaryOp(Sin, (n-(two*(Number (Integer j))))*a)])))
                | UnaryOp(Cos,a), true -> 
                    ((binomCoeff n (n / two)) / (two**n)) + Polynomial.Expand.mainOpOf (
                        (one / (two**(n-one))) * (ExpressionType.simplifyExpression
                         (NaryOp(Sum,
                            [for j in 0I..((i/2I) - 1I) -> 
                              (binomCoeff n (Number (Integer j))) *
                              UnaryOp(Cos, (n-(two*(Number (Integer j))))*a)]))))  
                | UnaryOp(Cos,a), false -> 
                    (one / (two**(n-one))) * Polynomial.Expand.mainOpOf (ExpressionType.simplifyExpression
                        (NaryOp(Sum,
                            [for j in 0I..(i/2I) ->  
                              (binomCoeff n (Number (Integer j))) *
                              UnaryOp(Cos, (n-(two*(Number (Integer j))))*a)])))
                | _ -> u
            | _ -> u

        let rec private contractionRules u =
            let v = Polynomial.Expand.algebraicExpression u
            match v with
            | BinaryOp (b,ToThePowerOf,s) -> trigPower v
            | NaryOp (Product,aList) -> 
                let s = Seperate.sinCos v
                match (snd s) with
                | Number(Integer i) when i = 1I -> v
                | UnaryOp(Sin,a) -> v
                | UnaryOp(Cos,a) -> v
                | BinaryOp (b,ToThePowerOf,n) -> Polynomial.Expand.mainOpOf ((fst s)*(trigPower (snd s)))
                | _ -> 
                    let rec trigProduct u' =
                        match u' with
                        | NaryOp(Product, aList) when aList.Length = 2 -> 
                            let A = aList.[0]
                            let B = aList.[1]
                            match A with
                            | BinaryOp (b,ToThePowerOf,s) -> 
                                let A' = trigPower A 
                                contractionRules (A' * B)
                            | _ ->
                                match B with
                                | BinaryOp (b,ToThePowerOf,s) -> 
                                    let B' = trigPower B
                                    contractionRules (A * B')
                                | _ -> 
                                    let theta = operand A 1
                                    let phi = operand B 1
                                    match A, B with
                                    | UnaryOp(Sin,a), UnaryOp(Sin,b) -> ((UnaryOp(Cos,theta - phi))/Number(Integer 2I)) - ((UnaryOp(Cos,theta + phi))/Number(Integer 2I)) 
                                    | UnaryOp(Cos,a), UnaryOp(Cos,b) -> ((UnaryOp(Cos,theta + phi))/Number(Integer 2I)) + ((UnaryOp(Cos,theta - phi))/Number(Integer 2I)) 
                                    | UnaryOp(Sin,a), UnaryOp(Cos,b) -> ((UnaryOp(Sin,theta + phi))/Number(Integer 2I)) + ((UnaryOp(Sin,theta - phi))/Number(Integer 2I)) 
                                    | UnaryOp(Cos,a), UnaryOp(Sin,b) -> ((UnaryOp(Sin,theta + phi))/Number(Integer 2I)) + ((UnaryOp(Sin,phi - theta))/Number(Integer 2I)) 
                                    | _ -> u'
                        | NaryOp(Product, aList)  ->
                            let A = aList.[0]
                            let B = trigProduct (NaryOp(Product, aList.Tail))
                            contractionRules (A*B)    
                        | _ -> u'            
                    Polynomial.Expand.mainOpOf ((fst s)*(trigProduct (snd s)))
            | NaryOp (Sum,aList) -> 
                List.fold ( fun acc y ->
                    match y with
                    | NaryOp (Product,aList) -> acc + (contractionRules y) |> Polynomial.Expand.algebraicExpression
                    | BinaryOp (b,ToThePowerOf,s) -> acc + (contractionRules y) |> Polynomial.Expand.algebraicExpression
                    | _ -> acc + y |> Polynomial.Expand.algebraicExpression) (Number (Integer 0I)) aList 
            | _ -> v
            |> ExpressionType.simplifyExpression

        let rec trigFunction u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b) -> ComplexNumber (trigFunction a, trigFunction b)
            | BinaryOp (a,ToThePowerOf,b) -> contractionRules (BinaryOp (trigFunction a,ToThePowerOf, trigFunction b))
            | BinaryOp (a,op,b) -> BinaryOp (trigFunction a,op, trigFunction b)            
            | NaryOp (Product,aList) -> contractionRules (NaryOp (Product,List.map (fun x -> trigFunction x) aList)) |> ExpressionType.simplifyExpression
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> trigFunction x) aList) |> ExpressionType.simplifyExpression      
            | UnaryOp(op,a) -> UnaryOp(op,trigFunction a)            
            |> Polynomial.Expand.algebraicExpression
            |> ExpressionType.simplifyExpression
            
    [<RequireQualifiedAccess>]
    module Simplify =  
        
        ///Pythagorean trigonometric identity
        let byIdentity u =
            let L = ExpressionStructure.variables u |> List.map (fun x -> Symbol x)
            let sin(x) = UnaryOp(Sin,x)
            let cos(x) = UnaryOp(Cos,x)
            let rec loop u' L' =
                match L' with
                | [] -> u'
                | _ ->
                    let x = L'.Head        
                    let i1 = Polynomial.Divide.getMBRemainder u' (sin(x)**Number(Integer 2I) + cos(x)**Number(Integer 2I) - Number(Integer 1I)) [sin(x);cos(x)]
                    let i2 = Polynomial.Divide.getMBRemainder u' (sin(x)**Number(Integer 2I) + cos(x)**Number(Integer 2I) - Number(Integer 1I)) [cos(x);sin(x)]
                    let out = 
                        Polynomial.Collect.termsOfGPE (List.sortBy (fun (x:Expression) -> x.TreeSize) [i1;i2]).Head [Number(Integer 1I)] 
                        |> ExpressionType.simplifyExpression                
                    loop out L'.Tail
            let out = loop u L
            match out.TreeSize < u.TreeSize with
            | true -> out
            | false -> u

        let rec expression u = 
           let simplify u = 
                let v = Substitute.forTrigFunction u
                let w = Polynomial.RationalExpression.transform v
                let n = Contract.trigFunction (Expand.trigFunction (Polynomial.RationalExpression.numerator w))
                let d = Contract.trigFunction (Expand.trigFunction (Polynomial.RationalExpression.denominator w))
                let out = 
                    match d = Number (Integer 0I) with
                    | true -> Number Undefined
                    | false -> n/d
                out
           match u with
            | BinaryOp (a,Equals,b) -> expression (a-b)
            | _ -> simplify u
            


