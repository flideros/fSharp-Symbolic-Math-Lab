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

    let spectrumBrush = 
        let spectrumColors = 
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

            seq{for i in 0.0..1.0..28.0 -> convertHsvToRgb (i*12.) 1. 1.}
        let step = 1. / float (Seq.length spectrumColors)  //need to catch divide by zero
        let sb = 
            LinearGradientBrush(
                StartPoint = Point (0.5,0.), 
                EndPoint = Point (0.5,1.),
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation)
        
        do  Seq.iteri (fun i c -> sb.GradientStops.Add(GradientStop( Color = c, Offset=(float i) * step))) spectrumColors
            
        sb