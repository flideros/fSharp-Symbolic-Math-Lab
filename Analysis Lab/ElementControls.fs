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
open LoadDomain
open BuilderDomain

type MemberBuilderControl(mousePosition:SharedValue<Point>,
                          newMemberOption:SharedValue<(Member option)>,
                          jointList:SharedValue<System.Windows.Point list>) as this =  
    inherit UserControl()    
    //do Install() |> ignore     
    
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
    
    // Initialize
    let mpX = mousePosition.Get.X
    let mpY = mousePosition.Get.Y  
    let j = {x = X mpX; y = Y mpY}
    
    let mutable memberBuilder : BuilderDomain.MemberBuilder = (j,None)

    // Logic
    let getJointIndex (p1:System.Windows.Point) =          
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) (List.toSeq jointList.Get)
    let makeJointFrom (point :System.Windows.Point) = {x = X point.X; y = Y point.Y}
    let setMemberOption (mb: BuilderDomain.MemberBuilder) =
        match mb with 
        | j,None -> do  newMemberOption.Set (None)
                        memberBuilder <- mb
        | j1,Some j2 -> do newMemberOption.Set (Some (j1,j2))
                           memberBuilder <- mb    
    let handleMousePositionChange (mb: BuilderDomain.MemberBuilder) = 
        let mpX = SharedValue(mousePosition.Get.X)
        let mpY = SharedValue(mousePosition.Get.Y)
        match this.IsKeyboardFocused || 
              p1X_TextBox.IsKeyboardFocused || 
              p2X_TextBox.IsKeyboardFocused ||
              p1Y_TextBox.IsKeyboardFocused || 
              p2Y_TextBox.IsKeyboardFocused with
        | true -> ()
        | false ->
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
                | true, true -> 
                    let pi = Point(x1,y1) |> getJointIndex
                    match pi with
                    | None ->  Some {x = X x1; y = Y y1}
                    | Some i -> Some (makeJointFrom jointList.Get.[i])
                | true, false -> None
                | false, true -> None
                | false, false -> None     
            let j2 = 
                match x2b, y2b with 
                | true, true -> 
                    let pi = Point(x2,y2) |> getJointIndex
                    match pi with
                    | None ->  Some {x = X x2; y = Y y2}
                    | Some i -> Some (makeJointFrom jointList.Get.[i])
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
    let handleMouseDown () =
        let x1b,x1 = Double.TryParse p1X_TextBox.Text
        let y1b,y1 = Double.TryParse p1Y_TextBox.Text
        let x2b,x2 = Double.TryParse p2X_TextBox.Text
        let y2b,y2 = Double.TryParse p2Y_TextBox.Text
        let j1 = 
            match x1b, y1b with 
            | true, true -> 
                let pi = Point(x1,y1) |> getJointIndex
                match pi with
                | None ->  Some {x = X x1; y = Y y1}
                | Some i -> Some (makeJointFrom jointList.Get.[i])
            | true, false -> None
            | false, true -> None
            | false, false -> None
        let j2 = 
            match x2b, y2b with 
            | true, true -> 
                let pi = Point(x2,y2) |> getJointIndex
                match pi with
                | None ->  Some {x = X x2; y = Y y2}
                | Some i -> Some (makeJointFrom jointList.Get.[i])                
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
    let handleMouseUp () =       
        match this.IsKeyboardFocused || 
              p1X_TextBox.IsKeyboardFocused || 
              p2X_TextBox.IsKeyboardFocused ||
              p1Y_TextBox.IsKeyboardFocused || 
              p2Y_TextBox.IsKeyboardFocused with
        | true -> ()
        | false ->
            match this.IsVisible, p2_StackPanel.IsVisible with
            | false, false 
            | false, true 
            | true, true -> ()
            | true, false -> 
                do  p1X_TextBox.Text <- p2X_TextBox.Text
                    p1Y_TextBox.Text <- p2Y_TextBox.Text
                
    do  this.Content <- screen_Grid        
        mousePosition.Changed.Add(fun _ -> handleMousePositionChange memberBuilder)
        setMemberOption memberBuilder

    member _this.handleMBMouseDown () =  handleMouseDown ()
    member _this.handleMBMouseUp () =  handleMouseUp ()
    member _this.handleMBKeyDown (e:Input.KeyEventArgs) = handleKeyDown e

type JointForceBuilderControl(mousePosition:SharedValue<Point>,
                              newJointForceOption:SharedValue<(LoadDomain.JointForce option)>,
                              jointList:SharedValue<System.Windows.Point list>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    
    // Controls    
    let angle_TextBlock = 
        let tb = TextBlock(Text = "Angle (Degrees)")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let angle_TextBox = 
        let tb = TextBox()
        let mouseDown () = 
            match Double.TryParse tb.Text with
            | false,_ -> tb.Text <- ""
            | true,_ -> ()
        do  tb.MaxLines <- 15
            tb.TabIndex <- 0
            tb.IsReadOnly <- false
            tb.BorderThickness <- Thickness(3.)
            tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> mouseDown()))
        tb
    let magnitude_TextBlock = 
        let tb = TextBlock(Text = "Magnitude")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let magnitude_TextBox = 
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
    
        // Main grid
    let jointForceBuilder_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(angle_TextBlock) |> ignore
            sp.Children.Add(angle_TextBox) |> ignore
            sp.Children.Add(magnitude_TextBlock) |> ignore
            sp.Children.Add(magnitude_TextBox) |> ignore            
            sp.Visibility <- Visibility.Visible
        sp
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(jointForceBuilder_StackPanel) |> ignore
        g
    
    // Initialize
    let mpX = mousePosition.Get.X
    let mpY = mousePosition.Get.Y  
    let j = {x = X mpX; y = Y mpY}

    let mutable forceBuilder : BuilderDomain.JointForceBuilder Option = None          
    let getJointIndex (p1:System.Windows.Point) =          
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) (List.toSeq jointList.Get)
    let makeJointFrom (point :System.Windows.Point) = {x = X point.X; y = Y point.Y}    
    let handleMouseDown () = 
        match getJointIndex mousePosition.Get, this.IsVisible with 
        | None, true -> 
            forceBuilder <- None
            newJointForceOption.Set None
        | Some i, true -> 
            let ji = makeJointFrom jointList.Get.[i]
            let fb =  {_magnitude = None; _direction = None; joint = ji}            
            forceBuilder <- Some fb
            newJointForceOption.Set None
        | _, false -> ()
    let handleKeyDown (e:Input.KeyEventArgs) =
        match e.Key with 
        | Input.Key.Enter -> 
            let angleb,angle = Double.TryParse angle_TextBox.Text
            let magnitudeb,magnitude = Double.TryParse magnitude_TextBox.Text             
            match forceBuilder with 
            | None -> 
                newJointForceOption.Set None
            | Some {_magnitude = m; _direction = d; joint = {x = X x;y = Y y}} -> 
                match angleb, magnitudeb with                
                | true, true -> 
                    let dir = Vector(x + (50.0 * cos (angle * Math.PI/180.)), y - (50.0 * sin (angle * Math.PI/180.)))
                    forceBuilder <- Some {joint = forceBuilder.Value.joint; _direction = Some dir; _magnitude = Some magnitude}
                    newJointForceOption.Set (Some {direction = dir; magnitude = magnitude; joint = forceBuilder.Value.joint})
                | true, false -> 
                    let dir = Vector(x + (50.0 * cos (angle * Math.PI/180.)), y - (50.0 * sin (angle * Math.PI/180.)))
                    forceBuilder <- Some {forceBuilder.Value with _direction = Some dir}
                    newJointForceOption.Set None
                | false, true -> 
                    forceBuilder <- Some {forceBuilder.Value with _magnitude = Some magnitude}
                    newJointForceOption.Set None
                | false, false -> ()
                                     
        | _ -> () // logic for other keys

    do  this.Content <- screen_Grid

    
    member _this.handleFBMouseDown () =  handleMouseDown ()    
    member _this.handleFBKeyDown (e:Input.KeyEventArgs) = handleKeyDown e

type SupportBuilderControl(mousePosition:SharedValue<Point>,
                           newSupportOption:SharedValue<(ElementDomain.Support option)>,
                           jointList:SharedValue<System.Windows.Point list>) as this =  
    inherit UserControl()    
    do Install() |> ignore     
    
    // Controls    
    let angle_TextBlock = 
        let tb = TextBlock(Text = "Angle (Degrees)")
        do tb.SetValue(Grid.RowProperty,0)
           tb.Margin <- Thickness(Left = 0., Top = 5., Right = 0., Bottom = 0.)
        tb
    let angle_TextBox = 
        let tb = TextBox()
        let mouseDown () = 
            match Double.TryParse tb.Text with
            | false,_ -> tb.Text <- ""
            | true,_ -> ()
        do  tb.MaxLines <- 15
            tb.TabIndex <- 0
            tb.Text <- "0"
            tb.IsReadOnly <- false
            tb.BorderThickness <- Thickness(3.)
            tb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            tb.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> mouseDown()))
        tb
    // Support type selection
    let supportType_Label =
       let l = TextBlock()
       do  l.Margin <- Thickness(Left = 0., Top = 10., Right = 0., Bottom = 10.)
           l.FontStyle <- FontStyles.Normal
           l.FontSize <- 20.            
           l.TextWrapping <- TextWrapping.Wrap
           l.Text <- "Support Type"
       l
    let supportType_ComboBox =
        let cb = ComboBox()        
        do  cb.Text <- "Support Type"
            //cb.Width <- 200.
            //cb.Height <- 30.
            cb.FontSize <- 15.
            cb.VerticalContentAlignment <- VerticalAlignment.Center
            cb.SelectedItem <- "Roller"            
            cb.ItemsSource <- ["Roller";"Pin";"Fixed";"Hinge";"Simple"]
        cb    

        // Main grid
    let supportBuilder_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 0., Top = 15., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(supportType_Label) |> ignore
            sp.Children.Add(supportType_ComboBox) |> ignore
            sp.Children.Add(angle_TextBlock) |> ignore
            sp.Children.Add(angle_TextBox) |> ignore    
            sp.Visibility <- Visibility.Visible
        sp
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(supportBuilder_StackPanel) |> ignore
        g
    
    // Initialize
    let mpX = mousePosition.Get.X
    let mpY = mousePosition.Get.Y  
    let j = {x = X mpX; y = Y mpY}

    let mutable supportBuilder : BuilderDomain.SupportBuilder Option = None          
    let getJointIndex (p1:System.Windows.Point) =          
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) (List.toSeq jointList.Get)
    let makeJointFrom (point :System.Windows.Point) = {x = X point.X; y = Y point.Y}    
    let handleMouseDown () = 
        match getJointIndex mousePosition.Get, this.IsVisible with 
        | None, true -> 
            supportBuilder <- None
            newSupportOption.Set None
        | Some i, true -> 
            let ji = makeJointFrom jointList.Get.[i]
            let sb =  
                match supportType_ComboBox.SelectedItem.ToString() with 
                | "Roller" -> Roller {_magnitude = Some 0.0; _direction = None; joint = ji}    
                | "Pin" -> Pin ({_magnitude = Some 0.0; _direction = None; joint = ji}, 
                                {_magnitude = Some 0.0; _direction = None; joint = ji})
                | _ -> Roller {_magnitude = Some 0.0; _direction = None; joint = ji}
            supportBuilder <- Some sb
            newSupportOption.Set None
        | _, false -> ()
    let handleKeyDown (e:Input.KeyEventArgs) =
        match e.Key with 
        | Input.Key.Enter -> 
            let angleb,angle = Double.TryParse angle_TextBox.Text             
            match supportBuilder with 
            | None -> 
                newSupportOption.Set None            
            | Some (Roller ({_magnitude = _m; _direction = None; joint = {x = X x;y = Y y}})) -> 
                match angleb with                
                | true -> 
                    let dir = Vector(x + (50.0 * cos ((angle + 90.) * Math.PI/180.)), y - (50.0 * sin ((angle + 90.) * Math.PI/180.)))                    
                    let newSupport = ElementDomain.Roller {direction = dir; magnitude = 0.0; joint = {x = X x;y = Y y}}
                    supportBuilder <- None
                    newSupportOption.Set (Some newSupport)
                | false -> ()
            | Some (Pin ({_magnitude = _m; _direction = None; joint = {x = X x;y = Y y}},
                         {_magnitude = _mt; _direction = None; joint = {x = X xt;y = Y yt}})) -> 
                match angleb with                
                | true -> 
                    let dirT = Vector(x + (50.0 * cos (angle * Math.PI/180.)), y - (50.0 * sin (angle * Math.PI/180.)))
                    let dirN = Vector(x + (50.0 * cos ((angle + 90.) * Math.PI/180.)), y - (50.0 * sin ((angle + 90.) * Math.PI/180.)))
                    let newSupport = 
                        ElementDomain.Pin {tangent = {direction = dirT; magnitude = 0.0; joint = {x = X x;y = Y y}};
                                           normal  = {direction = dirN; magnitude = 0.0; joint = {x = X x;y = Y y}}}
                    supportBuilder <- None
                    newSupportOption.Set (Some newSupport)
                | false -> ()
            | _ -> ()
        | _ -> () // logic for other keys

    do  this.Content <- screen_Grid

    
    member _this.handleSBMouseDown () =  handleMouseDown ()    
    member _this.handleSBKeyDown (e:Input.KeyEventArgs) = handleKeyDown e      
    