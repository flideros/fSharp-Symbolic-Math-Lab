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
open System.Runtime.InteropServices
open System.Diagnostics
open System.Windows.Interop


//\/--- Browser Controls ------------------------------------------------------------------\/
   
 ///--- This control uses the native WebBrowser ActiveX control.  
 ///--- https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/frame

type FrameBrowser(page:String) as this =
     inherit Frame()     
     do
        this.Source <- Uri(page)

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


//\/--- Win32 AppContainer ---------------------------------------------------------------\/
(*
module External =

    [<DllImport("user32.dll", EntryPoint="GetWindowThreadProcessId",  SetLastError=true,
             CharSet=CharSet.Unicode, ExactSpelling=true,
             CallingConvention=CallingConvention.StdCall)>]
    extern void GetWindowThreadProcessId(float hWnd, float lpdwProcessId)
            
    [<DllImport("user32.dll", SetLastError=true)>]
    extern void FindWindow (string lpClassName, string lpWindowName)

    [<DllImport("user32.dll", SetLastError=true)>]
    extern void  SetParent (IntPtr hWndChild, IntPtr hWndNewParent)

    [<DllImport("user32.dll", EntryPoint="GetWindowLongA", SetLastError=true)>]
    extern void  GetWindowLong (IntPtr hwnd, int nIndex)

    [<DllImport("user32.dll", EntryPoint="SetWindowLongA", SetLastError=true)>]
    extern void  SetWindowLongA(System.IntPtr hWnd, int nIndex, int dwNewLong)

    [<DllImport("user32.dll", SetLastError=true)>]
    extern void  SetWindowPos(IntPtr hwnd, float hWndInsertAfter, float x, float y, float cx, float cy, float wFlags)
        
    [<DllImport("user32.dll", SetLastError=true)>]
    extern void  MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint)

type AppContainer() as this =
    inherit  UserControl()       

    let SWP_NOOWNERZORDER = 0x200
    let SWP_NOREDRAW = 0x8
    let SWP_NOZORDER = 0x4
    let SWP_SHOWWINDOW = 0x0040
    let WS_EX_MDICHILD = 0x40
    let SWP_FRAMECHANGED = 0x20
    let SWP_NOACTIVATE = 0x10
    let cSWP_ASYNCWINDOWPOS = 0x4000
    let SWP_NOMOVE = 0x2
    let SWP_NOSIZE = 0x1
    let GWL_STYLE = (-16)
    let WS_VISIBLE = 0x10000000
    let WS_CHILD = 0x40000000
    
    /// <summary>
    /// Track if the application has been created
    /// </summary>
    let mutable _iscreated = false;

    /// <summary>
    /// Handle to the application Window
    /// </summary>
    [<DefaultValue>] val mutable  _appWin : IntPtr

    [<DefaultValue>] val mutable _childp : Process

    /// <summary>
    /// The name of the exe to launch
    /// </summary>
    let mutable exeName = ""
    member this.ExeName with get () = exeName        
    member this.ExeName with set (value) = exeName <- value
    
    /// <summary>
    /// Force redraw of control when size changes
    /// </summary>
    /// <param name="e">Not used</param>
    let OnSizeChanged (s:obj) (e:SizeChangedEventArgs) = this.InvalidateVisual()

    /// <summary>
    /// Create control when visibility changes
    /// </summary>
    /// <param name="e">Not used</param>
    let OnVisibleChanged(s:obj)(e:RoutedEventArgs) = 
        // If control needs to be initialized/created
        if _iscreated = false then
            do 
                _iscreated <- true
                this._appWin <- IntPtr.Zero
            try
                let procInfo = new System.Diagnostics.ProcessStartInfo(this.ExeName)
                do
                    procInfo.WorkingDirectory <- System.IO.Path.GetDirectoryName(this.ExeName)
                    // Start the process
                    this._childp <- System.Diagnostics.Process.Start(procInfo)

                    // Wait for process to be created and enter idle condition
                    this._childp.WaitForInputIdle() |> ignore

                    // Get the main handle
                    this._appWin  <-  this._childp.MainWindowHandle
            with
            | Failure msg -> "caught: " + msg |> ignore
    
            // Put it into this form
            let  helper = new WindowInteropHelper(Window.GetWindow(this))
            do
                External.SetParent(this._appWin, helper.Handle)

                // Remove border and whatnot
                External.SetWindowLongA(this._appWin, GWL_STYLE, WS_VISIBLE)

                // Move the window to overlay it on this window
                External.MoveWindow(this._appWin, 0, 0, (int)this.ActualWidth, (int)this.ActualHeight, true)
     
    let OnResize (s:obj)(e:RoutedEventArgs) =
        if (this._appWin <> IntPtr.Zero) then
            External.MoveWindow(this._appWin, 0, 0, (int)this.ActualWidth, (int)this.ActualHeight, true)

    member this.InitializeComponent() =            
        this.SizeChanged.AddHandler(new SizeChangedEventHandler(OnSizeChanged))
        this.Loaded.AddHandler(new RoutedEventHandler(OnVisibleChanged))
        this.SizeChanged.AddHandler(new SizeChangedEventHandler(OnResize))
        

    member x.Dispose() = (x :> IDisposable).Dispose() 

    interface IDisposable with 
        member this.Dispose() = this.Dispose()
*)
//\--- Win32 AppContainer ----------------------------------------------------------------------/\
