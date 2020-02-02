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
    open ColorUtilities

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

    let spectrumBrush = 
        let spectrumColors = seq{for i in 0.0..1.0..30.0 -> convertHsvToRgb (i*12.) 1. 1.}
        let step = 1. / float (Seq.length spectrumColors)  //need to catch divide by zero here
        let sb = 
            LinearGradientBrush(
                StartPoint = Point (0.5,0.), 
                EndPoint = Point (0.5,1.),
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation)
        
        do  Seq.iteri (fun i c -> sb.GradientStops.Add(GradientStop( Color = c, Offset=(float i) * step))) spectrumColors
            
        sb

    let checkerBrush = 
        let square1 = RectangleGeometry(Rect(Point(0.,0.),Point(100.,100.)))
        let square2 = RectangleGeometry(Rect(Point(0.,0.),Point(50.,50.)))
        let square3 = RectangleGeometry(Rect(Point(50.,50.),Point(100.,100.)))
    
        let gd1 = GeometryDrawing(Brush = Brushes.White)
        do  gd1.Geometry <- square1

        let gd2 = GeometryDrawing(Brush = Brushes.LightGray)
        let gg2 = GeometryGroup()
        do  gg2.Children.Add(square2)
            gg2.Children.Add(square3)
            gd2.Geometry <- gg2

        let dg1 = DrawingGroup()
        do  dg1.Children.Add(gd1)
            dg1.Children.Add(gd2)

        let db = 
            DrawingBrush(Viewport = Rect(0.,0.,10.,10.),
                         ViewportUnits = BrushMappingMode.Absolute,
                         TileMode = TileMode.Tile
                        )
        do  db.Drawing <- dg1
        db
