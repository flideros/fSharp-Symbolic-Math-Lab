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

type SaturationAndLuminosityPicker (saturationValue:SharedValue<Saturation>,
                                    luminosityValue:SharedValue<Luminosity>,
                                    currentHue:SharedValue<Hue>
                                   ) as saturationAndLuminosityPicker =
    inherit UserControl() 
    let c = Canvas(Height = 300.,Width = 300.,Background = Brushes.Black)
    let b = Border(Height = 300.,Width = 300.,Background = CustomBrushes.checkerBrush)
    let locus =     
        let ellipse = 
            EllipseGeometry(Center = Point(0.,0.),
                            RadiusX = 5.,
                            RadiusY = 5.)
        let path = 
            Path(
                Stroke = Brushes.Black,
                StrokeThickness = 1.,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Data = ellipse)
        path
    let currentColor = SharedValue<Color>(ColorUtilities.convertHsvToRgb currentHue.Get 1. 1.)
    let fillGradient color = 
        let lgb = 
            LinearGradientBrush(
               StartPoint = Point (0.0,0.0), 
               EndPoint = Point (1.0,0.0))
        do  lgb.GradientStops.Add(GradientStop(Color = Colors.White,Offset = 0.))
            lgb.GradientStops.Add(GradientStop(Color = color,Offset = 1.))
        lgb
    let opacityMaskGradient = 
        let lgb = 
            LinearGradientBrush(
               StartPoint = Point (0.0,0.0), 
               EndPoint = Point (0.0,1.0))
        do  lgb.GradientStops.Add(GradientStop(Color = Colors.White,Offset = 0.))
            lgb.GradientStops.Add(GradientStop(Color = Colors.Transparent,Offset = 1.))
        lgb
    let saturationLuminosityPicker_ShapeContainer = 
        let rectangle = Rectangle(Height = 300.,Width = 300.)
        do  rectangle.Fill <- (fillGradient currentColor.Get)
            rectangle.OpacityMask <- opacityMaskGradient
        rectangle 
    let convertYToLuminosity (p:Point) = 
        let h = 1.-((100./c.Height)*(p.Y)*0.01)
        match h with 
        | x when x < 1. && x > 0.993 -> luminosityValue.Set(1.)
        | x when x > 0. && x < 0.007 -> luminosityValue.Set(0.)
        | x -> match h > 1. || h < 0. with 
               | false -> luminosityValue.Set(x)
               | true -> ()
    do  c.Children.Add(saturationLuminosityPicker_ShapeContainer) |> ignore
        c.Children.Add(locus) |> ignore
        b.Child <- c
        saturationAndLuminosityPicker.Content <- b

    let handleHueChanged hue = 
        do  currentColor.Set(ColorUtilities.convertHsvToRgb hue 1. 1.)
            saturationLuminosityPicker_ShapeContainer.Fill <- fillGradient currentColor.Get
    let handleOnChange_SaturationValue saturation = 
        match saturationAndLuminosityPicker.IsMouseOver with
        | true -> ()
        | false -> locus.SetValue(Canvas.LeftProperty,saturation*c.Width)
    let handleOnChange_LuminosityValue luminosity = 
        match saturationAndLuminosityPicker.IsMouseOver with
        | true -> ()
        | false -> locus.SetValue(Canvas.TopProperty,(1.-luminosity)*c.Height)
    let handlePreviewMouseMove(e:MouseEventArgs) = (**)
        let point = e.MouseDevice.GetPosition(c)                
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  
            let y = 
                match point.Y < 300. && point.Y > 0. with
                | true -> point.Y
                | false when point.Y >= 300. -> 300.
                | false -> 0.
            let x = 
                match point.X <= 300. && point.X >= 0. with
                | true -> point.X
                | false when point.X >= 300. -> 300.
                | false -> 0.
            do  locus.SetValue(Canvas.TopProperty, y)
                locus.SetValue(Canvas.LeftProperty, x)
                saturationValue.Set(x/c.Width)
                convertYToLuminosity point
    do  currentHue.Changed.Add(handleHueChanged)
        c.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))
        saturationValue.Changed.Add(handleOnChange_SaturationValue)
        luminosityValue.Changed.Add(handleOnChange_LuminosityValue)

type HslColorPicker() as colorPicker =
    inherit UserControl() 
    
    let colorPicker_Grid = 
        let g = 
            Grid(
                Background = SolidColorBrush(Color.FromArgb(byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE")),
                Height = 400.,
                Width = 600.
                )
        let column1 = ColumnDefinition(Width = GridLength(400.))
        let column2 = ColumnDefinition(Width = GridLength(1.,GridUnitType.Star))
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)

        let row1 = RowDefinition(Height = GridLength(400.))
        let row2 = RowDefinition(Height = GridLength(0.,GridUnitType.Star))
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
        g
            
    let hueValue = SharedValue<Hue> (0.)
    let saturationValue = SharedValue<Saturation> (1.)
    let luminosityValue = SharedValue<Luminosity> (1.)    
    let currentColor = SharedValue<Color> (Colors.Transparent)
    let selectedColor = SharedValue<Color> (Colors.Transparent)

    let hueSlider = 
        let h = HueSlider(hueValue=hueValue)    
        do  h.SetValue(Grid.ColumnProperty,3)
            h.SetValue(Grid.RowProperty,2)

        h
    let saturationSlider = 
        let s = SaturationSlider(saturationValue=saturationValue, currentHue = hueValue)
        do  s.SetValue(Grid.RowProperty,1)
        s
    let luminositySlider = 
        let l = LuminositySlider(luminosityValue = luminosityValue, currentHue = hueValue)
        do  l.SetValue(Grid.ColumnProperty,2)
            l.SetValue(Grid.RowProperty,2)
        l
    
    let colorDetails_Grid = 
        let g = Grid(Margin = Thickness(5.,0.,5.,0.))
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)    
            g.RowDefinitions.Add(row3)

            g.SetValue(Grid.ColumnProperty,1)
        g
    let colorDetailsControls_Grid = 
        let g = Grid(Margin = Thickness(5.,0.,5.,0.))
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition()
        let row3 = RowDefinition(Height = GridLength.Auto)
        let row4 = RowDefinition()
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)    
            g.RowDefinitions.Add(row3)
            g.RowDefinitions.Add(row4)
            g.SetValue(Grid.RowProperty,0)
            g.SetValue(Grid.ColumnSpanProperty,3)
        g
    
    let selectedColor_TextBlock = 
        let t = TextBlock(Text = "Selected Color", FontWeight = FontWeights.Bold)
        t.SetValue(Grid.RowProperty,0)
        t
    let selectedColor_Border = 
        let b = 
            Border(
                BorderBrush = SystemColors.ControlDarkDarkBrush,
                BorderThickness = Thickness(2.),
                Background = CustomBrushes.checkerBrush,
                Margin = Thickness(0.,0.,0.,5.))
        do  b.SetValue(Grid.RowProperty,1)
        b
    let selectedColor_Rectangle = 
        Shapes.Rectangle(Margin=Thickness(5.), 
                         RadiusX = 5., 
                         RadiusY = 5.,
                         Stroke = Brushes.Black,
                         Stretch = Stretch.Fill,
                         HorizontalAlignment = HorizontalAlignment.Stretch,
                         MinWidth = 50., 
                         MinHeight = 50.,
                         VerticalAlignment = VerticalAlignment.Stretch)
    let selectedColor_shapeContainer = 
        ShapeContainer(
            shapes = SharedValue (Shapes.CustomShape selectedColor_Rectangle),
            width = SharedValue(175.),
            height = SharedValue(50.),
            color = selectedColor)   
    
    let opacity_TextBlock = 
        let t = TextBlock(Text = "Opacity", FontWeight = FontWeights.Bold)
        t.SetValue(Grid.RowProperty,2)
        t
    let opacity_Slider =
        let s = 
            Slider(Orientation = Orientation.Horizontal,
                   Minimum = 0.,
                   Maximum = 1.,
                   TickFrequency = 0.01,
                   SmallChange = 0.01,
                   LargeChange = 0.02,
                   IsDirectionReversed = true,
                   OverridesDefaultStyle = true,
                   Value = 1.)
        s
    let opacitySliderGradient_Border =
        let lgb = LinearGradientBrush(StartPoint = Point(0.,0.5),EndPoint = Point (1.,0.5))
        let gs1 = GradientStop(Offset = 0., Color = Colors.Black)
        let gs2 = GradientStop(Offset = 1., Color = Colors.Transparent)
        do  lgb.GradientStops.Add(gs1)
            lgb.GradientStops.Add(gs2)
        let b =
            Border(
                BorderBrush = SystemColors.ControlDarkDarkBrush,
                BorderThickness = Thickness(0.,0.,0.,0.),
                Background = lgb,
                VerticalAlignment = VerticalAlignment.Top,                
                Child = opacity_Slider) 
        do  b.SetValue(Grid.RowProperty,3)
        b      
    let opacitySlider_Border = 
        let b =
            Border(
                BorderBrush = SystemColors.ControlDarkDarkBrush,
                BorderThickness = Thickness(2.,2.,2.,2.),
                Background = CustomBrushes.checkerBrush,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = Thickness(0.,2.,0.,2.),
                Child = opacitySliderGradient_Border) 
        do  b.SetValue(Grid.RowProperty,3)
        b

    do  selectedColor_Border.Child <- selectedColor_shapeContainer
        colorDetailsControls_Grid.Children.Add(selectedColor_TextBlock) |> ignore
        colorDetailsControls_Grid.Children.Add(selectedColor_Border) |> ignore
        colorDetailsControls_Grid.Children.Add(opacity_TextBlock) |> ignore
        colorDetailsControls_Grid.Children.Add(opacitySlider_Border) |> ignore

        colorDetails_Grid.Children.Add(colorDetailsControls_Grid) |> ignore

    let saturationAndLuminosityPicker = 
        let s = 
            SaturationAndLuminosityPicker (
                saturationValue = saturationValue,
                luminosityValue = luminosityValue,
                currentHue = hueValue )
        do  
            s.SetValue(Grid.RowProperty,2)            
            s.SetValue(Grid.ColumnProperty,1)
        s
    let hslColorPickerGrid = 
        let g = Grid()        
        let column0 = ColumnDefinition(Width = GridLength 15.)
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        let column4 = ColumnDefinition(Width = GridLength(50., GridUnitType.Star))
        do  g.ColumnDefinitions.Add(column0)
            g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)
            g.ColumnDefinitions.Add(column4)

        let row0 = RowDefinition(Height = GridLength.Auto)
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3= RowDefinition(Height = GridLength(0., GridUnitType.Star))
        do  g.RowDefinitions.Add(row0)
            g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.RowDefinitions.Add(row3)
            
            g.Children.Add(saturationAndLuminosityPicker) |> ignore            
            g.Children.Add(hueSlider) |> ignore
            g.Children.Add(saturationSlider) |> ignore
            g.Children.Add(luminositySlider) |> ignore

            g.SetValue(Grid.ColumnProperty,0)
        g

    do  colorPicker_Grid.Children.Add(hslColorPickerGrid) |> ignore
        colorPicker_Grid.Children.Add(colorDetails_Grid) |> ignore
        colorPicker.Content <- colorPicker_Grid

    let handleMouseMove (e:MouseEventArgs) =  
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  currentColor.Set(ColorUtilities.convertHsvToRgb (hueValue.Get) 1. 1. )                      
    let handleSelectColor() = 
        do  selectedColor.Set(ColorUtilities.convertHsvToRgb (hueValue.Get) (saturationValue.Get) (luminosityValue.Get) )
    let handleOpacityOnChange() = do  selectedColor_shapeContainer.Opacity <- opacity_Slider.Value

    do  hueSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove (e)))
        hueSlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        saturationSlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        luminositySlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        saturationAndLuminosityPicker.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        opacity_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleOpacityOnChange()))