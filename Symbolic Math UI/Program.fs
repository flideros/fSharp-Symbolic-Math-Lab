// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module App  
   
open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes  
open System.IO
open System.Windows.Markup
open System.Windows.Controls 
open System.Reflection
open System.Windows.Media.Imaging
open Style
open Control
open BasicCalculator
open DataLab

//Define app resources and load them to the main prograam.
let resource = new Uri("app.xaml",System.UriKind.Relative)
let mainProgram = Application.LoadComponent(resource) :?> Application
(*
// Create Types
let browser = FrameBrowser(@"https://www.bing.com/")
let browserBorder = Border()
let dockPanel = DockPanelStyle()
let button = ButtonStyle("Frank's Button")
let notepad = new AppContainer()



// Compose Types
browserBorder.Child <- browser

dockPanel.Children.Add (button) |> ignore
dockPanel.Children.Add (browserBorder) |> ignore

do notepad.ExeName <- "wordpad.exe"
   notepad.InitializeComponent()

let dataLab = new DataLab(RenderTransformOrigin = Point(0.,0.))
*)
let calculator = new Calculator(OverridesDefaultStyle = true) 
let graphingCalc = GraphingCalculator.GraphingCalculator()

// Make a window and add content
let window = new Window(RenderTransformOrigin = Point(0.,0.))
window.Title <- "Math is fun!" 
window.Content <- graphingCalc//calculator//dataLab//notepad//grid//
window.Width <- 380.//Double.NaN//440.//
window.Height <- 620.//Double.NaN//640.//
//window.MinWidth <- 440.0
//window.MinHeight <- 640.0
//window.MaxWidth <- 440.0
//window.MaxHeight <- 640.0
//window.SizeToContent <- SizeToContent.WidthAndHeight

//----------{not needed unless a Xaml used for window}----------//
// Load XAML -  XAML - MUST be Embedded Resource  ("use  {file name}.xaml")    
//{not needed unless a Xaml used for window} let mutable this : Window = Utilities.contentAsXamlObject("MainWindow.xaml"):?> Window  

[<STAThread>] 
[<EntryPoint>]
let main(_) =  
    do mainProgram.Run(window) |> ignore
    0
  
