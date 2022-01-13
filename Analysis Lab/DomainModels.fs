namespace Math.Presentation.WolframEngine.Analysis

open System.Windows

module CoordinateDomain =
    

    type X = X of float
    type Y = Y of float
    type Z = Z of float

module AtomicDomain =    
    open CoordinateDomain
    
    // Points in Euclidean affine spaces 
    // Joints are a consequence of connecting atomic or elemental structures together.
    type Joint = {x:X; y:Y}
    type Joint3D = {x:X; y:Y; z:Z}
    
    // Any arbitrary connector of two points. 
    // Equal end points implies a member that loops back in on itself.
    type MemberBuilder = (Joint*(Joint option))    
    type Member = (Joint*Joint)
    
module LoadDomain = 
    open AtomicDomain    
    
    // Abstract force applied to a joint. The direction vector is based on the joint as the origin
    // and only expresses the orientation in the 2D plane. The sign of magnitude of the force expresses 
    // the direction of the force along the direction vector. Positive magnitude points towards the 
    // Joint. Negative magnitude points away from the Joint.
    type JointForceBuilder = {_magnitude:float option; _direction:Vector option; joint:Joint}
    type JointForce = {magnitude:float; direction:Vector; joint:Joint}
    
    // Abstract force applied at a joint expressed as component forces acting along each axis. 
    type ComponentForces = {magnitudeX:float; magnitudeY:float; atJoint:Joint}
    type ComponentForces3D = {magnitudeX3D:float; magnitudeY3D:float; magnitudeZ3D:float; atJoint3D:Joint3D}

module ElementDomain =
    open LoadDomain
        
    type Pin = {tangent:JointForce;normal:JointForce}
    
    type SupportBuilder = | Roller of JointForceBuilder | Pin of (JointForceBuilder*JointForceBuilder)       
    type Support = | Roller of JointForce | Pin of Pin


module TrussDomain =
    open AtomicDomain
    open LoadDomain
    open ElementDomain
    
    type Truss = {members:Member list; forces:JointForce list; supports:Support list}    
    
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
        | Force of JointForce
        | Support of Support
    type MemberForce = (float*TrussPart)
    type TrussBuildOp =
        | BuildMember of MemberBuilder
        | BuildForce of JointForceBuilder
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
         forces:JointForce list option; 
         supports:Support list option; 
         modification: Joint option;
         mode:TrussSelectionMode}    
    type ErrorStateData = {errors : Error list; truss : Truss}
    type SupportReactionResult = 
        {support: Support;
         xReactionForce: JointForce option;
         yReactionForce: JointForce option}    
    
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

