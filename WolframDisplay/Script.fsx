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


let testResult = "{{Ry0 -> -13.2124, Rx1 -> 70.7107, Ry1 -> 100.561}}" //
let testReaction = "Ry1 -> -13.2124"

let parseReaction (r:string) =
    let r' = r.Trim()
    let index = 
        match r.Contains("->") with
        | false -> false,0
        | true -> r'.Substring(2,r.IndexOf(" ->")-2) |> System.Int32.TryParse
    let reaction = 
        match r.Contains("->") with
        | false -> ""
        | true -> r'.Substring(0,2)
    let magnitude = 
        match r.Contains("->") with
        | false -> false,0.0
        | true -> r'.Substring(r.LastIndexOf(">")+1) |> System.Double.TryParse
    match r' with 
    | "Unable to compute result" -> (0,"",0.0)
    | _ -> (snd index),reaction,(snd magnitude)

parseReaction testReaction

let parseReactions (r:string) = 
    let r' = r |> String.filter (fun c -> (c = '{') = false) |> String.filter (fun c -> (c = '}') = false)
    let reactions = 
        match r with 
        | "{}" -> ["Unable to compute result"]
        |  "" -> ["Unable to compute result"]
        |  _ -> Array.toList (r'.Split(','))
    let parseReaction (r:string) =
        let r' = r.Trim()
        let index = 
            match r.Contains("->") with
            | false -> false,0
            | true -> r'.Substring(2,r.IndexOf(" ->")-2) |> System.Int32.TryParse
        let reaction = 
            match r.Contains("->") with
            | false -> ""
            | true -> r'.Substring(0,2)
        let magnitude = 
            match r.Contains("->") with
            | false -> false,0.0
            | true -> r'.Substring(r.LastIndexOf(">")+1) |> System.Double.TryParse
        match r' with 
        | "Unable to compute result" -> (0,"",0.0)
        | _ -> (snd index),reaction,(snd magnitude)
    List.map parseReaction reactions

parseReactions testResult

//truss1 -- supports joints alligned verticaly, 2 reactions in x direction   
let x33,x4,x5,x6 = TrussDomain.X 0., TrussDomain.X 2.,TrussDomain.X 4.,TrussDomain.X 6.
let y22,y33 = TrussDomain.Y 0., TrussDomain.Y 25.
let jD = {TrussDomain.x=x6;TrussDomain.y=y22}
let jC = {TrussDomain.x=x5;TrussDomain.y=y22}
let jB = {TrussDomain.x=x4;TrussDomain.y=y22}
let jA = {TrussDomain.x=x33;TrussDomain.y=y22}
let jF = {TrussDomain.x=x33;TrussDomain.y=y33}

let rFC = {magnitude = 0.; direction = Vector (x = 1.,y = 0.); joint = jA}
let rFB = {magnitude = 0.; direction = Vector (x = 0.,y = 1.); joint = jA}
let rA = {magnitude = 0.; direction = Vector (x = -1.,y = 25.); joint = jF}

let fD = {magnitude = 12.5; direction = Vector (x=6.,y = 1.); joint = jD}
let fC = {magnitude = 12.5; direction = Vector (x=4.,y = 1.); joint = jC}
let fB = {magnitude = 12.5; direction = Vector (x=2.,y = 1.); joint = jB}
let fA = {magnitude = 12.5; direction = Vector (x=0.,y = 1.); joint = jA}

let sF,sA = Pin {tangent=rFC;normal=rFB}, Roller rA
let partList2 = [Force fA;Force fB;Force fC;Force fD;Support sF;Support sA]

//getYMomentReactionEquations partList2
getXMomentReactionEquations partList2
getYForceReactionEquation partList2
getXForceReactionEquation partList2

//truss2 -- reactions not aligned on any axis, 2 reactions in y direction
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
let sFa,sAa = Pin {tangent=rFCa;normal=rFBa}, Roller rAa
let partList2a = [Force fCa;Force fBa;Support sFa;Support sAa]


getYMomentReactionEquations partList2a
//getXMomentReactionEquations partList2a
getYForceReactionEquation partList2a
getXForceReactionEquation partList2a

//truss3 -- reactions not aligned on any axis, 2 reactions in y direction
let xx3,xx4,xx5,xx6,xx7 = TrussDomain.X 0., TrussDomain.X 3.,TrussDomain.X 6.,TrussDomain.X 9.,TrussDomain.X 12.
let yy2,yy3 = TrussDomain.Y 4., TrussDomain.Y 0.
let jjC = {TrussDomain.x=xx7;TrussDomain.y=yy3}
let jjB = {TrussDomain.x=xx5;TrussDomain.y=yy3}
let jjA = {TrussDomain.x=xx3;TrussDomain.y=yy3}
let jjE = {TrussDomain.x=xx6;TrussDomain.y=yy2}
let rrFCY = {magnitude = 0.; direction = Vector (x=12.,y = -1.); joint = jjC}
let rrFCX = {magnitude = 0.; direction = Vector (x=11.,y = 0.); joint = jjC}
let rrE = {magnitude = 1.; direction = Vector (x=9.,y = 3.); joint = jjE}
let ffC = {magnitude = 10.; direction = Vector (x=0.,y = 1.); joint = jjA}
let ffB = {magnitude = 5.; direction = Vector (x=6.,y = 1.); joint = jjB}
let ssF,ssA = Pin {tangent=rrFCX;normal=rrFCY}, Roller rrE
let partList3 = [Force ffC;Force ffB;Support ssF;Support ssA]

getYMomentReactionEquations partList3
//getXMomentReactionEquations partList3
getYForceReactionEquation partList3
getXForceReactionEquation partList3


List.concat [for i in 0..3 -> ["Rx" + i.ToString();"Ry" + i.ToString()]]

let x0,x1,x2,x3 = TrussDomain.X 0., TrussDomain.X 1., TrussDomain.X 2., TrussDomain.X 3.
let y0,y1,y2,y3 = TrussDomain.Y 0., TrussDomain.Y 1.,TrussDomain.Y 2., TrussDomain.Y 3.


let j1 = {TrussDomain.x=x0;TrussDomain.y=y0}
let j2 = {TrussDomain.x=x1;TrussDomain.y=y0}
let j3 = {TrussDomain.x=x2;TrussDomain.y=y0}
let j4 = {TrussDomain.x=x1;TrussDomain.y=y3}
let j5 = {TrussDomain.x=x2;TrussDomain.y=y3}


let m1 = j1,j2
let m2 = j2,j3
let m3 = j4,j5
let m4 = j1,j4
let m5 = j4,j2
let m6 = j5,j3
let m7 = j3,j4

let f1 = {magnitude = 100.; direction = Vector (x=0.,y = 1.); joint = j1}
let f2 = {magnitude = 100.; direction = Vector (x=1.,y = 0.); joint = j1}

let f3 = {magnitude = 100.; direction = Vector (x=2.,y = 11.); joint = j4}


let f4 = {magnitude = 12.; direction = Vector (x=3.,y = 3.); joint = j4}

let f5 = {magnitude = -3.; direction = Vector (x=7.,y = 0.); joint = j5}

let s1,s2 = Pin {tangent=f1;normal=f2}, Roller f3

let mList = [m1;m2;m3;m4;m5;m6;m7]

let fList = [f4; f5]
let sList = [s1;s2]

let truss = {members=mList;forces=fList;supports=sList}

getPartListFrom truss
getJointPartListFrom truss

checkTrussStability truss

getReactionForcesFrom sList |> getDirectionsFrom

getComponentForcesFrom f5

let partList = [Force f4;Force f5;Support s1;Support s2;Member m1]

getYMomentReactionEquations partList

let checkCase_1  m1 m2 = (getMemberLineOfActionFrom m1) = (getMemberLineOfActionFrom m2)

checkCase_1 m1 m2

getZeroForceMembers truss

let ee = getNodeList truss
ee.Length
ee

let members = List.choose (fun x -> match x with | Member m -> Some m | _ -> None) partList

let resultants = 
   [{support=s1;
     xReactionForce = Some f1;
     yReactionForce = Some f2};
    {support=s2;
     xReactionForce = None;
     yReactionForce = Some f3}]

//getJointReactionEquations truss resultants





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