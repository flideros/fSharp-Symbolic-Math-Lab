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



type SaturationBrightnessPicker () as saturationBrightnessPicker =
     inherit UserControl() 

     let saturationBrightnessPicker_ShapeContainer = 
         let c = 
             ShapeContainer(
                 shapes = SharedValue ControlLibrary.Shapes.Rectangle,
                 width = SharedValue 400.,
                 height = SharedValue 400.,
                 color = SharedValue Colors.Black)
         c
     do saturationBrightnessPicker.Content <- saturationBrightnessPicker_ShapeContainer

type ColorPicker() as colorPicker =
    inherit UserControl() 
    
    let colorPicker_Grid = 
        let g = 
            Grid(
                Background = SolidColorBrush(Color.FromArgb(byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE")),
                Height = 400.,
                Width = 600.
                )
        let column1 = ColumnDefinition(Width = GridLength(600.))
        let column2 = ColumnDefinition(Width = GridLength(0.,GridUnitType.Star))
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)

        let row1 = RowDefinition(Height = GridLength(400.))
        let row2 = RowDefinition(Height = GridLength(0.,GridUnitType.Star))
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
        g
    
    let colorSwatch1_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch1.png", UriKind.RelativeOrAbsolute))
    let colorSwatch2_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch2.png", UriKind.RelativeOrAbsolute))
    let colorSwatch3_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch3.png", UriKind.RelativeOrAbsolute))
    let colorSwatch4_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch4.png", UriKind.RelativeOrAbsolute))
    let colorSwatch5_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch5.png", UriKind.RelativeOrAbsolute))
    let colorSwatch1_Image = 
        let i = 
            Image(
                Height = 20.,
                Width = 20.,
                Source = colorSwatch1_Bitmap,
                Margin = Thickness(left=45.,top=0.,right=0.,bottom=0.),
                ToolTip = "Color Swatch 1"                
                )
        i
    let colorSwatch2_Image =
        let i = 
            Image(
                Height = 20.,
                Width = 20.,
                Source = colorSwatch2_Bitmap,
                Margin = Thickness(left=5.,top=0.,right=0.,bottom=0.),
                ToolTip = "Color Swatch 2"                
                )
        i
    let colorSwatch3_Image =
        let i = 
            Image(
                Height = 20.,
                Width = 20.,
                Source = colorSwatch3_Bitmap,
                Margin = Thickness(left=5.,top=0.,right=0.,bottom=0.),
                ToolTip = "Color Swatch 3"                
                )
        i
    let colorSwatch4_Image =
        let i = 
            Image(
                Height = 20.,
                Width = 20.,
                Source = colorSwatch2_Bitmap,
                Margin = Thickness(left=5.,top=0.,right=0.,bottom=0.),
                ToolTip = "Color Swatch 4"                
                )
        i
    let colorSwatch5_Image =
        let i = 
            Image(
                Height = 20.,
                Width = 20.,
                Source = colorSwatch5_Bitmap,
                Margin = Thickness(left=5.,top=0.,right=0.,bottom=0.),
                ToolTip = "Color Swatch 5"                
                )
        i    
    let colorSwatch_StackPanel = 
        let sp = 
            StackPanel(
                //Background = SolidColorBrush(Colors.Black),                
                Height = 31.,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Horizontal
                )
        do sp.SetValue(Grid.RowProperty, 0)
        sp
    let colorSwatch_Label = 
        let l = 
            Label(
                Content = " Choose a Swatch",
                Foreground = SolidColorBrush(Colors.Black),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
                )
        l    
    
    do  colorSwatch_StackPanel.Children.Add(colorSwatch_Label)  |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch1_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch2_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch3_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch4_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch5_Image) |> ignore
    
    let hueValue = SharedValue<Hue>(0.)
    let currentColor = SharedValue<Color> (Colors.Transparent)


    let hueSlider = HueSlider(hueValue=hueValue)
    do  hueSlider.SetValue(Grid.ColumnProperty,1)

    let saturationSlider = SaturationSlider(saturationValue=SharedValue (1.), currentHue = hueValue)
    do  saturationSlider.SetValue(Grid.ColumnProperty,2)

    let colorTab_Grid =         
        let g = 
            Grid()
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength(50., GridUnitType.Star))
        
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)        
        g
    let colorTab_TabControl =         
        let t = 
            TabControl(
                Margin = Thickness(left=0.,top=0.,right=0.,bottom=0.),                
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch)
        t
    let colorTabItem1_TabItem  =         
        let colorSwatch = 
            let i = 
                Image(
                    //Height = 240.,
                    //Width = 320.,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Source = colorSwatch5_Bitmap,
                    Margin = Thickness(0.))
            i
        let tab = TabItem(Header = "Color Picker 1")
        let g = Grid()
        
        let row1 = RowDefinition(Height = GridLength(31.))
        let row2 = RowDefinition(Height = GridLength.Auto)
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
        
        do  g.Children.Add(colorSwatch) |> ignore
            g.Children.Add(colorSwatch_StackPanel) |> ignore
            colorSwatch.SetValue(Grid.RowProperty, 1)
            tab.Content <- g 
        tab
    let xyLabel = Label(Content = "x y")
    let colorTabItem2_TabItem =         
        let colorSwatch = 
            let i = 
                Image(
                    //Height = 400.,
                    //Width = 600.,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Source = colorSwatch4_Bitmap,
                    Margin = Thickness(0.))
            i//I might delete this function...
        let tab = TabItem(Header = "Color Picker 2")
        let g = Grid()        
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength(50., GridUnitType.Star))
        
        do  g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)
        
            g.Children.Add(colorSwatch) |> ignore
            //g.Children.Add(SaturationBrightnessPicker ()) |> ignore
            g.Children.Add(xyLabel) |> ignore
            g.Children.Add(hueSlider) |> ignore
            g.Children.Add(saturationSlider) |> ignore
            tab.Content <- g 
        tab

    do  colorTab_TabControl.Items.Add(colorTabItem1_TabItem) |> ignore
        colorTab_TabControl.Items.Add(colorTabItem2_TabItem) |> ignore
        colorTab_Grid.Children.Add(colorTab_TabControl) |> ignore

    
        colorPicker_Grid.Children.Add(colorTab_Grid) |> ignore
                
        colorPicker.Content <- colorPicker_Grid

    // Setters
    let setSwatch swatch = 
        let g = Grid()
        do  g.Children.Add(swatch) |> ignore
            colorTabItem1_TabItem.Content <- g

    let handleMouseMove (e:MouseEventArgs) =  
        match e.LeftButton = MouseButtonState.Pressed with
        | false -> do xyLabel.Content <- hueValue.Get.ToString()
        | true ->  do xyLabel.Content <- hueValue.Get.ToString()
                      currentColor.Set(ColorUtilities.convertHsvToRgb (hueValue.Get) 1. 1. )

    do  hueSlider.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleMouseMove (e)))
