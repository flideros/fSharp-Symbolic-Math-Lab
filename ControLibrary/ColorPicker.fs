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

type ColorPicker() as colorPicker =
    inherit UserControl() 
    
    let colorPicker_Grid = 
        let g = 
            Grid(
                Background = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE")),
                Height = 330.,
                Width = 480.
                )
        let row1 = RowDefinition(Height = GridLength(35.))
        let row2 = RowDefinition(Height = GridLength(200.))
        let row3 = RowDefinition(Height = GridLength(64.))
        let row4 = RowDefinition(Height = GridLength(6., GridUnitType.Star))
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.RowDefinitions.Add(row3)
            g.RowDefinitions.Add(row4)
        g
    
    let colorSwatch_StackPanel = 
        let sp = 
            StackPanel(
                Background = SolidColorBrush(Colors.Black),                
                Height = 35.,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Horizontal
                )
        do sp.SetValue(Grid.RowProperty, 0)
        sp
    let colorSwatch_Label = 
        let l = 
            Label(
                Content = " Choose a Swatch",
                Foreground = SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
                )
        l    
    let colorSwatch1_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch1.png", UriKind.RelativeOrAbsolute))
    let colorSwatch2_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch2.png", UriKind.RelativeOrAbsolute))
    let colorSwatch3_Bitmap = new BitmapImage(new Uri(__SOURCE_DIRECTORY__ + "/ColorSwatch3.png", UriKind.RelativeOrAbsolute))
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
    
    do  colorSwatch_StackPanel.Children.Add(colorSwatch_Label) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch1_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch2_Image) |> ignore
        colorSwatch_StackPanel.Children.Add(colorSwatch3_Image) |> ignore

    let main_Grid = 
        let g = 
            Grid(
                Background = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE")),
                Margin = Thickness(left=0.,top=0.,right=0.,bottom=34.),
                Height = 330.,
                Width = 480.
                )        

        let column1 = ColumnDefinition(Width = GridLength(170.))
        let column2 = ColumnDefinition(Width = GridLength(164.))
        let column3 = ColumnDefinition(Width = GridLength(176.))
        do  g.SetValue(Grid.RowProperty, 1)
            g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(column2)
            g.ColumnDefinitions.Add(column3)
        g

    let colorBox_Grid =         
        let g = 
            Grid(
                Margin = Thickness(left=10.,top=30.,right=0.,bottom=0.)
                )        
        do  g.SetValue(Grid.ColumnProperty, 0)
            g.SetValue(Grid.RowProperty, 0)
        g
    let colorBox_Border =         
        let b = 
            Border(
                BorderBrush = SolidColorBrush(Colors.Black),
                BorderThickness = Thickness(2.),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Background = SolidColorBrush(Colors.White),
                Width = 154.,
                Height = 154.
                )        
        b
    let colorBox_Image =
        let i = 
            Image(
                Height = 150.,
                Width = 150.,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Source = colorSwatch1_Bitmap,
                Margin = Thickness(2.)
                )
        i
    let colorBox_Canvas = 
        let g = 
            Canvas(
                Height = 150.,
                Width = 150.,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Background = SolidColorBrush(Colors.Transparent),
                Margin = Thickness(2.)
                )        
        do  g.SetValue(Grid.ColumnProperty, 0)
            g.SetValue(Grid.RowProperty, 0)
        g
    
    do  colorBox_Grid.Children.Add(colorBox_Border) |> ignore
        colorBox_Grid.Children.Add(colorBox_Image) |> ignore
        colorBox_Grid.Children.Add(colorBox_Canvas) |> ignore
        main_Grid.Children.Add(colorBox_Grid) |> ignore
        
        colorPicker_Grid.Children.Add(colorSwatch_StackPanel) |> ignore
        
        colorPicker_Grid.Children.Add(main_Grid) |> ignore

        colorPicker.Content <- colorPicker_Grid