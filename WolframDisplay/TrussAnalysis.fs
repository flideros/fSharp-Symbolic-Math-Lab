namespace Math.Presentation.WolframEngine

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

///  CURRENT  PROJECT  ///
(*Truss analysis: This project will explore the mathematics of truss analysis. 
->  Task 1 - Domain Model and implementation
    Task 2 - UI controls
    Task 3 - Develop Wolfram Language Stucture and Interactions
    Task 4 - Continuous Development of features
*)

module TrussDomain =
    
    // Parameters
    type X = X of float
    type Y = Y of float
    type Z = Z of float
   
    // Geometry
    type Point = 
        | Point of X * Y 
        | Point3D of X * Y * Z    
    type ArcSegment =
        { Point : Point
          Size : Size
          RotationAngle : float
          IsLargeArc : bool
          SweepDirection : SweepDirection }    
    type BezierSegment =
        { Point1 : Point
          Point2 : Point
          Point3 : Point }
    type QuadraticBezierSegment =
        { Point1 : Point
          Point2 : Point }     
    type PathGeometry =
        /// Creates a line between two points.
        | LineSegment of Point        
        /// Creates an elliptical arc between two points.
        | ArcSegment of ArcSegment
        /// Creates a cubic Bezier curve between two points.
        | BezierSegment of BezierSegment
        /// Creates a quadratic Bezier curve.
        | QuadraticBezierSegment of QuadraticBezierSegment
        /// Creates a series of lines.
        | PolyLineSegment of seq<Point>        
        /// Creates a series of cubic Bezier curves.
        | PolyBezierSegment of seq<Point>        
        /// Creates a series of quadratic Bezier curves.
        | PolyQuadraticBezierSegment  of seq<Point>
    type Trace = {startPoint:Point; traceSegments:PathGeometry list}

    // Domain Types
    type Joint = {x:X; y:Y; z:Z}
    type Member = (Joint*Joint)
    type Force = {magnitude:float; direction:Point; joint:Joint}
    type Support = | Pin of Force | Roller of (Force*Force)    
    type Truss = {members:Member list; forces:Force list; supports:Support list}

    // Types to describe results
    type Error = 
        | TrussIndeterminate
        | LazyCoder  
        | InputError
        | Other of string
    
    // Data associated with each state     
    type EvaluatedStateData = {truss:Truss}
    type AccumulateMemberData = {newMember : Member option;  truss : Truss}
    type AccumulateSupportData = {newSupport : Support option;  truss : Truss}
    type AccumulateForceData = {newForce : Force option;  truss : Truss}
    type SelectionStateData = {truss:Truss; members:Member option; forces:Force option; supports:Support option}
    type ErrorStateData = { error : Error ; truss : Truss}
    
    // States
    type TrussAnalysisState =         
        | EvaluatedState of EvaluatedStateData
        | AccumulateMemberState of AccumulateMemberData
        | AccumulateSupport of AccumulateSupportData
        | AccumulateForce of AccumulateForceData
        | SelectionState of SelectionStateData
        | ErrorState of ErrorStateData

    // Services
    //type AccumulateMember = Type signature of function

module TrussImplementation = 
    open TrussDomain
    
    let initialState = EvaluatedState {truss = {members=[]; forces=[]; supports=[]}}

    let getJointList (members: Member list) = 
        let (l1,l2) = List.unzip members
        List.concat [l1;l2] |> List.distinct



module TrussServices = 
    let o = "TBD"

//   UI Shell, no funtionality at the moment, 
//\/--- but will run an exampe computation ----\/\\
type TrussAnalysis() as this  =  
    inherit UserControl()    
    do Install() |> ignore
           
    let mutable state = TrussImplementation.initialState
    
    (*Wolfram Kernel*)
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-WSTP -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.3/WolframKernel.exe\"")
    do  link.WaitAndDiscardAnswer()        
    let kernel = 
        let k = new Wolfram.NETLink.MathKernel(link)
        do  k.AutoCloseLink <- true
            k.CaptureGraphics <- true
            k.CaptureMessages <- true
            k.CapturePrint <- true
            k.GraphicsFormat <- "Automatic"
            //k.GraphicsHeight <- 700
            k.GraphicsResolution <- 100
            //k.GraphicsWidth <- 0
            k.HandleEvents <- true
            k.Input <- null
            k.LinkArguments <- null
            k.PageWidth <- 200
            k.ResultFormat <- Wolfram.NETLink.MathKernel.ResultFormatType.OutputForm
            k.UseFrontEnd <- true
        k
    
        
    (*Controls*)    
    let label = 
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 240., Top = 400., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 400.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "0"
        l
    let parameter_Slider =
        let s = Slider()                       
        do  s.SetValue(Grid.RowProperty, 0)
            s.Margin <- Thickness(left = 10., top = 450., right = 0., bottom = 0.)
            s.Minimum <- 0.
            s.Maximum <- 6.
            s.TickPlacement <- System.Windows.Controls.Primitives.TickPlacement.BottomRight
            s.TickFrequency <- 1.
            s.IsSnapToTickEnabled <- true
            s.IsEnabled <- true
            s.AutoToolTipPlacement <- Primitives.AutoToolTipPlacement.TopLeft 
            s.IsSelectionRangeEnabled <- true
        s
    let slider_Grid = 
        let g = Grid()
        do  g.Children.Add(parameter_Slider) |> ignore
            g.Width <- 200.
        g
    let result_Viewbox image =                    
        let vb = Viewbox()   
        do  vb.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            vb.Child <- image
        vb
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
        sp
    let canvas = 
        let c = Canvas(ClipToBounds = true)
        do  c.Background <- System.Windows.Media.Brushes.White 
            c.Children.Add(label) |> ignore
            c.Children.Add(slider_Grid) |> ignore
            c.Children.Add(result_StackPanel) |> ignore
        c
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Logic*)    

    (*Actions*)
    let setGraphicsFromKernel (k:MathKernel) = 
        let rec getImages i =
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage (k.Graphics.[i])
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
            match i + 1 = k.Graphics.Length with
            | true -> ()
            | false -> getImages (i+1)
        match k.Graphics.Length > 0 with                
        | true ->                                        
            result_StackPanel.Children.Clear()            
            getImages 0             
        | false -> 
          result_StackPanel.Children.Clear()
          let p = parameter_Slider.Value.ToString()
          let graphics = link.EvaluateToImage("Factor[x^" + p + " + 1]", width = 400, height = 400)
          let image = Image()            
          do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
              result_StackPanel.Children.Add(result_Viewbox image) |> ignore
            
    let handleSliderValueChange () = do label.Text <- parameter_Slider.Value.ToString()    
    let handleSliderChange () = 
        let p = parameter_Slider.Value.ToString()
        do  label.Text <- parameter_Slider.Value.ToString()        
            kernel.Compute("Factor[x^" + p + " + 1]") //kernel.Compute("Plot[Sin[x (1 + " + p + " x)], {x, 0, 2 * Pi}]")//
            setGraphicsFromKernel kernel

    (*Initialize*)
    do  this.Content <- screen_Grid
        
        setGraphicsFromKernel kernel
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> setGraphicsFromKernel kernel))
        parameter_Slider.PreviewMouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSliderChange ()))
        parameter_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleSliderValueChange ()))
module TrussAnalysis = 
    let window =
        "Needs[\"NETLink`\"]        
        TrussAnalysis[] :=
        	NETBlock[
        		Module[{form, pictureBox},
        			InstallNET[];        			
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\Symbolic_Math.dll\"];
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\ControlLibrary.dll\"];
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\WolframDisplay.dll\"];			
        			LoadNETAssembly[\"System.Core\"];
        			LoadNETAssembly[\"System\"];
        			LoadNETAssembly[\"PresentationCore\"];
        			LoadNETAssembly[\"PresentationFramework\"];			
        			LoadNETType[\"System.Windows.Window\"];
        			form = NETNew[\"System.Windows.Window\"];
        			form@Width = 600;
        			form@Height = 600;			
        			form@Title = \"Truss Analysis\";
        			pictureBox = NETNew[\"Math.Presentation.WolframEngine.TrussAnalysis\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@ShowDialog[];			
        		]
        	]"
    