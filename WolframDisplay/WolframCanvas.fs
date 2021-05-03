namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

(*Test Area*)
type WolframCanvas() as this  =  
    inherit UserControl()
    do Install() |> ignore
    
    (*Wolfram Kernel*)
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.2/WolframKernel.exe\"")
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
            k.PageWidth <- 200;
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
            tb.Text <- "$InverseTrigFunctions = {ArcSin, ArcCos, ArcSec, ArcCsc, ArcTan,ArcCot, ArcSinh, ArcCosh, ArcSech, ArcCsch, ArcTanh, ArcCoth}; \r\n Table[DensityPlot[Im[f[(x + I y)^3]], {x, -2, 2}, {y, -2, 2},ColorFunction -> \"Pastel\", \n ExclusionsStyle -> {None, Purple},Mesh -> None, PlotLabel -> Im[f[(x + I y)^3]],Ticks -> None], {f, $InverseTrigFunctions}]"
        tb 
    let compute_Button = 
        Button( Name = "Button",
                Content = "Compute",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 53.,
                Height = 33.,
                Background = Brushes.Aqua)
    let input_StackPanel = 
        let sp = StackPanel()
        do  sp.Children.Add(compute_Button) |> ignore
            sp.Children.Add(input_TextBox) |> ignore
            sp.Orientation <- Orientation.Vertical
        sp
    
    let result_TextBlock =                    
        let tb = TextBlock()
        tb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        tb.FontStyle <- FontStyles.Normal
        tb.FontSize <- 14.
        tb
    let result_Viewbox image =                    
        let vb = Viewbox()   
        do  vb.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 20.)
            vb.Child <- image
        vb
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
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
            //sp.Children.Add(result_Viewbox) |> ignore
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
        do  s.SetValue(Grid.RowProperty, 0)
            s.Visibility <- Visibility.Collapsed
        let handleValueChanged (s) = 
            result_StackPanel.RenderTransform <- 
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
    let setGraphicsFrom (k:MathKernel) = 
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
    let setPngGraphics (k:MathKernel) = 
        let outText = k.Result.ToString().Contains("-Graphics-") || k.Result.ToString().Contains("-Image-")
        let grahicsCount = k.Graphics.Length
        match outText && grahicsCount = 0 with
        | true -> 
            kernel.Compute("Export[\"file2.png\","+ input_TextBox.Text + "]; \r Show[Import[\"file2.png\"]]")
            messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
            print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""            
            result_TextBlock.Text <- (string) kernel.Result
            setGraphicsFrom kernel 
        | false -> ()
    let fetch (text:string) =
        do compute_Button.Content <- "Working"
        match kernel.IsComputing with 
        | false -> 
            do  kernel.Compute(text)            
                messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
                print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""            
                result_TextBlock.Text <- (string) kernel.Result
                setGraphicsFrom kernel
                setPngGraphics kernel
                compute_Button.Content <- "Compute"
        | true -> 
            do  kernel.Abort()
                messages_TextBlock.Text <- kernel.Messages |> Seq.fold (+) ""
                print_TextBlock.Text <- kernel.PrintOutput |> Seq.fold (+) ""
            compute_Button.Content <- "Compute"
    let compute = fun (text:string) -> fetch text             
    
    (**)
    do  output_ScrollViewer.Content <- output_StackPanel
        canvas.Children.Add(output_ScrollViewer) |> ignore
        this.Content <- screen_Grid

        //add event handler to each button
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> compute input_TextBox.Text))