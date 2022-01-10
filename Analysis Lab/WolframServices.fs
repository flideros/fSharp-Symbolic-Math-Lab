namespace Math.Presentation.WolframEngine.Analysis

module WolframServices =
    let test = "Solve[{0*Ry0 + 3*Ry1 + -150 == 0,-3*Ry0 + 0*Ry1 + -105 == 0,-1*Rx0 + 0*Rx1 + 0 == 0,-1*Ry0 + -1*Ry1 + 15 == 0}, { Ry0, Ry1,Rx1,Rx0}]"
    let testResult = "{{Ry0 -> -13.2124, Rx1 -> 70.7107, Ry1 -> 100.561}}"

    // create helper functions to assemble Wolfram code
    
    let floatToString (f:float) = 
        let f' = f.ToString()
        match String.exists (fun x -> x = '.') f' with
        | true -> f'
        | false -> f' + ".0"

    let createEquation (constant:float) (variables:(float*string) list) = 
        let variableExpressions = List.fold (fun acc x-> acc + floatToString(fst x) + "*" + (snd x) + " + ") "" variables
        variableExpressions + (floatToString constant) + " == 0.0"
    
    let solveEquations (eq:string list) (v:string list) = 
        let eq' = List.filter (fun x -> x<>"") eq
        "DecimalForm[Solve[{" + 
        (List.fold (fun acc x-> match acc with | "" -> x | _ -> acc + "," + x) "" eq') + "},{" +
        (List.fold (fun acc x-> match acc with | "" -> x | _ -> acc + "," + x) "" v) + "}]]"

    // create helper functions to parse Wolfram code

    let parseResults (r:string) = 
        let r' = r |> String.filter (fun c -> (c = '{') = false) |> String.filter (fun c -> (c = '}') = false)
        let result = 
            match r with 
            | "{}" -> ["Unable to compute result"]
            |  "" -> ["Unable to compute result"]
            |  _ -> Array.toList (r'.Split(','))
        let parseResult (r:string) =
            let r' = r.Trim()
            let index = 
                match r.Contains("->") with
                | false -> false,-1
                | true -> 
                    match r.Contains("M")  with
                    | true -> r'.Substring(1,r.IndexOf(" ->")-1) |> System.Int32.TryParse
                    | false -> r'.Substring(2,r.IndexOf(" ->")-2) |> System.Int32.TryParse
            let part = 
                match r.Contains("->") with
                | false -> "Unable to compute result"
                | true -> 
                    match r.Contains("M") with
                    | true -> 
                        let out = r'.Substring(0,1)
                        match out.Contains("M") with
                        | true -> "Unable to compute result"
                        | false -> out
                    | false -> r'.Substring(0,2)
            let magnitude = 
                match r.Contains("->") with
                | false -> false,0.0
                | true -> r'.Substring(r.LastIndexOf(">")+1) |> System.Double.TryParse
            match r' with 
            | "Unable to compute result" -> (-1,"Unable to compute result",0.0)
            | _ -> (snd index),part,(snd magnitude)
        List.map parseResult result

