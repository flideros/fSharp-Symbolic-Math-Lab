//#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\Desktop\SymbolicMath\Symbolic Math\bin\Debug\Symbolic_Math.dll"
//#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
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
let pi,i = Symbol (Constant Pi),Symbol (Constant E)
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

let expression = five/(sin(pi*((Number(Real 0.30))*x)**(Number(Real 2.0))))/i //(sin(x**two))**(Number(Real 0.5)) //

Math.Pure.Change.Calculus.Differential.derivativeOf (x2+five) x

let rec checkForRealPowersOfExpression exp =
    let subExp = ExpressionStructure.subExpressions exp 
    match (List.exists (fun x -> (ExpressionStructure.kind x) = "Real Power") subExp) with
    | false -> exp
    | true -> ExpressionFunction.evaluateRealPowersOfExpression exp |> checkForRealPowersOfExpression

ExpressionStructure.subExpressions expression

expression
|> ExpressionStructure.substitute (Expression.Symbol (Constant Pi), Number (Real (System.Math.PI))) 
|> ExpressionStructure.substitute (Expression.Symbol (Constant E), Number (Real (System.Math.E)))            
|> ExpressionStructure.substitute (x, (Number(Real 10.10))) 
|> checkForRealPowersOfExpression

|> ExpressionFunction.evaluateRealPowersOfExpression
|> ExpressionFunction.evaluateRealPowersOfExpression
|> ExpressionType.simplifyExpression

let evaluate expression xValue = 
    ExpressionStructure.substitute (x, xValue) expression      
    |> ExpressionFunction.evaluateRealPowersOfExpression
    |> ExpressionType.simplifyExpression
    
    |> ExpressionType.simplifyExpression

evaluate expression (Number(Real 2.0))

let xCoordinates = seq {for x in -150. .. 0.1.. 150. -> Number(Real x)}
let yCoordinates = Seq.map (fun x -> evaluate expression x ) xCoordinates
let coordinatePairs = Seq.zip xCoordinates yCoordinates |> Seq.filter (fun (_x,y) -> match y with | Number(Real r) when System.Double.IsNaN(r) = true -> false | _ -> true)
       
let partitionInfinity  = 
    let rec loop acc lcc = function
        | (Number(Real x),Number(Real y))::pl when y <> infinity && y <> -infinity -> loop ((Number(Real x),Number(Real y))::acc) lcc pl
        | [] -> acc::lcc //, []
        | pl -> 
            let infinityPoint = 
                match pl,acc with
                | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = infinity && y1 <= 0. -> (x0,Number(Real -150.))
                | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 <= 0. -> (x0,Number(Real -151.))
                | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = infinity && y1 >= 0. -> (x0,Number(Real 152.))
                | (x0,Number(Real y0))::_plTail,(_x1,Number(Real y1))::_accTail when y0 = -infinity && y1 >= 0. -> (x0,Number(Real 153.))                     
                | [],acc -> (fst acc.Head, Number(Real 154.))                        
                | _ -> (fst pl.Head, Number(Real 155.))
            let p =
                  match pl with
                  | (x0,Number(Real _y0))::(x1,Number(Real y1))::_t when y1 <= 0. -> (x0,Number(Real -156.))
                  | (x0,Number(Real _y0))::(x1,Number(Real y1))::_t when y1 > 0.  -> (x0,Number(Real 157.))
                  | _ -> (fst pl.Head, Number(Real 158.))
            loop [p] ((List.rev (infinityPoint::acc))::lcc) pl.Tail             
    loop [] []


let cList = (Seq.toList coordinatePairs) |> partitionInfinity



List.iter (fun x -> printf "%A \n" x) (cList.[0])




ExpressionFunction.evaluateRealPowersOfExpression ((Number (Real 25.)*Number (Real 25.))**Number (Real 0.5))

25.**0.5

List.iter (printfn "%A") [for x in -150.0..0.1..150.0 -> 1.0/x]

(Number(Real -150.0)) / (Number(Real 0.0))

let v = x5 +
        Number(Integer 7I)*x4 +
        Number(Integer 17I)*x3 + 
        Number(Integer 17I)*x2 +
        Number(Integer 7I)*x +
        Number(Integer 1I)

let _out = ExpressionStructure.substitute (x, Number(Real 1.4)) v |> ExpressionFunction.evaluateRealPowersOfExpression |> ExpressionType.simplifyExpression
Polynomial.Variables.ofExpression v

let _u = (Number(Integer 2I)**Number(Real 1.3)) |> ExpressionType.simplifyExpression 


let expressionVariables = Polynomial.Variables.ofExpression v
let check = Polynomial.Check.isPolynomialSV v

let evaluate2 expression = 
    match check with
    | false -> Number Undefined
    | true -> ExpressionStructure.substitute (x, Number(Real 1.4)) expression |> ExpressionFunction.evaluateRealPowersOfExpression |> ExpressionType.simplifyExpression

seq {for x in  50. .. 0.2 .. 60. -> Number (Real x) }


let u = Number(Integer 54233480688657908494580122963258952897654000350692006139111119134831511204958711I)


#time
Integer.factorByTrialDivision u
#time








//Print.expression _out

#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
open FSharp.Data
open OpenMath
open Math.Foundations
open MathML
open MathML.Element

//let zz = Logic.Set.Difference.definition

type UnicodeG = XmlProvider<"D:/MyFolders/Desktop/ucd.nounihan.grouped/ucd.nounihan.grouped.xml">

let dataG = UnicodeG.Load("D:/MyFolders/Desktop/ucd.nounihan.grouped/ucd.nounihan.grouped.xml")




//printf "%A" chars

let c2 = dataG.Repertoire.Groups |> Array.map (fun ds -> ds.Blk) |> Array.iter (fun x -> printfn "%A" x )
let c3 = dataG.Repertoire.Groups |> Array.choose (fun ds -> match ds.Blk with  
                                                            | Some x when x = "Playing_Cards" -> Some ds | _ -> Option.None)
                                 |> Array.map (fun x -> x.Chars) |> Array.concat 
c3.Length

printfn "%A" c2

dataG.Repertoire.Groups.[21].Blk


//-----------display-----------//



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
            | Plus -> (aText + (mo.openTag + " + " + mo.closeTag) + bText) //placeholder
            | Minus -> (aText + (mo.openTag + " - " + mo.closeTag) + bText) //placeholder
            | Times -> (aText + (mo.openTag + " * " + mo.closeTag) + bText) //placeholder
            | DividedBy -> (aText + (mo.openTag + " / " + mo.closeTag) + bText) //placeholder
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








