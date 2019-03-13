namespace BasicCalculator

(* Related blog post: http://fsharpforfunandprofit.com/posts/calculator-complete-v2/ *)

// ================================================
// Domain using a state machine
// ================================================          
module CalculatorDomain =
    
    // Calculators use numbers...Duh!
    type Number = float    
    
    // But, we store them in strings...go figure.
    type DigitAccumulator = string
    
    // Hummm..,something's missing...or, is it? 
    type NonZeroDigit = 
        | One  | Two | Three | Four 
        | Five | Six | Seven | Eight | Nine
    
    // Calculators perform math operations on nubers made of strings. (Mathematical String Theory)
    type CalculatorMathOp = 
        | Add 
        | Subtract 
        | Multiply 
        | Divide
        | Inverse 
        | Percent
        | Root
        | ChangeSign
        | MemoryAdd
        | MemorySubtract
    
    type PendingOp = (CalculatorMathOp * Number)
    
    // types to describe errors
    type MathOperationError = | DivideByZero    
    type MathOperationResult = 
        | Success of Number 
        | Failure of MathOperationError
    
    // data associated with each state    
    type AccumulatorStateData = {digits:DigitAccumulator; pendingOp:PendingOp option; memory:DigitAccumulator}
    type ComputedStateData = {displayNumber:Number; pendingOp:PendingOp option; memory:DigitAccumulator}    
    type ErrorStateData = {error:MathOperationError;memory:string}
    type ZeroStateData = {pendingOp:PendingOp option; memory:DigitAccumulator}  
    
    // five states        
    type CalculatorState =         
        | ZeroState of ZeroStateData 
        | AccumulatorState of AccumulatorStateData 
        | AccumulatorWithDecimalState of AccumulatorStateData 
        | ComputedState of ComputedStateData 
        | ErrorState of ErrorStateData
        
    // 10 inputs
    type CalculatorInput = 
        | Zero 
        | Digit of NonZeroDigit
        | DecimalSeparator
        | MathOp of CalculatorMathOp
        | Equals 
        | Clear
        | ClearEntry
        | Back
        | MemoryStore
        | MemoryClear
        | MemoryRecall
    
    type Calculate = CalculatorInput * CalculatorState -> CalculatorState
    
    // services used by the calculator itself
    type AccumulateNonZeroDigit = NonZeroDigit * DigitAccumulator -> DigitAccumulator 
    type AccumulateZero = DigitAccumulator -> DigitAccumulator 
    type AccumulateSeparator = DigitAccumulator -> DigitAccumulator 
    type DoMathOperation = CalculatorMathOp * Number * Number * DigitAccumulator -> MathOperationResult 
    type GetNumberFromAccumulator = AccumulatorStateData -> Number

    // services used by the UI or testing
    type GetDisplayFromState = CalculatorState -> string
    type GetPendingOpFromState = CalculatorState -> string
    type GetMemoFromState = CalculatorState -> string

    type CalculatorServices = {
        accumulateNonZeroDigit :AccumulateNonZeroDigit 
        accumulateZero :AccumulateZero 
        accumulateSeparator :AccumulateSeparator
        doMathOperation :DoMathOperation 
        getNumberFromAccumulator :GetNumberFromAccumulator 
        getDisplayFromState :GetDisplayFromState 
        getPendingOpFromState :GetPendingOpFromState 
        getMemoFromState :GetMemoFromState}

// ================================================
// Implementation of Calculator
// ================================================          
module CalculatorImplementation =
    open CalculatorDomain
    open Utilities

    let accumulateNonZeroDigit services digit accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateNonZeroDigit (digit, digits)
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let accumulateZero services accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateZero digits
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let accumulateSeparator services accumulatorData =
        let digits = accumulatorData.digits
        let newDigits = services.accumulateSeparator digits
        let newAccumulatorData = { accumulatorData with digits = newDigits }
        newAccumulatorData // return

    let getComputationState services (accumulatorStateData:AccumulatorStateData) nextOp = 

        // helper to create a new ComputedState from a given displayNumber 
        // and the nextOp parameter
        let getNewState displayNumber =
            let newPendingOp = 
                nextOp |> Option.map (fun op -> op, displayNumber )
            { displayNumber = displayNumber; pendingOp = newPendingOp; memory = accumulatorStateData.memory }
            |> ComputedState

        let currentNumber = 
            services.getNumberFromAccumulator accumulatorStateData 

        // If there is no pending op, create a new ComputedState using the currentNumber
        let computeStateWithNoPendingOp = 
            getNewState currentNumber 

        maybe {
            let! (op,previousNumber) = accumulatorStateData.pendingOp
            let result = services.doMathOperation(op,previousNumber,currentNumber,accumulatorStateData.memory)
            let newState =
                match result with
                | Success resultNumber ->
                    // If there was a pending op, create a new ComputedState using the result
                    getNewState resultNumber 
                | Failure error -> ErrorState {error=error;memory=""}
            return newState
            } |> ifNone computeStateWithNoPendingOp 

    let replacePendingOp (computedStateData:ComputedStateData) nextOp = 
        let newPending = maybe {
            let! existingOp,displayNumber  = computedStateData.pendingOp
            let! next = nextOp
            return next,displayNumber  
            }
        {computedStateData with pendingOp=newPending}
        |> ComputedState

    let handleZeroState services pendingOp input memory = 
        // create a new accumulatorStateData object that is used when transitioning to other states
        let accumulatorStateData = {digits = ""; pendingOp = pendingOp; memory = memory}
        match input with
        | Zero -> 
            ZeroState {pendingOp = pendingOp; memory = memory}// stay in ZeroState 
        | Digit digit -> 
            accumulatorStateData 
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorState  // transition to AccumulatorState  
        | DecimalSeparator -> 
            accumulatorStateData 
            |> accumulateSeparator services 
            |> AccumulatorWithDecimalState  // transition to AccumulatorWithDecimalState  
        | MathOp op -> 
            match op with
            | Divide
            | Multiply
            | Subtract
            | Add ->            
                let nextOp = Some op
                let newState = getComputationState services accumulatorStateData nextOp 
                newState  // transition to ComputedState or ErrorState
            | MemoryAdd        
            | MemorySubtract -> ZeroState {pendingOp = pendingOp; memory = memory}
            | ChangeSign -> AccumulatorState {digits = "-"; pendingOp = pendingOp; memory = memory}
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services accumulatorStateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | ClearEntry -> 
            ZeroState {pendingOp = pendingOp ; memory = memory} // transition to ZeroState and keep any pending ops
        | Clear -> 
            ZeroState {pendingOp = None ; memory = memory} // transition to ZeroState and throw away any pending ops      
        | Back ->
            ZeroState {pendingOp = pendingOp ; memory = memory} // stay in ZeroState        
        | MemoryStore ->
            ZeroState {pendingOp = pendingOp ; memory = "0"} // add zero to memory and stay in ZeroState   
        | MemoryClear ->
            ZeroState {pendingOp = pendingOp ; memory = ""} // clear memory and stay in ZeroState        
        | MemoryRecall -> 
            match memory = "" with
            | false -> 
                match memory.Contains(".") with
                | true -> AccumulatorState {digits = memory; pendingOp = pendingOp; memory = memory}
                | false -> AccumulatorWithDecimalState {digits = memory; pendingOp = pendingOp; memory = memory} //
            | true ->  ZeroState {pendingOp = pendingOp; memory = memory}// stay in ZeroState
            
    let handleAccumulatorState services stateData input = //memory = 
        match input with
        | Zero -> 
            stateData 
            |> accumulateZero services 
            |> AccumulatorState  // stay in AccumulatorState  
        | Digit digit -> 
            stateData 
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorState  // stay in AccumulatorState  
        | DecimalSeparator -> 
            stateData 
            |> accumulateSeparator services 
            |> AccumulatorWithDecimalState  // transition to AccumulatorWithDecimalState
        | MathOp op -> 
            match op with
            | Divide
            | Multiply
            | Subtract
            | Add ->            
                let nextOp = Some op
                let newState = getComputationState services stateData nextOp 
                newState  // transition to ComputedState or ErrorState
            | MemoryAdd -> 
                let newMemory = 
                    match System.Double.TryParse stateData.digits, System.Double.TryParse stateData.memory with
                    | (true, d),  (true, e) -> (d + e).ToString()
                    | (false,_), (true,  e) -> e.ToString()
                    | (true,d), (false, _) -> d.ToString()
                    | (false,_), (false, _) -> ""
                AccumulatorState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = newMemory}
            | MemorySubtract ->
                let newMemory = 
                    match System.Double.TryParse stateData.digits, System.Double.TryParse stateData.memory with
                    | (true, d),  (true, e) -> (e - d).ToString()
                    | (false,_), (true,  e) -> e.ToString()
                    | (true,d), (false, _) -> (-d).ToString()
                    | (false,_), (false, _) -> ""
                AccumulatorState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = newMemory}
            | ChangeSign -> 
                let newDigits = 
                    match stateData.digits.Contains("-") with
                    | true -> stateData.digits.Replace("-","")
                    | false -> stateData.digits.Insert(0,"-")
                AccumulatorState {digits = newDigits; pendingOp = stateData.pendingOp; memory = stateData.memory}
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | ClearEntry -> 
            ZeroState {pendingOp = stateData.pendingOp ; memory = stateData.memory} // transition to ZeroState and keep any pending ops
        | Clear -> 
            ZeroState {pendingOp = None ; memory = stateData.memory} // transition to ZeroState and throw away any pending ops      
        | Back ->            
            match stateData.digits.Length with
            | x when x <= 1 -> ZeroState {pendingOp = stateData.pendingOp ; memory = stateData.memory}
            | x -> AccumulatorState {digits = stateData.digits.Remove(x-1); pendingOp = stateData.pendingOp ; memory = stateData.memory} //        
        | MemoryStore ->
            AccumulatorState {digits = stateData.digits; pendingOp = stateData.pendingOp ; memory = stateData.digits} // store current digits in memory
        | MemoryClear ->
             AccumulatorState {digits = stateData.digits; pendingOp = stateData.pendingOp ; memory = ""} //       
        | MemoryRecall ->            
            let nextOp = match stateData.pendingOp with 
                         | Some (op,number) -> Some op
                         | None -> None           
            let accumulatorStateDataFromMemory = {digits = (match stateData.memory.Length with | 0 -> stateData.digits | _ -> stateData.memory); pendingOp = stateData.pendingOp; memory = stateData.memory}
            let newState = getComputationState services accumulatorStateDataFromMemory nextOp
            newState //  
    
    let handleAccumulatorWithDecimalState services stateData input = 
        match input with
        | Zero -> 
            stateData
            |> accumulateZero services 
            |> AccumulatorWithDecimalState // stay in AccumulatorWithDecimalState 
        | Digit digit -> 
            stateData
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorWithDecimalState  // stay in AccumulatorWithDecimalState 
        | DecimalSeparator -> 
            //ignore
            stateData 
            |> AccumulatorWithDecimalState  // stay in AccumulatorWithDecimalState 
        | MathOp op -> 
            match op with
            | Divide
            | Multiply
            | Subtract
            | Add ->            
                let nextOp = Some op
                let newState = getComputationState services stateData nextOp 
                newState  // transition to ComputedState or ErrorState
            | MemoryAdd -> 
                let newMemory = 
                    match System.Double.TryParse stateData.digits, System.Double.TryParse stateData.memory with
                    | (true, d),  (true, e) -> (d + e).ToString()
                    | (false,_), (true,  e) -> e.ToString()
                    | (true,d), (false, _) -> d.ToString()
                    | (false,_), (false, _) -> ""
                AccumulatorWithDecimalState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = newMemory}
            | MemorySubtract ->
                let newMemory = 
                    match System.Double.TryParse stateData.digits, System.Double.TryParse stateData.memory with
                    | (true, d),  (true, e) -> (e - d).ToString()
                    | (false,_), (true,  e) -> e.ToString()
                    | (true,d), (false, _) -> (-d).ToString()
                    | (false,_), (false, _) -> ""
                AccumulatorWithDecimalState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = newMemory}
            | ChangeSign -> 
                let newDigits = 
                    match stateData.digits.Contains("-") with
                    | true -> stateData.digits.Replace("-","")
                    | false -> stateData.digits.Insert(0,"-")
                AccumulatorWithDecimalState {digits = newDigits; pendingOp = stateData.pendingOp; memory = stateData.memory}
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | ClearEntry -> 
            ZeroState {pendingOp = stateData.pendingOp; memory = stateData.memory} // transition to ZeroState and keep any pending ops
        | Clear -> 
            ZeroState {pendingOp = None ; memory = stateData.memory} // transition to ZeroState and throw away any pending ops      
        | Back ->            
            match stateData.digits.Length with
            | x when x <= 1 -> ZeroState {pendingOp = stateData.pendingOp ; memory = stateData.memory}
            | x -> AccumulatorWithDecimalState {digits = stateData.digits.Remove(x-1); pendingOp = stateData.pendingOp ; memory = stateData.memory} //         
        | MemoryStore ->
            AccumulatorWithDecimalState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = stateData.digits} // store current digits in memory
        | MemoryClear ->
             AccumulatorWithDecimalState {digits = stateData.digits; pendingOp = stateData.pendingOp; memory = ""} //       
        | MemoryRecall ->            
            let nextOp = match stateData.pendingOp with 
                         | Some (op,number) -> Some op
                         | None -> None           
            let accumulatorStateDataFromMemory = {digits = (match stateData.memory.Length with | 0 -> stateData.digits | _ -> stateData.memory); pendingOp = stateData.pendingOp; memory = stateData.memory}
            let newState = getComputationState services accumulatorStateDataFromMemory nextOp
            newState //

    let handleComputedState services (stateData:ComputedStateData) input memory = 
        let emptyAccumulatorStateData = {digits=""; pendingOp=stateData.pendingOp; memory = memory}
        match input with
        | Zero -> 
            ZeroState {pendingOp = emptyAccumulatorStateData.pendingOp; memory = memory} // transition to ZeroState with any pending ops
        | Digit digit -> 
            emptyAccumulatorStateData 
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorState  // transition to AccumulatorState  
        | DecimalSeparator -> 
            emptyAccumulatorStateData 
            |> accumulateSeparator services 
            |> AccumulatorWithDecimalState  // transition to AccumulatorWithDecimalState  
        | MathOp op -> 
            match op with
            | Divide
            | Multiply
            | Subtract
            | Add ->            
                let nextOp = Some op 
                let newData = {displayNumber = stateData.displayNumber; pendingOp = Some (op,stateData.displayNumber); memory = stateData.memory}
                replacePendingOp newData nextOp
            | MemoryAdd -> 
                let newMemory = 
                    match stateData.displayNumber, System.Double.TryParse stateData.memory with
                    | d,  (true, e) -> (d + e).ToString()
                    | d, (false, _) -> d.ToString()                    
                ComputedState {displayNumber = stateData.displayNumber; pendingOp = stateData.pendingOp; memory = newMemory}
            | MemorySubtract ->
                let newMemory = 
                    match stateData.displayNumber, System.Double.TryParse stateData.memory with
                    | d,  (true, e) -> (e - d).ToString()
                    | d, (false, _) -> (-d).ToString()                    
                ComputedState {displayNumber = stateData.displayNumber; pendingOp = stateData.pendingOp; memory = newMemory}
            | ChangeSign ->  ComputedState {displayNumber = -1.0*(stateData.displayNumber); pendingOp = stateData.pendingOp; memory = stateData.memory}             
        | Equals -> 
            // replace the pending op, if any
            let nextOp = None
            replacePendingOp stateData nextOp 
        | ClearEntry -> 
            ZeroState {pendingOp = stateData.pendingOp ; memory = memory} // transition to ZeroState and keep any pending ops
        | Clear -> 
            ZeroState {pendingOp = None ; memory = memory} // transition to ZeroState and throw away any pending ops      
        | Back ->
            ZeroState {pendingOp = stateData.pendingOp ; memory = memory} // I can do better...later...
        | MemoryStore ->
            AccumulatorState {digits = stateData.displayNumber.ToString(); pendingOp = stateData.pendingOp ; memory = stateData.displayNumber.ToString()} // store current digits in memory
        | MemoryClear ->
             AccumulatorState {digits = stateData.displayNumber.ToString(); pendingOp = stateData.pendingOp ; memory = ""} //       
        | MemoryRecall ->            
            let nextOp = match stateData.pendingOp with 
                         | Some (op,_) -> Some op
                         | None -> None           
            let accumulatorStateDataFromMemory = {digits = (match memory.Length with | 0 -> stateData.displayNumber.ToString() | _ -> memory); pendingOp = stateData.pendingOp; memory = memory}
            let newState = getComputationState services accumulatorStateDataFromMemory nextOp
            newState //

    let handleErrorState stateData input memory =
        match input with
        | Zero 
        | Digit _ 
        | DecimalSeparator 
        | MathOp _ 
        | ClearEntry
        | Back
        | MemoryStore
        | MemoryClear
        | MemoryRecall
        | Equals -> 
            // stay in error state             
            ErrorState stateData
        | Clear -> 
            ZeroState {pendingOp = None ; memory = ""}  // transition to ZeroState and throw away any pending ops
        
    let createCalculate (services:CalculatorServices) : Calculate = 
        // create some local functions with partially applied services
        let handleZeroState = handleZeroState services
        let handleAccumulator = handleAccumulatorState services
        let handleAccumulatorWithDecimal = handleAccumulatorWithDecimalState services
        let handleComputed = handleComputedState services
        let handleError = handleErrorState 

        fun (input,state) -> 
            match state with
            | ZeroState stateData -> 
                handleZeroState stateData.pendingOp input stateData.memory
            | AccumulatorState stateData -> 
                handleAccumulator stateData input 
            | AccumulatorWithDecimalState stateData -> 
                handleAccumulatorWithDecimal stateData input
            | ComputedState stateData -> 
                handleComputed stateData input stateData.memory
            | ErrorState stateData -> 
                handleError stateData input stateData.memory

// ================================================
// Implementation of CalculatorServices 
// ================================================          
module CalculatorServices =
    open CalculatorDomain
    open Utilities

    let appendToAccumulator maxLen (accumulator:DigitAccumulator) appendCh = 
        // ignore new input if there are too many digits
        if (accumulator.Length > maxLen) then
            accumulator // ignore new input
        else
            // append the new char
            accumulator + appendCh

    let accumulateNonZeroDigit maxLen :AccumulateNonZeroDigit = 
        fun (digit, accumulator) ->

        // determine what character should be appended to the display
        let appendCh= 
            match digit with
            | One -> "1"
            | Two -> "2"
            | Three-> "3"
            | Four -> "4"
            | Five -> "5"
            | Six-> "6"
            | Seven-> "7"
            | Eight-> "8"
            | Nine-> "9"
        appendToAccumulator maxLen accumulator appendCh

    let accumulateZero maxLen :AccumulateZero = 
        fun accumulator -> appendToAccumulator maxLen accumulator "0"

    let accumulateSeparator maxLen :AccumulateSeparator = 
        fun accumulator ->
            let appendCh = 
                if accumulator = "" then "0." else "."
            appendToAccumulator maxLen accumulator appendCh

    let getNumberFromAccumulator :GetNumberFromAccumulator =
        fun accumulatorStateData ->
            let digits = accumulatorStateData.digits
            match System.Double.TryParse digits with
            | true, d -> d
            | false, _ -> 0.0

    let doMathOperation  :DoMathOperation = fun (op,f1,f2,memory) ->
        match op with
        | Add -> Success (f1 + f2)
        | Subtract -> Success (f1 - f2)
        | Multiply -> Success (f1 * f2)
        | Divide -> 
            if f2 = 0.0 then
                Failure DivideByZero 
            else
                Success (f1 / f2)
        | ChangeSign  -> Success (f1 * -1.)

    let getDisplayFromState divideByZeroMsg :GetDisplayFromState =
        
        // helper
        let floatToString = sprintf "%g" 
        
        fun calculatorState ->
            match calculatorState with
            | ZeroState _ -> "0"
            | AccumulatorState stateData 
            | AccumulatorWithDecimalState stateData -> 
                stateData 
                |> getNumberFromAccumulator 
                |> floatToString 
            | ComputedState stateData -> 
                stateData.displayNumber
                 |> floatToString 
            | ErrorState stateData -> 
                match stateData with
                | {error=DivideByZero;memory = _} -> divideByZeroMsg

    let getPendingOpFromState :GetPendingOpFromState=

        let opToString = function
            | Add -> "+"             
            | Subtract -> "-"
            | Multiply -> "*"  
            | Divide -> "/"
            | ChangeSign -> "(change sign)"

        let displayStringForPendingOp pendingOp =
            maybe {
                let! op, number = pendingOp 
                return sprintf "%g %s" number (opToString op)
                }
            |> defaultArg <| ""

        fun calculatorState ->
            match calculatorState with
            | ZeroState pendingOp -> 
                displayStringForPendingOp pendingOp.pendingOp 
            | AccumulatorState stateData 
            | AccumulatorWithDecimalState stateData -> 
                stateData.pendingOp 
                |> displayStringForPendingOp 
            | ComputedState stateData -> 
                stateData.pendingOp
                |> displayStringForPendingOp 
            | ErrorState _ -> 
                ""

    let getMemoFromState :GetMemoFromState =
        
        let getMemoFrom stateData =
            match stateData = "" with
            | true -> ""
            | false -> "M"

        fun calculatorState ->
            match calculatorState with
            | ZeroState stateData -> getMemoFrom stateData.memory
            | AccumulatorState stateData -> getMemoFrom stateData.memory
            | AccumulatorWithDecimalState stateData -> getMemoFrom stateData.memory
            | ComputedState stateData -> getMemoFrom stateData.memory
            | ErrorState stateData -> getMemoFrom stateData.memory


    let createServices () = {
        accumulateNonZeroDigit = accumulateNonZeroDigit (10)
        accumulateZero = accumulateZero (15)
        accumulateSeparator = accumulateSeparator (15)
        doMathOperation = doMathOperation
        getNumberFromAccumulator = getNumberFromAccumulator 
        getDisplayFromState = getDisplayFromState ("ERR-DIV0")
        getPendingOpFromState = getPendingOpFromState
        getMemoFromState = getMemoFromState }
