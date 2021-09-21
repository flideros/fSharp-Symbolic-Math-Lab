﻿#r @"PresentationCore"
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

let x0,x1,x2 = TrussDomain.X 0., TrussDomain.X 3., TrussDomain.X 6.
let y0,y1 = TrussDomain.Y 4., TrussDomain.Y 0.

//truss1   
let x3,x4,x5 = TrussDomain.X 0., TrussDomain.X 4.,TrussDomain.X 8.
let y2,y3 = TrussDomain.Y 5., TrussDomain.Y 0.
let jC = {TrussDomain.x=x3;TrussDomain.y=y2}
let jB = {TrussDomain.x=x4;TrussDomain.y=y2}
let jA = {TrussDomain.x=x5;TrussDomain.y=y2}
let jF = {TrussDomain.x=x5;TrussDomain.y=y3}
let rFC = {magnitude = 0.; direction = Vector (x=9.,y = 0.); joint = jF}
let rFB = {magnitude = 0.; direction = Vector (x=0.,y = -1.); joint = jF}
let rA = {magnitude = 1.; direction = Vector (x=9.,y = 5.); joint = jA}
let fC = {magnitude = -80.; direction = Vector (x=0.,y = 6.); joint = jC}
let fB = {magnitude = -60.; direction = Vector (x=4.,y = 6.); joint = jB}
let sF,sA = Pin (rFC,rFB), Roller rA
let partList2 = [Force fC;Force fB;Support sF;Support sA]

getYSupportReactionEquations partList2
getXSupportReactionEquations partList2

//truss2   
let x3a,x4a,x5a,x6a = TrussDomain.X 0., TrussDomain.X 12.,TrussDomain.X 18.,TrussDomain.X 24.
let y2a,y3a = TrussDomain.Y 8., TrussDomain.Y 0.
let jCa = {TrussDomain.x=x6a;TrussDomain.y=y3a}
let jBa = {TrussDomain.x=x4a;TrussDomain.y=y3a}
let jAa = {TrussDomain.x=x3a;TrussDomain.y=y3a}
let jEa = {TrussDomain.x=x5a;TrussDomain.y=y2a}
let rFCa = {magnitude = 0.; direction = Vector (x=25.,y = 0.); joint = jCa}
let rFBa = {magnitude = 0.; direction = Vector (x=25.,y = -1.); joint = jCa}
let rAa = {magnitude = 1.; direction = Vector (x=18.,y = 5.); joint = jEa}
let fCa = {magnitude = 2000.; direction = Vector (x=0.,y = 6.); joint = jAa}
let fBa = {magnitude = 1000.; direction = Vector (x=12.,y = 6.); joint = jBa}
let sFa,sAa = Pin (rFCa,rFBa), Roller rAa
let partList2a = [Force fCa;Force fBa;Support sFa;Support sAa]


getYSupportReactionEquations partList2a
getXSupportReactionEquations partList2a

let j1 = {TrussDomain.x=x0;TrussDomain.y=y0}
let j2 = {TrussDomain.x=x1;TrussDomain.y=y0}
let j3 = {TrussDomain.x=x2;TrussDomain.y=y0}
let j4 = {TrussDomain.x=x1;TrussDomain.y=y1}
let j5 = {TrussDomain.x=x2;TrussDomain.y=y1}


let m1 = j1,j2
let m2 = j1,j4
let m3 = j2,j4
let m4 = j4,j5
let m5 = j2,j3
let m6 = j2,j5
let m7 = j3,j5

let f1 = {magnitude = 100.; direction = Vector (x=0.,y = 1.); joint = j1}
let f2 = {magnitude = 100.; direction = Vector (x=1.,y = 0.); joint = j1}

let f3 = {magnitude = 1.; direction = Vector (x=6.,y = 1.); joint = j3}


let f4 = {magnitude = 12.; direction = Vector (x=3.,y = 3.); joint = j4}

let f5 = {magnitude = -3.; direction = Vector (x=7.,y = 0.); joint = j5}

let s1,s2 = Pin (f1,f2), Roller f3

let mList = [m1;m2;m3;m4;m5;m6;m7]

let fList = [f4;f5]
let sList = [s1;s2]

let truss = {members=mList;forces=fList;supports=sList}

checkTrussStability truss

getReactionForcesFrom sList |> getDirectionsFrom

getComponentForcesFrom f5

let partList = [Force f4;Force f5;Support s1;Support s2]

getYSupportReactionEquations partList



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