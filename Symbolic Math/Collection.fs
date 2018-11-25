namespace Math.Foundations.Logic

type Bag<'T> = 'T list

[<RequireQualifiedAccess>]
module Set =

    open System.Collections.Generic
    open OpenMath
    
    //type Set<'T> = F# Collection Type Set
    
    // Content Dictionaries
    let private _omSet = GET.definitions "set1"
    let private _omSetName = GET.definitions "setname1"

    // Definition    
    let definition = GET.definitionEntry _omSet "set"

        
    /// <Definition>
    /// The Universe is the set of the sets of discorse
    /// https://math.stackexchange.com/a/435659
    /// </Definition>
    type Universe =
         | R /// Real
         | N /// Natural
         | Q /// Rational
         | Z /// Integral
         | C /// Complex
         | P /// Positive Primes
         | EmptySet 
         with member this.getDefinition = 
                match this with 
                | R -> GET.definitionEntry _omSetName "R"
                | N -> GET.definitionEntry _omSetName "N"
                | Q -> GET.definitionEntry _omSetName "Q"
                | Z -> GET.definitionEntry _omSetName "Z"
                | C -> GET.definitionEntry _omSetName "C"
                | P -> GET.definitionEntry _omSetName "P"
                | EmptySet -> GET.definitionEntry _omSet "emptyset"

    let createTheUniverse = _omSetName

    module Union =

        let oF left right =
           Set.union left right |> Seq.distinct |> List.ofSeq       
        
        let oFList left right =
           List.append left right |> Seq.distinct |> List.ofSeq

        // Definition
        let definition = GET.definitionEntry _omSet "union"

    module Intersection = 
        
        let oF left right = Set.intersect left right

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> cache.Contains n)

        // Definition
        let definition = GET.definitionEntry _omSet "intersect"
    
    module Difference =

        let oF left right = Set.difference left right

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> not (cache.Contains n))

        // Definition
        let definition = GET.definitionEntry _omSet "setdiff"

[<RequireQualifiedAccess>]
module Bag =

    let countItem bag x = 
        match Seq.exists (fun x' -> x' = x) bag  with
        | true -> snd (Seq.find (fun x -> fst x = true) (Seq.countBy (fun elem -> elem = x) bag))
        | false -> 0

    let union b1 b2 = 
        let intersection = Set.Union.oF b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b1 x))
                 | false -> (x,(countItem b2 x))) intersection
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x)])

    let intersection b1 b2 = 
        let intersection = Set.Union.oF b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b2 x))
                 | false -> (x,(countItem b1 x))) intersection
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x) ])