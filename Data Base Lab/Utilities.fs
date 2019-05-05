﻿module internal Utilities

open System.Windows
open System.Windows.Controls
open System.Reflection

open System.IO
open System.Windows.Markup

/// Use this implementation of the dynamic binding operator
    /// to bind to Xaml components in code-behind, see example below
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
    
    
