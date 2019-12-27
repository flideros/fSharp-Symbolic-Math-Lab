namespace GraphingCalculator
module Models =

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
    open System.Windows.Media.Media3D
    open Utilities

    let testModel = // from Model3DCollection Class example
            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Note: to illuminate an object from additional directions, create 
            // additional lights.
        let model = GeometryModel3D()        
            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
        let geometry = 
            let meshGeometry = MeshGeometry3D()
            // Create a collection of normal vectors for the MeshGeometry3D.
            let normals = 
                let normalCollection = Vector3DCollection()
                do  normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                normalCollection
            // Create a collection of vertex positions for the MeshGeometry3D. 
            let positions = 
                let positionCollection = Point3DCollection()
                do  positionCollection.Add(new Point3D(-0.5,-0.5, 0.5))
                    positionCollection.Add(new Point3D( 0.5,-0.5, 0.5))
                    positionCollection.Add(new Point3D( 0.5, 0.5, 0.5))
                    positionCollection.Add(new Point3D( 0.5, 0.5, 0.5))
                    positionCollection.Add(new Point3D(-0.5, 0.5, 0.5))
                    positionCollection.Add(new Point3D(-0.5,-0.5, 0.5))
                positionCollection
            // Create a collection of texture coordinates for the MeshGeometry3D.
            let textureCoordinates = 
                let textureCoordinatesCollection = PointCollection()
                do  textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                textureCoordinatesCollection
            // Create a collection of triangle indices for the MeshGeometry3D.
            let triangleIndices = 
                let triangleIndicesCollection = Int32Collection()
                do  triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(1)
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(5)
                triangleIndicesCollection
            
            do  meshGeometry.Normals <- normals
                meshGeometry.Positions <- positions
                meshGeometry.TextureCoordinates <- textureCoordinates
                meshGeometry.TriangleIndices <- triangleIndices
                
            meshGeometry
            // The material property of GeometryModel3D specifies the material applied to the 3D object.  
            // In this sample the material applied to the 3D object is made up of two materials layered  
            // on top of each other - a DiffuseMaterial (gradient brush) with an EmissiveMaterial 
            // layered on top (blue SolidColorBrush). The EmmisiveMaterial alters the appearance of  
            // the gradient toward blue
        let material = 
            let linearGradiantBrush = LinearGradientBrush()
            do  linearGradiantBrush.StartPoint <- System.Windows.Point(0., 0.5)
                linearGradiantBrush.EndPoint <- System.Windows.Point(1., 0.5)
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Yellow, 0.0))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Red, 0.25))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Blue, 0.75))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.LimeGreen, 1.0))
            // Define material that will use the gradient.
            let diffuseMaterial = DiffuseMaterial(linearGradiantBrush)
            // Add this gradient to a MaterialGroup.
            let materialGroup = MaterialGroup()
            do  materialGroup.Children.Add(diffuseMaterial)
            // Define an Emissive Material with a blue brush.
            let emissiveMaterial = 
                let c = Color.FromScRgb(1.f,255.f,0.f,0.f)               
                EmissiveMaterial(new SolidColorBrush(c))                
            do  materialGroup.Children.Add(emissiveMaterial)         
            materialGroup            
        do  model.Geometry <- geometry
            model.Material <- material            
        model
    let testModel2 = // from Model3DCollection Class example
            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Note: to illuminate an object from additional directions, create 
            // additional lights.
        let model = GeometryModel3D()        
            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
        let geometry = 
            let meshGeometry = MeshGeometry3D()
            // Create a collection of normal vectors for the MeshGeometry3D.
            let normals = 
                let normalCollection = Vector3DCollection()
                do  normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                    normalCollection.Add(Vector3D(0., 0., 1.))
                normalCollection
            // Create a collection of vertex positions for the MeshGeometry3D. 
            let positions = 
                let positionCollection = Point3DCollection()
                do  positionCollection.Add(new Point3D(0.5, 0.5, -0.5))
                    positionCollection.Add(new Point3D(-0.5, 0.5, -0.5))
                    positionCollection.Add(new Point3D(-0.5, -0.5, -0.5))
                    positionCollection.Add(new Point3D(-0.5, -0.5, -0.5))
                    positionCollection.Add(new Point3D(0.5, -0.5, -0.5))
                    positionCollection.Add(new Point3D(0.5, 0.5, -0.5))
                positionCollection
            // Create a collection of texture coordinates for the MeshGeometry3D.
            let textureCoordinates = 
                let textureCoordinatesCollection = PointCollection()
                do  textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                    textureCoordinatesCollection.Add(System.Windows.Point(0., 1.))
                textureCoordinatesCollection
            // Create a collection of triangle indices for the MeshGeometry3D.
            let triangleIndices = 
                let triangleIndicesCollection = Int32Collection()
                do  triangleIndicesCollection.Add(0)
                    triangleIndicesCollection.Add(1)
                    triangleIndicesCollection.Add(2)
                    triangleIndicesCollection.Add(3)
                    triangleIndicesCollection.Add(4)
                    triangleIndicesCollection.Add(5)
                triangleIndicesCollection
        
            do  meshGeometry.Normals <- normals
                meshGeometry.Positions <- positions
                meshGeometry.TextureCoordinates <- textureCoordinates
                meshGeometry.TriangleIndices <- triangleIndices
            
            meshGeometry
            // The material property of GeometryModel3D specifies the material applied to the 3D object.  
            // In this sample the material applied to the 3D object is made up of two materials layered  
            // on top of each other - a DiffuseMaterial (gradient brush) with an EmissiveMaterial 
            // layered on top (blue SolidColorBrush). The EmmisiveMaterial alters the appearance of  
            // the gradient toward blue
        let material = 
            let linearGradiantBrush = LinearGradientBrush()
            do  linearGradiantBrush.StartPoint <- System.Windows.Point(0., 0.5)
                linearGradiantBrush.EndPoint <- System.Windows.Point(1., 0.5)
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Yellow, 0.0))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Red, 0.25))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Blue, 0.75))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.LimeGreen, 1.0))
            // Define material that will use the gradient.
            let diffuseMaterial = DiffuseMaterial(linearGradiantBrush)
            // Add this gradient to a MaterialGroup.
            let materialGroup = MaterialGroup()
            do  materialGroup.Children.Add(diffuseMaterial)
            // Define an Emissive Material with a blue brush.
            let emissiveMaterial = 
                let c = Color.FromScRgb(1.f,255.f,255.f,255.f)               
                EmissiveMaterial(new SolidColorBrush(c))                
            do  materialGroup.Children.Add(emissiveMaterial)         
            materialGroup
            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.        
        do  model.Geometry <- geometry
            model.Material <- material
            
        model

    let testModel3 = 

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
                do  positionCollection.Add(new Point3D(-0.5,-0.5, 1.0))
                    positionCollection.Add(new Point3D( 0.5,-0.5, 1.0))
                    positionCollection.Add(new Point3D( 0.5, 0.5, 1.0))
                    positionCollection.Add(new Point3D(-0.5, 0.5, 1.0))
                    //Back
                    positionCollection.Add(new Point3D(-1.0,-1.0,-1.0))
                    positionCollection.Add(new Point3D( 1.0,-1.0,-1.0))
                    positionCollection.Add(new Point3D( 1.0, 1.0,-1.0))
                    positionCollection.Add(new Point3D(-1.0, 1.0,-1.0))
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
        let material = 
            let linearGradiantBrush = LinearGradientBrush()
            do  linearGradiantBrush.StartPoint <- System.Windows.Point(0., 0.5)
                linearGradiantBrush.EndPoint <- System.Windows.Point(1., 0.5)
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.YellowGreen, 0.0))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Red, 0.25))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Blue, 0.75))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Chocolate, 1.0))
            // Define material that will use the gradient.
            let diffuseMaterial = DiffuseMaterial(linearGradiantBrush)
            // Add this gradient to a MaterialGroup.
            let materialGroup = MaterialGroup()
            do  materialGroup.Children.Add(diffuseMaterial)
            // Define an Emissive Material with a blue brush.
            let emissiveMaterial = 
                let c = Colors.Chocolate              
                EmissiveMaterial(new SolidColorBrush(c))                
            do  materialGroup.Children.Add(emissiveMaterial)         
            materialGroup
            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.        
        let model = GeometryModel3D(geometry,material)
        
        model

    let testModel4 = 

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
                do  positionCollection.Add(new Point3D(-0.05,-0.05, 1.0))
                    positionCollection.Add(new Point3D( 0.05,-0.05, 1.0))
                    positionCollection.Add(new Point3D( 0.05, 0.05, 1.0))
                    positionCollection.Add(new Point3D(-0.05, 0.05, 1.0))
                    //Back
                    positionCollection.Add(new Point3D(-0.05,-0.05, -1.0))
                    positionCollection.Add(new Point3D( 0.05,-0.05, -1.0))
                    positionCollection.Add(new Point3D( 0.05, 0.05, -1.0))
                    positionCollection.Add(new Point3D(-0.05, 0.05, -1.0))
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
        let material = 
            let linearGradiantBrush = LinearGradientBrush()
            do  linearGradiantBrush.StartPoint <- System.Windows.Point(0., 0.5)
                linearGradiantBrush.EndPoint <- System.Windows.Point(1., 0.5)
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.YellowGreen, 0.0))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Red, 0.25))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Blue, 0.75))
                linearGradiantBrush.GradientStops.Add(GradientStop(Colors.Chocolate, 1.0))
            // Define material that will use the gradient.
            let diffuseMaterial = DiffuseMaterial(linearGradiantBrush)
            // Add this gradient to a MaterialGroup.
            let materialGroup = MaterialGroup()
            do  materialGroup.Children.Add(diffuseMaterial)
            // Define an Emissive Material with a blue brush.
            let emissiveMaterial = 
                let c = Colors.Chocolate              
                EmissiveMaterial(new SolidColorBrush(c))                
            do  materialGroup.Children.Add(emissiveMaterial)         
            materialGroup
            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.        
        let model = GeometryModel3D(geometry,material)
        
        model