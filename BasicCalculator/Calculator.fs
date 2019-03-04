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


type Calculator() as calculator =     
    inherit UserControl()
    //Define app resources and load them to the main program.
    //let resource = new Uri("app.xaml",System.UriKind.Relative)
    //let mainProgram = Application.LoadComponent(resource) :?> Application

    // Create Types
        
        //Colors
    let gridColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xF6", byte "0xF7", byte "0xF9"))
    let buttonColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xFB", byte "0xFB", byte "0xFB"))
    let backgroundColor = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xF6", byte "0xF7", byte "0xF9"))
    
        //Buttons     
    let mutable one = 
        Button(Name = "oneButton",
                Content = "1",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    
    let mutable two = 
        Button(Name = "twoButton",
                Content = "2",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor)
    let mutable three = 
        Button(Name = "threeButton",
                Content = "3",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable four = 
        Button(Name = "fourButton",
                Content = "4",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable five = 
        Button(Name = "fiveButton",
                Content = "5",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable six = 
        Button(Name = "sixButton",
                Content = "6",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable seven = 
        Button(Name = "sevenButton",
                Content = "7",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable eight = 
        Button(Name = "eightButton",
                Content = "8",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable nine = 
        Button(Name = "nineButton",
                Content = "9",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable zero = 
        Button(Name = "zeroButton",
                Content = "0",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 71.,
                Height = 33.,
                Background = buttonColor) //
    let mutable decimalPoint = 
        Button(Name = "decimalPointButton",
                Content = ".",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable add = 
        Button(Name = "addButton",
                Content = "+",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 265., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable subtract = 
        Button(Name = "subtractButton",
                Content = "-",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable multiply = 
        Button(Name = "multiplyButton",
                Content = "*",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable divide = 
        Button(Name = "divideButton",
                Content = "/",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable equals = 
        Button(Name = "equalsButton",
                Content = "=",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 227., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 71.,
                Background = buttonColor) //
    let mutable root = 
        Button(Name = "rootButton",
                Content = "\u221A",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable sign = 
        Button(Name = "signButton",
                Content = "\u00B1",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable inverse = 
        Button(Name = "inverseButton",
                Content = "1/x",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 189., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable percent = 
        Button(Name = "percentButton",
                Content = "%",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 151., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable back = 
        Button(Name = "backButton",
                Content = "\u2b05",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable clear = 
        Button(Name = "clearButton",
                Content = "C",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable clearEntry = 
        Button(Name = "clearEntryButton",
                Content = "CE",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 113., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable clearMemory = 
        Button(Name = "clearMemoryButton",
                Content = "MC",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 10., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable recallMemory = 
        Button(Name = "recallMemoryButton",
                Content = "MR",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 48., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable storeMemory = 
        Button(Name = "storeMemoryButton",
                Content = "MS",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 86., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable addToMemory = 
        Button(Name = "addToMemoryButton",
                Content = "M+",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 125., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
    let mutable subtractFromMemoy = 
        Button(Name = "subtractFromButton",
                Content = "M-",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness(Left = 162., Top = 75., Right = 0., Bottom = 0.),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 33.,
                Height = 33.,
                Background = buttonColor) //
            
        // Text Blocks
    let mutable result =
        TextBlock(Name = "result",
                    TextAlignment = TextAlignment.Right, 
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = Thickness(Left = 26., Top = 29., Right = 0., Bottom = 0.),
                    TextWrapping = TextWrapping.NoWrap,
                    Text = "0",
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 41.,
                    Width = 169.,
                    Background = backgroundColor,
                    FontSize = 27.)
    let mutable memo =
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
    let mutable helper =
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
    let mutable grid = Grid( Height = 304.,
                             Width = 206.,
                             Background = gridColor,
                             RenderTransformOrigin = Point(0.5,0.5))
    
    

 (*   
    // Add click event to each button
    // MEMORY COMMAND
    let memoCommand (oper : string) = 
        match result.Text with
        | "NaN" | "∞"-> ignore()    
        | _ ->    match oper with
                  | "MS" -> Perform.memo <- txtResult.Text    // Memory Store puts the number on the display into the memory 
                            memo.Text <- "M"
                  | "MR" -> result.Text <- Perform.memo    // Memory Recall uses the number in memory
                            Perform.memo <- "0.0"
                            memo.Text <- ""
                  | "MC" -> Perform.memo <- "0.0"             // Memory Clear
                            memo.Text <- ""
                  | "M+" -> Perform.memo <- (Double.Parse(Perform.memo) + Double.Parse(txtResult.Text)).ToString()
                            memo.Text <- "M"
                  | "M-" -> Perform.memo <- (Double.Parse(Perform.memo) - Double.Parse(txtResult.Text)).ToString() 
                            memo.Text <- "M"
                  | _    -> Perform.memo <- ("0")
                            memo.Text <- ""
                  Perform.blnCommand <- true
                  memo.ToolTip <- Perform.memo
            
    do btnMS.Click.Add(fun _ -> memoCommand("MS"))
       btnMR.Click.Add(fun _ -> memoCommand("MR"))
       btnMC.Click.Add(fun _ ->  memoCommand("MC"))  
       btnMPlus.Click.Add(fun _ -> memoCommand("M+"))
       btnMMinus.Click.Add(fun _ -> memoCommand("M-")) 
    
    *)

    let mutable textBlockList = [result; memo; helper]
    let mutable buttonList =[one;two;three;four;five;six;seven;eight;nine;zero;
        decimalPoint;add;subtract;multiply;
        divide;equals;root;sign;inverse;percent;back;clear;clearEntry;
        clearMemory;recallMemory;storeMemory;addToMemory;subtractFromMemoy]

    do 
        for x in buttonList do grid.Children.Add(x) |> ignore
        for x in textBlockList do grid.Children.Add(x) |> ignore
        calculator.Content <- grid
