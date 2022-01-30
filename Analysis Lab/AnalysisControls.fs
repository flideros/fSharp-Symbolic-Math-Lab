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