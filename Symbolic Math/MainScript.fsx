//#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
open FSharp.Data
open OpenMath
open Math.Foundations
open MathML
open MathML.Element
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
(EM 2.0<em>)
MathSize (EM 2.0<em>)
let mi = Element.mi []
let mn = Element.mn  []
let mo = Element.mo  []

let getStringsFrom (x:Expression) = 
        let eNumber (n:Expression) = 
            match n with 
            | Number (Real r) -> ( mn.openTag + r.ToString() + mn.closeTag )
            | Number (Integer i) -> ( mn.openTag + i.ToString() + mn.closeTag )
            | _ ->  ""
        let eComplexNumber  _ = ""
        let eSymbol (v:Expression) = 
            match v with 
            | Symbol (Variable v) -> ( mi.openTag + v + mi.closeTag )
            | Symbol (Constant c) -> ( mi.openTag + c.symbol + mi.closeTag )
            | _ -> ""
        let eBinaryOp (aText, op, bText)  = 
            match op with
            | Plus -> (aText + (mo.openTag + " + " + mo.closeTag) + bText)
            | Equals -> (aText + (mo.openTag + " = " + mo.closeTag) + bText)
        let eUnaryOp _ = ""
        let eNaryOp _ = ""
        let initialGenerator = fun t -> t
        Cata.foldbackExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp initialGenerator x

getStringsFrom (BinaryOp((BinaryOp(a,Plus,two),Equals,four)))

let elem n = match n with
             | Number (NumberType.Real r) -> element (Token Mi) [] 




// PRINT
(*let printExpression n = 
    match n with
    | Number (NumberType.Real r) -> 
        use file = System.IO.File.CreateText("D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\output\content\Test.md")
        fprintf file "<mn>%f</mn>" r*)
let printExpression n =    
        //let r = (getStringsFrom n)
        use file = System.IO.File.CreateText("D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\output\content\Test.md")
        fprintf file "<math>%s</math>" (getStringsFrom n)


printExpression (BinaryOp((BinaryOp(a,Plus,two),Equals,five + nine)))


let printNumbersToFile fileName =
   use file = System.IO.File.CreateText(fileName)
   let x = [1;2;3;4;5]
   x
   |> List.iter (fun elem -> fprintf file "%d " elem)
    
printNumbersToFile "D:\MyFolders\Desktop\Test.csv"


I.symbol

let ff = MathMLElement.Token Mi

ff.ToString().ToLower()


let hhh = Id "none"
let vvv = Accent true


(hhh.GetType().Name.ToLowerInvariant() + hhh.ToString().Replace(hhh.GetType().Name + " "," = \"") + "\"").ToString().Replace("\"\"", "\"")
(vvv.GetType().Name.ToLowerInvariant() + vvv.ToString().Replace(vvv.GetType().Name + " "," = \"") + "\"").ToString().Replace("\"\"", "\"")

let getString (x:MathMLAttribute) = (x.GetType().Name.ToLowerInvariant() + x.ToString().Replace(x.GetType().Name + " "," = \"") + "\"").ToString().Replace("\"\"", "\"")

getString vvv







