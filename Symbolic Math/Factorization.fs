namespace Math.Pure.Structure

open Math.Pure.Objects
open Math.Pure.Quantity 
open Math.Pure.Change.Calculus.Differential
open Math.Pure.Structure.Polynomial
    
    module Factorization =
        
        let squareFree u x =
            match u = Number(Integer 0I) with
            | true -> u
            | false -> 
                let c = Coefficients.leadingGPE u x
                let U = Expand.algebraicExpression (u/c)
                let P = Number(Integer 1I)
                let R = Divisors.gcd U (derivativeOf U x) x
                let F = Divide.getQuotient U R x
                let rec loop P R F j =
                    match R = Number(Integer 1I) with 
                    | true -> P*F**j
                    | false ->
                        let G = Divisors.gcd R F x
                        let s = Divide.getQuotient F G x
                        let P' = P*s**j
                        let R' = Divide.getQuotient R G x                        
                        loop P' R' G (j + Number(Integer 1I))
                c*(loop P R F (Number(Integer 1I)))   
                
        let squareFreeZ u x =
            let one = Number(Integer 1I)
            let zero = Number(Integer 0I)
            let content u = 
                match u = zero with
                | true -> zero
                | false -> 
                    let cList = 
                        Coefficients.listOfGPE u x 
                        |> List.choose (fun x -> 
                        match x <> zero with 
                        | true -> Some x 
                        | _ -> None)
                    Divisors.normalize (List.reduce (fun acc x' -> Divisors.mVPolyGCD acc x' [] (IntegralNumbers)) cList) [x] (IntegralNumbers)
            match u = zero with
            | true -> u
            | false -> 
                let c = 
                    let lc = Coefficients.leadingGPE u x
                    let c' = Number(Integer (Coefficients.signOfGPE lc [x]))
                    c'*(content u)
                let U = Expand.algebraicExpression (u/c)
                let P = one
                let R = Divisors.mVPolyGCD U (derivativeOf U x) [x] (IntegralNumbers)
                let F = Divide.getQuotient U R x 
                let rec loop P R F j =
                    match R = one with 
                    | true -> P*F**j
                    | false ->
                        let G = Divisors.mVPolyGCD R F [x] (IntegralNumbers)
                        let s = Divide.getQuotient F G x
                        let P' = P*s**j
                        let R' = Divide.getQuotient R G x                        
                        loop P' R' G (j + Number(Integer 1I))
                c*(loop P R F (Number(Integer 1I)))
        
        let squareFreeP u x p =
            match u = Number(Integer 0I) with
            | true -> u
            | false -> 
                let c = Coefficients.leadingGPE u x
                let U = Expand.algebraicExpressionP (u/c) p
                let P = Number(Integer 1I)
                let R = Divisors.gcdP U (derivativeOf U x) x p
                let F = Divide.getQuotientP U R x p
                let rec loop P R F j =
                    match R = Number(Integer 1I) with 
                    | true -> P*F**j
                    | false ->
                        let G = Divisors.gcdP R F x p
                        let s = Divide.getQuotientP F G x p
                        let P' = P*s**j
                        let R' = Divide.getQuotientP R G x p
                        loop P' R' G (j + Number(Integer 1I))
                c*(loop P R F (Number(Integer 1I))) 
        
        let factorGPE v x =
            match v with
            | Number n as num -> v
            | Symbol s -> v
            | NaryOp(Product,[Number n;Symbol s]) -> v
            | BinaryOp(a,ToThePowerOf,Number (Integer i)) when a = x -> v 
            | _ when ExpressionStructure.freeOf v x = true -> v
            | _ ->
                let rec fact list u = 
                    match Check.isPolynomialGPE u [x] with
                    | true -> 
                        let deg = match Degree.ofGPE u [x] with | Integer i -> i | _ -> 0I
                        let coeffList = Coefficients.listOfGPE u x
                        let firstCoeffIndex = List.findIndex (fun x -> x <> Number(Integer 0I)) coeffList
                        let candidates = Factor.candidates (coeffList.[firstCoeffIndex]) (coeffList.[(coeffList.Length - 1)]) |> List.choose (fun x -> match x with | Number Undefined -> None | _ -> Some x)
                        let check n = ExpressionStructure.substitute (x,n) u |> ExpressionType.simplifyExpression
                        let rootlist = 
                            let r1 = List.tryPick (fun x' -> match check x' with | Number (Integer i) when i = 0I -> Some (x - x') | _ -> None) candidates
                            match r1 with
                            | None -> //(Divisors.makeMonic u x) :: list
                                let rec tryHigherDegreeMonic d =
                                    let dCandidates = List.map (fun c -> ((x**d) - c)) candidates
                                    let r2 = List.tryPick (fun x' -> match Divide.getRemainder u x' x = Number (Integer 0I) with | true -> Some x' | _ -> None) dCandidates
                                    match r2, Number (Degree.ofGPE u [x]) = (d) with                                
                                    | Some e, _ -> 
                                        let list' = e :: list
                                        let u' = Divide.getQuotient u e x
                                        match u' with 
                                        | Number n -> list'
                                        | _ -> fact list' u'
                                    | None, true -> (Divisors.makeMonic u x) :: list
                                    | None, false -> tryHigherDegreeMonic (d + Number(Integer 1I))
                                tryHigherDegreeMonic (Number (Integer 2I))

                            | Some e -> 
                                let list' = e :: list
                                let u' = Divide.getQuotient u e x
                                match u' with 
                                | Number n -> list'
                                | _ -> fact list' u'                
                        [List.fold (fun acc x -> acc * x) (Number(Integer 1I)) rootlist]
                    | false -> [] 
            
                let multiple w = Divide.getQuotient w (Expand.algebraicExpression (fact [] w).Head) x
            
                let rec getConstantTerm w x'=
                    match Check.sumContainsConstantTerm w with
                    | true -> (w,x')
                    | false -> getConstantTerm (Distribute.expression (w / x)) (x'*x)
            
           
                match Check.sumContainsConstantTerm v with
                | true -> (multiple v)*(fact [] v).Head
                | false -> 
                    let gC = getConstantTerm v (Number(Integer 1I))
                    let v', x' = fst gC, snd gC
                    (multiple v')*x'*(fact [] v').Head

