namespace GraphingCalculator

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
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Media3D
open System.Windows.Shapes
open Microsoft.Win32

open Utilities

module Style = 
    
    //Colors
    let black = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0x00", byte "0x00", byte "0x00"))
    let screenColor = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE"))
    let transparentBlack = SolidColorBrush(Color.FromArgb (byte "0x00",  byte "0x00", byte "0x00", byte "0x00"))

    //Radial Gradient Brush
    let radialGradientBrush = new RadialGradientBrush()
    do
        RadialGradientBrush().GradientStops.Add(new GradientStop(Color = Colors.White, Offset = 0.))
        RadialGradientBrush().GradientStops.Add(new GradientStop(Color = Colors.Gray, Offset = 1.))

    //Button
    type CalcButton() as calcButton = 
        inherit Button()
    
        do
            calcButton.Margin <- Thickness(left=5.,top=5.,right=5.,bottom=5.)
            calcButton.FontSize <- 24.
            calcButton.BorderBrush <- SolidColorBrush(Colors.Black)
            calcButton.BorderThickness <- Thickness(2.)
            calcButton.Background <- 
                let brush = LinearGradientBrush(StartPoint = Point (0.,0.), EndPoint = Point (0.03,0.9))
                do
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xCC", byte "0xCC", byte "0xCC"), Offset = 0.))
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE"), Offset = 1.))
                brush

    type FunctionButton() as button = 
        inherit Button()
        do  button.FontSize <- 14.
            button.Margin <- Thickness(left = 5.,top = 5.,right = 5.,bottom = 0.)

    //Text box
    type CalcTextBox() as textBox = 
        inherit TextBox()
        do textBox.FontFamily <- FontFamily("Courier New")
   
    type FunctionTextBox() as textBox = 
        inherit TextBox()
        do  textBox.FontFamily <- FontFamily("Courier New")
            textBox.MaxLines <- 1
            textBox.Margin <- Thickness(left = 5.,top = 0.,right = 5.,bottom = 0.) 

    //Text block
    type FunctionTextBlock() as textBox = 
        inherit TextBlock()
        do  textBox.FontSize <- 14.
            textBox.Margin <- Thickness(left = 5.,top = 5.,right = 5., bottom = 5.)

