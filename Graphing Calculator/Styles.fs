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
