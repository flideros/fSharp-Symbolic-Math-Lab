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
        | One | Two | Three | Four 
        | Five | Six | Seven | Eight | Nine
    
    // Calculators perform math operations on nubers made of strings. (Mathematical String Theory)
    type CalculatorMathOp = 
        | Add 
        | Subtract 
        | Multiply 
        | Divide    
    
    type PendingOp = (CalculatorMathOp * Number)
    
    // types to describe errors
    type MathOperationError = | DivideByZero    
    type MathOperationResult = 
        | Success of Number 
        | Failure of MathOperationError
    
    // data associated with each state
    type AccumulatorStateData = {digits:DigitAccumulator; pendingOp:PendingOp option}
    type ComputedStateData = {displayNumber:Number; pendingOp:PendingOp option}    
    type ErrorStateData = MathOperationError
    type ZeroStateData = PendingOp option    
    
    // five states        
    type CalculatorState = 
        | ZeroState of ZeroStateData 
        | AccumulatorState of AccumulatorStateData 
        | AccumulatorWithDecimalState of AccumulatorStateData 
        | ComputedState of ComputedStateData 
        | ErrorState of ErrorStateData
    
    // six inputs
    type CalculatorInput = 
        | Zero 
        | Digit of NonZeroDigit
        | DecimalSeparator
        | MathOp of CalculatorMathOp
        | Equals 
        | Clear
    
    type Calculate = CalculatorInput * CalculatorState -> CalculatorState
    
    // services used by the calculator itself
    type AccumulateNonZeroDigit = NonZeroDigit * DigitAccumulator -> DigitAccumulator 
    type AccumulateZero = DigitAccumulator -> DigitAccumulator 
    type AccumulateSeparator = DigitAccumulator -> DigitAccumulator 
    type DoMathOperation = CalculatorMathOp * Number * Number -> MathOperationResult 
    type GetNumberFromAccumulator = AccumulatorStateData -> Number

    // services used by the UI or testing
    type GetDisplayFromState = CalculatorState -> string
    type GetPendingOpFromState = CalculatorState -> string

    type CalculatorServices = {
        accumulateNonZeroDigit :AccumulateNonZeroDigit 
        accumulateZero :AccumulateZero 
        accumulateSeparator :AccumulateSeparator
        doMathOperation :DoMathOperation 
        getNumberFromAccumulator :GetNumberFromAccumulator 
        getDisplayFromState :GetDisplayFromState 
        getPendingOpFromState :GetPendingOpFromState 
        }

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

    let getComputationState services accumulatorStateData nextOp = 

        // helper to create a new ComputedState from a given displayNumber 
        // and the nextOp parameter
        let getNewState displayNumber =
            let newPendingOp = 
                nextOp |> Option.map (fun op -> op, displayNumber )
            {displayNumber=displayNumber; pendingOp = newPendingOp }
            |> ComputedState

        let currentNumber = 
            services.getNumberFromAccumulator accumulatorStateData 

        // If there is no pending op, create a new ComputedState using the currentNumber
        let computeStateWithNoPendingOp = 
            getNewState currentNumber 

        maybe {
            let! (op,previousNumber) = accumulatorStateData.pendingOp
            let result = services.doMathOperation(op,previousNumber,currentNumber)
            let newState =
                match result with
                | Success resultNumber ->
                    // If there was a pending op, create a new ComputedState using the result
                    getNewState resultNumber 
                | Failure error -> 
                    error |> ErrorState
            return newState
            } |> ifNone computeStateWithNoPendingOp 

    let replacePendingOp (computedStateData:ComputedStateData) nextOp = 
        let newPending = maybe {
            let! existing,displayNumber  = computedStateData.pendingOp
            let! next = nextOp
            return next,displayNumber  
            }
        {computedStateData with pendingOp=newPending}
        |> ComputedState

    let handleZeroState services pendingOp input = 
        // create a new accumulatorStateData object that is used when transitioning to other states
        let accumulatorStateData = {digits=""; pendingOp=pendingOp}
        match input with
        | Zero -> 
            ZeroState pendingOp // stay in ZeroState 
        | Digit digit -> 
            accumulatorStateData 
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorState  // transition to AccumulatorState  
        | DecimalSeparator -> 
            accumulatorStateData 
            |> accumulateSeparator services 
            |> AccumulatorWithDecimalState  // transition to AccumulatorWithDecimalState  
        | MathOp op -> 
            let nextOp = Some op
            let newState = getComputationState services accumulatorStateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services accumulatorStateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Clear -> 
            ZeroState None // transition to ZeroState and throw away any pending ops

    let handleAccumulatorState services stateData input = 
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
            let nextOp = Some op
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Clear -> 
            ZeroState None // transition to ZeroState and throw away any pending ops

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
            let nextOp = Some op
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Equals -> 
            let nextOp = None
            let newState = getComputationState services stateData nextOp 
            newState  // transition to ComputedState or ErrorState
        | Clear -> 
            ZeroState None // transition to ZeroState and throw away any pending ops

    let handleComputedState services stateData input = 
        let emptyAccumulatorStateData = {digits=""; pendingOp=stateData.pendingOp}
        match input with
        | Zero -> 
            ZeroState stateData.pendingOp  // transition to ZeroState with any pending ops
        | Digit digit -> 
            emptyAccumulatorStateData 
            |> accumulateNonZeroDigit services digit 
            |> AccumulatorState  // transition to AccumulatorState  
        | DecimalSeparator -> 
            emptyAccumulatorStateData 
            |> accumulateSeparator services 
            |> AccumulatorWithDecimalState  // transition to AccumulatorWithDecimalState  
        | MathOp op -> 
            // replace the pending op, if any
            let nextOp = Some op
            replacePendingOp stateData nextOp 
        | Equals -> 
            // replace the pending op, if any
            let nextOp = None
            replacePendingOp stateData nextOp 
        | Clear -> 
            ZeroState None // transition to ZeroState and throw away any pending ops

    let handleErrorState stateData input =
        match input with
        | Zero 
        | Digit _ 
        | DecimalSeparator 
        | MathOp _ 
        | Equals -> 
            // stay in error state             
            ErrorState stateData
        | Clear -> 
            ZeroState None // transition to ZeroState and throw away any pending ops
        
    let createCalculate (services:CalculatorServices) :Calculate = 
        // create some local functions with partially applied services
        let handleZeroState = handleZeroState services
        let handleAccumulator = handleAccumulatorState services
        let handleAccumulatorWithDecimal = handleAccumulatorWithDecimalState services
        let handleComputed = handleComputedState services
        let handleError = handleErrorState 

        fun (input,state) -> 
            match state with
            | ZeroState stateData-> 
                handleZeroState stateData input
            | AccumulatorState stateData -> 
                handleAccumulator stateData input
            | AccumulatorWithDecimalState stateData -> 
                handleAccumulatorWithDecimal stateData input
            | ComputedState stateData -> 
                handleComputed stateData input
            | ErrorState stateData -> 
                handleError stateData input

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
        fun accumulator ->
            let appendCh = "0"
            appendToAccumulator maxLen accumulator "0"

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

    let doMathOperation  :DoMathOperation = fun (op,f1,f2) ->
        match op with
        | Add -> Success (f1 + f2)
        | Subtract -> Success (f1 - f2)
        | Multiply -> Success (f1 * f2)
        | Divide -> 
            if f2 = 0.0 then
                Failure DivideByZero 
            else
                Success (f1 / f2)

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
                | DivideByZero -> divideByZeroMsg

    let getPendingOpFromState :GetPendingOpFromState=

        let opToString = function
            | Add -> "+" 
            | Subtract -> "-"
            | Multiply -> "*"  
            | Divide -> "/"

        let displayStringForPendingOp pendingOp =
            maybe {
                let! op, number = pendingOp 
                return sprintf "%g %s" number (opToString op)
                }
            |> defaultArg <| ""

        fun calculatorState ->
            match calculatorState with
            | ZeroState pendingOp -> 
                displayStringForPendingOp pendingOp 
            | AccumulatorState stateData 
            | AccumulatorWithDecimalState stateData -> 
                stateData.pendingOp 
                |> displayStringForPendingOp 
            | ComputedState stateData -> 
                stateData.pendingOp
                 |> displayStringForPendingOp 
            | ErrorState stateData -> 
                ""

    let createServices () = {
        accumulateNonZeroDigit = accumulateNonZeroDigit (10)
        accumulateZero = accumulateZero (15)
        accumulateSeparator = accumulateSeparator (15)
        doMathOperation = doMathOperation
        getNumberFromAccumulator = getNumberFromAccumulator 
        getDisplayFromState = getDisplayFromState ("ERR-DIV0")
        getPendingOpFromState = getPendingOpFromState }