﻿namespace Math.Presentation.WolframEngine

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
->  Task 2 - UI controls
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
        | Selection
        | Analysis
        | MemberBuild
        | ForceBuild
        | SupportBuild

    // Types to describe error results
    type Error = 
        | LazyCoder  
        | Input
        | TrussBuildOpFailure
        | TrussModeError
        | WrongStateData
        | NoJointSelected
        | Other of string
 
    // Data associated with each state     
    type TrussStateData = {truss:Truss; mode:TrussMode} // Includes the empty truss
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
    type GetPointFromMemberBuilder = MemberBuilder -> System.Windows.Point
    type GetTrussFromState = TrussAnalysisState -> Truss
    type GetPointFromForceBuilder = ForceBuilder -> System.Windows.Point
    type GetPointFromForce = Force -> System.Windows.Point
    type GetDirectionFromForce = Force -> float
    type GetPointFromSupport = Support -> System.Windows.Point
    type GetDirectionFromSupport = Support -> float
    type SendPointToMemberBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToForceBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToForceBuilder = float -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToRollerSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToPinSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToSupportBuilder = float*float option -> TrussAnalysisState -> TrussAnalysisState    
    type SetTrussMode = TrussMode -> TrussAnalysisState -> TrussAnalysisState

    type TrussServices = 
        {getJointSeqFromTruss:GetJointSeqFromTruss;
         getMemberSeqFromTruss:GetMemberSeqFromTruss;
         getPointFromMemberBuilder:GetPointFromMemberBuilder;
         getPointFromForceBuilder:GetPointFromForceBuilder;
         getPointFromSupport:GetPointFromSupport;
         getDirectionFromSupport:GetDirectionFromSupport;
         getPointFromForce:GetPointFromForce;
         getDirectionFromForce:GetDirectionFromForce;
         getTrussFromState:GetTrussFromState;
         sendPointToMemberBuilder:SendPointToMemberBuilder;
         sendPointToForceBuilder:SendPointToForceBuilder;
         sendMagnitudeToForceBuilder:SendMagnitudeToForceBuilder;
         sendPointToRollerSupportBuilder:SendPointToRollerSupportBuilder;
         sendPointToPinSupportBuilder:SendPointToPinSupportBuilder;
         sendMagnitudeToSupportBuilder:SendMagnitudeToSupportBuilder;
         setTrussMode:SetTrussMode}

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
        | true -> mb |> BuildMember |> TrussBuildOp
        | false -> (a,j) |> Member |> TrussPart
        
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

    let getTrussFromState (state :TrussAnalysisState) = 
        match state with
        | TrussDomain.TrussState ts -> ts.truss
        | TrussDomain.BuildState bs-> bs.truss
        | TrussDomain.SelectionState ss -> ss.truss
        | TrussDomain.ErrorState es -> es.truss
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
    let getPointFromMemberBuilder (mb:MemberBuilder) = 
        let j,_ = mb
        let p = System.Windows.Point(getXFrom j, getYFrom j)
        p
    let getPointFromForceBuilder (fb:ForceBuilder) =         
        let p = System.Windows.Point(getXFrom fb.joint, getYFrom fb.joint)
        p
    let getDirectionFromForce (f:Force) =
        let y = f.direction.Y - (getYFrom f.joint)
        let x = f.direction.X - (getXFrom f.joint)
        Math.Atan2(y,x) * (-180./Math.PI)
    let getPointFromForce (force:Force) = System.Windows.Point(getXFrom force.joint , getYFrom force.joint)
    let getPointFromSupport (support:Support) = 
        match support with
        | Pin (f,_) -> getPointFromForce f
        | Roller f -> getPointFromForce f
    let getDirectionFromSupport (support:Support) = 
        match support with
        | Pin (f,_) -> (getDirectionFromForce f) - 90.
        | Roller f -> (getDirectionFromForce f) - 90.

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
               | TrussPart tp -> {truss = (tp |> addTrussPartToTruss bs.truss); mode = MemberBuild} |> TrussState
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

    let setTrussMode (mode :TrussMode) (state :TrussAnalysisState) = {truss = getTrussFromState state; mode = mode} |> TrussState

    let createServices () = 
       {getJointSeqFromTruss = getJointSeqFromTruss;
        getMemberSeqFromTruss = getMemberSeqFromTruss;
        getPointFromMemberBuilder = getPointFromMemberBuilder;
        getTrussFromState = getTrussFromState;
        getPointFromForceBuilder = getPointFromForceBuilder
        getPointFromForce = getPointFromForce
        getDirectionFromForce = getDirectionFromForce
        getPointFromSupport = getPointFromSupport;
        getDirectionFromSupport = getDirectionFromSupport;
        sendPointToMemberBuilder = sendPointToMemberBuilder;
        sendPointToForceBuilder = sendPointToForceBuilder;
        sendMagnitudeToForceBuilder = sendMagnitudeToForceBuilder;
        sendPointToRollerSupportBuilder = sendPointToRollerSupportBuilder;
        sendPointToPinSupportBuilder = sendPointToPinSupportBuilder;
        sendMagnitudeToSupportBuilder = sendMagnitudeToSupportBuilder;
        setTrussMode = setTrussMode}

type TrussAnalysis() as this  =  
    inherit UserControl()    
    do Install() |> ignore

    let initialState = TrussDomain.TrussState {truss = {members=[]; forces=[]; supports=[]}; mode = TrussDomain.MemberBuild}

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
    let clear = SolidColorBrush(Colors.Transparent)
    let black = SolidColorBrush(Colors.Black) 
    let blue = SolidColorBrush(Colors.Blue)
    let olive = SolidColorBrush(Colors.Olive)
    let red = SolidColorBrush(Colors.Red)
    let green = SolidColorBrush(Colors.Green)
    let bluePen, redPen, blackPen = Pen(blue, 0.5), Pen(red, 0.5), Pen(black, 0.5) 
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
    let trussMember (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line()
        do  l.Stroke <- black
            l.StrokeThickness <- 0.4
            l.Visibility <- Visibility.Visible
            l.X1 <- p1.X
            l.Y1 <- p1.Y
            l.X2 <- p2.X
            l.Y2 <- p2.Y
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
    let trussForce (p1:System.Windows.Point, p2:System.Windows.Point) = 
        let l = Line()
        do  l.Stroke <- green
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
    let support () =
        let path = Path()            
        do  path.Stroke <- black
            path.Fill <- olive
            path.Opacity <- 0.5
            path.StrokeThickness <- 1.
        path

    do  bluePen.Freeze()
        redPen.Freeze()
        blackPen.Freeze() 
    
    (*Controls*)      
    let label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- state.ToString()
        l
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
        // Controls border
    let trussControls_Border = 
        let border = Border()            
        do  border.BorderBrush <- black
            border.Cursor <- System.Windows.Input.Cursors.Arrow
            border.Background <- clear
            border.IsHitTestVisible <- true
            border.BorderThickness <- Thickness(Left = 1., Top = 1., Right = 1., Bottom = 1.)
            border.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 10., Right = 10., Bottom = 10.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_StackPanel) |> ignore
            sp.Children.Add(trussMemberBuilder_StackPanel) |> ignore
            sp.Children.Add(supportType_StackPanel) |> ignore
            sp.Children.Add(trussForceBuilder_StackPanel) |> ignore
            border.Child <- sp
        border
        // Wolfram result 
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
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()
    
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
        // Send
    let sendPointToMemberBuilder (p:System.Windows.Point) s = trussServices.sendPointToMemberBuilder p s
    let sendPointToForceBuilder (p:System.Windows.Point) s = trussServices.sendPointToForceBuilder p s
    let sendPointToPinSupportBuilder (p:System.Windows.Point) s = trussServices.sendPointToPinSupportBuilder p s
    let sendPointToRollerSupportBuilder (p:System.Windows.Point) s = trussServices.sendPointToRollerSupportBuilder p s
    let sendMagnitudeToForceBuilder m s = trussServices.sendMagnitudeToForceBuilder m s
    // For supports, set the initial magnitude to 0.0 until the truss reaction forces are evaluated.
    let sendMagnitudeToSupportBuilder m s = trussServices.sendMagnitudeToSupportBuilder m s
        // Draw
    let drawJoint (p:System.Windows.Point) =
        let j = trussJoint p
        do  canvas.Children.Add(j) |> ignore
    let drawMember (p1:System.Windows.Point, p2:System.Windows.Point) =
        let l = trussMember (p1,p2)
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
    let drawBuildForceLine (p1:System.Windows.Point, p2:System.Windows.Point) =
        let l = trussForce (p1,p2)
        do  canvas.Children.Add(l) |> ignore    
    let drawBuildForceDirection (p:System.Windows.Point, dir:float, mag:float) =
        let angle = 8.
        let length = 25.
        match mag > 0. with
        | true -> 
            let p1 = System.Windows.Point(p.X + (length * cos ((dir - angle) * Math.PI/180.)), p.Y - (length * sin ((dir - angle) * Math.PI/180.)))
            let p2 = System.Windows.Point(p.X + (length * cos ((dir + angle) * Math.PI/180.)), p.Y - (length * sin ((dir + angle) * Math.PI/180.)))
            let l1 = trussForce (p,p1)
            let l2 = trussForce (p,p2)
            let l3 = trussForce (p1,p2)
            do  canvas.Children.Add(l1) |> ignore
                canvas.Children.Add(l2) |> ignore
                canvas.Children.Add(l3) |> ignore
        | false -> 
            let p1 = System.Windows.Point(p.X - (length * cos ((dir - angle) * Math.PI/180.)), p.Y + (length * sin ((dir - angle) * Math.PI/180.)))
            let p2 = System.Windows.Point(p.X - (length * cos ((dir + angle) * Math.PI/180.)), p.Y + (length * sin ((dir + angle) * Math.PI/180.)))
            let l1 = trussForce (p,p1)
            let l2 = trussForce (p,p2)
            let l3 = trussForce (p1,p2)
            do  canvas.Children.Add(l1) |> ignore
                canvas.Children.Add(l2) |> ignore
                canvas.Children.Add(l3) |> ignore    
    let drawBuildSupport (p:System.Windows.Point, dir:float) =
        let angle = 45.
        let length = 25.         
        let p1 = System.Windows.Point(p.X + (length * cos ((dir - angle - 90.) * Math.PI/180.)), p.Y - (length * sin ((dir - angle - 90.) * Math.PI/180.)))
        let p2 = System.Windows.Point(p.X + (length * cos ((dir + angle - 90.) * Math.PI/180.)), p.Y - (length * sin ((dir + angle - 90.) * Math.PI/180.)))
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
                match rollerSupport_RadioButton.IsChecked.Value with
                | true -> psc.Add(a2)
                | false -> psc.Add(l2)                    
                psc.Add(l3)
                pf.Segments <- psc
                pf.StartPoint <- p
                pfc.Add(pf)
                pg.Figures <- pfc
                support.Data <- pg
            support
        do  canvas.Children.Add(support) |> ignore
    let drawSupport (support:TrussDomain.Support) = 
        let p = TrussServices.getPointFromSupport support
        let dir = TrussServices.getDirectionFromSupport support
        do  drawBuildSupportJoint p
            drawBuildSupport (p,dir)
    let drawForce (force:TrussDomain.Force) =  
        let p = TrussServices.getPointFromForce force
        let vp = System.Windows.Point(force.direction.X,force.direction.Y)        
        let arrowPoint = 
            match force.magnitude > 0. with 
            | true -> p
            | false -> vp
        let dir = TrussServices.getDirectionFromForce force
        do  drawBuildForceJoint p
            drawBuildForceLine (p,vp)
            drawBuildForceDirection (arrowPoint,dir,force.magnitude)
    let drawTruss s =
        do  canvas.Children.Clear()
            canvas.Children.Add(label) |> ignore
            canvas.Children.Add(trussControls_Border) |> ignore
        let joints = getJointsFrom s
        let members = getMembersFrom s 
        let forces = List.toSeq (getForcesFrom s)
        let supports = List.toSeq (getSupportsFrom s)
        Seq.iter (fun m -> drawMember m) members
        Seq.iter (fun j -> drawJoint j) joints
        Seq.iter (fun f -> drawForce f) forces
        Seq.iter (fun s -> drawSupport s) supports
        // Set
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
            let newState = 
                match forceBuilder_RadioButton.IsChecked.Value, 
                      memberBuilder_RadioButton.IsChecked.Value,                                            
                      supportBuilder_RadioButton.IsChecked.Value,
                      selection_RadioButton.IsChecked.Value,
                      analysis_RadioButton.IsChecked.Value,                                            
                      forceBuilder_RadioButton.IsMouseOver,
                      memberBuilder_RadioButton.IsMouseOver,                                            
                      supportBuilder_RadioButton.IsMouseOver,
                      selection_RadioButton.IsMouseOver,
                      analysis_RadioButton.IsMouseOver with
                | true,false,false,false,false, true,false,false,false,false
                | false,true,false,false,false, true,false,false,false,false
                | false,false,true,false,false, true,false,false,false,false
                | false,false,false,true,false, true,false,false,false,false
                | false,false,false,false,true, true,false,false,false,false ->                 
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceMagnitude_TextBox.IsReadOnly <- false
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the focre horizontal."
                    trussForceMagnitude_TextBox.Text <- "Enter magnitude"
                    trussServices.setTrussMode TrussDomain.TrussMode.ForceBuild state
                | true,false,false,false,false, false,true,false,false,false
                | false,true,false,false,false, false,true,false,false,false
                | false,false,true,false,false, false,true,false,false,false
                | false,false,false,true,false, false,true,false,false,false
                | false,false,false,false,true, false,true,false,false,false -> 
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    trussServices.setTrussMode TrussDomain.TrussMode.MemberBuild state
                | true,false,false,false,false, false,false,true,false,false
                | false,true,false,false,false, false,false,true,false,false
                | false,false,true,false,false, false,false,true,false,false
                | false,false,false,true,false, false,false,true,false,false
                | false,false,false,false,true, false,false,true,false,false -> 
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    supportType_StackPanel.Visibility <- Visibility.Visible
                    trussForceMagnitude_TextBox.IsReadOnly <- true
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the contact plane from horizontal."
                    trussForceMagnitude_TextBox.Text <- "0"
                    trussServices.setTrussMode TrussDomain.TrussMode.SupportBuild state                                    
                | _ -> state //TrussDomain.ErrorState {errors = [TrussDomain.TrussModeError]; truss = getTrussFrom state}
            
            do  state <- newState
                label.Text <- state.ToString() 
        | false -> 
            match state with
            | TrussDomain.TrussState ts -> 
                match ts.mode with
                | TrussDomain.TrussMode.MemberBuild ->                
                    let newState = sendPointToMemberBuilder p state
                    do  drawBuildJoint p1
                        state <- newState
                        trussMemberP1X_TextBox.IsReadOnly <- true
                        trussMemberP1Y_TextBox.IsReadOnly <- true
                        trussMemberP2_StackPanel.Visibility <- Visibility.Visible
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)).ToString()
                | TrussDomain.TrussMode.ForceBuild ->                     
                    match joint with
                    | None ->                         
                        state <- TrussDomain.ErrorState {errors = [TrussDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        let newState = sendPointToForceBuilder p state
                        drawBuildForceJoint p
                        state <- newState
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)).ToString()
                | TrussDomain.TrussMode.SupportBuild -> 
                    match joint with
                    | None ->                         
                        state <- TrussDomain.ErrorState {errors = [TrussDomain.NoJointSelected]; truss = getTrussFrom state}
                        label.Text <- state.ToString()
                    | Some i -> 
                        match pinSupport_RadioButton.IsChecked.Value with
                        | true -> 
                            let newState = sendPointToPinSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                        | false -> 
                            let newState = sendPointToRollerSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                | TrussDomain.TrussMode.Analysis -> ()
                | TrussDomain.TrussMode.Selection -> ()
            | TrussDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussDomain.BuildMember bm ->                 
                    let newState = sendPointToMemberBuilder p state
                    do  drawTruss newState
                        state <- newState
                        trussMemberP1X_TextBox.IsReadOnly <- true
                        trussMemberP1Y_TextBox.IsReadOnly <- true
                        trussMemberP2X_TextBox.IsReadOnly <- true
                        trussMemberP2Y_TextBox.IsReadOnly <- true
                        trussMemberP2_StackPanel.Visibility <- Visibility.Collapsed
                        label.Text <- "Joints " + (Seq.length (getJointsFrom newState)) .ToString()
                | TrussDomain.BuildForce bf -> ()
                | TrussDomain.BuildSupport bs -> ()
            | TrussDomain.SelectionState ss -> ()
            | TrussDomain.ErrorState es -> 
                match es.errors with 
                | [TrussDomain.NoJointSelected] -> ()                    
                | _ -> ()
    let handleMouseUp () =         
        match state with
        | TrussDomain.TrussState ts -> 
            match ts.mode with
            | TrussDomain.TrussMode.MemberBuild -> ()
            | TrussDomain.TrussMode.ForceBuild -> ()
            | TrussDomain.TrussMode.SupportBuild -> ()
            | TrussDomain.TrussMode.Analysis -> ()
            | TrussDomain.TrussMode.Selection -> ()
        | TrussDomain.BuildState bs -> 
            match bs.buildOp with
            | TrussDomain.BuildMember bm -> ()
            | TrussDomain.BuildForce bf -> ()
            | TrussDomain.BuildSupport bs -> ()
        | TrussDomain.SelectionState ss -> ()
        | TrussDomain.ErrorState es -> 
            match es.errors with 
            | [TrussDomain.NoJointSelected] -> 
                match  forceBuilder_RadioButton.IsChecked.Value,
                        memberBuilder_RadioButton.IsChecked.Value with 
                | true, false -> 
                    let newState = trussServices.setTrussMode TrussDomain.TrussMode.ForceBuild state
                    drawTruss newState
                    state <- newState
                    label.Text <- newState.ToString()
                | false, true -> ()
                | _ -> ()
            | _ -> ()
    let handleMouseMove (e : Input.MouseEventArgs) =
        let p2 = adjustMouseEventArgPoint e        
        match state with
        | TrussDomain.TrussState ts -> 
            match ts.mode with
            | TrussDomain.TrussMode.MemberBuild -> 
                match trussControls_Border.IsMouseOver with 
                | false -> 
                    match trussMemberP1X_TextBox.IsReadOnly && trussMemberP1Y_TextBox.IsReadOnly with
                    | false -> ()
                    | true -> 
                        do  trussMemberP1X_TextBox.Text <- p2.X.ToString()
                            trussMemberP1Y_TextBox.Text <- p2.Y.ToString()
                | true -> ()
            | TrussDomain.TrussMode.ForceBuild -> ()
            | TrussDomain.TrussMode.SupportBuild -> ()
            | TrussDomain.TrussMode.Analysis -> ()
            | TrussDomain.TrussMode.Selection -> ()
        | TrussDomain.BuildState bs ->
            match bs.buildOp with
            | TrussDomain.BuildMember bm ->
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
            | TrussDomain.BuildForce bf -> ()
            | TrussDomain.BuildSupport bs -> ()
        | TrussDomain.SelectionState ss -> ()
        | TrussDomain.ErrorState es -> ()
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
            | TrussDomain.TrussState ts -> 
                match ts.mode with
                | TrussDomain.TrussMode.MemberBuild ->                
                    match p1 with 
                    | Some p ->
                        let newState = sendPointToMemberBuilder p state
                        do  drawBuildJoint p
                            state <- newState
                            trussMemberP1X_TextBox.IsReadOnly <- true
                            trussMemberP1Y_TextBox.IsReadOnly <- true
                            trussMemberP2_StackPanel.Visibility <- Visibility.Visible
                            label.Text <- state.ToString()
                    | None -> label.Text <- state.ToString()
                | TrussDomain.TrussMode.ForceBuild -> ()                    
                | TrussDomain.TrussMode.SupportBuild -> ()
                | TrussDomain.TrussMode.Analysis -> ()
                | TrussDomain.TrussMode.Selection -> ()
            | TrussDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussDomain.BuildMember _bm ->                 
                    match p2 with
                    | None -> ()
                    | Some p -> 
                        let newState = sendPointToMemberBuilder p state
                        do  drawTruss newState
                            state <- newState
                            trussMemberP1X_TextBox.IsReadOnly <- true
                            trussMemberP1Y_TextBox.IsReadOnly <- true
                            trussMemberP2X_TextBox.IsReadOnly <- true
                            trussMemberP2Y_TextBox.IsReadOnly <- true
                            trussMemberP2_StackPanel.Visibility <- Visibility.Collapsed
                            label.Text <- state.ToString()
                | TrussDomain.BuildForce bf -> 
                    match magb, dirb with
                    | true, true when bf._direction = None -> 
                        let jointPoint = TrussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = sendMagnitudeToForceBuilder mag state |> sendPointToForceBuilder dirPoint
                        state <- newState
                        drawBuildForceLine (jointPoint,dirPoint)
                        drawBuildForceDirection (arrowPoint,dir,mag)
                        label.Text <- newState.ToString()
                    | true, true -> 
                        let jointPoint = TrussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = sendMagnitudeToForceBuilder mag state //|> sendPointToForceBuilder dirPoint
                        state <- newState
                        drawBuildForceLine (jointPoint,dirPoint)
                        drawBuildForceDirection (arrowPoint,dir,mag)
                        label.Text <-  newState.ToString()
                    | true, false -> 
                        let newState = sendMagnitudeToForceBuilder mag state
                        state <- newState
                        label.Text <- newState.ToString()
                    | false, true -> 
                        let jointPoint = TrussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let newState = sendPointToForceBuilder dirPoint state
                        state <- newState
                        drawBuildForceLine (jointPoint,dirPoint)
                        label.Text <- newState.ToString()
                    | false, false -> () 
                | TrussDomain.BuildSupport bs -> 
                    match dirb with
                    | true -> 
                        let jointPoint = 
                            match bs with 
                            | TrussDomain.Roller f -> TrussServices.getPointFromForceBuilder f
                            | TrussDomain.Pin (f,_) -> TrussServices.getPointFromForceBuilder f
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos ((dir + 90.) * Math.PI/180.)), jointPoint.Y - (50.0 * sin ((dir + 90.) * Math.PI/180.)))  
                        let arrowPoint = jointPoint                            
                        let newState = 
                            match rollerSupport_RadioButton.IsChecked.Value with
                            | true -> sendMagnitudeToSupportBuilder (mag,None) state |> sendPointToRollerSupportBuilder dirPoint
                            | false -> sendMagnitudeToSupportBuilder (mag,Some mag) state |> sendPointToPinSupportBuilder dirPoint
                        state <- newState
                        drawBuildSupport (arrowPoint,dir)
                        label.Text <-  newState.ToString()
                    | false -> 
                        let newState = sendMagnitudeToSupportBuilder (mag,None) state
                        state <- newState
                        label.Text <- newState.ToString()                    
            | TrussDomain.SelectionState ss -> ()
            | TrussDomain.ErrorState es -> ()
        | _ -> () // logic for other keys

    (*Initialize*)
    // label.Text <- state.ToString()
    do  this.Content <- screen_Grid        
        setGraphicsFromKernel kernel
        
    (*add event handlers*)        
        this.PreviewKeyDown.AddHandler(Input.KeyEventHandler(fun _ e -> handleKeyDown(e)))
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        this.PreviewMouseLeftButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
        this.PreviewMouseLeftButtonUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleMouseUp()))
        this.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove e))

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
