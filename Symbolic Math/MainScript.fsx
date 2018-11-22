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
let printEuc a = match a with | (a, b, c) -> sprintf "(%s, %s, %s)" (Print.expression a) (Print.expression b) (Print.expression c)

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
        Number(Integer 8I)

let v = x5 +
        Number(Integer 7I)*x4 +
        Number(Integer 17I)*x3 + 
        Number(Integer 17I)*x2 +
        Number(Integer 7I)*x +
        Number(Integer 1I)


let _out = squareFree u x

Print.expression _out

#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
open FSharp.Data
open OpenMath
open Math.Foundations


let zz = Logic.Set.C


printfn "%O" zz.getDefinition
