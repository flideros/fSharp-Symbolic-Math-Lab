namespace Math.Pure.Objects

type Constant =
    | E
    | I
    | Pi
    with
    member this.symbol = 
        match this with 
        | E -> "e"
        | I -> "i"
        | Pi -> "π"
    
type Symbol = 
    | Constant of Constant
    | Variable of string
    | Inconsistent
    
type Result<'T> =
    | Pass of 'T
    | Fail
    with
    member this.value = 
        let v = match this with
                | Pass t -> Some t
                | Fail -> None
        v.Value
