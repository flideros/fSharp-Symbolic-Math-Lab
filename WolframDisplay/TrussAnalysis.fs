﻿namespace Math.Presentation.WolframEngine

open System
open System.Numerics
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
    type Joint = {x:X; y:Y} //; z:Z}
    type Member = (Joint*Joint)
    type Force = {magnitude:float; direction:Vector; joint:Joint}
    type Support = | Roller of Force | Pin of (Force*Force)    
    type Truss = {members:Member list; forces:Force list; supports:Support list}
    type TrussStability = | Stable | NotEnoughReactions | ReactionsAreParallel 
                          | ReactionsAreConcurrent | InternalCollapseMechanism
    type TrussDeterminacy = | Determinate | Indeterminate
    
    type TrussPart =
        | Joint of Joint
        | Member of Member
        | Force of Force
        | Support of Support
    
    type TrussBuildOp =
        | AddTrussPart
        | SubtractTrussPart
        | SelectTrussPart

    type PendingBuildOp = (TrussBuildOp * TrussPart)

    // Types to describe error results
    type Error = 
        | LazyCoder  
        | InputError
        | Other of string
 
    // Data associated with each state     
    type EvaluatedStateData = {truss:Truss}
    type AccumulateTrussPartData = {newTrussPart : PendingBuildOp option;  truss : Truss}
    type SelectionStateData = {truss:Truss; members:Member option; forces:Force option; supports:Support option}
    type ErrorStateData = { error : Error ; truss : Truss}
    
    // States
    type TrussAnalysisState =         
        | EvaluatedState of EvaluatedStateData
        | AccumulateTrussPartState of AccumulateTrussPartData
        | SelectionState of SelectionStateData
        | ErrorState of ErrorStateData

    
    // Services
    type Test = string -> string
    type GetPointListFromTruss = Truss -> System.Windows.Point seq
    
    type TrussServices = 
        {test:Test;
         getJointSeqFromTruss:GetPointListFromTruss}


module TrussImplementation = 
    open TrussDomain
    
    let getYFrom (j:Joint) = match j.y with | Y y -> y
    let getXFrom (j:Joint) = match j.x with | X x -> x
    let getJointListFrom (members: Member list) = 
        let (l1,l2) = List.unzip members
        List.concat [l1;l2] |> List.distinct    
    let getReactionForcesFrom (supports: Support list) = 
        List.fold (fun acc r -> match r with | Roller f -> f::acc  | Pin (f1,f2) -> f1::f2::acc) [] supports
    let getDirectionsFrom (forces:Force list) = List.map (fun x -> 
        Vector(
            x = x.direction.X - (getXFrom x.joint), 
            y = x.direction.Y - (getYFrom x.joint))) forces
    let getLineOfActionFrom (force:Force) = 
        match (force.direction.X - (getXFrom force.joint)) = 0. with 
        | true -> 
            let m = (force.direction.Y - (getYFrom force.joint)) / (force.direction.X - (getXFrom force.joint))                   
            let b = (getYFrom force.joint) - (m * (getXFrom force.joint))
            (m,b)
        | false -> (0.,(getYFrom force.joint))

    //let sumHorizontalForces (forces:Force list) = 

    let checkTrussStability (truss:Truss) = 
        let m = truss.members.Length 
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1) 0 truss.supports
        let j = (getJointListFrom truss.members).Length
        let checkNotEnoughReactions = m + r < 2 * j
        let checkForParallelReactions = 
            let reactions = getReactionForcesFrom truss.supports |> getDirectionsFrom            
            let rec compareReactions (reactions:Vector list) (premise:bool) = 
                match reactions with 
                | [] -> premise
                | _::[] -> premise
                | r1::r2::tail -> 
                    match r1.X*r2.Y = r1.Y*r2.X with 
                    | true -> compareReactions (r2::tail) true                                    
                    | false ->  false                                     
            compareReactions reactions false
        let checkForConcurrentReactions = 
            let reactions = getReactionForcesFrom truss.supports
            let compareReactions (reactions:Force list) = 
                match reactions with 
                | [] -> false
                | _::[] -> false
                | r1::r2::tail -> 
                    match (r1.direction.X - (getXFrom r1.joint)),(r2.direction.X - (getXFrom r2.joint)) with
                    | 0.,0. -> false 
                    | 0.,_ -> 
                        let a = 0.
                        let b = (r2.direction.Y - (getYFrom r2.joint)) / (r2.direction.X - (getXFrom r2.joint))                   
                        let c = getYFrom r1.joint
                        let d = (getYFrom r2.joint) - (b * (getXFrom r2.joint))
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/b
                            let y =  c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail
                    | _,0. -> 
                        let a = (r1.direction.Y - (getYFrom r1.joint)) / (r1.direction.X - (getXFrom r1.joint))
                        let b = 0.
                        let c = (getYFrom r1.joint) - (a * (getXFrom r1.joint))
                        let d = (getYFrom r2.joint)
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/a
                            let y = a*((d - c)/a) + c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail
                    | _,_ ->                     
                        let a = (r1.direction.Y - (getYFrom r1.joint)) / (r1.direction.X - (getXFrom r1.joint))
                        let b = (r2.direction.Y - (getYFrom r2.joint)) / (r2.direction.X - (getXFrom r2.joint))                   
                        let c = (getYFrom r1.joint) - (a * (getXFrom r1.joint))
                        let d = (getYFrom r2.joint) - (b * (getXFrom r2.joint))
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/(a - b)
                            let y = a*((d - c)/(a - b)) + c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail  
            compareReactions reactions

        match checkNotEnoughReactions, 
              checkForParallelReactions, 
              checkForConcurrentReactions with
        | true, true,true -> [NotEnoughReactions; ReactionsAreParallel;ReactionsAreConcurrent]
        | true, true,false -> [NotEnoughReactions; ReactionsAreParallel]
        
        | true, false,true -> [NotEnoughReactions;ReactionsAreConcurrent]
        | true, false,false -> [NotEnoughReactions]
        
        | false, true,true -> [ReactionsAreParallel;ReactionsAreConcurrent]
        | false, true,false -> [ReactionsAreParallel]
        
        | false, false,true -> [ReactionsAreConcurrent]
        | false, false,false -> [Stable]
    let checkTrussDeterminacy (truss:Truss)  = 
        let stability = checkTrussStability truss
        let m = truss.members.Length 
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1) 0 truss.supports
        let j = (getJointListFrom truss.members).Length
        match m + r = 2 * j && stability = [Stable] with
        | true  -> Determinate 
        | false -> Indeterminate

module TrussServices = 
   open TrussDomain
   open TrussImplementation

   let getWolframCode s = (s + "Steady") 

   let getJointSeqFromTruss (t:Truss) = 
       let pointMap (j:Joint) = System.Windows.Point (x = (getXFrom j),y = (getYFrom j))
       let j = getJointListFrom t.members
       let l = List.map (fun x -> pointMap x ) j 
       List.toSeq l
   let createServices () = 
       {test = getWolframCode;
        getJointSeqFromTruss = getJointSeqFromTruss }

//   UI Shell, no funtionality at the moment, 
//\/--- but will run an exampe computation ----\/\\
type TrussAnalysis() as this  =  
    inherit UserControl()    
    do Install() |> ignore

    let initialState = TrussDomain.EvaluatedState {truss = {members=[]; forces=[]; supports=[]}}

    let mutable state = initialState
    
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

    (*Model*)        
    let image = Image()
    do  image.SetValue(Panel.ZIndexProperty, -100)    
    let visual = DrawingVisual()     
    let black = SolidColorBrush(Colors.Black)
    let blue = SolidColorBrush(Colors.Blue)
    let red = SolidColorBrush(Colors.Red)
    let bluePen, redPen, blackPen = Pen(blue, 0.5), Pen(red, 0.5), Pen(black, 0.5)
    do  bluePen.Freeze()
        redPen.Freeze()
        blackPen.Freeze() 
    
    (*Controls*)    
    let label = 
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 240., Top = 450., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 400.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Rock"
        l
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
            c.Children.Add(result_StackPanel) |> ignore
        c
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()
    
    (*Actions*)
    let getBitmap visual = 
        let bitmap = 
            RenderTargetBitmap(
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight, 
                96.,
                96.,
                PixelFormats.Pbgra32)        
        do  bitmap.Render(visual)
            bitmap.Freeze()
        bitmap
    let getTruss = 
        match state with
        | TrussDomain.EvaluatedState es -> es.truss
        | TrussDomain.AccumulateTrussPartState aps-> aps.truss
        | TrussDomain.SelectionState ss -> ss.truss
        | TrussDomain.ErrorState es -> es.truss

    let getVertexIndex (p1:System.Windows.Point) = 
        let v = trussServices.getJointSeqFromTruss (getTruss)
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 9.) (v)


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
          let graphics = link.EvaluateToImage("Factor[x^2- 4x + 1]", width = 400, height = 400)
          let image = Image()            
          do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
              result_StackPanel.Children.Add(result_Viewbox image) |> ignore
    let testLabelFromService = 
        do label.Text <- trussServices.test "Rock"

    (*Initialize*)
    do  this.Content <- screen_Grid
        
        setGraphicsFromKernel kernel
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> testLabelFromService))
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
    