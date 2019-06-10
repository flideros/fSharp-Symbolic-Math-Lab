namespace GraphingCalculator

open GraphingCalculator.ConventionalDomain


module RpnDomain =

    // -------------Types
    type Stack = StackContents of Number list

    type StackOperation =    
        | Push       /// Pushes a value onto the stack.    
        | Pop        /// Pop a value from the stack and return it and the new stack as a tuple
        | Drop       /// Removes the top value from the stack (if it exists).    
        | Duplicate  /// Pushes a duplicate copy of the top value onto the stack.    
        | ClearStack /// Clears the entire stack contents.    
        | Swap       /// Swaps the two top values on the stack.

    type CalculatorOp = 
        | StackOp of StackOperation
        | MathOp  of CalculatorMathOp
    
    // types to describe errors
    type OperationError = 
        | DivideByZero of Stack
        | UnknownError of Stack
        | EmptyStack of Stack
        | TooFewArguments of Stack

    type OperationResult = 
        | Success of Stack 
        | Failure of OperationError

    // Data associated with each state 
    type ReadyStateData = {stack : Stack}
    type DigitAccumulatorStateData = {digits:DigitAccumulator; stack : Stack}
    type ErrorStateData = {error:OperationError} 

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
    type DoStackOperation = StackOperation * Number option * Stack -> OperationResult
    type DoMathOperation = CalculatorMathOp * Stack -> OperationResult
    type GetNumberFromAccumulator = DigitAccumulatorStateData -> Number
    
    type RpnServices = {        
        doStackOperation :DoStackOperation
        doMathOperation :DoMathOperation
        accumulateZero :AccumulateZero
        accumulateNonZeroDigit :AccumulateNonZeroDigit
        accumulateSeparator :AccumulateSeparator
        getNumberFromAccumulator :GetNumberFromAccumulator
        }

module StackOperations =
    open RpnDomain

    /// Push a value on the stack
    let push x (StackContents contents) = StackContents (x::contents)
    
    /// Pop a value from the stack and return it and the new stack as a tuple
    let pop (StackContents contents) = 
        match contents with 
        | top::rest -> 
            let newStack = StackContents rest
            (top,newStack)
        | [] -> (nan, (StackContents contents))
    
    /// Removes the top value from the stack (if it exists).
    let drop (StackContents contents) =
        match contents with 
        | top::rest -> StackContents rest           
        | [] -> StackContents contents

    /// Duplicate the top value on the stack
    let duplicate (StackContents contents) =
        match contents with 
        | top::rest -> push top (StackContents contents)          
        | [] -> StackContents contents   
        
    /// Swap the top two values
    let swap (StackContents contents) =
        match contents with 
        | top::next::rest -> StackContents (next::top::rest)         
        | [_] | [] -> StackContents contents   
    
    let clearStack =  StackContents []

module RpnImplementation =    
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

    let doStackOperation services state input =
        match state with
        | ReadyState r ->
            let stack = r.stack
            let newStack = services.doStackOperation (input, None ,stack)
            match newStack with
            | Success stackData -> ReadyState {stack = stackData}
            | Failure errorData -> ErrorState {error = errorData}
        | DigitAccumulatorState d ->
            let stack = d.stack
            let number = services.getNumberFromAccumulator d
            let newStack = services.doStackOperation (input, Some(number), stack)
            match newStack with
            | Success stackData -> ReadyState {stack = stackData}
            | Failure errorData -> ErrorState {error = errorData}
        | DecimalAccumulatorState d ->
            let stack = d.stack
            let number = services.getNumberFromAccumulator d
            let newStack = services.doStackOperation (input, Some(number), stack)            
            match newStack with
            | Success stackData -> ReadyState {stack = stackData}
            | Failure errorData -> ErrorState {error = errorData}
        | ErrorState e ->            
            let stack = 
                match e.error with
                | DivideByZero x
                | UnknownError x
                | EmptyStack x
                | TooFewArguments x -> x 
            let newStack = services.doStackOperation (input, None, stack)
            match newStack with
            | Success stackData -> ReadyState {stack = stackData}
            | Failure errorData -> ErrorState {error = errorData}

    let doMathOperation services stack input =        
        let result = services.doMathOperation (input,stack) 
        match result with
        | Success x -> ReadyState {stack = x}
        | Failure x -> ErrorState {error = x}
    
    let handleReadyState services stack input =
        match input with
        | Op op -> 
            match op with
            | StackOp s -> 
                match s with 
                | ClearStack
                | Push -> ReadyState {stack = stack} // stay in ReadyState 
                | Pop
                | Drop
                | Duplicate                 
                | Swap -> doStackOperation services (ReadyState {stack = stack}) s
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
                {digits=""; stack = stack} 
                |> accumulateNonZeroDigit services d
                |> DigitAccumulatorState  // transition to AccumulatorState
            | DecimalSeparator -> 
                {digits=""; stack = stack} 
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
            | StackOp stackOp ->
                let state = 
                    match stateData with
                    | DigitAccumulatorState x -> DigitAccumulatorState x
                    | DecimalAccumulatorState x -> DecimalAccumulatorState x
                    | ReadyState x -> ErrorState {error = (UnknownError x.stack)}
                    | ErrorState e -> ErrorState e
                doStackOperation services state stackOp
            | MathOp mathOp -> 
                let stack = 
                    match stateData with
                    | DigitAccumulatorState x -> x.stack
                    | DecimalAccumulatorState x -> x.stack
                    | ReadyState x -> x.stack
                    | ErrorState e -> 
                        match e.error with
                        | DivideByZero x
                        | UnknownError x
                        | EmptyStack x
                        | TooFewArguments x -> x
                doMathOperation services stack mathOp
        | Input i -> 
            let digits = 
                match stateData with
                | DigitAccumulatorState x -> {digits = x.digits; stack = x.stack}
                | DecimalAccumulatorState x -> {digits = x.digits; stack = x.stack}
                | ReadyState x -> {digits = ""; stack = x.stack}
                | ErrorState e -> 
                    match e.error with
                    | DivideByZero x
                    | UnknownError x
                    | EmptyStack x
                    | TooFewArguments x -> {digits = ""; stack = x}
            match i with
            | Zero -> DigitAccumulatorState (accumulateZero services digits)
            | Digit d -> DigitAccumulatorState (accumulateNonZeroDigit services d digits)
            | DecimalSeparator -> DecimalAccumulatorState (accumulateSeparator services digits)
            // ToDO
            | ClearEntry -> DigitAccumulatorState digits 
            | Back -> DigitAccumulatorState digits             
            // not used in RPN mode
            | ConventionalDomain.MathOp op -> DigitAccumulatorState digits
            | Equals   
            | Clear 
            | MemoryStore
            | MemoryClear
            | MemoryRecall -> DigitAccumulatorState digits
        | Enter -> doStackOperation services stateData Push

    let handleDecimalAccumulatorState services stateData input =
        match input with
        | Op op -> 
            match op with
            | StackOp stackOp ->
                let state = 
                    match stateData with
                    | DigitAccumulatorState x -> DigitAccumulatorState x
                    | DecimalAccumulatorState x -> DecimalAccumulatorState x
                    | ReadyState x -> ErrorState {error = (UnknownError x.stack)}
                    | ErrorState e -> ErrorState e
                doStackOperation services state stackOp
            | MathOp mathOp -> 
                let stack = 
                    match stateData with
                    | DigitAccumulatorState x -> x.stack
                    | DecimalAccumulatorState x -> x.stack
                    | ReadyState x -> x.stack
                    | ErrorState e -> 
                        match e.error with
                        | DivideByZero x
                        | UnknownError x
                        | EmptyStack x
                        | TooFewArguments x -> x
                doMathOperation services stack mathOp
        | Input i -> 
            let digits = 
                match stateData with
                | DigitAccumulatorState x -> {digits = x.digits; stack = x.stack}
                | DecimalAccumulatorState x -> {digits = x.digits; stack = x.stack}
                | ReadyState x -> {digits = ""; stack = x.stack}
                | ErrorState e -> 
                    match e.error with
                    | DivideByZero x
                    | UnknownError x
                    | EmptyStack x
                    | TooFewArguments x -> {digits = ""; stack = x}
            match i with
            | Zero -> DecimalAccumulatorState (accumulateZero services digits)
            | Digit d -> DecimalAccumulatorState (accumulateNonZeroDigit services d digits)
            | DecimalSeparator -> DecimalAccumulatorState (accumulateSeparator services digits)
            // ToDO
            | ClearEntry -> DecimalAccumulatorState digits 
            | Back -> DecimalAccumulatorState digits             
            // not used in RPN mode
            | ConventionalDomain.MathOp op -> DecimalAccumulatorState digits
            | Equals   
            | Clear 
            | MemoryStore
            | MemoryClear
            | MemoryRecall -> DecimalAccumulatorState digits
        | Enter -> doStackOperation services stateData Push

    let handleErrorState stateData input =
        match input with
        | Op _
        | Input _ -> ErrorState stateData
        | Enter -> 
            let errorStack =
                match stateData.error with
                | DivideByZero e
                | UnknownError e
                | EmptyStack e -> e
            ReadyState {stack = errorStack}

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
                handleDigitAccumulator (DigitAccumulatorState stateData) input 
            | DecimalAccumulatorState stateData -> 
                handleDecimalAccumulator (DecimalAccumulatorState stateData) input
            | ErrorState stateData -> 
                handleError stateData input

(**)
module RpnServices =
    open RpnDomain
    open StackOperations

    let accumulateZero maxLen :AccumulateZero = 
        fun accumulator -> CalculatorServices.appendToAccumulator maxLen accumulator "0"

    let accumulateNonZeroDigit maxLen :AccumulateNonZeroDigit = 
        fun (digit, accumulator) ->

        // determine what character should be appended to the display
        let appendCh = 
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
        CalculatorServices.appendToAccumulator maxLen accumulator appendCh
    
    let accumulateSeparator maxLen :AccumulateSeparator = 
        fun accumulator ->
            let appendCh = 
                if accumulator = "" then "0." else "."
            CalculatorServices.appendToAccumulator maxLen accumulator appendCh

    let getNumberFromAccumulator :GetNumberFromAccumulator =
        fun accumulatorStateData ->
            let digits = accumulatorStateData.digits
            match System.Double.TryParse digits with
            | true, d -> d
            | false, _ -> 0.0 

    let doMathOperation  :DoMathOperation = fun (op,stack) ->
        let stackContents = match stack with | StackContents s -> s
        match stackContents.Length with
        | x when x = 0 -> Failure (EmptyStack stack)
        | x when x = 1 -> 
            let f1, toStack = pop stack 
            match op with            
            | Percent -> 
                let calc = f1/100.                
                Success (push calc toStack) 
            | Root -> Failure (TooFewArguments stack)
            | Inverse when f1 = 0. -> Failure (DivideByZero stack)
            | Inverse -> Failure (EmptyStack stack)
            | Add -> Failure (TooFewArguments stack)
            | Subtract -> Failure (TooFewArguments stack)
            | Multiply -> Failure (TooFewArguments stack)            
            | Divide -> Failure (TooFewArguments stack)       
            | ChangeSign  -> 
                let calc = f1  * -1.                
                Success (push calc toStack)
            | _ -> Failure (UnknownError stack)
        | x -> 
            let f1, toStack1 = pop stack 
            let f2, toStack2 = pop toStack1            
            match op with
            | Percent -> 
                let calc = f1/100.                
                Success (push calc toStack1)
            | Root -> 
                let calc = (System.Math.Pow(f1,f2))                
                Success (push calc toStack2)
            | Inverse when f1 = 0. -> Failure (DivideByZero stack)
            | Inverse -> 
                let calc = f2 / f1                
                Success (push calc toStack2)
            | Add -> 
                let calc = f1 + f2                
                Success (push calc toStack2)
            | Subtract -> 
                let calc = f1 - f2                
                Success (push calc toStack2)
            | Multiply -> 
                let calc = f1 * f2                
                Success (push calc toStack2)
            | Divide -> 
                let calc = f1 / f2                
                Success (push calc toStack2)
            | ChangeSign  -> 
                let calc = f1  * -1.                
                Success (push calc toStack1)
            | _ -> Failure (UnknownError stack)

    let doStackOperation :DoStackOperation = fun (op,number,StackContents stack) ->        
        
        match stack.Length with
        | x when x = 0 -> 
            match op with
            | Push -> 
                match number with 
                | Some n -> Success (push n (StackContents stack))
                | None -> Failure (TooFewArguments (StackContents stack))
            | ClearStack -> Success (StackContents [])
            | Drop -> Failure (EmptyStack (StackContents stack))
            | Duplicate -> Failure (EmptyStack (StackContents stack))            
            | Swap -> Failure (TooFewArguments (StackContents stack))
            | _ -> Failure (UnknownError (StackContents stack))
        | x when x = 1 -> 
            let f1 = stack.Head 
            match op with
            | Push -> 
                match number with 
                | Some n -> Success (push n (StackContents stack))
                | None -> Failure (TooFewArguments (StackContents stack))
            | Drop -> Success (StackContents [])
            | Duplicate -> 
                let newStack = f1::stack
                Success (StackContents newStack)
            | ClearStack -> Success (StackContents [])
            | Swap -> Failure (TooFewArguments (StackContents stack))
            | _ -> Failure (UnknownError (StackContents stack))
        | x -> 
            let f1 = stack.[0] 
            let f2 = stack.[1]
            match op with
            | Push -> 
                match number with 
                | Some n -> Success (push n (StackContents stack))
                | None -> Failure (TooFewArguments (StackContents stack))
            | Drop -> 
                let newStack = match stack with | x::rest -> rest | [] -> []
                Success (StackContents newStack)
            | Duplicate -> 
                let newStack = f1::stack
                Success (StackContents newStack)            
            | Swap -> 
                let newStack = f2::(f1::stack)
                Success (StackContents newStack)
            | ClearStack -> Success (StackContents [])
            | _ -> Failure (UnknownError (StackContents stack))

    let createServices () = {
        accumulateNonZeroDigit = accumulateNonZeroDigit (10)
        accumulateZero = accumulateZero (15)
        accumulateSeparator = accumulateSeparator (15)
        doMathOperation = doMathOperation 
        doStackOperation = doStackOperation
        getNumberFromAccumulator = getNumberFromAccumulator}
