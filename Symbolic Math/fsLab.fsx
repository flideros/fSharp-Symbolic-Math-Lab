#load "../packages/FSharp.Formatting.2.14.4/FSharp.Formatting.fsx"
//conflict with fsharp.data

open FSharp.Literate
open System.IO


let source = __SOURCE_DIRECTORY__
let script = Path.Combine(source, "../packages/FSharp.Formatting.2.14.4/literate/demo.fsx")
let template = Path.Combine(source, "../packages/FSharp.Formatting.2.14.4/literate/templates/template-project.html")



// ----------------------------------------------------------------------------
// SETUP
// ----------------------------------------------------------------------------

/// Return path relative to the current file location
let relative subdir = Path.Combine(__SOURCE_DIRECTORY__, subdir)

// Create output directories & copy content files there
// (We have two sets of samples in "output" and "output-all" directories,
//  for simplicitly, this just creates them & copies content there)
if not (Directory.Exists(relative "output")) then
  Directory.CreateDirectory(relative "output") |> ignore
  Directory.CreateDirectory (relative "output/content") |> ignore

for fileInfo in DirectoryInfo(relative "../../Symbolic Math/packages/FSharp.Formatting.2.14.4/literate").EnumerateFiles() do
  fileInfo.CopyTo(Path.Combine(relative "output/content", fileInfo.Name)) |> ignore

// ----------------------------------------------------------------------------
// EXAMPLES
// ----------------------------------------------------------------------------

/// Processes a single F# Script file and produce HTML output
let processScriptAsHtml () =
  let file = relative "output/content/demo.fsx"
  let output = relative "output/demo-script.html"
  let template = relative "templates/template-file.html"
  Literate.ProcessScriptFile(file, template, output)

/// Processes a single F# Script file and produce LaTeX output
let processScriptAsLatex () =
  let file = relative "output/content/demo.fsx"
  let output = relative "output/demo-script.tex"
  let template = relative "templates/template-color.tex"
  Literate.ProcessScriptFile(file, template, output, format = OutputKind.Latex)

/// Processes a single Markdown document and produce HTML output
let processDocAsHtml () =
  let file = relative "output/content/demo.md"
  let output = relative "output/demo-doc.html"
  let template = relative "templates/template-file.html"
  Literate.ProcessMarkdown(file, template, output)

/// Processes a single Markdown document and produce LaTeX output
let processDocAsLatex () =
  let file = relative "output/content/demo.md"
  let output = relative "output/demo-doc.tex"
  let template = relative "templates/template-color.tex"
  Literate.ProcessMarkdown(file, template, output, format = OutputKind.Latex)

