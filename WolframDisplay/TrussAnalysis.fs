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
->  Task 2 - UI controls
->  Task 3 - Develop Wolfram Language Stucture and Interactions
->  Task 4 - Continuous Development of features
*)

module WolframServices =
    let test = "Solve[{0*Ry0 + 3*Ry1 + -150 == 0,-3*Ry0 + 0*Ry1 + -105 == 0,-1*Rx0 + 0*Rx1 + 0 == 0,-1*Ry0 + -1*Ry1 + 15 == 0}, { Ry0, Ry1,Rx1,Rx0}]"
    let testResult = "{{Ry0 -> -13.2124, Rx1 -> 70.7107, Ry1 -> 100.561}}"

    // create helper functions to assemble Wolfram code
    
    let floatToString (f:float) = 
        let f' = f.ToString()
        match String.exists (fun x -> x = '.') f' with
        | true -> f'
        | false -> f' + ".0"

    let createEquation (constant:float) (variables:(float*string) list) = 
        let variableExpressions = List.fold (fun acc x-> acc + floatToString(fst x) + "*" + (snd x) + " + ") "" variables
        variableExpressions + (floatToString constant) + " == 0.0"
    
    let solveEquations (eq:string list) (v:string list) = 
        let eq' = List.filter (fun x -> x<>"") eq
        "DecimalForm[Solve[{" + 
        (List.fold (fun acc x-> match acc with | "" -> x | _ -> acc + "," + x) "" eq') + "},{" +
        (List.fold (fun acc x-> match acc with | "" -> x | _ -> acc + "," + x) "" v) + "}]]"

    // create helper functions to parse Wolfram code

    let parseResults (r:string) = 
        let r' = r |> String.filter (fun c -> (c = '{') = false) |> String.filter (fun c -> (c = '}') = false)
        let result = 
            match r with 
            | "{}" -> ["Unable to compute result"]
            |  "" -> ["Unable to compute result"]
            |  _ -> Array.toList (r'.Split(','))
        let parseResult (r:string) =
            let r' = r.Trim()
            let index = 
                match r.Contains("->") with
                | false -> false,-1
                | true -> 
                    match r.Contains("M")  with
                    | true -> r'.Substring(1,r.IndexOf(" ->")-1) |> System.Int32.TryParse
                    | false -> r'.Substring(2,r.IndexOf(" ->")-2) |> System.Int32.TryParse
            let part = 
                match r.Contains("->") with
                | false -> "Unable to compute result"
                | true -> 
                    match r.Contains("M") with
                    | true -> 
                        let out = r'.Substring(0,1)
                        match out.Contains("M") with
                        | true -> "Unable to compute result"
                        | false -> out
                    | false -> r'.Substring(0,2)
            let magnitude = 
                match r.Contains("->") with
                | false -> false,0.0
                | true -> r'.Substring(r.LastIndexOf(">")+1) |> System.Double.TryParse
            match r' with 
            | "Unable to compute result" -> (-1,"Unable to compute result",0.0)
            | _ -> (snd index),part,(snd magnitude)
        List.map parseResult result

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
    type ComponentForces = {joint:Joint; magnitudeX:float; magnitudeY:float}
    type Force = {magnitude:float; direction:Vector; joint:Joint}    
    type SupportBuilder = | Roller of ForceBuilder | Pin of (ForceBuilder*ForceBuilder)   
    type Pin = {tangent:Force;normal:Force}
    type Support = | Roller of Force | Pin of Pin 
    type Truss = {members:Member list; forces:Force list; supports:Support list}
    type TrussStability = 
        | Stable 
        | NotEnoughReactions 
        | ReactionsAreParallel 
        | ReactionsAreConcurrent 
        | InternalCollapseMechanism
    type TrussDeterminacy = 
        | Determinate 
        | Indeterminate
    type TrussPart = // A joint by itself is not a part, rather it is a cosequence of connecting two (or more) members
        | Member of Member
        | Force of Force
        | Support of Support
    type MemberForce = (float*TrussPart)
    type TrussBuildOp =
        | BuildMember of MemberBuilder
        | BuildForce of ForceBuilder
        | BuildSupport of SupportBuilder
    type BuildOpResult =
        | TrussPart of TrussPart
        | TrussBuildOp of TrussBuildOp
    type TrussMode =
        | Settings
        | Selection
        | Analysis
        | MemberBuild
        | ForceBuild
        | SupportBuild
    type TrussSelectionMode =
        | Delete
        | Modify
        | Inspect  
    type Node = (Joint*TrussPart list)

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
    type SelectionStateData = 
        {truss:Truss; 
         members:Member list option; 
         forces:Force list option; 
         supports:Support list option; 
         mode:TrussSelectionMode}    
    type ErrorStateData = {errors : Error list; truss : Truss}
    type SupportReactionResult = 
        {support: Support;
         xReactionForce: Force option;
         yReactionForce: Force option}    
    
    // Data associated with each analysis state
    type SupportReactionEquationStateData = 
        {momentEquations: string list;
         forceXEquation: string
         forceYEquation: string} 
    
    type SupportReactionResultStateData = {reactions : SupportReactionResult list}

    type MethodOfJointsCalculationStateData = 
        {solvedMembers: MemberForce list;
         memberEquations : string list;
         nodes : Node list;
         reactions : SupportReactionResult list;
         variables : string list}

    type MethodOfJointsAnalysisStateData = 
        {zeroForceMembers: TrussPart list;
         tensionMembers: MemberForce list;
         compressionMembers: MemberForce list;
         reactions : SupportReactionResult list}
    
    // Analysis States
    type AnalysisState =
        | Truss
        | SupportReactionEquations of SupportReactionEquationStateData
        | SupportReactionResult of SupportReactionResultStateData
        | MethodOfJointsCalculation of MethodOfJointsCalculationStateData
        | MethodOfJointsAnalysis of MethodOfJointsAnalysisStateData 
    
    type AnalysisStateData = 
        {stability:TrussStability list; 
         determinancy:TrussDeterminacy;
         analysis:AnalysisState;
         truss:Truss}
    
    // States
    type TrussAnalysisState =
        | TrussState of TrussStateData
        | BuildState of TrussBuildData
        | SelectionState of SelectionStateData
        | AnalysisState  of AnalysisStateData
        | ErrorState of ErrorStateData
    
    // Services
    type CheckSupportTypeIsRoller = Support -> bool
    type CheckTruss = Truss -> TrussAnalysisState

    type GetJointSeqFromTruss = Truss -> System.Windows.Point seq
    type GetMemberSeqFromTruss = Truss -> (System.Windows.Point * System.Windows.Point) seq
    type GetPointFromMemberBuilder = MemberBuilder -> System.Windows.Point
    type GetTrussFromState = TrussAnalysisState -> Truss
    type GetPointFromForceBuilder = ForceBuilder -> System.Windows.Point
    type GetPointFromForce = Force -> System.Windows.Point
    type GetDirectionFromForce = Force -> float
    type GetPointFromSupport = Support -> System.Windows.Point
    type GetDirectionFromSupport = Support -> float    
    type GetSelectedMemberFromState = TrussAnalysisState -> (System.Windows.Point * System.Windows.Point) option
    type GetSelectedForceFromState = TrussAnalysisState -> (System.Windows.Point * System.Windows.Point) option
    type GetSelectedSupportFromState = TrussAnalysisState -> (System.Windows.Point * float * bool) option
    type GetSupportReactionEquationsFromState = bool -> TrussAnalysisState -> TrussAnalysisState
    type GetSupportReactionSolve = bool -> TrussAnalysisState -> string
    type GetReactionForcesFromState = bool -> TrussAnalysisState -> Force list
    type GetMemberOptionFromTrussPart = TrussPart -> (System.Windows.Point*System.Windows.Point) Option
    type GetAnalysisReport = TrussAnalysisState -> string
    type GetSupportIndexAtJoint = Joint -> Support list -> int*string
    type GetMemberIndex = Member -> Truss -> int
    type GetForceIndex = Force -> Truss -> int
    type GetSupportIndex = Support -> Truss -> int*string
    
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
         getSupportIndex:GetSupportIndex
         }

module TrussImplementation = 
    open TrussDomain
    open WolframServices
    
    let getYFrom (j:Joint) = match j.y with | Y y -> y
    let getXFrom (j:Joint) = match j.x with | X x -> x
    let getReactionForcesFrom (s: Support list) = 
        List.fold (fun acc r -> match r with | Roller f -> f::acc  | Pin p -> p.tangent::p.normal::acc) [] s
    
    let getMembersAt (j:Joint) (m:Member list) = 
        List.choose (fun (p1,p2) -> match p1 = j || p2 = j with | true -> Some (Member (p1,p2)) | false -> None) m
    let getForcesAt (j:Joint) (f:Force list) = 
        List.choose (fun force -> 
            match force.joint = j with 
            | true ->  Some (Force force)
            | false -> None) f
    let getSupportsAt (j:Joint) (s:Support list) = 
        List.choose (fun spt -> 
            match spt with 
            | Roller f when f.joint = j -> Some (Support spt) 
            | Pin p when p.normal.joint = j -> Some (Support spt)
            | _ -> None) s
        
    let getJointListFrom (m: Member list) = 
        let (l1,l2) = List.unzip m
        List.concat [l1;l2] |> List.distinct    
    let getJointPartListFrom (t:Truss) =
        let joints = getJointListFrom t.members
        let jointPartsTo j =
            j,List.concat [(getMembersAt j t.members);(getForcesAt j t.forces);(getSupportsAt j t.supports)]
        List.map jointPartsTo joints
    let getPartListFrom (t:Truss) =
        getJointPartListFrom t
        |> List.map (fun (_p,l) -> l)
        |> List.concat 
        |> List.distinct

    let getDirectionsFrom (f:Force list) = List.map (fun x -> 
        Vector(
            x = x.direction.X - (getXFrom x.joint), 
            y = x.direction.Y - (getYFrom x.joint))) f
    let getLineOfActionFrom (f:Force) = 
        match (f.direction.X - (getXFrom f.joint)) = 0. with 
        | false -> 
            let m = (f.direction.Y - (getYFrom f.joint)) / (f.direction.X - (getXFrom f.joint))                   
            let b = (getYFrom f.joint) - (m * (getXFrom f.joint))
            (m,b)
        | true -> (Double.PositiveInfinity,0.)
    let getMemberLineOfActionFrom (m:Member) = 
        let j1,j2 = m
        match ((getXFrom j2) - (getXFrom j1)) = 0. with 
        | false -> 
            let m = ((getYFrom j2) - (getYFrom j1)) / ((getXFrom j2) - (getXFrom j1))                   
            let b = (getYFrom j1) - (m * (getXFrom j1))
            (m,b)
        | true -> (1.,0.)
    
    let getComponentForcesFrom (f:Force) =
        let x = f.direction.X - (getXFrom f.joint)
        let y = f.direction.Y - (getYFrom f.joint)
        let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n 
        {joint = f.joint; magnitudeX = -f.magnitude*(x/d); magnitudeY = f.magnitude*(y/d)}
    let getResultantForcesFrom (r:SupportReactionResult) =        
        match r with 
        | {support = Roller r; xReactionForce = Some x; yReactionForce = None} -> 
            {magnitude = x.magnitude; direction = x.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = Some x; yReactionForce = None} -> 
            {magnitude = x.magnitude; direction = x.direction ; joint = p.normal.joint}        
        | {support = Roller r; xReactionForce = None; yReactionForce = Some y} -> 
            {magnitude = y.magnitude; direction = y.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = None; yReactionForce = Some y} -> 
            {magnitude = y.magnitude; direction = p.normal.direction ; joint = p.normal.joint}        
        | {support = Roller r; xReactionForce = Some x; yReactionForce = Some y} ->             
            let magXY = 
                let out = Math.Sqrt(x.magnitude*x.magnitude + y.magnitude*y.magnitude)
                match out with 
                | x when x = 0. -> 1.
                | x -> x
            let direction = 
                let x' = x.direction.X - (getXFrom x.joint)
                let y' = y.direction.Y - (getYFrom y.joint)
                Vector(x = ((getXFrom x.joint) + x'*(x.magnitude/magXY)), y = ((getYFrom y.joint) + y'*(y.magnitude/magXY)))
            {magnitude = magXY; direction = direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = Some x; yReactionForce = Some y} -> 

            let magXY = 
                let out = Math.Sqrt(x.magnitude*x.magnitude + y.magnitude*y.magnitude)
                match out with 
                | x when x = 0. -> 1.
                | x -> x
            let direction = 
                let x' = x.direction.X - (getXFrom x.joint)
                let y' = y.direction.Y - (getYFrom y.joint)
                Vector(x = ((getXFrom x.joint) + x'*(x.magnitude/magXY)), y = ((getYFrom y.joint) + y'*(y.magnitude/magXY)))                
                

            {magnitude = magXY; direction = direction ; joint = p.normal.joint}
        | {support = Roller r; xReactionForce = None; yReactionForce = None} -> 
            {magnitude = r.magnitude; direction = r.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = None; yReactionForce = None} -> 
            {magnitude = 0.0; direction = p.normal.direction ; joint = p.normal.joint}
       
    let getSupportComponentForcesFrom (s:Support) = // need to refactor this function
        match s with
        | Pin p ->              
            let x = p.tangent.direction.X - (getXFrom p.tangent.joint)
            let y = p.normal.direction.Y - (getYFrom p.normal.joint)
            let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n 
            {joint = p.tangent.joint; magnitudeX = p.tangent.magnitude*(x/d); magnitudeY = p.normal.magnitude*(y/d)}
        | Roller r -> 
            let x = r.direction.X - (getXFrom r.joint)
            let y = r.direction.Y - (getYFrom r.joint)
            let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n
            {joint = r.joint; magnitudeX = r.magnitude*(x/d); magnitudeY = r.magnitude*(y/d)}

    let getJointFromSupportBuilder (sb:SupportBuilder) = 
        match sb with
        | SupportBuilder.Roller r -> r.joint
        | SupportBuilder.Pin (p,_) -> p.joint
    let getJointFromSupport (s:Support) = match s with | Pin p -> p.normal.joint | Roller r -> r.joint
            
    let sumForcesX (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        List.fold (fun acc x -> x.magnitudeX + acc ) 0. forces
    let sumForcesY (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        List.fold (fun acc y -> y.magnitudeY + acc ) 0. forces
    let sumForceMoments (s:Support) (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        let j = getJointFromSupport s
        let getMomentArmY fj = (getYFrom fj - getYFrom j)
        let getMomentArmX fj = (getXFrom fj - getXFrom j)
        List.fold (fun acc cf -> cf.magnitudeX*(getMomentArmY cf.joint) + cf.magnitudeY*(getMomentArmX cf.joint) + acc ) 0. forces
    
    // Support Reaction Equations
    let getXForceReactionEquation (p:TrussPart list) =         
        let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) p        
        let reactions = List.mapi (fun i x -> 
            match x with 
            | Pin p -> 1.,"Rx" + i.ToString() // Pins always have an x and y component regardless of orientation.
            | Roller r -> 
                let x = r.direction.X - (getXFrom r.joint)
                let y = r.direction.Y - (getYFrom r.joint)
                let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n
                Math.Abs x/d,"Rx" + i.ToString()) supports 
        createEquation (sumForcesX p) reactions
    let getYForceReactionEquation (p:TrussPart list) =   
        let supports = List.choose (fun y -> match y with | Support s -> Some s | _ -> None) p        
        let reactions = List.mapi (fun i y -> 
            match y with 
            | Pin p -> 1.,"Ry" + i.ToString() // Pins always have an x and y component regardless of orientation.
            | Roller r -> 
                let x = r.direction.X - (getXFrom r.joint)
                let y = r.direction.Y - (getYFrom r.joint)
                let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n
                y/d,"Ry" + i.ToString()) supports 
        createEquation (sumForcesY p) reactions
    let addSupportXY (p:TrussPart list) =
        let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) p        
        match supports with
        | [] -> false
        | Pin _p :: Roller r :: [] 
        | Roller r :: Pin _p :: [] -> 
            let m,_b = getLineOfActionFrom r           
            match m = 0. || m = Double.PositiveInfinity with | true -> false | false -> true
        | Pin p1 :: Pin p2 :: [] -> true
        | _ -> true
    let supportXY rSting add (p:TrussPart list) =   
        let supports = List.choose (fun y -> match y with | Support s -> Some s | _ -> None) p 
        match supports with                                
        | [] -> []
        | Pin p :: Roller r :: [] 
        | Roller r :: Pin p :: [] ->
            let x = match r.direction.X - (getXFrom r.joint) with | n when n = 0. -> 1. | n-> n
            let y = match r.direction.Y - (getYFrom r.joint) with | n when n = 0. -> 1. | n-> n
            let d = Math.Sqrt (x*x + y*y)
            let i = List.findIndex (fun x -> x = (Roller r)) supports                
            match add with 
            | true ->  [createEquation (0.) [(d/y,"Ry" + i.ToString());(-d/x,"Rx" + i.ToString())]] 
            | false -> []
        | Pin p1 :: Pin p2 :: [] ->         
            let i1,i2 = List.findIndex (fun x -> x = (Pin p1)) supports,  List.findIndex (fun x -> x = (Pin p2)) supports
            match add with 
            | true ->  [createEquation (0.) [(1., rSting + i1.ToString());(-1., rSting + i2.ToString())]] 
            | false -> [] 
        | _ -> []
    let getYMomentReactionEquations (p:TrussPart list) =         
        let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) p        
        let addRx = addSupportXY p
        let rollerXY = supportXY "Rx" addRx p
        let getSupportMoments (s:Support) = 
            let j = getJointFromSupport s
            let getMomentArmX sj = getXFrom sj - getXFrom j
            let getMomentArmY sj = getYFrom sj - getYFrom j
            let ly = List.mapi (fun i s -> (getJointFromSupport s |> getMomentArmX),"Ry" + i.ToString()) supports
            let lx = 
                match s with 
                | Pin _ ->
                    match addRx with 
                    | false -> []
                    | true -> List.mapi (fun i s -> (getJointFromSupport s |> getMomentArmY),"Rx" + i.ToString()) supports
                | Roller _ -> List.mapi (fun i s -> (getJointFromSupport s |> getMomentArmY),"Rx" + i.ToString()) supports
            List.concat [ly;lx]
        let momentEquations = List.map (fun y -> createEquation (sumForceMoments y p) (getSupportMoments y)) supports        
        List.concat [momentEquations; rollerXY] 
    let getXMomentReactionEquations (p:TrussPart list) =         
        let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) p
        // This function only gets the first roller. TODO apply to all rollers in an arbitrary truss        
        let addRy = addSupportXY p
        let getSupportMoments (s:Support) = 
            let j = getJointFromSupport s
            let getMomentArmX sj = getXFrom sj - getXFrom j
            let getMomentArmY sj = getYFrom sj - getYFrom j            
            let lx = List.mapi (fun i y -> (getJointFromSupport y |> getMomentArmY),"Rx" + i.ToString()) supports
            let ly = 
                match s with 
                | Pin _ ->
                    match addRy with 
                    | false -> []
                    | true -> List.mapi (fun i s -> (getJointFromSupport s |> getMomentArmX),"Ry" + i.ToString()) supports 
                | Roller _ -> List.mapi (fun i s -> (getJointFromSupport s |> getMomentArmX),"Ry" + i.ToString()) supports
            List.concat [ly;lx]        
        let momentEquations = List.map (fun x -> createEquation (sumForceMoments x p) (getSupportMoments x)) supports
        let rollerXY = supportXY "Ry" addRy p
        List.concat [momentEquations; rollerXY]
    

    // Basic operations on truss
    let addTrussPartToTruss (t:Truss) (p:TrussPart)  = 
        match p with 
        | Member m -> {t with members = m::t.members}
        | Force f -> {t with forces = f::t.forces}
        | Support s -> {t with supports = s::t.supports}    
    let removeTrussPartFromTruss (t:Truss) (p:TrussPart option)  = 
        match p with 
        | Some (Member m) -> 
            let mOut = List.except [m] t.members
            {t with members = mOut}        
        | Some (Force f) -> 
            let fOut = List.except [f] t.forces
            {t with forces = fOut}
        | Some (Support s) -> 
            let sOut = List.except [s] t.supports
            {t with supports = sOut}
        | None -> t
    let getMemberIndex (m:Member) (t:Truss) =
        let i = List.tryFindIndex (fun x -> x = m) t.members
        match i with | Some n -> n | None -> -1    
    let getForceIndex (f:Force) (t:Truss) =
        let i = List.tryFindIndex (fun x -> x = f) t.forces
        match i with | Some n -> n | None -> -1
    let getSupportIndex (s:Support) (t:Truss) =
        let i = match List.tryFindIndex (fun x -> x = s) t.supports with | Some n -> n | None -> -1
        match s with
        | Pin _ -> i,"Pin"
        | Roller _ -> i,"Roller"
    // Method of Joints
    let isColinear (p1:TrussPart) (p2:TrussPart) = 
        match p1, p2 with
        | Member m1, Member m2 -> (getMemberLineOfActionFrom m1) = (getMemberLineOfActionFrom m2)
        | Member m1, Force f 
        | Force f, Member m1 -> (getMemberLineOfActionFrom m1) = (getLineOfActionFrom f)
        (*| Support (Roller r), Member m1 
        | Member m1,Support (Roller r) -> (getMemberLineOfActionFrom m1) = (getLineOfActionFrom r)
        | Support (Pin p), Member m1 
        | Member m1,Support (Pin (p)) -> (getMemberLineOfActionFrom m1) = (getLineOfActionFrom p.normal)*) 
        | _ -> false
    let getZFM (tpl:TrussPart list) = 
        match tpl with 
        // case 1 -- no load, 2 non-colinear members, both members are zero force
        | [Member m1;Member m2] when (isColinear (Member m1) (Member m2)) = false -> [Member m1;Member m2]                     
        // case 2 -- no load, 3 members, 2 colinear members, non-colinear member is zero force        
        | [Member m1;Member m2;Member m3] when (isColinear (Member m1) (Member m2)) && ((isColinear (Member m1) (Member m3)) = false) -> [Member m3]
        | [Member m1;Member m2;Member m3] when (isColinear (Member m1) (Member m3)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m2]
        | [Member m1;Member m2;Member m3] when (isColinear (Member m3) (Member m2)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m1]
        // case 3 -- applied load colinear with 1 of 2 members, non-colinear member is zero force
        | [Member m1;Member m2;Force f]
        | [Member m1;Force f;Member m2]
        | [Force f;Member m1;Member m2] when (isColinear (Member m1) (Force f)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m2]            
        | [Member m1;Member m2;Force f] 
        | [Member m1;Force f;Member m2]
        | [Force f;Member m1;Member m2] when (isColinear (Member m2) (Force f)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m1]
        
        | [Member m1;Member m2;Support s]
        | [Member m1;Support s;Member m2]
        | [Support s;Member m1;Member m2] when (isColinear (Member m1) (Support s)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m2]            
        | [Member m1;Member m2;Support s] 
        | [Member m1;Support s;Member m2]
        | [Support s;Member m1;Member m2] when (isColinear (Member m2) (Support s)) && ((isColinear (Member m1) (Member m2)) = false) -> [Member m1]
        | _ -> []     
    let partitionNode (tpl:Node list) = 
        List.map (fun (j,pl) -> 
            let zfm = getZFM pl
            let pl' = List.except zfm pl
            let rec recurse pl zfm = 
                match getZFM pl with
                | [] -> j,pl',zfm
                | x -> 
                    let zfm' = (List.concat [zfm; x])
                    let pl'' = List.except zfm' pl'
                    recurse pl'' zfm' 
            recurse pl' zfm) tpl
    let getZeroForceMembers (t:Truss) =
        let jointParts = getJointPartListFrom t        
        let partition = partitionNode jointParts
        List.fold (fun acc x -> match x with | (_j,_pl,zfm) -> List.concat[zfm;acc]) [] partition 
    let getNodeList (t:Truss) =
        let jointParts = getJointPartListFrom t        
        let partition = partitionNode jointParts
        let zfm = getZeroForceMembers t
        List.fold (fun acc x -> match x with | (j,pl,_zfm) -> Node (j,List.except zfm pl)::acc) [] partition    
    
    let getMemberVariables (t:Truss) =
        let nl = getNodeList t
        let processNode (n:Node) = 
            let _j,pl = n            
            let members = 
                List.choose (fun x -> match x with | Member m -> Some m | _ -> None) pl
                |> List.map (fun x -> "M" + (getMemberIndex x t).ToString())            
            List.concat [members] 
        List.map (fun x -> processNode x) nl
        |> List.concat
        |> List.distinct
    let getReactionSignX (f:Force) = 
        let x = (getXFrom f.joint) - f.direction.X           
        match x < 0. with
        | true  -> -1.
        | false -> 1.
    let getReactionSignY (f:Force) =                 
        let y = (getYFrom f.joint) - f.direction.Y 
        match y < 0. with
        | true -> 1.
        | false -> -1.
    let getMemberExpressionsX (j:Joint) (m:Member) (index:int) =
        let p1,p2 = m
        let j' = match p1 = j with | true -> p2 | false -> p1
        let x = (getXFrom j') - (getXFrom j)         
        let y = (getYFrom j') - (getYFrom j)
        let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n        
        let name = "M" + index.ToString()
        ( x/d,name) //Math.Abs
    let getMemberExpressionsY (j:Joint) (m:Member) (index:int) =
        let p1,p2 = m
        let j' = match p1 = j with | true -> p2 | false -> p1
        let x = (getXFrom j') - (getXFrom j)
        let y = (getYFrom j) - (getYFrom j')        
        let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 0. | n -> n         
        let name = "M" + index.ToString()
        ( y/d,name) //Math.Abs
    let analyzeNode (n:Node) (t:Truss) (r:SupportReactionResult list) = 
        let j,pl = n
        let supportReactions = 
            let s = List.tryFind (fun x -> match x with | Support _ -> true | _ -> false) pl
            match s with
            | Some (Support spt) -> List.tryFind (fun x -> x.support = spt) r
            | _ -> None
        let sfx, sfy = 
            match supportReactions with 
            | None -> 0.,0. 
            | Some {support=_spt; xReactionForce = Some xf; yReactionForce = Some yf} -> (getReactionSignX xf)*xf.magnitude, (getReactionSignY yf)*yf.magnitude
            | Some {support=_spt; xReactionForce = None; yReactionForce = Some yf} -> 0., (getReactionSignY yf)*yf.magnitude
            | Some {support=_spt; xReactionForce = Some xf; yReactionForce = None} -> (getReactionSignX xf)*xf.magnitude,0.
            | Some {support=_spt; xReactionForce = None; yReactionForce = None} -> 0.,0.
        let sumfx, sumfy = (sumForcesX pl) + sfx, (sumForcesY pl) + sfy
        let membersX = 
            List.choose (fun x -> match x with | Member m -> Some m | _ -> None) pl
            |> List.map (fun x -> getMemberExpressionsX j x (getMemberIndex x t))
        let membersY = 
            List.choose (fun y -> match y with | Member m -> Some m | _ -> None) pl
            |> List.map (fun y -> getMemberExpressionsY j y (getMemberIndex y t))
        [createEquation sumfx membersX;createEquation sumfy membersY]
    let getMemberForceAtJoint (j:Joint) (m:TrussPart) (mf:MemberForce list) = 
        let tf = List.tryFind (fun (_f,p) -> p = m ) mf 
        match tf with 
        | None -> m
        | Some (f,Member p) when Member p = m->  
            let p1,p2 = p
            let dir = 
                match j = p1 with 
                | true ->  Vector(X = getXFrom p2, Y  = getYFrom p2)
                | false -> Vector(X = getXFrom p1, Y = getYFrom p1)
            {magnitude = -f; 
             direction = dir; 
             joint = j} |> Force
        | Some _ -> m

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
            | true -> {tangent = {magnitude = f1._magnitude.Value; direction = v1; joint = f1.joint};
                       normal = {magnitude = f2._magnitude.Value; direction = v2.Value; joint = f2.joint}} |> Pin |> Support |> TrussPart
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
            | true -> {tangent = {magnitude = m1; direction = f1._direction.Value; joint = f1.joint};
                       normal = {magnitude = m2.Value; direction = f2._direction.Value; joint = f2.joint}} |> Pin |> Support |> TrussPart

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
        | TrussDomain.TrussState ts -> ts.truss
        | TrussDomain.BuildState bs-> bs.truss
        | TrussDomain.SelectionState ss -> ss.truss
        | TrussDomain.AnalysisState s -> s.truss
        | TrussDomain.ErrorState es -> es.truss
    let getSelectedMemberFromState (state :TrussAnalysisState) = 
        match state with
        | TrussDomain.TrussState ts -> None
        | TrussDomain.BuildState bs-> None
        | TrussDomain.SelectionState ss -> 
            match ss.members with
            | None -> None
            | Some m -> 
                let a,b = m.Head
                (System.Windows.Point (x = (getXFrom a),y = (getYFrom a)),
                 System.Windows.Point (x = (getXFrom b),y = (getYFrom b))) |> Some
        | TrussDomain.AnalysisState s -> None
        | TrussDomain.ErrorState es -> None
    let getSelectedForceFromState (state :TrussAnalysisState) = 
        match state with
        | TrussDomain.TrussState ts -> None
        | TrussDomain.BuildState bs-> None
        | TrussDomain.SelectionState ss -> 
            match ss.forces with
            | None -> None
            | Some f -> 
                let a,b = f.Head.joint,f.Head.direction
                (System.Windows.Point (x = (getXFrom a),y = (getYFrom a)),
                 System.Windows.Point (x = (b.X),y = (b.Y))) |> Some
        | TrussDomain.AnalysisState s -> None
        | TrussDomain.ErrorState es -> None
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
        | Pin p -> getPointFromForce p.normal
        | Roller f -> getPointFromForce f
    let getDirectionFromSupport (support:Support) = 
        match support with
        | Pin p -> -(getDirectionFromForce p.normal)
        | Roller f -> -(getDirectionFromForce f) 
    let getSelectedSupportFromState (state :TrussAnalysisState) = 
        match state with
        | TrussDomain.TrussState ts -> None
        | TrussDomain.BuildState bs-> None
        | TrussDomain.SelectionState ss -> 
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

        | TrussDomain.AnalysisState s -> None
        | TrussDomain.ErrorState es -> None
    let getSupportReactionEquationsFromState (yAxis: bool) (state :TrussAnalysisState) = 
        let truss = getTrussFromState state        
        let parts = getPartListFrom truss
        match yAxis with
        | true -> 
            match state with
            | TrussDomain.TrussState ts -> state
            | TrussDomain.BuildState bs-> state
            | TrussDomain.SelectionState ss -> state
            | TrussDomain.AnalysisState s -> 
                match List.contains Stable s.stability with
                | true ->                 
                    {s with analysis = 
                            {momentEquations = getYMomentReactionEquations parts;
                             forceXEquation = getXForceReactionEquation parts;
                             forceYEquation = getYForceReactionEquation parts} 
                             |> SupportReactionEquations} 
                             |> AnalysisState
                | false -> state
            | TrussDomain.ErrorState es -> state
        | false -> 
            match state with
            | TrussDomain.TrussState ts -> state
            | TrussDomain.BuildState bs-> state
            | TrussDomain.SelectionState ss -> state
            | TrussDomain.AnalysisState s -> 
                match List.contains Stable s.stability with
                | true ->                 
                    {s with analysis = 
                            {momentEquations = getXMomentReactionEquations parts;
                             forceXEquation = getXForceReactionEquation parts; 
                             forceYEquation = getYForceReactionEquation parts} 
                             |> SupportReactionEquations} 
                             |> AnalysisState
                | false -> state
            | TrussDomain.ErrorState es -> state
    let getReactionForcesFromState showComponents (state :TrussAnalysisState)  =         
        match state with
        | TrussDomain.TrussState ts -> []
        | TrussDomain.BuildState bs-> []
        | TrussDomain.SelectionState ss -> []
        | TrussDomain.AnalysisState s -> 
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
        | TrussDomain.ErrorState es -> []
    let getMemberOptionFromTrussPart p = 
        match p with
        | Member (j1,j2) -> Some (System.Windows.Point (getXFrom j1,(getYFrom j1)),System.Windows.Point (getXFrom j2,(getYFrom j2)))
        | _ -> None
    let getSupportIndexAtJoint (j:Joint) supports = 
        let i = 
            List.findIndex (fun (x:TrussDomain.Support) -> 
                match x with 
                | Pin p -> p.normal.joint = j
                | Roller r -> r.joint = j
                ) supports
        match supports.[i] with
        | Pin p -> i,"Pin"
        | Roller r -> i,"Roller"

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
                {truss = ts.truss; members = Some [m]; forces = None; supports = None; mode = sm} 
                |> SelectionState
            | Analysis -> state
            | MemberBuild -> state
            | ForceBuild -> state
            | SupportBuild -> state         
        | BuildState  bs -> state
        | SelectionState  ss -> 
            {truss = ss.truss; members = Some [m]; forces = None; supports = None; mode = sm} 
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
                
                {truss = ts.truss; members = None; forces = f; supports = None; mode = sm} 
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
            {truss = ss.truss; members = None; forces = f; supports = None; mode = sm} 
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
                {truss = ts.truss; members = None; forces = None; supports = s; mode = sm} 
                |> SelectionState
            | Analysis -> state
            | MemberBuild -> state
            | ForceBuild -> state
            | SupportBuild -> state         
        | BuildState  bs -> state
        | SelectionState  ss -> 
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
            {truss = ss.truss; members = None; forces = None; supports = s; mode = sm} 
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
            | TrussDomain.TrussState _ts -> state
            | TrussDomain.BuildState _bs-> state
            | TrussDomain.SelectionState _ss -> state
            | TrussDomain.ErrorState _es -> state
            | TrussDomain.AnalysisState a ->                     
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
                    { a with analysis = 
                        {solvedMembers = zeroMembers; 
                         memberEquations = memberEquations; 
                         nodes = nodes;
                         reactions = sr.reactions;
                         variables = variables} |> MethodOfJointsCalculation 
                    } |> TrussDomain.AnalysisState
                | MethodOfJointsCalculation mj ->                    
                    let selectedNode = List.tryFind (fun (j,_pl) -> (getXFrom j) = p.X && (getYFrom j) = p.Y) mj.nodes                    
                    let memberEquations =
                        match selectedNode with
                        | None -> []
                        | Some n -> analyzeNode n a.truss mj.reactions
                    { a with analysis = 
                        {solvedMembers = mj.solvedMembers; 
                         memberEquations = memberEquations; 
                         nodes = mj.nodes;
                         reactions = mj.reactions;
                         variables = mj.variables} |> MethodOfJointsCalculation 
                    } |> TrussDomain.AnalysisState
                | MethodOfJointsAnalysis _mj -> state

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
            | TrussDomain.TrussState _ts -> state
            | TrussDomain.BuildState _bs-> state
            | TrussDomain.SelectionState _ss -> state
            | TrussDomain.ErrorState _es -> state
            | TrussDomain.AnalysisState a -> 
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
                        |> List.map (fun x -> MemberForce x) 
                    let replaceMembersWithForces (n:Node) =
                        let (j,pl) = n
                        let memberCount pl' = List.filter (fun x -> match x with | Member _ -> true | _ -> false) pl' |> List.length
                        let newPl = 
                            let rec replace pl' (sMembers:MemberForce list) = 
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
                        } |> TrussDomain.AnalysisState
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
                         } |> TrussDomain.AnalysisState                                
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
        | TrussDomain.TrussState ts -> ""
        | TrussDomain.BuildState bs-> ""
        | TrussDomain.SelectionState ss -> ""
        | TrussDomain.ErrorState es -> ""
        | TrussDomain.AnalysisState a -> 
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
        | TrussDomain.TrussState ts -> "--Ready--"
        | TrussDomain.BuildState bs -> "--Ready--"
        | TrussDomain.SelectionState ss -> "--Ready--"                
        | TrussDomain.ErrorState es -> "--Ready--"
        | TrussDomain.AnalysisState a -> 
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
        | TrussDomain.TrussState ts -> "\"--Ready--\""
        | TrussDomain.BuildState bs-> "\"--Ready--\""
        | TrussDomain.SelectionState ss -> 
            match ss.forces, ss.members, ss.supports with
            | Some f, None, None -> 
                let p1,p2 = f.Head.joint, f.Head.direction                
                let angle =
                    let x = Math.Abs(p2.X - (getXFrom p1))
                    let y = Math.Abs(p2.Y - (getYFrom p1))
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
                    let x = Math.Abs((getXFrom p2) - (getXFrom p1))
                    let y = Math.Abs((getYFrom p2) - (getYFrom p1))
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
        | TrussDomain.ErrorState es -> "\"--Ready--\""
        | TrussDomain.AnalysisState a -> 
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
        getSupportIndex = getSupportIndex
        }

type TrussAnalysis() as this =  
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
    
    (*Truss Services*)
    let trussServices = TrussServices.createServices()
    
    (*Model*)        
    let image = Image()
    do  image.SetValue(Panel.ZIndexProperty, -100)          
    let visual = DrawingVisual()     
    let clear = SolidColorBrush(Colors.Transparent)
    let black = SolidColorBrush(Colors.Black) 
    let blue =  SolidColorBrush(Colors.Blue)
    let blue2 = SolidColorBrush(Colors.RoyalBlue)
    let olive = SolidColorBrush(Colors.Olive)
    let red =   SolidColorBrush(Colors.Red)
    let green = SolidColorBrush(Colors.Green)
    let bluePen, blueGridline, redPen, 
        redGridline, blackPen = 
            Pen(blue, 0.5), Pen(blue, 0.2), Pen(red, 0.5), 
            Pen(red, 0.1), Pen(black, 0.5) 
    do  bluePen.Freeze()
        redPen.Freeze()
        blackPen.Freeze()
        redGridline.Freeze()
        blueGridline.Freeze()
   
    (*Controls*)      
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
        l 
        // Analysis State
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
            sp.Visibility <- Visibility.Collapsed
        sp
        // Orgin and grid
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
            l.Text <- "200"
        l
    let xUp_Button = 
        let b = Button()
        let handleClick () = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let x' = x + 100.
            do xOrgin_TextBlock.Text <- x'.ToString()
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
            l.Text <- "500"
        l
    let yUp_Button = 
        let b = Button()
        let handleClick () = 
            let y = Double.Parse yOrgin_TextBlock.Text
            let y' = y + 100.
            do yOrgin_TextBlock.Text <- y'.ToString()
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
    let settings_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Settings",FontSize=15.)        
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
            sp.Children.Add(settings_RadioButton) |> ignore
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
        // Selection mode selection
    let selectionMode_Label =
        let l = TextBlock()
        do  l.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 20.            
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Selection Mode"
        l
    let delete_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Delete", FontSize=15.)        
        do  r.Content <- tb
            r.IsChecked <- true |> Nullable<bool>
        r
    let inspect_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Inspect", FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let modify_RadioButton = 
        let r = RadioButton()
        let tb = TextBlock(Text="Modify", FontSize=15.)            
        do  r.Content <- tb
            r.IsChecked <- false |> Nullable<bool>
        r
    let delete_Button = 
        let b = Button()
        let handleClick () = 
            let newState = trussServices.removeTrussPartFromTruss state            
            do  state <- newState 
                label.Text <- newState.ToString()
        do  b.Content <- "Delete"
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Bold
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleClick()))
        b
    let selectionMode_StackPanel = 
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 0., Right = 0., Bottom = 0.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(selectionMode_Label) |> ignore
            sp.Children.Add(delete_RadioButton) |> ignore
            sp.Children.Add(inspect_RadioButton) |> ignore
            sp.Children.Add(modify_RadioButton) |> ignore
            sp.Children.Add(delete_Button) |> ignore
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
        // Truss parts
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
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendMemberToState l TrussDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendMemberToState l TrussDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendMemberToState l TrussDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
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
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendForceToState l TrussDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendForceToState l TrussDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendForceToState l TrussDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
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
                match delete_RadioButton.IsChecked.Value, 
                      inspect_RadioButton.IsChecked.Value, 
                      modify_RadioButton.IsChecked.Value with
                | true,false,false -> trussServices.sendSupportToState p TrussDomain.TrussSelectionMode.Delete s
                | false,true,false -> trussServices.sendSupportToState p TrussDomain.TrussSelectionMode.Inspect s
                | false,false,true -> trussServices.sendSupportToState p TrussDomain.TrussSelectionMode.Modify s
                | _ -> s //add code to throw an error here.
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
        // Wolfram result 
    let result_Viewbox (image:UIElement) =         
        let vb = Viewbox()   
        do  image.Opacity <- 0.85            
            vb.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            vb.IsHitTestVisible <- false
            vb.Child <- image            
        vb
    let code_TextBlock = 
        let l = TextBox()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Ready"
            l.Visibility <- Visibility.Hidden
        l 
    let message_TextBlock = 
        let l = TextBlock()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 30.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- "Calculate Reactions"
            l.Visibility <- Visibility.Visible
            l.Background <- clear
        l
    let result_TextBlock = 
        let l = TextBox()
        do  //l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- ""
            l.Visibility <- Visibility.Hidden
        l 
    let result_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Margin <- Thickness(Left = 180., Top = 10., Right = 0., Bottom = 0.)
            sp.Visibility <- Visibility.Visible
        sp    
    let result_ScrollViewer = 
        let sv = new ScrollViewer();
        do  sv.VerticalScrollBarVisibility <- ScrollBarVisibility.Hidden 
            sv.MaxHeight <- 1000.
            sv.HorizontalScrollBarVisibility <- ScrollBarVisibility.Auto 
            sv.MaxWidth <-800.
            sv.Content <- result_StackPanel
            sv.SetValue(Canvas.ZIndexProperty,3) 
            sv.Visibility <- Visibility.Collapsed    
        sv

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

        // Settings
    let toggleCodeText_Button = 
        let b = Button() 
        let onOff() = 
            match b.Content.ToString() with 
            | "Code Text Off" -> 
                do  code_TextBlock.Visibility <- Visibility.Visible
                    b.Content <- "Code Text On" 
            | "Code Text On" -> 
                do  code_TextBlock.Visibility <- Visibility.Collapsed
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
                do  result_TextBlock.Visibility <- Visibility.Visible
                    b.Content <- "Result Text On" 
            | "Result Text On" -> 
                do  result_TextBlock.Visibility <- Visibility.Collapsed
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
        do  b.Content <- "State Text On" 
            b.FontSize <- 12.
            b.FontWeight <- FontWeights.Regular
            b.VerticalAlignment <- VerticalAlignment.Center
            b.Margin <- Thickness(Left = 0., Top = 5., Right = 5., Bottom = 0.)
            b.Visibility <- Visibility.Visible
            b.Click.AddHandler(RoutedEventHandler(fun _ _ -> onOff() ))
        b
    let settings_StackPanel = 
        let sp = StackPanel()
        do  sp.Orientation <- Orientation.Vertical
            sp.HorizontalAlignment <- HorizontalAlignment.Left
            sp.Margin <- Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.)
            sp.Visibility <- Visibility.Collapsed
            sp.Children.Add(toggleCodeText_Button) |> ignore
            sp.Children.Add(toggleResultText_Button) |> ignore
            sp.Children.Add(toggleStateText_Button) |> ignore
            sp.Children.Add(orgin_StackPanel) |> ignore

        sp  
        // Controls border
    let trussControls_Border = 
        let border = Border()            
        do  border.BorderBrush <- black
            border.Cursor <- System.Windows.Input.Cursors.Arrow
            border.Background <- clear
            border.Opacity <- 0.8
            border.IsHitTestVisible <- true
            border.BorderThickness <- Thickness(Left = 1., Top = 1., Right = 1., Bottom = 1.)
            border.Margin <- Thickness(Left = 10., Top = 20., Right = 0., Bottom = 0.)
            border.SetValue(Canvas.ZIndexProperty,4)
        let sp = StackPanel()
        do  sp.Margin <- Thickness(Left = 10., Top = 10., Right = 10., Bottom = 10.)
            sp.MaxWidth <- 150.
            sp.IsHitTestVisible <- true
            sp.Orientation <- Orientation.Vertical
            sp.Children.Add(trussMode_StackPanel) |> ignore
            sp.Children.Add(trussMemberBuilder_StackPanel) |> ignore
            sp.Children.Add(supportType_StackPanel) |> ignore
            sp.Children.Add(trussForceBuilder_StackPanel) |> ignore
            sp.Children.Add(settings_StackPanel) |> ignore
            sp.Children.Add(selectionMode_StackPanel) |> ignore
            sp.Children.Add(analysis_StackPanel) |> ignore
            sp.SetValue(Canvas.ZIndexProperty,4)
            border.Child <- sp
        border    
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
        let l1,l2,e = orgin p
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
    let drawSolvedMembers (state:TrussDomain.TrussAnalysisState) = 
        match state with
        | TrussDomain.AnalysisState a -> 
            match a.analysis with
            | TrussDomain.Truss -> ()
            | TrussDomain.SupportReactionEquations _sre -> () 
            | TrussDomain.SupportReactionResult _srr-> ()                    
            | TrussDomain.MethodOfJointsCalculation mjc ->                     
                do  List.iter (fun  (n, p) -> 
                    match n, TrussServices.getMemberOptionFromTrussPart p with
                    | (z, Some m) when z = 0. -> drawSolvedMember m olive
                    | (z, Some m) when z > 0. -> drawSolvedMember m blue2
                    | (z, Some m) when z < 0. -> drawSolvedMember m red
                    | _ -> ()) mjc.solvedMembers
            | TrussDomain.MethodOfJointsAnalysis mja ->                     
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
    let drawSupport (support:TrussDomain.Support) = 
        let p = trussServices.getPointFromSupport support
        let dir = trussServices.getDirectionFromSupport support
        let isRollerSupportType = trussServices.checkSupportTypeIsRoller support
        do  drawBuildSupportJoint p
            drawBuildSupport (p,dir,isRollerSupportType)
    let drawForce color (force:TrussDomain.Force) =  
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
        let orginPoint = 
            let x = Double.Parse xOrgin_TextBlock.Text
            let y = Double.Parse yOrgin_TextBlock.Text
            System.Windows.Point(x,y)        
        do  canvas.Children.Clear()
            canvas.Children.Add(grid) |> ignore
            drawOrgin orginPoint
            canvas.Children.Add(label) |> ignore
            canvas.Children.Add(result_ScrollViewer) |> ignore
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
        drawSolvedMembers s

        // Set
    let setOrgin p = 
        let joints = getJointsFrom state |> Seq.toList
        let selectedJoint = getJointIndex p
        match selectedJoint with 
        | None -> 
            do  xOrgin_TextBlock.Text <- "0."
                yOrgin_TextBlock.Text <- "0."
        | Some i -> 
            do  xOrgin_TextBlock.Text <- joints.[i].X.ToString()
                yOrgin_TextBlock.Text <- joints.[i].Y.ToString()
    let setGraphicsFromKernel (k:MathKernel) =        
        let code = code_TextBlock.Text // trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value state  //
        let rec getImages i =
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage (k.Graphics.[i])
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
            match i + 1 = k.Graphics.Length with
            | true -> ()
            | false -> getImages (i+1)
        match k.Graphics.Length > 0 with
        | true ->                                        
            let text = link.EvaluateToOutputForm("Style[" + code + ",FontSize -> 30]",pageWidth = 0)
            result_StackPanel.Children.Clear()            
            getImages 0
            result_TextBlock.Text <- text
            result_StackPanel.Children.Add(message_TextBlock) |> ignore
            result_StackPanel.Children.Add(code_TextBlock) |> ignore
            result_StackPanel.Children.Add(result_TextBlock) |> ignore
        | false -> 
            result_StackPanel.Children.Clear()             
            let graphics = link.EvaluateToImage("Style[" + code + ",FontSize -> 30]", width = 0, height = 0)
            let text = link.EvaluateToOutputForm("Style[" + code + ",FontSize -> 30]",pageWidth = 0)
            let image = Image()            
            do  image.Source <- ControlLibrary.Image.convertDrawingImage(graphics)
                result_TextBlock.Text <- text
                result_StackPanel.Children.Add(message_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_Viewbox image) |> ignore
                result_StackPanel.Children.Add(code_TextBlock) |> ignore
                result_StackPanel.Children.Add(result_TextBlock) |> ignore
    let setStateFromAnaysis s =
        do  setGraphicsFromKernel kernel
        let newState = 
            match s with 
            | TrussDomain.AnalysisState a -> 
                match a.analysis with
                | TrussDomain.Truss -> s
                | TrussDomain.SupportReactionEquations _r -> trussServices.analyzeEquations result_TextBlock.Text s
                | TrussDomain.SupportReactionResult _r -> s 
                | TrussDomain.MethodOfJointsCalculation _mj -> trussServices.analyzeEquations result_TextBlock.Text s
                | TrussDomain.MethodOfJointsAnalysis _mj -> s                
            | _ -> s        
        let newCode = 
            match newState with 
            | TrussDomain.AnalysisState a -> 
                match a.analysis with
                | TrussDomain.Truss -> ""
                | TrussDomain.SupportReactionEquations r -> "" 
                | TrussDomain.SupportReactionResult r -> "\"Choose a joint to begin Method of Joints analysis\""                    
                | TrussDomain.MethodOfJointsCalculation r -> "\"Choose next joint\"" 
                | TrussDomain.MethodOfJointsAnalysis _ -> TrussServices.getAnalysisReport newState//"\"Analysis Complete\""                
            | _ -> "opps"
        let newMessage = 
            match newState with 
            | TrussDomain.AnalysisState a -> 
                match a.analysis with
                | TrussDomain.Truss -> ""
                | TrussDomain.SupportReactionEquations r -> "" 
                | TrussDomain.SupportReactionResult r -> "Choose a joint to begin Method of Joints analysis."                    
                | TrussDomain.MethodOfJointsCalculation r -> "Choose next joint."
                | TrussDomain.MethodOfJointsAnalysis _ -> "Analysis Complete. Click Compute to see report."                
            | _ -> "opps"
        let members = getMembersFrom newState
        let forces = getForcesFrom newState
        let supports = getSupportsFrom newState        
        let reactions = trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value newState
        do  state <- newState
            label.Text <- newState.ToString()
            code_TextBlock.Text <- newCode
            message_TextBlock.Text <- newMessage
            Seq.iter (fun (f:TrussDomain.Force) -> 
                match f.magnitude = 0.0 with 
                | true -> () 
                | false -> 
                    drawForce blue f
                    drawReactionForceLabel f.direction (trussServices.getSupportIndexAtJoint f.joint supports) f.magnitude
                ) reactions            
            Seq.iteri (fun i m -> drawMemberLabel m i) members
            Seq.iteri (fun i (f:TrussDomain.Force) -> drawForceLabel f.direction i f.magnitude) forces
            drawSolvedMembers newState            

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
            let newState = // Logic for Truss Mode radio buttons
                match forceBuilder_RadioButton.IsChecked.Value, 
                      memberBuilder_RadioButton.IsChecked.Value,                                            
                      supportBuilder_RadioButton.IsChecked.Value,
                      selection_RadioButton.IsChecked.Value,
                      analysis_RadioButton.IsChecked.Value,                                            
                      settings_RadioButton.IsChecked.Value,                      
                      forceBuilder_RadioButton.IsMouseOver,
                      memberBuilder_RadioButton.IsMouseOver,                                            
                      supportBuilder_RadioButton.IsMouseOver,
                      selection_RadioButton.IsMouseOver,
                      analysis_RadioButton.IsMouseOver, 
                      settings_RadioButton.IsMouseOver with
                // Force
                | true,false,false,false,false,false, true,false,false,false,false,false
                | false,true,false,false,false,false, true,false,false,false,false,false
                | false,false,true,false,false,false, true,false,false,false,false,false
                | false,false,false,true,false,false, true,false,false,false,false,false
                | false,false,false,false,true,false, true,false,false,false,false,false                
                | false,false,false,false,false,true, true,false,false,false,false,false ->    
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceMagnitude_TextBox.IsReadOnly <- false
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the focre horizontal."
                    trussForceMagnitude_TextBox.Text <- "Enter magnitude"
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussDomain.TrussMode.ForceBuild state
                    drawTruss state
                    newState             
                // Member
                | true,false,false,false,false,false, false,true,false,false,false,false
                | false,true,false,false,false,false, false,true,false,false,false,false
                | false,false,true,false,false,false, false,true,false,false,false,false
                | false,false,false,true,false,false, false,true,false,false,false,false
                | false,false,false,false,true,false, false,true,false,false,false,false
                | false,false,false,false,false,true, false,true,false,false,false,false -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Visible
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussDomain.TrussMode.MemberBuild state
                    drawTruss newState
                    newState
                // Support
                | true,false,false,false,false,false,  false,false,true,false,false,false
                | false,true,false,false,false,false,  false,false,true,false,false,false
                | false,false,true,false,false,false,  false,false,true,false,false,false
                | false,false,false,true,false,false,  false,false,true,false,false,false
                | false,false,false,false,true,false,  false,false,true,false,false,false 
                | false,false,false,false,false,true,  false,false,true,false,false,false -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Visible
                    supportType_StackPanel.Visibility <- Visibility.Visible
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true
                    trussForceAngle_TextBox.Text <- "Enter Angle"
                    trussForceAngle_TextBox.ToolTip <- "Angle of the contact plane from horizontal."
                    trussForceMagnitude_TextBox.Text <- "0"
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newTrust = trussServices.setTrussMode TrussDomain.TrussMode.SupportBuild state
                    drawTruss newTrust
                    newTrust                                
                // Selection
                | true,false,false,false,false,false,  false,false,false,true,false,false
                | false,true,false,false,false,false,  false,false,false,true,false,false
                | false,false,true,false,false,false,  false,false,false,true,false,false
                | false,false,false,true,false,false,  false,false,false,true,false,false
                | false,false,false,false,true,false,  false,false,false,true,false,false 
                | false,false,false,false,false,true,  false,false,false,true,false,false -> 
                    message_TextBlock.Text <- "--Select a Truss Part--"                    
                    code_TextBlock.Text <- "Ready"
                    setGraphicsFromKernel kernel
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- 
                        match inspect_RadioButton.IsChecked.Value with
                        | false -> Visibility.Collapsed
                        | true -> Visibility.Visible
                    result_ScrollViewer.IsHitTestVisible <- false
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Visible
                    trussForceMagnitude_TextBox.IsReadOnly <- true                    
                    let newState = trussServices.setTrussMode TrussDomain.TrussMode.Selection state                    
                    drawTruss newState
                    newState
                // Analysis
                | true,false,false,false,false,false,  false,false,false,false,true,false
                | false,true,false,false,false,false,  false,false,false,false,true,false
                | false,false,true,false,false,false,  false,false,false,false,true,false
                | false,false,false,true,false,false,  false,false,false,false,true,false
                | false,false,false,false,true,false,  false,false,false,false,true,false 
                | false,false,false,false,false,true,  false,false,false,false,true,false -> 
                    message_TextBlock.Text <- "Calculate Reactions"
                    let newState = 
                        trussServices.checkTruss (trussServices.getTrussFromState state)
                        |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                    code_TextBlock.Text <- trussServices.getAnalysisReport newState
                    setGraphicsFromKernel kernel
                    result_ScrollViewer.Visibility <- Visibility.Visible
                    result_ScrollViewer.IsHitTestVisible <- true
                    reactionRadio_StackPanel.Visibility <- Visibility.Visible
                    analysis_StackPanel.Visibility <- Visibility.Visible
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Collapsed
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true                    
                    code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                    drawTruss newState
                    newState                    
                // Settings
                | true,false,false,false,false,false,  false,false,false,false,false,true
                | false,true,false,false,false,false,  false,false,false,false,false,true
                | false,false,true,false,false,false,  false,false,false,false,false,true
                | false,false,false,true,false,false,  false,false,false,false,false,true
                | false,false,false,false,true,false,  false,false,false,false,false,true 
                | false,false,false,false,false,true,  false,false,false,false,false,true -> 
                    reactionRadio_StackPanel.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.Visibility <- Visibility.Collapsed
                    result_ScrollViewer.IsHitTestVisible <- true
                    analysis_StackPanel.Visibility <- Visibility.Collapsed
                    trussMemberBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceBuilder_StackPanel.Visibility <- Visibility.Collapsed
                    supportType_StackPanel.Visibility <- Visibility.Collapsed
                    settings_StackPanel.Visibility <- Visibility.Visible
                    selectionMode_StackPanel.Visibility <- Visibility.Collapsed
                    trussForceMagnitude_TextBox.IsReadOnly <- true
                    message_TextBlock.Text <- "Calculate Reactions"
                    code_TextBlock.Text <- "Ready"
                    let newState = trussServices.setTrussMode TrussDomain.TrussMode.Settings state
                    drawTruss state
                    newState
                | _ -> // Logic for Support Type radio buttons
                    match rollerSupport_RadioButton.IsChecked.Value, pinSupport_RadioButton.IsChecked.Value, 
                          rollerSupport_RadioButton.IsMouseOver, pinSupport_RadioButton.IsMouseOver 
                          with 
                    | true,false, true,false
                    | false,true, true,false -> rollerSupport_RadioButton.IsChecked <- Nullable true
                                                pinSupport_RadioButton.IsChecked <- Nullable false
                                                trussServices.sendStateToSupportBuilder false state
                    | true,false, false,true
                    | false,true, false,true -> rollerSupport_RadioButton.IsChecked <- Nullable false 
                                                pinSupport_RadioButton.IsChecked <- Nullable true
                                                trussServices.sendStateToSupportBuilder true state
                    | _ -> // Logic for Selection Mode radio buttons
                        match delete_RadioButton.IsChecked.Value, 
                              inspect_RadioButton.IsChecked.Value, 
                              modify_RadioButton.IsChecked.Value, 
                              delete_RadioButton.IsMouseOver, 
                              inspect_RadioButton.IsMouseOver,
                              modify_RadioButton.IsMouseOver
                              with 
                        | true,false,false, true,false,false
                        | false,true,false, true,false,false 
                        | false,false,true, true,false,false ->  
                            delete_RadioButton.IsChecked <- Nullable true
                            inspect_RadioButton.IsChecked <- Nullable false
                            modify_RadioButton.IsChecked <- Nullable false
                            delete_Button.Visibility <- Visibility.Visible                            
                            result_ScrollViewer.Visibility <- Visibility.Collapsed
                            result_ScrollViewer.IsHitTestVisible <- true
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            trussServices.setSelectionMode TrussDomain.TrussSelectionMode.Delete state
                        | true,false,false, false,true,false
                        | false,true,false, false,true,false 
                        | false,false,true, false,true,false ->                        
                            setGraphicsFromKernel kernel
                            delete_RadioButton.IsChecked <- Nullable false 
                            inspect_RadioButton.IsChecked <- Nullable true
                            modify_RadioButton.IsChecked <- Nullable false
                            delete_Button.Visibility <- Visibility.Collapsed                            
                            result_ScrollViewer.Visibility <- Visibility.Visible
                            result_ScrollViewer.IsHitTestVisible <- false
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            trussServices.setSelectionMode TrussDomain.TrussSelectionMode.Inspect state
                        | true,false,false, false,false,true
                        | false,true,false, false,false,true 
                        | false,false,true, false,false,true ->                        
                            delete_RadioButton.IsChecked <- Nullable false 
                            inspect_RadioButton.IsChecked <- Nullable false
                            modify_RadioButton.IsChecked <- Nullable true
                            delete_Button.Visibility <- Visibility.Collapsed                            
                            result_ScrollViewer.Visibility <- Visibility.Collapsed
                            result_ScrollViewer.IsHitTestVisible <- true
                            message_TextBlock.Text <- "--Select a Truss Part--"
                            code_TextBlock.Text <- "Ready"
                            trussServices.setSelectionMode TrussDomain.TrussSelectionMode.Modify state
                        | _ -> // Logic for Analysis Mode radio buttons                            
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
                                    trussServices.checkTruss (trussServices.getTrussFromState state)
                                    |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                                code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                                newState
                            | true,false, false,true
                            | false,true, false,true -> 
                                xAxis_RadioButton.IsChecked <- Nullable false 
                                yAxis_RadioButton.IsChecked <- Nullable true                                
                                let newState = 
                                    trussServices.checkTruss (trussServices.getTrussFromState state)
                                    |> trussServices.getSupportReactionEquationsFromState yAxis_RadioButton.IsChecked.Value
                                code_TextBlock.Text <- trussServices.getSupportReactionSolve yAxis_RadioButton.IsChecked.Value newState
                                newState
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
                                    drawTruss state
                                    Seq.iter (fun (f:TrussDomain.Force) -> match f.magnitude = 0.0 with | true -> () | false -> drawForce red f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value state)
                                    state
                                | true,false, false,true
                                | false,true, false,true -> 
                                    resultant_RadioButton.IsChecked <- Nullable false 
                                    components_RadioButton.IsChecked <- Nullable true
                                    drawTruss state
                                    Seq.iter (fun (f:TrussDomain.Force) -> match f.magnitude = 0.0 with | true -> () | false -> drawForce blue f) (trussServices.getReactionForcesFromState components_RadioButton.IsChecked.Value state)
                                    state
                                | _ -> state
            do  state <- newState
                label.Text <- newState.ToString() 
        | false ->  
            match state with
            | TrussDomain.TrussState ts -> 
                match ts.mode with
                | TrussDomain.TrussMode.MemberBuild ->                
                    let newState = trussServices.sendPointToMemberBuilder p state
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
                        let newState = trussServices.sendPointToForceBuilder p state
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
                            let newState = trussServices.sendPointToPinSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                        | false -> 
                            let newState = trussServices.sendPointToRollerSupportBuilder p state
                            drawBuildSupportJoint p
                            state <- newState
                            label.Text <- state.ToString()
                | TrussDomain.TrussMode.Analysis -> setGraphicsFromKernel kernel //()//
                | TrussDomain.TrussMode.Selection -> label.Text <- state.ToString()
                | TrussDomain.TrussMode.Settings -> ()
            | TrussDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussDomain.BuildMember bm ->                 
                    let newState = trussServices.sendPointToMemberBuilder p state
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
            | TrussDomain.AnalysisState a -> 
                match a.analysis with
                | TrussDomain.Truss -> ()
                | TrussDomain.SupportReactionEquations r -> () 
                | TrussDomain.SupportReactionResult r -> 
                    let newState = trussServices.sendPointToCalculation p state
                    let newCode = 
                        match newState with 
                        | TrussDomain.AnalysisState a ->
                            match a.analysis with
                            | TrussDomain.MethodOfJointsCalculation r -> WolframServices.solveEquations r.memberEquations r.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state <- newState
                        code_TextBlock.Text <- newCode
                        setOrgin p 
                        drawTruss newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)
                | TrussDomain.MethodOfJointsCalculation mjc -> 
                    let newState = trussServices.sendPointToCalculation p state
                    let newCode = 
                        match newState with 
                        | TrussDomain.AnalysisState a ->
                            match a.analysis with
                            | TrussDomain.MethodOfJointsCalculation mjc -> WolframServices.solveEquations mjc.memberEquations mjc.variables
                            | _ -> "1"
                        | _ -> "2"
                    do  state <- newState
                        code_TextBlock.Text <- newCode
                        setOrgin p 
                        drawTruss newState
                        Seq.iteri (fun i m -> drawMemberLabel m i) (getMembersFrom newState)                        
                | TrussDomain.MethodOfJointsAnalysis mja -> drawSolvedMembers state 
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
            | TrussDomain.TrussMode.Settings -> ()
        | TrussDomain.BuildState bs -> 
            match bs.buildOp with
            | TrussDomain.BuildMember bm -> ()
            | TrussDomain.BuildForce bf -> ()
            | TrussDomain.BuildSupport bs -> ()
        | TrussDomain.SelectionState ss -> 
            let truss = getTrussFrom state
            match ss.forces, ss.members, ss.supports with
            | Some f, None, None -> 
                let i = trussServices.getForceIndex f.Head truss
                message_TextBlock.Text <- "--Force " + i.ToString() + "--"
                code_TextBlock.Text <- trussServices.getAnalysisReport state
                setGraphicsFromKernel kernel
            | None, Some m, None ->                
                let i = trussServices.getMemberIndex m.Head truss
                message_TextBlock.Text <- "--Member " + i.ToString() + "--"
                code_TextBlock.Text <- trussServices.getAnalysisReport state
                setGraphicsFromKernel kernel
            | None, None, Some s -> 
                let i,st = trussServices.getSupportIndex s.Head truss
                message_TextBlock.Text <- "--" + st + " Support " + i.ToString() + "--"
                code_TextBlock.Text <- trussServices.getAnalysisReport state
                setGraphicsFromKernel kernel
            | _ -> message_TextBlock.Text <- "--Select a Truss Part--"
                   code_TextBlock.Text <- "Ready"
            drawTruss state
        | TrussDomain.AnalysisState a -> ()
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
            | TrussDomain.TrussMode.Settings -> ()
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
        | TrussDomain.AnalysisState s -> ()
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
                        let newState = trussServices.sendPointToMemberBuilder p state
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
                | TrussDomain.TrussMode.Settings -> ()
            | TrussDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussDomain.BuildMember _bm ->                 
                    match p2 with
                    | None -> ()
                    | Some p -> 
                        let newState = trussServices.sendPointToMemberBuilder p state
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
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state |> trussServices.sendPointToForceBuilder dirPoint
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        drawBuildForceDirection green (arrowPoint,dir,mag)
                        label.Text <- newState.ToString()
                    | true, true -> 
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let arrowPoint = 
                            match mag > 0. with 
                            | true -> jointPoint
                            | false -> dirPoint
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        drawBuildForceDirection green (arrowPoint,dir,mag)
                        label.Text <-  newState.ToString()
                    | true, false -> 
                        let newState = trussServices.sendMagnitudeToForceBuilder mag state
                        state <- newState
                        label.Text <- newState.ToString()
                    | false, true -> 
                        let jointPoint = trussServices.getPointFromForceBuilder bf
                        let dirPoint = System.Windows.Point(jointPoint.X + (50.0 * cos (dir * Math.PI/180.)), jointPoint.Y - (50.0 * sin (dir * Math.PI/180.)))  
                        let newState = trussServices.sendPointToForceBuilder dirPoint state
                        state <- newState
                        drawBuildForceLine green (jointPoint,dirPoint)
                        label.Text <- newState.ToString()
                    | false, false -> () 
                | TrussDomain.BuildSupport bs -> 
                    match dirb with
                    | true -> 
                        let jointPoint = 
                            match bs with 
                            | TrussDomain.Roller f -> trussServices.getPointFromForceBuilder f
                            | TrussDomain.Pin (f,_) -> trussServices.getPointFromForceBuilder f
                        let dirPoint = System.Windows.Point(jointPoint.X + (1.0 * cos ((dir) * Math.PI/180.)), jointPoint.Y - (1.0 * sin ((dir) * Math.PI/180.)))  
                        let arrowPoint = jointPoint                            
                        let newState = 
                            match rollerSupport_RadioButton.IsChecked.Value with
                            | true -> trussServices.sendMagnitudeToSupportBuilder (mag,None) state |> trussServices.sendPointToRollerSupportBuilder dirPoint
                            | false -> trussServices.sendMagnitudeToSupportBuilder (mag,Some mag) state |> trussServices.sendPointToPinSupportBuilder dirPoint
                        state <- newState
                        drawBuildSupport (arrowPoint,dir - 90.,rollerSupport_RadioButton.IsChecked.Value)
                        label.Text <-  newState.ToString()
                    | false -> 
                        let newState = trussServices.sendMagnitudeToSupportBuilder (mag,None) state
                        state <- newState
                        label.Text <- newState.ToString()                    
            | TrussDomain.SelectionState ss -> ()
            | TrussDomain.AnalysisState s -> ()
            | TrussDomain.ErrorState es -> ()
        | Input.Key.Delete -> 
            match state with
            | TrussDomain.TrussState ts -> 
                match ts.mode with
                | TrussDomain.TrussMode.MemberBuild -> ()
                | TrussDomain.TrussMode.ForceBuild -> ()                    
                | TrussDomain.TrussMode.SupportBuild -> ()
                | TrussDomain.TrussMode.Analysis -> ()
                | TrussDomain.TrussMode.Selection -> ()
                | TrussDomain.TrussMode.Settings -> ()
            | TrussDomain.BuildState bs -> 
                match bs.buildOp with
                | TrussDomain.BuildMember _bm -> ()
                | TrussDomain.BuildForce bf -> ()
                | TrussDomain.BuildSupport bs -> ()        
            | TrussDomain.SelectionState ss -> 
                match ss.mode with
                | TrussDomain.TrussSelectionMode.Delete -> 
                    let newState = trussServices.removeTrussPartFromTruss state            
                    do  state <- newState 
                        label.Text <- newState.ToString()
                        drawTruss newState
                | TrussDomain.TrussSelectionMode.Modify  -> ()
                | TrussDomain.TrussSelectionMode.Inspect -> ()
            | TrussDomain.AnalysisState s -> ()                
            | TrussDomain.ErrorState es -> ()
        | _ -> () // logic for other keys

    (*Initialize*)
    // label.Text <- state.ToString()
    do  
        this.Content <- screen_Grid
        setGraphicsFromKernel kernel        
        
    (*add event handlers*)        
        this.PreviewKeyDown.AddHandler(Input.KeyEventHandler(fun _ e -> handleKeyDown(e)))
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))
        this.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        this.PreviewMouseLeftButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleMouseDown e))
        this.PreviewMouseLeftButtonUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleMouseUp()))
        this.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove e))
        xUp_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        xDown_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        yUp_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        yDown_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        delete_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> drawTruss state))
        compute_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> setStateFromAnaysis state))

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
