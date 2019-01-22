//#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
open Math.TypeExtensions
open Math.Foundations.Logic
open Math.Pure.Objects
open Math.Pure.Quantity
open Math.Pure.Structure
open Math.Pure.Space.Trigonometry
open Math.Pure.Structure.Polynomial
open Math.Pure.Structure.Logarithm
open Math.Pure.Structure.Expoential
open Math.Pure.Structure.NumberTheory
open Math.Pure.Structure.Factorization
let a,b,c,d,e,f = Symbol (Variable "a"),Symbol (Variable "b"),Symbol (Variable "c"),Symbol (Variable "d"),Symbol (Variable "e"),Symbol (Variable "f")
let t,w,x,y,z = Symbol (Variable "t"),Symbol (Variable "w"),Symbol (Variable "x"),Symbol (Variable "y"),Symbol (Variable "z")
let zero,one,two,three,four,five,six,seven,eight,nine,ten = Number (Integer 0I),Number (Integer 1I),Number (Integer 2I),Number (Integer 3I),Number (Integer 4I),Number (Integer 5I),Number (Integer 6I),Number (Integer 7I),Number (Integer 8I),Number (Integer 9I),Number (Integer 10I)
let pi,i = Symbol (Constant Pi),Symbol (Constant I)
let sin(u) = UnaryOp(Sin,u)
let cos(u) = UnaryOp(Cos,u)
//let printEuc a = match a with | (a, b, c) -> sprintf "(%s, %s, %s)" (Print.expression a) (Print.expression b) (Print.expression c)

let x2 = x**two 
let x3 = x**three
let x4 = x**four
let x5 = x**five
let y2 = y**two 
let y3 = y**three
let y4 = y**four
let y5 = y**five
let u = Number(Integer 1I)*x4 +
        Number(Integer 7I)*x3 + 
        Number(Integer 18I)*x2 +
        Number(Integer 202I)*x +
        Number(Real 8.7)

let v = x5 +
        Number(Integer 7I)*x4 +
        Number(Integer 17I)*x3 + 
        Number(Integer 17I)*x2 +
        Number(Integer 7I)*x +
        Number(Integer 1I)

let _out = squareFree u x

//Print.expression _out

#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
open FSharp.Data
open OpenMath
open Math.Foundations
open MathML
open MathML.Element

let zz = Logic.Set.Difference.definition

//-----------display-----------//

let mi x = Element.mi [] [x]
let mn x = Element.mn [] [x]
let mo x = Element.mo [] [x]

let rec getStringsFrom (x:Expression) = 
        let eNumber acc (n:Expression) = 
            match n with 
            | Number (Real r) -> ( (mn r).openTag + r.ToString() + (mn r).closeTag )::acc 
            | Number (Integer i) -> ( (mn i).openTag + i.ToString() + (mn i).closeTag )::acc 
            | _ -> acc
        let eComplexNumber acc _ = acc
        let eSymbol acc (v:Expression) = 
            match v with 
            | Symbol (Variable v) -> ( (mi v).openTag + v + (mi v).closeTag )::acc 
            | Symbol (Constant c) -> ( (mi c).openTag + c.symbol + (mi c).closeTag )::acc 
            | _ -> acc
        let eBinaryOp acc (bOpp:Expression)  = 
            match bOpp with
            | BinaryOp (a,Plus,b) -> //((mo Plus).openTag + " + " + (mo Plus).closeTag)::acc
                let a' = match (getStringsFrom a) with | a::[] -> a | _ -> ""
                let b' =  match (getStringsFrom b) with | b::[] -> b | _ -> ""
                let plus = (mo Plus).openTag + " + " + (mo Plus).closeTag
                [(a' + plus + b')]//::acc

            | _ -> acc
        let eUnaryOp acc _ = acc
        let eNaryOp acc _ = acc
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp [] x
        //|> Seq.rev
        //|> Seq.distinct
        //|> Seq.rev
        |> Seq.toList


getStringsFrom (BinaryOp(a,Plus,two))

let elem n = match n with
             | Number (NumberType.Real r) -> element (Token Mi) [] [r]


let math = element Math [] [elem]



let printExpression n = 
    match n with
    | Number (NumberType.Real r) -> 
        use file = System.IO.File.CreateText("D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\output\content\Test.md")
        fprintf file "<mn>%f</mn>" r



printExpression (Number (Real 232.33225))


let printNumbersToFile fileName =
   use file = System.IO.File.CreateText(fileName)
   let x = [1;2;3;4;5]
   x
   |> List.iter (fun elem -> fprintf file "%d " elem)
    
printNumbersToFile "D:\MyFolders\Desktop\Test.csv"


I.symbol
