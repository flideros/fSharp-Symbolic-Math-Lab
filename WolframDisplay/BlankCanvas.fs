namespace Math.Presentation.WolframEngine

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type BlankCanvasState = {x1 : float; x2 : float; x3 : float; y1 : float; y2 : float; y3 : float; 
                          verticies : System.Windows.Point seq; selectedVertex : int}
type BlankCanvas() as this  =  
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
            l.Text <- "Blank canvas for next project"
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
    
    (*Initialize*)
    do  this.Content <- screen_Grid
        
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
module BlankCanvas = 
    let window =
        "Needs[\"NETLink`\"]        
        BlankCanvas[] :=
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
        			form@Width = 500;
        			form@Height = 500;			
        			form@Title = \"Blank Canvas\";
        			pictureBox = NETNew[\"Math.Presentation.BlankCanvas\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@Show[];			
        		]
        	]"

