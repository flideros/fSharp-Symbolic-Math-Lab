// This modul must be internal and must exist in all assemblies.

module internal Utilities

open System.Windows
open System.Windows.Controls
open System.Reflection

open System.IO
open System.Windows.Markup

// This operator is similar to (|>). 
// But, it returns argument as a return value.
// Then you can chain functions which returns unit.
// http://fssnip.net/9q

let ($) x f = f x ; x

let (?) (c:obj) (s:string) =
    match c with
    | :? ResourceDictionary as r ->  r.[s] :?> 'T
    | :? Control as c -> c.FindName(s) :?> 'T
    | _ -> failwith "dynamic lookup failed"

// strXamlName - MUST be Embedded Resource  - this function must be in same Assembly
// XAML - MUST be Embedded Resource  ("use just xaml file name" as parameter)  
let contentAsXamlObject (strXamlName : string) =   
    let res = Assembly.GetExecutingAssembly().GetManifestResourceNames() |> Array.filter(fun x -> x.ToLower().IndexOf(strXamlName.ToLower()) > -1)
    match res.Length = 1 with
    | true -> let mySr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(res.[0]))
              XamlReader.Load(mySr.BaseStream)
    | false -> null

// helper to make defaultArg better for piping
let ifNone defaultValue input = 
    // just reverse the parameters!
    defaultArg input defaultValue


        
[<AutoOpen>]
type MaybeBuilder() =
        member this.Bind(x, f) = Option.bind f x
        member this.Return(x) = Some x
   
    let maybe = new MaybeBuilder()