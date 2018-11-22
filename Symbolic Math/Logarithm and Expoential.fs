namespace Math.Pure.Structure

open Math.Pure.Structure.ExpressionStructure
open Math.Pure.Objects
open Math.Pure.Quantity


module Expoential =

    [<RequireQualifiedAccess>]
    module Expand =

        let rec private expandRules A = 
            match A with
            | NaryOp(Sum,aList) -> 
                let f = operand A 1
                (expandRules f)*(expandRules (A-f))
            | NaryOp(Product,aList) -> 
                let f = operand A 1
                match f with
                | Number (Integer i) -> (expandRules (A/f))**f
                | _ -> UnaryOp(Exp,A)
            | _ -> UnaryOp(Exp,A)

        let rec expoential u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b) -> ComplexNumber (expoential a, expoential b)
            | BinaryOp (a,op,b) -> BinaryOp (expoential a,op, expoential b)
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> expoential x) aList)        
            | UnaryOp(Exp,a) -> expandRules (expoential (Polynomial.Expand.algebraicExpression a))
            | UnaryOp(op,a) -> UnaryOp(op,expoential a)
            |> ExpressionType.simplifyExpression
    
    [<RequireQualifiedAccess>]
    module Contract =

        let rec private contractionRules u = 
            let v = Polynomial.Expand.mainOpOf u
            match v with
            | BinaryOp (b,ToThePowerOf,s) -> 
                match b with
                | UnaryOp(Exp,a) ->
                    let p = a*s 
                    match p with
                    | NaryOp (Product,aList) -> UnaryOp(Exp,contractionRules p)
                    | BinaryOp (a,ToThePowerOf,b) -> UnaryOp(Exp,contractionRules p)
                    | _ -> UnaryOp(Exp,p)
                | _ -> v
            | NaryOp (Product,aList) -> 
                let yTup = List.fold ( fun acc y ->
                    match y with
                    | UnaryOp(Exp,a) -> ((fst acc),(snd acc) + a)
                    | _ -> ((fst acc) * y,(snd acc))) (Number (Integer 1I),Number (Integer 0I))aList
                UnaryOp(Exp,(snd yTup)) * (fst yTup)
            | NaryOp (Sum,aList) -> 
                 List.fold ( fun acc y ->
                    match y with
                    | NaryOp (Product,aList) -> acc + (contractionRules y)
                    | BinaryOp (b,ToThePowerOf,s) -> acc + (contractionRules y)
                    | _ -> acc + y) (Number (Integer 0I)) aList
            | _ -> v
            |> ExpressionType.simplifyExpression

        let rec expoential u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b) -> ComplexNumber (expoential a, expoential b)
            | BinaryOp (a,ToThePowerOf,b) -> contractionRules (BinaryOp (expoential a,ToThePowerOf, expoential b))
            | BinaryOp (a,op,b) -> BinaryOp (expoential a,op, expoential b)            
            | NaryOp (Product,aList) -> contractionRules (NaryOp (Product,List.map (fun x -> expoential x) aList))
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> expoential x) aList)        
            | UnaryOp(op,a) -> UnaryOp(op,expoential a)
            |> ExpressionType.simplifyExpression

module Logarithm =
    
    [<RequireQualifiedAccess>]
    module Expand =

        let rec private expandRules A = 
            match A with
            | BinaryOp(a,ToThePowerOf,b) -> b * (expandRules a)       
            | NaryOp(Product,aList) -> 
                let f = operand A 1
                (expandRules f) + (expandRules (A/f))
            | _ -> UnaryOp(Ln,A)

        let rec logarithm u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b) -> ComplexNumber (logarithm a, logarithm b)
            | BinaryOp (a,op,b) -> BinaryOp (logarithm a,op, logarithm b)
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> logarithm x) aList)        
            | UnaryOp(Ln,a) -> expandRules (logarithm (Polynomial.Expand.algebraicExpression a))
            | UnaryOp(op,a) -> UnaryOp(op,logarithm a)
            |> ExpressionType.simplifyExpression

    [<RequireQualifiedAccess>]
    module Contract =
 
        let rec private contractionRules u = 
            let v = Polynomial.Expand.mainOpOf u
            match v with            
            | NaryOp (Product,aList) -> 
                match ExpressionType.isNumber aList.Head with
                | false -> u
                | true -> 
                    let isLn x = // Move this Function to it's own module
                        match x with 
                        | UnaryOp(Ln,a) -> true
                        | _ -> false
                    match List.tryFindIndex isLn aList with
                    | None -> u
                    | Some value -> 
                        match aList.[value] with
                        | UnaryOp(Ln,x) -> (NaryOp(Product,(List.map (fun x -> contractionRules x) aList.Tail)) / aList.[value]) * UnaryOp(Ln,(contractionRules x)**aList.Head)
                        | _ -> u
            | NaryOp (Sum,aList) -> 
                let isLn x = // Move this Function to it's own module
                    match x with 
                    | UnaryOp(Ln,a) -> true
                    | _ -> false
                let aList' = List.map (fun x -> contractionRules x) aList
                let ln, noLn = List.partition (fun x -> isLn x) aList'
                let lnOut = List.fold (fun acc x -> match x with | UnaryOp(Ln,a) -> acc*(contractionRules a) | _ -> acc) (Number(Integer 1I)) ln
                ((NaryOp (Sum,noLn)) + UnaryOp(Ln,lnOut))
                 
            | _ -> v
            |> ExpressionType.simplifyExpression
 
        let rec logarithm u = 
            match u with
            | Number n -> u
            | Symbol s -> u
            | ComplexNumber (a,b) -> ComplexNumber (logarithm a, logarithm b)
            | BinaryOp (a,ToThePowerOf,b) -> (BinaryOp (logarithm a,ToThePowerOf, logarithm b))
            | BinaryOp (a,op,b) -> BinaryOp (logarithm a,op, logarithm b)            
            | NaryOp (Product,aList) -> contractionRules u
            | NaryOp (Sum,aList) -> contractionRules u            
            | NaryOp (op,aList) -> NaryOp (op,List.map (fun x -> logarithm x) aList)        
            | UnaryOp(op,a) -> UnaryOp(op,logarithm a)
            |> ExpressionType.simplifyExpression
