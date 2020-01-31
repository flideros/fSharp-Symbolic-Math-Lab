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

module ColorUtilities =
    
    let convertHsvToRgb h s v =
        let h = match h = 360. with | true -> 0. | false -> h/60.
        let i = Math.Truncate(h)
        let f = h - i
        let p = v * (1. - s)
        let q = v * (1. - (s * f))
        let t = v * (1. - (s * (1. - f)))
        match s,i with
        | 0., _ -> Color.FromArgb(byte( 255 ),byte( v * 255. ),byte( v * 255. ),byte( v * 255. ))
        | _, 0. -> Color.FromArgb(byte( 255 ),byte( v * 255. ),byte( t * 255. ),byte( p * 255. ))
        | _, 1. -> Color.FromArgb(byte( 255 ),byte( q * 255. ),byte( v * 255. ),byte( p * 255. ))
        | _, 2. -> Color.FromArgb(byte( 255 ),byte( p * 255. ),byte( v * 255. ),byte( t * 255. ))
        | _, 3. -> Color.FromArgb(byte( 255 ),byte( p * 255. ),byte( q * 255. ),byte( v * 255. ))
        | _, 4. -> Color.FromArgb(byte( 255 ),byte( t * 255. ),byte( p * 255. ),byte( v * 255. ))
        | _, _  -> Color.FromArgb(byte( 255 ),byte( v * 255. ),byte( p * 255. ),byte( q * 255. ))

    let getHueFromRGB (color : Color) =        
        let r,g,b = (float) color.R/255.,(float) color.G/255.,(float) color.B/255.
        let cMax = System.Math.Max(r,(System.Math.Max(g,b)))
        let cMin = System.Math.Min(r,(System.Math.Min(g,b)))
        let delta = cMax-cMin
        match delta < 0.00001 with
        | true -> 0.
        | false -> 
            let h =
                match cMax with
                | x when x = r -> ((g-b)/delta) % 6.
                | x when x = g -> ((b-r)/delta) + 2.
                | x when x = b -> ((r-g)/delta) + 4.
                | _  -> 0.         
            match h < 0. with
            | true -> (60. * h) + 360.
            | false -> 60. * h

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
