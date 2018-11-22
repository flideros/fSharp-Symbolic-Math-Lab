namespace Math.Foundations.Logic

type Bag<'T> = 'T list

[<RequireQualifiedAccess>]
module Set =

    open System.Collections.Generic
    open FSharp.Data
    open OpenMath

    let private _omSetName = GET.definitions "setname1"
    
    type Universe =
         | R //Real
         | N // Natural
         | Q // Rational
         | Z // Integral
         | C // Complex
         | P // Positive Primes
         with member this.getDefinition = 
                match this with 
                | R -> _omSetName |> Array.tryFind (fun (r : CDDefinition) -> r.Name = "R")
                | N -> _omSetName |> Array.tryFind (fun (n : CDDefinition) -> n.Name = "N")
                | Q -> _omSetName |> Array.tryFind (fun (q : CDDefinition) -> q.Name = "Q")
                | Z -> _omSetName |> Array.tryFind (fun (z : CDDefinition) -> z.Name = "Z")
                | C -> _omSetName |> Array.tryFind (fun (c : CDDefinition) -> c.Name = "C")
                | P -> _omSetName |> Array.tryFind (fun (p : CDDefinition) -> p.Name = "P")

    let union left right =
        List.append left right |> Seq.distinct |> List.ofSeq

    let intersection (left:list< 'a >) (right:list< 'a >) =
        let cache = HashSet< 'a >(right, HashIdentity.Structural)
        left |> List.filter (fun n -> cache.Contains n)

    let difference (left:list< 'a >) (right:list< 'a >) =
        let cache = HashSet< 'a >(right, HashIdentity.Structural)
        left |> List.filter (fun n -> not (cache.Contains n))

[<RequireQualifiedAccess>]
module Bag =

    let countItem bag x = 
        match Seq.exists (fun x' -> x' = x) bag  with
        | true -> snd (Seq.find (fun x -> fst x = true) (Seq.countBy (fun elem -> elem = x) bag))
        | false -> 0

    let union b1 b2 = 
        let intersection = Set.union b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b1 x))
                 | false -> (x,(countItem b2 x))) intersection
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x)])

    let intersection b1 b2 = 
        let intersection = Set.union b1 b2
        List.map (fun x -> 
                 match (countItem b1 x) > (countItem b2 x) with
                 | true -> (x,(countItem b2 x))
                 | false -> (x,(countItem b1 x))) intersection
        |> List.collect (fun x -> [for i in 1..(snd x) -> (fst x) ])