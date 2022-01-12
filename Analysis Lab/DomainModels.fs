namespace Math.Presentation.WolframEngine.Analysis

open System.Windows

module CoordinateDomain =
    type X = X of float
    type Y = Y of float
    type Z = Z of float

module ObjectDomain =    
    open CoordinateDomain
        
    type Joint = {x:X; y:Y}
    type Joint3D = {x:X; y:Y; z:Z}
        
    type MemberBuilder = (Joint*(Joint option))    
    type Member = (Joint*Joint)
    
module LoadDomain = 
    open ObjectDomain    
    
    type ForceBuilder = {_magnitude:float option; _direction:Vector option; joint:Joint}
    type Force = {magnitude:float; direction:Vector; joint:Joint}
    type ComponentForces = {magnitudeX:float; magnitudeY:float; atJoint:Joint}

module TrussDomain =
    open ObjectDomain
    open LoadDomain
    
    type Pin = {tangent:Force;normal:Force}
    type SupportBuilder = | Roller of ForceBuilder | Pin of (ForceBuilder*ForceBuilder)       
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
         modification: Joint option;
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

