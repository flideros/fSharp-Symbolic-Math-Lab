module GraphicResources

open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging


// Brushes
let clear = SolidColorBrush(Colors.Transparent)
let black = SolidColorBrush(Colors.Black) 
let blue =  SolidColorBrush(Colors.Blue)
let blue2 = SolidColorBrush(Colors.RoyalBlue)
let olive = SolidColorBrush(Colors.Olive)
let red =   SolidColorBrush(Colors.Red)
let green = SolidColorBrush(Colors.Green)


// Pens
let bluePen = 
    let p = Pen(blue, 0.5) 
    do p.Freeze() 
    p
let blueGridline = 
    let p = Pen(blue, 0.2)
    do p.Freeze() 
    p
let redPen = 
    let p = Pen(red, 0.5)
    do p.Freeze() 
    p
let redGridline = 
    let p = Pen(red, 0.1)
    do p.Freeze() 
    p
let blackPen = 
    let p = Pen(black, 0.5)
    do p.Freeze() 
    p


// Visuals
let visual = DrawingVisual()
let image = 
    let i = Image()
    do  i.SetValue(Panel.ZIndexProperty, -100)
    i