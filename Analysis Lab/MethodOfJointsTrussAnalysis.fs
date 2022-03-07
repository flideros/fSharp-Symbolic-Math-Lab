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
open AnalysisDomain.TrussAnalysisDomain

type MethodOfJointsAnalysisControl(
        kernel:MathKernel, 
        state:SharedValue<TrussAnalysisState>,
        setGraphicsFromKernel:(MathKernel -> unit),        
        drawSolvedMember:((System.Windows.Point*System.Windows.Point) -> Brush -> unit),
        drawTruss:(TrussAnalysisState -> unit),
        drawForce:(SolidColorBrush -> LoadDomain.JointForce -> unit),
        drawMemberLabel:(Point*Point -> int -> unit),
        drawForceLabel:(System.Windows.Vector -> int -> float -> unit),
        drawReactionForceLabel:(System.Windows.Vector -> (int*string) -> float -> unit),
        setOrgin:(System.Windows.Point-> unit),
        wolframResult : SharedValue<string>,
        wolframCode : SharedValue<string>,
        wolframMessage : SharedValue<string> 
        ) as this =  
    inherit UserControl()    
    do Install() |> ignore

    (*Truss Services*)
    let trussServices = TrussServices.createServices()

    (*Controls*) 
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
            sp.Visibility <- Visibility.Visible
        sp 
    let screen_Grid =
        let g = Grid()              
        do  g.Children.Add(analysis_StackPanel) |> ignore
        g
    
    let drawSolvedMembers (s:AnalysisDomain.TrussAnalysisDomain.TrussAnalysisState) = 
        match s with
        | AnalysisDomain.TrussAnalysisDomain.AnalysisState a -> 
            match a.analysis with
            | MethodOfJointsAnalysis Truss -> ()
            | MethodOfJointsAnalysis (SupportReactionEquations _sre) -> () 
            | MethodOfJointsAnalysis (SupportReactionResult _srr)-> ()                    
            | MethodOfJointsAnalysis (MethodOfJointsCalculation mjc) ->                     
                do  List.iter (fun  (n, p) -> 
                    match n, TrussServices.getMemberOptionFromTrussPart p with
                    | (z, Some m) when z = 0. -> drawSolvedMember m olive
                    | (z, Some m) when z > 0. -> drawSolvedMember m blue2
                    | (z, Some m) when z < 0. -> drawSolvedMember m red
                    | _ -> ()) mjc.solvedMembers
            | MethodOfJointsAnalysis (MethodOfJointsAnalysisReport mja) ->                     
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

    let getTrussFrom s = trussServices.getTrussFromState s 
    let getForcesFrom s = (trussServices.getTrussFromState s).forces 
    let getSupportsFrom s = (trussServices.getTrussFromState s).supports   
    let getMembersFrom s = trussServices.getMemberSeqFromTruss (getTrussFrom s)
        
    let setStateFromAnaysis s =
        match this.Visibility = Visibility.Visible with
        | false -> state.Set s
        | true ->
            do  setGraphicsFromKernel kernel
            let newState = 
                match s with 
                | AnalysisDomain.TrussAnalysisDomain.AnalysisState a -> 
                    match a.analysis with
                    | MethodOfJointsAnalysis Truss -> s
                    | MethodOfJointsAnalysis (SupportReactionEquations _r) -> trussServices.analyzeEquations wolframResult.Get s
                    | MethodOfJointsAnalysis (SupportReactionResult _r) -> s 
                    | MethodOfJointsAnalysis (MethodOfJointsCalculation _mj) -> trussServices.analyzeEquations wolframResult.Get s 
                    | MethodOfJointsAnalysis (MethodOfJointsAnalysisReport _mj) -> s                
                | _ -> s       
            let newCode = 
                match newState with 
                | AnalysisDomain.TrussAnalysisDomain.AnalysisState a -> 
                    match a.analysis with
                    | MethodOfJointsAnalysis Truss -> "opps7"
                    | MethodOfJointsAnalysis (SupportReactionEquations r) -> "opps6" 
                    | MethodOfJointsAnalysis (SupportReactionResult r) -> "\"Choose a joint to begin Method of Joints analysis\""                    
                    | MethodOfJointsAnalysis (MethodOfJointsCalculation r) -> "\"Choose next joint\"" 
                    | MethodOfJointsAnalysis (MethodOfJointsAnalysisReport _) -> TrussServices.getAnalysisReport newState//"\"Analysis Complete\""                
                | _ -> "opps2"
            let newMessage = 
                match newState with 
                | AnalysisDomain.TrussAnalysisDomain.AnalysisState a -> 
                    match a.analysis with
                    | MethodOfJointsAnalysis Truss -> "opps4"
                    | MethodOfJointsAnalysis (SupportReactionEquations r) -> "opps5" 
                    | MethodOfJointsAnalysis (SupportReactionResult r) -> "Choose a joint to begin Method of Joints analysis."                    
                    | MethodOfJointsAnalysis (MethodOfJointsCalculation r) -> "Choose next joint."
                    | MethodOfJointsAnalysis (MethodOfJointsAnalysisReport _) -> "Analysis Complete. Click Compute to see report."                
                | _ -> "opps3"
            let members = getMembersFrom newState
            let forces = getForcesFrom newState
            let supports = getSupportsFrom newState        
            let reactions = trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value newState
            do  state.Set newState          
                wolframCode.Set newCode
                wolframMessage.Set newMessage
                Seq.iter (fun (f:LoadDomain.JointForce) -> 
                    match f.magnitude = 0.0 with 
                    | true -> () 
                    | false -> 
                        drawForce blue f
                        drawReactionForceLabel f.direction (trussServices.getSupportIndexAtJoint f.joint supports) f.magnitude) reactions            
                Seq.iteri (fun i m -> drawMemberLabel m i) members
                Seq.iteri (fun i (f:LoadDomain.JointForce) -> drawForceLabel f.direction i f.magnitude) forces
                drawSolvedMembers newState

    let handleMouseDown p (s:AnalysisDomain.TrussAnalysisDomain.TrussAnalysisState) =
        match this.Visibility = Visibility.Visible with
        | false -> state.Set s
        | true ->            
            match s with
            | AnalysisDomain.TrussAnalysisDomain.AnalysisState a ->
                match a.analysis with
                | MethodOfJointsAnalysis Truss -> ()
                | MethodOfJointsAnalysis (SupportReactionEquations r) -> () 
                | MethodOfJointsAnalysis (SupportReactionResult r) -> 
                    let newState = trussServices.sendPointToCalculation p s
                    let newCode = 
                        match newState with 
                        | AnalysisDomain.TrussAnalysisDomain.AnalysisState a ->
                            match a.analysis with
                            | MethodOfJointsAnalysis (MethodOfJointsCalculation r) -> WolframServices.solveEquations r.memberEquations r.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state.Set newState
                        wolframCode.Set newCode
                        setOrgin p 
                        drawTruss newState
                        drawSolvedMembers newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)                    
                | MethodOfJointsAnalysis (MethodOfJointsCalculation mjc) -> 
                    let newState = trussServices.sendPointToCalculation p s
                    let newCode = 
                        match newState with 
                        | AnalysisDomain.TrussAnalysisDomain.AnalysisState a ->
                            match a.analysis with
                            | MethodOfJointsAnalysis (MethodOfJointsCalculation mjc) -> WolframServices.solveEquations mjc.memberEquations mjc.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state.Set newState
                        wolframCode.Set newCode
                        setOrgin p 
                        drawTruss newState
                        drawSolvedMembers newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)                     
                | MethodOfJointsAnalysis (MethodOfJointsAnalysisReport mja) -> do drawSolvedMembers s                     
            | _ -> ()
    let handleClickEvent(s:AnalysisDomain.TrussAnalysisDomain.TrussAnalysisState) =
        match this.Visibility = Visibility.Visible with
        | false -> state.Set s
        | true ->
            match compute_Button.IsPressed with 
            | true -> setStateFromAnaysis s
            | false -> 
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
                        trussServices.checkTruss (trussServices.getTrussFromState s)
                        |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                    wolframCode.Set (trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState)
                    state.Set newState
                | true,false, false,true
                | false,true, false,true -> 
                    xAxis_RadioButton.IsChecked <- Nullable false 
                    yAxis_RadioButton.IsChecked <- Nullable true                                
                    let newState = 
                        trussServices.checkTruss (trussServices.getTrussFromState s)
                        |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                    wolframCode.Set (trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState)
                    state.Set newState
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
                        drawTruss s
                        drawSolvedMembers s
                        Seq.iter (fun (f:LoadDomain.JointForce) -> 
                            match f.magnitude = 0.0 with 
                            | true -> () 
                            | false -> drawForce red f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value s)
                    | true,false, false,true
                    | false,true, false,true -> 
                        resultant_RadioButton.IsChecked <- Nullable false 
                        components_RadioButton.IsChecked <- Nullable true
                        drawTruss s
                        drawSolvedMembers s
                        Seq.iter (fun (f:LoadDomain.JointForce) -> 
                            match f.magnitude = 0.0 with 
                            | true -> () 
                            | false -> drawForce blue f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value s)
                    | _ -> ()
    
    (*Initialize*)
    do  this.Content <- screen_Grid        
        setGraphicsFromKernel kernel
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> setStateFromAnaysis state.Get))

    member _this.handleMojClick s = handleClickEvent s
    member _this.handleMojMouseDown s = handleMouseDown s
    member _this.setStateFromMojAnaysis s = setStateFromAnaysis s