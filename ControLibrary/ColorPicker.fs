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

type SaturationAndBrightnessPicker (saturationValue:SharedValue<Saturation>,
                                    brightnessValue:SharedValue<Brightness>,
                                    currentHue:SharedValue<Hue>
                                   ) as saturationAndBrightnessPicker =
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
    let saturationBrightnessPicker_ShapeContainer = 
        let rectangle = Rectangle(Height = 300.,Width = 300.)
        do  rectangle.Fill <- (fillGradient currentColor.Get)
            rectangle.OpacityMask <- opacityMaskGradient
        rectangle 
    let convertYToBrightness (p:Point) = 
        let h = 1.-((100./c.Height)*(p.Y)*0.01)
        match h with 
        | x when x < 1. && x > 0.993 -> brightnessValue.Set(1.)
        | x when x > 0. && x < 0.007 -> brightnessValue.Set(0.)
        | x -> match h > 1. || h < 0. with 
               | false -> brightnessValue.Set(x)
               | true -> ()
    do  c.Children.Add(saturationBrightnessPicker_ShapeContainer) |> ignore
        c.Children.Add(locus) |> ignore
        b.Child <- c
        saturationAndBrightnessPicker.Content <- b


    let initializeComponent() = locus.SetValue(Canvas.LeftProperty,(saturationValue.Get)*c.Width)
    let handleHueChanged hue = 
        do  currentColor.Set(ColorUtilities.convertHsvToRgb hue 1. 1.)
            saturationBrightnessPicker_ShapeContainer.Fill <- fillGradient currentColor.Get
    let handleOnChange_SaturationValue saturation = 
        match saturationAndBrightnessPicker.IsMouseOver with
        | true -> ()
        | false -> locus.SetValue(Canvas.LeftProperty,saturation*c.Width)
    let handleOnChange_BrightnessValue brightness = 
        match saturationAndBrightnessPicker.IsMouseOver with
        | true -> ()
        | false -> locus.SetValue(Canvas.TopProperty,(1.-brightness)*c.Height)
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
                convertYToBrightness point
    do  currentHue.Changed.Add(handleHueChanged)
        c.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handlePreviewMouseMove (e)))
        saturationValue.Changed.Add(handleOnChange_SaturationValue)        
        brightnessValue.Changed.Add(handleOnChange_BrightnessValue)
        saturationAndBrightnessPicker.Loaded.AddHandler(RoutedEventHandler(fun _ _ -> initializeComponent()))

type HsvColorPicker(selectedColor:SharedValue<Color>) as colorPicker =
    inherit UserControl() 
    (* https://en.wikipedia.org/wiki/List_of_color_spaces_and_their_uses
    HSV is a transformation of Cartesian RGB primaries. HSV (hue, saturation, value)
    is a natural way to think about a color. A perfectly bright color in HSV is analogous 
    to shining a white light on a colored object.  
    *)
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
    
    // Shared Values
    let hueValue = SharedValue<Hue> (0.)
    let saturationValue = SharedValue<Saturation> (1.)
    let brightnessValue = SharedValue<Brightness> (1.)    
    let currentColor = SharedValue<Color> (Colors.Transparent)
    
    // Color Controls
    let hueSlider = 
        let h = HueSlider(hueValue=hueValue)    
        do  h.SetValue(Grid.ColumnProperty,3)
            h.SetValue(Grid.RowProperty,2)

        h
    let saturationSlider = 
        let s = SaturationSlider(saturationValue=saturationValue, currentHue = hueValue)
        do  s.SetValue(Grid.RowProperty,1)
        s
    let brightnessSlider = 
        let l = BrightnessSlider(brightnessValue = brightnessValue, currentHue = hueValue)
        do  l.SetValue(Grid.ColumnProperty,2)
            l.SetValue(Grid.RowProperty,2)
        l
    let saturationAndBrightnessPicker = 
        let s = 
            SaturationAndBrightnessPicker (
                saturationValue = saturationValue,
                brightnessValue = brightnessValue,
                currentHue = hueValue )
        do  
            s.SetValue(Grid.RowProperty,2)            
            s.SetValue(Grid.ColumnProperty,1)
        s
    let hsvColorPicker_Grid = 
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
            
            g.Children.Add(saturationAndBrightnessPicker) |> ignore            
            g.Children.Add(hueSlider) |> ignore
            g.Children.Add(saturationSlider) |> ignore
            g.Children.Add(brightnessSlider) |> ignore

            g.SetValue(Grid.ColumnProperty,0)
        g

    // Color Detail Controls
    let selectedColor_TextBlock = 
        let t = TextBlock(Text = "Selected Color", FontWeight = FontWeights.Bold)
        t.SetValue(Grid.RowProperty,0)
        t    
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
            width = SharedValue(165.),
            height = SharedValue(50.),
            color = selectedColor)   
    let selectedColor_Border = 
        let b = 
            Border(
                BorderBrush = SystemColors.ControlDarkDarkBrush,
                BorderThickness = Thickness(2.),
                Background = CustomBrushes.checkerBrush,
                Margin = Thickness(0.,0.,0.,5.))
        do  b.SetValue(Grid.RowProperty,1)
            b.Child <- selectedColor_shapeContainer
        b

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
    
    let colorDetailControls_Grid = 
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
                
                g.Children.Add(selectedColor_TextBlock) |> ignore
                g.Children.Add(selectedColor_Border) |> ignore
                g.Children.Add(opacity_TextBlock) |> ignore
                g.Children.Add(opacitySlider_Border) |> ignore
            g    
    
    // HSV 
    let hsv_Label = 
        let l = Label(Content = "HSV",FontWeight = FontWeights.Bold)
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,0)
            l.SetValue(Grid.ColumnSpanProperty,2)
        l
    let opacity_Label = 
        let l = Label(Content = "Opacity")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,1)
        l
    let hue_Label = 
        let l = Label(Content = "Hue")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,2)
        l
    let saturation_Label = 
        let l = Label(Content = "Saturation")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,3)
        l
    let brightness_Label = 
        let l = Label(Content = "Brightness")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,4)
        l
    
    let opacity_TextBox = 
        let tb = TextBox(MaxLength = 8,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,1)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_SaturationBrightnessOrOpacity tb.Text with
            | true -> opacity_Slider.Value <- Double.Parse(tb.Text)
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 1, inclusive.") |> ignore
                    tb.Text <- opacity_Slider.Value.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb
    let hue_TextBox = 
        let tb = TextBox(MaxLength = 8,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,2)
        let handleHueChange hue = tb.Text <- hue.ToString()
        do  hueValue.Changed.Add(handleHueChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_Hue tb.Text with
            | true -> 
                let newColor = ColorUtilities.convertHsvToRgb (Double.Parse(tb.Text)) saturationValue.Get brightnessValue.Get
                hueValue.Set (Double.Parse tb.Text)
                selectedColor.Set newColor      
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 360, inclusive.") |> ignore
                    tb.Text <- hueValue.Get.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        
        tb
    let saturation_TextBox = 
        let tb = TextBox(MaxLength = 8,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,3)
        let handleSaturationChange saturation = tb.Text <- saturation.ToString()
        do  saturationValue.Changed.Add(handleSaturationChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_SaturationBrightnessOrOpacity tb.Text with
            | true -> 
                let newColor = ColorUtilities.convertHsvToRgb hueValue.Get (Double.Parse(tb.Text)) brightnessValue.Get
                saturationValue.Set (Double.Parse tb.Text)
                selectedColor.Set newColor      
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 1, inclusive.") |> ignore
                    tb.Text <- saturationValue.Get.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb
    let brightness_TextBox = 
        let tb = TextBox(MaxLength = 8,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,4)
        let handleBrightnessChange brightness = tb.Text <- brightness.ToString()
        do  brightnessValue.Changed.Add(handleBrightnessChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_SaturationBrightnessOrOpacity tb.Text with
            | true -> 
                let newColor = ColorUtilities.convertHsvToRgb hueValue.Get saturationValue.Get (Double.Parse(tb.Text)) 
                brightnessValue.Set (Double.Parse tb.Text)
                selectedColor.Set newColor      
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 1, inclusive.") |> ignore
                    tb.Text <- brightnessValue.Get.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb

    // sRGB 
    let sRGB_Label = 
        let l = Label(Content = "sRGB",FontWeight = FontWeights.Bold)
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,0)
            l.SetValue(Grid.ColumnSpanProperty,2)
        l
    let sA_Label = 
        let l = Label(Content = "A")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,1)
        l
    let sR_Label = 
        let l = Label(Content = "R")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,2)
        l
    let sG_Label = 
        let l = Label(Content = "G")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,3)
        l
    let sB_Label = 
        let l = Label(Content = "B")
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,4)
        l
        
    let sA_TextBox = 
        let tb = TextBox(MaxLength = 5,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,1)        
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_ARGB tb.Text with
            | true -> opacity_Slider.Value <- Double.Parse(tb.Text) / 255.
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 255, inclusive.") |> ignore
                    tb.Text <- System.Math.Truncate(opacity_Slider.Value * 255.).ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))            
        tb
    let sR_TextBox = 
        let tb = TextBox(MaxLength = 5,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,2)
        let handleSelectedColorChange (color:Color) = tb.Text <- color.R.ToString()
        do  selectedColor.Changed.Add(handleSelectedColorChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_ARGB tb.Text with
            | true -> 
                let newColor = Color.FromRgb(Byte.Parse(tb.Text),selectedColor.Get.G,selectedColor.Get.B)                
                let h = ColorUtilities.getHueFromRGB(newColor)
                let s = ColorUtilities.getSaturationFromRGB(newColor)
                let v = ColorUtilities.getBrightnessFromRGB(newColor)
                do  hueValue.Set h
                    saturationValue.Set s
                    brightnessValue.Set v
                    selectedColor.Set newColor
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 255, inclusive.") |> ignore
                    tb.Text <- currentColor.Get.R.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb
    let sG_TextBox = 
        let tb = TextBox(MaxLength = 5,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,3)
        let handleSelectedColorChange (color:Color) = tb.Text <- color.G.ToString()
        do  selectedColor.Changed.Add(handleSelectedColorChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_ARGB tb.Text with
            | true -> 
                let newColor = Color.FromRgb(selectedColor.Get.R,Byte.Parse(tb.Text),selectedColor.Get.B)
                let h = ColorUtilities.getHueFromRGB(newColor)
                let s = ColorUtilities.getSaturationFromRGB(newColor)
                let v = ColorUtilities.getBrightnessFromRGB(newColor)
                do  hueValue.Set h
                    saturationValue.Set s
                    brightnessValue.Set v
                    selectedColor.Set newColor
            | false ->       
                do  MessageBox.Show("Enter a number between 0 and 255, inclusive.") |> ignore
                    tb.Text <- currentColor.Get.G.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb
    let sB_TextBox = 
        let tb = TextBox(MaxLength = 5,MinWidth = 25.)
        do  tb.SetValue(Grid.ColumnProperty,1)
            tb.SetValue(Grid.RowProperty,4)
        let handleSelectedColorChange (color:Color) = tb.Text <- color.B.ToString()
        do  selectedColor.Changed.Add(handleSelectedColorChange)
        let handleTextChanged() = 
            match ColorUtilities.isValidInput_ARGB tb.Text with
            | true -> 
                let newColor = Color.FromRgb(selectedColor.Get.R,selectedColor.Get.G,Byte.Parse(tb.Text)) 
                let h = ColorUtilities.getHueFromRGB(newColor)
                let s = ColorUtilities.getSaturationFromRGB(newColor)
                let v = ColorUtilities.getBrightnessFromRGB(newColor)
                do  hueValue.Set h
                    saturationValue.Set s
                    brightnessValue.Set v
                    selectedColor.Set newColor
            | false ->       
                do  MessageBox .Show("Enter a number between 0 and 255, inclusive.") |> ignore
                    tb.Text <- currentColor.Get.B.ToString()
        do  tb.PreviewKeyUp .AddHandler(KeyEventHandler(fun _ _ -> handleTextChanged()))
        tb

    let hexColor_Label = 
        let l = Label()
        do  l.SetValue(Grid.ColumnProperty,0)
            l.SetValue(Grid.RowProperty,3)
        let handleColorChange (color:Color) = 
            let colorWithOpacity = 
                match Byte.TryParse (sA_TextBox.Text) with
                | true, a -> Color.FromArgb (a,color.R,color.G,color.B)
                | _ -> Color.FromArgb (Byte.Parse("255"),color.R,color.G,color.B)
            do  selectedColor.Set colorWithOpacity
                l.Content <- colorWithOpacity.ToString()        
        do  selectedColor.Changed.Add(handleColorChange)
            sA_TextBox.TextChanged.AddHandler(TextChangedEventHandler(fun _ _ -> handleColorChange selectedColor.Get))
        l 
    
    // detail grids
    let sRGBDetails_Grid = 
        let g = Grid(Margin = Thickness(2.,2.,7.,2.))
        let column1 = ColumnDefinition(Width=GridLength 13.)
        let column2 = ColumnDefinition()
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)

        let row1 = RowDefinition()
        let row2 = RowDefinition()
        let row3 = RowDefinition()
        let row4 = RowDefinition()
        let row5 = RowDefinition()
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.RowDefinitions.Add(row3)
            g.RowDefinitions.Add(row4)
            g.RowDefinitions.Add(row5)

            g.SetValue(Grid.ColumnProperty,2)
            g.SetValue(Grid.RowProperty,1)

            g.Children.Add(sRGB_Label) |> ignore
            g.Children.Add(sA_Label) |> ignore
            g.Children.Add(sR_Label) |> ignore
            g.Children.Add(sG_Label) |> ignore
            g.Children.Add(sB_Label) |> ignore            
            g.Children.Add(sA_TextBox) |> ignore
            g.Children.Add(sR_TextBox) |> ignore
            g.Children.Add(sG_TextBox) |> ignore
            g.Children.Add(sB_TextBox) |> ignore
        g
    let hsvDetails_Grid = 
        let g = Grid(Margin = Thickness(2.,2.,7.,2.))
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)

        let row1 = RowDefinition()
        let row2 = RowDefinition()
        let row3 = RowDefinition()
        let row4 = RowDefinition()
        let row5 = RowDefinition()
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.RowDefinitions.Add(row3)
            g.RowDefinitions.Add(row4)
            g.RowDefinitions.Add(row5)

            g.SetValue(Grid.ColumnProperty,0)
            g.SetValue(Grid.ColumnSpanProperty,2)
            g.SetValue(Grid.RowProperty,1)

            g.Children.Add(hsv_Label) |> ignore
            g.Children.Add(opacity_Label) |> ignore
            g.Children.Add(saturation_Label) |> ignore
            g.Children.Add(hue_Label) |> ignore
            g.Children.Add(brightness_Label) |> ignore            
            g.Children.Add(opacity_TextBox) |> ignore
            g.Children.Add(saturation_TextBox) |> ignore
            g.Children.Add(hue_TextBox) |> ignore
            g.Children.Add(brightness_TextBox) |> ignore
        g
    let colorDetails_Grid = 
        let g = Grid(Margin = Thickness(5.,0.,5.,0.))
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition(Width=GridLength 60.)
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        let row4 = RowDefinition(Height = GridLength.Auto)
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)    
            g.RowDefinitions.Add(row3)
            g.RowDefinitions.Add(row4)

            g.SetValue(Grid.ColumnProperty,1)

            g.Children.Add(colorDetailControls_Grid) |> ignore
            g.Children.Add(hsvDetails_Grid) |> ignore
            g.Children.Add(sRGBDetails_Grid) |> ignore
            g.Children.Add(hexColor_Label) |> ignore
        g    

    do  // Assemble the pieces
        colorPicker_Grid.Children.Add(hsvColorPicker_Grid) |> ignore
        colorPicker_Grid.Children.Add(colorDetails_Grid) |> ignore
        colorPicker.Content <- colorPicker_Grid

    // Event Handlers
    let handleMouseMove (e:MouseEventArgs) =  
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> ()
        | true ->  currentColor.Set(ColorUtilities.convertHsvToRgb (hueValue.Get) 1. 1. )                      
    let handleSelectColor() = 
        let h,s,v = (hueValue.Get), (saturationValue.Get), (brightnessValue.Get)
        let color = ColorUtilities.convertHsvToRgb h s v 
        do  selectedColor.Set(color)
            hue_TextBox.Text <- h.ToString()
            saturation_TextBox.Text <- s.ToString()
            brightness_TextBox.Text <- v.ToString()
            opacity_TextBox.Text <- opacity_Slider.Value.ToString()
            sA_TextBox.Text <- System.Math.Truncate(255. * opacity_Slider.Value) .ToString()
            sR_TextBox.Text <- color.R.ToString()
            sG_TextBox.Text <- color.G.ToString()
            sB_TextBox.Text <- color.B.ToString()
    let handleOpacityOnChange() = 
        do selectedColor_shapeContainer.Opacity <- opacity_Slider.Value
           opacity_TextBox.Text <- opacity_Slider.Value.ToString()
           sA_TextBox.Text <- System.Math.Truncate(255. * opacity_Slider.Value) .ToString()
    
    do  // Add event handlers to control interactions
        hueSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove (e)))
        hueSlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        hueSlider.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> handleSelectColor ()))
        saturationSlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        saturationSlider.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> handleSelectColor ()))
        brightnessSlider.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        brightnessSlider.MouseLeave.AddHandler(Input.MouseEventHandler(fun _ _ -> handleSelectColor ()))
        saturationAndBrightnessPicker.MouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleSelectColor ()))
        opacity_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleOpacityOnChange()))
