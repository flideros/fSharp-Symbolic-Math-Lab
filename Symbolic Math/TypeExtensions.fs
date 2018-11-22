namespace Math

module TypeExtensions = 

    open System
    open System.Threading
    open System.Threading.Tasks

    type Microsoft.FSharp.Control.Async with
        static member Choice(tasks : Async<'b option> seq) : Async<'b option> = async {
            match Seq.toArray tasks with
            | [||] -> return None
            | [|t|] -> return! t
            | tasks ->
            let! t = Async.CancellationToken
            return! Async.FromContinuations <|
                fun (sc,ec,cc) ->
                    let noneCount = ref 0
                    let exnCount = ref 0
                    let innerCts = CancellationTokenSource.CreateLinkedTokenSource t
 
                    let scont (result : 'b option) =
                        match result with
                        | Some _ when Interlocked.Increment exnCount = 1 -> innerCts.Cancel() ; sc result
                        | None when Interlocked.Increment noneCount = tasks.Length -> sc None
                        | _ -> ()
 
                    let econt (exn : exn) =
                        if Interlocked.Increment exnCount = 1 then 
                            innerCts.Cancel() ; ec exn
 
                    let ccont (exn : OperationCanceledException) =
                        if Interlocked.Increment exnCount = 1 then
                            innerCts.Cancel(); cc exn
 
                    for task in (tasks) do
                        ignore <| Task.Factory.StartNew(fun () -> Async.StartWithContinuations(task, scont, econt, ccont, innerCts.Token))
        }