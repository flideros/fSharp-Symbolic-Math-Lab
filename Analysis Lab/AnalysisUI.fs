﻿namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Numerics
open System.Windows
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink
open GraphicResources

type Analysis() as this =  
    inherit UserControl()    
    do Install() |> ignore
    

    let initialState = TrussAnalysisDomain.TrussState {truss = {members=[]; forces=[]; supports=[]}; mode = TrussAnalysisDomain.MemberBuild}

    let mutable state = initialState
    
    (*Wolfram Kernel*)
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-WSTP -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.3/WolframKernel.exe\"")
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
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()

    (*Controls*)      
    let label =
        let l = TextBox()
        do  l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- state.ToString()
            l.BorderBrush <- SolidColorBrush(Colors.Transparent)
            l.Opacity <- 0.5
            l.MaxLines <- 30
        l 
        // Analysis State
    let axis_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Moment Axis"
        l
    let xAxis_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="X Axis",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let yAxis_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Y Axis",FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
            r.Margin <- Thickness(Left = 5., Top = 0., Right = 0., Bottom = 0.)
        r
    let momentAxisRadio_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 5., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Horizontal
            sp.Children.Add(xAxis_RadioButton) |> ignore
            sp.Children.Add(yAxis_RadioButton) |> ignore
        sp
    let compute_Button = 
        let b = Button()        
        do  b.Content <- "Compute"
            b.FontSize <- 18.
            b.FontWeight <- FontWeights.Bold
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
        b
    let resultant_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Resultant",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let components_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Components",FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
            r.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
        r
    let reaction_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Reaction View"
        l
    let reactionRadio_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 5., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(reaction_Label) |> ignore
            sp.Children.Add(resultant_RadioButton) |> ignore
            sp.Children.Add(components_RadioButton) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
    let legend_StackPanel = 
        let zeroLegend = 
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = 0., Top = 5., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Normal
                l.FontWeight <- FontWeights.Black
                l.FontSize <- 15.
                l.MaxWidth <- 500.
                l.Text <- "Zero Force"  
                l.Foreground <- olive
                l.TextAlignment <- TextAlignment.Center
                //l.Opacity <- 0.5                
            l 
        let compressionLegend = 
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Normal
                l.FontWeight <- FontWeights.Black
                l.FontSize <- 15.
                l.MaxWidth <- 500.
                l.Text <- "Compression"
                l.Foreground <- red
                l.TextAlignment <- TextAlignment.Center
                //l.Opacity <- 0.5                
            l 
        let tensionLegend = 
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 5.)
                l.FontStyle <- FontStyles.Normal
                l.FontWeight <- FontWeights.Black
                l.FontSize <- 15.
                l.MaxWidth <- 500.
                l.Text <- "Tension"
                l.Foreground <- blue2
                l.TextAlignment <- TextAlignment.Center
                //l.Opacity <- 0.5                
            l 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 5., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(zeroLegend) |> ignore
            sp.Children.Add(compressionLegend) |> ignore
            sp.Children.Add(tensionLegend) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
    let analysis_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(legend_StackPanel) |> ignore
            sp.Children.Add(compute_Button) |> ignore
            sp.Children.Add(axis_Label) |> ignore            
            sp.Children.Add(momentAxisRadio_StackPanel) |> ignore            
            sp.Children.Add(reactionRadio_StackPanel) |> ignore            
            sp.Visibility <- Visibility.Collapsed
        sp
        // Orgin and grid
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
            l.Text <- "200"
        l
    let xUp_Button = 
        let b = Button()
        let handleClick () = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let x' = x + 100.
            do xOrgin_TextBlock.Text <- x'.ToString()
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
            l.Text <- "500"
        l
    let yUp_Button = 
        let b = Button()
        let handleClick () = 
            let y = Double.Parse yOrgin_TextBlock.Text
            let y' = y + 100.
            do yOrgin_TextBlock.Text <- y'.ToString()
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
        // Truss mode selection
    let trussMode_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Truss Mode"
        l
    let forceBuilder_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Build Force",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let memberBuilder_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Build Member",FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
        r
    let supportBuilder_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Build Support",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let selection_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Select Parts",FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let analysis_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Analysis",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let settings_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Settings",FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let trussMode_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_Label) |> ignore
            sp.Children.Add(memberBuilder_RadioButton) |> ignore
            sp.Children.Add(forceBuilder_RadioButton) |> ignore
            sp.Children.Add(supportBuilder_RadioButton) |> ignore
            sp.Children.Add(selection_RadioButton) |> ignore
            sp.Children.Add(analysis_RadioButton) |> ignore
            sp.Children.Add(settings_RadioButton) |> ignore
        sp
        // Support type selection
    let supportType_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Support Type"
        l
    let pinSupport_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Pin Support", FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let rollerSupport_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Roller Support", FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
        r    
    let supportType_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(supportType_Label) |> ignore
            sp.Children.Add(pinSupport_RadioButton) |> ignore
            sp.Children.Add(rollerSupport_RadioButton) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
        // Selection mode selection
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
    let delete_Button = 
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
        b
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
            sp.Children.Add(delete_Button) |> ignore
            sp.Children.Add(newP_StackPanel) |> ignore
            sp.Children.Add(newF_StackPanel) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
        // Member builder P1
    let trussMemberP1X_TextBlock = 
        let tb = TextBlock(Text = "X1")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussMemberP1X_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let trussMemberP1Y_TextBlock = 
        let tb = TextBlock(Text = "Y1")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussMemberP1Y_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let trussMemberP1_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMemberP1X_TextBlock) |> ignore
            sp.Children.Add(trussMemberP1X_TextBox) |> ignore
            sp.Children.Add(trussMemberP1Y_TextBlock) |> ignore
            sp.Children.Add(trussMemberP1Y_TextBox) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
        // Member builder P2
    let trussMemberP2X_TextBlock = 
        let tb = TextBlock(Text = "X2")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussMemberP2X_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let trussMemberP2Y_TextBlock = 
        let tb = TextBlock(Text = "Y2")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussMemberP2Y_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb 
    let trussMemberP2_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMemberP2X_TextBlock) |> ignore
            sp.Children.Add(trussMemberP2X_TextBox) |> ignore
            sp.Children.Add(trussMemberP2Y_TextBlock) |> ignore
            sp.Children.Add(trussMemberP2Y_TextBox) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
    let trussMemberBuilder_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMemberP1_StackPanel) |> ignore
            sp.Children.Add(trussMemberP2_StackPanel) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
        // Force builder    
    let trussForceAngle_TextBlock = 
        let tb = TextBlock(Text = "Angle (Degrees)")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussForceAngle_TextBox = 
        let tb = TextBox()
        let mouseDown () = 
            match Double.TryParse tb.Text with
            | false,_ -> tb.Text <- ""
            | true,_ -> ()
        do  tb.MaxLines <- 15
            tb.TabIndex <- 0
            tb.IsReadOnly <- false
            tb.BorderThickness <- Thickness(3.)
            tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ ->mouseDown()))
        tb
    let trussForceMagnitude_TextBlock = 
        let tb = TextBlock(Text = "Magnitude")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let trussForceMagnitude_TextBox = 
        let tb = TextBox()
        let mouseDown () = 
            match Double.TryParse tb.Text with
            | false,_ -> tb.Text <- ""
            | true,_ -> ()
        do  tb.MaxLines <- 15
            tb.TabIndex <- 0
            tb.IsReadOnly <- false
            tb.BorderThickness <- Thickness(3.)
            tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ ->mouseDown()))
        tb    
    let trussForceBuilder_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussForceAngle_TextBlock) |> ignore
            sp.Children.Add(trussForceAngle_TextBox) |> ignore
            sp.Children.Add(trussForceMagnitude_TextBlock) |> ignore
            sp.Children.Add(trussForceMagnitude_TextBox) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
        // Truss parts
    let trussJoint (p:System.Windows.Point) = 
        let radius = 6.
        let e = Ellipse()
        let highlight () = e.Fill <- blue 
        let unhighlight () = e.Fill <- clear
        do  e.Stroke <- blue
            e.Margin <- Thickness(Left=p.X - radius, Top=p.Y - radius, Right = 0., Bottom = 0.)
            e.Width <- 2. * radius
            e.Height <- 2. * radius
            e.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            e.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
        e
    let trussMemberSelected (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line() 
        do  l.Stroke <- black
            l.StrokeThickness <- 4.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y            
        l 
    let trussMemberSolved (p1:System.Windows.Point, p2:System.Windows.Point) color = 
        let l = Line() 
        do  l.Stroke <- color
            l.StrokeThickness <- 4.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
            l.Opacity <- 0.5
        l 
    let trussMember (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line() 
        let sendLineToState l s =  
            let newState = 
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendMemberToState l TrussAnalysisDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendMemberToState l TrussAnalysisDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendMemberToState l TrussAnalysisDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
            do  state <- newState
                label.Text <- state.ToString()
        let highlight () = 
            l.Stroke <- blue 
            l.StrokeThickness <- 4.0
        let unhighlight () = 
            l.Stroke <- black
            l.StrokeThickness <- 1.0
        do  l.Stroke <- black
            l.StrokeThickness <- 1.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
            l.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            l.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
            l.MouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> sendLineToState l state ))
        l    
    let trussForceJoint (p:System.Windows.Point) = 
        let radius = 4.
        let e = Ellipse()
        let highlight () = e.Fill <- clear 
        let unhighlight () = e.Fill <- green
        do  e.Stroke <- blue
            e.Margin <- Thickness(Left=p.X - radius, Top=p.Y - radius, Right = 0., Bottom = 0.)
            e.Width <- 2. * radius
            e.Height <- 2. * radius
            e.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            e.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
        e
    let trussForceSelected (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line() 
        do  l.Stroke <- red
            l.StrokeThickness <- 4.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y            
        l    
    let trussForce color (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line()
        let sendLineToState l s =  
            let newState = 
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendForceToState l TrussAnalysisDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendForceToState l TrussAnalysisDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendForceToState l TrussAnalysisDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
            do  state <- newState
                label.Text <- state.ToString()
        let highlight () = 
            l.Stroke <- red 
            l.StrokeThickness <- 4.0
        let unhighlight () = 
            l.Stroke <- black
            l.StrokeThickness <- 2.0
        do  l.Stroke <- color
            l.StrokeThickness <- 2.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
            l.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            l.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
            l.MouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> sendLineToState l state ))
        l
    let trussForceDirection color (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line()        
        do  l.Stroke <- color
            l.StrokeThickness <- 2.0
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
        l
    let trussSupportJoint (p:System.Windows.Point) = 
        let radius = 8.
        let e = Ellipse()
        let highlight () = e.Fill <- olive 
        let unhighlight () = e.Fill <- clear
        do  e.Stroke <- blue
            e.Margin <- Thickness(Left=p.X - radius, Top=p.Y - radius, Right = 0., Bottom = 0.)
            e.Width <- 2. * radius
            e.Height <- 2. * radius
            e.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            e.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
        e
    let trussSupportSelected () =
        let path = Path()  
        do  path.Stroke <- red
            path.Fill <- olive
            path.Opacity <- 0.5
            path.StrokeThickness <- 4.
        path
    let support () =
        let path = Path()            
        let sendPathToState p s =  
            let newState = 
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendSupportToState p TrussAnalysisDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendSupportToState p TrussAnalysisDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendSupportToState p TrussAnalysisDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
            do  state <- newState
                label.Text <- state.ToString()
        let highlight () = 
            path.Stroke <- red 
            path.StrokeThickness <- 4.0
        let unhighlight () = 
            path.Stroke <- black
            path.StrokeThickness <- 2.0
        do  path.Stroke <- black
            path.Fill <- olive
            path.Opacity <- 0.5
            path.StrokeThickness <- 1.
            path.MouseEnter.AddHandler(Input.MouseEventHandler(fun _ _ -> highlight ()))
            path.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> unhighlight ()))
            path.MouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> sendPathToState path state ))
        path
        // Wolfram result 
    let result_Viewbox (image:UIElement) =         
        let vb = Viewbox()   
        do  image.Opacity <- 0.85            
            vb.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            vb.IsHitTestVisible <- false
            vb.Child <- image            
        vb
    let code_TextBlock = 
        let l = TextBox()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Ready"
            l.Visibility <- Visibility.Hidden
        l 
    let message_TextBlock = 
        let l = TextBlock()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 30.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Calculate Reactions"
            l.Visibility <- Visibility.Visible
            l.Background <- clear
        l
    let result_TextBlock = 
        let l = TextBox()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
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
            sv.MaxHeight <- 1000.
            sv.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxWidth <-800.
            sv.Content <- result_StackPanel
            sv.SetValue(Canvas.ZIndexProperty,3) 
            sv.Visibility <- Visibility.Collapsed    
        sv

        // Main canvas    
    let canvas = 
        let c = Canvas()        
        do  c.Background <- System.Windows.Media.Brushes.White 
            c.ClipToBounds <- true
            c.Cursor <- System.Windows.Input.Cursors.Cross
            c.Children.Add(label) |> ignore             
        c
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(canvas) |> ignore
        g

        // Settings
    let toggleCodeText_Button = 
        let b = Button() 
        let onOff() = 
            match b.Content.ToString() with 
            | "Code Text Off" -> 
                do  code_TextBlock.Visibility <- Visibility.Visible
                    b.Content <- "Code Text On" 
            | "Code Text On" -> 
                do  code_TextBlock.Visibility <- Visibility.Collapsed
                    b.Content <- "Code Text Off" 
            | _ -> ()
        do  b.Content <- "Code Text Off" 
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Regular
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
            b.Visibility <- Visibility.Visible
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> onOff() ))
        b
    let toggleResultText_Button = 
        let b = Button() 
        let onOff() = 
            match b.Content.ToString() with 
            | "Result Text Off" -> 
                do  result_TextBlock.Visibility <- Visibility.Visible
                    b.Content <- "Result Text On" 
            | "Result Text On" -> 
                do  result_TextBlock.Visibility <- Visibility.Collapsed
                    b.Content <- "Result Text Off" 
            | _ -> ()
        do  b.Content <- "Result Text Off" 
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Regular
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
            b.Visibility <- Visibility.Visible
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> onOff() ))
        b
    let toggleStateText_Button = 
        let b = Button() 
        let onOff() = 
            match b.Content.ToString() with 
            | "State Text Off" -> 
                do  label.Visibility <- Visibility.Visible
                    b.Content <- "State Text On" 
            | "State Text On" -> 
                do  label.Visibility <- Visibility.Collapsed
                    b.Content <- "State Text Off" 
            | _ -> ()
        do  b.Content <- "State Text On" 
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Regular
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
            b.Visibility <- Visibility.Visible
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> onOff() ))
        b
    let settings_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            sp.Visibility <- Visibility.Collapsed
            sp.Children.Add(toggleCodeText_Button) |> ignore
            sp.Children.Add(toggleResultText_Button) |> ignore
            sp.Children.Add(toggleStateText_Button) |> ignore
            sp.Children.Add(orgin_StackPanel) |> ignore

        sp  
        // Controls border
    let trussControls_Border = 
        let border = Border()            
        do  border.BorderBrush <- black
            border.Cursor <- System.Windows.Input.Cursors.Arrow
            border.Background <- clear
            border.Opacity <- 0.8
            border.IsHitTestVisible <- true
            border.BorderThickness <- Thickness(Left = 1., Top = 1., Right = 1., Bottom = 1.)
            border.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            border.SetValue(Canvas.ZIndexProperty,4)
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 10., Right = 10., Bottom = 10.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_StackPanel) |> ignore
            sp.Children.Add(trussMemberBuilder_StackPanel) |> ignore
            sp.Children.Add(supportType_StackPanel) |> ignore
            sp.Children.Add(trussForceBuilder_StackPanel) |> ignore
            sp.Children.Add(settings_StackPanel) |> ignore
            sp.Children.Add(selectionMode_StackPanel) |> ignore
            sp.Children.Add(analysis_StackPanel) |> ignore
            sp.SetValue(Canvas.ZIndexProperty,4)
            border.Child <- sp
        border    
    (*Actions*) 
    let adjustMouseButtonEventArgPoint (e:Input.MouseButtonEventArgs) = 
        let p = e.GetPosition(this)
        System.Windows.Point(p.X,(p.Y - 0.04)) // not sure why the mouse position adds this to the point
    let adjustMouseEventArgPoint (e:Input.MouseEventArgs) = 
        let p = e.GetPosition(this)
        System.Windows.Point(p.X,(p.Y - 0.04)) // not sure why the mouse position adds this to the point
        // Get
    let getBitmapFrom visual = 
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
    let getTrussFrom s = trussServices.getTrussFromState s
    let getJointsFrom s = trussServices.getJointSeqFromTruss (getTrussFrom s)
    let getMembersFrom s = trussServices.getMemberSeqFromTruss (getTrussFrom s)
    let getForcesFrom s = (trussServices.getTrussFromState s).forces 
    let getSupportsFrom s = (trussServices.getTrussFromState s).supports
    let getJointIndex (p1:System.Windows.Point) = 
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) (getJointsFrom state)
        // Draw
    let drawOrgin (p:System.Windows.Point) = 
        let l1,l2,e = orgin p
        do  canvas.Children.Add(e) |> ignore
            canvas.Children.Add(l1) |> ignore
            canvas.Children.Add(l2) |> ignore
    let drawJoint (p:System.Windows.Point) =
        let j = trussJoint p
        do  canvas.Children.Add(j) |> ignore
    let drawMember (p1:System.Windows.Point, p2:System.Windows.Point) =
        let l = trussMember (p1,p2)
        do  canvas.Children.Add(l) |> ignore
    let drawSolvedMember (p1:System.Windows.Point, p2:System.Windows.Point) color =        
        let l = trussMemberSolved (p1,p2)  color
        do  canvas.Children.Add(l) |> ignore
    let drawSolvedMembers (state:TrussAnalysisDomain.TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.AnalysisState a -> 
            match a.analysis with
            | TrussAnalysisDomain.Truss -> ()
            | TrussAnalysisDomain.SupportReactionEquations _sre -> () 
            | TrussAnalysisDomain.SupportReactionResult _srr-> ()                    
            | TrussAnalysisDomain.MethodOfJointsCalculation mjc ->                     
                do  List.iter (fun  (n, p) -> 
                    match n, TrussServices.getMemberOptionFromTrussPart p with
                    | (z, Some m) when z = 0. -> drawSolvedMember m olive
                    | (z, Some m) when z > 0. -> drawSolvedMember m blue2
                    | (z, Some m) when z < 0. -> drawSolvedMember m red
                    | _ -> ()) mjc.solvedMembers
            | TrussAnalysisDomain.MethodOfJointsAnalysis mja ->                     
                do  List.iter (fun  (n, p) -> 
                    match n, TrussServices.getMemberOptionFromTrussPart p with
                    | (z, Some m) -> drawSolvedMember m blue2
                    | _ -> ()) mja.tensionMembers
                do  List.iter (fun  (n, p) -> 
                    match n, TrussServices.getMemberOptionFromTrussPart p with
                    | (z, Some m) -> drawSolvedMember m red
                    | _ -> ()) mja.compressionMembers
                do  List.iter (fun  p -> 
                    match TrussServices.getMemberOptionFromTrussPart p with
                    | Some m -> drawSolvedMember m olive
                    | None -> ()) mja.zeroForceMembers
        | _ -> ()
        
    let drawMemberLabel (p1:System.Windows.Point, p2:System.Windows.Point) (i:int) =
        let l =
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = (p1.X + p2.X)/2., Top = (p1.Y + p2.Y)/2., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Italic
                l.FontSize <- 15.
                l.FontWeight <- FontWeights.Bold
                l.TextAlignment <- TextAlignment.Center
                l.MaxWidth <- 50.                
                l.Text <- "M" + i.ToString()                
                l.Background <- SolidColorBrush(Colors.Transparent)
            l 
        do  canvas.Children.Add(l) |> ignore 
    let drawForceLabel (p:System.Windows.Vector) (i:int) (f:float) =
           let l =
               let l = TextBlock()
               do  l.Margin <- Thickness(Left = p.X, Top = p.Y, Right = 0., Bottom = 0.)
                   l.FontStyle <- FontStyles.Italic
                   l.FontSize <- 15.
                   l.FontWeight <- FontWeights.Bold
                   l.TextAlignment <- TextAlignment.Right
                   l.MaxWidth <- 50.                
                   l.Text <- "F" + i.ToString()                
                   l.Background <- SolidColorBrush(Colors.Transparent)
               l 
           do  canvas.Children.Add(l) |> ignore  
    let drawReactionForceLabel (p:System.Windows.Vector) ((i,s):int*string) (f:float) =
        
        let l =
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = p.X - 20., Top = p.Y - 20., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Italic
                l.FontSize <- 15.
                l.FontWeight <- FontWeights.Bold
                l.TextAlignment <- TextAlignment.Right
                l.MaxWidth <- 50.                
                l.Text <- s + i.ToString()                
                l.Background <- SolidColorBrush(Colors.Transparent)
            l 
        do  canvas.Children.Add(l) |> ignore  

    let drawBuildJoint (p:System.Windows.Point) =
        let j = trussJoint p
        do  j.Stroke <- red
            canvas.Children.Add(j) |> ignore
    let drawBuildForceJoint (p:System.Windows.Point) =
        let j = trussForceJoint p
        do  //j.Stroke <- red
            canvas.Children.Add(j) |> ignore 
    let drawBuildSupportJoint (p:System.Windows.Point) =
        let j = trussSupportJoint p
        do  //j.Stroke <- red
            canvas.Children.Add(j) |> ignore
    let drawBuildForceLine color (p1:System.Windows.Point, p2:System.Windows.Point) =
        let l = trussForce color (p1,p2)
        do  canvas.Children.Add(l) |> ignore     
    let drawBuildForceDirection color (p:System.Windows.Point, dir:float, mag:float) =
        let angle = 8.
        let length = 25.
        match mag > 0. with
        | true -> 
            let p1 = System.Windows.Point(p.X + (length * cos ((dir - angle) * Math.PI/180.)), p.Y - (length * sin ((dir - angle) * Math.PI/180.)))
            let p2 = System.Windows.Point(p.X + (length * cos ((dir + angle) * Math.PI/180.)), p.Y - (length * sin ((dir + angle) * Math.PI/180.)))
            let l1 = trussForceDirection color (p,p1)
            let l2 = trussForceDirection color (p,p2)
            let l3 = trussForceDirection color (p1,p2)
            do  canvas.Children.Add(l1) |> ignore
                canvas.Children.Add(l2) |> ignore
                canvas.Children.Add(l3) |> ignore
        | false -> 
            let p1 = System.Windows.Point(p.X - (length * cos ((dir - angle) * Math.PI/180.)), p.Y + (length * sin ((dir - angle) * Math.PI/180.)))
            let p2 = System.Windows.Point(p.X - (length * cos ((dir + angle) * Math.PI/180.)), p.Y + (length * sin ((dir + angle) * Math.PI/180.)))
            let l1 = trussForceDirection color (p,p1)
            let l2 = trussForceDirection color (p,p2)
            let l3 = trussForceDirection color (p1,p2)
            do  canvas.Children.Add(l1) |> ignore
                canvas.Children.Add(l2) |> ignore
                canvas.Children.Add(l3) |> ignore        
    let drawBuildSupport (p:System.Windows.Point, dir:float, isRollerSupportType: bool) =
        let angle = 45.
        let length = 25.         
        let p1 = System.Windows.Point(p.X + (length * cos ((dir - angle) * Math.PI/180.)), p.Y - (length * sin ((dir - angle) * Math.PI/180.)))
        let p2 = System.Windows.Point(p.X + (length * cos ((dir + angle) * Math.PI/180.)), p.Y - (length * sin ((dir + angle) * Math.PI/180.)))
        let support =             
            let pg = PathGeometry()
            let pfc = PathFigureCollection()
            let pf = PathFigure()
            let psc = PathSegmentCollection()
            let l1 = LineSegment(Point=p1)
            let l2 = LineSegment(Point=p2)
            let a2 = ArcSegment(Point=p2,IsLargeArc=false,Size=Size(30.,30.))
            let l3 = LineSegment(Point=p)
            let support = support ()            
            do  psc.Add(l1)
                match isRollerSupportType with
                | true -> psc.Add(a2)
                | false -> psc.Add(l2)                    
                psc.Add(l3)
                pf.Segments <- psc
                pf.StartPoint <- p
                pfc.Add(pf)
                pg.Figures <- pfc
                support.Data <- pg
                support.Tag <- p
            support
        do  canvas.Children.Add(support) |> ignore
    let drawSupport (support:ElementDomain.Support) = 
        let p = trussServices.getPointFromSupport support
        let dir = trussServices.getDirectionFromSupport support
        let isRollerSupportType = trussServices.checkSupportTypeIsRoller support
        do  drawBuildSupportJoint p
            drawBuildSupport (p,dir,isRollerSupportType)
    let drawForce color (force:LoadDomain.JointForce) =  
        let p = trussServices.getPointFromForce force
        let vp = System.Windows.Point(force.direction.X,force.direction.Y)        
        let arrowPoint = 
            match force.magnitude > 0. with 
            | true -> p
            | false -> vp
        let dir = trussServices.getDirectionFromForce force
        do  drawBuildForceJoint p
            drawBuildForceLine color (p,vp)
            drawBuildForceDirection color (arrowPoint,dir,force.magnitude)
    
    let drawSelectedMember s =
        let selectedMember = trussServices.getSelectedMemberFromState
        match selectedMember s with 
        | None -> () 
        | Some m -> 
            let p1,p2 = m
            let l = trussMemberSelected (p1,p2)
            do  canvas.Children.Add(l) |> ignore
    let drawSelectedForce s =
        let selectedForce = trussServices.getSelectedForceFromState
        match selectedForce s with 
        | None -> () 
        | Some f -> 
            let p1,p2 = f
            let l = trussForceSelected (p1,p2)
            do  canvas.Children.Add(l) |> ignore
    let drawSelectedSupport s =
        let selectedSupport = trussServices.getSelectedSupportFromState
        match selectedSupport s with 
        | None -> () 
        | Some s -> 
            let p,dir,isRollerSupportType = s
            let angle = 45.
            let length = 25.         
            let p1 = System.Windows.Point(p.X + (length * cos ((dir - angle ) * Math.PI/180.)), p.Y - (length * sin ((dir - angle) * Math.PI/180.)))
            let p2 = System.Windows.Point(p.X + (length * cos ((dir + angle ) * Math.PI/180.)), p.Y - (length * sin ((dir + angle) * Math.PI/180.)))
            let support =             
                let pg = PathGeometry()
                let pfc = PathFigureCollection()
                let pf = PathFigure()
                let psc = PathSegmentCollection()
                let l1 = LineSegment(Point=p1)
                let l2 = LineSegment(Point=p2)
                let a2 = ArcSegment(Point=p2,IsLargeArc=false,Size=Size(30.,30.))
                let l3 = LineSegment(Point=p)
                let support = trussSupportSelected ()            
                do  psc.Add(l1)
                    match isRollerSupportType with
                    | true -> psc.Add(a2)
                    | false -> psc.Add(l2)                    
                    psc.Add(l3)
                    pf.Segments <- psc
                    pf.StartPoint <- p
                    pfc.Add(pf)
                    pg.Figures <- pfc
                    support.Data <- pg
                    //support.Tag <- p
                support
            do  canvas.Children.Add(support) |> ignore
    let drawTruss s =
        let orginPoint = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let y = Double.Parse yOrgin_TextBlock.Text
            System.Windows.Point(x,y)        
        do  canvas.Children.Clear()
            canvas.Children.Add(grid) |> ignore
            drawOrgin orginPoint
            canvas.Children.Add(label) |> ignore
            canvas.Children.Add(result_ScrollViewer) |> ignore
            canvas.Children.Add(trussControls_Border) |> ignore
        let joints = getJointsFrom s
        let members = getMembersFrom s 
        let forces = List.toSeq (getForcesFrom s)
        let supports = List.toSeq (getSupportsFrom s)
        Seq.iter (fun m -> drawMember m) members
        //Seq.iteri (fun i m -> drawMemberLabel m i) members
        Seq.iter (fun j -> drawJoint j) joints
        Seq.iter (fun f -> drawForce green f) forces
        Seq.iter (fun s -> drawSupport s) supports
        drawSelectedMember s
        drawSelectedForce s
        drawSelectedSupport s
        drawSolvedMembers s

        // Set
    let setOrgin p = 
        let joints = getJointsFrom state |> Seq.toList
        let selectedJoint = getJointIndex p
        match selectedJoint with 
        | None -> 
            do  xOrgin_TextBlock.Text <- "0."
                yOrgin_TextBlock.Text <- "0."
        | Some i -> 
            do  xOrgin_TextBlock.Text <- joints.[i].X.ToString()
                yOrgin_TextBlock.Text <- joints.[i].Y.ToString()
    let setGraphicsFromKernel (k:MathKernel) =        
        let code = code_TextBlock.Text // trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value state  //
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
                result_StackPanel.Children.Add(message_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
                result_StackPanel.Children.Add(code_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_TextBlock) |> ignore
    let setStateFromAnaysis s =
        do  setGraphicsFromKernel kernel
        let newState = 
            match s with 
            | TrussAnalysisDomain.AnalysisState a -> 
                match a.analysis with
                | TrussAnalysisDomain.Truss -> s
                | TrussAnalysisDomain.SupportReactionEquations _r -> trussServices.analyzeEquations result_TextBlock.Text s
                | TrussAnalysisDomain.SupportReactionResult _r -> s 
                | TrussAnalysisDomain.MethodOfJointsCalculation _mj -> trussServices.analyzeEquations result_TextBlock.Text s
                | TrussAnalysisDomain.MethodOfJointsAnalysis _mj -> s                
            | _ -> s        
        let newCode = 
            match newState with 
            | TrussAnalysisDomain.AnalysisState a -> 
                match a.analysis with
                | TrussAnalysisDomain.Truss -> ""
                | TrussAnalysisDomain.SupportReactionEquations r -> "" 
                | TrussAnalysisDomain.SupportReactionResult r -> "\"Choose a joint to begin Method of Joints analysis\""                    
                | TrussAnalysisDomain.MethodOfJointsCalculation r -> "\"Choose next joint\"" 
                | TrussAnalysisDomain.MethodOfJointsAnalysis _ -> TrussServices.getAnalysisReport newState//"\"Analysis Complete\""                
            | _ -> "opps"
        let newMessage = 
            match newState with 
            | TrussAnalysisDomain.AnalysisState a -> 
                match a.analysis with
                | TrussAnalysisDomain.Truss -> ""
                | TrussAnalysisDomain.SupportReactionEquations r -> "" 
                | TrussAnalysisDomain.SupportReactionResult r -> "Choose a joint to begin Method of Joints analysis."                    
                | TrussAnalysisDomain.MethodOfJointsCalculation r -> "Choose next joint."
                | TrussAnalysisDomain.MethodOfJointsAnalysis _ -> "Analysis Complete. Click Compute to see report."                
            | _ -> "opps"
        let members = getMembersFrom newState
        let forces = getForcesFrom newState
        let supports = getSupportsFrom newState        
        let reactions = trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value newState
        do  state <- newState
            label.Text <- newState.ToString()
            code_TextBlock.Text <- newCode
            message_TextBlock.Text <- newMessage
            Seq.iter (fun (f:LoadDomain.JointForce) -> 
                match f.magnitude = 0.0 with 
                | true -> () 
                | false -> 
                    drawForce blue f
                    drawReactionForceLabel f.direction (trussServices.getSupportIndexAtJoint f.joint supports) f.magnitude
                ) reactions            
            Seq.iteri (fun i m -> drawMemberLabel m i) members
            Seq.iteri (fun i (f:LoadDomain.JointForce) -> drawForceLabel f.direction i f.magnitude) forces
            drawSolvedMembers newState            

    // Handle
    let handleMouseDown (e : Input.MouseButtonEventArgs) =        
        let p1 = adjustMouseButtonEventArgPoint e
        let joint = getJointIndex p1
        let p = 
            match joint with
            | None -> p1
            | Some i -> 
                let joints = getJointsFrom state
                let p2 = Seq.item i joints
                p2
        match trussControls_Border.IsMouseOver with 
        | true ->             
            let newState = // Logic for Truss Mode radio buttons
                match forceBuilder_RadioButton.IsChecked.Value, 
                      memberBuilder_RadioButton.IsChecked.Value,                                            
                      supportBuilder_RadioButton.IsChecked.Value,
                      selection_RadioButton.IsChecked.Value,
                      analysis_RadioButton.IsChecked.Value,                                            
                      settings_RadioButton.IsChecked.Value,                      
                      forceBuilder_RadioButton.IsMouseOver,
                      memberBuilder_RadioButton.IsMouseOver,                                            
                      supportBuilder_RadioButton.IsMouseOver,
                      selection_RadioButton.IsMouseOver,
                      analysis_RadioButton.IsMouseOver, 
                      settings_RadioButton.IsMouseOver with
                // Force
                | true,false,false,false,false,false, true,false,false,false,false,false
                | false,true,false,false,false,false, true,false,false,false,false,false
                | false,false,true,false,false,false, true,false,false,false,false,false
                | false,false,false,true,false,false, true,false,false,false,false,false
                | false,false,false,false,true,false, true,false,false,false,false,false                
                | false,false,false,false,false,true, true,false,false,false,false,false ->    
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceMagnitude_TextBox.IsReadOnly <- false
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the focre horizontal."
                    trussForceMagnitude_TextBox.Text <- "Enter magnitude"
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.ForceBuild state
                    drawTruss state
                    newState             
                // Member
                | true,false,false,false,false,false, false,true,false,false,false,false
                | false,true,false,false,false,false, false,true,false,false,false,false
                | false,false,true,false,false,false, false,true,false,false,false,false
                | false,false,false,true,false,false, false,true,false,false,false,false
                | false,false,false,false,true,false, false,true,false,false,false,false
                | false,false,false,false,false,true, false,true,false,false,false,false -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.MemberBuild state
                    drawTruss newState
                    newState
                // Support
                | true,false,false,false,false,false,  false,false,true,false,false,false
                | false,true,false,false,false,false,  false,false,true,false,false,false
                | false,false,true,false,false,false,  false,false,true,false,false,false
                | false,false,false,true,false,false,  false,false,true,false,false,false
                | false,false,false,false,true,false,  false,false,true,false,false,false 
                | false,false,false,false,false,true,  false,false,true,false,false,false -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    supportType_StackPanel.Visibility <- Visibility.Visible
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the contact plane from horizontal."
                    trussForceMagnitude_TextBox.Text <- "0"
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newTrust = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.SupportBuild state
                    drawTruss newTrust
                    newTrust                                
                // Selection
                | true,false,false,false,false,false,  false,false,false,true,false,false
                | false,true,false,false,false,false,  false,false,false,true,false,false
                | false,false,true,false,false,false,  false,false,false,true,false,false
                | false,false,false,true,false,false,  false,false,false,true,false,false
                | false,false,false,false,true,false,  false,false,false,true,false,false 
                | false,false,false,false,false,true,  false,false,false,true,false,false -> 
                    message_TextBlock.Text <- "--Select a Truss Part--"                    
                    code_TextBlock.Text <- "Ready"
                    setGraphicsFromKernel kernel
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- 
                        match inspect_RadioButton.IsChecked.Value with
                        | false -> Visibility.Collapsed
                        | true -> Visibility.Visible
                    result_ScrollViewer.IsHitTestVisible <- false
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Visible
                    newP_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true                    
                    let newState = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.Selection state                    
                    drawTruss newState
                    newState
                // Analysis
                | true,false,false,false,false,false,  false,false,false,false,true,false
                | false,true,false,false,false,false,  false,false,false,false,true,false
                | false,false,true,false,false,false,  false,false,false,false,true,false
                | false,false,false,true,false,false,  false,false,false,false,true,false
                | false,false,false,false,true,false,  false,false,false,false,true,false 
                | false,false,false,false,false,true,  false,false,false,false,true,false -> 
                    message_TextBlock.Text <- "Calculate Reactions"
                    let newState = 
                        trussServices.checkTruss (trussServices.getTrussFromState state)
                        |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                    code_TextBlock.Text <- trussServices.getAnalysisReport newState
                    setGraphicsFromKernel kernel
                    result_ScrollViewer.Visibility <- Visibility.Visible
                    result_ScrollViewer.IsHitTestVisible <- true
                    reactionRadio_StackPanel.Visibility <- Visibility.Visible
                    analysis_StackPanel.Visibility <- Visibility.Visible
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true                    
                    code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                    drawTruss newState
                    newState                    
                // Settings
                | true,false,false,false,false,false,  false,false,false,false,false,true
                | false,true,false,false,false,false,  false,false,false,false,false,true
                | false,false,true,false,false,false,  false,false,false,false,false,true
                | false,false,false,true,false,false,  false,false,false,false,false,true
                | false,false,false,false,true,false,  false,false,false,false,false,true 
                | false,false,false,false,false,true,  false,false,false,false,false,true -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Visible
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.Settings state
                    drawTruss state
                    newState
                | _ -> // Logic for Support Type radio buttons
                    match rollerSupport_RadioButton.IsChecked.Value, pinSupport_RadioButton.IsChecked.Value, 
                          rollerSupport_RadioButton.IsMouseOver, pinSupport_RadioButton.IsMouseOver 
                          with 
                    | true,false, true,false
                    | false,true, true,false -> rollerSupport_RadioButton.IsChecked <- Nullable true
                                                pinSupport_RadioButton.IsChecked <- Nullable false
                                                trussServices.sendStateToSupportBuilder false state
                    | true,false, false,true
                    | false,true, false,true -> rollerSupport_RadioButton.IsChecked <- Nullable false 
                                                pinSupport_RadioButton.IsChecked <- Nullable true
                                                trussServices.sendStateToSupportBuilder true state
                    | _ -> // Logic for Selection Mode radio buttons
                        match delete_RadioButton.IsChecked.Value, 
                              inspect_RadioButton.IsChecked.Value, 
                              modify_RadioButton.IsChecked.Value, 
                              delete_RadioButton.IsMouseOver, 
                              inspect_RadioButton.IsMouseOver,
                              modify_RadioButton.IsMouseOver
                              with 
                        // Delete
                        | true,false,false, true,false,false
                        | false,true,false, true,false,false 
                        | false,false,true, true,false,false ->  
                            delete_RadioButton.IsChecked <- Nullable true
                            inspect_RadioButton.IsChecked <- Nullable false
                            modify_RadioButton.IsChecked <- Nullable false
                            delete_Button.Visibility <- Visibility.Visible                            
                            result_ScrollViewer.Visibility <- Visibility.Collapsed
                            result_ScrollViewer.IsHitTestVisible <- true
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            trussServices.setSelectionMode TrussAnalysisDomain.TrussSelectionMode.Delete state
                        // Inspect
                        | true,false,false, false,true,false
                        | false,true,false, false,true,false 
                        | false,false,true, false,true,false ->                        
                            setGraphicsFromKernel kernel
                            delete_RadioButton.IsChecked <- Nullable false 
                            inspect_RadioButton.IsChecked <- Nullable true
                            modify_RadioButton.IsChecked <- Nullable false
                            delete_Button.Visibility <- Visibility.Collapsed                            
                            result_ScrollViewer.Visibility <- Visibility.Visible
                            result_ScrollViewer.IsHitTestVisible <- false
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            trussServices.setSelectionMode TrussAnalysisDomain.TrussSelectionMode.Inspect state
                        // Modify
                        | true,false,false, false,false,true
                        | false,true,false, false,false,true 
                        | false,false,true, false,false,true ->                        
                            delete_RadioButton.IsChecked <- Nullable false 
                            inspect_RadioButton.IsChecked <- Nullable false
                            modify_RadioButton.IsChecked <- Nullable true
                            delete_Button.Visibility <- Visibility.Collapsed                            
                            result_ScrollViewer.Visibility <- Visibility.Collapsed
                            result_ScrollViewer.IsHitTestVisible <- true
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            trussServices.setSelectionMode TrussAnalysisDomain.TrussSelectionMode.Modify state
                        | _ -> // Logic for Analysis Mode radio buttons                            
                            match xAxis_RadioButton.IsChecked.Value, 
                                  yAxis_RadioButton.IsChecked.Value,                                   
                                  xAxis_RadioButton.IsMouseOver, 
                                  yAxis_RadioButton.IsMouseOver
                                  with 
                            | true,false, true,false
                            | false,true, true,false -> 
                                xAxis_RadioButton.IsChecked <- Nullable true 
                                yAxis_RadioButton.IsChecked <- Nullable false
                                let newState = 
                                    trussServices.checkTruss (trussServices.getTrussFromState state)
                                    |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                                code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                                newState
                            | true,false, false,true
                            | false,true, false,true -> 
                                xAxis_RadioButton.IsChecked <- Nullable false 
                                yAxis_RadioButton.IsChecked <- Nullable true                                
                                let newState = 
                                    trussServices.checkTruss (trussServices.getTrussFromState state)
                                    |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                                code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                                newState
                            | _ -> // Logic for Reaction Display radio buttons                            
                                match resultant_RadioButton.IsChecked.Value, 
                                      components_RadioButton.IsChecked.Value,                                   
                                      resultant_RadioButton.IsMouseOver, 
                                      components_RadioButton.IsMouseOver
                                      with 
                                | true,false, true,false
                                | false,true, true,false -> 
                                    resultant_RadioButton.IsChecked <- Nullable true 
                                    components_RadioButton.IsChecked <- Nullable false
                                    drawTruss state
                                    Seq.iter (fun (f:LoadDomain.JointForce) -> match f.magnitude = 0.0 with | true -> () | false -> drawForce red f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value state)
                                    state
                                | true,false, false,true
                                | false,true, false,true -> 
                                    resultant_RadioButton.IsChecked <- Nullable false 
                                    components_RadioButton.IsChecked <- Nullable true
                                    drawTruss state
                                    Seq.iter (fun (f:LoadDomain.JointForce) -> match f.magnitude = 0.0 with | true -> () | false -> drawForce blue f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value state)
                                    state
                                | _ -> state
            do  state <- newState
                label.Text <- newState.ToString() 
        | false ->  
            match state with
            | TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | TrussAnalysisDomain.TrussMode.MemberBuild ->                
                    let newState = trussServices.sendPointToMemberBuilder p state
                    do  drawBuildJoint p1
                        state <- newState
                        trussMemberP1X_TextBox.IsReadOnly <- true
                        trussMemberP1Y_TextBox.IsReadOnly <- true
                        trussMemberP2_StackPanel.Visibility <- Visibility.Visible
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)).ToString()
                | TrussAnalysisDomain.TrussMode.ForceBuild ->                     
                    match joint with
                    | None ->                         
                        state <- TrussAnalysisDomain.ErrorState {errors = [ErrorDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        let newState = trussServices.sendPointToForceBuilder p state
                        drawBuildForceJoint p
                        state <- newState
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)).ToString()
                | TrussAnalysisDomain.TrussMode.SupportBuild -> 
                    match joint with
                    | None ->                         
                        state <- TrussAnalysisDomain.ErrorState {errors = [ErrorDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        match pinSupport_RadioButton.IsChecked.Value with
                        | true -> 
                            let newState = trussServices.sendPointToPinSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                        | false -> 
                            let newState = trussServices.sendPointToRollerSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                | TrussAnalysisDomain.TrussMode.Analysis -> setGraphicsFromKernel kernel //()//
                | TrussAnalysisDomain.TrussMode.Selection ->                     
                    match modify_RadioButton.IsChecked.Value with
                    | false -> label.Text <- state.ToString()
                    | true ->                         
                        let newState = trussServices.sendPointToModification p state
                        match newState with 
                        | TrussAnalysisDomain.SelectionState ss -> 
                            match ss.forces, ss.supports with 
                            | Some f, None ->
                                state <- newState
                                label.Text <- state.ToString()
                                newP_StackPanel.Visibility <- Visibility.Collapsed
                                newF_StackPanel.Visibility <- Visibility.Visible
                                newFMag_TextBox.Text <- f.Head.magnitude.ToString()
                                newFDir_TextBox.Text <- (trussServices.getDirectionFromForce f.Head).ToString()                                
                            | None, Some s ->                                
                                state <- newState
                                label.Text <- state.ToString()
                                newP_StackPanel.Visibility <- Visibility.Collapsed
                                newF_StackPanel.Visibility <- Visibility.Visible
                                newFMag_TextBox.Text <- "0."
                                newFDir_TextBox.Text <- (90. + trussServices.getDirectionFromSupport s.Head).ToString()                                
                            | _ -> 
                                do  setOrgin p
                                    drawTruss newState
                                    state <- newState
                                    label.Text <- state.ToString()
                                    newP_StackPanel.Visibility <- Visibility.Visible
                                    newF_StackPanel.Visibility <- Visibility.Collapsed
                                    newPX_TextBox.Text <- p.X.ToString()
                                    newPY_TextBox.Text <- p.Y.ToString()
                        | _-> ()
                | TrussAnalysisDomain.TrussMode.Settings -> ()
            | TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussAnalysisDomain.BuildMember bm ->                 
                    let newState = trussServices.sendPointToMemberBuilder p state
                    do  drawTruss newState
                        state <- newState
                        trussMemberP1X_TextBox.IsReadOnly <- true
                        trussMemberP1Y_TextBox.IsReadOnly <- true
                        trussMemberP2X_TextBox.IsReadOnly <- true
                        trussMemberP2Y_TextBox.IsReadOnly <- true
                        trussMemberP2_StackPanel.Visibility <- Visibility.Collapsed
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)) .ToString()
                | TrussAnalysisDomain.BuildForce bf -> ()
                | TrussAnalysisDomain.BuildSupport bs -> ()
            | TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | TrussAnalysisDomain.Delete -> ()
                | TrussAnalysisDomain.Inspect ->()
                | TrussAnalysisDomain.Modify -> 
                    let newState = trussServices.sendPointToModification p state
                    do  setOrgin p
                        drawTruss newState
                        state <- newState
                        label.Text <- state.ToString()
                    match newState with
                    | TrussAnalysisDomain.SelectionState ss -> 
                        match ss.modification, ss.forces, ss.supports with
                        |None, None, Some s-> 
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            newF_StackPanel.Visibility <- Visibility.Visible
                            newFMag_TextBox.Text <- "0."
                            newFDir_TextBox.Text <- (90. + trussServices.getDirectionFromSupport s.Head).ToString()
                        | None, Some f, None-> 
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            newF_StackPanel.Visibility <- Visibility.Visible
                            newFMag_TextBox.Text <- f.Head.magnitude.ToString()
                            newFDir_TextBox.Text <- (trussServices.getDirectionFromForce f.Head).ToString() 
                        | Some m, None, None -> 
                            newP_StackPanel.Visibility <- Visibility.Visible
                            newF_StackPanel.Visibility <- Visibility.Collapsed
                            newPX_TextBox.Text <- p.X.ToString()
                            newPY_TextBox.Text <- p.Y.ToString()
                        | None, None, None
                        | _ ->
                            newP_StackPanel.Visibility <- Visibility.Collapsed
                            newF_StackPanel.Visibility <- Visibility.Collapsed                        
                    | _ -> ()
            | TrussAnalysisDomain.AnalysisState a -> 
                match a.analysis with
                | TrussAnalysisDomain.Truss -> ()
                | TrussAnalysisDomain.SupportReactionEquations r -> () 
                | TrussAnalysisDomain.SupportReactionResult r -> 
                    let newState = trussServices.sendPointToCalculation p state
                    let newCode = 
                        match newState with 
                        | TrussAnalysisDomain.AnalysisState a ->
                            match a.analysis with
                            | TrussAnalysisDomain.MethodOfJointsCalculation r -> WolframServices.solveEquations r.memberEquations r.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state <- newState
                        code_TextBlock.Text <- newCode
                        setOrgin p 
                        drawTruss newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)
                | TrussAnalysisDomain.MethodOfJointsCalculation mjc -> 
                    let newState = trussServices.sendPointToCalculation p state
                    let newCode = 
                        match newState with 
                        | TrussAnalysisDomain.AnalysisState a ->
                            match a.analysis with
                            | TrussAnalysisDomain.MethodOfJointsCalculation mjc -> WolframServices.solveEquations mjc.memberEquations mjc.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state <- newState
                        code_TextBlock.Text <- newCode
                        setOrgin p 
                        drawTruss newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)                        
                | TrussAnalysisDomain.MethodOfJointsAnalysis mja -> drawSolvedMembers state 
            | TrussAnalysisDomain.ErrorState es -> 
                match es.errors with 
                | [NoJointSelected] -> ()                    
                | _ -> ()
    let handleMouseUp () =         
        match state with
        | TrussAnalysisDomain.TrussState ts -> 
            match ts.mode with
            | TrussAnalysisDomain.TrussMode.MemberBuild -> ()
            | TrussAnalysisDomain.TrussMode.ForceBuild -> ()
            | TrussAnalysisDomain.TrussMode.SupportBuild -> ()
            | TrussAnalysisDomain.TrussMode.Analysis -> ()
            | TrussAnalysisDomain.TrussMode.Selection -> ()
            | TrussAnalysisDomain.TrussMode.Settings -> ()
        | TrussAnalysisDomain.BuildState bs -> 
            match bs.buildOp with
            | TrussAnalysisDomain.BuildMember bm -> ()
            | TrussAnalysisDomain.BuildForce bf -> ()
            | TrussAnalysisDomain.BuildSupport bs -> ()
        | TrussAnalysisDomain.SelectionState ss -> 
            match ss.mode with
            | TrussAnalysisDomain.Modify -> 
                match ss.forces, ss.supports with
                | Some f, None -> 
                    match newF_StackPanel.Visibility = Visibility.Collapsed with
                    | true -> 
                        do  newP_StackPanel.Visibility <- Visibility.Collapsed
                            newF_StackPanel.Visibility <- Visibility.Visible
                            newFMag_TextBox.Text <- f.Head.magnitude.ToString()
                            newFDir_TextBox.Text <- (trussServices.getDirectionFromForce f.Head).ToString() 
                    | false -> ()
                | None, Some s -> 
                    match newF_StackPanel.Visibility = Visibility.Collapsed with
                    | true -> 
                        do  newP_StackPanel.Visibility <- Visibility.Collapsed
                            newF_StackPanel.Visibility <- Visibility.Visible
                            newFMag_TextBox.Text <-"0."
                            newFDir_TextBox.Text <- (90. + trussServices.getDirectionFromSupport s.Head).ToString() 
                    | false -> ()                
                | _ -> newF_StackPanel.Visibility <- Visibility.Collapsed
            | TrussAnalysisDomain.Delete -> ()
            | TrussAnalysisDomain.Inspect -> 
                let truss = getTrussFrom state
                match ss.forces, ss.members, ss.supports with
                | Some f, None, None -> 
                    let i = trussServices.getForceIndex f.Head truss
                    message_TextBlock.Text <- "--Force " + i.ToString() + "--"
                    code_TextBlock.Text <- trussServices.getAnalysisReport state
                    setGraphicsFromKernel kernel
                | None, Some m, None ->                
                    let i = trussServices.getMemberIndex m.Head truss
                    message_TextBlock.Text <- "--Member " + i.ToString() + "--"
                    code_TextBlock.Text <- trussServices.getAnalysisReport state
                    setGraphicsFromKernel kernel
                | None, None, Some s -> 
                    let i,st = trussServices.getSupportIndex s.Head truss
                    message_TextBlock.Text <- "--" + st + " Support " + i.ToString() + "--"
                    code_TextBlock.Text <- trussServices.getAnalysisReport state
                    setGraphicsFromKernel kernel
                | _ -> message_TextBlock.Text <- "--Select a Truss Part--"
                       code_TextBlock.Text <- "Ready"
                drawTruss state
        | TrussAnalysisDomain.AnalysisState a -> ()
        | TrussAnalysisDomain.ErrorState es -> 
            match es.errors with 
            | [NoJointSelected] -> 
                match  forceBuilder_RadioButton.IsChecked.Value,
                        memberBuilder_RadioButton.IsChecked.Value with 
                | true, false -> 
                    let newState = trussServices.setTrussMode TrussAnalysisDomain.TrussMode.ForceBuild state
                    drawTruss newState
                    state <- newState
                    label.Text <- newState.ToString()
                | false, true -> ()
                | _ -> ()
            | _ -> ()
    let handleMouseMove (e : Input.MouseEventArgs) =
        let p2 = adjustMouseEventArgPoint e        
        match state with
        | TrussAnalysisDomain.TrussState ts -> 
            match ts.mode with
            | TrussAnalysisDomain.TrussMode.MemberBuild -> 
                match trussControls_Border.IsMouseOver with 
                | false -> 
                    match trussMemberP1X_TextBox.IsReadOnly && trussMemberP1Y_TextBox.IsReadOnly with
                    | false -> ()
                    | true -> 
                        do  trussMemberP1X_TextBox.Text <- p2.X.ToString()
                            trussMemberP1Y_TextBox.Text <- p2.Y.ToString()
                | true -> ()
            | TrussAnalysisDomain.TrussMode.ForceBuild -> ()
            | TrussAnalysisDomain.TrussMode.SupportBuild -> ()
            | TrussAnalysisDomain.TrussMode.Analysis -> ()
            | TrussAnalysisDomain.TrussMode.Selection -> ()
            | TrussAnalysisDomain.TrussMode.Settings -> ()
        | TrussAnalysisDomain.BuildState bs ->
            match bs.buildOp with
            | TrussAnalysisDomain.BuildMember bm ->
                let p1 = trussServices.getPointFromMemberBuilder bm                
                let memberBuilder = trussMember (p1,p2)
                match Input.Mouse.LeftButton = Input.MouseButtonState.Pressed,
                      trussControls_Border.IsMouseOver with                
                | false, false -> 
                    match trussControls_Border.IsMouseOver with 
                    | false -> 
                        match trussMemberP2X_TextBox.IsReadOnly && trussMemberP2Y_TextBox.IsReadOnly with
                        | false -> ()
                        | true -> 
                            do  trussMemberP2X_TextBox.Text <- p2.X.ToString()
                                trussMemberP2Y_TextBox.Text <- p2.Y.ToString()
                    | true -> ()
                | true, false ->
                    do  memberBuilder.StrokeThickness <- 1.
                        drawTruss state
                        drawBuildJoint p1                    
                        canvas.Children.Add(memberBuilder) |> ignore   
                | true, true 
                | false, true -> ()
            | TrussAnalysisDomain.BuildForce bf -> ()
            | TrussAnalysisDomain.BuildSupport bs -> ()
        | TrussAnalysisDomain.SelectionState ss -> ()
        | TrussAnalysisDomain.AnalysisState s -> ()
        | TrussAnalysisDomain.ErrorState es -> ()
    let handleKeyDown (e:Input.KeyEventArgs) =
        match e.Key with 
        | Input.Key.Enter -> 
            let x1b,x1 = Double.TryParse trussMemberP1X_TextBox.Text
            let y1b,y1 = Double.TryParse trussMemberP1Y_TextBox.Text
            let x2b,x2 = Double.TryParse trussMemberP2X_TextBox.Text
            let y2b,y2 = Double.TryParse trussMemberP2Y_TextBox.Text
            let magb, mag = Double.TryParse trussForceMagnitude_TextBox.Text
            let dirb, dir = Double.TryParse trussForceAngle_TextBox.Text
            let p1 = 
                match x1b, y1b with 
                | true, true -> Some (System.Windows.Point(x1,y1))
                | true, false -> None
                | false, true -> None
                | false, false -> None
            let p2 = 
                match x2b, y2b with 
                | true, true -> Some (System.Windows.Point(x2,y2))
                | true, false -> None
                | false, true -> None
                | false, false -> None            
            match state with
            | TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | TrussAnalysisDomain.TrussMode.MemberBuild ->                
                    match p1 with 
                    | Some p ->
                        let newState = trussServices.sendPointToMemberBuilder p state
                        do  drawBuildJoint p
                            state <- newState
                            trussMemberP1X_TextBox.IsReadOnly <- true
                            trussMemberP1Y_TextBox.IsReadOnly <- true
                            trussMemberP2_StackPanel.Visibility <- Visibility.Visible
                            label.Text <- state.ToString()
                    | None -> label.Text <- state.ToString()
                | TrussAnalysisDomain.TrussMode.ForceBuild -> ()                    
                | TrussAnalysisDomain.TrussMode.SupportBuild -> ()
                | TrussAnalysisDomain.TrussMode.Analysis -> ()
                | TrussAnalysisDomain.TrussMode.Selection -> ()
                | TrussAnalysisDomain.TrussMode.Settings -> ()
            | TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussAnalysisDomain.BuildMember _bm ->                 
                    match p2 with
                    | None -> ()
                    | Some p -> 
                        let newState = trussServices.sendPointToMemberBuilder p state
                        do  drawTruss newState
                            state <- newState
                            trussMemberP1X_TextBox.IsReadOnly <- true
                            trussMemberP1Y_TextBox.IsReadOnly <- true
                            trussMemberP2X_TextBox.IsReadOnly <- true
                            trussMemberP2Y_TextBox.IsReadOnly <- true
                            trussMemberP2_StackPanel.Visibility <- Visibility.Collapsed
                            label.Text <- state.ToString()
                | TrussAnalysisDomain.BuildForce bf -> 
                    match magb, dirb with
                    | true, true when bf._direction = None -> 
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state |> trussServices.sendPointToForceBuilder dirPoint
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        drawBuildForceDirection green (arrowPoint,dir,mag)
                        label.Text <- newState.ToString()
                    | true, true -> 
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        drawBuildForceDirection green (arrowPoint,dir,mag)
                        label.Text <-  newState.ToString()
                    | true, false -> 
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state
                        state <- newState
                        label.Text <- newState.ToString()
                    | false, true -> 
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let newState = trussServices.sendPointToForceBuilder dirPoint state
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        label.Text <- newState.ToString()
                    | false, false -> () 
                | TrussAnalysisDomain.BuildSupport bs -> 
                    match dirb with
                    | true -> 
                        let jointPoint = 
                            match bs with 
                            | BuilderDomain.Roller f -> trussServices.getPointFromForceBuilder f
                            | BuilderDomain.Pin (f,_) -> trussServices.getPointFromForceBuilder f
                        let dirPoint = System.Windows.Point(jointPoint.X + (1.0 * cos ((dir) * Math.PI/180.)), jointPoint.Y - (1.0 * sin ((dir) * Math.PI/180.)))  
                        let arrowPoint = jointPoint                            
                        let newState = 
                            match rollerSupport_RadioButton.IsChecked.Value with
                            | true -> trussServices.sendMagnitudeToSupportBuilder (mag,None) state |> trussServices.sendPointToRollerSupportBuilder dirPoint
                            | false -> trussServices.sendMagnitudeToSupportBuilder (mag,Some mag) state |> trussServices.sendPointToPinSupportBuilder dirPoint
                        state <- newState
                        drawBuildSupport (arrowPoint,dir - 90.,rollerSupport_RadioButton.IsChecked.Value)
                        label.Text <-  newState.ToString()
                    | false -> 
                        let newState = trussServices.sendMagnitudeToSupportBuilder (mag,None) state
                        state <- newState
                        label.Text <- newState.ToString()                    
            | TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | TrussAnalysisDomain.Delete -> ()
                | TrussAnalysisDomain.Inspect ->()
                | TrussAnalysisDomain.Modify -> 
                    match ss.forces, ss.supports with
                    | None, None ->                     
                        let p =
                            let x = Double.TryParse newPX_TextBox.Text
                            let y = Double.TryParse newPY_TextBox.Text
                            match x,y with
                            | (true,x),(true,y) -> Point(x,y)
                            | _ -> Point(0.,0.)
                        let newState = trussServices.sendPointToModification p state
                        do  drawTruss newState
                            state <- newState
                            label.Text <- state.ToString()
                        match newState with
                        | TrussAnalysisDomain.SelectionState ss -> 
                            match ss.modification with
                            | None -> 
                                newP_StackPanel.Visibility <- Visibility.Collapsed
                                setOrgin (Point(0.,0.))
                                drawTruss newState
                            | Some m -> setOrgin p
                        | _ -> ()
                    | Some f, None -> 
                        let _mag,mag = Double.TryParse newFMag_TextBox.Text
                        let _dir,dir = Double.TryParse newFDir_TextBox.Text
                        let newState = trussServices.modifyTrussForce mag dir state 
                        do  state <- newState
                            newF_StackPanel.Visibility <- Visibility.Collapsed
                            setOrgin (Point(0.,0.))
                            drawTruss newState
                            label.Text <- newState.ToString()
                    | None,Some s ->                         
                        let _dir,dir = Double.TryParse newFDir_TextBox.Text
                        let newState = trussServices.modifyTrussSupport dir state 
                        do  state <- newState
                            newF_StackPanel.Visibility <- Visibility.Collapsed
                            setOrgin (Point(0.,0.))
                            drawTruss newState
                            label.Text <- newState.ToString()
                    | _ -> ()
            | TrussAnalysisDomain.AnalysisState s -> ()
            | TrussAnalysisDomain.ErrorState es -> ()
        | Input.Key.Delete -> 
            match state with
            | TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | TrussAnalysisDomain.TrussMode.MemberBuild -> ()
                | TrussAnalysisDomain.TrussMode.ForceBuild -> ()                    
                | TrussAnalysisDomain.TrussMode.SupportBuild -> ()
                | TrussAnalysisDomain.TrussMode.Analysis -> ()
                | TrussAnalysisDomain.TrussMode.Selection -> ()
                | TrussAnalysisDomain.TrussMode.Settings -> ()
            | TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussAnalysisDomain.BuildMember _bm -> ()
                | TrussAnalysisDomain.BuildForce bf -> ()
                | TrussAnalysisDomain.BuildSupport bs -> ()        
            | TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | TrussAnalysisDomain.TrussSelectionMode.Delete -> 
                    let newState = trussServices.removeTrussPartFromTruss state            
                    do  state <- newState 
                        label.Text <- newState.ToString()
                        drawTruss newState
                | TrussAnalysisDomain.TrussSelectionMode.Modify  -> ()
                | TrussAnalysisDomain.TrussSelectionMode.Inspect -> ()
            | TrussAnalysisDomain.AnalysisState s -> ()                
            | TrussAnalysisDomain.ErrorState es -> ()
        | _ -> () // logic for other keys

    (*Initialize*)
    // label.Text <- state.ToString()
    do  //kernel.Compute( window )
        this.Content <- screen_Grid
        setGraphicsFromKernel kernel        
        
    (*add event handlers*)        
        this.PreviewKeyDown.AddHandler(Input.KeyEventHandler(fun _ e -> handleKeyDown(e)))
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        this.PreviewMouseLeftButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
        this.PreviewMouseLeftButtonUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleMouseUp()))
        this.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove e))
        xUp_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        xDown_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        yUp_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        yDown_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        delete_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> setStateFromAnaysis state))