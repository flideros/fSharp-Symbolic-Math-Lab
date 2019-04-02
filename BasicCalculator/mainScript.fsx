
#light

#I @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA\Office14\"
#r "Office.dll"
#r "stdole.dll"
#r "windowsbase.dll"
#r "presentationcore.dll"
#r "presentationframework.dll"

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
//open Utilities
//open Style
//open GraphingCalculatorDomain



// ------Create Types---------        
let calculatorGrid = new Grid(Name = "calculator")    
let rectangle = new Rectangle()
let textBlock = new TextBlock()

calculatorGrid.Name