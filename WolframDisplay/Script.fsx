#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#r @"D:\MyFolders\Desktop\SymbolicMath\packages\Wolfram.NETLink.1.7.1\lib\net461\Wolfram.NETLink.dll"
#r @"D:\MyFolders\Desktop\SymbolicMath\Symbolic Math UI\bin\Debug\WolframDisplay.dll"

open System
open System.IO
open System.Windows
open System.Windows.Controls
open System.Windows.Media
open Wolfram.NETLink
open Math.Presentation.WolframEngine

open TrussDomain
open TrussImplementation

let x0,x1,x2,x3,x4 = X 0., X 1., X 2., X 3., X 4.
let y0,y1,y2,y3,y4 = Y 0., Y 1., Y 2., Y 3., Y 4.
let z0,z1,z2,z3,z4 = Z 0., Z 1., Z 2., Z 3., Z 4.

let j1 = {x=x0;y=y0}
let j2 = {x=x0;y=y1}
let j3 = {x=x1;y=y0}
let j4 = {x=x1;y=y1}
let j5 = {x=x2;y=y0}
let j6 = {x=x2;y=y1}

let m1 = j1,j2
let m2 = j1,j3
let m3 = j2,j4
let m4 = j1,j4
let m5 = j4,j3
let m6 = j3,j5
let m7 = j4,j5
let m8 = j4,j6
let m9 = j5,j6

let f1 = {magnitude = 1.; direction = Vector (x=1.,y=1.); joint = j1}
let f2 = {magnitude = 1.; direction = Vector (x=1.,y=0.); joint = j1}
let f3 = {magnitude = 1.; direction = Vector (x=1.,y=1.); joint = j3}
let f4 = {magnitude = 1.; direction = Vector (x=1.,y = -2.); joint = j3}
let f5 = {magnitude = 1.; direction = Vector (x=0.,y = 2.); joint = j5}

let s1,s2,s3,s4 = Pin (f1,f2), Roller f1,  Roller f3, Roller f5

let mList = [m1;m2;m3;m4;m5;m6;m7;m8;m9]
let fList = [f4]
let sList = [s1;s4]

let truss = {members=mList;forces=fList;supports=sList}

checkTrussStability truss

getReactionForcesFrom sList |> getDirectionsFrom








//////////////////////////////////////////////////////

System.Media.SystemSounds.Asterisk .Play()
//Example:
let code = "Circle[{3.4,7.5},45.2]"
let sp = code.Split(',') 
let px = float ( sp.[0].Replace("Circle[{","") )
let py = float ( sp.[1].Replace("}","") )
let r = float ( sp.[2].Replace("]","") )
sp.Length

Install()

// I had to add ml64i4.dll to the same folder as the NETLink DLL 
// to get the Wolfram.NETLink to work
//..\SymbolicMath\packages\Wolfram.NETLink.1.7.1\lib\net461\ml64i4.dll"


let cmdLine = "-linkmode launch -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.2/MathKernel.exe\""
// This launches the Mathematica kernel:
let _iKernelLink = Wolfram.NETLink.MathLinkFactory.CreateKernelLink(cmdLine);
// Discard the initial InputNamePacket the kernel will send when launched.
_iKernelLink.WaitAndDiscardAnswer();

// 
_iKernelLink.Evaluate("2+2")
_iKernelLink.WaitForAnswer()
_iKernelLink.GetInteger()



// simple computation 
let result = _iKernelLink.EvaluateToOutputForm("5+2", 0)
 // or
_iKernelLink.Evaluate("2+2")
_iKernelLink.WaitForAnswer()
_iKernelLink.GetInteger()

// using methods from IMathLink:
_iKernelLink.PutFunction("EvaluatePacket", 1)
_iKernelLink.PutFunction("Minus", 2)
_iKernelLink.Put(65)
_iKernelLink.Put(2)
_iKernelLink.EndPacket()
_iKernelLink.WaitForAnswer()
_iKernelLink.GetInteger()

// Always Close link when done
_iKernelLink.Close()