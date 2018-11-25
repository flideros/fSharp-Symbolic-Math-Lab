#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open FSharp.Data
open OpenMath
open Math.Foundations













/////
let list1 = ["a";"f";"g";"g"]//[]
let list2 = ["g";"d";"s"]//[]
let set1 :Set<string> = Set ["a";"f";"g";"g"]//[]
let set2 :Set<string> = Set ["g";"d";"s"]//[] 

let intersectSet = Logic.Set.Intersection.oF set1 set2
let intersectList = Logic.Set.Intersection.oFList list1 list2
let unionSet = Logic.Set.Union.oF set1 set2
let unionList = Logic.Set.Union.oFList list1 list2
let diffSet = Logic.Set.Difference.oF set1 set2
let diffList = Logic.Set.Difference.oFList list1 list2

/////
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

let EmptySet = Logic.Set.EmptySet.getDefinition.Value
R.Description

