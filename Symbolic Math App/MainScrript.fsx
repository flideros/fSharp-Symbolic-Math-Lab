#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#load "Operator.fs"
#load "TypeExtension.fs"
#load "Container.fs"

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes
open TypeExtension
open Operator
open Container

[<RequireQualifiedAccess>]
type MyShapes = Rectangle | Ellipse | Polygon
/// Shape container control which reacts when properties of a shape is changed.
type ShapeContainer(shapes:share<MyShapes>,width:share<int>,height:share<int>,color:share<Color>) as this =
  inherit Label(Width=250., Height=250.)
  let mutable shape = Ellipse() :> Shape
  let setWidth  width  = shape.Width  <- float width
  let setHeight height = shape.Height <- float height
  let setColor  color  = shape.Fill   <- SolidColorBrush(color)
  
  let poly =     
    let pc, p = PointCollection(), Polygon()
    do pc.Add(Point(1.0,50.0)); pc.Add(Point(10.0,80.0)); pc.Add(Point(51.0,50.0))
       p.Points <- pc
       p.Stretch <- shape.Stretch
    p 
  
  let initShape () =
    this.Content <- shape
    setWidth  width.Get
    setHeight height.Get
    setColor  color.Get
  let setShape du =
    match du with
      | MyShapes.Rectangle -> shape <- Rectangle()
      | MyShapes.Ellipse   -> shape <- Ellipse  ()
      | MyShapes.Polygon -> shape <- poly
            
            
    initShape ()
  do
    // specifying cooperations with shared values and the shape
    width.Changed.Add  setWidth
    height.Changed.Add setHeight
    color.Changed.Add  setColor 
    shapes.Changed.Add setShape
    // initialization
    initShape ()

//
// compose controls
//
/// This StackPanel contains every controls in this program
let stackPanel = StackPanel(Orientation=Orientation.Vertical)

let width = SharedValue(120)
Volume("Width",(50, 240),width) |> stackPanel.add // add a volume to the StackPanel

let height = SharedValue(80)
Volume("Height",(50, 200),height) |> stackPanel.add // add a volume to the StackPanel

let color = SharedValue(Colors.Blue)
ColorVolume(color) |> stackPanel.add // add volumes to the StackPanel

let shapes = SharedValue(MyShapes.Ellipse)
let ellipseButton   = Button(Content="Ellipse") $ stackPanel.add
let rectangleButton = Button(Content="Rectangle") $ stackPanel.add
let polygonButton = Button(Content="Polygon") $ stackPanel.add
ellipseButton.Click.Add(  fun _ -> shapes.Set MyShapes.Ellipse)   // add event handler to fire dependency calculation
rectangleButton.Click.Add(fun _ -> shapes.Set MyShapes.Rectangle)
polygonButton.Click.Add(fun _ -> shapes.Set MyShapes.Polygon)
// This is a shape control shown in the bottom of this program's window
ShapeContainer(shapes,width,height,color) |> stackPanel.add

// Make a window and show it
let window = Window(Title="F# is fun!",Width=260., Height=420., Content=stackPanel)
window.Show()

