namespace Math.Presentation.WolframEngine.Analysis

open System.Windows

module GenericDomain =
    
    // This container is used by some controls to share a variable. If the value is changed, 
    // it fires changed event. Controls should have this instead of their own internal data
    type SharedValue<'a when 'a : equality>(value:'a) =
        let mutable _value = value
        let changed = Event<'a>()
        member _sv.Get       = _value
        member _sv.Set value =
            let old = _value 
            _value <- value
            if old <> _value then _value |> changed.Trigger
        member _sv.Changed = changed.Publish

module AtomicDomain =    
    
    // Arbitrary coordinate lables.
    type X = X of float
    type Y = Y of float
    type Z = Z of float
    
    // Points in Euclidean affine spaces 
    // Joints are a consequence of connecting atomic or elemental structures together.
    type Joint = {x:X; y:Y}
    type Joint3D = {x:X; y:Y; z:Z}
    
    // Any arbitrary connector of two points. 
    // Equal end points implies a member that loops back in on itself.        
    type Member = (Joint*Joint)
    
    module Material =
    
        // StructuralProperties
        type Composition =
            | Metallica (*In honor of the Band*)
            | Ceramic
            | Polymeric
            | Composite
        type CrystalStructure =
            | Cubic
            | Tetragonal
            | Hexagonal
            | Trigonal
            | Orthorhombic
            | Monoclinic
            | Triclinic
            | Glassy
        type Microstructure = 
            | PhaseGrains // A distinct crystal structure and/or chemical composition.
            | ComponentGrains // A pure substance with a uniquely-defined chemical composition.
            | PointDefect // Such as vacancies and interstitials.            
            | PlanarDefect // Such as surfaces, twin boundaries, and grain boundaries.
            | Dislocation        
        type StructuralProperties = {
            composition:Composition List; 
            crystalStructure:CrystalStructure;
            microStructures:Microstructure list}

        type PhysicalProperties =
            | State // Solid, liquid or gas; not as simple as it may seem.
            | Density // Mass of a material per unit volume.
            | Magnetisum //The physical attraction for iron, inherent in a material or induced by moving electric fields.
            | Solubility // The maximum amount of a solute that can be added to a solvent.
            | Viscocity // Resistance of a material (usually liquid) to flow.

        type MechanicalProperties = 
            | Malleability
            | Ductility
            | TensileStrength
            | FlexuralStrength
            | Hardness
            | Toughness
            | Brittleness
            | Elasticity
            | PlasticDeformation
            | Stiffness

        type ElectricalProperties =
         | Conductivity
         | Resistivity
         | DielectricStrength
        
        type ChemicalProperties =
         | Toxicity
         | ChemicalResistance
         | CorrosionResistance
         | Combustibility
         | Passivity
         | Biocompatibility
        
        type ThermalProperties =
         | SpecificHeat
         | ThermalExpansion
         | ThermalConductivit
         | GlassTransitionTemp
         | MeltingPoint
        
        type OpticalProperties =
         | Transmissivity
         | Absorptivity
         | IndexOfRefraction
         | Photoconductivity
         | Polarization

        type Properties = 
            | Structural of StructuralProperties
            | Phyiscal of PhysicalProperties list
            | Mechanical of MechanicalProperties list
            | Electrical 
            | Chemical
            | Thermal
            | Optical

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
    type Support = 
        | Roller of JointForce 
        | Pin of Pin
        | Hinge
        | Fixed
        | Simple
    type SupportReactionResult = 
        {support: Support;
         xReactionForce: JointForce option;
         yReactionForce: JointForce option}

    type Part = 
        | Member of Member
        | Force of JointForce
        | Support of Support    
    type Node = (Joint*Part list)

    type Truss = {members:Member list; forces:JointForce list; supports:Support list}
    type TrussMemberForce = (float*Part)

    type System = 
        | TrussSystem of Truss
        | Beam // etc. TODO        

module BuilderDomain = 
    open AtomicDomain
    open ElementDomain
    
    type MemberBuilder = (Joint*(Joint option))
    type JointForceBuilder = {_magnitude:float option; _direction:Vector option; joint:Joint}
    type SupportBuilder = | Roller of JointForceBuilder | Pin of (JointForceBuilder*JointForceBuilder)       
    
    type TrussBuildOp =
        | BuildMember of MemberBuilder
        | BuildForce of JointForceBuilder
        | BuildSupport of SupportBuilder
        | Control
    type BuildOpResult =
        | TrussPart of Part
        | TrussBuildOp of TrussBuildOp

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

module ControlDomain = 
    
    type WolframResultControlSettings = {codeVisible:bool;resultVisible:bool;isHitTestVisible:bool;isVisible:bool}
    
    type SelectionMode =
        | Delete
        | Modify
        | Inspect

    type ControlMode =
        | Settings
        | Selection
        | Analysis
        | MemberBuild
        | ForceBuild
        | SupportBuild

module AnalysisDomain =    
    
    open ErrorDomain
    open AtomicDomain
    open LoadDomain
    open ElementDomain
    open BuilderDomain

    type SupportReactionEquationStateData = 
        {momentEquations: string list;
         forceXEquation: string
         forceYEquation: string} 
    type SupportReactionResultStateData = {reactions : SupportReactionResult list}
    
    module TrussAnalysisDomain =
                
        type TrussStability = 
            | Stable 
            | NotEnoughReactions 
            | ReactionsAreParallel 
            | ReactionsAreConcurrent 
            | InternalCollapseMechanism
        type TrussDeterminacy = 
            | Determinate 
            | Indeterminate
            
        type MethodOfJointsCalculationStateData = 
            {solvedMembers: TrussMemberForce list;
             memberEquations : string list;
             nodes : Node list;
             reactions : SupportReactionResult list;
             variables : string list}
        type MethodOfJointsAnalysisStateData = 
            {zeroForceMembers: Part list;
             tensionMembers: TrussMemberForce list;
             compressionMembers: TrussMemberForce list;
             reactions : SupportReactionResult list}
        type MethodOfJointsAnalysisState =
            | Truss
            | SupportReactionEquations of SupportReactionEquationStateData
            | SupportReactionResult of SupportReactionResultStateData
            | MethodOfJointsCalculation of MethodOfJointsCalculationStateData
            | MethodOfJointsAnalysisReport of MethodOfJointsAnalysisStateData 

        type TrussAnalysis = 
            | MethodOfJointsAnalysis of MethodOfJointsAnalysisState
        
        // Data associated with each truss state
        type TrussStateData = {truss:Truss; mode:ControlDomain.ControlMode} // Includes the empty truss
        type TrussBuildData = {buildOp : TrussBuildOp;  truss : Truss}
        type TrussSelectionStateData = 
            {truss:Truss; 
             members:Member list option; 
             forces:JointForce list option; 
             supports:Support list option; 
             modification: Joint option;
             mode:ControlDomain.SelectionMode}
        type TrussErrorStateData = {errors : Error list; truss : Truss}        
        type TrussAnalysisStateData = 
            {stability:TrussStability list; 
             determinancy:TrussDeterminacy;
             analysis:TrussAnalysis;
             truss:Truss}         
        type TrussAnalysisState  =
            | TrussState of TrussStateData
            | BuildState of TrussBuildData
            | SelectionState of TrussSelectionStateData
            | AnalysisState  of TrussAnalysisStateData
            | ErrorState of TrussErrorStateData   
    
    // Analysis States
    type AnalysisState = //
        | Truss of TrussAnalysisDomain.TrussAnalysisState 
         

