namespace Math.Pure.Change

open Math.Pure.Objects
open Math.Pure.Quantity
open Math.Pure.Structure
open Math.Pure.Structure.Polynomial
open Math.Pure.Structure.ExpressionStructure

module Calculus =

    module Differential = 

        let rec derivativeOf u' x = 
            let u = u' |> ExpressionType.simplifyRealExpression
            match u = x with
            | true -> Number (Integer 1I) //DERIV-1
            | false -> 
                match u with
                | BinaryOp(v,ToThePowerOf,w) -> //DERIV-2 Functional Power Rule
                     w * (v**(w - Number (Integer 1I))) * (derivativeOf v x ) + (derivativeOf w x ) * (v**w) * UnaryOp(Ln,v) //|> ExpressionType.simplifyRealExpression
                | NaryOp(Sum,uList) -> //DERIV-3 Sum Rule
                    let v = operand u 1
                    let w = (u - v) |> ExpressionType.simplifyRealExpression
                    (derivativeOf v x ) + (derivativeOf w x ) 
                | NaryOp(Product,uList) -> //DERIV-4 The product rule
                    let v = operand u 1
                    let w = (u / v) |> ExpressionType.simplifyRealExpression
                    (derivativeOf v x ) * w + v * (derivativeOf w x ) 
                | NaryOp(Product,uList) as a when RationalExpression.denominator a <> Number (Integer 1I) || RationalExpression.denominator a <> Number (Real 1.0)-> // The quotient rule
                    let v = RationalExpression.numerator u
                    let w = RationalExpression.denominator u
                    ((derivativeOf v x) * w - v * (derivativeOf w x ))/(w**(Number(Integer 2I))) |> ExpressionType.simplifyRealExpression
                
                | BinaryOp(f,Derivative,x) -> 
                    match f with 
                    | UnaryOp(Sin,v) -> UnaryOp(Cos,v) //DERIV-5
                    | UnaryOp(Cos,v) -> -UnaryOp(Sin,v)
                    | UnaryOp(Tan,v) -> derivativeOf (UnaryOp(Sin,v)/UnaryOp(Cos,v)) x 
                    | UnaryOp(Cot,v) -> derivativeOf (UnaryOp(Cos,v)/UnaryOp(Sin,v)) x
                    | UnaryOp(Sec,v) -> derivativeOf (Number(Integer 1I)/UnaryOp(Cos,v)) x
                    | UnaryOp(Csc,v) -> derivativeOf (Number(Integer 1I)/UnaryOp(Sin,v)) x
                    | UnaryOp(ArcSin,v) -> Number(Integer 1I)/((Number(Integer 1I) - x**Number(Integer 2I))**(Number(Integer 1I)/Number(Integer 2I)))
                    | UnaryOp(ArcCos,v) -> -Number(Integer 1I)/((Number(Integer 1I) - x**Number(Integer 2I))**(Number(Integer 1I)/Number(Integer 2I)))
                    | UnaryOp(ArcTan,v) -> Number(Integer 1I)/(Number(Integer 1I) - x**Number(Integer 2I))                    
                    | UnaryOp(ArcSec,v) -> Number(Integer 1I)/((Math.Pure.Structure.ExpressionFunction.abs x) * ((-Number(Integer 1I) + x**Number(Integer 2I))**(Number(Integer 1I)/Number(Integer 2I))))
                    | UnaryOp(ArcCsc,v) -> -Number(Integer 1I)/((Math.Pure.Structure.ExpressionFunction.abs x) * ((-Number(Integer 1I) + x**Number(Integer 2I))**(Number(Integer 1I)/Number(Integer 2I))))
                    | UnaryOp(ArcCot,v) -> Number(Integer 1I)/(Number(Integer 1I) - x**Number(Integer 2I))   
                    | _ -> BinaryOp(f,Derivative,x) //DERIV-7                 
                | v when freeOf v x -> Number (Integer 0I) //DERIV-6                                
                | UnaryOp(op,g) as f -> (derivativeOf (BinaryOp(f,Derivative,x)) x ) * (derivativeOf g x ) //chain rule
                | _ -> BinaryOp(u,Derivative,x) //DERIV-7
                   
    module Integral =

        let private one, two, four, six = Number(Integer 1I), Number(Integer 2I), Number(Integer 4I), Number(Integer 6I)
        
        let table u x = 
  
            match u with
            | Symbol (Variable v) as V when V = x -> Pass ((x**two)/two)
            | y when ExpressionStructure.freeOf y x -> Pass (y*x)
            | BinaryOp(Symbol(Variable y),ToThePowerOf,n) when n = (-one) -> Pass (UnaryOp(Ln,Symbol(Variable y)))
            | BinaryOp(Symbol(Variable y),ToThePowerOf,n) when ExpressionStructure.freeOf n x && n <> (-one) -> Pass ((one/(n+one))*Symbol(Variable y)**(n+one))
            | BinaryOp(a,ToThePowerOf,Symbol(Variable y)) when ExpressionStructure.freeOf a x -> Pass ((one/(UnaryOp(Ln,a)))*a**Symbol(Variable y))
            | UnaryOp(Ln,Symbol(Variable y)) -> Pass (Symbol(Variable y) * UnaryOp(Ln,Symbol(Variable y)) - Symbol(Variable y))
            | UnaryOp(Exp,Symbol(Variable y)) -> Pass (BinaryOp(Symbol (Constant E),ToThePowerOf,Symbol(Variable y)))
            | BinaryOp(Symbol (Constant E),ToThePowerOf,Symbol(Variable y)) -> Pass (BinaryOp(Symbol (Constant E),ToThePowerOf,Symbol(Variable y)))
            | UnaryOp(Sin,Symbol(Variable y)) -> Pass (-UnaryOp(Cos,Symbol(Variable y)))
            | UnaryOp(Cos,Symbol(Variable y)) -> Pass (UnaryOp(Sin,Symbol(Variable y)))
            | UnaryOp(Tan,Symbol(Variable y)) -> Pass (UnaryOp(Ln,UnaryOp(Abs,UnaryOp(Sec,Symbol(Variable y)))))
            | UnaryOp(Sec,Symbol(Variable y)) -> Pass (UnaryOp(Ln,UnaryOp(Abs,UnaryOp(Sec,Symbol(Variable y)) + UnaryOp(Tan,Symbol(Variable y)))))
            | BinaryOp(UnaryOp(Sec,Symbol(Variable y)),ToThePowerOf,two) -> Pass (UnaryOp(Tan,Symbol(Variable y)))
            | NaryOp(Product,[UnaryOp(Sec,x1);UnaryOp(Tan,x2)]) when x1 = x2 -> Pass (UnaryOp(Sec,x))
            | _ -> Fail (Symbol(Error OtherError))
        
        let rec indefinateEval f x =
            
            let F = table f x
            
            let linearProperties u x = 
                match u with
                | NaryOp(Product,xList) ->
                    let seperated = Factor.seperate u x
                    let free, dependant = fst seperated, snd seperated 
                    match free = one with
                    | true -> Fail (Symbol(Error OtherError))
                    | false -> 
                        let tryEval = indefinateEval dependant x
                        match tryEval with
                        | Pass result -> Pass (free * result)
                        | Fail error -> Fail error
                | NaryOp(Sum,xList) -> 
                    let xList' = List.map (fun w -> indefinateEval w x) xList
                    match List.exists (fun w -> w = Fail (Symbol(Error OtherError))) xList' with
                    | false -> 
                        let xList'' = List.map (fun (w : Result<Expression>) -> w.value) xList'
                        Pass (NaryOp(Sum, xList''))
                    | true -> Fail (Symbol(Error OtherError))
                | _ -> Fail (Symbol(Error OtherError))

            let trialSubstitutions u = 
                ExpressionStructure.subExpressions u 
                |> Seq.distinct
                |> Seq.takeWhile (fun x -> x <> u)
                |> Seq.toList 
            
            let substitutionMethod f x = 
                let p = trialSubstitutions f
                let out = 
                    List.tryPick (fun g -> 
                        match g <> x && ExpressionStructure.freeOf g x = false with
                        | false -> None
                        | true -> 
                            let globalV = Symbol (Variable "globalV")
                            let u = ExpressionStructure.substitute (g,globalV) (f/(Differential.derivativeOf g x))
                            match ExpressionStructure.freeOf u x with
                            | false -> None
                            | true -> 
                                match indefinateEval u globalV with
                                | Fail _ -> None
                                | Pass s -> Some (Pass (ExpressionStructure.substitute (globalV,g) ((indefinateEval u globalV).value)))                
                        ) p  
                match out with 
                | None -> Fail (Symbol(Error OtherError))
                | Some g -> g          
           
            let rationalReduction1 a b c n x = 
                let d = (b**two - four*a*c)
                let b2 = b**two
                let ac = four*a*c
                match n with
                | Number(Integer i) when i = 1I -> 
                    match ExpressionType.compareExpressions d ac with
                    | 1 -> -two * ((UnaryOp(ArcTanh,(two*a*x+b)/(d**(one/two))))/(d**(one/two)))
                    | 0 -> -two / (two*a*x + b)
                    | -1 -> two * ((UnaryOp(ArcTan,(two*a*x+b)/((four*a*c-b**two)**(one/two))))/((four*a*c-b**two)**(one/two)))
                    | _ -> Number Undefined
                | Number(Integer i) when i > 1I -> 
                    ((-(b+two*a*x))/((n-one)*d*(a*x**two+b*x*c)**(n-one))) + 
                    ((-(four*n-six)*a)/((n-one)*d*(indefinateEval (one/((a*x**two+b*x+c)**(n-one))) x).value))
                | _ -> Number Undefined

            let rationalReduction2 r s a b c n x = 
                match n with
                | Number(Integer i) when i = 1I -> 
                    (r/(two*a))*(UnaryOp(Ln,a*x**two + b*x + c )) + (s-r*b/(two*a)) * (rationalReduction1 a b c n x)
                | Number(Integer i) when i > 1I -> 
                    (-r / (two*(n-one)*a*((a*x**two+b*x+c)**(n-one)))) + ((-b*r + two*a*s) / two*a) * (rationalReduction1 a b c n x)
                | _ -> Number Undefined

            let rationalReduction3 g p j x = 
                let v = Symbol(Variable "v")
                let fX = indefinateEval g x
                let expansion = 
                    match fX with
                    | Fail e -> Fail e
                    | Pass xVal -> Pass (Expansion.ofGPE p xVal x v)
                match expansion with 
                | Fail e -> Fail e
                | Pass expansionValue -> 
                    match ExpressionStructure.freeOf expansionValue x with
                    | false -> Fail (Symbol(Error OtherError))
                    | true -> 
                        match Degree.ofGPE expansionValue [v] with
                        | Integer i when i = 1I -> 
                            match j with
                            | Number (Integer i) when i > 1I -> 
                                let out = -one / ((j-one)*(Coefficients.leadingGPE expansionValue v)*expansionValue**(j-one))
                                Pass(ExpressionStructure.substitute (v, fX.value) out)
                            | Number (Integer i) when i = 1I -> 
                                let out = (UnaryOp(Ln,expansionValue))/(Coefficients.leadingGPE expansionValue v)
                                Pass(ExpressionStructure.substitute (v, fX.value) out)
                            | _ -> Fail (Symbol(Error OtherError))
                        | Integer i when i = 2I -> 
                            let abc = Coefficients.listOfGPE expansionValue v
                            let out = (rationalReduction2 (Number (Integer 0I)) one abc.[2] abc.[1] abc.[0] j v)
                            Pass(ExpressionStructure.substitute (v, fX.value) out)
                        | _ -> Fail (Symbol(Error OtherError))

            let rationalReduction4 g p j x =
                let gD = Degree.ofGPE g [x]  
                let pD = Degree.ofGPE p [x]
                match g, gD, pD with
                | Number n, Integer a, Integer b when a = 0I && b = 1I -> 
                    match j with 
                    | Number (Integer i) when i > 1I -> Pass (-g / ((j-one)*(Coefficients.leadingGPE p x)*p**(j-one)))                            
                    | Number (Integer i) when i = 1I -> Pass (g*(UnaryOp(Ln,p))/(Coefficients.leadingGPE p x))
                    | _ -> Fail (Symbol(Error OtherError))
                | Number n, Integer a, Integer b when a = 0I && b = 2I -> 
                    let abc = Coefficients.listOfGPE p x
                    Pass (Number n*(rationalReduction1 abc.[2] abc.[1] abc.[0] j x))
                | _, Integer a, Integer b when a = 1I && b = 2I -> 
                    let abc = Coefficients.listOfGPE p x
                    let rs = Coefficients.listOfGPE g x
                    Pass (rationalReduction2 rs.[1] rs.[0] abc.[2] abc.[1] abc.[0] j x)
                | _, Integer a, Integer b when b > 2I -> rationalReduction3 g p j x
                | _ -> Fail (Symbol(Error OtherError))
            
            let rationalIntegrate f x = 
                let g = RationalExpression.numerator f
                let p = ExpressionType.Base (RationalExpression.denominator f)
                let j = ExpressionType.Exponent (RationalExpression.denominator f)
                match Check.isGRE f [x] with
                | false -> 
                    match Check.isPolynomialGPE p [x] with
                    | true -> rationalReduction4 g p j x
                    | false -> Fail (Symbol(Error OtherError))
                | true ->  rationalReduction4 g p j x
            
            match F with
            | Pass result as out -> out
            | Fail _ -> 
                match linearProperties f x with
                | Pass result as out -> out
                | Fail _ -> 
                    match substitutionMethod f x with
                    | Pass result as out -> out
                    | Fail _ -> 
                        let g = Expand.algebraicExpression f
                        let r = rationalIntegrate f x
                        match r with 
                        | Pass result as out -> out
                        | Fail _ ->
                            match g = f with
                            | false -> indefinateEval g x
                            | true -> rationalIntegrate g x




                