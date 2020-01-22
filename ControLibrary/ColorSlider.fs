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
    let g = Grid(Width = 30.,Height = 200., Background = CustomBrushes.spectrumBrush)
    let t = ColorThumb(Tag = Point(15.,0.))
    let sliderCanvas = 
        
        let c = 
            Canvas(
                ClipToBounds = true,
                Height = 250.,
                Width = 50.)      
        
        do  t.SetValue(Canvas.TopProperty,0.)
            t.SetValue(Canvas.LeftProperty,15.)  
            g.SetValue(Canvas.TopProperty,10.)
            g.SetValue(Canvas.LeftProperty,5.)
            c.Children.Add(g) |> ignore
            c.Children.Add(t) |> ignore            
        c
    
    do  colorSlider.Content <- sliderCanvas 
        
    let handlePreviewMouseLeftButtonDown(e:MouseButtonEventArgs) = 
        let p0 = e.MouseDevice.GetPosition(sliderCanvas)  
        do  t.Tag <- p0 
    let handlePreviewMouseMove(e:MouseEventArgs) = 
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true -> 
            let p0 = e.MouseDevice.GetPosition(sliderCanvas)
            do  t.SetValue(Canvas.TopProperty,((p0 - (t.Tag :?> Point)).Y))
                t.Tag <- p0

    do  t.PreviewMouseLeftButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handlePreviewMouseLeftButtonDown (e)))
        colorSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))