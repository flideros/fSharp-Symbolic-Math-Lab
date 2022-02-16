namespace Math.Pure.Structure

open Math.Pure.Objects
open Math.Pure.Quantity
open Math.Pure.Structure
open Math.Pure.Structure.Polynomial  
   
    [<RequireQualifiedAccess>]
    module PartialFraction =
        
        //u/v1v2 is proper and = u1/v1 + u2/v2
        let private expansion1 u v1 v2 x = 
            let s = Euclidean.extAlgorithm v1 v2 x
            let A, B = match s with | (a,b,c) -> b, match s with | (a,b,c) -> c
            let u1 = Divide.getRemainder (Expand.algebraicExpression (B*u)) v1 x
            let u2 = Divide.getRemainder (Expand.algebraicExpression (A*u)) v2 x
            (u1,u2)

        //u/v is a proper rational expression and v is the irreducable factorization of v in F[x]
        let rec private expansion2 u v x =
            match v with 
            | NaryOp(Product, aList) -> 
                let f = (aList).Head
                let r = v/f
                match ExpressionStructure.freeOf f x with
                | true -> (Number(Integer 1I)/f) * expansion2 u r x
                | false -> 
                    let s = expansion1 u (Expand.algebraicExpression f) (Expand.algebraicExpression r) x
                    let u1, w = fst s, snd s
                    (u1/f + (expansion2 w r x))
            | _ -> u/v
            
        //u/v is a proper rational expression where u is in expanded form and v is the irreducable factorization of v in Q[x]
        let private expansion3 u v x = 
            let q = Divide.getQuotient u (Expand.algebraicExpression v) x
            let r = Divide.getRemainder u (Expand.algebraicExpression v) x
            let f = expansion2 r v x
            let expansion xList = 
                List.map (fun x' -> 
                    let s,t,one = Symbol (Variable "s"), Symbol (Variable "t"), Number(Integer 1I)
                    let num = RationalExpression.numerator x'
                    let denom = RationalExpression.denominator x'
                    let dBase = match denom with | BinaryOp(a,ToThePowerOf,b) -> a | _ -> denom
                    let dPow = match denom with | BinaryOp(a,ToThePowerOf,b) -> b | _ -> one
                    (Expansion.ofGPE num dBase x t)*s
                    |> Expand.algebraicExpression
                    |> ExpressionStructure.substitute (s,one/(t**dPow)) 
                    |> Expand.algebraicExpression 
                    |> ExpressionStructure.substitute (t,dBase)) xList
            match f with
            | NaryOp(Product,(Number n)::(NaryOp(Sum,sList))::[]) -> q + (Number n)*(NaryOp(Sum,expansion sList)) |> ExpressionType.simplifyExpression 
            | NaryOp(Sum,sList) -> q + NaryOp(Sum,expansion sList) |> ExpressionType.simplifyExpression
            | _ when f = (u/v) -> 
                match u with
                | NaryOp(Sum,sList) -> q + NaryOp(Sum,expansion (List.map (fun r -> r/v)sList)) |> ExpressionType.simplifyExpression
                | _ -> q + r/v
            | _ -> q + r/v

        //
        let expansion f x = 
            let n = ExpressionStructure.numerator f |> Expand.algebraicExpression
            let d = Factorization.factorGPE (ExpressionStructure.denominator f |> Expand.algebraicExpression) x
            expansion3 n d x
         

        let numerical r =
            let multiply (factorList : Expression list) =
                let rec loop list acc =
                   match list with
                   | head :: tail -> loop tail (BinaryOp(acc,Times,head))
                   | [] -> acc
                loop factorList.Tail factorList.Head
            match r with 
            | Number (Integer i) as int when i > 3I -> 
                let factors = NumberTheory.Integer.factorByTrialDivision int
                multiply factors
            | Number (Rational ({numerator = n; denominator = d})) when d <> 0I -> 
                let nFactors = NumberTheory.Integer.factorByTrialDivision (Number(Integer n))
                let dFactors = NumberTheory.Integer.factorByTrialDivision (Number(Integer d))
                BinaryOp(multiply nFactors,DividedBy,multiply dFactors)
            | _ -> r


