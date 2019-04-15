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

//Define app resources and load them to the main progream.
let resource = new Uri("app.xaml",System.UriKind.Relative)
let mainProgram = Application.LoadComponent(resource) :?> Application

// Create Types
let browser = FrameBrowser(@"https://www.bing.com/")
let browserBorder = Border()
let dockPanel = DockPanelStyle()
let button = ButtonStyle("Frank's Button")

// Compose Types
browserBorder.Child <- browser

dockPanel.Children.Add (button) |> ignore
dockPanel.Children.Add (browserBorder) |> ignore


let calculator = new Calculator(OverridesDefaultStyle = true) 

// Make a window and add content
let window = new Window()
window.Title <- "F# is fun!" 
window.Content <-  calculator
window.SizeToContent <- SizeToContent.WidthAndHeight
//----------{not needed unless a Xaml used for window}----------//
// Load XAML -  XAML - MUST be Embedded Resource  ("use  {file name}.xaml")    
//{not needed unless a Xaml used for window} let mutable this : Window = Utilities.contentAsXamlObject("MainWindow.xaml"):?> Window  

[<STAThread>] 
[<EntryPoint>]
let main(_) =  
    do mainProgram.Run(window) |> ignore
    0
    
    
