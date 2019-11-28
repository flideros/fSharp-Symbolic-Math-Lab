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

// Controls
let browser = FrameBrowser(@"https://www.bing.com/")
let browserBorder = Border()
let dockPanel = DockPanelStyle()
let button = ButtonStyle("Frank's Button")
let notepad = new AppContainer()
let dataLab = DataLab(RenderTransformOrigin = Point(0.,0.))
let calculator = Calculator(OverridesDefaultStyle = true) 
let graphingCalc = GraphingCalculator.GraphingCalculator()

// Tab Control 
let tabs = TabControl()

let item1 = TabItem(Header = "Graphing Calculator")
do  item1.Content <- graphingCalc
let item2 = TabItem(Header = "SQL Pad")
do  item2.Content <- dataLab
let item3 = TabItem(Header = "Calculator")
do  item3.Content <- calculator

do  tabs.Items.Add(item1) |> ignore
    tabs.Items.Add(item2) |> ignore
    tabs.Items.Add(item3) |> ignore

// Compose Controls
browserBorder.Child <- browser
dockPanel.Children.Add (button) |> ignore
dockPanel.Children.Add (browserBorder) |> ignore
// Initialize Components
do notepad.ExeName <- "wordpad.exe"
   notepad.InitializeComponent()

(**)
// Make a window and add content
let window = new Window(RenderTransformOrigin = Point(0.,0.))
window.Title <- "Math is fun!" 
window.Content <- graphingCalc//tabs//notepad//dataLab//calculator//
window.Width <- 380.
window.Height <- 620.

//----------{not needed unless a Xaml used for window}----------//
// Load XAML -  XAML - MUST be Embedded Resource  ("use  {file name}.xaml")    
//{not needed unless a Xaml used for window} let mutable this : Window = Utilities.contentAsXamlObject("MainWindow.xaml"):?> Window  

[<STAThread>] 
[<EntryPoint>]
let main(_) =  
    do mainProgram.Run(window) |> ignore
    0
  
