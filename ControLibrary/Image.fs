namespace ControlLibrary

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes  
open System.Windows.Media.Animation
open System.IO
open System.Windows.Markup
open System.Reflection
open System.Windows.Media.Imaging
open System.Windows.Media.Media3D
open Utilities
open System.Windows.Input

module Image = 
    
    let convertDrawingImage (image:System.Drawing.Image) =
        let bitmap = new System.Windows.Media.Imaging.BitmapImage()
        let memoryStream = new System.IO.MemoryStream()
        do  bitmap.BeginInit()
            image.Save(memoryStream, image.RawFormat)
            memoryStream.Seek(int64 0, System.IO.SeekOrigin.Begin) |> ignore
            bitmap.StreamSource <- memoryStream
            bitmap.EndInit()
        bitmap