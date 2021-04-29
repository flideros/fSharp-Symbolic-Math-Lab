namespace Math.Presentation

open System
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
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
        let out = link.GetInteger().ToString()
        do  setDisplayedText out
            link.Close()            

    let compute = fun (text:string) -> fetch text             

    do  canvas.Children.Add(textBlock) |> ignore
        canvas.Children.Add(button) |> ignore
        
        this.Content <- screen_Grid

        //add event handler to each button
        button.Click.AddHandler(RoutedEventHandler(fun _ _ -> compute "51+6"))