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
    
    (*Image Converter
        One-way converter from System.Drawing.Image to System.Windows.Media.ImageSource*)
    let convertDrawingImage (image:System.Drawing.Image) =
        let bitmap = new System.Windows.Media.Imaging.BitmapImage()
        let memoryStream = new System.IO.MemoryStream()
        do  bitmap.BeginInit()
            image.Save(memoryStream, image.RawFormat)
            memoryStream.Seek(int64 0, System.IO.SeekOrigin.Begin) |> ignore
            bitmap.StreamSource <- memoryStream
            bitmap.EndInit()
        bitmap
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
    let compute_Button = 
        Button( Name = "Button",
                Content = "Press me",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = Brushes.Aqua)
    let result_textBlock =                    
        let tb = TextBlock()
        //tb.Text <- "Random stuff"
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 20.
        tb
    let result_Viewbox =                    
        let vb = Viewbox()   
        vb
    
    
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
            result_textBlock.RenderTransform <- 
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
    (*Actions*)
    let setDisplayedText = 
        fun text -> result_textBlock.Text <- text
    let fetch (text:string) =
        match kernel.IsComputing with 
        | false -> 
            do  kernel.Compute(text)                
            let image = Image()
            let out = (string) kernel.Result
            let getGraphicFrom (k:MathKernel) = 
                match k.Graphics.Length > 0 with                
                | true ->
                    image.Source <- convertDrawingImage (k.Graphics.[0])
                    result_Viewbox.Child <- image
                    canvas.Children.Add(result_Viewbox) |> ignore
                | false -> ()
            do  setDisplayedText out
                getGraphicFrom kernel
        | true -> kernel.Abort()
    let compute = fun (text:string) -> fetch text             
    (**)
    do  canvas.Children.Add(result_textBlock) |> ignore
        canvas.Children.Add(compute_Button) |> ignore
        
        this.Content <- screen_Grid

        //add event handler to each button
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> compute "Plot[Sin[x], {x, 0, 6 Pi}]"))