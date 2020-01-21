namespace ControlLibrary

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes  
open System.Windows.Media.Animation
open System.IO
open System.Windows.Markup
open System.Windows.Controls 
open System.Reflection
open System.Windows.Media.Imaging
open System.Windows.Media.Media3D
open Utilities
open System.Windows.Input

type ColorThumb() as colorThumb = 
    inherit UserControl()   
   
    let colorThumb_Grid = 
        let c = Canvas()        
        c

    let glassArrow_Points =
        let pc = PointCollection()
        let p0 = Point(100.,20.)
        let p1 = Point(80.,50.)
        let p2 = Point(100.,80.)
        do  pc.Add(p0) 
            pc.Add(p1) 
            pc.Add(p2)
        pc      
    let glassArrow_Shape = 
        Polygon(Stroke = SystemColors.ControlDarkDarkBrush, 
                Points = glassArrow_Points,
                StrokeThickness = 2., 
                Fill = CustomBrushes.glassBrush,
                Stretch = Stretch.Fill)       
    let glassArrow_ShapeContainer = 
        let b = 
            Border()
        do  b.Child <- glassArrow_Shape
            b.Width <- 35.
            b.Height <-25.
        b   
    
    let arrow_Points =
        let poly = Polygon(Stroke = SystemColors.ControlDarkDarkBrush, StrokeThickness = 2.)
        glassArrow_Points,poly        
    let arrow_Shape = ControlLibrary.Shapes.CustomPolygon arrow_Points    
    let arrow_ShapeContainer = 
        let c = 
            ShapeContainer(
                shapes = SharedValue arrow_Shape,
                width = SharedValue glassArrow_ShapeContainer.Width,//200.,
                height = SharedValue glassArrow_ShapeContainer.Height,//200.,
                color = SharedValue Colors.Transparent)
        c
    
    do  colorThumb_Grid.Children.Add(arrow_ShapeContainer) |> ignore
        colorThumb_Grid.Children.Add(glassArrow_ShapeContainer) |> ignore

        colorThumb.Content <- colorThumb_Grid

type ColorSlider() as colorSlider = 
    inherit UserControl()   

    let spectrumBrush = ()
   
    let t = ColorThumb()
    let b = Border()
    let sliderCanvas = 
        //let g = Grid()
        let c = 
            Canvas(
                ClipToBounds = true,
                Height = 200.,
                Width = 50.)//,Background = Brushes.DarkRed)      
        
        do  b.SetValue(Canvas.TopProperty,0.)
            b.SetValue(Canvas.LeftProperty,15.)
            b.Child <- t
            c.Children.Add(b) |> ignore            
        c
    
    do  colorSlider.Content <- sliderCanvas //ColorThumb()
        sliderCanvas.Background <- CustomBrushes.spectrumBrush