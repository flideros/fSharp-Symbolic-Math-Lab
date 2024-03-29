﻿namespace Math.Presentation.WolframEngine.Analysis
// This is a development area.
// This code is isolated from the Analysis UI for the time being as I develop this code.

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
open AnalysisDomain.TrussAnalysisDomain
    
type Truss(kernel:MathKernel,
           setGraphicsFromKernel:(MathKernel -> unit),
           wolframCode : SharedValue<string>,
           wolframMessage : SharedValue<string> ,
           wolframResult : SharedValue<string>,           
           wolframSettings : SharedValue<WolframResultControlSettings> 
          ) as this =  
    inherit UserControl()    
    do Install() |> ignore
    
    // Internal State
    let initialState = AnalysisDomain.TrussAnalysisDomain.TrussState {truss = {members=[]; forces=[]; supports=[]}; mode = ControlDomain.MemberBuild}
    let mutable state = initialState
    
    (*Shared Values*)  
    let jointList = SharedValue<(Point list)> []
    let orginPosition = SharedValue<Point> (Point(0.,0.))
    let mousePosition = SharedValue<Point> (Point(0.,0.))
    let newMemberOption = SharedValue<(AtomicDomain.Member option)> (None)
    let newForceOption = SharedValue<(LoadDomain.JointForce option)> (None)
    let newSupportOption = SharedValue<(ElementDomain.Support option)> (None)
    let system = SharedValue<(ElementDomain.System option)> (None)
    let selectedPart = SharedValue<(ElementDomain.Part option)> (None)
    let selectionMode = SharedValue<(ControlDomain.SelectionMode)> (ControlDomain.SelectionMode.Delete)
    let externalState = SharedValue<TrussAnalysisState> (state)
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()

    (*Controls*) 
        // General purpose text for code output such as displaying ontrol state
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
            l.Visibility <- Visibility.Collapsed
        l        
        // Orgin and grid
    let coordinateGrid_Control = 
        let g = GridControl(orginPosition)
        g
        // Member Builder    
    let memberBuilder_Control = 
        let mb = MemberBuilderControl(mousePosition,newMemberOption,jointList)
        do  mb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
        mb    
        // Force builder    
    let jointForceBuilder_Control = 
        let fb = JointForceBuilderControl(mousePosition,newForceOption,jointList)
        do  fb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            fb.Visibility <- Visibility.Collapsed
        fb    
        // Support builder    
    let supportBuilder_Control = 
        let sb = SupportBuilderControl(mousePosition,newSupportOption,jointList)
        do  sb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            sb.Visibility <- Visibility.Collapsed
        sb
        // Selection
    let selection_Control = 
        let sb = SelectionControl(orginPosition,mousePosition,jointList,system,selectedPart,selectionMode,wolframMessage)
        do  sb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            sb.Visibility <- Visibility.Collapsed          
        sb
        // Truss mode selection    
    let trussMode_ComboBox =
        let cb = ComboBox()        
        do  cb.Text <- "Selection Mode"
            //cb.Width <- 200.
            //cb.Height <- 30.
            cb.FontSize <- 15.
            cb.VerticalContentAlignment <- VerticalAlignment.Center
            cb.SelectedItem <- "Member builder"            
            cb.ItemsSource <- ["Member builder";"Force builder";"Support builder";"Selection";"Analysis";"Settings"]
        cb
    let trussMode_StackPanel = 
        let trussMode_Label =
            let l = TextBlock()
            do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
                l.FontStyle <- FontStyles.Normal
                l.FontSize <- 20.            
                l.TextWrapping <- TextWrapping.Wrap
                l.Text <- "Truss Mode"
            l
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_Label) |> ignore
            sp.Children.Add(trussMode_ComboBox) |> ignore 
        sp         
        // Settings    
    let settings_StackPanel = 
        let toggleCodeText_Button = 
            let b = Button() 
            let onOff() = 
                match b.Content.ToString() with 
                | "Code Text Off" -> 
                    do  wolframSettings.Set {wolframSettings.Get with codeVisible = true}
                        b.Content <- "Code Text On" 
                | "Code Text On" -> 
                    do  wolframSettings.Set {wolframSettings.Get with codeVisible = false}
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
                    do  wolframSettings.Set {wolframSettings.Get with resultVisible = true}
                        b.Content <- "Result Text On" 
                | "Result Text On" -> 
                    do  wolframSettings.Set {wolframSettings.Get with resultVisible = false}
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
            do  b.Content <- "State Text Off" 
                b.FontSize <- 12.
                b.FontWeight <- FontWeights.Regular
                b.VerticalAlignment <- VerticalAlignment.Center
                b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
                b.Visibility <- Visibility.Visible
                b.Click.AddHandler(RoutedEventHandler(fun _ _ -> onOff() ))
            b
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            sp.Visibility <- Visibility.Collapsed
            sp.Children.Add(toggleCodeText_Button) |> ignore
            sp.Children.Add(toggleResultText_Button) |> ignore
            sp.Children.Add(toggleStateText_Button) |> ignore
            sp.Children.Add(coordinateGrid_Control) |> ignore

        sp        
        // Controls
    let controls_StackPanel =
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 10., Right = 10., Bottom = 10.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_StackPanel) |> ignore
            sp.Children.Add(memberBuilder_Control) |> ignore
            sp.Children.Add(jointForceBuilder_Control) |> ignore
            sp.Children.Add(supportBuilder_Control) |> ignore
            sp.Children.Add(settings_StackPanel) |> ignore
            sp.Children.Add(selection_Control) |> ignore
            sp.SetValue(Canvas.ZIndexProperty,4)
        sp            
    let trussControls_Border =         
        let border = Border()            
        do  border.BorderBrush <- black
            border.Cursor <- System.Windows.Input.Cursors.Arrow
            border.Background <- clear
            border.Opacity <- 0.8
            border.IsHitTestVisible <- true
            border.BorderThickness <- Thickness(Left = 1.5, Top = 1.5, Right = 1.5, Bottom = 1.5)
            border.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            border.SetValue(Canvas.ZIndexProperty,4)
            border.Child <- controls_StackPanel
        border
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

    (*Truss parts*)
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
                match selectionMode.Get with
                | ControlDomain.Delete -> trussServices.sendMemberToState l ControlDomain.SelectionMode.Delete s
                | ControlDomain.Inspect -> trussServices.sendMemberToState l ControlDomain.SelectionMode.Inspect s
                | ControlDomain.Modify -> trussServices.sendMemberToState l ControlDomain.SelectionMode.Modify s
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
                match selectionMode.Get with
                | ControlDomain.Delete -> trussServices.sendForceToState l ControlDomain.SelectionMode.Delete s
                | ControlDomain.Inspect -> trussServices.sendForceToState l ControlDomain.SelectionMode.Inspect s
                | ControlDomain.Modify -> trussServices.sendForceToState l ControlDomain.SelectionMode.Modify s
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
                match selectionMode.Get with
                | ControlDomain.Delete -> trussServices.sendSupportToState p ControlDomain.SelectionMode.Delete s
                | ControlDomain.Inspect -> trussServices.sendSupportToState p ControlDomain.SelectionMode.Inspect s
                | ControlDomain.Modify -> trussServices.sendSupportToState p ControlDomain.SelectionMode.Modify s
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
        let l1,l2,e = coordinateGrid_Control.orginPoint p 
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
        let orginPoint = orginPosition.Get
        do  canvas.Children.Clear()
            canvas.Children.Add(coordinateGrid_Control.gridLines) |> ignore
            drawOrgin orginPoint
            canvas.Children.Add(label) |> ignore            
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
        // Set
    let setOrgin p = 
        let joints = getJointsFrom state |> Seq.toList
        let selectedJoint = getJointIndex p
        match selectedJoint with 
        | None -> 
            do  orginPosition.Set (Point(0.,0.))
        | Some i -> 
            do  orginPosition.Set (Point(joints.[i].X,joints.[i].Y)) 
    let setjointList (s: AnalysisDomain.TrussAnalysisDomain.TrussAnalysisState) = jointList.Set (getJointsFrom s |> Seq.toList)
 
    (*Analysis*) 
    let methodOfJointsAnalysis =         
        let moj = MethodOfJointsAnalysisControl(
                    kernel,
                    externalState,
                    setGraphicsFromKernel,
                    drawSolvedMember,
                    drawTruss,drawForce,
                    drawMemberLabel,
                    drawForceLabel,
                    drawReactionForceLabel,
                    setOrgin,                    
                    wolframResult,
                    wolframCode,
                    wolframMessage)
        do  moj.Visibility <- Visibility.Collapsed
        moj     
    
    (*Event Handlers*)
    let handleSelectionModeChanged s = 
        // Logic for Selection Mode 
        match selectionMode.Get with 
        | ControlDomain.Delete ->                             
            wolframSettings.Set({wolframSettings.Get with isVisible = false})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            let newState = trussServices.setSelectionMode ControlDomain.SelectionMode.Delete s
            state <- newState
        | ControlDomain.Inspect ->                        
            setGraphicsFromKernel kernel                   
            wolframSettings.Set({wolframSettings.Get with isVisible = true})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = false}
            wolframCode.Set "Ready"
            let newState = trussServices.setSelectionMode ControlDomain.SelectionMode.Inspect s
            state <- newState
        | ControlDomain.Modify ->                           
            wolframSettings.Set({wolframSettings.Get with isVisible = false})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            let newState = trussServices.setSelectionMode ControlDomain.SelectionMode.Modify s
            state <- newState
    let handleTrussModeChanged s = 
        // Logic for Selection Mode 
        match string trussMode_ComboBox.SelectedItem with 
        // Force
        | "Force builder" ->    
            methodOfJointsAnalysis.Visibility <- Visibility.Collapsed
            wolframSettings.Set({wolframSettings.Get with isVisible = false})                   
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            memberBuilder_Control.Visibility <- Visibility.Collapsed
            supportBuilder_Control.Visibility <- Visibility.Collapsed
            jointForceBuilder_Control.Visibility <- Visibility.Visible
            selection_Control.Visibility <- Visibility.Collapsed
            settings_StackPanel.Visibility <- Visibility.Collapsed
            wolframMessage.Set "Calculate Reactions"
            wolframCode.Set "Ready"
            let newState = trussServices.setTrussMode ControlDomain.ForceBuild s
            drawTruss s

            state <- newState             
        // Member
        | "Member builder" -> 
            methodOfJointsAnalysis.Visibility <- Visibility.Collapsed
            wolframSettings.Set({wolframSettings.Get with isVisible = false})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            memberBuilder_Control.Visibility <- Visibility.Visible
            supportBuilder_Control.Visibility <- Visibility.Collapsed
            jointForceBuilder_Control.Visibility <- Visibility.Collapsed
            selection_Control.Visibility <- Visibility.Collapsed
            settings_StackPanel.Visibility <- Visibility.Collapsed
            wolframMessage.Set "Calculate Reactions"
            wolframCode.Set "Ready"
            let newState = trussServices.setTrussMode ControlDomain.MemberBuild s
            drawTruss newState
            state <- newState
        // Support
        | "Support builder" -> 
            wolframSettings.Set({wolframSettings.Get with isVisible = false})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            memberBuilder_Control.Visibility <- Visibility.Collapsed
            supportBuilder_Control.Visibility <- Visibility.Visible
            jointForceBuilder_Control.Visibility <- Visibility.Collapsed
            selection_Control.Visibility <- Visibility.Collapsed
            settings_StackPanel.Visibility <- Visibility.Collapsed
            wolframMessage.Set "Calculate Reactions"
            wolframCode.Set "Ready"
            let newTrust = trussServices.setTrussMode ControlDomain.SupportBuild s
            drawTruss newTrust
            state <- newTrust                                
        // Selection
        | "Selection" -> 
            system.Set (Some (ElementDomain.System.TrussSystem (trussServices.getTrussFromState s)))
            wolframMessage.Set "--Select a Truss Part--"
            wolframCode.Set "Ready"                    
            setGraphicsFromKernel kernel
            wolframSettings.Set(
                {wolframSettings.Get with 
                    isVisible = 
                        match selectionMode.Get = ControlDomain.Inspect with
                        | false -> false
                        | true -> true })             
            methodOfJointsAnalysis.Visibility <- Visibility.Collapsed
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = false}
            memberBuilder_Control.Visibility <- Visibility.Collapsed
            supportBuilder_Control.Visibility <- Visibility.Collapsed
            jointForceBuilder_Control.Visibility <- Visibility.Collapsed
            selection_Control.Visibility <- Visibility.Visible
            settings_StackPanel.Visibility <- Visibility.Collapsed              
            let newState = trussServices.setTrussMode ControlDomain.Selection state
            drawTruss newState
            label.Text <- newState.ToString()
            state <- newState
        // Analysis
        | "Analysis" ->             
            methodOfJointsAnalysis.Visibility <- Visibility.Visible
            wolframMessage.Set "Calculate Reactions"
            let newState = 
                trussServices.checkTruss (trussServices.getTrussFromState s)
                |> trussServices.getSupportReactionEquationsFromState true // LOOK
            wolframCode.Set (trussServices.getAnalysisReport newState)
            setGraphicsFromKernel kernel
            wolframSettings.Set({wolframSettings.Get with isVisible = true})                  
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            memberBuilder_Control.Visibility <- Visibility.Collapsed
            supportBuilder_Control.Visibility <- Visibility.Collapsed
            jointForceBuilder_Control.Visibility <- Visibility.Collapsed
            selection_Control.Visibility <- Visibility.Collapsed
            settings_StackPanel.Visibility <- Visibility.Collapsed             
            wolframCode.Set (trussServices.getSupportReactionSolve true newState) // LOOK
            drawTruss newState
            state <- newState 
            externalState.Set newState 
        // Settings
        | "Settings" -> 
            wolframSettings.Set({wolframSettings.Get with isVisible = false})
            wolframSettings.Set {wolframSettings.Get with isHitTestVisible = true}
            memberBuilder_Control.Visibility <- Visibility.Collapsed
            supportBuilder_Control.Visibility <- Visibility.Collapsed
            jointForceBuilder_Control.Visibility <- Visibility.Collapsed
            selection_Control.Visibility <- Visibility.Collapsed
            settings_StackPanel.Visibility <- Visibility.Visible
            wolframMessage.Set "Calculate Reactions"
            wolframCode.Set "Ready"
            let newState = trussServices.setTrussMode ControlDomain.ControlMode.Settings s
            drawTruss newState
            state <- newState
        | _ -> ()
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
            let newState =  methodOfJointsAnalysis.handleMojClick state
            do  state <- externalState.Get
                label.Text <- state.ToString() 
        | false ->  
            do  memberBuilder_Control.handleMBMouseDown ()
                jointForceBuilder_Control.handleFBMouseDown ()
                supportBuilder_Control.handleSBMouseDown ()
                selection_Control.handleSelectionMouseDown ()
            match state with
            | AnalysisDomain.TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | ControlDomain.MemberBuild ->                     
                    let newState = trussServices.sendMemberOptionToState (newMemberOption.Get) state
                    do  drawTruss newState
                        drawBuildJoint p
                        state <- newState
                        setjointList newState
                        label.Text <- newState.ToString() 
                | ControlDomain.ForceBuild ->                     
                    match joint with
                    | None ->                         
                        state <- AnalysisDomain.TrussAnalysisDomain.ErrorState {errors = [ErrorDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        let newState = trussServices.sendJointForceOptionToState (newForceOption.Get) state                        
                        drawBuildForceJoint p
                        state <- newState
                        label.Text <- newState.ToString() 
                | ControlDomain.SupportBuild -> 
                    match joint with
                    | None ->                         
                        state <- AnalysisDomain.TrussAnalysisDomain.ErrorState {errors = [ErrorDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        let newState = trussServices.sendSupportOptionToState (newSupportOption.Get) state
                        do  drawBuildSupportJoint p
                            state <- newState
                            label.Text <- newState.ToString()
                | ControlDomain.Analysis -> setGraphicsFromKernel kernel                    
                | ControlDomain.Selection -> 
                    match selectionMode.Get = ControlDomain.Modify with
                    | false -> label.Text <- state.ToString()
                    | true ->
                        let newState = trussServices.sendPointToModification p state
                        match newState with 
                        | AnalysisDomain.TrussAnalysisDomain.SelectionState ss ->                             
                            match ss.forces, ss.supports, ss.members with 
                            | Some [f], None, None ->
                                selectedPart.Set (Some (ElementDomain.Force f))
                                state <- newState
                                label.Text <- state.ToString()
                            | None, Some [s], None ->                                
                                selectedPart.Set (Some (ElementDomain.Support s))
                                state <- newState
                                label.Text <- state.ToString()
                            | None, None, Some [m] ->                                
                                selectedPart.Set (Some (ElementDomain.Member m))
                                state <- newState
                                label.Text <- state.ToString()                             
                            | _ -> 
                                selectedPart.Set None
                                setOrgin p
                                drawTruss newState
                                state <- newState
                                label.Text <- state.ToString()
                        | _-> ()
                | ControlDomain.Settings -> ()
            | AnalysisDomain.TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | BuilderDomain.BuildMember bm ->                 
                    let newState = trussServices.sendMemberOptionToState (newMemberOption.Get) state
                    do  drawTruss newState
                        state <- newState
                        setjointList newState
                        label.Text <- newState.ToString()
                | BuilderDomain.BuildForce bf -> () 
                | BuilderDomain.BuildSupport bs -> ()
                | BuilderDomain.Control -> 
                    let newState = trussServices.sendMemberOptionToState (newMemberOption.Get) state                    
                    do  drawTruss newState
                        state <- newState                        
                        setjointList newState
                        label.Text <- newState.ToString()
            | AnalysisDomain.TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | ControlDomain.Delete -> ()
                | ControlDomain.Inspect ->()
                | ControlDomain.Modify -> 
                    let newState = trussServices.sendPointToModification p state
                    do  setOrgin p
                        drawTruss newState
                        state <- newState
                        label.Text <- newState.ToString()
                    match newState with
                    | AnalysisDomain.TrussAnalysisDomain.SelectionState ss -> 
                        match ss.modification, ss.forces, ss.supports with
                        |None, None, Some s-> selectedPart.Set (Some (ElementDomain.Support s.Head))
                        | None, Some f, None-> selectedPart.Set (Some (ElementDomain.Force f.Head))
                        | Some m, None, None -> selectedPart.Set None
                        | None, None, None
                        | _ -> selectedPart.Set None                       
                    | _ -> ()
            | AnalysisDomain.TrussAnalysisDomain.AnalysisState a ->                 
                match a.analysis with
                | MethodOfJointsAnalysis _ -> do methodOfJointsAnalysis.handleMojMouseDown p state                    
            | AnalysisDomain.TrussAnalysisDomain.ErrorState es -> 
                match es.errors with 
                | [ErrorDomain.NoJointSelected] -> ()                    
                | _ -> label.Text <- state.ToString()
    let handleMouseUp (e : Input.MouseButtonEventArgs) =          
        selection_Control.handleSelectionMouseUp()
        match state with
        | AnalysisDomain.TrussAnalysisDomain.TrussState ts -> 
            match ts.mode with
            | ControlDomain.MemberBuild -> do memberBuilder_Control.handleMBMouseUp ()
            | ControlDomain.ForceBuild -> ()
            | ControlDomain.SupportBuild -> ()
            | ControlDomain.Analysis -> ()
            | ControlDomain.Selection -> ()
            | ControlDomain.Settings -> ()
        | AnalysisDomain.TrussAnalysisDomain.BuildState bs -> 
            match bs.buildOp with
            | BuilderDomain.BuildMember bm -> ()
            | BuilderDomain.BuildForce bf -> ()
            | BuilderDomain.BuildSupport bs -> ()
            | BuilderDomain.Control -> ()
        | AnalysisDomain.TrussAnalysisDomain.SelectionState ss -> 
            match ss.mode with
            | ControlDomain.Modify 
            | ControlDomain.Delete -> 
                match ss.forces, ss.supports, ss.members with
                | Some [f], None, None -> 
                    selectedPart.Set (Some (ElementDomain.Force f))
                    drawTruss state
                | None, Some [s], None ->  
                    selectedPart.Set (Some (ElementDomain.Support s)) 
                    drawTruss state
                | None, None, Some [m] -> 
                    selectedPart.Set (Some (ElementDomain.Member m)) 
                    drawTruss state
                | _ -> ()            
            | ControlDomain.Inspect -> 
                let truss = getTrussFrom state
                match ss.forces, ss.members, ss.supports with
                | Some f, None, None -> 
                    let i = trussServices.getForceIndex f.Head truss
                    wolframMessage.Set ("--Force " + i.ToString() + "--")
                    wolframCode.Set (trussServices.getAnalysisReport state)
                    setGraphicsFromKernel kernel
                | None, Some m, None ->                
                    let i = trussServices.getMemberIndex m.Head truss
                    wolframMessage.Set ("--Member " + i.ToString() + "--")
                    wolframCode.Set (trussServices.getAnalysisReport state)
                    setGraphicsFromKernel kernel 
                | None, None, Some s -> 
                    let i,st = trussServices.getSupportIndex s.Head truss
                    wolframMessage.Set ("--" + st + " Support " + i.ToString() + "--")
                    wolframCode.Set (trussServices.getAnalysisReport state)
                    setGraphicsFromKernel kernel 
                | _ -> 
                    wolframMessage.Set "--Select a Truss Part--"
                    wolframCode.Set "Ready"
                drawTruss state
        | AnalysisDomain.TrussAnalysisDomain.AnalysisState a -> ()
        | AnalysisDomain.TrussAnalysisDomain.ErrorState es -> 
            match es.errors with 
            | [ErrorDomain.NoJointSelected] -> 
                match  string trussMode_ComboBox.SelectedItem with 
                | "Force Builder" -> 
                    let newState = trussServices.setTrussMode ControlDomain.ControlMode.ForceBuild state
                    drawTruss newState
                    state <- newState
                    label.Text <- newState.ToString()
                | "Member Builder" -> 
                    let newState = trussServices.setTrussMode ControlDomain.ControlMode.SupportBuild state
                    drawTruss newState
                    state <- newState
                    label.Text <- newState.ToString()
                | _ -> ()
            | _ -> ()
    let handleMouseMove (e : Input.MouseEventArgs) = mousePosition.Set (adjustMouseEventArgPoint e)
    let handleKeyDown (e:Input.KeyEventArgs) =
        memberBuilder_Control.handleMBKeyDown e
        jointForceBuilder_Control.handleFBKeyDown e
        supportBuilder_Control.handleSBKeyDown e
        selection_Control.handleSelectionKeyDown e
        match e.Key with 
        | Input.Key.Enter ->
            match state with
            | AnalysisDomain.TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | ControlDomain.MemberBuild ->                     
                    let newState = trussServices.sendMemberOptionToState (newMemberOption.Get) state
                    do  drawTruss newState
                        //drawBuildJoint p
                        state <- newState
                        setjointList newState
                        label.Text <- newState.ToString()                   
                | ControlDomain.ForceBuild -> ()                    
                | ControlDomain.SupportBuild -> ()
                | ControlDomain.Analysis -> ()
                | ControlDomain.Selection -> ()
                | ControlDomain.Settings -> ()
            | AnalysisDomain.TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | BuilderDomain.BuildMember _bm -> ()
                | BuilderDomain.BuildForce _bf -> ()
                | BuilderDomain.BuildSupport bs -> ()
                | BuilderDomain.Control -> 
                    let newState = 
                        match memberBuilder_Control.IsVisible, jointForceBuilder_Control.IsVisible, supportBuilder_Control.IsVisible with
                        | true, false, false -> trussServices.sendMemberOptionToState (newMemberOption.Get) state
                        | false, true, false -> trussServices.sendJointForceOptionToState (newForceOption.Get) state                        
                        | false, false, true -> trussServices.sendSupportOptionToState (newSupportOption.Get) state
                        | _ -> state
                    do  drawTruss newState
                        state <- newState                        
                        setjointList newState
                        label.Text <- newState.ToString()
            | AnalysisDomain.TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | ControlDomain.Delete -> ()
                | ControlDomain.Inspect -> ()
                | ControlDomain.Modify -> ()                    
            | AnalysisDomain.TrussAnalysisDomain.AnalysisState s -> ()
            | AnalysisDomain.TrussAnalysisDomain.ErrorState es -> ()
        | Input.Key.Delete -> 
            match state with
            | AnalysisDomain.TrussAnalysisDomain.TrussState ts -> 
                match ts.mode with
                | ControlDomain.MemberBuild -> ()
                | ControlDomain.ForceBuild -> ()                    
                | ControlDomain.SupportBuild -> ()
                | ControlDomain.Analysis -> ()
                | ControlDomain.Selection -> ()
                | ControlDomain.Settings -> ()
            | AnalysisDomain.TrussAnalysisDomain.BuildState bs -> 
                match bs.buildOp with
                | BuilderDomain.BuildMember _bm -> ()
                | BuilderDomain.BuildForce bf -> ()
                | BuilderDomain.BuildSupport bs -> ()
                | BuilderDomain.Control -> ()
            | AnalysisDomain.TrussAnalysisDomain.SelectionState ss -> 
                match ss.mode with
                | ControlDomain.SelectionMode.Delete -> 
                    let newState = trussServices.removeTrussPartFromTruss state            
                    do  state <- newState 
                        label.Text <- newState.ToString()
                        drawTruss newState
                | ControlDomain.SelectionMode.Modify  -> ()
                | ControlDomain.SelectionMode.Inspect -> ()
            | AnalysisDomain.TrussAnalysisDomain.AnalysisState s -> ()                
            | AnalysisDomain.TrussAnalysisDomain.ErrorState es -> ()
        | _ -> () // logic for other keys
    let handleSystemChanged s =        
        match system.Get with
        | None -> label.Text <- "System = None" 
        | Some (ElementDomain.System.TrussSystem truss) ->
            let newState = trussServices.setTruss truss s
            do  state <- newState
                drawTruss newState
                jointList.Set (trussServices.getJointSeqFromTruss truss |> Seq.toList)
                label.Text <- state.ToString()
        | _ -> ()
    let handleSelectedPartChanged s =        
        match selectedPart.Get with
        | None -> 
            let newState = trussServices.setTrussMode ControlDomain.ControlMode.Selection s
            state <- newState
            drawTruss newState
        | Some p -> ()
    let handleExternalStatechanged () = 
        state <- externalState.Get
        label.Text <- externalState.Get.ToString()

    (*Initialize*)
    do  controls_StackPanel.Children.Add(methodOfJointsAnalysis) |> ignore
        this.Content <- screen_Grid        
        setGraphicsFromKernel kernel
        
    (*add event handlers*)        
        this.PreviewKeyDown.AddHandler(Input.KeyEventHandler(fun _ e -> handleKeyDown(e)))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        this.PreviewMouseLeftButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
        this.PreviewMouseLeftButtonUp.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseUp(e)))
        this.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove e))
        trussMode_ComboBox.SelectionChanged.AddHandler(SelectionChangedEventHandler(fun _ _ -> handleTrussModeChanged state))
        orginPosition.Changed.AddHandler((fun _ _ -> drawTruss state))
        system.Changed.AddHandler((fun _ _ -> handleSystemChanged state))
        selectedPart.Changed.AddHandler((fun _ _ -> handleSelectedPartChanged state))
        selectionMode .Changed.AddHandler((fun _ _ -> handleSelectionModeChanged state))
        externalState.Changed.AddHandler((fun _ _ -> handleExternalStatechanged ()))