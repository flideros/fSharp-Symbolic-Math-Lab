#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r "System.Xml.Linq.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\OpenMath\bin\Debug\netstandard2.0\OpenMath.dll"
#r @"D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Symbolic Math\bin\Debug\Symbolic_Math.dll"
open FSharp.Data
open OpenMath
open Math.Foundations


let z = Logic.Set.C

let zz = z.getDefinition
zz
(OpenMath.GET.cD "setname1").CdDefinitions |> Array.tryFind (fun x -> x.Name = "C")

let CDgroups =  ["algstr1"]

let CDs =  ["veccalc1";
            "transc1";
            "sts";
            "setname1";
            "set1";
            "s_dist1";
            "s_data1";
            "rounding1";
            "relation3";
            "relation1";
            "quant1";
            "piece1";
            "physical_consts1";
            "nums1";
            "multiset1";
            "minmax1";
            "metasig";
            "metagrp";
            "meta";
            "mathmltypes";
            "mathmlattr";
            "logic1";
            "list1";
            "linalg2";
            "linalg1";
            "limit1";
            "interval1";
            "integer1";
            "fns2";
            "fns1";
            "error";
            "complex1";
            "calculus1";
            "bigfloat1";
            "arith1";
            "altenc";
            "alg1"]

let alg1s = GET.cDSignature "alg1"

alg1s.Signatures.[0].Name

let CDGroup = GET.cDGroup "algstr1"

let alg1 = GET.cD "alg1"
let physical_consts1 = GET.cD "physical_consts1"
let arith1 = GET.cD "arith1"


alg1.CdDefinitions
printfn "%O" alg1.CdDefinitions.[0].Fmps

arith1.CdDefinitions |> Array.map (fun ds -> ds.Name)
                         