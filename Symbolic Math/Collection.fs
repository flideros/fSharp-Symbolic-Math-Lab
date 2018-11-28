namespace Math.Foundations.Logic

type Bag<'T> = 'T list

[<RequireQualifiedAccess>]
module Set =
    open Math.Pure.Objects
    open System.Collections.Generic
    open Math.Pure.Quantity
    open OpenMath
    
    //type Set<'T> = F# Collection Type Set
    
    // Content Dictionaries
    let private _omSet = GET.definitions "set1"    

    // Definition    
    let definition = GET.definitionEntry _omSet "set"

    module Size =

        let oF set = Integer (bigint(Set.count set))

        let oFSequence seq = 
            try 
            
            Integer (bigint(Seq.length seq))
            
            with
            | :? System.InvalidOperationException -> PositiveInfinity
                             
        // Definition
        let definition = Size.definition    

    module Map =

        let tO set func = Set.map func set     

        let tOList set func = List.map func set
                              |> Seq.distinct |> List.ofSeq

        // Definition
        let definition = Map.definition

    module Union =

        let oF left right = Set.union left right       
        
        let oFMany setList = List.fold (fun acc x -> Set.union acc x) Set.empty setList

        let oFList left right =
           List.append left right |> Seq.distinct |> List.ofSeq

        let oFManyLists lists =
           List.fold (fun acc x ->List.append acc x) [] lists |> Seq.distinct |> List.ofSeq

        // Definition
        let definition = Union.definition

    module Intersection = 
        
        let oF left right = Set.intersect left right

        let oFMany sets = Set.intersectMany sets

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> cache.Contains n)
            |> Seq.distinct |> List.ofSeq

        let oFManyLists lists = 
            List.reduce (fun acc x -> oFList acc x) lists

        // Definition
        let definition = Intersect.definition
    
    module Difference = 

        let oF left right = Set.difference left right

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> not (cache.Contains n))
            |> Seq.distinct |> List.ofSeq

        // Definition
        let definition = Setdiff.definition




[<RequireQualifiedAccess>]
module Bag =

    let countItem bag x = 
        match Seq.exists (fun x' -> x' = x) bag  with
        | true -> snd (Seq.find (fun x -> fst x = true) (Seq.countBy (fun elem -> elem = x) bag))
        | false -> 0

    let union b1 b2 = 
        let intersection = Set.Union.oFList b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b1 x)) 
                 | false -> (x,(countItem b2 x))) intersection 
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x)])

    let intersection b1 b2 = 
        let intersection = Set.Union.oFList b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b2 x))
                 | false -> (x,(countItem b1 x))) intersection
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x) ])