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

module CustomBrushes =
    let glassBrush = 
        let gb = 
            LinearGradientBrush(
                StartPoint = Point (0.,0.), 
                EndPoint = Point (1.,1.),
                Opacity = 75.)
        do  gb.GradientStops.Add(GradientStop( Color=Colors.WhiteSmoke, Offset=0.2))
            gb.GradientStops.Add(GradientStop( Color=Colors.Transparent, Offset=0.4))
            gb.GradientStops.Add(GradientStop( Color=Colors.WhiteSmoke, Offset=0.5))
            gb.GradientStops.Add(GradientStop( Color=Colors.Transparent, Offset=0.75))
            gb.GradientStops.Add(GradientStop( Color=Colors.WhiteSmoke, Offset=0.9))
            gb.GradientStops.Add(GradientStop( Color=Colors.Transparent, Offset=1.))
        gb