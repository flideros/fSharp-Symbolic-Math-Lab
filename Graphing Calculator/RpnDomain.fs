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
    type ErrorStateData = {error:MathOperationError} 

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

    // Services used by the calculator itself
    type DoStackOperation = StackOperation * Stack -> CalculatorState
    type DoMathOperation = CalculatorMathOp * Stack -> CalculatorState
    
    type RpnServices = {        
        doStackOperation :DoStackOperation
        doMathOperation :DoMathOperation
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
(*        | Input i -> 
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
*)
