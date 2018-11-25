namespace Math.Foundations.Logic

type Bag<'T> = 'T list

[<RequireQualifiedAccess>]
module Set =

    open System.Collections.Generic
    open FSharp.Data
    open OpenMath
    
    //type Set<'T> = F# Collection Type Set
    
    // Definition
    let private _omSet = GET.definitions "set1"
    let definition = _omSet |> Array.tryFind (fun x -> x.Name = "set")

    // Definition
    let private _omSetName = GET.definitions "setname1"
    let createTheUniverse = _omSetName
    
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
                | R -> _omSetName |> Array.tryFind (fun (r : CDDefinition) -> r.Name = "R")
                | N -> _omSetName |> Array.tryFind (fun (n : CDDefinition) -> n.Name = "N")
                | Q -> _omSetName |> Array.tryFind (fun (q : CDDefinition) -> q.Name = "Q")
                | Z -> _omSetName |> Array.tryFind (fun (z : CDDefinition) -> z.Name = "Z")
                | C -> _omSetName |> Array.tryFind (fun (c : CDDefinition) -> c.Name = "C")
                | P -> _omSetName |> Array.tryFind (fun (p : CDDefinition) -> p.Name = "P")
                | EmptySet -> _omSet |> Array.tryFind (fun (p : CDDefinition) -> p.Name = "emptyset")

    module Union =

        let oF left right =
           Set.union left right |> Seq.distinct |> List.ofSeq       
        
        let oFList left right =
           List.append left right |> Seq.distinct |> List.ofSeq

        // Definition
        let private _omSet = GET.definitions "set1"
        let definition = _omSet |> Array.tryFind (fun x -> x.Name = "union")

    module Intersection = 
        
        let oF left right = Set.intersect left right

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> cache.Contains n)

        // Definition
        let private _omSet = GET.definitions "set1"
        let definition = _omSet |> Array.tryFind (fun x -> x.Name = "intersect")
    
    module Difference =

        let oF left right = Set.difference left right

        let oFList (left:list< 'a >) (right:list< 'a >) =
            let cache = HashSet< 'a >(right, HashIdentity.Structural)
            left |> List.filter (fun n -> not (cache.Contains n))

        // Definition
        let private _omSet = GET.definitions "set1"
        let definition = _omSet |> Array.tryFind (fun x -> x.Name = "setdiff")

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