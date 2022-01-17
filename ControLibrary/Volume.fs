namespace ControlLibrary

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes  
open System.Windows.Media.Animation
open System.IO
open System.Windows.Markup
open System.Windows.Controls 
open System.Reflection
open System.Windows.Media.Imaging
open System.Windows.Media.Media3D
open Utilities
open System.Windows.Input

/// Volume control , it shows a value and allows you to change it.
type Volume(title:string, range:int * int, value:SharedValue<int>) as this =
    inherit StackPanel(Orientation=Orientation.Vertical,VerticalAlignment = VerticalAlignment.Center)
  
    do Label(Content=title,Width=110.) |> this.Children.Add |> ignore 
    let label  = Label(Content=value.Get,Width=27.)    
    let slider = Slider(Minimum=float(fst range), Maximum=float(snd range), TickFrequency=2., Width=127.)    
    let changedHandler value =
        label.Content <- string value
        slider.Value  <- float value
    do
        this.Children.Add(slider) |> ignore  
        this.Children.Add(label) |> ignore  
        // specifying how to cooperate shared value and slider control
        slider.ValueChanged.Add(fun arg -> int arg.NewValue |> value.Set)
        value.Changed.Add changedHandler
        changedHandler value.Get // initialization

/// Volume control of a color
type ColorVolume (color:SharedValue<Color>) as this =
  inherit StackPanel(Orientation=Orientation.Vertical)
  // shared values for controls which represents ARGB of selected color
  let alpha = SharedValue(int color.Get.A)
  let red   = SharedValue(int color.Get.R)
  let green = SharedValue(int color.Get.G)
  let blue  = SharedValue(int color.Get.B)
  // specifying how to calculate dependent shared values
  let argbChanged = 
    alpha.Changed 
    |> Observable.merge red.Changed 
    |> Observable.merge green.Changed 
    |> Observable.merge blue.Changed
  do    
    argbChanged.Add(fun _ ->
      color.Set(Color.FromArgb(byte alpha.Get,byte red.Get,byte green.Get,byte blue.Get))
      )
    color.Changed.Add(fun color ->
      alpha.Set (int color.A)
      red.Set   (int color.R)
      green.Set (int color.G)
      blue.Set  (int color.B)
      )
    // adding volume controls
    Volume("Alpha", (0,255), alpha) |> this.Children.Add |> ignore 
    Volume("Red"  , (0,255), red  ) |> this.Children.Add |> ignore 
    Volume("Green", (0,255), green) |> this.Children.Add |> ignore 
    Volume("Blue" , (0,255), blue ) |> this.Children.Add |> ignore 
