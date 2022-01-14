namespace Math.Presentation.WolframEngine.Analysis

open System.Windows

module CoordinateDomain =
    
    // Arbitrary coordinate lables.
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
    type Member = (Joint*Joint)
    
module LoadDomain = 
    open AtomicDomain    
    
    // Abstract force applied to a joint. The direction vector is based on the joint as the origin
    // and only expresses the orientation in the 2D plane. The sign of magnitude of the force expresses 
    // the direction of the force along the direction vector. Positive magnitude points towards the 
    // Joint. Negative magnitude points away from the Joint.    
    type JointForce = {magnitude:float; direction:Vector; joint:Joint}
    
    // Abstract force applied at a joint expressed as component forces acting along each axis. 
    type ComponentForces = {magnitudeX:float; magnitudeY:float; atJoint:Joint}
    type ComponentForces3D = {magnitudeX3D:float; magnitudeY3D:float; magnitudeZ3D:float; atJoint3D:Joint3D}

module ElementDomain =
    open AtomicDomain
    open LoadDomain
        
    type Pin = {tangent:JointForce;normal:JointForce}
    type Support = | Roller of JointForce | Pin of Pin
    type Truss = {members:Member list; forces:JointForce list; supports:Support list}

module BuilderDomain = 
    open AtomicDomain
    
    type MemberBuilder = (Joint*(Joint option))
    type JointForceBuilder = {_magnitude:float option; _direction:Vector option; joint:Joint}
    type SupportBuilder = | Roller of JointForceBuilder | Pin of (JointForceBuilder*JointForceBuilder)       

module ErrorDomain = 
    // Types to describe error results
    type Error = 
        | LazyCoder  
        | Input
        | TrussBuildOpFailure
        | TrussModeError
        | WrongStateData
        | NoJointSelected
        | Other of string

module TrussAnalysisDomain =
    open ErrorDomain
    open AtomicDomain
    open LoadDomain
    open ElementDomain
    open BuilderDomain
    
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
    
    type TrussNode = (Joint*TrussPart list)    
  
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
         nodes : TrussNode list;
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
    
    

