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

let testCanvas = Math.Presentation.TestCanvas2()
let stack = 
    let sp = StackPanel()
    let dataLab = 
        let b = Button(Content = "Data Lab")
        let handleClick () =
            let w = Window(SizeToContent = SizeToContent.WidthAndHeight)
            do  w.Content <- DataLab(RenderTransformOrigin = Point(0.,0.))
            w.Topmost <- true
            w.Show() 
        b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b    
    let colorPicker = 
        let b = Button(Content = "Color Picker")
        let handleClick () =
            let w = Window(SizeToContent = SizeToContent.WidthAndHeight)
            do  w.Content <- HsvColorPicker(selectedColor=SharedValue(Colors.Transparent))
            w.Topmost <- true
            w.Show() 
        b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b    
    let basicCalculator = 
        let b = Button(Content = "Basic Calculator")
        let handleClick () =
            let w = Window(SizeToContent = SizeToContent.WidthAndHeight)
            do  w.Content <- BasicCalculator.Calculator()
            w.Topmost <- true
            w.Show() 
        b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b    
    let graphingCalculator = 
        let b = Button(Content = "Graphing Calculator")
        let handleClick () =
            let w = Window(SizeToContent = SizeToContent.WidthAndHeight)
            do  w.Content <- GraphingCalculator.GraphingCalculator()
            w.Topmost <- true
            w.Show() |> ignore
        b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b    
    do  sp.Children.Add(dataLab) |> ignore
        sp.Children.Add(graphingCalculator) |> ignore
        sp.Children.Add(basicCalculator) |> ignore
        sp.Children.Add(colorPicker) |> ignore 
    sp

// Tab Control 
let tabs = TabControl()

let item0 = TabItem(Header = "Test Canvas")
do  item0.Content <- testCanvas
let item1 = TabItem(Header = "Past Projects")
do  item1.Content <- stack

do  tabs.Items.Add(item0) |> ignore
    tabs.Items.Add(item1) |> ignore
(**)
// Make a window and add content
let window = new Window(RenderTransformOrigin = Point(0.,0.))
window.Title <- "Math is fun!" 
window.Content <- tabs//notepad//dataLab//calculator//graphingCalc//
window.MinWidth <- 420.
window.MinHeight <- 620.
window.Width <- 900.
window.Height <- 900.
//----------{not needed unless a Xaml used for window}----------//
// Load XAML -  XAML - MUST be Embedded Resource  ("use  {file name}.xaml")    
//let mutable this : Window = Utilities.contentAsXamlObject("MainWindow.xaml"):?> Window  

[<STAThread>] 
[<EntryPoint>]
let main(_) =  
    do mainProgram.Run(window) |> ignore
    0
  
