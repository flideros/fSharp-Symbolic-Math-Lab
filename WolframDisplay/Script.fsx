#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#r @"D:\MyFolders\Desktop\SymbolicMath\packages\Wolfram.NETLink.1.7.1\lib\net461\Wolfram.NETLink.dll"

open System
open System.IO
open System.Windows
open System.Windows.Controls
open System.Windows.Media
open Wolfram.NETLink

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