namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type WolframDrawCanvas() as this  =  
    inherit UserControl()    
    do Install() |> ignore
       
    
    let mutable globalV = ControlLibrary.SharedValue(1.1)
     
    
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
    
    (*Controls*)                    
    let text_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 16.
        tb.TextWrapping <- TextWrapping.Wrap
        tb.HorizontalAlignment <- HorizontalAlignment.Left
        tb.MaxWidth <- 700.       
        tb.Text <- globalV.Get.ToString()
        tb    
    let canvas = 
        let c = Canvas(ClipToBounds = true)
        do  c.Background <- System.Windows.Media.Brushes.Aqua
            c.Children.Add(text_TextBlock) |> ignore
        c
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Actions*)
    let setGlobalV () = 
        let out = globalV.Get + 1.3
        globalV.Set(out)
        text_TextBlock.Text <- globalV.Get.ToString()
        
    (*Initialize*)
    do this.Content <- screen_Grid

    (*add event handlers*)
       //this.MouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> setGlobalV ()))
    
    member public _sv.SetValue( f : float ) = globalV.Set(f)

(*Test Area*)
type WolframCanvas() as this  =  
    inherit UserControl()    
    do Install() |> ignore
    
    let mutable fontSizePoints = ControlLibrary.SharedValue(44)
    
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
    
    (*Controls*)    
    let mathPictureBox = 
        let mb = new UI.MathPictureBox()
        do  mb.Link <- link
            mb.Scale(Drawing.SizeF(7.f,7.f))
            mb.MathCommand <- "Style[\"{Wolfram Canvas}\",FontSize -> 44]"
        mb
    let mathBox_Grid = 
        let g = Grid()
        let host = new System.Windows.Forms.Integration.WindowsFormsHost()
        do  host.Child <- mathPictureBox
            g.Margin <-Thickness(Left = 800., Top = 20., Right = 0., Bottom = 0.)
            g.Children.Add(host) |> ignore
        g
    
    let mathSize_Volume = 
        let v = ControlLibrary.Volume("Compute Math Size",(5,80),fontSizePoints)
        do  v.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        v    
    let destination_ComboBox =
        let cb = ComboBox()        
        do  cb.Text <- "Output Destination"
            cb.Width <- 200.
            cb.Height <- 30.
            cb.FontSize <- 18.
            cb.VerticalContentAlignment <- VerticalAlignment.Center
            cb.SelectedItem <- "Compute"
            cb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            cb.ItemsSource <- ["Compute";"Animate";"Math Picture Box";"Asteroids";"Load Test Code";"Run Code"]
        cb    
    let compute_Button = 
        Button( Name = "Compute",
                Content = "Compute",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 60.,
                Height = 50.,
                Background = Brushes.Aqua)
    let command_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(compute_Button) |> ignore
            sp.Children.Add(destination_ComboBox) |> ignore
            sp.Children.Add(mathSize_Volume) |> ignore 
            //sp.Children.Add(asteroids_Button) |> ignore
            sp.Orientation <- Orientation.Horizontal
            sp.VerticalAlignment <- VerticalAlignment.Center
        sp
    
    let input_TextBox = 
        let tb = TextBox()
        do  tb.Margin <-Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            tb.VerticalScrollBarVisibility <- ScrollBarVisibility.Visible
            tb.HorizontalScrollBarVisibility <- ScrollBarVisibility.Visible
            tb.TextWrapping <- TextWrapping.Wrap
            tb.AcceptsReturn <- true
            tb.MaxHeight  <- 300.
            tb.MaxWidth <- 700.
            tb.FontSize <- 16.
            tb.Text <- "$InverseTrigFunctions = {ArcSin, ArcCos, ArcSec, ArcCsc, ArcTan,ArcCot, ArcSinh, ArcCosh, ArcSech, ArcCsch, ArcTanh, ArcCoth}; \r\n Table[DensityPlot[Im[f[(x + I y)^3]], {x, -2, 2}, {y, -2, 2},ColorFunction -> \"Pastel\", \n ExclusionsStyle -> {None, Purple},Mesh -> None, PlotLabel -> Im[f[(x + I y)^3]],Ticks -> None], {f, $InverseTrigFunctions}]"
        tb 
    let input_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(command_StackPanel) |> ignore            
            sp.Children.Add(input_TextBox) |> ignore
            sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
        sp

    let result_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 16.
        tb.TextWrapping <- TextWrapping.Wrap
        tb.HorizontalAlignment <- HorizontalAlignment.Left
        tb.MaxWidth <- 700.        
        tb
    let result_Viewbox image =                    
        let vb = Viewbox()   
        do  vb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 20.)
            vb.Child <- image
        vb
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
        sp

    let messages_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 14.
        tb
    let print_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 14.
        tb

    let output_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(input_StackPanel) |> ignore
            sp.Children.Add(result_StackPanel) |> ignore
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Orientation <- Orientation.Vertical
        sp
    let output_ScrollViewer = 
        let sv = new ScrollViewer();
        do  sv.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxHeight <- 700.
            sv.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxWidth <-800.
        sv
    
    let canvas = Canvas(ClipToBounds = true)
    let canvas_DockPanel =
        let d = DockPanel()
        do  d.Children.Add(canvas) |> ignore
        d
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
        do  g.Children.Add(canvas_DockPanel) |> ignore
        g
    
    (*Actions*)
    let setButtonsText = fun message -> 
        match message with 
        | "Math Picture Box" -> compute_Button.Content <-"MathBox"
        | _ -> compute_Button.Content <- message
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
            result_StackPanel.Children.Add(result_TextBlock) |> ignore
            getImages 0 
            result_StackPanel.Children.Add(messages_TextBlock) |> ignore
            result_StackPanel.Children.Add(print_TextBlock) |> ignore
        | false ->             
            result_StackPanel.Children.Clear()
            result_StackPanel.Children.Add(result_TextBlock) |> ignore
            result_StackPanel.Children.Add(messages_TextBlock) |> ignore
            result_StackPanel.Children.Add(print_TextBlock) |> ignore
    let setGraphicsFromIKernel (k:IKernelLink) = 
        match kernel.Graphics.Length = 0 with
        | true ->              
            let graphics = k.EvaluateToTypeset("Style[" + input_TextBox.Text + ",FontSize -> " + fontSizePoints.Get.ToString() + "]",0)
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore                
        | false -> ()        
    
    let sendToCompute = fun (text:string) ->
        do setButtonsText "Busy"
        match kernel.IsComputing with 
        | false -> 
            do  kernel.Compute(text)
                messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
                print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""            
                result_TextBlock.Text <- (string) kernel.Result
                setGraphicsFromKernel kernel
                setGraphicsFromIKernel link
                setButtonsText "Compute"           
        | true -> 
            do  kernel.Abort()
                messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
                print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""
                setButtonsText "Compute"    
    let sendToAnimationWindow = fun (code:string) -> 
        setButtonsText "Busy"
        kernel.Compute("AnimationWindow[" + code + ",{t,0,30},Format -> \"StandardForm\", FramePause -> 0.2]")
        setButtonsText "Animate"
    let sendToMathPictureBox = fun (code:string) ->
        setButtonsText "Busy"
        mathPictureBox.MathCommand <- code
        setButtonsText "Math Box"
    let asteroids = fun () -> 
        let loadAsteroids = fun () -> kernel.Compute( WolframCodeBlock.asteroids() )
        let launchAsteroids = fun () -> kernel.Compute("Asteroids[]")
        do setButtonsText "Busy" |> loadAsteroids |> launchAsteroids                        
           setButtonsText (destination_ComboBox.SelectedItem.ToString())
    let loadTestCode = fun () -> 
        do  setButtonsText "Busy" 
        match kernel.IsComputing with 
        | false -> 
            do
            kernel.Compute( WolframCodeBlock.testCode )        
                                   
            messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
            print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""            
            result_TextBlock.Text <- WolframCodeBlock.testCode
            input_TextBox.Text <- "Test[]"
            destination_ComboBox.SelectedItem <- "Run Code"             
            setButtonsText (destination_ComboBox.SelectedItem.ToString())
            result_StackPanel.Children.Clear()                                       
            result_StackPanel.Children.Add(result_TextBlock) |> ignore
            result_StackPanel.Children.Add(messages_TextBlock) |> ignore
            result_StackPanel.Children.Add(print_TextBlock) |> ignore
        | true -> ()
    let RunCode = fun () -> 
        do  setButtonsText "Busy" 
        match kernel.IsComputing with 
        | false -> 
            do
                kernel.Compute( input_TextBox.Text )//mathPictureBox.MathCommand <- input_TextBox.Text//                                    
                messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
                print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""            
                result_TextBlock.Text <- kernel.Result.ToString()
                input_TextBox.Text <- ""
                destination_ComboBox.SelectedItem <- "Run Code"             
                setButtonsText (destination_ComboBox.SelectedItem.ToString())
                result_StackPanel.Children.Clear()                                       
                result_StackPanel.Children.Add(result_TextBlock) |> ignore
                result_StackPanel.Children.Add(messages_TextBlock) |> ignore
                result_StackPanel.Children.Add(print_TextBlock) |> ignore
                setGraphicsFromKernel kernel
        | true -> ()

    let send = fun code ->
        match (string) destination_ComboBox.SelectedValue with
        | "Animate" -> sendToAnimationWindow code
        | "Math Picture Box" -> sendToMathPictureBox code
        | "Asteroids" -> asteroids()
        | "Load Test Code" -> loadTestCode ()
        | "Run Code" -> RunCode ()
        | _ -> sendToCompute code

    (*Initialize*)
    do  output_ScrollViewer.Content <- output_StackPanel
        canvas.Children.Add(output_ScrollViewer) |> ignore
        canvas.Children.Add(mathBox_Grid) |> ignore
        kernel.Compute( WolframCodeBlock.animationWindow )
        result_StackPanel.Children.Clear()        
        this.Content <- screen_Grid

        //add event handlers
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> send input_TextBox.Text)) 
        destination_ComboBox.SelectionChanged.AddHandler(SelectionChangedEventHandler(fun _ _-> setButtonsText (destination_ComboBox.SelectedValue.ToString())))