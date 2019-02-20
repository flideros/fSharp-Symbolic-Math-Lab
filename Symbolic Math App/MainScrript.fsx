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
open Control

let triangle = PointCollection()    
triangle.Add(Point(1.0,50.0)); triangle.Add(Point(10.0,80.0)); triangle.Add(Point(51.0,50.0))

//
// compose controls
//
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
let window = Window(Title="F# is fun!",Width=260., Height=420., Content=stackPanel)
window.Show()

