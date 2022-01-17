namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Numerics
open System.Windows
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink
//open GraphicResources
open GenericDomain
open AtomicDomain

type MemberBuilderControl(mousePosition:SharedValue<Point>,
                          newMemberOption:SharedValue<Member option>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    // Controls
        // Member builder P1
    let p1X_TextBlock = 
        let tb = TextBlock(Text = "X1")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let p1X_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let p1Y_TextBlock = 
        let tb = TextBlock(Text = "Y1")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let p1Y_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let p1_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(p1X_TextBlock) |> ignore
            sp.Children.Add(p1X_TextBox) |> ignore
            sp.Children.Add(p1Y_TextBlock) |> ignore
            sp.Children.Add(p1Y_TextBox) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
        // Member builder P2
    let p2X_TextBlock = 
        let tb = TextBlock(Text = "X2")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let p2X_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb
    let p2Y_TextBlock = 
        let tb = TextBlock(Text = "Y2")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let p2Y_TextBox = 
        let tb = TextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        let toggleIsReadOnly () = tb.IsReadOnly <- false
        do  tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> toggleIsReadOnly()))
        tb 
    let p2_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(p2X_TextBlock) |> ignore
            sp.Children.Add(p2X_TextBox) |> ignore
            sp.Children.Add(p2Y_TextBlock) |> ignore
            sp.Children.Add(p2Y_TextBox) |> ignore
            sp.Visibility <- Visibility.Collapsed
        sp
        // Main grid
    let memberBuilder_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(p1_StackPanel) |> ignore
            sp.Children.Add(p2_StackPanel) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(memberBuilder_StackPanel) |> ignore
        g
    
    // initialize
    let mpX = SharedValue(mousePosition.Get.X)
    let mpY = SharedValue(mousePosition.Get.Y)    
    let j = {x = X mpX.Get; y = Y mpY.Get}
    
    let mutable memberBuilder : BuilderDomain.MemberBuilder = (j,None)

    let handleMousePositionChange (mb: BuilderDomain.MemberBuilder) = 
        let mpX = SharedValue(mousePosition.Get.X)
        let mpY = SharedValue(mousePosition.Get.Y)
        match p1X_TextBox.IsReadOnly && p1X_TextBox.IsReadOnly, mb with
        | true, (j,None) -> 
            do  p1X_TextBox.Text <- mpX.Get.ToString()
                p1Y_TextBox.Text <- mpY.Get.ToString()
        | true, (j1,Some j2) -> 
            match p2X_TextBox.IsReadOnly && p2X_TextBox.IsReadOnly with
            | true -> 
                do  p2X_TextBox.Text <- mpX.Get.ToString()
                    p2Y_TextBox.Text <- mpY.Get.ToString() 
            | false -> ()
        | false, (j,None) -> ()
        | false, (j1,Some j2) -> 
            match p2X_TextBox.IsReadOnly && p2X_TextBox.IsReadOnly with
            | true -> 
                do  p2X_TextBox.Text <- mpX.Get.ToString()
                    p2Y_TextBox.Text <- mpY.Get.ToString() 
            | false -> ()



    do  this.Content <- screen_Grid
        mousePosition.Changed.Add(fun _ -> handleMousePositionChange memberBuilder)
        //this.IsVisibleChanged. 

