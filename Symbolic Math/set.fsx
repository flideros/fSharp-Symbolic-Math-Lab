#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open FSharp.Data
open OpenMath
open Math.Foundations
open Math.Pure.Objects
open Math.Foundations.Logic
open Math.Pure.Quantity

E.symbol

PositiveInfinity.definition

Set.MuliIntersection.definition

Set.Union.definition


let seqInfinite = Seq.initInfinite (fun index ->
    let n = float ( index + 1 )
    1.0 / (n * n * (if ((index + 1) % 2 = 0) then 1.0 else -1.0)))
printfn "%A" seqInfinite

Set.Size.oFSequence seqInfinite
///
RealNumbers.definition

/////
let list1 = [1;2;3;7;77;6;67;75]
let list2 = [3;2;4;7;6;55;66]
let list3 = [5;2;6;4;77;55;7]
let set1 :Set<int> = Set.ofList [1;2;3;7;77;6;67;75]
let set2 :Set<int> = Set.ofList [3;2;4;7;6;55;66]
let set3 :Set<int> = Set.ofList [5;2;6;4;77;55;7] 


let ll = Logic.Set.Difference.oFList list2 list3

let l = Logic.Set.Difference.oF set2 set3




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





let ocd = "set1"
let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
printfn "%O" d

