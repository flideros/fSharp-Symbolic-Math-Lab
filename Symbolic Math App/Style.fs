module Style

open System.Windows          
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Markup

//\/--- Border Style ---------------------------------------------------------------------\/

type BorderStyle() as border = 
    inherit Border()
    do
        //border.Background <- Brushes.LightBlue;
        //border.BorderBrush <- Brushes.Black;
        border.BorderThickness <- Thickness(2.)
        border.CornerRadius <- CornerRadius(45.)
        border.Padding <- Thickness(25.)
        border.VerticalAlignment <- VerticalAlignment.Stretch
        border.HorizontalAlignment <- HorizontalAlignment.Stretch
        // Get style from application resources in app.xmal
        border.Style <- new Style(border.GetType(),border.FindResource(Border().GetType()):?>Style)
    
//\--- Border Style ----------------------------------------------------------------------/\


//\/--- Button Styles --------------------------------------------------------------------\/

type ButtonStyle(name:string) as button =
     inherit Button()
     
     do       
        button.Content <- name // add some error handling code        
        // Get style from application resources in app.xmal
        button.Style <- new Style(button.GetType(),button.FindResource(Button().GetType()):?>Style)            
        button.MaxHeight <- 30.
        button.MinWidth <- 80.        
        
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