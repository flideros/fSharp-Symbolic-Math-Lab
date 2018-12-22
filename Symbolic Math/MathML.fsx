#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open MathML


let ggg = Number 5.0
let rrr = EM 5.0<em>
let ttt = Attr.accent true 

match rrr with
| EM n -> n.ToString() + "em"
| Number n -> n.ToString()

let _s =  [ColumnAlign.Center; ColumnAlign.Left]