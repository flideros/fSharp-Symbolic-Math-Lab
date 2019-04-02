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
open Utilities
open Style
//open GraphingCalculatorDomain

type GraphingCalculator() as graphingCalculator =
    inherit UserControl()

    //Colors 
    let black = new SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xF6", byte "0xF7", byte "0xF9"))

// ------Create Types---------        
    let calculatorGrid = new Grid(Name = "calculator")    
    let rectangle = new Rectangle(StrokeThickness = 2., Stroke = black, Fill = radialGradientBrush, RadiusX = 10., RadiusY = 10.)
    let textBlock = new TextBlock()


