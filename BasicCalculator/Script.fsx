#light

#I @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA\Office14\"
#r "Office.dll"
#r "stdole.dll"
#r "Microsoft.Office.Interop.Word.dll"

open Microsoft.Office.Interop.Word

let private mWord : ApplicationClass option ref = ref None

let OpenWord()        = mWord := Some(new ApplicationClass())
let GetWordInstance() = Option.get !mWord
let CloseWord()       = (GetWordInstance()).Quit()

let comarg x = ref (box x)
   
let OpenDocument filePath = 
    printfn "Opening %s..." filePath
    
    GetWordInstance().Documents.Open(comarg filePath)

OpenWord() 
OpenDocument(@"D:\MyFolders\Desktop\02300 Earthwork.docx")
GetWordInstance().ActiveDocument.ExportAsFixedFormat(@"D:\MyFolders\Desktop\02300 Earthwork",WdExportFormat.wdExportFormatXPS)
CloseWord()






//\/-------------------  random scripts  -------------------\/


// Here’s a simple F# script to automate the task of printing every Word doc in the same folder as the script. 
// Simply copy the .FSX file into the desired folder, and whenever you want to print out all the documents 
// right click and select ‘Run in F# Interactive…’. 

(*#I @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA\Office14\"
#r "Office.dll"
#r "stdole.dll"
#r "Microsoft.Office.Interop.Word.dll"

open Microsoft.Office.Interop.Word

let private mWord : ApplicationClass option ref = ref None

let OpenWord()        = mWord := Some(new ApplicationClass())
let GetWordInstance() = Option.get !mWord
let CloseWord()       = (GetWordInstance()).Quit()

let comarg x = ref (box x)
   
let OpenDocument filePath = 
    printfn "Opening %s..." filePath
    
    GetWordInstance().Documents.Open(comarg filePath)*)

let PrintDocument (doc : Document) =
    
    printfn "Printing %s..." doc.Name

    doc.PrintOut(
        Background  = comarg true, 
        Range       = comarg WdPrintOutRange.wdPrintAllDocument,
        Copies      = comarg 1, 
        PageType    = comarg WdPrintOutPages.wdPrintAllPages,
        PrintToFile = comarg false,
        Collate     = comarg true, 
        ManualDuplexPrint = comarg false,    
        PrintZoomColumn = comarg 2,             // Pages 'across'
        PrintZoomRow    = comarg 2)             // Pages 'up down'

let CloseDocument (doc : Document) =
    printfn "Closing %s..." doc.Name
    doc.Close(SaveChanges = comarg false)

// -------------------------------------------------------------

let currentFolder = __SOURCE_DIRECTORY__

open System
open System.IO

try
    OpenWord()

    printfn "Printing all files in [%s]..." currentFolder

    Directory.GetFiles(currentFolder, "*.docx")
    |> Array.iter 
        (fun filePath -> 
            let doc = OpenDocument filePath
            PrintDocument doc
            CloseDocument doc)
finally
    CloseWord()

printfn "Press any key..."
Console.ReadKey(true) |> ignore

// -------------------------------------------------------------

/// Get all files under a given folder
open System.IO

let rec allFilesUnder baseFolder = 
    seq {
        yield! Directory.GetFiles(baseFolder)
        for subDir in Directory.GetDirectories(baseFolder) do
            yield! allFilesUnder subDir 
        }
    
/// Active Pattern for determining file extension
let (|EndsWith|_|) extension (file : string) = 
    if file.EndsWith(extension) 
    then Some() 
    else None

/// Shell executing a program
open System.Diagnostics

let shellExecute program args =
    let startInfo = new ProcessStartInfo()
    startInfo.FileName <- program
    startInfo.Arguments <- args
    startInfo.UseShellExecute <- true

    let proc = Process.Start(startInfo)
    proc.WaitForExit()
    ()

// -------------------------------------------------------------

// Launches all .fs and .fsi files under the current folder in Notepad
open System

allFilesUnder Environment.CurrentDirectory
|> Seq.filter (function 
            | EndsWith ".fs" _
            | EndsWith ".fsi" _
                -> true
            | _ -> false)
|> Seq.iter (shellExecute "Notepad.exe")


// -------------------------------------------------------------





// -------------------------------------------------------------




// -------------------------------------------------------------