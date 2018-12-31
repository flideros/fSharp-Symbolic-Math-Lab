#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open MathML


let ggg = Number 5.0
let rrr = EM 5.0<em>
let ttt =  Accent  true

match rrr with
| EM n -> n.ToString() + "em"
| Number n -> n.ToString()

let defaultAttributes = [MathColor "black"; MathBackground "white"; MathVariant Normal; Id ""; Xref ""; Class ""; Style ""; Href ""]
let altAttributes = [MathBackground "yellow"; Position 1; Xref "bbb";]

let isValidElementAttributeOf defaultAttrs attr = List.exists (fun elem -> elem.GetType() = attr.GetType()) defaultAttrs

let scrub l = List.choose (fun elem ->
    match elem with
    | elem when isValidElementAttributeOf defaultAttributes elem -> Option.Some elem
    | _ -> Option.None) l


scrub altAttributes


//Test
isValidElementAttributeOf defaultAttributes (MathBackground "yellow")















rrr.GetType()