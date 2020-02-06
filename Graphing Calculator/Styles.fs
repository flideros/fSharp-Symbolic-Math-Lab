namespace GraphingCalculator

open System
open System.Windows
open System.Windows.Controls  
open System.Windows.Media
open System.Windows.Shapes
open System.IO
open System.Windows.Markup
open System.Windows.Controls
open System.Reflection
open System.Windows.Media.Imaging
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Media3D
open System.Windows.Shapes
open Microsoft.Win32

open Utilities

module Style = 
    
    // shared values
    let selectedColor = ControlLibrary.SharedValue<Color>(Color.FromScRgb(1.f,160.f,0.f,0.f))

    //Materials
    let genericMaterial = 
        let linearGradiantBrush = LinearGradientBrush()
        do  linearGradiantBrush.StartPoint <- System.Windows.Point(0., 0.5)
            linearGradiantBrush.EndPoint <- System.Windows.Point(1., 0.5)
            linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Yellow, 0.0))
            linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Magenta, 0.25))
            linearGradiantBrush.GradientStops.Add(GradientStop(Colors.LightBlue, 0.75))
            linearGradiantBrush.GradientStops.Add(GradientStop(Colors.LimeGreen, 1.0))
        // Define material that will use the gradient.
        let diffuseMaterial = DiffuseMaterial(linearGradiantBrush)
        // Add this gradient to a MaterialGroup.
        let materialGroup = MaterialGroup()
        do  materialGroup.Children.Add(diffuseMaterial)
        // Define an Emissive Material with a blue brush.
        let emissiveMaterial c = 
            let c = selectedColor.Get              
            EmissiveMaterial(SolidColorBrush(c))                
        let handleColorChange c = 
            do  materialGroup.Children.Clear()
                materialGroup.Children.Add(diffuseMaterial)
                materialGroup.Children.Add(EmissiveMaterial(SolidColorBrush(c)))
        do  materialGroup.Children.Add(emissiveMaterial selectedColor.Get)
            selectedColor.Changed.Add(handleColorChange)

        materialGroup

    
    //Images
    let checkedBox = Image(Source = new BitmapImage(new Uri((__SOURCE_DIRECTORY__ + "/5091-512.png"), UriKind.RelativeOrAbsolute)),Width=20., Height=20. )
    
    //Colors
    let black = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0x00", byte "0x00", byte "0x00"))
    let screenColor = SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE"))
    let transparentBlack = SolidColorBrush(Color.FromArgb (byte "0x00",  byte "0x00", byte "0x00", byte "0x00"))

    //Linear Gradient Brush
    let linearGradientBrush_1 = 
        let brush = LinearGradientBrush(StartPoint = Point (0.,0.), EndPoint = Point (0.03,0.9))
        do
            brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0x80", byte "0x80", byte "0x80"), Offset = 0.))
            brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xA0", byte "0xA0", byte "0xA0"), Offset = 1.))
        brush
    
    let linearGradientBrush_2 = 
        let brush = LinearGradientBrush(StartPoint = Point (0.,0.), EndPoint = Point (0.03,0.9))
        do
            brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0x60",  byte "0x80", byte "0x80", byte "0x80"), Offset = 0.))
            brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0x60",  byte "0xA0", byte "0xA0", byte "0xA0"), Offset = 1.))
        brush
    
    //Radial Gradient Brush
    let radialGradientBrush = new RadialGradientBrush()
    do
        RadialGradientBrush().GradientStops.Add(new GradientStop(Color = Colors.Green, Offset = 0.))
        RadialGradientBrush().GradientStops.Add(new GradientStop(Color = Colors.Gray, Offset = 1.))

    //Button
    type CalcButton() as calcButton = 
        inherit Button()
    
        do
            calcButton.Margin <- Thickness(left=0.,top=0.,right=0.,bottom=0.)
            calcButton.FontSize <- 16.
            calcButton.BorderBrush <- SolidColorBrush(Colors.Black)
            calcButton.BorderThickness <- Thickness(2.)
            calcButton.HorizontalAlignment <- HorizontalAlignment.Center
            calcButton.Margin <- Thickness(Left = 2., Top = 2., Right = 2., Bottom = 2.)//162., Top = 75., Right = 0., Bottom = 0.),
            calcButton.VerticalAlignment <- VerticalAlignment.Top
            calcButton.Width <- 50.
            calcButton.Height <- 30.
            calcButton.Background <- 
                let brush = LinearGradientBrush(StartPoint = Point (0.,0.), EndPoint = Point (0.03,0.9))
                do
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xCC", byte "0xCC", byte "0xCC"), Offset = 0.))
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xEE", byte "0xEE", byte "0xEE"), Offset = 1.))
                brush//radialGradientBrush//

    type FuncButton() as funcButton = 
        inherit Button()
        
        do
            funcButton.Margin <- Thickness(left=5.,top=5.,right=0.,bottom=0.)
            funcButton.Height <- 25.
            funcButton.Width <- 38.
            funcButton.BorderBrush <- black
            funcButton.FontSize <- 16.
            funcButton.Background <- 
                let brush = linearGradientBrush_1
                do
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0x80", byte "0x80", byte "0x80"), Offset = 0.))
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xA0", byte "0xA0", byte "0xA0"), Offset = 1.))
                brush
            funcButton.Foreground <- SolidColorBrush(Colors.DarkBlue)
            
    type basicCalcButton() as funcButton = 
        inherit Button()
     
        do
            funcButton.Margin <- Thickness(left=7.,top=0.,right=7.,bottom=0.)
            funcButton.Height <- 30.
            funcButton.Width <- 50.
            funcButton.BorderBrush <- black
            funcButton.FontSize <- 16.
            funcButton.Background <- 
                let brush = LinearGradientBrush(StartPoint = Point (0.,0.), EndPoint = Point (0.03,0.9))
                do
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0x80", byte "0x80", byte "0x80"), Offset = 0.))
                    brush.GradientStops.Add(new GradientStop(Color = Color.FromArgb (byte "0xFF",  byte "0xA0", byte "0xA0", byte "0xA0"), Offset = 1.))
                brush
            funcButton.Foreground <- SolidColorBrush(Color.FromArgb (byte "0xFF",  byte "0xE0", byte "0xE0", byte "0xE0"))

    type FunctionButton() as button = 
        inherit Button()
        do  button.FontSize <- 14.
            button.Margin <- Thickness(left = 5.,top = 5.,right = 5.,bottom = 0.)
            
    //Text box
    type CalcTextBox() as textBox = 
        inherit TextBox()
        do textBox.FontFamily <- FontFamily("Courier New")
        do textBox.FontSize <- 22.
   
    type FunctionTextBox() as textBox = 
        inherit TextBox()
        do  textBox.FontFamily <- FontFamily("Courier New")
            textBox.MaxLines <- 1
            textBox.Margin <- Thickness(left = 5.,top = 0.,right = 5.,bottom = 0.) 

    //Text block
    type FunctionTextBlock() as textBox = 
        inherit TextBlock()
        do  textBox.FontSize <- 14.
            textBox.Margin <- Thickness(left = 5.,top = 5.,right = 5., bottom = 5.)

module Pixel =

    let Box () = 
        let geometry = 
            let meshGeometry = MeshGeometry3D()
            // Create a collection of normal vectors for the MeshGeometry3D.
            let normals = 
                let normalCollection = Vector3DCollection()
                do  normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                
                    normalCollection.Add(Vector3D(0., 0., -1.))
                    normalCollection.Add(Vector3D(0., 0., -1.))
                    normalCollection.Add(Vector3D(0., 0., -1.))
                    normalCollection.Add(Vector3D(0., 0., -1.))

                normalCollection
            // Create a collection of vertex positions for the MeshGeometry3D. 
            let positions = 
                let positionCollection = Point3DCollection()
                    //Front
                do  positionCollection.Add(new Point3D(-0.05,-0.05, 0.1))
                    positionCollection.Add(new Point3D( 0.05,-0.05, 0.1))
                    positionCollection.Add(new Point3D( 0.05, 0.05, 0.1))
                    positionCollection.Add(new Point3D(-0.05, 0.05, 0.1))
                    //Back
                    positionCollection.Add(new Point3D(-0.05,-0.05, -0.00))
                    positionCollection.Add(new Point3D( 0.05,-0.05, -0.00))
                    positionCollection.Add(new Point3D( 0.05, 0.05, -0.00))
                    positionCollection.Add(new Point3D(-0.05, 0.05, -0.00))
                positionCollection
       
            // Create a collection of triangle indices for the MeshGeometry3D.
            let triangleIndices = 
                let triangleIndicesCollection = Int32Collection()
                    //front
                do  triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(1)
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(0)
                    //
                    triangleIndicesCollection.Add(6)
                    triangleIndicesCollection.Add(5)
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(7)
                    triangleIndicesCollection.Add(6)
                    //
                    triangleIndicesCollection.Add(1)
                    triangleIndicesCollection.Add(5)
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(5)
                    triangleIndicesCollection.Add(6)
                    triangleIndicesCollection.Add(2)
                    //
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(6)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(6)
                    triangleIndicesCollection.Add(7)
                    //
                    triangleIndicesCollection.Add(5)
                    triangleIndicesCollection.Add(1)
                    triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(5)
                    //
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(7)
                    triangleIndicesCollection.Add(4)
                    //
            
                triangleIndicesCollection

            do  meshGeometry.Normals <- normals
                meshGeometry.Positions <- positions
                meshGeometry.TriangleIndices <- triangleIndices

            meshGeometry            
        let model = GeometryModel3D(geometry,Style.genericMaterial)             
        model
