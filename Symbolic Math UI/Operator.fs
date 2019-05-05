namespace UI

module Operator =

    /// This operator is similar to (|>). 
    /// But, it returns argument as a return value.
    /// Then you can chain functions which returns unit.
    /// http://fssnip.net/9q
    let ($) x f = f x ; x
