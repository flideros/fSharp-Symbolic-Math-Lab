namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Numerics
open System.Windows
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink
open GraphicResources
open GenericDomain
open ControlDomain

type GridControl(orginPoint:SharedValue<Point>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    
    // Controls
        // Orgin and grid lines
    let orgin (p:System.Windows.Point) = 
        let radius = 8.
        let line1 = Line()
        do  line1.X1 <- p.X + 15.
            line1.X2 <- p.X - 15.
            line1.Y1 <- p.Y
            line1.Y2 <- p.Y
            line1.Stroke <- black
            line1.StrokeThickness <- 2.
        let line2 = Line()
        do  line2.X1 <- p.X
            line2.X2 <- p.X
            line2.Y1 <- p.Y + 15.
            line2.Y2 <- p.Y - 15.
            line2.Stroke <- black
            line2.StrokeThickness <- 2.
        let e = Ellipse()
        let highlight () = e.Fill <- red
        let unhighlight () = e.Fill <- clear
        do  e.Stroke <- red
            e.StrokeThickness <- 2.
            e.Opacity <- 0.4
            e.Margin <- Thickness(Left=p.X - radius, Top=p.Y - radius, Right = 0., Bottom = 0.)
            e.Width <- 2. * radius
            e.Height <- 2. * radius
            e.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            e.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
        (line1,line2,e)
    let grid = 
        let gridLines (p:System.Windows.Point) = 
            let yInterval,xInterval = 5,5
            let yOffset, xOffset = p.Y, p.X
            let lines = Image()        
            do  lines.SetValue(Panel.ZIndexProperty, -100)
        
            let gridLinesVisual = DrawingVisual() 
            let context = gridLinesVisual.RenderOpen()
                
            let rows = (int)(SystemParameters.PrimaryScreenHeight)
            let columns = (int)(SystemParameters.PrimaryScreenWidth)

            let x = System.Windows.Point(0., 0.)
            let x' = System.Windows.Point(SystemParameters.PrimaryScreenWidth, 0.)
            let y = System.Windows.Point(0.,0.)        
            let y' = System.Windows.Point(0., SystemParameters.PrimaryScreenHeight)
             
            //lines
            let horozontalLines = 
                seq{for i in 0..rows -> 
                        match i % yInterval = 0 with //Interval
                        | true -> context.DrawLine(blueGridline,x,x')
                                  x.Offset(0.,yOffset)
                                  x'.Offset(0.,yOffset)
                        | false -> context.DrawLine(redGridline,x,x')
                                   x.Offset(0.,yOffset)
                                   x'.Offset(0.,yOffset)}
            let verticalLines = 
                seq{for i in 0..columns -> 
                        match i % xInterval = 0 with //Interval
                        | true -> context.DrawLine(blueGridline,y,y')
                                  y.Offset(xOffset,0.)
                                  y'.Offset(xOffset,0.)
                        | false -> context.DrawLine(redGridline,y,y')
                                   y.Offset(xOffset,0.)
                                   y'.Offset(xOffset,0.)}        
            do  
                Seq.iter (fun x -> x) horozontalLines
                Seq.iter (fun y -> y) verticalLines
                context.Close()

            let bitmap = 
                RenderTargetBitmap(
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight, 
                    96.,
                    96.,
                    PixelFormats.Pbgra32)        
            do  
                bitmap.Render(gridLinesVisual)
                bitmap.Freeze()
                lines.Source <- bitmap
            lines
        let startPoint = System.Windows.Point(20.,20.)
        let gl = gridLines startPoint
        gl    
        // Orgin point coordinates
    let xOrgin_TextBlock =
        let l = TextBlock()
        do  //l.Margin <- Thickness(Left = 10., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 14.
            l.Width <- 50.
            l.Height <- 25.
            l.VerticalAlignment <- VerticalAlignment.Center
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "0"
        l
    let xUp_Button = 
        let b = Button()
        let handleClick () = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let x' = x + 100.
            do xOrgin_TextBlock.Text <- x'.ToString()
               orginPoint.Set (Point(x',orginPoint.Get.Y))
        do  b.Content <- "U"
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b
    let xDown_Button = 
        let b = Button()
        let handleClick () = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let x' = x - 100.
            do xOrgin_TextBlock.Text <- x'.ToString()
               orginPoint.Set (Point(x',orginPoint.Get.Y))
        do  b.Content <- "D"
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b
    let xOrgin_StackPanel =
        let sp = StackPanel()
        do  sp.Height <- 30.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Horizontal            
            sp.Children.Add(xOrgin_TextBlock) |> ignore
            sp.Children.Add(xUp_Button) |> ignore
            sp.Children.Add(xDown_Button) |> ignore
        sp
    let yOrgin_TextBlock =
        let l = TextBlock()
        do  //l.Margin <- Thickness(Left = 10., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 14.
            l.Width <- 50.
            l.Height <- 25.
            l.VerticalAlignment <- VerticalAlignment.Center
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "0"
        l
    let yUp_Button = 
        let b = Button()
        let handleClick () = 
            let y = Double.Parse yOrgin_TextBlock.Text
            let y' = y + 100.
            do yOrgin_TextBlock.Text <- y'.ToString()
               orginPoint.Set (Point(orginPoint.Get.X,y'))
        do  b.Content <- "U"
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b
    let yDown_Button = 
        let b = Button()
        let handleClick () = 
            let y = Double.Parse yOrgin_TextBlock.Text
            let y' = y - 100.
            do yOrgin_TextBlock.Text <- y'.ToString()
               orginPoint.Set (Point(orginPoint.Get.X,y'))
        do  b.Content <- "D"
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b
    let yOrgin_StackPanel =
        let sp = StackPanel()
        do  sp.Height <- 30.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Horizontal            
            sp.Children.Add(yOrgin_TextBlock) |> ignore
            sp.Children.Add(yUp_Button) |> ignore
            sp.Children.Add(yDown_Button) |> ignore
        sp
    let orgin_StackPanel = 
        let sp = StackPanel()
        let x_TextBlock = 
            let l = TextBlock()
            do  //l.Margin <- Thickness(Left = 10., Top = 50., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Normal
                l.FontSize <- 14.
                //l.Width <- 50.
                l.Height <- 25.
                l.VerticalAlignment <- VerticalAlignment.Center
                l.HorizontalAlignment <- HorizontalAlignment.Left
                //l.TextWrapping <- TextWrapping.Wrap
                l.Text <- "Orgin Point X"
            l
        let y_TextBlock = 
            let l = TextBlock()
            do  //l.Margin <- Thickness(Left = 10., Top = 50., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Normal
                l.FontSize <- 14.
                //l.Width <- 50.
                l.Height <- 25.
                l.VerticalAlignment <- VerticalAlignment.Center
                //l.TextWrapping <- TextWrapping.Wrap
                l.Text <- "Orgin Point Y"
            l
        do  sp.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(x_TextBlock) |> ignore
            sp.Children.Add(xOrgin_StackPanel) |> ignore
            sp.Children.Add(y_TextBlock) |> ignore
            sp.Children.Add(yOrgin_StackPanel) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(orgin_StackPanel) |> ignore
        g
    
    do  this.Content <- screen_Grid

    member _this.orginPoint p = orgin p
    member _this.gridLines = grid 

type WolframResultControl(wolframCode:SharedValue<string>,
                          wolframMessage:SharedValue<string>,
                          wolframResult:SharedValue<string>,
                          wolframLink:SharedValue<IKernelLink>,
                          wolframSettings:SharedValue<WolframResultControlSettings>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    
    // Wolfram Limk
    let link = wolframLink.Get
    
    // Controls
    let result_Viewbox (image:UIElement) =         
        let vb = Viewbox()   
        do  image.Opacity <- 0.85            
            vb.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            vb.IsHitTestVisible <- false
            vb.Child <- image            
        vb
    let code_TextBlock = 
        let l = TextBox()
        do  l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Ready"
            l.Visibility <- Visibility.Visible
        l 
    let message_TextBlock = 
        let l = TextBlock()
        do  l.FontStyle <- FontStyles.Normal
            l.FontSize <- 30.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Calculate Reactions"
            l.Visibility <- Visibility.Visible
            l.Background <- clear
        l
    let result_TextBlock = 
        let l = TextBox()
        do  l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- ""
            l.Visibility <- Visibility.Hidden
        l 
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Margin <- Thickness(Left = 180., Top = 10., Right = 0., Bottom = 0.)
            sp.Visibility <- Visibility.Visible
        sp
    let result_ScrollViewer = 
        let sv = new ScrollViewer();
        do  sv.VerticalScrollBarVisibility <- ScrollBarVisibility.Hidden 
            sv.MaxHeight <- 700.
            sv.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxWidth <-800.
            sv.Content <- result_StackPanel 
            sv.Visibility <- Visibility.Visible    
        sv
    // main grid
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(result_ScrollViewer) |> ignore
        g
    
    // logic
    let setGraphicsFromKernel (k:MathKernel) =        
        let code = code_TextBlock.Text 
        let rec getImages i =
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage (k.Graphics.[i])
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
            match i + 1 = k.Graphics.Length with
            | true -> ()
            | false -> getImages (i+1)
        match k.Graphics.Length > 0 with
        | true ->                                        
            let text = link.EvaluateToOutputForm("Style[" + code + ",FontSize -> 30]",pageWidth = 0)
            result_StackPanel.Children.Clear()            
            getImages 0
            result_TextBlock.Text <- text
            wolframResult.Set text
            result_StackPanel.Children.Add(message_TextBlock) |> ignore
            result_StackPanel.Children.Add(code_TextBlock) |> ignore
            result_StackPanel.Children.Add(result_TextBlock) |> ignore
        | false -> 
            result_StackPanel.Children.Clear()             
            let graphics = link.EvaluateToImage("Style[" + code + ",FontSize -> 30]", width = 0, height = 0)
            let text = link.EvaluateToOutputForm("Style[" + code + ",FontSize -> 30]",pageWidth = 0)
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
                result_TextBlock.Text <- text
                wolframResult.Set text
                result_StackPanel.Children.Add(message_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
                result_StackPanel.Children.Add(code_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_TextBlock) |> ignore
    let handleWolframCodeChange code = code_TextBlock.Text <- code
    let handleWolframMessageChange message = message_TextBlock.Text <- message
    let handleWolframResultChange result = result_TextBlock.Text <- result
    let handleWolframSettingsChange settings = 
        do  code_TextBlock.Visibility <- match wolframSettings.Get.codeVisible with | true -> Visibility.Visible | false -> Visibility.Collapsed
            result_TextBlock.Visibility <- match wolframSettings.Get.resultVisible with | true -> Visibility.Visible | false -> Visibility.Collapsed
            result_ScrollViewer.IsHitTestVisible <- wolframSettings.Get.isHitTestVisible
    
    do  this.Content <- screen_Grid
        wolframCode.Changed.Add handleWolframCodeChange 
        wolframResult.Changed.Add handleWolframResultChange 
        wolframMessage.Changed.Add handleWolframMessageChange 
        wolframSettings.Changed.Add handleWolframSettingsChange
        handleWolframCodeChange wolframCode.Get
        handleWolframResultChange wolframResult.Get
        handleWolframMessageChange wolframMessage.Get
        handleWolframSettingsChange wolframSettings.Get

    member _this.result image = result_Viewbox image
    member _this.setGraphics k = setGraphicsFromKernel k

type SelectionControl (orginPoint:SharedValue<Point>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    
    // Controls    
    let selectionMode_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Selection Mode"
        l
    let delete_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Delete", FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
        r
    let inspect_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Inspect", FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let modify_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Modify", FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    (*let delete_Button = 
        let b = Button()
        let handleClick () = 
            let newState = trussServices.removeTrussPartFromTruss state            
            do  state <- newState 
                label.Text <- newState.ToString()
        do  b.Content <- "Delete"
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Bold
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b*)
    let newPX_TextBlock = 
        let tb = TextBlock(Text = "X")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let newPX_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let newPY_TextBlock = 
        let tb = TextBlock(Text = "Y")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let newPY_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let newP_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(newPX_TextBlock) |> ignore
            sp.Children.Add(newPX_TextBox) |> ignore
            sp.Children.Add(newPY_TextBlock) |> ignore
            sp.Children.Add(newPY_TextBox) |> ignore            
            sp.Visibility <- Visibility.Collapsed
        sp    
    let newFMag_TextBlock = 
        let tb = TextBlock(Text = "Magnitude")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let newFMag_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let newFDir_TextBlock = 
        let tb = TextBlock(Text = "Direction (Degrees)")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let newFDir_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let newF_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(newFMag_TextBlock) |> ignore
            sp.Children.Add(newFMag_TextBox) |> ignore
            sp.Children.Add(newFDir_TextBlock) |> ignore
            sp.Children.Add(newFDir_TextBox) |> ignore            
            sp.Visibility <- Visibility.Collapsed
        sp    
    let selectionMode_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(selectionMode_Label) |> ignore
            sp.Children.Add(delete_RadioButton) |> ignore
            sp.Children.Add(inspect_RadioButton) |> ignore
            sp.Children.Add(modify_RadioButton) |> ignore
            //sp.Children.Add(delete_Button) |> ignore
            sp.Children.Add(newP_StackPanel) |> ignore
            sp.Children.Add(newF_StackPanel) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
    
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(selectionMode_StackPanel) |> ignore
        g
    
    do  this.Content <- screen_Grid
