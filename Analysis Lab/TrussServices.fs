namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Windows

module TrussServices = 
    open ErrorDomain
    open TrussAnalysisDomain    
    open BuilderDomain
    open AtomicDomain
    open LoadDomain
    open ElementDomain
    
    // Services
    type CheckSupportTypeIsRoller = Support -> bool
    type CheckTruss = Truss -> TrussAnalysisState

    type GetJointSeqFromTruss = Truss -> System.Windows.Point seq
    type GetMemberSeqFromTruss = Truss -> (System.Windows.Point * System.Windows.Point) seq
    type GetPointFromMemberBuilder = MemberBuilder -> System.Windows.Point
    type GetTrussFromState = TrussAnalysisState -> Truss
    type GetPointFromForceBuilder = JointForceBuilder -> System.Windows.Point
    type GetPointFromForce = JointForce -> System.Windows.Point
    type GetDirectionFromForce = JointForce -> float
    type GetPointFromSupport = Support -> System.Windows.Point
    type GetDirectionFromSupport = Support -> float    
    type GetSelectedMemberFromState = TrussAnalysisState -> (System.Windows.Point * System.Windows.Point) option
    type GetSelectedForceFromState = TrussAnalysisState -> (System.Windows.Point * System.Windows.Point) option
    type GetSelectedSupportFromState = TrussAnalysisState -> (System.Windows.Point * float * bool) option
    type GetSupportReactionEquationsFromState = bool -> TrussAnalysisState -> TrussAnalysisState
    type GetSupportReactionSolve = bool -> TrussAnalysisState -> string
    type GetReactionForcesFromState = bool -> TrussAnalysisState -> JointForce list
    type GetMemberOptionFromTrussPart = TrussPart -> (System.Windows.Point*System.Windows.Point) Option
    type GetAnalysisReport = TrussAnalysisState -> string
    type GetSupportIndexAtJoint = Joint -> Support list -> int*string
    type GetMemberIndex = Member -> Truss -> int
    type GetForceIndex = JointForce -> Truss -> int
    type GetSupportIndex = Support -> Truss -> int*string
    
    type SendPointToModification = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToMemberBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToForceBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToForceBuilder = float -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToRollerSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToPinSupportBuilder = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState
    type SendMagnitudeToSupportBuilder = float*float option -> TrussAnalysisState -> TrussAnalysisState    
    type SendMemberToState = System.Windows.Shapes.Line -> TrussSelectionMode -> TrussAnalysisState -> TrussAnalysisState
    type SendForceToState = System.Windows.Shapes.Line -> TrussSelectionMode -> TrussAnalysisState -> TrussAnalysisState
    type SendSupportToState = System.Windows.Shapes.Path -> TrussSelectionMode -> TrussAnalysisState -> TrussAnalysisState
    type SendStateToSupportBuilder = bool -> TrussAnalysisState -> TrussAnalysisState
    type SendPointToCalculation = System.Windows.Point -> TrussAnalysisState -> TrussAnalysisState

    type ModifyTrussForce = float -> float -> TrussAnalysisState -> TrussAnalysisState
    type ModifyTrussSupport = float -> TrussAnalysisState -> TrussAnalysisState
    type AnalyzeEquations = string -> TrussAnalysisState -> TrussAnalysisState
    
    type SetTrussMode = TrussMode -> TrussAnalysisState -> TrussAnalysisState
    type SetSelectionMode = TrussSelectionMode -> TrussAnalysisState -> TrussAnalysisState

    type RemoveTrussPartFromTruss = TrussAnalysisState -> TrussAnalysisState
    
    type TrussServices = 
        {checkSupportTypeIsRoller:CheckSupportTypeIsRoller;
         checkTruss:CheckTruss;
         getJointSeqFromTruss:GetJointSeqFromTruss;
         getMemberSeqFromTruss:GetMemberSeqFromTruss;
         getPointFromMemberBuilder:GetPointFromMemberBuilder;
         getPointFromForceBuilder:GetPointFromForceBuilder;
         getPointFromSupport:GetPointFromSupport;
         getDirectionFromSupport:GetDirectionFromSupport;
         getPointFromForce:GetPointFromForce;
         getDirectionFromForce:GetDirectionFromForce;
         getTrussFromState:GetTrussFromState;
         getSelectedMemberFromState:GetSelectedMemberFromState
         getSelectedForceFromState:GetSelectedForceFromState
         getSelectedSupportFromState:GetSelectedSupportFromState
         sendPointToMemberBuilder:SendPointToMemberBuilder;
         sendPointToForceBuilder:SendPointToForceBuilder;
         sendMagnitudeToForceBuilder:SendMagnitudeToForceBuilder;
         sendPointToRollerSupportBuilder:SendPointToRollerSupportBuilder;
         sendPointToPinSupportBuilder:SendPointToPinSupportBuilder;
         sendMagnitudeToSupportBuilder:SendMagnitudeToSupportBuilder;
         sendMemberToState:SendMemberToState;
         sendForceToState:SendForceToState;
         sendSupportToState:SendSupportToState;
         setTrussMode:SetTrussMode;
         setSelectionMode:SetSelectionMode;
         removeTrussPartFromTruss:RemoveTrussPartFromTruss;
         getSupportReactionEquationsFromState:GetSupportReactionEquationsFromState;
         getSupportReactionSolve:GetSupportReactionSolve;
         sendStateToSupportBuilder:SendStateToSupportBuilder;
         analyzeEquations:AnalyzeEquations;
         getReactionForcesFromState:GetReactionForcesFromState;
         sendPointToCalculation:SendPointToCalculation;
         getMemberOptionFromTrussPart:GetMemberOptionFromTrussPart;
         getAnalysisReport:GetAnalysisReport;
         getSupportIndexAtJoint:GetSupportIndexAtJoint;
         getMemberIndex:GetMemberIndex;
         getForceIndex:GetForceIndex;
         getSupportIndex:GetSupportIndex;
         sendPointToModification:SendPointToModification;
         modifyTrussForce:ModifyTrussForce;
         modifyTrussSupport:ModifyTrussSupport
         }

    open TrussImplementation

    let checkSupportTypeIsRoller (support:Support) = match support with | Roller r -> true | Pin p -> false
    let checkTruss (truss:Truss) = 
        {truss = truss; 
         stability = checkTrussStability truss; 
         determinancy = checkTrussDeterminacy truss;
         analysis = Truss} |> TrussAnalysisState.AnalysisState

    let makeJointFrom (point :System.Windows.Point) = {x = X point.X; y = Y point.Y}
    let makeVectorFrom (point :System.Windows.Point) = Vector(x = point.X, y = point.Y)

    let getTrussFromState (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> ts.truss
        | TrussAnalysisDomain.BuildState bs-> bs.truss
        | TrussAnalysisDomain.SelectionState ss -> ss.truss
        | TrussAnalysisDomain.AnalysisState s -> s.truss
        | TrussAnalysisDomain.ErrorState es -> es.truss
    let getSelectedMemberFromState (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> None
        | TrussAnalysisDomain.BuildState bs-> None
        | TrussAnalysisDomain.SelectionState ss -> 
            match ss.members with
            | None -> None
            | Some m -> 
                let a,b = m.Head
                (System.Windows.Point (x = (getXFrom a),y = (getYFrom a)),
                 System.Windows.Point (x = (getXFrom b),y = (getYFrom b))) |> Some
        | TrussAnalysisDomain.AnalysisState s -> None
        | TrussAnalysisDomain.ErrorState es -> None
    let getSelectedForceFromState (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> None
        | TrussAnalysisDomain.BuildState bs-> None
        | TrussAnalysisDomain.SelectionState ss -> 
            match ss.forces with
            | None -> None
            | Some f -> 
                let a,b = f.Head.joint,f.Head.direction
                (System.Windows.Point (x = (getXFrom a),y = (getYFrom a)),
                 System.Windows.Point (x = (b.X),y = (b.Y))) |> Some
        | TrussAnalysisDomain.AnalysisState s -> None
        | TrussAnalysisDomain.ErrorState es -> None
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
    let getPointFromForceBuilder (fb:JointForceBuilder) =         
        let p = System.Windows.Point(getXFrom fb.joint, getYFrom fb.joint)
        p
    let getDirectionFromForce (f:JointForce) =
        let y = f.direction.Y - (getYFrom f.joint)
        let x = f.direction.X - (getXFrom f.joint)
        Math.Atan2(y,x) * (-180./Math.PI)
    let getPointFromForce (force:JointForce) = System.Windows.Point(getXFrom force.joint , getYFrom force.joint)
    let getPointFromSupport (support:Support) = 
        match support with
        | Pin p -> getPointFromForce p.normal
        | Roller f -> getPointFromForce f
    let getDirectionFromSupport (support:Support) = 
        match support with
        | Pin p -> -(getDirectionFromForce p.normal)
        | Roller f -> -(getDirectionFromForce f) 
    let getSelectedSupportFromState (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> None
        | TrussAnalysisDomain.BuildState bs-> None
        | TrussAnalysisDomain.SelectionState ss -> 
            match ss.supports with
            | None -> None
            | Some spt -> 
                match spt.Head with
                | Roller r -> 
                    let p = System.Windows.Point (x = getXFrom r.joint, y = getYFrom r.joint)
                    let d = getDirectionFromSupport spt.Head
                    Some (p,d,true)
                | Pin p -> 
                    let p = System.Windows.Point (x = getXFrom p.tangent.joint, y = getYFrom p.normal.joint)
                    let d = getDirectionFromSupport spt.Head
                    Some (p,d,false)

        | TrussAnalysisDomain.AnalysisState s -> None
        | TrussAnalysisDomain.ErrorState es -> None
    let getSupportReactionEquationsFromState (yAxis: bool) (state :TrussAnalysisState) = 
        let truss = getTrussFromState state        
        let parts = getPartListFrom truss
        match yAxis with
        | true -> 
            match state with
            | TrussAnalysisDomain.TrussState ts -> state
            | TrussAnalysisDomain.BuildState bs-> state
            | TrussAnalysisDomain.SelectionState ss -> state
            | TrussAnalysisDomain.AnalysisState s -> 
                match List.contains Stable s.stability with
                | true ->                 
                    {s with analysis = 
                            {momentEquations = getYMomentReactionEquations parts;
                             forceXEquation = getXForceReactionEquation parts;
                             forceYEquation = getYForceReactionEquation parts} 
                             |> SupportReactionEquations} 
                             |> AnalysisState
                | false -> state
            | TrussAnalysisDomain.ErrorState es -> state
        | false -> 
            match state with
            | TrussAnalysisDomain.TrussState ts -> state
            | TrussAnalysisDomain.BuildState bs-> state
            | TrussAnalysisDomain.SelectionState ss -> state
            | TrussAnalysisDomain.AnalysisState s -> 
                match List.contains Stable s.stability with
                | true ->                 
                    {s with analysis = 
                            {momentEquations = getXMomentReactionEquations parts;
                             forceXEquation = getXForceReactionEquation parts; 
                             forceYEquation = getYForceReactionEquation parts} 
                             |> SupportReactionEquations} 
                             |> AnalysisState
                | false -> state
            | TrussAnalysisDomain.ErrorState es -> state
    let getReactionForcesFromState showComponents (state :TrussAnalysisState)  =         
        match state with
        | TrussAnalysisDomain.TrussState ts -> []
        | TrussAnalysisDomain.BuildState bs-> []
        | TrussAnalysisDomain.SelectionState ss -> []
        | TrussAnalysisDomain.AnalysisState s -> 
            match s.analysis with
            | Truss -> []
            | SupportReactionEquations sre -> []
            | SupportReactionResult srr -> 
                List.fold (fun acc r -> 
                    let result = getResultantForcesFrom r
                    match r.xReactionForce,  r.yReactionForce, showComponents with
                    | Some a, Some b, false -> result::acc
                    | None, Some b, false -> result::acc
                    | Some a, None, false -> result::acc
                    | None, None, false -> acc
                    | Some a, Some b, true -> a::b::acc
                    | None, Some b, true -> b::acc
                    | Some a, None, true -> a::acc
                    | None, None, true -> acc) [] srr.reactions            
            | MethodOfJointsCalculation mj -> 
                List.fold (fun acc r -> 
                    let result = getResultantForcesFrom r
                    match r.xReactionForce,  r.yReactionForce, showComponents with
                    | Some a, Some b, false -> result::acc
                    | None, Some b, false -> result::acc
                    | Some a, None, false -> result::acc
                    | None, None, false -> acc
                    | Some a, Some b, true -> a::b::acc
                    | None, Some b, true -> b::acc
                    | Some a, None, true -> a::acc
                    | None, None, true -> acc) [] mj.reactions            
            | MethodOfJointsAnalysis mj -> 
                List.fold (fun acc r -> 
                    let result = getResultantForcesFrom r
                    match r.xReactionForce,  r.yReactionForce, showComponents with
                    | Some a, Some b, false -> result::acc
                    | None, Some b, false -> result::acc
                    | Some a, None, false -> result::acc
                    | None, None, false -> acc
                    | Some a, Some b, true -> a::b::acc
                    | None, Some b, true -> b::acc
                    | Some a, None, true -> a::acc
                    | None, None, true -> acc) [] mj.reactions
        | TrussAnalysisDomain.ErrorState es -> []
    let getMemberOptionFromTrussPart p = 
        match p with
        | Member (j1,j2) -> Some (System.Windows.Point (getXFrom j1,(getYFrom j1)),System.Windows.Point (getXFrom j2,(getYFrom j2)))
        | _ -> None
    let getSupportIndexAtJoint (j:Joint) supports = 
        let i = 
            List.findIndex (fun (x:ElementDomain.Support) -> 
                match x with 
                | Pin p -> p.normal.joint = j
                | Roller r -> r.joint = j
                ) supports
        match supports.[i] with
        | Pin p -> i,"Pin"
        | Roller r -> i,"Roller"

    let sendPointToModification (p1 :System.Windows.Point) (state :TrussAnalysisState) =       
        let joints = getJointSeqFromTruss (getTrussFromState state)
        let joint  = Seq.tryFind (fun (p2:System.Windows.Point) ->  (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) joints
        match state with 
        | TrussState es ->             
            match joint with
            | None -> state
            | Some p -> 
                {truss = es.truss; 
                 members = None; 
                 forces = None; 
                 supports = None; 
                 modification = Some (makeJointFrom p);
                 mode = Modify} |> SelectionState            
        | BuildState bs-> ErrorState {errors = [WrongStateData]; truss = bs.truss}           
        | SelectionState ss -> 
            match ss.mode with
            | TrussAnalysisDomain.Inspect          
            | TrussAnalysisDomain.Delete -> state
            | TrussAnalysisDomain.Modify -> 
                match joint with
                | None ->
                    match ss.modification with
                    | None -> state
                    | Some j -> {ss with members = None; forces = None; supports = None; modification = None; truss = modifyTruss ss.truss (makeJointFrom p1) j} |> SelectionState // execute modification and return None
                | Some p ->                     
                    match ss.modification with
                    | None -> {ss with members = None; forces = None; supports = None; modification = Some (makeJointFrom p)} |> SelectionState // find joint and return Some Node
                    | Some j -> {ss with members = None; forces = None; supports = None; modification = None; truss = modifyTruss ss.truss (makeJointFrom p1) j} |> SelectionState // execute modification and return None
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
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
       | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
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
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
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
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendPointToRollerSupportBuilder (point :System.Windows.Point) (state :TrussAnalysisState) =       
        match state with 
        | TrussState es -> 
            {buildOp = makeRollerSupportBuilderFrom (makeJointFrom point) |> BuildSupport; truss = es.truss} |> BuildState
        | BuildState bs-> 
            match bs.buildOp with
            | BuildSupport sb -> 
                let j = getJointFromSupportBuilder sb
                let x,y = getXFrom j, getYFrom j
                let point' = System.Windows.Point(x + (y-point.Y),y - (point.X-x))
                let op = addDirectionToSupportBuilder (makeVectorFrom point',None) sb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = SupportBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
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
                let point' = System.Windows.Point(x + (y-point.Y),y - (point.X-x))
                let op = addDirectionToSupportBuilder (makeVectorFrom point,Some (makeVectorFrom point')) sb
                match op with
                | TrussPart tp -> {truss = addTrussPartToTruss bs.truss tp; mode = SupportBuild} |> TrussState
                | TrussBuildOp tb -> {buildOp = tb; truss = bs.truss} |> BuildState
            | _ -> ErrorState {errors = [TrussBuildOpFailure]; truss = bs.truss}
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
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
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}
    let sendMemberToState (l:System.Windows.Shapes.Line) (sm:TrussSelectionMode)(state :TrussAnalysisState) = 
        let m =  ({x = l.X1 |> X; y = l.Y1 |> Y}, {x = l.X2 |> X; y = l.Y2 |> Y})
        match state with 
        | TrussState  ts -> 
            match ts.mode with
            | Settings -> state
            | Selection -> 
                {truss = ts.truss; members = Some [m]; forces = None; supports = None; modification = None; mode = sm} 
                |> SelectionState
            | Analysis -> state
            | MemberBuild -> state
            | ForceBuild -> state
            | SupportBuild -> state         
        | BuildState  bs -> state
        | SelectionState  ss -> 
            {truss = ss.truss; members = Some [m]; forces = None; supports = None; modification = None; mode = sm} 
            |> SelectionState
        | AnalysisState s -> state    
        | ErrorState  es -> state
    let sendForceToState (l:System.Windows.Shapes.Line) (sm:TrussSelectionMode)(state :TrussAnalysisState) = 
        let p1,p2 =  {x = l.X1 |> X; y = l.Y1 |> Y}, {x = l.X2 |> X; y = l.Y2 |> Y}                    
        match state with 
        | TrussState  ts -> 
            match ts.mode with
            | Settings -> state
            | Selection -> 
                // TODO add force modify functionality
                let f = 
                    let t1 = List.tryFind ( fun x -> x.joint = p1 && x.direction.X = getXFrom p2 && x.direction.Y = getYFrom p2) ts.truss.forces                    
                    let t2 = List.tryFind ( fun x -> x.joint = p2 && x.direction.X = getXFrom p2 && x.direction.Y = getYFrom p2) ts.truss.forces
                    let t3 = List.tryFind ( fun x -> x.joint = p1 && x.direction.X = getXFrom p1 && x.direction.Y = getYFrom p1) ts.truss.forces                    
                    let t4 = List.tryFind ( fun x -> x.joint = p2 && x.direction.X = getXFrom p1 && x.direction.Y = getYFrom p1) ts.truss.forces
                    match t1,t2,t3,t4 with
                    | Some f, None, None, None -> Some [f]
                    | None, Some f, None, None -> Some [f]
                    | None, None, Some f, None -> Some [f]
                    | None, None, None, Some f -> Some [f]                    
                    | _ -> None
                
                {truss = ts.truss; members = None; forces = f; supports = None; modification = None; mode = sm} 
                |> SelectionState
            | Analysis -> state
            | MemberBuild -> state
            | ForceBuild -> state
            | SupportBuild -> state         
        | BuildState  bs -> state
        | SelectionState  ss -> 
            let f = 
                let t1 = List.tryFind ( fun x -> x.joint = p1 && x.direction.X = getXFrom p2 && x.direction.Y = getYFrom p2) ss.truss.forces                    
                let t2 = List.tryFind ( fun x -> x.joint = p2 && x.direction.X = getXFrom p2 && x.direction.Y = getYFrom p2) ss.truss.forces
                let t3 = List.tryFind ( fun x -> x.joint = p1 && x.direction.X = getXFrom p1 && x.direction.Y = getYFrom p1) ss.truss.forces                    
                let t4 = List.tryFind ( fun x -> x.joint = p2 && x.direction.X = getXFrom p1 && x.direction.Y = getYFrom p1) ss.truss.forces
                match t1,t2,t3,t4 with
                | Some f, None, None, None -> Some [f]
                | None, Some f, None, None -> Some [f]
                | None, None, Some f, None -> Some [f]
                | None, None, None, Some f -> Some [f]                    
                | _ -> None
            {truss = ss.truss; members = None; forces = f; supports = None; modification = None; mode = sm} 
            |> SelectionState
        | AnalysisState s -> state
        | ErrorState  es -> state
    let sendSupportToState (path:System.Windows.Shapes.Path) (sm:TrussSelectionMode)(state :TrussAnalysisState) = 
        //let p1,p2 =  {x = l.X1 |> X; y = l.Y1 |> Y}, {x = l.X2 |> X; y = l.Y2 |> Y}                    
        let p = path.Tag :?> System.Windows.Point
        let j = makeJointFrom p
        match state with 
        | TrussState  ts -> 
            match ts.mode with
            | Settings -> state
            | Selection -> 
                let spt = 
                    List.tryFind (                     
                        fun x -> 
                            let f = 
                                match x with
                                | Roller r -> r
                                | Pin p -> p.normal
                            f.joint = j                            
                            ) ts.truss.supports
                let s =  
                    match spt with
                    | Some s -> Some [s]                    
                    | _ -> None
                {truss = ts.truss; members = None; forces = None; supports = s; modification = None; mode = sm} 
                |> SelectionState
            | Analysis -> state
            | MemberBuild -> state
            | ForceBuild -> state
            | SupportBuild -> state         
        | BuildState  bs -> state
        | SelectionState  ss -> 
            // TODO add support modify functionality
            let spt = 
                List.tryFind (                     
                    fun x -> 
                        let f = 
                            match x with
                            | Roller r -> r
                            | Pin p -> p.normal
                        f.joint = j                            
                        ) ss.truss.supports
            let s =  
                match spt with
                | Some s -> Some [s]                    
                | _ -> None
            {truss = ss.truss; members = None; forces = None; supports = s; modification = None; mode = sm} 
            |> SelectionState
        | AnalysisState s -> state
        | ErrorState  es -> state
    let sendStateToSupportBuilder toPin (state :TrussAnalysisState) =
        match state with 
        | TrussState es -> state
        | BuildState bs-> 
            match bs.buildOp with
            | BuildMember _bm -> state
            | BuildForce _bf -> state
            | BuildSupport bs -> 
                match bs with
                | SupportBuilder.Roller {_magnitude = m; _direction = v; joint = j} -> 
                    match toPin with 
                    | true -> sendPointToPinSupportBuilder (System.Windows.Point (getXFrom j,getYFrom j)) (TrussState {truss = getTrussFromState state;mode = SupportBuild}) 
                    | false -> state
                | SupportBuilder.Pin 
                    ({_magnitude = m1; _direction = v1; joint = j1},
                     {_magnitude = m2; _direction = v2; joint = j2}) -> 
                        match toPin with 
                        | true -> state 
                        | false -> sendPointToRollerSupportBuilder (System.Windows.Point (getXFrom j2,getYFrom j2)) (TrussState {truss = getTrussFromState state;mode = SupportBuild})        
        | SelectionState ss -> ErrorState {errors = [WrongStateData]; truss = ss.truss} 
        | AnalysisState s -> ErrorState {errors = [WrongStateData]; truss = s.truss}
        | ErrorState es -> ErrorState {errors = WrongStateData::es.errors; truss = es.truss}  
    let sendPointToCalculation (p1 :System.Windows.Point) (state :TrussAnalysisState) = 
        let joints = getJointSeqFromTruss (getTrussFromState state)
        let joint  = Seq.tryFind (fun (p2:System.Windows.Point) ->  (p1.X - p2.X) ** 2. + (p1.Y - p2.Y) ** 2. < 36.) joints
        match joint with
        | None -> state
        | Some p -> 
            match state with
            | TrussAnalysisDomain.TrussState _ts -> state
            | TrussAnalysisDomain.BuildState _bs-> state 
            | TrussAnalysisDomain.SelectionState _ss -> state
            | TrussAnalysisDomain.ErrorState _es -> state
            | TrussAnalysisDomain.AnalysisState a ->
                match a.analysis with
                | Truss -> state                
                | SupportReactionEquations _sr -> state
                | SupportReactionResult sr -> 
                    let nodes = getNodeList a.truss
                    let variables = getMemberVariables a.truss
                    let selectedNode = List.tryFind (fun (j,_pl) -> (getXFrom j) = p.X && (getYFrom j) = p.Y) nodes
                    let zeroMembers = getZeroForceMembers a.truss |> List.map (fun x -> (0.,x))
                    let memberEquations =
                        match selectedNode with
                        | None -> []
                        | Some n -> analyzeNode n a.truss sr.reactions
                    let newAnalysis = 
                        {solvedMembers = zeroMembers; 
                         memberEquations = memberEquations; 
                         nodes = nodes;
                         reactions = sr.reactions;
                         variables = variables} |> MethodOfJointsCalculation 
                    TrussAnalysisDomain.AnalysisState { a with analysis = newAnalysis } 
                | MethodOfJointsCalculation mj ->                    
                    let selectedNode = List.tryFind (fun (j,_pl) -> (getXFrom j) = p.X && (getYFrom j) = p.Y) mj.nodes                    
                    let memberEquations =
                        match selectedNode with
                        | None -> []
                        | Some n -> analyzeNode n a.truss mj.reactions
                    let newAnalysis =
                        {solvedMembers = mj.solvedMembers; 
                         memberEquations = memberEquations; 
                         nodes = mj.nodes;
                         reactions = mj.reactions;
                         variables = mj.variables} |> MethodOfJointsCalculation
                    TrussAnalysisDomain.AnalysisState { a with analysis = newAnalysis }
                | MethodOfJointsAnalysis _mj -> state

    let modifyForce (newFMag:float) (newFDir:float) (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState _ts -> state
        | TrussAnalysisDomain.BuildState _bs-> state        
        | TrussAnalysisDomain.ErrorState _es -> state
        | TrussAnalysisDomain.AnalysisState a -> state
        | TrussAnalysisDomain.SelectionState ss ->             
            match ss.forces with 
            | None -> state
            | Some f -> 
                let newTruss = modifyTrussForce ss.truss newFMag newFDir f.Head
                {ss with truss = newTruss; forces = None} |> SelectionState
    let modifySupport (newFDir:float) (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState _ts -> state
        | TrussAnalysisDomain.BuildState _bs-> state        
        | TrussAnalysisDomain.ErrorState _es -> state
        | TrussAnalysisDomain.AnalysisState a -> state
        | TrussAnalysisDomain.SelectionState ss ->             
            match ss.supports with 
            | None -> state
            | Some f -> 
                let newTruss = modifyTrussSupport ss.truss newFDir f.Head
                {ss with truss = newTruss; supports = None} |> SelectionState
            
    let setTrussMode (mode :TrussMode) (state :TrussAnalysisState) = {truss = getTrussFromState state; mode = mode} |> TrussState
    let setSelectionMode (mode :TrussSelectionMode) (state :TrussAnalysisState) =
        match state with 
        | TrussState ts -> state
        | BuildState ts -> state
        | SelectionState ts -> SelectionState {ts with mode = mode}
        | AnalysisState s -> state
        | ErrorState ts -> state
    
    let tryParseResult (s:string) = 
        let results = WolframServices.parseResults s
        match List.contains (-1,"Unable to compute result",0.0) results  with
        | true -> None
        | false -> Some (results)
    
    let analyzeEquations (s:string) (state:TrussAnalysisState)  =        
        let result = tryParseResult s
        match result with
        | None -> state
        | Some rList ->             
            match state with
            | TrussAnalysisDomain.TrussState _ts -> state
            | TrussAnalysisDomain.BuildState _bs-> state
            | TrussAnalysisDomain.SelectionState _ss -> state
            | TrussAnalysisDomain.ErrorState _es -> state
            | TrussAnalysisDomain.AnalysisState a -> 
                match a.analysis with
                | Truss -> state                
                | SupportReactionEquations _sr ->
                    let parts = getPartListFrom a.truss
                    let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) parts
                    let findReaction i' r' = List.tryFind (fun x -> match x with | i, r,_m  when i=i' && r=r'-> true | _ -> false) rList
                    let getReactionForce s r m =                    
                        let j = getJointFromSupport s
                        match r with 
                        | "Rx" ->                         
                            let x' = 
                                match m < 0.0 with
                                | false -> (getXFrom j) - 50.0
                                | true -> (getXFrom j) + 50.0
                            Some {magnitude = System.Math.Abs m; direction = Vector (x = x', y = getYFrom j); joint = j}                    
                        | "Ry" -> 
                            let y' = 
                                match m < 0.0 with
                                | false -> (getYFrom j) + 50.0
                                | true -> (getYFrom j) - 50.0
                            Some {magnitude = System.Math.Abs m; direction = Vector (x = getXFrom j, y = y'); joint = j}
                        | _ -> None                
                    let results = 
                        let reactions =
                            List.mapi (
                                (fun i x -> 
                                    {support = x;
                                     xReactionForce = 
                                        match findReaction i "Rx" with 
                                        | None -> None;
                                        | Some (_i,r,m) -> getReactionForce x r m 
                                     yReactionForce = 
                                        match findReaction i "Ry" with 
                                        | None -> None
                                        | Some (_i,r,m) -> getReactionForce x r m
                                        })
                                ) supports
                        {reactions = reactions} |> SupportReactionResult
                    {a with analysis = results} |> AnalysisState
                | SupportReactionResult _sr -> state
                | MethodOfJointsCalculation mj ->                     
                    let truss = getTrussFromState state
                    let getSolvedMember i = truss.members.[i] |> Member
                    let solvedMembers = 
                        [(List.map (fun (i,_s:string,m:float) -> (m,getSolvedMember i)) rList); mj.solvedMembers]
                        |> List.concat 
                        |> List.distinctBy (fun (m,p) -> (m,p))
                        |> List.map (fun x -> TrussMemberForce x) 
                    let replaceMembersWithForces (n:TrussNode) =
                        let (j,pl) = n
                        let memberCount pl' = List.filter (fun x -> match x with | Member _ -> true | _ -> false) pl' |> List.length
                        let newPl = 
                            let rec replace pl' (sMembers:TrussMemberForce list) = 
                                match memberCount pl' > 2 with
                                | false -> pl'
                                | true -> 
                                    match sMembers with
                                    | [] -> pl'
                                    | sm::smt -> 
                                        let newPl' = 
                                            List.map (fun x -> 
                                            match x with 
                                            | Member m -> 
                                                let (_f,mb') = sm
                                                match x = mb' with
                                                | true -> getMemberForceAtJoint j x [sm] 
                                                | false -> x
                                            | _ -> x) pl'
                                        replace newPl' smt
                            replace pl solvedMembers                            
                        (j,newPl)
                    let newNodes = List.map (fun x -> replaceMembersWithForces x) mj.nodes                    
                    match solvedMembers.Length >= truss.members.Length with
                    | false -> 
                        { a with analysis = 
                            {solvedMembers = solvedMembers ; 
                             memberEquations = mj.memberEquations; 
                             nodes = newNodes;//mj.nodes;//
                             reactions = mj.reactions;
                             variables = mj.variables} |> MethodOfJointsCalculation 
                        } |> TrussAnalysisDomain.AnalysisState
                    | true -> 
                        let solvedMembers' = 
                            solvedMembers
                            |> List.countBy (fun (n,l) -> l)
                            |> List.choose 
                                (fun (k,n) -> 
                                    match n = 1 with 
                                    | true -> Some (List.find (fun (n',k') -> k' = k) solvedMembers) 
                                    | false -> Some ( (List.filter (fun (n',k') -> k' = k) solvedMembers |> List.averageBy (fun (n',k') -> n') , k))
                                )
                        {a with analysis =    
                            {zeroForceMembers = List.filter (fun (n,m) -> n = 0.0) solvedMembers' |> List.map  (fun (n,m) -> m);
                             tensionMembers =  List.filter (fun (n,m) -> n > 0.0) solvedMembers';
                             compressionMembers =  List.filter (fun (n,m) -> n < 0.0) solvedMembers';
                             reactions = mj.reactions} |> MethodOfJointsAnalysis  
                         } |> TrussAnalysisDomain.AnalysisState                                
                | MethodOfJointsAnalysis mj -> state

    let removeTrussPart (state :TrussAnalysisState) =
        match state with 
        | SelectionState ss -> 
            let part = 
                match ss.members, ss.forces, ss.supports with
                | Some m,None,None -> Some (Member m.Head)
                | None,Some f,None -> Some (Force f.Head)
                | None,None,Some s -> Some (Support s.Head)
                | _ -> None
            let newTruss = removeTrussPartFromTruss ss.truss part
            
            {truss = newTruss; mode = Selection} |> TrussState
            
        | _ -> state
    let getSupportReactionSolve (yAxis: bool) (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> ""
        | TrussAnalysisDomain.BuildState bs-> ""
        | TrussAnalysisDomain.SelectionState ss -> ""
        | TrussAnalysisDomain.ErrorState es -> ""
        | TrussAnalysisDomain.AnalysisState a -> 
            match a.analysis with
            | Truss -> ""
            | MethodOfJointsCalculation mj -> ""
            | MethodOfJointsAnalysis mj -> ""
            | SupportReactionResult sr -> ""
            | SupportReactionEquations sr -> 
                let eq = 
                    match yAxis with
                    | true -> sr.forceXEquation :: sr.momentEquations
                    | false -> sr.forceYEquation :: sr.momentEquations
                let v = 
                    match sr.momentEquations.Length > 1 with
                    | true -> List.concat [for i in 0..(sr.momentEquations.Length - 1) -> ["Rx" + i.ToString();"Ry" + i.ToString()]]
                    | false -> []
                (WolframServices.solveEquations eq v)
    let getTrussCheck (state :TrussAnalysisState) =
        match state with
        | TrussAnalysisDomain.TrussState ts -> "--Ready--"
        | TrussAnalysisDomain.BuildState bs -> "--Ready--"
        | TrussAnalysisDomain.SelectionState ss -> "--Ready--"                
        | TrussAnalysisDomain.ErrorState es -> "--Ready--"
        | TrussAnalysisDomain.AnalysisState a -> 
            let stability = List.fold (fun acc x -> match acc = "" with | true -> x.ToString() | false -> x.ToString() + ". " + acc) "" a.stability
            let determinancy =  
                match a.determinancy with
                | Determinate -> "Determinate"
                | Indeterminate -> "Indeterminate"
            "\"--Ready--\\n"            
            + stability           
            + "\\n" 
            + determinancy                
            + "\""
     
    let getAnalysisReport (state :TrussAnalysisState) = 
        match state with
        | TrussAnalysisDomain.TrussState ts -> "\"--Ready--\""
        | TrussAnalysisDomain.BuildState bs-> "\"--Ready--\""
        | TrussAnalysisDomain.SelectionState ss -> 
            match ss.forces, ss.members, ss.supports with
            | Some f, None, None -> 
                let p1,p2 = f.Head.joint, f.Head.direction                
                let angle =
                    let x = (p2.X) - (getXFrom p1) 
                    let y = (getYFrom p1) - (p2.Y)  
                    Math.Round((Math.Atan2(y,x) * 180.)/Math.PI,4) 
                "\"Magnitude = " 
                + f.Head.magnitude.ToString()                
                + "\\nAngle = " 
                + angle.ToString() + "\[Degree]"
                + "\\nJoint \\n" 
                + p1.x.ToString() + ", "+ p1.y.ToString()                
                + "\""
            | None, Some m, None -> 
                let p1,p2 = m.Head
                let length = 
                    let x = (getXFrom p2) - (getXFrom p1)
                    let y = (getYFrom p2) - (getYFrom p1)
                    Math.Round(Math.Sqrt (x*x + y*y),4)
                let angle =
                    let x = Math.Abs(float (getXFrom p2) - (getXFrom p1))
                    let y = Math.Abs(float (getYFrom p2) - (getYFrom p1))
                    Math.Round((Math.Atan2(y,x) * 180.)/Math.PI,4)
                "\"Joints \\n" 
                + p1.x.ToString() + ", "+ p1.y.ToString() 
                + "\\n" 
                + p2.x.ToString() + ", "+ p2.y.ToString()  
                + "\\nLength = " 
                + length.ToString()
                + "\\nAngle = " 
                + angle.ToString() + "\[Degree]"
                + "\""
            | None, None, Some s -> 
                match s.Head with
                | Pin p -> 
                    let j = p.normal.joint
                    "\"Joint \\n"
                    + j.x.ToString() + ", "+ j.y.ToString()
                    + "\""
                | Roller r -> 
                    let j = r.joint
                    "\"Joint \\n"
                    + j.x.ToString() + ", "+ j.y.ToString()
                    + "\""
            | _ -> "\"--Ready--\""
        | TrussAnalysisDomain.ErrorState es -> "\"--Ready--\""
        | TrussAnalysisDomain.AnalysisState a -> 
            match a.analysis with
            | Truss -> getTrussCheck state
            | SupportReactionResult sr -> getTrussCheck state
            | SupportReactionEquations sr -> getTrussCheck state
            | MethodOfJointsCalculation mjc -> getTrussCheck state
            | MethodOfJointsAnalysis mja -> 
                let truss = getTrussFromState state
                let forces = 
                    match truss.forces with
                    | [] -> "none \\n"
                    | _ -> List.mapi (fun i f ->                             
                            "F" + i.ToString() + " = " + f.magnitude.ToString() + " \\n"
                            ) truss.forces |> List.fold (fun acc x -> acc + x ) "" 
                let reactions = 
                    List.map (fun x ->                        
                        let i = List.findIndex (fun s -> s = x.support) truss.supports
                        let supportType = match truss.supports.[i] with | Pin p -> "Pin" | Roller r -> "Roller"
                        match x.xReactionForce, x.yReactionForce with
                        | None, None -> ""
                        | Some x, None -> supportType + i.ToString() + " X = " + x.magnitude.ToString() + " \\n"
                        | None, Some y -> supportType + i.ToString() + " Y = " + y.magnitude.ToString() + " \\n"
                        | Some x, Some y -> supportType + i.ToString() + " X = " + x.magnitude.ToString() + " \\n" + supportType + i.ToString() + " Y = " + y.magnitude.ToString() + " \\n"
                        ) mja.reactions |> List.fold (fun acc x -> acc + x ) ""                    
                let zeroForceMeners = 
                    match mja.zeroForceMembers with
                    | [] -> None
                    | _ -> List.map (fun p -> 
                            let m = match p with | Member m -> Some m | _ -> None                            
                            let i = 
                                match m with 
                                | Some m -> getMemberIndex m truss
                                | None -> -1
                            "M" + i.ToString() + " = 0.0 \\n"
                            ) mja.zeroForceMembers |> List.fold (fun acc x -> acc + x ) "" |> Some
                let tensionForceMeners = 
                    match mja.tensionMembers with
                    | [] -> None
                    | _ -> List.map (fun (f,p) -> 
                            let m = match p with | Member m -> Some m | _ -> None                            
                            let i = 
                                match m with 
                                | Some m -> getMemberIndex m truss
                                | None -> -1
                            "M" + i.ToString() + " = " + f.ToString() + " Tension \\n"
                            ) mja.tensionMembers |> List.fold (fun acc x -> acc + x ) "" |> Some
                let compressionForceMeners = 
                    match mja.compressionMembers with
                    | [] -> None
                    | _ -> List.map (fun (f,p) -> 
                            let m = match p with | Member m -> Some m | _ -> None                            
                            let f' = -f
                            let i = 
                                match m with 
                                | Some m -> getMemberIndex m truss
                                | None -> -1
                            "M" + i.ToString() + " = " + f'.ToString() + " Compression \\n"
                            ) mja.compressionMembers |> List.fold (fun acc x -> acc + x ) "" |> Some
                let forcesHeading = "\"--Forces-- \\n"
                let reactionHeading = "--Reactions-- \\n"
                let memberHeading = "--Member Forces-- \\n"                
                match tensionForceMeners, compressionForceMeners, zeroForceMeners with
                | Some t, Some c, Some z -> forcesHeading + forces + reactionHeading + reactions + memberHeading + t + c + z + "\""
                | Some t, Some c, None   -> forcesHeading + forces + reactionHeading + reactions + memberHeading + t + c + "\""
                | None,   Some c, Some z -> forcesHeading + forces + reactionHeading + reactions + memberHeading + c + z + "\"" 
                | Some t, None,   Some z -> forcesHeading + forces + reactionHeading + reactions + memberHeading + t + z + "\""
                | Some t, None,   None   -> forcesHeading + forces + reactionHeading + reactions + memberHeading + t + "\""
                | None,   Some c, None   -> forcesHeading + forces + reactionHeading + reactions + memberHeading + c + "\""                
                | None,   None,   Some z -> forcesHeading + forces + reactionHeading + reactions + memberHeading + z + "\""                 
                | None,   None,   None   -> ""
            
    let createServices () = 
       {checkSupportTypeIsRoller = checkSupportTypeIsRoller;
        checkTruss = checkTruss;
        getJointSeqFromTruss = getJointSeqFromTruss;
        getMemberSeqFromTruss = getMemberSeqFromTruss;
        getPointFromMemberBuilder = getPointFromMemberBuilder;
        getTrussFromState = getTrussFromState;
        getPointFromForceBuilder = getPointFromForceBuilder;
        getPointFromForce = getPointFromForce;
        getDirectionFromForce = getDirectionFromForce;
        getPointFromSupport = getPointFromSupport;
        getDirectionFromSupport = getDirectionFromSupport;
        getSelectedMemberFromState = getSelectedMemberFromState;
        getSelectedForceFromState = getSelectedForceFromState;
        getSelectedSupportFromState = getSelectedSupportFromState;
        sendPointToMemberBuilder = sendPointToMemberBuilder;
        sendPointToForceBuilder = sendPointToForceBuilder;
        sendMagnitudeToForceBuilder = sendMagnitudeToForceBuilder;
        sendPointToRollerSupportBuilder = sendPointToRollerSupportBuilder;
        sendPointToPinSupportBuilder = sendPointToPinSupportBuilder;
        sendMagnitudeToSupportBuilder = sendMagnitudeToSupportBuilder;
        sendMemberToState = sendMemberToState;
        sendForceToState = sendForceToState;
        sendSupportToState = sendSupportToState;
        setSelectionMode = setSelectionMode;
        setTrussMode = setTrussMode;
        removeTrussPartFromTruss = removeTrussPart;
        getSupportReactionEquationsFromState = getSupportReactionEquationsFromState;
        getSupportReactionSolve = getSupportReactionSolve;
        sendStateToSupportBuilder = sendStateToSupportBuilder;
        analyzeEquations = analyzeEquations;
        getReactionForcesFromState = getReactionForcesFromState;
        sendPointToCalculation = sendPointToCalculation;
        getMemberOptionFromTrussPart = getMemberOptionFromTrussPart;
        getAnalysisReport = getAnalysisReport;
        getSupportIndexAtJoint = getSupportIndexAtJoint;
        getMemberIndex = getMemberIndex;
        getForceIndex = getForceIndex;
        getSupportIndex = getSupportIndex;
        sendPointToModification = sendPointToModification;
        modifyTrussForce = modifyForce;
        modifyTrussSupport = modifySupport
        }

