#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"

#load "Operator.fs"
#load "TypeExtension.fs"
#load "Container.fs"
#load "Control.fs"

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open TypeExtension
open UI
open Operator
//open Control
open System.Windows.Shapes


Rectangle().GetType()



let brush = Brushes.Bisque

//
// compose controls
//

type Browser(page:Uri) =
     inherit UIElement()  
     let browser = new WebBrowser()
     do browser.Navigate(page)


let canvas = StackPanel(Orientation=Orientation.Vertical)
//canvas.HorizontalAlignment <- HorizontalAlignment.Stretch
//canvas.VerticalAlignment <- VerticalAlignment.Stretch
//canvas.Background <- brush

let browser = Browser(Uri("https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.webbrowser?view=netframework-4.7.2")) //$ canvas.add //
//browser 


let window = Window(Title="F# is fun!",Width=260., Height=420., Content=browser)
window.Show()







let triangle = PointCollection()    
triangle.Add(Point(1.0,50.0)); triangle.Add(Point(10.0,80.0)); triangle.Add(Point(51.0,50.0))
/// This StackPanel contains every control in this program
let stackPanel = StackPanel(Orientation=Orientation.Vertical)

let width = SharedValue(120)
Volume("Width",(50, 240),width) |> stackPanel.add // add a volume to the StackPanel

let height = SharedValue(80)
Volume("Height",(50, 200),height) |> stackPanel.add // add a volume to the StackPanel

let color = SharedValue(Colors.Blue)
ColorVolume(color) |> stackPanel.add // add volumes to the StackPanel

let shapes = SharedValue(UI.Shapes.Ellipse)

let ellipseButton   = Button(Content="Ellipse") $ stackPanel.add
ellipseButton.Click.Add(  fun _ -> shapes.Set UI.Shapes.Ellipse)   // add event handler to fire dependency calculation

let rectangleButton = Button(Content="Rectangle") $ stackPanel.add
rectangleButton.Click.Add(fun _ -> shapes.Set UI.Shapes.Rectangle)

let polygonButton = Button(Content="Polygon") $ stackPanel.add
polygonButton.Click.Add(fun _ -> shapes.Set (UI.Shapes.Polygon triangle))

// This is a shape control shown in the bottom of this program's window
ShapeContainer(shapes,width,height,color) |> stackPanel.add

// Make a window and show it
let window2 = Window(Title="F# is fun!",Width=260., Height=420., Content=stackPanel)
window2.Show()

