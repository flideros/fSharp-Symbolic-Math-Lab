namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type CircumCircleState = {x1 : float; x2 : float; x3 : float; y1 : float; y2 : float; y3 : float; 
                          verticies : System.Windows.Point seq; selectedVertex : int}
type CircumCircle() as this  =  
    inherit UserControl()    
    do Install() |> ignore
        
    (* Based on the Wolfram .NET/Link Example: Circumcircle
       but utilizing F# and WPF *)  
       
    let mutable state = {x1 = 0.; x2 = 0.; x3 = 0.; y1 = 0.; y2 = 0.; y3 = 0.; verticies=seq[]; selectedVertex = 0}
    
    (*Wolfram Kernel*)
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-WSTP -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.2/WolframKernel.exe\"")
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
    do  kernel.Compute(
            (* Determines the circumcircle of a triangle. Code from Eric Weisstein's PlaneGeometry.m, available on MathWorld. *)
            "Circumcircle[{{x1_,y1_}, {x2_,y2_}, {x3_,y3_}}] :=
    	        Module[{a, d, f, g},
    		        a = Det[{{x1,y1,1}, {x2,y2,1}, {x3,y3,1}}];
    		        d = -1/2 Det[{{x1^2+y1^2,y1,1}, {x2^2+y2^2,y2,1}, {x3^2+y3^2,y3,1}}];
    		        f = 1/2 Det[{{x1^2+y1^2,x1,1}, {x2^2+y2^2,x2,1}, {x3^2+y3^2,x3,1}}];
    		        g = -Det[{{x1^2+y1^2,x1,y1}, {x2^2+y2^2,x2,y2}, {x3^2+y3^2,x3,y3}}];
    		        Circle[{-d/a,-f/a}, Sqrt[(f^2+d^2)/a^2-g/a]]
    	        ]")
    
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
        do  l.Margin <- Thickness(Left = 40., Top = 80., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 400.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Click the mouse three times to define the vertices of a triangle. You can then drag any of the vertices and watch the figure change. The circle is the circumcircle and the lines are the perpendicular bisectors of the sides of the triangle. Note that the three bisectors intersect at the circumcenter."
        l
    let canvas = 
        let c = Canvas(ClipToBounds = true)
        do  c.Background <- System.Windows.Media.Brushes.Aqua 
            c.Children.Add(label) |> ignore
        c    
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Logic*)
    let isOverPoint (p1:System.Windows.Point) = 
        Seq.exists (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) (state.verticies)

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
    
    let getVertexIndex (p1:System.Windows.Point) = 
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 9.) (state.verticies)

    let getVerticies (s : CircumCircleState) = 
        let p1 = System.Windows.Point(X=s.x1,Y=s.y1)
        let p2 = System.Windows.Point(X=s.x2,Y=s.y2)
        let p3 = System.Windows.Point(X=s.x3,Y=s.y3)
        seq[p1;p2;p3]    

    let getBisectors (s : CircumCircleState) = 
        let t1 = System.Windows.Point(X = (s.x1 + s.x2) / 2., Y= (s.y1 + s.y2) / 2.)
        let t2 = System.Windows.Point(X = (s.x2 + s.x3) / 2., Y= (s.y2 + s.y3) / 2.)
        let t3 = System.Windows.Point(X = (s.x3 + s.x1) / 2., Y= (s.y3 + s.y1) / 2.)
        [t1;t2;t3]    
    
    let getCircle s = 
        do  (kernel.Compute(
                "N[Circumcircle[{{" 
                + s.x1.ToString() + "," + s.y1.ToString() + "},{"
                + s.x2.ToString() + "," + s.y2.ToString() + "},{"
                + s.x3.ToString() + "," + s.y3.ToString() + "}}]]"))
        (string) kernel.Result

    let parseCircle (c : string) = 
        let sp = c.Split(',') 
        let px = float ( sp.[0].Replace("Circle[{","") )
        let py = float ( sp.[1].Replace("}","") )
        let r = float ( sp.[2].Replace("]","") )
        let center = Point(X=px, Y=py)
        (center,r)
    
    let handleMouseMove (e : Input.MouseEventArgs) = 
        let i = state.selectedVertex
        let p = e.MouseDevice.GetPosition(this)        
        let p1 = Seq.item ((i + 1) % 3) state.verticies
        let p2 = Seq.item ((i + 2) % 3) state.verticies
        let newState = 
            let s = 
                match i with 
                | 0 -> {state with x1 = p.X; y1 = p.Y}
                | 1 -> {state with x2 = p.X; y2 = p.Y}
                | _ -> {state with x3 = p.X; y3 = p.Y}
            let newVerticies = getVerticies s
            {s with verticies = newVerticies}
        match Input.Mouse.LeftButton = Input.MouseButtonState.Pressed with
        |false -> do  label.Text <- getCircle state
        | true -> 
            do  state <- newState
            let center,radius = getCircle state |> parseCircle
            let bisectors = getBisectors state
            let context = visual.RenderOpen()
            do  context.DrawEllipse(black,blackPen,p1,6.,6.)
                context.DrawEllipse(black,blackPen,p2,6.,6.)
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.DrawLine(blackPen,p1,p2)
                context.DrawLine(blackPen,p2,p)
                context.DrawLine(blackPen,p,p1)
                context.DrawLine(bluePen,center,bisectors.[0])
                context.DrawLine(bluePen,center,bisectors.[1])
                context.DrawLine(bluePen,center,bisectors.[2])
                context.DrawEllipse(Brushes.Transparent,bluePen,center,6.,6.)
                context.DrawEllipse(Brushes.Transparent,redPen,center,radius,radius)
                context.Close()
            let bitmap = getBitmap visual
            do  image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore
                canvas.Children.Add(label) |> ignore   
                label.Text <- getCircle state
    
    let handleMouseDown (e : Input.MouseButtonEventArgs) =
        match Seq.length state.verticies with
        | 0 -> 
            let context = visual.RenderOpen()
            let p = e.MouseDevice.GetPosition(this)
            let verticies = Seq.append state.verticies [p]
            do  state <- {state with x1 = p.X; y1 = p.Y; verticies = verticies}
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.Close()
            let bitmap = getBitmap visual        
            do  image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore                
        | 1 -> 
            let context = visual.RenderOpen()
            let p1 = Seq.item 0 state.verticies
            let p = e.MouseDevice.GetPosition(this) 
            let verticies = Seq.append state.verticies [p]
            do  state <- {state with x2 = p.X; y2 = p.Y; verticies = verticies}
                context.DrawEllipse(black,blackPen,p1,6.,6.)
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.DrawLine(blackPen,p1,p)
                context.Close()
            let bitmap = getBitmap visual
            do  image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore
        | 2 ->             
            let p1 = Seq.item 0 state.verticies
            let p2 = Seq.item 1 state.verticies
            let p = e.MouseDevice.GetPosition(this) 
            let verticies = Seq.append state.verticies [p]            
            do  state <- {state with x3 = p.X; y3 = p.Y; verticies = verticies}
            let center,radius = getCircle state |> parseCircle    
            let bisectors = getBisectors state
            let context = visual.RenderOpen()
            do  context.DrawEllipse(black,blackPen,p1,6.,6.)
                context.DrawEllipse(black,blackPen,p2,6.,6.)
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.DrawLine(blackPen,p1,p2)
                context.DrawLine(blackPen,p2,p)
                context.DrawLine(blackPen,p,p1)
                context.DrawLine(bluePen,center,bisectors.[0])
                context.DrawLine(bluePen,center,bisectors.[1])
                context.DrawLine(bluePen,center,bisectors.[2])
                context.DrawEllipse(Brushes.Transparent,bluePen,center,6.,6.)
                context.DrawEllipse(Brushes.Transparent,redPen,center,radius,radius)
                context.Close()
            let bitmap = getBitmap visual
            do  image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore
                canvas.Children.Add(label) |> ignore
        | 3 -> 
            let p = e.MouseDevice.GetPosition(this) 
            let i = match getVertexIndex p with | Some i -> i | None -> 0            
            match isOverPoint p with
            | true ->                 
                do  state <- {state with selectedVertex = i}
                    label.Text <- "Got Vertex"                     
                    this.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove e))
            | false ->
                do  label.Text <- getCircle state                    
        | _ -> ()

    (*Initialize*)
    do  this.Content <- screen_Grid
        
    (*add event handlers*)
        this.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
module CircumCircle = 
    let window =
        "Needs[\"NETLink`\"]        
        CircumCircle[] :=
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
        			LoadNETType[\"ControlLibrary.SharedValue<float>\"];        
        			form = NETNew[\"System.Windows.Window\"];
        			form@Width = 500;
        			form@Height = 500;			
        			form@Title = \"CircumCircle\";
        			pictureBox = NETNew[\"Math.Presentation.CircumCircle\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@Show[];			
        		]
        	]"