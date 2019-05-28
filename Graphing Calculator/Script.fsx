#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#load "Utilities.fs"
#load "ConventionalDomain.fs"

open GraphingCalculator.ConventionalDomain

// ==============================================
// Types
// ==============================================

type Stack = StackContents of Number list


type StackOperation =    
    | Push       // Pushes a value onto the stack.    
    | Drop       // Removes the top value from the stack (if it exists).    
    | Duplicate  // Pushes a duplicate copy of the top value onto the stack.    
    | ClearStack // Clears the entire stack contents.    
    | Swap       // Swaps the two top values on the stack.

// data associated with each state 
type EmptyStackStateData = {stack : Stack}
type DigitAccumulatorStateData = {digits:DigitAccumulator; stack : Stack}
type ErrorStateData = {error:MathOperationError} 

type CalculatorState =         
    | EmptyStack of EmptyStackStateData
    | DigitAccumulator of DigitAccumulatorStateData
    | ErrorStateData of ErrorStateData

// ==============================================
// Stack primitives
// ==============================================

/// Push a value on the stack
let push x (StackContents contents) =   
    StackContents (x::contents)

/// Pop a value from the stack and return it 
/// and the new stack as a tuple
let pop (StackContents contents) = 
    match contents with 
    | top::rest -> 
        let newStack = StackContents rest
        (top,newStack)
    | [] -> 
        failwith "Stack underflow"

// ==============================================
// Operator core
// ==============================================

// pop the top two elements
// do a binary operation on them
// push the result 
let binary mathFn stack = 
    let y,stack' = pop stack    
    let x,stack'' = pop stack'  
    let z = mathFn x y
    push z stack''      

// pop the top element
// do a unary operation on it
// push the result 
let unary f stack = 
    let x,stack' = pop stack  
    push (f x) stack'         

// ==============================================
// Other core 
// ==============================================

/// Pop and show the top value on the stack
let SHOW stack = 
    let x,_ = pop stack
    printfn "The answer is %f" x
    stack  // keep going with same stack

/// Duplicate the top value on the stack
let DUP stack = 
    let x,s = pop stack  
    push x (push x s)   
    
/// Swap the top two values
let SWAP stack = 
    let x,s = pop stack  
    let y,s' = pop s
    push y (push x s')   

/// Drop the top value on the stack
let DROP stack = 
    let _,s = pop stack  //pop the top of the stack
    s                    //return the rest

// ==============================================
// Words based on primitives
// ==============================================

// Constants
// -------------------------------
let EMPTY = StackContents []
let START  = EMPTY


// Numbers
// -------------------------------
let ONE = push 1.0
let TWO = push 2.0
let THREE = push 3.0
let FOUR = push 4.0
let FIVE = push 5.0

// Math functions
// -------------------------------
let ADD = binary (+)
let SUB = binary (-)
let MUL = binary (*)
let DIV = binary (/)

let NEG = unary (fun x -> -x)


// ==============================================
// Words based on composition
// ==============================================

let SQUARE =  
    DUP >> MUL 

let CUBE = 
    DUP >> DUP >> MUL >> MUL 

let SUM_NUMBERS_UPTO = 
    DUP      // n, n 2 items on stack
    >> ONE   // n, n, 1 3 items on stack 
    >> ADD   // n, (n+1) 2 items on stack
    >> MUL   // n(n+1) 1 item on stack
    >> TWO   // n(n+1), 2 2 items on stack 
    >> DIV   // n(n+1)/2 1 item on stack
