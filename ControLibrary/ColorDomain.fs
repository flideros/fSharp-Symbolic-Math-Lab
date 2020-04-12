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

type Hue = float
type Saturation = float
type Brightness = float
type Opacity = float

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

    let getSaturationFromRGB (color : Color) =        
        let r,g,b = (float) color.R/255.,(float) color.G/255.,(float) color.B/255.
        let cMax = System.Math.Max(r,(System.Math.Max(g,b)))
        let cMin = System.Math.Min(r,(System.Math.Min(g,b)))
        let delta = cMax-cMin
        match cMax = 0. with
        | true -> 0.
        | false -> delta/cMax

    let getBrightnessFromRGB (color : Color) =        
        let r,g,b = (float) color.R/255.,(float) color.G/255.,(float) color.B/255.
        let cMax = System.Math.Max(r,(System.Math.Max(g,b)))
        cMax
    
    let isValidInput_ARGB (text:string) =
        let n = Double.TryParse text 
        match n with 
        | true, n when n >= 0. && n <= 255. -> true
        | _, _ -> false

    let isValidInput_Hue (text:string) =
        let n = Double.TryParse text 
        match n with 
        | true, n when n >= 0. && n <= 360. -> true
        | _, _ -> false

    let isValidInput_SaturationBrightnessOrOpacity (text:string) =
        let n = Double.TryParse text 
        match n with 
        | true, n when n >= 0. && n <= 1. -> true
        | _, _ -> false

    let stringToColorBrush s = 
        match s with
        | "black" | "Black" -> Brushes.Black
        | _ -> Brushes.Transparent