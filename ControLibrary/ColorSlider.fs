﻿namespace ControlLibrary

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

// Hue
type HueThumb(hueValue:SharedValue<Hue>) as hueThumb = 
    inherit UserControl()   
    
    let color = 
        let c = ColorUtilities.convertHsvToRgb hueValue.Get 1. 1.
        SharedValue<Color>(c)

    let hueThumb_Grid = 
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
            b.Height <-30. 
        b   
    
    let arrow_Points =
        let poly = Polygon(Stroke = SystemColors.ControlDarkDarkBrush, StrokeThickness = 2.)
        glassArrow_Points,poly        
    let arrow_Shape = ControlLibrary.Shapes.CustomPolygon arrow_Points    
    let arrow_ShapeContainer = 
        
        let c = 
            ShapeContainer(
                shapes = SharedValue arrow_Shape,
                width = SharedValue glassArrow_ShapeContainer.Width,
                height = SharedValue glassArrow_ShapeContainer.Height,
                color = color)
        c
    
    do  hueThumb_Grid.Children.Add(arrow_ShapeContainer) |> ignore
        hueThumb_Grid.Children.Add(glassArrow_ShapeContainer) |> ignore

        hueThumb.Content <- hueThumb_Grid

    let handleOnChanged_HueValue (hue) = color.Set (ColorUtilities.convertHsvToRgb hue 1. 1.)

    do hueValue.Changed.Add(handleOnChanged_HueValue)

type HueSlider( hueValue:SharedValue<Hue>) as hueSlider = 
    inherit UserControl() 
    
    let upperHueValue = 360.

    let spectrum = Grid(Width = 30.,Height = 300., Background = CustomBrushes.spectrumBrush)
    let thumb = HueThumb(hueValue = hueValue)
    let sliderCanvas =         
        let c = 
            Canvas(
                ClipToBounds = false,
                Height = (330.),
                Width = 50.,
                Background = Brushes.Transparent)      
        
        do  thumb.SetValue(Canvas.TopProperty,0.)
            thumb.SetValue(Canvas.LeftProperty,15.)  
            spectrum.SetValue(Canvas.BottomProperty,15.)
            spectrum.SetValue(Canvas.LeftProperty,5.)
            c.Children.Add(spectrum) |> ignore
            c.Children.Add(thumb) |> ignore            
        c
    
    do  hueSlider.Content <- sliderCanvas          
    
    let convertYToHue (p:Point) = 
        let h = (upperHueValue/spectrum.Height)*(p.Y)
        match h with 
        | x when x < 360. && x > 359.3 -> 360.
        | x when x > 0. && x < 0.7 -> 0.
        | _ -> h
    let handlePreviewMouseMove(e:MouseEventArgs) = 
        let point = e.MouseDevice.GetPosition(spectrum)                
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  
            match point.Y >= 0.0 && point.Y <= 300.0 with 
            | true -> 
                do  hueValue.Set(convertYToHue point)
                let newColor = ColorUtilities.convertHsvToRgb (hueValue.Get) 1. 1.
                do  thumb.SetValue(Canvas.TopProperty,point.Y)
                    
            | false -> ()
                
    do  hueSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))

// Saturation
type SaturationThumb(saturationValue:SharedValue<Saturation>,
                     currentHue:SharedValue<Hue>) as saturationThumb = 
    inherit UserControl()   
    
    let thumbColor = SharedValue<Color>(ColorUtilities.convertHsvToRgb (currentHue.Get) saturationValue.Get 1.)

    let saturationThumb_Grid = 
        let c = Canvas()        
        c

    let glassArrow_Points =
        let pc = PointCollection()
        let p0 = Point(20.,-100.)
        let p1 = Point(50.,-80.)
        let p2 = Point(80.,-100.)
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
            b.Width <- 30.
            b.Height <-30. 
        b   
    
    let arrow_Points =
        let poly = Polygon(Stroke = SystemColors.ControlDarkDarkBrush, StrokeThickness = 2.)
        glassArrow_Points,poly        
    let arrow_Shape = ControlLibrary.Shapes.CustomPolygon arrow_Points    
    let arrow_ShapeContainer = 
        let c = 
            ShapeContainer(
                shapes = SharedValue arrow_Shape,
                width = SharedValue glassArrow_ShapeContainer.Width,
                height = SharedValue glassArrow_ShapeContainer.Height,
                color = thumbColor)
        c
    
    do  saturationThumb_Grid.Children.Add(arrow_ShapeContainer) |> ignore
        saturationThumb_Grid.Children.Add(glassArrow_ShapeContainer) |> ignore

        saturationThumb.Content <- saturationThumb_Grid

    let handleOnChange_CurrentHue hue  = thumbColor.Set (ColorUtilities.convertHsvToRgb (hue) saturationValue.Get 1.)
    let handleOnChange_CurrentSaturation saturation  = thumbColor.Set (ColorUtilities.convertHsvToRgb (currentHue.Get) saturation 1.)
        
    do  currentHue.Changed.Add(handleOnChange_CurrentHue)
        saturationValue.Changed.Add(handleOnChange_CurrentSaturation)

type SaturationSlider( saturationValue:SharedValue<Saturation>,
                       currentHue:SharedValue<Hue>) as saturationSlider = 
    inherit UserControl() 
    
    let upperSaturationValue = 100. 
    let currentColor = SharedValue<Color>(ColorUtilities.convertHsvToRgb currentHue.Get saturationValue.Get 1. )
    
    let saturation_linearGradientBrush = LinearGradientBrush(Colors.White,(ColorUtilities.convertHsvToRgb currentHue.Get saturationValue.Get 1. ),0.)
    let gradient = Grid(Width = 300.,Height = 20., Background = saturation_linearGradientBrush)
    let thumb = SaturationThumb(saturationValue = saturationValue, currentHue = currentHue)
    
    let sliderCanvas =         
        let c = 
            Canvas(
                ClipToBounds = false,
                Height = 25.,
                Width = 300.,
                Background= Brushes.Transparent)      
        let trackLine = Line(X1 = 15., X2 = 315., Y1 = 41., Y2 = 41., Stroke = Brushes.LightSlateGray, StrokeThickness = 2.)
        do  thumb.SetValue(Canvas.TopProperty,10.)
            thumb.SetValue(Canvas.LeftProperty,0.)  
            gradient.SetValue(Canvas.TopProperty,20.)
            gradient.SetValue(Canvas.LeftProperty,15.)
            c.Children.Add(gradient) |> ignore
            c.Children.Add(thumb) |> ignore
            c.Children.Add(trackLine) |> ignore
        c
    
    do  saturationSlider.Content <- sliderCanvas          
    
    let convertXToSaturation (p:Point) = 
        let w = (upperSaturationValue/gradient.Width)*(p.X)*0.01
        match w with 
        | x when x < 1. && x > 0.993 -> 1.
        | x when x > 0. && x < 0.007 -> 0.
        | _ -> w
    let handlePreviewMouseMove(e:MouseEventArgs) = (**)
        let point = e.MouseDevice.GetPosition(gradient)                
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  
            match point.X >= 0.0 && point.X <= 300.0 with 
            | true -> 
                do  saturationValue.Set(convertXToSaturation point)
                let newColor = ColorUtilities.convertHsvToRgb (ColorUtilities.getHueFromRGB currentColor.Get) (saturationValue.Get) 1.
                do  thumb.SetValue(Canvas.LeftProperty,point.X)
                    currentColor.Set newColor
            | false -> ()
    let handleOnChange_ThumbHue hue  = 
        match saturationSlider.IsMouseOver with
        | true -> ()
        | false -> gradient.Background <- LinearGradientBrush(Colors.White,ColorUtilities.convertHsvToRgb hue 1. 1.,0.)    
    let handleOnChange_SaturationValue saturation  = 
        match saturationSlider.IsMouseOver with
        | true -> ()
        | false -> thumb.SetValue(Canvas.LeftProperty,saturation*sliderCanvas.Width)

    do  saturationSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))
        currentHue.Changed.Add(handleOnChange_ThumbHue)
        saturationValue.Changed.Add(handleOnChange_SaturationValue)

// Luminosity
type LuminosityThumb(luminosityValue:SharedValue<Luminosity>,
                     currentHue:SharedValue<Hue>) as luminosityThumb = 
    inherit UserControl()   
    
    let thumbColor = SharedValue<Color>(ColorUtilities.convertHsvToRgb currentHue.Get 1. luminosityValue.Get)

    let luminosityThumb_Grid = 
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
            b.Width <- 30.
            b.Height <-30. 
        b   
    
    let arrow_Points =
        let poly = Polygon(Stroke = SystemColors.ControlDarkDarkBrush, StrokeThickness = 2.)
        glassArrow_Points,poly        
    let arrow_Shape = ControlLibrary.Shapes.CustomPolygon arrow_Points    
    let arrow_ShapeContainer = 
        let c = 
            ShapeContainer(
                shapes = SharedValue arrow_Shape,
                width = SharedValue glassArrow_ShapeContainer.Width,
                height = SharedValue glassArrow_ShapeContainer.Height,
                color = thumbColor)
        c
    
    do  luminosityThumb_Grid.Children.Add(arrow_ShapeContainer) |> ignore
        luminosityThumb_Grid.Children.Add(glassArrow_ShapeContainer) |> ignore

        luminosityThumb.Content <- luminosityThumb_Grid

    let handleOnChange_CurrentHue hue  = thumbColor.Set (ColorUtilities.convertHsvToRgb (hue) 1. luminosityValue.Get)
    let handleOnChange_CurrentLuminosity luminosity  = thumbColor.Set (ColorUtilities.convertHsvToRgb (currentHue.Get) 1. luminosity)
        
    do  currentHue.Changed.Add(handleOnChange_CurrentHue)
        luminosityValue.Changed.Add(handleOnChange_CurrentLuminosity)

type LuminositySlider( luminosityValue:SharedValue<Luminosity>,
                       currentHue:SharedValue<Hue>) as luminositySlider = 
    inherit UserControl() 
    
    let upperSaturationValue = 100. 
    let currentColor = SharedValue<Color>(ColorUtilities.convertHsvToRgb currentHue.Get 1. luminosityValue.Get)
    
    let luminosity_linearGradientBrush = LinearGradientBrush((ColorUtilities.convertHsvToRgb currentHue.Get  1. luminosityValue.Get),Colors.Black,90.)
    let gradient = Grid(Width = 20.,Height = 300., Background = luminosity_linearGradientBrush)
    let thumb = LuminosityThumb(luminosityValue = luminosityValue, currentHue = currentHue)
    
    let sliderCanvas =         
        let c = 
            Canvas(
                ClipToBounds = false,
                Height = (330.),
                Width = 35.,
                Background= Brushes.Transparent)      
        let trackLine = Line(X1 = 0., X2 = 0., Y1 = 15., Y2 = 315., Stroke = Brushes.LightSlateGray, StrokeThickness = 2.)
        do  thumb.SetValue(Canvas.TopProperty,0.)
            thumb.SetValue(Canvas.LeftProperty,0.)  
            gradient.SetValue(Canvas.BottomProperty,15.)
            gradient.SetValue(Canvas.LeftProperty,0.0)
            c.Children.Add(gradient) |> ignore
            c.Children.Add(thumb) |> ignore
            c.Children.Add(trackLine) |> ignore
        c
    
    do  luminositySlider.Content <- sliderCanvas          
    
    let convertYToLuminosity (p:Point) = 
        let h = 1.-((upperSaturationValue/gradient.Height)*(p.Y)*0.01)
        match h with 
        | x when x < 1. && x > 0.993 -> 1.
        | x when x > 0. && x < 0.007 -> 0.
        | _ -> h
    let handlePreviewMouseMove(e:MouseEventArgs) = (**)
        let point = e.MouseDevice.GetPosition(gradient)                
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  
            match point.Y >= 0.0 && point.Y <= 300.0 with 
            | true -> 
                do  luminosityValue.Set(convertYToLuminosity point)
                let newColor = ColorUtilities.convertHsvToRgb (ColorUtilities.getHueFromRGB currentColor.Get) 1. (luminosityValue.Get)
                do  thumb.SetValue(Canvas.TopProperty,point.Y)
                    currentColor.Set newColor
            | false -> ()
    let handleOnChange_ThumbHue hue  = 
        match luminositySlider.IsMouseOver with
        | true -> ()
        | false -> gradient.Background <- LinearGradientBrush(ColorUtilities.convertHsvToRgb hue 1. 1.,Colors.Black,90.)
    let handleOnChange_LuminosityValue luminosity  = 
        match luminositySlider.IsMouseOver with
        | true -> ()
        | false -> 
            do thumb.SetValue(Canvas.TopProperty,((1. - luminosity)*300.))                
     
    do  luminositySlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))
        currentHue.Changed.Add(handleOnChange_ThumbHue)
        luminosityValue.Changed.Add(handleOnChange_LuminosityValue)

