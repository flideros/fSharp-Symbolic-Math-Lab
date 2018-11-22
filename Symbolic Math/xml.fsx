#r @"C:\Users\flideros\Documents\Visual Studio 2017\Projects\SymbolicMath\packages\FSharp.Data.3.0.0\lib\net45\FSharp.Data.dll"
#r @"C:\Users\flideros\AppData\Local\assembly\dl3\T66OZJRL.NYY\QZ4BGQTK.WBJ\aa1497d2\f7d70ce8_2839d301\System.Xml.Linq.dll"
#r "C:/Users/flideros/Documents/Visual Studio 2017/Projects/SymbolicMath/SymbolicMath/bin/Debug/SymbolicMath.dll"
open FSharp.Data
open OpenMath
open Math.Foundations





let R = Logic.Set.R.getDefinition.Value
R.Description


let h = GET.cD "set1"

h.CdBase


GET.cDFile h


let cds = ["alg1";"altenc";"arith1";"bigfloat1";"calculus1";"complex1";"error";"fns1";"fns2";"integer1";"interval1";"limit1";"linalg1";"linalg2";"list1";"logic1";"mathmlattr";"mathmltypes";"meta";"metagrp";"metasig";"minmax1";"multiset1";"nums1";"piece1";"quant1";"relation1";"relation3";"rounding1";"s_data1";"s_dist1";"set1";"setname1";"sts";"transc1";"veccalc1"]

GET.cDFiles cds