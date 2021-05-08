namespace Math.Presentation

open System
open System.IO

module WolframCodeBlock =

    let animationWindow =
        let sr = new StreamReader("AnimationWindow.txt")
        let out = try sr.ReadToEnd() finally "" |> ignore
        out

    let asteroids = fun () ->
        let sr = new StreamReader("AsteroidsGame.txt")
        let out = try sr.ReadToEnd() finally "" |> ignore
        out
    
    let testCode = 
        let sr = new StreamReader("TestCode.txt")
        let out = try sr.ReadToEnd() finally "" |> ignore
        out

    
