namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

type CircumCircleState = {x1 : float; x2 : float; x3 : float; y1 : float; y2 : float; y3 : float; 
                          verticies : System.Windows.Point seq}
type CircumCircle() as this  =  
    inherit UserControl()    
    do Install() |> ignore
        
    (**)       
    let mutable state = {x1 = 0.; x2 = 0.; x3 = 0.; y1 = 0.; y2 = 0.; y3 = 0.;verticies=seq[]}
    
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
    
    (*Model*)
        
    let image = Image()        
    do  image.SetValue(Panel.ZIndexProperty, -100)
    
    let visual = DrawingVisual() 
    
    let black = SolidColorBrush(Colors.Black)
    let blue = SolidColorBrush(Colors.Blue)
    let red = SolidColorBrush(Colors.Red)
    let bluePen, redPen, blackPen = Pen(blue, 0.5), Pen(red, 0.3), Pen(black, 0.5)
    do  bluePen.Freeze()
        redPen.Freeze()
        blackPen.Freeze()
    
    
    (*Controls*)    
    let label = 
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 200., Top = 200., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
        l
    let canvas = 
        let c = Canvas(ClipToBounds = true)
        do  c.Background <- System.Windows.Media.Brushes.Aqua 
        c    
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Logic*)
    let isOverPoint (p1:System.Windows.Point) = Seq.exists (fun (p2:System.Windows.Point) -> (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 9.) (state.verticies)

    (*Actions*)    
    let handleMouseDown (e : Input.MouseButtonEventArgs)= 
        
        match Seq.length state.verticies with
        | 0 -> 
            let context = visual.RenderOpen()
            let p = e.MouseDevice.GetPosition(this)
            let verticies = Seq.append state.verticies [p]
            do  state <- {state with x1 = p.X; y1 = p.Y; verticies = verticies}
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.Close()
            let bitmap = 
                RenderTargetBitmap(
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight, 
                    96.,
                    96.,
                    PixelFormats.Pbgra32)        
            do  bitmap.Render(visual)
                bitmap.Freeze()
                image.Source <- bitmap
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
            let bitmap = 
                RenderTargetBitmap(
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight, 
                    96.,
                    96.,
                    PixelFormats.Pbgra32)        
            do  bitmap.Render(visual)
                bitmap.Freeze()
                image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore
        | 2 -> 
            let context = visual.RenderOpen()
            let p1 = Seq.item 0 state.verticies
            let p2 = Seq.item 1 state.verticies
            let p = e.MouseDevice.GetPosition(this) 
            let verticies = Seq.append state.verticies [p]
            do  state <- {state with x3 = p.X; y3 = p.Y; verticies = verticies}
                context.DrawEllipse(black,blackPen,p1,6.,6.)
                context.DrawEllipse(black,blackPen,p2,6.,6.)
                context.DrawEllipse(red,redPen,p,6.,6.)
                context.DrawLine(blackPen,p1,p2)
                context.DrawLine(blackPen,p2,p)
                context.DrawLine(blackPen,p,p1)
                context.Close()
            let bitmap = 
                RenderTargetBitmap(
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight, 
                    96.,
                    96.,
                    PixelFormats.Pbgra32)        
            do  bitmap.Render(visual)
                bitmap.Freeze()
                image.Source <- bitmap
                canvas.Children.Clear()
                canvas.Children.Add(image) |> ignore
                canvas.Children.Add(label) |> ignore
        | 3 -> 
            let p = e.MouseDevice.GetPosition(this)             
            match isOverPoint p with
            | true -> 
                do  label.Text <- "true"                 
            | false ->
                do  label.Text <- "false"                
        | _ -> ()


    (*Initialize*)
    do this.Content <- screen_Grid

    (*add event handlers*)
       this.MouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
    
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
    let runCode = fun () -> 
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
        | "Run Code" -> runCode ()
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