namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Windows
open TrussAnalysisDomain
open BuilderDomain
open AtomicDomain
open LoadDomain
open ElementDomain
open WolframServices

module TrussImplementation =     
    
    let getYFrom (j:Joint) = match j.y with | Y y -> y
    let getXFrom (j:Joint) = match j.x with | X x -> x
    let getReactionForcesFrom (s: Support list) = 
        List.fold (fun acc r -> match r with | Roller f -> f::acc  | Pin p -> p.tangent::p.normal::acc | _ -> acc) [] s
    
    let getMembersAt (j:Joint) (m:Member list) = 
        List.choose (fun (p1,p2) -> match p1 = j || p2 = j with | true -> Some (Member (p1,p2)) | false -> None) m
    let getForcesAt (j:Joint) (f:JointForce list) = 
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

    let getDirectionsFrom (f:JointForce list) = List.map (fun x -> 
        Vector(
            x = x.direction.X - (getXFrom x.joint), 
            y = x.direction.Y - (getYFrom x.joint))) f
    let getLineOfActionFrom (f:JointForce) = 
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
    
    let getComponentForcesFrom (f:JointForce) =
        let x = f.direction.X - (getXFrom f.joint)
        let y = f.direction.Y - (getYFrom f.joint)
        let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n 
        {atJoint = f.joint; magnitudeX = -f.magnitude*(x/d); magnitudeY = f.magnitude*(y/d)}
    let getResultantForcesFrom (r:SupportReactionResult) =        
        match r with 
        | {support = Roller r; xReactionForce = Some x; yReactionForce = None} -> 
            Some {magnitude = x.magnitude; direction = x.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = Some x; yReactionForce = None} -> 
            Some {magnitude = x.magnitude; direction = x.direction ; joint = p.normal.joint}        
        | {support = Roller r; xReactionForce = None; yReactionForce = Some y} -> 
            Some {magnitude = y.magnitude; direction = y.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = None; yReactionForce = Some y} -> 
            Some {magnitude = y.magnitude; direction = p.normal.direction ; joint = p.normal.joint}        
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
            Some {magnitude = magXY; direction = direction ; joint = r.joint}
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
            Some {magnitude = magXY; direction = direction ; joint = p.normal.joint}
        | {support = Roller r; xReactionForce = None; yReactionForce = None} -> 
            Some {magnitude = r.magnitude; direction = r.direction ; joint = r.joint}
        | {support = Pin p; xReactionForce = None; yReactionForce = None} -> 
            Some {magnitude = 0.0; direction = p.normal.direction ; joint = p.normal.joint}
        | _ -> None
    let getSupportComponentForcesFrom (s:Support) = // need to refactor this function
        match s with
        | Pin p ->              
            let x = p.tangent.direction.X - (getXFrom p.tangent.joint)
            let y = p.normal.direction.Y - (getYFrom p.normal.joint)
            let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n 
            Some {atJoint = p.tangent.joint; magnitudeX = p.tangent.magnitude*(x/d); magnitudeY = p.normal.magnitude*(y/d)}
        | Roller r -> 
            let x = r.direction.X - (getXFrom r.joint)
            let y = r.direction.Y - (getYFrom r.joint)
            let d = match Math.Sqrt (x*x + y*y) with | n when n = 0. -> 1. | n -> n
            Some {atJoint = r.joint; magnitudeX = r.magnitude*(x/d); magnitudeY = r.magnitude*(y/d)}
        | _ -> None
    let getJointFromSupportBuilder (sb:BuilderDomain.SupportBuilder) = 
        match sb with
        | BuilderDomain.SupportBuilder.Roller r -> r.joint
        | BuilderDomain.SupportBuilder.Pin (p,_) -> p.joint
    let getJointFromSupport (s:Support) = match s with | Pin p -> Some p.normal.joint | Roller r -> Some r.joint | _ -> None
            
    let sumForcesX (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        List.fold (fun acc x -> x.magnitudeX + acc ) 0. forces
    let sumForcesY (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        List.fold (fun acc y -> y.magnitudeY + acc ) 0. forces
    let sumForceMoments (s:Support) (p:TrussPart list) = 
        let forces = List.choose (fun x -> match x with | Force f -> Some (getComponentForcesFrom f) | _ -> None) p
        let j = getJointFromSupport s
        let getMomentArmY fj = (getYFrom fj - getYFrom j.Value)
        let getMomentArmX fj = (getXFrom fj - getXFrom j.Value)
        List.fold (fun acc cf -> cf.magnitudeX*(getMomentArmY cf.atJoint) + cf.magnitudeY*(getMomentArmX cf.atJoint) + acc ) 0. forces
    
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
                Math.Abs x/d,"Rx" + i.ToString() 
            | _ -> 0.0,"Not Implemented") supports
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
                y/d,"Ry" + i.ToString()
            | _ -> 0.0,"Not Implemented") supports 
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
        let supportXY = supportXY "Rx" (addSupportXY p) p
        let getSupportMoments (s:Support) = 
            let j = getJointFromSupport s
            let getMomentArmX sj = getXFrom sj - getXFrom j.Value
            let getMomentArmY sj = getYFrom sj - getYFrom j.Value
            let ly = List.mapi (fun i s -> ((getJointFromSupport s).Value |> getMomentArmX),"Ry" + i.ToString()) supports
            let lx = 
                match s with 
                | Pin _ ->
                    match addSupportXY p with 
                    | false -> []
                    | true -> List.mapi (fun i s -> ((getJointFromSupport s).Value |> getMomentArmY),"Rx" + i.ToString()) supports
                | Roller _ -> List.mapi (fun i s -> ((getJointFromSupport s).Value |> getMomentArmY),"Rx" + i.ToString()) supports
                | _ -> []
            List.concat [ly;lx]
        let momentEquations = List.map (fun y -> createEquation (sumForceMoments y p) (getSupportMoments y)) supports        
        List.concat [momentEquations; supportXY] 
    let getXMomentReactionEquations (p:TrussPart list) =         
        let supports = List.choose (fun x -> match x with | Support s -> Some s | _ -> None) p      
        let supportXY = supportXY "Ry" (addSupportXY p) p
        let getSupportMoments (s:Support) = 
            let j = getJointFromSupport s
            let getMomentArmX sj = getXFrom sj - getXFrom j.Value
            let getMomentArmY sj = getYFrom sj - getYFrom j.Value            
            let lx = List.mapi (fun i y -> ((getJointFromSupport y).Value |> getMomentArmY),"Rx" + i.ToString()) supports
            let ly = 
                match s with 
                | Pin _ ->
                    match addSupportXY p with 
                    | false -> []
                    | true -> List.mapi (fun i s -> ((getJointFromSupport s).Value |> getMomentArmX),"Ry" + i.ToString()) supports 
                | Roller _ -> List.mapi (fun i s -> ((getJointFromSupport s).Value |> getMomentArmX),"Ry" + i.ToString()) supports
                | Fixed -> []
                | Hinge -> []
                | Simple -> []
            List.concat [ly;lx]        
        let momentEquations = List.map (fun x -> createEquation (sumForceMoments x p) (getSupportMoments x)) supports        
        List.concat [momentEquations; supportXY]    

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
    let getForceIndex (f:JointForce) (t:Truss) =
        let i = List.tryFindIndex (fun x -> x = f) t.forces
        match i with | Some n -> n | None -> -1
    let getSupportIndex (s:Support) (t:Truss) =
        let i = match List.tryFindIndex (fun x -> x = s) t.supports with | Some n -> n | None -> -1
        match s with
        | Pin _ -> i,"Pin"
        | Roller _ -> i,"Roller"
        | Fixed -> i,"Fixed"
        | Hinge -> i,"Hinge"
        | Simple -> i,"Simple"
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
    let partitionNode (tpl:TrussNode list) = 
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
        List.fold (fun acc x -> match x with | (j,pl,_zfm) -> TrussNode (j,List.except zfm pl)::acc) [] partition     
    let getMemberVariables (t:Truss) =
        let nl = getNodeList t
        let processNode (n:TrussNode) = 
            let _j,pl = n            
            let members = 
                List.choose (fun x -> match x with | Member m -> Some m | _ -> None) pl
                |> List.map (fun x -> "M" + (getMemberIndex x t).ToString())            
            List.concat [members] 
        List.map (fun x -> processNode x) nl
        |> List.concat
        |> List.distinct
    let getReactionSignX (f:JointForce) = 
        let x = (getXFrom f.joint) - f.direction.X           
        match x < 0. with
        | true  -> -1.
        | false -> 1.
    let getReactionSignY (f:JointForce) =                 
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
    let analyzeNode (n:TrussNode) (t:Truss) (r:SupportReactionResult list) = 
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
    let getMemberForceAtJoint (j:Joint) (m:TrussPart) (mf:TrussMemberForce list) = 
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

    // Add a member directly from MemberBuilderControl
    let addMemberToTruss (m:Member) (t:Truss) = {t with members = m::t.members |> List.distinct}

    // Add a force directly from MemberBuilderControl
    let addJointForceToTruss (f:JointForce) (t:Truss) = {t with forces = f::t.forces |> List.distinct}

    // Add a force directly from MemberBuilderControl
    let addSupportToTruss (s:Support) (t:Truss) = {t with supports = s::t.supports |> List.distinct}

    // Workflow for building a member
    let makeMemberBuilderFrom (j:Joint) = BuilderDomain.MemberBuilder (j,None)
    let addJointToMemberBuilder (j:Joint) (mb:BuilderDomain.MemberBuilder) = 
        let a, _b = mb
        match a = j with 
        | true -> mb |> BuildMember |> TrussBuildOp
        | false -> (a,j) |> Member |> TrussPart
        
    // Workflow for building a force
    let makeForceBuilderFrom (j:Joint) = {_magnitude = None; _direction = None; joint = j}
    let addDirectionToForceBuilder (v:Vector) (fb:BuilderDomain.JointForceBuilder) = 
        match fb._magnitude.IsSome with
        | false -> {_magnitude = fb._magnitude; _direction = Some v; joint = fb.joint}  |> BuildForce |> TrussBuildOp
        | true -> {magnitude = fb._magnitude.Value; direction = v; joint = fb.joint} |> Force |> TrussPart
    let addMagnitudeToForceBuilder m (fb:JointForceBuilder) = 
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
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1 | _ -> acc) 0 truss.supports        
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
            let compareReactions (reactions:JointForce list) = 
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
        let r = List.fold (fun acc r -> match r with | Pin _-> acc + 2 | Roller _ -> acc + 1 | _ -> acc) 0 truss.supports
        let j = (getJointListFrom truss.members).Length
        match m + r = 2 * j && stability = [Stable] with
        | true  -> Determinate 
        | false -> Indeterminate

    let modifyTruss (truss:Truss) (newJ:Joint) (oldJ:Joint) =
        let newDir (oldDir:Vector) = 
            let newX = oldDir.X + (getXFrom newJ) - (getXFrom oldJ)
            let newY = oldDir.Y + (getYFrom newJ) - (getYFrom oldJ)
            Vector(newX,newY)
        let newMembers = 
            List.map (fun (j1,j2) -> 
                match j1 = oldJ with 
                | true -> (newJ,j2)
                | false -> 
                    match j2 = oldJ with
                    | true -> (j1,newJ)
                    | false -> (j1,j2)) truss.members
        let newForces = 
            List.map (fun f -> 
                match f.joint = oldJ with 
                | true -> {f with joint = newJ; direction = newDir f.direction}
                | false -> f) truss.forces
        let newSupports = 
            List.map (fun s  -> 
                match s with
                | Roller r -> 
                    match r.joint = oldJ with 
                    | true -> {r with joint = newJ; direction = newDir r.direction} |> Roller
                    | false -> Roller r
                | Pin p -> 
                    match p.normal.joint = oldJ with
                    | true -> {tangent = {p.tangent with joint = newJ; direction = newDir p.tangent.direction}; normal = {p.normal with joint = newJ; direction = newDir p.normal.direction}} |> Pin
                    | false -> Pin p
                | _ -> s
                ) truss.supports
        {truss with members = newMembers; forces = newForces; supports = newSupports}
    let modifyTrussForce (truss:Truss) (newFMag:float) (newFDir:float) (oldF:JointForce) =
        let newDir = Vector((getXFrom oldF.joint) + (50.0 * cos (newFDir * Math.PI/180.)),(getYFrom oldF.joint) - (50.0 * sin (newFDir * Math.PI/180.)))        
        let newForces = 
            List.map (fun f -> 
                match f = oldF with 
                | true -> {f with direction = newDir; magnitude = newFMag}
                | false -> f) truss.forces        
        {truss with forces = newForces}
    let modifyTrussSupport (truss:Truss) (newSDir:float) (oldS:Support) =
        let newS =             
            match oldS with
            | Pin p -> 
                let newNormalDir = Vector((getXFrom p.normal.joint) + (50.0 * cos ((newSDir - 90.) * Math.PI/180.)),(getYFrom p.normal.joint) - (50.0 * sin ((newSDir + 90.) * Math.PI/180.)))
                let newTangentDir = Vector((getXFrom p.tangent.joint) + (50.0 * cos (newSDir * Math.PI/180.)),(getYFrom p.tangent.joint) - (50.0 * sin ((newSDir) * Math.PI/180.)))
                Pin {tangent = {p.tangent with direction = newTangentDir}; normal = {p.normal with direction = newNormalDir}}
            | Roller r ->
                let newDir = Vector((getXFrom r.joint) + (50.0 * cos ((newSDir - 90.) * Math.PI/180.)),(getYFrom r.joint) - (50.0 * sin ((newSDir + 90.) * Math.PI/180.)))
                Roller {r with direction = newDir} 
            | _ -> oldS
        let newSupports = 
            List.map (fun s -> 
                match s = oldS with 
                | true -> newS
                | false -> s) truss.supports       
        {truss with supports = newSupports}

