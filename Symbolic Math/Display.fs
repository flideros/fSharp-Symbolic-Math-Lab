module Display

open Math.TypeExtensions
open Math.Foundations.Logic
open Math.Pure.Objects
open Math.Pure.Quantity
open Math.Pure.Structure
open Math.Pure.Space
open MathML
open MathML.Element

let mi = Element.mi []
let mn = Element.mn [] 
let mo = Element.mo [] 

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
        let eUnaryOp _ = ""
        let eNaryOp _ = ""
        let initialGenerator = fun t -> t
        Cata.foldbackExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp initialGenerator x