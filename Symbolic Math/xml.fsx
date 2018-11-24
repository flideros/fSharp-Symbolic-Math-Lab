#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
#r @"FSharp.Core"

open FSharp.Data
open OpenMath
open Math.Foundations


Logic.Set.createTheUniverse








Logic.Set.createTheUniverse



let ocd = "set1"
let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
printfn "%O" d

let R = Logic.Set.R.getDefinition.Value
R.Description


let h = GET.cD "set1"

h.CdBase


GET.cDFile h


let cds = ["alg1";"altenc";"arith1";"bigfloat1";"calculus1";"complex1";
           "error";"fns1";"fns2";"integer1";"interval1";"limit1";"linalg1";
           "linalg2";"list1";"logic1";"mathmlattr";"mathmltypes";"meta";
           "metagrp";"metasig";"minmax1";"multiset1";"nums1";"piece1";"quant1"
           ;"relation1";"relation3";"rounding1";"s_data1";"s_dist1";"set1";
           "setname1";"sts";"transc1";"veccalc1"]

GET.cDFiles cds



let cD ocd = let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
             OpenMathCD.Load(d)

let _cD ocd = let d = "http://www.openmath.org/cd/" + ocd + ".ocd"
              OpenMathCD.Load(d)
let ooo = _cD "setname1"


ooo.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\"  + ooo.CdName + ".ocd")