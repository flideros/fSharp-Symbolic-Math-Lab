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
    let label =
        let l = TextBox()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- ""
            l.BorderBrush <- SolidColorBrush(Colors.Transparent)
            l.Opacity <- 0.5
            l.MaxLines <- 30
        l 
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
            sp.Children.Add(label) |> ignore
            sp.Visibility <- Visibility.Visible
        sp
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(memberBuilder_StackPanel) |> ignore
        g
    
    // Initialize
    let mpX = SharedValue(mousePosition.Get.X)
    let mpY = SharedValue(mousePosition.Get.Y)    
    let j = {x = X mpX.Get; y = Y mpY.Get}
    
    let mutable memberBuilder : BuilderDomain.MemberBuilder = (j,None)

    // Logic
    let setMemberOption (mb: BuilderDomain.MemberBuilder)=
        match mb with 
        | j,None -> do  newMemberOption.Set (None)
                        label.Text <- "None"
                        memberBuilder <- mb
        | j1,Some j2 -> do newMemberOption.Set (Some (j1,j2))
                           label.Text <- (Some (j1,j2)).ToString()
                           memberBuilder <- mb
    let handleMousePositionChange (mb: BuilderDomain.MemberBuilder) = 
        let mpX = SharedValue(mousePosition.Get.X)
        let mpY = SharedValue(mousePosition.Get.Y)
        match p1X_TextBox.IsReadOnly && p1X_TextBox.IsReadOnly && (p2_StackPanel.IsVisible = false), mb with
        | true, (j,None) -> 
            do  p1X_TextBox.Text <- mpX.Get.ToString()
                p1Y_TextBox.Text <- mpY.Get.ToString()
        | true, (j1,Some j2) -> 
            do  p1X_TextBox.Text <- mpX.Get.ToString()
                p1Y_TextBox.Text <- mpY.Get.ToString()        
        | false, (j1,Some j2) -> ()
        | false, (j,None) -> 
            match p2X_TextBox.IsReadOnly && p2X_TextBox.IsReadOnly with
            | true -> 
                do  p2X_TextBox.Text <- mpX.Get.ToString()
                    p2Y_TextBox.Text <- mpY.Get.ToString() 
            | false -> ()
    let handleKeyDown (e:Input.KeyEventArgs) =
        match e.Key with 
        | Input.Key.Enter -> 
            let x1b,x1 = Double.TryParse p1X_TextBox.Text
            let y1b,y1 = Double.TryParse p1Y_TextBox.Text
            let x2b,x2 = Double.TryParse p2X_TextBox.Text
            let y2b,y2 = Double.TryParse p2Y_TextBox.Text
            let j1 = 
                match x1b, y1b with 
                | true, true -> Some {x = X x1; y = Y y1}
                | true, false -> None
                | false, true -> None
                | false, false -> None
            let j2 = 
                match x2b, y2b with 
                | true, true -> Some {x = X x2; y = Y y2}
                | true, false -> None
                | false, true -> None
                | false, false -> None
            match this.IsVisible, p2_StackPanel.IsVisible with
            | false, false 
            | false, true -> ()
            | true, false -> 
                match j1.IsSome with
                | false -> ()
                | true -> 
                    do  setMemberOption (j1.Value,None)
                        p1X_TextBox.IsReadOnly <- false
                        p1Y_TextBox.IsReadOnly <- false
                        p2_StackPanel.Visibility <- Visibility.Visible
            | true, true -> 
                match j2.IsSome with
                | false -> ()
                | true -> 
                    let j,_j = memberBuilder
                    do  setMemberOption (j,j2)
                        p1X_TextBox.IsReadOnly <- true
                        p1Y_TextBox.IsReadOnly <- true
                        p2X_TextBox.IsReadOnly <- true
                        p2Y_TextBox.IsReadOnly <- true
                        p2_StackPanel.Visibility <- Visibility.Collapsed
               
        | _ -> () // logic for other keys 
    let handleMouseDown (e : Input.MouseButtonEventArgs) =
        let x1b,x1 = Double.TryParse p1X_TextBox.Text
        let y1b,y1 = Double.TryParse p1Y_TextBox.Text
        let x2b,x2 = Double.TryParse p2X_TextBox.Text
        let y2b,y2 = Double.TryParse p2Y_TextBox.Text
        let j1 = 
            match x1b, y1b with 
            | true, true -> Some {x = X x1; y = Y y1}
            | true, false -> None
            | false, true -> None
            | false, false -> None
        let j2 = 
            match x2b, y2b with 
            | true, true -> Some {x = X x2; y = Y y2}
            | true, false -> None
            | false, true -> None
            | false, false -> None
        match this.IsVisible, p2_StackPanel.IsVisible with
        | false, false 
        | false, true -> ()
        | true, false -> 
            match j1.IsSome with
            | false -> ()
            | true -> 
                do  setMemberOption (j1.Value,None)
                    p1X_TextBox.IsReadOnly <- false
                    p1Y_TextBox.IsReadOnly <- false
                    p2_StackPanel.Visibility <- Visibility.Visible
        | true, true -> 
            match j2.IsSome with
            | false -> ()
            | true -> 
                let j,_j = memberBuilder
                do  setMemberOption (j,j2)
                    p1X_TextBox.IsReadOnly <- true
                    p1Y_TextBox.IsReadOnly <- true
                    p2X_TextBox.IsReadOnly <- true
                    p2Y_TextBox.IsReadOnly <- true
                    p2_StackPanel.Visibility <- Visibility.Collapsed
    
    do  this.Content <- screen_Grid
        this.PreviewKeyDown.AddHandler(Input.KeyEventHandler(fun _ e -> handleKeyDown(e)))
        mousePosition.Changed.Add(fun _ -> handleMousePositionChange memberBuilder)
        setMemberOption memberBuilder
        //this.IsVisibleChanged. 

    member this.handleMBMouseDown (e : Input.MouseButtonEventArgs) =  handleMouseDown e
        

