namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

(*Test Area*)
type TestCanvas() as this  =  
    inherit UserControl()
    do Install() |> ignore
    
    let button = 
        Button( Name = "Button",
                Content = "Press me",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = Brushes.Aqua)
              

    let textBlock =                    
        let tb = TextBlock()
        //tb.Text <- "Random stuff"
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 60.
        tb
    // a function that sets the displayed text
    let  setDisplayedText = 
        fun text -> textBlock.Text <- text

    let canvas = Canvas(ClipToBounds = true)
    let scale_Slider =
        let s = 
            Slider(
                Margin = Thickness(left = 0., top = 20., right = 0., bottom = 0.),
                Minimum = 5.,
                Maximum = 100.,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                TickFrequency = 5.,
                IsSnapToTickEnabled = true,
                IsEnabled = true)        
        do s.SetValue(Grid.RowProperty, 0)
        let handleValueChanged (s) = 
            textBlock.RenderTransform <- 
                let tranforms = TransformGroup()
                do  tranforms.Children.Add(TranslateTransform(X = 100., Y = 100.))
                    tranforms.Children.Add(ScaleTransform(ScaleX = 15.0/s,ScaleY = 15.0/s))            
                tranforms
                
        s.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ e -> handleValueChanged (e.NewValue)))
        s
    let scaleSlider_Grid =
        let g = Grid()
        do 
            g.SetValue(DockPanel.DockProperty,Dock.Top)
            g.Children.Add(scale_Slider) |> ignore
        g 
    let canvas_DockPanel =
        let d = DockPanel()
        do d.Children.Add(scaleSlider_Grid) |> ignore
        do d.Children.Add(canvas) |> ignore
        d
    let screen_Grid =
        let g = Grid()
        do g.SetValue(Grid.RowProperty, 1)        
        do g.Children.Add(canvas_DockPanel) |> ignore        
        g
    
    (**)
    let fetch (text:string) =
        
        let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-linkmode launch -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.2/MathKernel.exe\"")
        do  link.WaitAndDiscardAnswer()
            link.Evaluate(text)
            link.WaitForAnswer() |> ignore
        let out = link.GetString() //.ToString() //.GetInteger().ToString()
        do  setDisplayedText out
            link.Close()            

    let compute = fun (text:string) -> fetch text             

    do  canvas.Children.Add(textBlock) |> ignore
        canvas.Children.Add(button) |> ignore
        
        this.Content <- screen_Grid

        //add event handler to each button
        button.Click.AddHandler(RoutedEventHandler(fun _ _ -> compute "WolframAlpha[\"what is the distance to the moon?\"]"))

(*Test Area*)
type TestCanvas2() as this  =  
    inherit UserControl()
    do Install() |> ignore
    
    (*Wolfram Kernel*)
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.2/MathKernel.exe\"")
    do  link.WaitAndDiscardAnswer()
    let kernel = 
        let k = new Wolfram.NETLink.MathKernel(link)
        do  k.AutoCloseLink <- true;
            k.CaptureGraphics <- true;
            k.CaptureMessages <- true;
            k.CapturePrint <- true;
            k.GraphicsFormat <- "Automatic";
            //k.GraphicsHeight <- 0;
            //k.GraphicsResolution <- 0;
            //k.GraphicsWidth <- 0;
            k.HandleEvents <- true;
            k.Input <- null;
            k.LinkArguments <- null;
            k.PageWidth <- 60;
            k.ResultFormat <- Wolfram.NETLink.MathKernel.ResultFormatType.OutputForm;
            k.UseFrontEnd <- true;
        k
    
    (*Controls*)
    
    let input_TextBox = 
        let tb = TextBox()
        do  tb.Margin <-Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            tb.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto
            tb.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto
            tb.TextWrapping <- TextWrapping.Wrap
            tb.AcceptsReturn <- true
        tb
    let compute_Button = 
        Button( Name = "Button",
                Content = "Press me",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 53.,
                Height = 33.,
                Background = Brushes.Aqua)
    let input_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(input_TextBox) |> ignore
            sp.Children.Add(compute_Button) |> ignore
            sp.Orientation <- Orientation.Horizontal
        sp
    
    let result_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 20.
        tb
    let result_Viewbox =                    
        let vb = Viewbox(Margin = Thickness(Left = 10., Top = 20., Right = 0., Bottom = 20.))   
        
        vb
    
    let output_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(input_StackPanel) |> ignore
            sp.Children.Add(result_TextBlock) |> ignore
            sp.Children.Add(result_Viewbox) |> ignore
            sp.Orientation <- Orientation.Vertical
        sp
    let output_ScrollViewer = 
        let sv = new ScrollViewer();
        do  sv.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxHeight <- 900.
            sv.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.Width <-900.
        sv
    
    let canvas = Canvas(ClipToBounds = true)
    let scale_Slider =
        let s = 
            Slider(
                Margin = Thickness(left = 0., top = 20., right = 0., bottom = 0.),
                Minimum = 5.,
                Maximum = 100.,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                TickFrequency = 5.,
                IsSnapToTickEnabled = true,
                IsEnabled = true)        
        do s.SetValue(Grid.RowProperty, 0)
        let handleValueChanged (s) = 
            output_StackPanel.RenderTransform <- 
                let tranforms = TransformGroup()
                do  //tranforms.Children.Add(TranslateTransform(X = 100., Y = 100.))
                    tranforms.Children.Add(ScaleTransform(ScaleX = 20.0/s,ScaleY = 20.0/s))            
                tranforms
                
        s.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ e -> handleValueChanged (e.NewValue)))
        s
    let scaleSlider_Grid =
        let g = Grid()
        do 
            g.SetValue(DockPanel.DockProperty,Dock.Top)
            g.Children.Add(scale_Slider) |> ignore
        g 
    let canvas_DockPanel =
        let d = DockPanel()
        do  d.Children.Add(scaleSlider_Grid) |> ignore
            
            d.Children.Add(canvas) |> ignore
        d
    let screen_Grid =
        let g = Grid()
        do g.SetValue(Grid.RowProperty, 1)        
        do g.Children.Add(canvas_DockPanel) |> ignore        
        g
    
    (*Actions*)
    let setDisplayedText = 
        fun text -> result_TextBlock.Text <- text
    let fetch (text:string) =
        match kernel.IsComputing with 
        | false -> 
            do  kernel.Compute(text)                
            let image = Image()
            let out = (string) kernel.Result
            let getGraphicFrom (k:MathKernel) = 
                match k.Graphics.Length > 0 with                
                | true ->
                    image.Source <- ControlLibrary.Image.convertDrawingImage (k.Graphics.[0])
                    result_Viewbox.Child <- image
                | false -> ()
            do  setDisplayedText out
                getGraphicFrom kernel
        | true -> kernel.Abort()
    let compute = fun (text:string) -> fetch text             
    
    (**)
    do  output_ScrollViewer.Content <- output_StackPanel
        canvas.Children.Add(output_ScrollViewer) |> ignore
        this.Content <- screen_Grid

        //add event handler to each button
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> compute input_TextBox.Text))