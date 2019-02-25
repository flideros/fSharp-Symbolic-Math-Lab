module Style

open System
open System.Windows          
open System.Windows.Controls
open System.Windows.Media
open TypeExtension
open UI
open Operator
open System.Windows.Shapes
open System.IO
open System.Windows.Markup
open System.Reflection
open System.Windows.Media.Imaging


//\/--- Border Style ------------------------------------------------------------------\/

type BorderStyle() as border = 
    inherit Border();
    do
        border.Background <- Brushes.LightBlue;
        border.BorderBrush <- Brushes.Black;
        border.BorderThickness <- Thickness(2.);
        border.CornerRadius <- CornerRadius(45.);
        border.Padding <- Thickness(25.);
        border.VerticalAlignment <- VerticalAlignment.Stretch
        border.HorizontalAlignment <- HorizontalAlignment.Stretch
    
//\--- Border Style -------------------------------------------------------------------/\


//\/--- Button Styles --------------------------------------------------------------------\/

type ButtonStyle(name:string) as button =
     inherit Button()

     let strokeBrushColor = Color()

     let rectangle = Rectangle()
     let border = Border()
     //let buttonFocusVisual = 
     

     do
        rectangle.Margin <- Thickness(2.)
        rectangle.Stroke <- Brushes.AliceBlue
        //button.Background <- Brushes.AliceBlue
        //button.BorderBrush <- Brushes.Red
        button.Content <- name // add some error handling code
        button.SnapsToDevicePixels <- true
        button.OverridesDefaultStyle <- true
        //button.FocusVisualStyle <- Value="{StaticResource ButtonFocusVisual}"
        button.MaxHeight <- 23.
        button.MinWidth <- 75.
        //button.Template <- 




//\--- Button Styles ----------------------------------------------------------------------/\


//\/--- StackPanel Style ------------------------------------------------------------------\/
 
type StackPanelStyle() as stackPanel =
    inherit StackPanel()
    do 
        stackPanel.Margin <- Thickness(10.)

//\--- StackPanel Style -------------------------------------------------------------------/\

//\/--- StackPanel Style ------------------------------------------------------------------\/
 
type DockPanelStyle() as dockPanel =
    inherit DockPanel()
    do 
        dockPanel.Margin <- Thickness(10.)
        dockPanel.LastChildFill <- true
        
//\--- StackPanel Style -------------------------------------------------------------------/\