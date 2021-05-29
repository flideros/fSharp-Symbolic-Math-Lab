﻿namespace Math.Presentation.WolframEngine

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type MohrsCircleState = {sigmaX : float;  tauXY : float;  tauXZ : float;
                          tauYX : float; sigmaY : float;  tauYZ : float;
                          tauZX : float;  tauZY : float; sigmaZ : float;
                          theta : float}
type MohrsCircle() as this  =  
    inherit UserControl()
    do Install() |> ignore
           
    let mutable state = {sigmaX = 25.0;  tauXY = -3.5;  tauXZ = 0.0;
                          tauYX = -3.5; sigmaY = 5.0;  tauYZ = 0.0;
                          tauZX = 0.0;  tauZY = 0.0; sigmaZ = 0.0;
                          theta = 0.0}
    
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
    
    let substituteValues (v : string) s = 
        v.Replace("\[Sigma]x", s.sigmaX.ToString())
         .Replace("\[Sigma]y", s.sigmaY.ToString())
         .Replace("\[Tau]",    s.tauXY.ToString())
         .Replace("\[Theta]",  s.theta.ToString() + "Degree")
    let sigma = 
        "sigma = {{\[Sigma]x, \[Tau]}, {\[Tau], \[Sigma]y}};
        A = {{Cos[\[Theta]], Sin[\[Theta]]}, {-Sin[\[Theta]], Cos[\[Theta]]}};
        At = Transpose[A];"
    let sigmaX' (s:MohrsCircleState) = 
        let s'Code = sigma + "sigmaPrime = InputForm[Extract[Simplify[Expand[A . sigma . At]],{1,1}]]"
        let code = substituteValues s'Code s
        kernel.Compute(code)
        kernel.Result.ToString()
    let sigmaY' (s:MohrsCircleState) = 
        let s'Code = sigma + "sigmaPrime = InputForm[Extract[Simplify[Expand[A . sigma . At]],{2,2}]]"
        let code = substituteValues s'Code s
        kernel.Compute(code)
        kernel.Result.ToString()
    let tau' (s:MohrsCircleState) = 
        let t'Code = sigma + "sigmaPrime = InputForm[Extract[Simplify[Expand[A . sigma . At]],{1,2}]]"
        let code = substituteValues t'Code s
        kernel.Compute(code)
        kernel.Result.ToString()
    let code (s:MohrsCircleState) = 
        let sX' = sigmaX' state
        let sY' = sigmaY' state
        let t' = tau' state
        let wCode =
            "Grid[{{Text@Style[\"Mohr's Circle\", 26], SpanFromLeft}, 
                {Graphics[{{
                    Line[{{\[Sigma]x, - (\[Tau])}, {\[Sigma]y, \[Tau]}}],
                    Circle[{(\[Sigma]x + \[Sigma]y)/2, 0}, Sqrt[((\[Sigma]x - \[Sigma]y)/2)^2 + (\[Tau])^2]],
                    Circle[{(\[Sigma]x + \[Sigma]y)/2, 0}, 0.35 Sqrt[((\[Sigma]x - \[Sigma]y)/2)^2 + (\[Tau])^2],{ArcTan[(\[Sigma]x - \[Sigma]y)/2, - \[Tau]],0}],             
                
                    {Red,
                    Circle[{(\[Sigma]x + \[Sigma]y)/2, 0},  0.45 Sqrt[((" + sX' + " - " + sY' + ")/2)^2 + (" + t' + ")^2],{ArcTan[(" + sX' + " - " + sY' + ")/2, - " + t' + "],0}],                    
                    Line[{{" + sX' + ", -(" + t' + ")}, {" + sY' + ", " + t' + "}}],}                                        
                    }},                    
                
                    Axes -> True, 
                    AxesOrigin -> {0, 0}, 
                    AspectRatio -> Automatic, 
                    AxesLabel -> {Style[\"\[Sigma]\", Large], Style[\"Sheer\", Large]},
                    ImageSize -> Scaled[0.8]],
                    SpanFromLeft}}]"
        substituteValues wCode s

    do  kernel.Compute(code state)
 
    (*Controls*)    
    let label = 
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 240., Top = 650., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 400.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "0"
        l
    let parameter_Slider =
        let s = Slider()                       
        do  s.SetValue(Grid.RowProperty, 0)
            s.Margin <- Thickness(left = 10., top = 650., right = 0., bottom = 0.)
            s.Minimum <- 0.
            s.Maximum <- 90.
            s.TickPlacement <- System.Windows.Controls.Primitives.TickPlacement.BottomRight
            s.TickFrequency <- 0.25
            s.IsSnapToTickEnabled <- false
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
            let graphics = link.EvaluateToImage(code state, width = 600, height = 600)
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
    let handleSliderValueChange () = do label.Text <- parameter_Slider.Value.ToString()    
    let handleSliderChange () = 
        let p = parameter_Slider.Value.ToString()
        do  state <- {state with theta = parameter_Slider.Value}
            label.Text <- parameter_Slider.Value.ToString()        
            kernel.Compute(code state)
            setGraphicsFromKernel kernel

    (*Initialize*)
    do  this.Content <- screen_Grid
    
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> setGraphicsFromKernel kernel))
        parameter_Slider.PreviewMouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSliderChange ()))
        parameter_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleSliderValueChange ()))
module MohrsCircle = 
    let window =
        "Needs[\"NETLink`\"]        
        MohrsCircle[] :=
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
        			form@Height = 800;			
        			form@Title = \"Mohrs Circle\";
        			pictureBox = NETNew[\"Math.Presentation.WolframEngine.MohrsCircle\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@Show[];			
        		]
        	]"
    