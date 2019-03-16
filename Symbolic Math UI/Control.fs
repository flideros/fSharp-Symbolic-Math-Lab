module Control

open System
open System.Windows          
open System.Windows.Controls
open System.Windows.Media
open TypeExtension
open UI
open Utilities
open System.Windows.Shapes
open System.IO
open System.Windows.Markup
open System.Reflection
open System.Windows.Media.Imaging
open Microsoft.Toolkit.Wpf.UI.Controls
//open Microsoft.Toolkit.Win32.UI.Controls

//\/--- Browser Controls ------------------------------------------------------------------\/
   
 ///--- This control uses the native WebBrowser ActiveX control.  
 ///--- https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/frame

type FrameBrowser(page:String) as this =
     inherit Frame()     
     do
        this.Source <- Uri(page)

///--- This control uses the Microsoft Edge rendering engine (EdgeHTML) to embed a view that 
///--- renders richly formatted HTML5 content from a remote web server, dynamically generated 
///--- code, or content files.
///--- https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/wpf-winforms/webview

type WebViewBrowser(page:String) as this =
     inherit Frame()    
     let webViewer = new WebView()
     do
        webViewer.Source <- Uri(page)
        this.Content <- webViewer

//\--- Browser Controls -------------------------------------------------------------------/\


//\/--- Volume Control --------------------------------------------------------------------\/

/// Volume control , it shows a value and allows you to change it.
type Volume(title:string, range:int * int, value:SharedValue<int>) as this =
  inherit StackPanel(Orientation=Orientation.Horizontal)
  do Label(Content=title,Width=50.) |> this.add 
  let label  = Label(Content=value.Get,Width=50.) $ this.add
  let slider = Slider(Minimum=float(fst range), Maximum=float(snd range), TickFrequency=2., Width=127.) $ this.add
  let changedHandler value =
    label.Content <- string value
    slider.Value  <- float value
  do
    // specifying how to cooperate shared value and slider control
    slider.ValueChanged.Add(fun arg -> int arg.NewValue |> value.Set)
    value.Changed.Add changedHandler

    changedHandler value.Get // initialization
//\--- Volume Control ----------------------------------------------------------------------/\

//\/--- Color Volume Control ---------------------------------------------------------------\/

/// Volume control of a color
type ColorVolume (color:SharedValue<Color>) as this =
  inherit StackPanel(Orientation=Orientation.Vertical)
  // shared values for controls which represents ARGB of selected color
  let alpha = SharedValue(int color.Get.A)
  let red   = SharedValue(int color.Get.R)
  let green = SharedValue(int color.Get.G)
  let blue  = SharedValue(int color.Get.B)
  do
    // specifying how to calculate dependent shared values
    let argbChanged = alpha.Changed |> Observable.merge red.Changed |> Observable.merge green.Changed |> Observable.merge blue.Changed
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
    Volume("Alpha", (0,255), alpha) |> this.add
    Volume("Red"  , (0,255), red  ) |> this.add
    Volume("Green", (0,255), green) |> this.add
    Volume("Blue" , (0,255), blue ) |> this.add

//\--- Color Volume Control -----------------------------------------------------------------/\