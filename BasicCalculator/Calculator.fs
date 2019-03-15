namespace BasicCalculator

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes  
open System.IO
open System.Windows.Markup
open System.Windows.Controls 
open System.Reflection
open System.Windows.Media.Imaging
open Utilities
open CalculatorDomain


type Calculator() as calculator =     
    inherit UserControl()
    
    // ------Create Types---------        
    //Colors 
    let gridColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xF6", byte "0xF7", byte "0xF9"))
    let buttonColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xFB", byte "0xFB", byte "0xFB"))
    let backgroundColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xF6", byte "0xF7", byte "0xF9"))
    
    //Buttons     
    let one = 
        Button(Name = "oneButton",
                Content = "1",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //    
    let two = 
        Button(Name = "twoButton",
                Content = "2",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor)
    let three = 
        Button(Name = "threeButton",
                Content = "3",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let four = 
        Button(Name = "fourButton",
                Content = "4",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let five = 
        Button(Name = "fiveButton",
                Content = "5",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let six = 
        Button(Name = "sixButton",
                Content = "6",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let seven = 
        Button(Name = "sevenButton",
                Content = "7",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let eight = 
        Button(Name = "eightButton",
                Content = "8",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let nine = 
        Button(Name = "nineButton",
                Content = "9",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let zero = 
        Button(Name = "zeroButton",
                Content = "0",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 71.,
                Height = 33.,
                Background = buttonColor) //
    let decimalPoint = 
        Button(Name = "decimalPointButton",
                Content = ".",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let add = 
        Button(Name = "addButton",
                Content = "+",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let subtract = 
        Button(Name = "subtractButton",
                Content = "-",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let multiply = 
        Button(Name = "multiplyButton",
                Content = "*",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let divide = 
        Button(Name = "divideButton",
                Content = "/",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let equals = 
        Button(Name = "equalsButton",
                Content = "=",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 71.,
                Background = buttonColor) //
    let root = 
        Button(Name = "rootButton",
                Content = "\u221A",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let changeSign = 
        Button(Name = "signButton",
                Content = "\u00B1",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let inverse = 
        Button(Name = "inverseButton",
                Content = "1/x",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let percent = 
        Button(Name = "percentButton",
                Content = "%",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let back = 
        Button(Name = "backButton",
                Content = "\u2b05",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let clear = 
        Button(Name = "clearButton",
                Content = "C",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let clearEntry = 
        Button(Name = "clearEntryButton",
                Content = "CE",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let clearMemory = 
        Button(Name = "clearMemoryButton",
                Content = "MC",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let recallMemory = 
        Button(Name = "recallMemoryButton",
                Content = "MR",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let storeMemory = 
        Button(Name = "storeMemoryButton",
                Content = "MS",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let addToMemory = 
        Button(Name = "addToMemoryButton",
                Content = "M+",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let subtractFromMemoy = 
        Button(Name = "subtractFromButton",
                Content = "M-",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
            
    // Text Blocks
    let result =
        TextBlock(Name = "result",
                    TextAlignment = TextAlignment.Right, 
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = Thickness(Left = 26., Top = 29., Right = 0., Bottom = 0.),
                    TextWrapping = TextWrapping.NoWrap,
                    Text = (0.0).ToString(),
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 41.,
                    Width = 169.,
                    Background = backgroundColor,
                    FontSize = 27.)
    let memo =
        TextBlock(Name = "memo",
                    TextAlignment = TextAlignment.Right, 
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = Thickness(Left = 10., Top = 29., Right = 0., Bottom = 0.),
                    TextWrapping = TextWrapping.Wrap,                    
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 41.,
                    Width = 17.,
                    Background = backgroundColor,
                    FontSize = 17.)            
    let helper =
        TextBlock(Name = "helper",
                    TextAlignment = TextAlignment.Right, 
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = Thickness(Left = 10., Top = 10., Right = 0., Bottom = 0.),
                    TextWrapping = TextWrapping.NoWrap,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 19.,
                    Width = 185.,
                    Background = backgroundColor)
    
    // Main Layout Grid
    let grid = Grid( Height = 304.,
                             Width = 206.,
                             Background = gridColor,
                             RenderTransformOrigin = Point(0.5,0.5))
    
    //-------setup calculator logic----------
    let services = CalculatorServices.createServices()
    let calculate = CalculatorImplementation.createCalculate services    
    
    // set initial state
    let mutable state = CalculatorDomain.ZeroState {pendingOp=None;memory=""}

    // a function that sets the displayed text
    let  setDisplayedText = 
        fun text -> result.Text <- text 

    // a function that sets the pending op text
    let  setPendingOpText = 
        fun text -> helper.Text <- text 
 
     // a function that sets the memo text
    let  setMemoText = 
        fun text -> memo.Text <- text

    let handleInput input =
             let newState = calculate(input,state)
             state <- newState 
             setDisplayedText (services.getDisplayFromState state)
             setPendingOpText (services.getPendingOpFromState state)
             setMemoText (services.getMemoFromState state)
    
    // Assemble the calculator controls
    let textBlockList = [result; memo; helper]
    let buttonList = [one; two; three; four; five; six; seven; eight; nine; zero; 
                      decimalPoint; add; subtract; multiply; divide; 
                      equals; root; changeSign; inverse; percent; back; clear; clearEntry; 
                      clearMemory; recallMemory; storeMemory; addToMemory; subtractFromMemoy]
    do 
        for x in buttonList do grid.Children.Add(x) |> ignore
        for x in textBlockList do grid.Children.Add(x) |> ignore        
        calculator.Content <- grid        
        
        //add event handler to each button
        one              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit One)))
        two              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Two)))
        three            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Three)))
        four             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Four)))
        five             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Five)))
        six              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Six)))
        seven            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Seven)))
        eight            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Eight)))
        nine             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Nine)))
        zero             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Zero)))
        decimalPoint     .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (DecimalSeparator)))
        add              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Add)))
        subtract         .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Subtract)))
        multiply         .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Multiply)))
        divide           .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Divide)))        
        equals           .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Equals)))
        clear            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Clear)))
        clearEntry       .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (ClearEntry)))
        clearMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryClear)))
        storeMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryStore)))
        recallMemory     .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryRecall)))
        changeSign       .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp ChangeSign)))
        back             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Back)))
        addToMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp MemoryAdd)))
        subtractFromMemoy.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp MemorySubtract)))
        inverse          .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Inverse))) 
        percent          .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Percent)))
        root             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MathOp Root)))