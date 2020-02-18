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
open ControlLibrary
open BasicCalculator
open DataLab

//Define app resources and load them to the main prograam.
let resource = new Uri("app.xaml",System.UriKind.Relative)
let mainProgram = Application.LoadComponent(resource) :?> Application

// Controls
let dataLab = DataLab(RenderTransformOrigin = Point(0.,0.))
let calculator = Calculator(OverridesDefaultStyle = true) 
let graphingCalc = GraphingCalculator.GraphingCalculator()
let colorPicker = HsvColorPicker(selectedColor=SharedValue(Colors.Transparent))
let testCanvas = Math.Presentation.TypeSetting.TestCanvas([Math.Presentation.Glyphs.sigma;Math.Presentation.Glyphs.p])

// Tab Control 
let tabs = TabControl()

let item0 = TabItem(Header = "Test Canvas")
do  item0.Content <- testCanvas
let item1 = TabItem(Header = "Graphing Calculator")
do  item1.Content <- graphingCalc
let item2 = TabItem(Header = "SQL Pad")
do  item2.Content <- dataLab
let item3 = TabItem(Header = "Calculator")
do  item3.Content <- calculator
let item4 = TabItem(Header = "ColorPicker")
do  item4.Content <- colorPicker

do  tabs.Items.Add(item0) |> ignore
    tabs.Items.Add(item1) |> ignore
    tabs.Items.Add(item2) |> ignore
    tabs.Items.Add(item4) |> ignore

(**)
// Make a window and add content
let window = new Window(RenderTransformOrigin = Point(0.,0.))
window.Title <- "Math is fun!" 
window.Content <- tabs//notepad//dataLab//calculator//graphingCalc//
window.MinWidth <- 420.
window.MinHeight <- 620.
window.Width <- 420.
window.Height <- 620.
//----------{not needed unless a Xaml used for window}----------//
// Load XAML -  XAML - MUST be Embedded Resource  ("use  {file name}.xaml")    
//let mutable this : Window = Utilities.contentAsXamlObject("MainWindow.xaml"):?> Window  

[<STAThread>] 
[<EntryPoint>]
let main(_) =  
    do mainProgram.Run(window) |> ignore
    0
  
