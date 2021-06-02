namespace Math.Presentation.WolframEngine

open System
open System.Windows       
open System.Windows.Controls 
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type MohrsCircleState = {sigmaX : float;  tauXY : float;  
                          tauYX : float; sigmaY : float;                          
                          theta : float}
type MohrsCircle() as this  =  
    inherit UserControl()
    
    do  Install() |> ignore
           
    let mutable state = {sigmaX = 244.0;  tauXY = 55.0;
                          tauYX = -55.0; sigmaY = -34.0;
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
    
    (*Logic*)    
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
        let sX = s.sigmaX  .ToString()
        let sY = s.sigmaY  .ToString()
        let t  = s.tauXY   .ToString()
        let sX' = sigmaX' s
        let sY' = sigmaY' s
        let t'  = tau' s
        let radius = "Sqrt[((" + sX + " - " + sY + ")/2)^2 + (" + t + ")^2]"
        let s1 = radius + " + (" + sX + " + " + sY + ")/2"
        let s2 = "-" + radius + " + (" + sX + " + " + sY + ")/2"
        let sMean = "(" + sX + " + " + sY + ")/2"
        let theta1 = "ArcTan[(" + sX + " - " + sY + ")/2, - " + t + "]"
        let theta2 = "ArcTan[(N[" + sX' + "] - N[" + sY' + "])/2, - N[" + t' + "]]"
        let thetaPrinciple = "-" + theta1 + "/2"
        let table = "Column[{
                    Style[\"Stress Components\",16],
                    TextGrid[{
                        {Style[Subscript[\[Sigma],x],14],Style[" + sX + ",14]}, 
                        {Style[Subscript[\[Sigma],y],14],Style[" + sY + ",14]},
                        {Style[\[Tau],14],Style[" + t + ",14]}                    
                       },Frame -> All,Spacings -> {1,1}],
                    Style[\" \",16],
                    Style[\"Computed Results\",16],
                    TextGrid[{
                        {Style[Subscript[\[Sigma],x]',14],Style[" + sX' + ",14]}, 
                        {Style[Subscript[\[Sigma],y]',14],Style[" + sY' + ",14]},
                        {Style[\[Tau]',14],Style[" + t' + ",14]},
                        {Style[Subscript[\[Sigma],1],14],Style[" + s1 + ",14]}, 
                        {Style[Subscript[\[Sigma],2],14],Style[" + s2 + ",14]},                    
                        {Style[Subscript[\[Sigma],mean],14],Style[" + sMean + ",14]},
                        {Style[Subscript[\[Tau],max],14],Style[" + radius + ",14]},
                        {Style[Subscript[\[Tau],min],14],Style[ - " + radius + ",14]},
                        {Style[Subscript[\[Theta],p],14],Style[" +  thetaPrinciple + "/Degree,14]},
                        {Style[Subscript[\[Theta],s],14],Style[" +  thetaPrinciple + "/Degree - 45,14]}
                       },Frame -> All,Spacings -> {1,1}]},Alignment -> Center]"
        let wCode =
            "GraphicsRow[{" + table + ",            
            Column[{
                Text@Style[\"Mohr's Circle\", 26],
                Graphics[{{
                    Line[{{" + sX + ", - (" + t + ")}, {" + sY + ", " + t + "}}],
                    Circle[{" + sMean + ", 0}, " + radius + "],
                    Circle[{" + sMean + ", 0}, 0.35 " + radius + ",{" + theta1 + ",0}],             
                    
                    Locator[{" + sY + ", " + t + "}],
                    Locator[{" + sX + ", -(" + t + ")}],
                    Locator[{" + sX' + ", -(" + t' + ")}, 
                        Graphics[{Red,Circle[{" + sX' + ", -(" + t' + ")}, 0.3]},AspectRatio -> Automatic, ImageSize -> 20]],
                  
                    {Blue,Dashed,
                    Circle[{" + sMean + ", 0},  0.35 " + radius + ",{-Pi/2," + theta1 + "}],
                    Line[{{" + sMean + ", -(" + radius + ")}, {" + sMean + ", " + radius + "}}]},
                 
                    {Red,                    
                    Circle[{" + sMean + ", 0},  0.45 " + radius + ",{" + theta2 + "," + theta1 + "}],                    
                    Line[{{" + sX' + ", -(" + t' + ")}, {" + sY' + ", " + t' + "}}]}                                        
                    }},                    
                
                    Axes -> True,  
                    AxesOrigin -> {0, 0}, 
                    AspectRatio -> Automatic, 
                    AxesLabel -> {Style[\"\[Sigma]\", Medium], Style[\[Tau], Medium]},
                    ImageSize -> Scaled[1]]
                    },Center,Frame -> All]}]"
        wCode
 
    (*Controls*)    
    let angle_TextBox = 
        let tb = TextBox()
        do  tb.FontStyle <- FontStyles.Normal
            tb.FontSize <- 16.
            tb.MaxWidth <- 100.
            tb.HorizontalAlignment <- HorizontalAlignment.Center
            tb.Text <- "0"
            tb.ToolTip <- "Hit Enter to change angle."
            tb.AcceptsReturn <- false
        tb
    let parameter_Slider =
        let s = Slider()                       
        do  s.SetValue(Grid.RowProperty, 0)            
            s.Minimum <- -90.
            s.Maximum <- 90.
            s.TickPlacement <- System.Windows.Controls.Primitives.TickPlacement.BottomRight
            s.TickFrequency <- 1.
            s.IsSnapToTickEnabled <- true
            s.IsEnabled <- true
            s.AutoToolTipPlacement <- Primitives.AutoToolTipPlacement.TopLeft 
            s.IsSelectionRangeEnabled <- true
            //s.Value <- 35.
        s
    let slider_Grid = 
        let g = Grid()
        let l = Label()
        let sp = StackPanel()
        do  l.HorizontalAlignment <- HorizontalAlignment.Center
            l.Content <- "\u03B8"
            l.FontSize <- 16.
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(l) |> ignore
            sp.Children.Add(parameter_Slider) |> ignore
            sp.Children.Add(angle_TextBox) |> ignore            
            
            g.Children.Add(sp) |> ignore
            g.Width <- 200.
        g    
    let sigmaX_TextBox = 
        let tb = TextBox()
        do  tb.Margin <-Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            tb.TextWrapping <- TextWrapping.Wrap
            tb.AcceptsReturn <- false
            tb.MaxHeight  <- 30.
            tb.Width <- 100.
            tb.FontSize <- 16.
            tb.TextAlignment <- TextAlignment.Center
            tb.Text <- state.sigmaX.ToString()
            tb.ToolTip <- "Hit Enter to change stress."
        tb
    let sigmaX_Grid = 
        let g = Grid()
        let l = Label()
        let sp = StackPanel()
        do  l.HorizontalAlignment <- HorizontalAlignment.Center
            l.Content <- "\u03C3" + "x"
            l.FontSize <- 16.
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(l) |> ignore
            sp.Children.Add(sigmaX_TextBox) |> ignore            
            g.Children.Add(sp) |> ignore
        g    
    let sigmaY_TextBox = 
        let tb = TextBox()
        do  tb.Margin <-Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            tb.TextWrapping <- TextWrapping.NoWrap
            tb.AcceptsReturn <- false
            tb.MaxHeight  <- 30.
            tb.Width <- 100.
            tb.FontSize <- 16.
            tb.TextAlignment <- TextAlignment.Center
            tb.Text <- state.sigmaY.ToString()
            tb.ToolTip <- "Hit Enter to change stress."
        tb
    let sigmaY_Grid = 
        let g = Grid()
        let l = Label()
        let sp = StackPanel()
        do  l.HorizontalAlignment <- HorizontalAlignment.Center
            l.Content <- "\u03C3" + "y"
            l.FontSize <- 16.
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(l) |> ignore
            sp.Children.Add(sigmaY_TextBox) |> ignore            
            g.Children.Add(sp) |> ignore
        g
    let tau_TextBox = 
        let tb = TextBox()
        do  tb.Margin <-Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            tb.TextWrapping <- TextWrapping.NoWrap
            tb.AcceptsReturn <- false
            tb.MaxHeight  <- 30.
            tb.Width <- 100.
            tb.FontSize <- 16.
            tb.TextAlignment <- TextAlignment.Center
            tb.Text <- state.tauXY.ToString()
            tb.IsReadOnly <- false
            tb.ToolTip <- "Hit Enter to change stress."
        tb
    let tau_Grid = 
        let g = Grid()
        let l = Label()
        let sp = StackPanel()
        do  l.HorizontalAlignment <- HorizontalAlignment.Center
            l.Content <- "\u03C4"
            l.FontSize <- 16.
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(l) |> ignore
            sp.Children.Add(tau_TextBox) |> ignore            
            g.Children.Add(sp) |> ignore
        g
    let control_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Horizontal
            sp.Orientation <- Orientation.Horizontal
            sp.Children.Add(slider_Grid) |> ignore
            sp.Children.Add(sigmaX_Grid) |> ignore
            sp.Children.Add(sigmaY_Grid) |> ignore
            sp.Children.Add(tau_Grid) |> ignore
            sp.Margin <- Thickness(left = 80., top = 610., right = 0., bottom = 0.)
        sp    
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
            c.Children.Add(control_StackPanel) |> ignore
            c.Children.Add(result_StackPanel) |> ignore
        c
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
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
            let graphics = link.EvaluateToImage(code state, width = 800, height = 600)
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
    let handleSliderValueChange () = do angle_TextBox.Text <- parameter_Slider.Value.ToString()    
    let handleSliderChange () = 
        do  state <- {state with theta = parameter_Slider.Value}
            angle_TextBox.Text <- parameter_Slider.Value.ToString()        
            kernel.Compute(code state)
            setGraphicsFromKernel kernel
    let handleSigmaXValueChanged () = 
        match Double.TryParse (sigmaX_TextBox.Text) with 
        | true,_ -> state <- {state with sigmaX = (float) sigmaX_TextBox.Text} 
        | false,_ -> state <- {state with sigmaX = 0.}
    let handleSigmaYValueChanged () = 
        match Double.TryParse (sigmaY_TextBox.Text) with 
        | true,_ -> state <- {state with sigmaY = (float) sigmaY_TextBox.Text} 
        | false,_ -> state <- {state with sigmaY = 0.}
    let handleTauValueChanged () = 
        match Double.TryParse (tau_TextBox.Text) with 
        | true,_ -> state <- {state with tauXY = (float) tau_TextBox.Text; tauYX = (float) tau_TextBox.Text} 
        | false,_ -> state <- {state with tauXY = 0.; tauYX = 0.}
    let handleThetaValueChanged () = 
        match Double.TryParse (angle_TextBox.Text) with 
        | true,_ -> 
            state <- {state with theta = (float) angle_TextBox.Text}
            parameter_Slider.Value <- (float) angle_TextBox.Text

        | false,_ -> state <- {state with theta = parameter_Slider.Value}
    let handleReturnKey (e:Input.KeyEventArgs) = 
            match e.Key with
            | Input.Key.Return -> 
                kernel.Compute(code state)
                setGraphicsFromKernel kernel
            | _ -> e.Handled <- true

    (*Initialize*)
    do  this.Content <- screen_Grid
        setGraphicsFromKernel kernel
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> setGraphicsFromKernel kernel))
        parameter_Slider.PreviewMouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSliderChange ()))
        parameter_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleSliderValueChange ()))
        sigmaX_TextBox.PreviewKeyUp .AddHandler(Input.KeyEventHandler(fun _ _ -> handleSigmaXValueChanged ()))
        sigmaY_TextBox.PreviewKeyUp.AddHandler(Input.KeyEventHandler(fun _ _ -> handleSigmaYValueChanged ()))
        tau_TextBox.PreviewKeyUp.AddHandler(Input.KeyEventHandler(fun _ _ -> handleTauValueChanged ()))
        angle_TextBox.PreviewKeyUp.AddHandler(Input.KeyEventHandler(fun _ _ -> handleThetaValueChanged ()))
        this.KeyUp.AddHandler(Input.KeyEventHandler(fun _ e -> handleReturnKey (e)))
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
                    form@Width = 820;
        			form@Height = 780;	
        			form@Title = \"Mohrs Circle\";
        			pictureBox = NETNew[\"Math.Presentation.WolframEngine.MohrsCircle\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@ShowDialog[];
        		]
        	]"
    