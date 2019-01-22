module Display

open Math.TypeExtensions
open Math.Foundations.Logic
open Math.Pure.Objects
open Math.Pure.Quantity
open Math.Pure.Structure
open Math.Pure.Space
open MathML
open MathML.Element

let mi x = Element.mi [] [x]

let getStrings (x:Expression) = 
        let eNumber acc (n:Expression) = 
            match n with 
            | Number (Real r) -> ( (mi r).openTag + r.ToString() + (mi r).closeTag )::acc 
            | Number (Integer i) -> ( (mi i).openTag + i.ToString() + (mi i).closeTag )::acc 
            | _ -> acc
        
        let eComplexNumber acc _ = acc
        let eSymbol acc (v:Expression) = 
            match v with 
            | Symbol (Variable v) -> v::acc 
            | Symbol (Constant c) -> c.symbol::acc 
            | _ -> acc
        let eBinaryOp acc _ = acc
        let eUnaryOp acc _ = acc
        let eNaryOp acc _ = acc
        let acc = []
        Cata.foldExpression eNumber eComplexNumber eSymbol eBinaryOp eUnaryOp eNaryOp acc x
        |> Seq.distinct
        |> Seq.toList