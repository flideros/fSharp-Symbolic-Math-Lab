#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open FSharp.Data
open OpenMath
open Math.Foundations





Logic.Set.Union.definition
Logic.Set.Difference.definition
Logic.Set.Intersection.definition


Logic.Set.definition

Logic.Set.createTheUniverse



let ocd = "set1"
let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
printfn "%O" d

let R = Logic.Set.R.getDefinition.Value
R.Description


