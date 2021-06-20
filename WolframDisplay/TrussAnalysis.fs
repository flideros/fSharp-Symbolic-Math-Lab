namespace Math.Presentation.WolframEngine

open System
open System.Numerics
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink

///  CURRENT  PROJECT  ///
(*Truss analysis: This project will explore the mathematics of truss analysis. 
->  Task 1 - Domain Model and implementation
    Task 2 - UI controls
    Task 3 - Develop Wolfram Language Stucture and Interactions
    Task 4 - Continuous Development of features
*)

module TrussDomain =
    
    // Parameters
    type X = X of float
    type Y = Y of float
    type Z = Z of float

    // Domain Types
    type Joint = {x:X; y:Y} //; z:Z}
    type MemberBuilder = (Joint*(Joint option))
    type Member = (Joint*Joint)
    type ForceBuilder = {_magnitude:float option; _direction:Vector option; joint:Joint}
    type Force = {magnitude:float; direction:Vector; joint:Joint}
    type SupportBuilder = | Roller of ForceBuilder | Pin of (ForceBuilder*ForceBuilder)   
    type Support = | Roller of Force | Pin of (Force*Force)    
    type Truss = {members:Member list; forces:Force list; supports:Support list}
    type TrussStability = | Stable | NotEnoughReactions | ReactionsAreParallel 
                          | ReactionsAreConcurrent | InternalCollapseMechanism
    type TrussDeterminacy = | Determinate | Indeterminate
    
    type TrussPart = // A joint by itself is not a part, rather it is a cosequence of connecting two (or more) members
        | Member of Member
        | Force of Force
        | Support of Support
    
    type TrussBuildOp =
        | BuildMember of MemberBuilder
        | BuildForce of ForceBuilder
        | BuildSupport of SupportBuilder

    type BuildOpResult =
        | TrussPart of TrussPart
        | TrussBuildOp of TrussBuildOp

    type TrussMode =
        | Select
        | Delete
        | MemberBuild
        | ForceBuild
        | SupportBuild

    // Types to describe error results
    type Error = 
        | LazyCoder  
        | Input
        | TrussBuildOpFailure
        | WrongStateData
        | Other of string
 
    // Data associated with each state     
    type TrussStateData = {truss:Truss;mode:TrussMode} // Includes the empty truss
    type TrussBuildData = {buildOp : TrussBuildOp;  truss : Truss}
    type SelectionStateData = {truss:Truss; members:Member option; forces:Force option; supports:Support option}    
    type ErrorStateData = {errors : Error list; truss : Truss}
    
    // States
    type TrussAnalysisState =         
        | TrussState of TrussStateData
        | BuildState of TrussBuildData
        | SelectionState of SelectionStateData
        | ErrorState of ErrorStateData
    
    // Services
    type GetJointSeqFromTruss = Truss -> System.Windows.Point seq
    type GetMemberSeqFromTruss = Truss -> (System.Windows.Point * System.Windows.Point) seq
    type SendPointToMemberBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToForceBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToForceBuilder = float -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToRollerSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToPinSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToSupportBuilder = float*float option -> TrussAnalysisState -> TrussAnalysisState

    type TrussServices = 
        {getJointSeqFromTruss:GetJointSeqFromTruss;
         getMemberSeqFromTruss:GetMemberSeqFromTruss;
         sendPointToMemberBuilder:SendPointToMemberBuilder;
         sendPointToForceBuilder:SendPointToForceBuilder;
         sendMagnitudeToForceBuilder:SendMagnitudeToForceBuilder;
         sendPointToRollerSupportBuilder:SendPointToRollerSupportBuilder;
         sendPointToPinSupportBuilder:SendPointToPinSupportBuilder;
         sendMagnitudeToSupportBuilder:SendMagnitudeToSupportBuilder}

module TrussImplementation = 
    open TrussDomain
    
    let getYFrom (j:Joint) = match j.y with | Y y -> y
    let getXFrom (j:Joint) = match j.x with | X x -> x
    let getJointListFrom (members: Member list) = 
        let (l1,l2) = List.unzip members
        List.concat [l1;l2] |> List.distinct    
    let getReactionForcesFrom (supports: Support list) = 
        List.fold (fun acc r -> match r with | Roller f -> f::acc  | Pin (f1,f2) -> f1::f2::acc) [] supports
    let getDirectionsFrom (forces:Force list) = List.map (fun x -> 
        Vector(
            x = x.direction.X - (getXFrom x.joint), 
            y = x.direction.Y - (getYFrom x.joint))) forces
    let getLineOfActionFrom (force:Force) = 
        match (force.direction.X - (getXFrom force.joint)) = 0. with 
        | true -> 
            let m = (force.direction.Y - (getYFrom force.joint)) / (force.direction.X - (getXFrom force.joint))                   
            let b = (getYFrom force.joint) - (m * (getXFrom force.joint))
            (m,b)
        | false -> (0.,(getYFrom force.joint))
    let getJointFromSupportBuilder (sb:SupportBuilder) = 
        match sb with
        | SupportBuilder.Roller r -> r.joint
        | SupportBuilder.Pin (p,_) -> p.joint


    let addTrussPartToTruss (t:Truss) (p:TrussPart)  = 
        match p with 
        | Member m -> {t with members = m::t.members}
        | Force f -> {t with forces = f::t.forces}
        | Support s -> {t with supports = s::t.supports}
    
    // Workflow for building a member
    let makeMemberBuilderFrom (j:Joint) = MemberBuilder (j,None)
    let addJointToMemberBuilder (j:Joint) (mb:MemberBuilder) = 
        let a, _b = mb
        match a = j with 
        | false -> mb |> BuildMember |> TrussBuildOp
        | true -> (a,j) |> Member |> TrussPart
        
    // Workflow for building a force
    let makeForceBuilderFrom (j:Joint) = {_magnitude = None; _direction = None; joint = j}
    let addDirectionToForceBuilder (v:Vector) (fb:ForceBuilder) = 
        match fb._magnitude.IsSome with
        | false -> {_magnitude = fb._magnitude; _direction = Some v; joint = fb.joint}  |> BuildForce |> TrussBuildOp
        | true -> {magnitude = fb._magnitude.Value; direction = v; joint = fb.joint} |> Force |> TrussPart
    let addMagnitudeToForceBuilder m (fb:ForceBuilder) = 
        match fb._direction.IsSome with
        | false -> {_magnitude = Some m; _direction = fb._direction; joint = fb.joint} |> BuildForce |> TrussBuildOp
        | true -> {magnitude = m; direction = fb._direction.Value; joint = fb.joint} |> Force |> TrussPart
        
    // Workflow for building a support
    let makeRollerSupportBuilderFrom (j:Joint) = SupportBuilder.Roller {_magnitude = None; _direction = None; joint = j}
    let makePinSupportBuilderFrom (j:Joint) = SupportBuilder.Pin ({_magnitude = None; _direction = None; joint = j},{_magnitude = None; _direction = None; joint = j})
    let addDirectionToSupportBuilder (v:Vector*(Vector option)) (sb:SupportBuilder) =
        let v1,v2 = v
        match sb with 
        | SupportBuilder.Roller fb -> 
            match fb._magnitude.IsSome with 
            | false -> SupportBuilder.Roller {fb with _direction = Some v1} |> BuildSupport |> TrussBuildOp
            | true -> {magnitude = fb._magnitude.Value; direction = v1; joint = fb.joint} |> Roller |> Support |> TrussPart
        | SupportBuilder.Pin fb -> 
            let f1,f2 = fb 
            match f1._magnitude.IsSome && f2._magnitude.IsSome && v2.IsSome with
            | false -> SupportBuilder.Pin ({f1 with  _direction = Some v1},{f2 with _direction = v2 }) |> BuildSupport |> TrussBuildOp
            | true -> ({magnitude = f1._magnitude.Value; direction = v1; joint = f1.joint},
                       {magnitude = f2._magnitude.Value; direction = v2.Value; joint = f2.joint}) |> Pin |> Support |> TrussPart
    let addMagnitudeToSupportBuilder (m:float*(float option)) (sb:SupportBuilder) =
        let m1,m2 = m
        match sb with 
        | SupportBuilder.Roller fb -> 
            match fb._direction.IsSome with 
            | false -> SupportBuilder.Roller {fb with _magnitude = Some m1}  |> BuildSupport|> TrussBuildOp
            | true -> {magnitude = m1; direction = fb._direction.Value; joint = fb.joint} |> Roller |> Support |> TrussPart
        | SupportBuilder.Pin fb -> 
            let f1,f2 = fb 
            match f1._direction.IsSome && f2._direction.IsSome && m2.IsSome with
            | false -> SupportBuilder.Pin ({f1 with  _magnitude = Some m1},{f2 with _magnitude = m2 }) |> BuildSupport |> TrussBuildOp
            | true -> ({magnitude = m1; direction = f1._direction.Value; joint = f1.joint},
                       {magnitude = m2.Value; direction = f2._direction.Value; joint = f2.joint}) |> Pin |> Support |> TrussPart

    // Inspect truss
    let checkTrussStability (truss:Truss) = 
        let m = truss.members.Length 
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1) 0 truss.supports
        let j = (getJointListFrom truss.members).Length
        let checkNotEnoughReactions = m + r < 2 * j
        let checkForParallelReactions = 
            let reactions = getReactionForcesFrom truss.supports |> getDirectionsFrom            
            let rec compareReactions (reactions:Vector list) (premise:bool) = 
                match reactions with 
                | [] -> premise
                | _::[] -> premise
                | r1::r2::tail -> 
                    match r1.X*r2.Y = r1.Y*r2.X with 
                    | true -> compareReactions (r2::tail) true                                    
                    | false ->  false                                     
            compareReactions reactions false
        let checkForConcurrentReactions = 
            let reactions = getReactionForcesFrom truss.supports
            let compareReactions (reactions:Force list) = 
                match reactions with 
                | [] -> false
                | _::[] -> false
                | r1::r2::tail -> 
                    match (r1.direction.X - (getXFrom r1.joint)),(r2.direction.X - (getXFrom r2.joint)) with
                    | 0.,0. -> false 
                    | 0.,_ -> 
                        let a = 0.
                        let b = (r2.direction.Y - (getYFrom r2.joint)) / (r2.direction.X - (getXFrom r2.joint))                   
                        let c = getYFrom r1.joint
                        let d = (getYFrom r2.joint) - (b * (getXFrom r2.joint))
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/b
                            let y =  c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail
                    | _,0. -> 
                        let a = (r1.direction.Y - (getYFrom r1.joint)) / (r1.direction.X - (getXFrom r1.joint))
                        let b = 0.
                        let c = (getYFrom r1.joint) - (a * (getXFrom r1.joint))
                        let d = (getYFrom r2.joint)
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/a
                            let y = a*((d - c)/a) + c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail
                    | _,_ ->                     
                        let a = (r1.direction.Y - (getYFrom r1.joint)) / (r1.direction.X - (getXFrom r1.joint))
                        let b = (r2.direction.Y - (getYFrom r2.joint)) / (r2.direction.X - (getXFrom r2.joint))                   
                        let c = (getYFrom r1.joint) - (a * (getXFrom r1.joint))
                        let d = (getYFrom r2.joint) - (b * (getXFrom r2.joint))
                        match a = b with
                        | true -> false
                        | false -> 
                            let x = (d - c)/(a - b)
                            let y = a*((d - c)/(a - b)) + c
                            List.forall (fun f -> 
                                let m,b = (getLineOfActionFrom f)
                                m*x + b = y) tail  
            compareReactions reactions

        match checkNotEnoughReactions, 
              checkForParallelReactions, 
              checkForConcurrentReactions with
        | true, true,true -> [NotEnoughReactions; ReactionsAreParallel;ReactionsAreConcurrent]
        | true, true,false -> [NotEnoughReactions; ReactionsAreParallel]
        
        | true, false,true -> [NotEnoughReactions;ReactionsAreConcurrent]
        | true, false,false -> [NotEnoughReactions]
        
        | false, true,true -> [ReactionsAreParallel;ReactionsAreConcurrent]
        | false, true,false -> [ReactionsAreParallel]
        
        | false, false,true -> [ReactionsAreConcurrent]
        | false, false,false -> [Stable]
    let checkTrussDeterminacy (truss:Truss)  = 
        let stability = checkTrussStability truss
        let m = truss.members.Length 
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1) 0 truss.supports
        let j = (getJointListFrom truss.members).Length
        match m + r = 2 * j && stability = [Stable] with
        | true  -> Determinate 
        | false -> Indeterminate

module TrussServices = 
    open TrussDomain
    open TrussImplementation

    let makeJointFrom (point :System.Windows.Point) = {x = X point.X; y = Y point.Y}
    let makeVectorFrom (point :System.Windows.Point) = Vector(x = point.X, y = point.Y)

    let getJointSeqFromTruss (t:Truss) =
       let pointMap (j:Joint) = System.Windows.Point (x = (getXFrom j),y = (getYFrom j))
       let j = getJointListFrom t.members
       let l = List.map (fun x -> pointMap x ) j
       List.toSeq l
    let getMemberSeqFromTruss (t:Truss) =
       let memberMap (m:Member) = 
           let a,b = m
           (System.Windows.Point (x = (getXFrom a),y = (getYFrom a)),
            System.Windows.Point (x = (getXFrom b),y = (getYFrom b)))
       let l = List.map (fun x -> memberMap x ) t.members
       List.toSeq l
    
    let sendPointToMemberBuilder (point :System.Windows.Point) (state :TrussAnalysisState) =       
       match state with 
       | TrussState es -> 
           {buildOp = makeMemberBuilderFrom (makeJointFrom point) |> BuildMember; 
            truss = es.truss} |> BuildState
       | BuildState bs-> 
           match bs.buildOp with
           | BuildMember mb -> 
               let result = addJointToMemberBuilder (makeJointFrom point) mb
               match result with
               | TrussPart tp -> {truss = tp |> addTrussPartToTruss bs.truss; mode = MemberBuild} |> TrussState
               | TrussBuildOp bo -> {buildOp = bo; truss = bs.truss} |> BuildState
           | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}           
       | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
       | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendPointToForceBuilder (point :System.Windows.Point) (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> 
            {buildOp = makeForceBuilderFrom (makeJointFrom point) |> BuildForce; truss = es.truss} |> BuildState
        | BuildState bs-> 
            match bs.buildOp with
            | BuildForce fb -> 
                let op = addDirectionToForceBuilder (makeVectorFrom point) fb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = ForceBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState
                 
                                             
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendMagnitudeToForceBuilder magnitude (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> ErrorState {errors = [WrongStateData]; truss = es.truss}
        | BuildState bs-> 
            match bs.buildOp with
            | BuildForce fb -> 
                let op = addMagnitudeToForceBuilder magnitude fb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = ForceBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState       
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendPointToRollerSupportBuilder (point :System.Windows.Point) (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> 
            {buildOp = makeRollerSupportBuilderFrom (makeJointFrom point) |> BuildSupport; truss = es.truss} |> BuildState
        | BuildState bs-> 
            match bs.buildOp with
            | BuildSupport sb -> 
                let op = addDirectionToSupportBuilder (makeVectorFrom point,None) sb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = SupportBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendPointToPinSupportBuilder (point :System.Windows.Point) (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> 
            {buildOp = makePinSupportBuilderFrom (makeJointFrom point) |> BuildSupport; truss = es.truss} |> BuildState
        | BuildState bs-> 
            match bs.buildOp with
            | BuildSupport sb -> 
                let j = getJointFromSupportBuilder sb
                let x,y = getXFrom j, getYFrom j
                let point' = System.Windows.Point(x + point.Y,y - point.X)
                let op = addDirectionToSupportBuilder (makeVectorFrom point,Some (makeVectorFrom point')) sb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = SupportBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendMagnitudeToSupportBuilder magnitude (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> ErrorState {errors = [WrongStateData]; truss = es.truss}
        | BuildState bs-> 
            match bs.buildOp with
            | BuildSupport sb -> 
                let op = addMagnitudeToSupportBuilder magnitude sb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = SupportBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState       
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}


    let createServices () = 
       {getJointSeqFromTruss = getJointSeqFromTruss;
        getMemberSeqFromTruss = getMemberSeqFromTruss;
        sendPointToMemberBuilder = sendPointToMemberBuilder;
        sendPointToForceBuilder = sendPointToForceBuilder;
        sendMagnitudeToForceBuilder = sendMagnitudeToForceBuilder;
        sendPointToRollerSupportBuilder = sendPointToRollerSupportBuilder;
        sendPointToPinSupportBuilder = sendPointToPinSupportBuilder;
        sendMagnitudeToSupportBuilder = sendMagnitudeToSupportBuilder}

//   UI Shell, no funtionality at the moment, 
//\/--- but will run an exampe computation ----\/\\
type TrussAnalysis() as this  =  
    inherit UserControl()    
    do Install() |> ignore

    

    let x0,x1,x2,x3,x4 = TrussDomain.X 240., TrussDomain.X 100., TrussDomain.X 200., TrussDomain.X 137., TrussDomain.X 499.
    let y0,y1,y2,y3,y4 = TrussDomain.Y 360., TrussDomain.Y 166., TrussDomain.Y 200., TrussDomain.Y 327., TrussDomain.Y 349.
   
    let j1 = {TrussDomain.x=x3;TrussDomain.y=y0}
    let j2 = {TrussDomain.x=x0;TrussDomain.y=y1}
    let j3 = {TrussDomain.x=x1;TrussDomain.y=y2}
    let j4 = {TrussDomain.x=x1;TrussDomain.y=y3}
    let j5 = {TrussDomain.x=x2;TrussDomain.y=y4}
    let j6 = {TrussDomain.x=x4;TrussDomain.y=y1}
    
    let m1 = j1,j2
    let m2 = j1,j3
    let m3 = j2,j4
    let m4 = j1,j4
    let m5 = j4,j3
    let m6 = j3,j5
    let m7 = j4,j5
    let m8 = j4,j6
    let m9 = j5,j6

    let initialState = TrussDomain.TrussState {truss = {members=[m1;m2;m3;m8;m9]; forces=[]; supports=[]}; mode = TrussDomain.SupportBuild}

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

    (*Model*)        
    let image = Image()
    do  image.SetValue(Panel.ZIndexProperty, -100)    
    let visual = DrawingVisual()     
    let black = SolidColorBrush(Colors.Black)
    let blue = SolidColorBrush(Colors.Blue)
    let red = SolidColorBrush(Colors.Red)
    let bluePen, redPen, blackPen = Pen(blue, 0.5), Pen(red, 0.5), Pen(black, 0.5)
       
    let trussJoint (p:System.Windows.Point) = 
        let radius = 6.
        let e = Ellipse()
        do  e.Fill <- blue
            e.Margin <- Thickness(Left=p.X - radius, Top=p.Y - radius, Right = 0., Bottom = 0.)
            e.Width <- 2.*radius
            e.Height <- 2.*radius
        e
    let trussMember (p1:System.Windows.Point,p2:System.Windows.Point) = 
        let l = Line()
        do  l.Stroke <- red
            l.StrokeThickness <- 2.
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
        l

    do  bluePen.Freeze()
        redPen.Freeze()
        blackPen.Freeze() 
    
    (*Controls*)    
    let label = 
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 240., Top = 450., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 400.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Rock"
        l
    let result_Viewbox image =                    
        let vb = Viewbox()   
        do  vb.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            vb.Child <- image
        vb
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
        sp
    let canvas = 
        let c = Canvas(ClipToBounds = true)        
        do  c.Background <- System.Windows.Media.Brushes.White 
            c.Children.Add(label) |> ignore            
            //c.Children.Add(result_StackPanel) |> ignore            
        c
    let screen_Grid =
        let g = Grid()
        do  g.SetValue(Grid.RowProperty, 1)        
            g.Children.Add(canvas) |> ignore
        g
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()
    
    (*Actions*)        
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
    let getTrussFrom s = 
        match s with
        | TrussDomain.TrussState ts -> ts.truss
        | TrussDomain.BuildState bs-> bs.truss
        | TrussDomain.SelectionState ss -> ss.truss
        | TrussDomain.ErrorState es -> es.truss
    let getJointsFrom s = trussServices.getJointSeqFromTruss (getTrussFrom s)
    let getmembersFrom s = trussServices.getMemberSeqFromTruss (getTrussFrom s)
    
    let getJointIndex (p1:System.Windows.Point) = 
        Seq.tryFindIndex (fun (p2:System.Windows.Point) -> 
            (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 9.) (getJointsFrom state)
    
    let sendPointToMemberBuilder (p:System.Windows.Point) = trussServices.sendPointToMemberBuilder p state
    let sendPointToForceBuilder (p:System.Windows.Point) = trussServices.sendPointToForceBuilder p state
    let sendMagnitudeToForceBuilder m = trussServices.sendMagnitudeToForceBuilder m state
        
    let drawTruss = 
        let drawJoint (p:System.Windows.Point) =
            let j = trussJoint p
            do  canvas.Children.Add(j) |> ignore
        let drawMember (p1:System.Windows.Point,p2:System.Windows.Point) =
            let l = trussMember (p1,p2)
            do  canvas.Children.Add(l) |> ignore
        
        let joints = getJointsFrom state
        let members = getmembersFrom state        
        
        Seq.iter (fun m -> drawMember m) members
        Seq.iter (fun j -> drawJoint j) joints
                

    let setGraphicsFromKernel (k:MathKernel) =
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
            getImages 0             
        | false -> 
          result_StackPanel.Children.Clear()
          let graphics = link.EvaluateToImage("Factor[x^2- 4x + 1]", width = 400, height = 400)
          let image = Image()            
          do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
              result_StackPanel.Children.Add(result_Viewbox image) |> ignore
    
    (*Initialize*)
    do label.Text <- "members: " + (getTrussFrom state).members.Length.ToString()
    do  this.Content <- screen_Grid        
        setGraphicsFromKernel kernel
    
    (*add event handlers*)
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss))

module TrussAnalysis = 
    let window =
        "Needs[\"NETLink`\"]        
        TrussAnalysis[] :=
        	NETBlock[
        		Module[{form, pictureBox},
        			InstallNET[];        			
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\Symbolic_Math.dll\"];
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\ControlLibrary.dll\"];
        			LoadNETAssembly[\"d:\\MyFolders\\Desktop\\SymbolicMath\\Symbolic Math UI\\Bin\\Debug\\WolframDisplay.dll\"];			
        			LoadNETAssembly[\"System.Core\"];
        			LoadNETAssembly[\"System\"];
        			LoadNETAssembly[\"PresentationCore\"];
        			LoadNETAssembly[\"PresentationFramework\"];			
        			LoadNETType[\"System.Windows.Window\"];
        			form = NETNew[\"System.Windows.Window\"];
        			form@Width = 600;
        			form@Height = 600;			
        			form@Title = \"Truss Analysis\";
        			pictureBox = NETNew[\"Math.Presentation.WolframEngine.TrussAnalysis\"];        			
        			form@Content = pictureBox;				
        			vertices = {};			
        			form@ShowDialog[];			
        		]
        	]"
