namespace GraphingCalculator

open GraphingCalculator.ConventionalDomain


module RpnDomain =

    // -------------Types
    type Stack = StackContents of Number list

    type StackOperation =    
        | Push       /// Pushes a value onto the stack.    
        | Drop       /// Removes the top value from the stack (if it exists).    
        | Duplicate  /// Pushes a duplicate copy of the top value onto the stack.    
        | ClearStack /// Clears the entire stack contents.    
        | Swap       /// Swaps the two top values on the stack.

    type CalculatorOp = 
        | StackOp of StackOperation
        | MathOp of CalculatorMathOp
        
    // Data associated with each state 
    type ReadyStateData = {stack : Stack}
    type DigitAccumulatorStateData = {digits:DigitAccumulator; stack : Stack}
    type ErrorStateData = {error:MathOperationError; currentstack : Stack} 

    type CalculatorInput =
        | Op of CalculatorOp
        | Input of ConventionalDomain.CalculatorInput
        | Enter 

    // States
    type CalculatorState =         
        | ReadyState of ReadyStateData
        | DigitAccumulatorState of DigitAccumulatorStateData
        | DecimalAccumulatorState of DigitAccumulatorStateData
        | ErrorState of ErrorStateData

    type Calculate = CalculatorInput * CalculatorState -> CalculatorState

    // Services used by the calculator itself
    type DoStackOperation = StackOperation * Stack -> CalculatorState
    type DoMathOperation = CalculatorMathOp * Stack -> CalculatorState
    
    type RpnServices = {        
        doStackOperation :DoStackOperation
        doMathOperation :DoMathOperation
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        }

module RpnImplementation =
    //open ConventionalDomain
    open RpnDomain

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

    let doStackOperation services stackData input =
        let stack = stackData.stack
        let newStack = services.doStackOperation (input,stack)
        newStack

    let doMathOperation services stack input =        
        let newStack = services.doMathOperation (input,stack) 
        newStack

    let handleReadyState services stack input =         
        let digitAccumulatorStateData = {digits = ""; stack = stack}
        match input with
        | Op op -> 
            match op with
            | StackOp s -> 
                match s with 
                | Push -> ReadyState {stack = stack} // stay in ReadyState  
                | Drop -> doStackOperation services digitAccumulatorStateData s
                | Duplicate -> doStackOperation services digitAccumulatorStateData s  
                | ClearStack -> ReadyState {stack = StackContents []}
                | Swap -> doStackOperation services digitAccumulatorStateData s  
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | Inverse 
                | Percent 
                | Root 
                | ChangeSign -> doMathOperation services stack m 
                | MemoryAdd -> ReadyState {stack = stack} // not used in RPN mode
                | MemorySubtract -> ReadyState {stack = stack} // not used in RPN mode
        | Input i -> 
            match i with
            | Zero -> ReadyState {stack = stack} // stay in ReadyState
            | Digit d -> 
                digitAccumulatorStateData 
                |> accumulateNonZeroDigit services d
                |> DigitAccumulatorState  // transition to AccumulatorState
            | DecimalSeparator -> 
                digitAccumulatorStateData 
                |> accumulateSeparator services
                |> DigitAccumulatorState  // transition to AccumulatorState 
            | Equals  -> ReadyState {stack = stack} // not used in RPN mode
            | Clear -> ReadyState {stack = stack} // not used in RPN mode
            | ClearEntry -> ReadyState {stack = stack} // ToDO
            | Back -> ReadyState {stack = stack} // ToDO
            | ConventionalDomain.MathOp op -> ReadyState {stack = stack} // not used in RPN mode
            | MemoryStore
            | MemoryClear
            | MemoryRecall -> ReadyState {stack = stack} // not used in RPN mode
        | Enter -> ReadyState {stack = stack} 

    let handleDigitAccumulatorState services stateData input =
        match input with
        | Op op -> 
            match op with
            | StackOp s -> 
                match s with 
                | Push -> doStackOperation services stateData s                    
                | Drop -> DigitAccumulatorState stateData
                | Duplicate -> DigitAccumulatorState stateData  
                | ClearStack -> DigitAccumulatorState stateData
                | Swap -> DigitAccumulatorState stateData  
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | Inverse 
                | Percent 
                | Root 
                | ChangeSign
                | MemoryAdd
                | MemorySubtract -> DigitAccumulatorState stateData
        | Input i -> 
            match i with
            | Zero -> 
                stateData 
                |> accumulateZero services 
                |> DigitAccumulatorState
            | Digit d -> 
                stateData
                |> accumulateNonZeroDigit services d
                |> DigitAccumulatorState  // transition to AccumulatorState
            | DecimalSeparator -> 
                stateData
                |> accumulateSeparator services
                |> DigitAccumulatorState  // transition to AccumulatorState 
            | Equals  -> DigitAccumulatorState stateData // not used in RPN mode
            | Clear -> DigitAccumulatorState stateData // not used in RPN mode
            | ClearEntry -> DigitAccumulatorState stateData // ToDO
            | Back -> DigitAccumulatorState stateData // ToDO
            | ConventionalDomain.MathOp op -> DigitAccumulatorState stateData // not used in RPN mode
            | MemoryStore
            | MemoryClear
            | MemoryRecall -> DigitAccumulatorState stateData // not used in RPN mode
        | Enter -> doStackOperation services stateData Push

    let handleDecimalAccumulatorState services stateData input =
        match input with
        | Op op -> 
            match op with
            | StackOp s -> 
                match s with 
                | Push -> doStackOperation services stateData s                    
                | Drop -> DecimalAccumulatorState stateData
                | Duplicate -> DecimalAccumulatorState stateData  
                | ClearStack -> DecimalAccumulatorState stateData
                | Swap -> DecimalAccumulatorState stateData  
            | MathOp m -> 
                match m with
                | Add
                | Subtract 
                | Multiply 
                | Divide 
                | Inverse 
                | Percent 
                | Root 
                | ChangeSign
                | MemoryAdd
                | MemorySubtract -> DecimalAccumulatorState stateData
        | Input i -> 
            match i with
            | Zero -> 
                stateData 
                |> accumulateZero services 
                |> DecimalAccumulatorState
            | Digit d -> 
                stateData
                |> accumulateNonZeroDigit services d
                |> DecimalAccumulatorState  // transition to AccumulatorState
            | DecimalSeparator -> DecimalAccumulatorState stateData // ignore 
            | Equals  -> DecimalAccumulatorState stateData // not used in RPN mode
            | Clear -> DecimalAccumulatorState stateData // not used in RPN mode
            | ClearEntry -> DecimalAccumulatorState stateData // ToDO
            | Back -> DecimalAccumulatorState stateData // ToDO
            | ConventionalDomain.MathOp op -> DecimalAccumulatorState stateData // not used in RPN mode
            | MemoryStore
            | MemoryClear
            | MemoryRecall -> DecimalAccumulatorState stateData // not used in RPN mode
        | Enter -> doStackOperation services stateData Push

    let handleErrorState stateData input =
        match input with
        | Op _
        | Input _ -> ErrorState stateData
        | Enter -> ReadyState {stack = stateData.currentstack}

    let createCalculate (services:RpnServices) : Calculate = 
        // create some local functions with partially applied services
        let handleReady = handleReadyState services
        let handleDigitAccumulator = handleDigitAccumulatorState services
        let handleDecimalAccumulator = handleDecimalAccumulatorState services
        let handleError = handleErrorState 

        fun (input,state) -> 
            match state with
            | ReadyState stateData -> 
                handleReady stateData.stack input
            | DigitAccumulatorState stateData -> 
                handleDigitAccumulator stateData input 
            | DecimalAccumulatorState stateData -> 
                handleDecimalAccumulator stateData input
            | ErrorState stateData -> 
                handleError stateData input

(**)
module RpnServices =
    open RpnDomain
    

