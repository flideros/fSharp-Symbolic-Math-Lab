#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open FSharp.Data
open OpenMath
open Math.Foundations
open Math.Pure.Objects


And.defenition
Abs.defenition
Function.Argument .defenition
let ocd = "set1"

let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")

printfn "%O" d



let ooo = GET.cD "setname1"
ooo.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\"  + ooo.CdName + ".ocd")



let h = GET.cD "set1"
let hh = h.CdDefinitions |> Array.collect (fun x -> [|x.Name,x.Role|])




let cds = ["alg1";"altenc";"arith1";"bigfloat1";"calculus1";"complex1";
           "error";"fns1";"fns2";"integer1";"interval1";"limit1";"linalg1";
           "linalg2";"list1";"logic1";"mathmlattr";"mathmltypes";"meta";
           "metagrp";"metasig";"minmax1";"multiset1";"nums1";"piece1";"quant1"
           ;"relation1";"relation3";"rounding1";"s_data1";"s_dist1";"set1";
           "setname1";"sts";"transc1";"veccalc1"]




let Con = (List.map (fun x -> let cd = (GET.cD x) 
                              cd.CdDefinitions
                              |> Array.collect (fun x -> match x.Role = Some "application" with
                                                         | true -> [|x.Name,cd.CdName|]
                                                         | false -> [||])
                              
                              ) cds) |> Seq.distinct |> Seq.toList
