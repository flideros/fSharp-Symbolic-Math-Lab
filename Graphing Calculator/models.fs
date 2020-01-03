// This module is not part of the Graphing Calculator implementation.
// I'm using this code to develop the algorithms for the implementation.
// But, I'm going to leave this module in the project for it's value
// as code examples and prototyping area.
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
    
    let testModel = 
            // from Model3DCollection Class example
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

    let makeBar () = 
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
                    
                triangleIndicesCollection

            do  meshGeometry.Normals <- normals
                meshGeometry.Positions <- positions
                meshGeometry.TriangleIndices <- triangleIndices    
            meshGeometry         
        let model = GeometryModel3D(geometry,material)               
        model
    let makeBox () = 
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
        let model = GeometryModel3D(geometry,material)             
        model
    
    let transformModel (model :GeometryModel3D) (v1 :Vector3D)  (point :Point3D) = 
        let v2 =             
            let v = Vector3D(0.,0.,1.)
            do  v.Normalize()
            v
        
        let transforms = Transform3DGroup()
        let translation = TranslateTransform3D(Vector3D(point.X,point.Y,point.Z))
        
        let axis = Vector3D.CrossProduct (v1, v2)        
        let angle = axis.Length * (180./System.Math.PI)
        let quaternion = 
            let q = (Quaternion(axis, angle))
            do q.Normalize()
            q
        let rotation = QuaternionRotation3D(quaternion) |> RotateTransform3D
        do  transforms.Children.Add(rotation)
            transforms.Children.Add(translation)
            model.Transform <- transforms
        model

    let helix = 
        
        let x(t) = sin(t)
        let y(t) = cos(t)
        let z(t) = -t 
        let points0 = seq{for t in 0.0..0.01..25.0 -> Point3D(x(t), y(t), z(t))} 
        let points1 = seq{for t in 0.01..0.01..25.01 -> Point3D(x(t), y(t), z(t))} 
        let points = Seq.zip points0 points1

        let normals = 
            seq{for p0, p1 in points 
                -> let vector = Vector3D( p1.X, p1.Y, p1.Z) - Vector3D( p0.X, p0.Y, p0.Z)
                   do vector.Normalize()
                   vector}        
        let pointsAndNormals = Seq.zip points0 normals        
        let models = seq{for p,n in pointsAndNormals -> 
                            let model = makeBox ()
                            transformModel model n p}
        let model3DGroup = Model3DGroup()        
        do Seq.iter (fun m -> model3DGroup.Children.Add(m)) models
        model3DGroup

    let surface = 
           
           // Surface example
           (*let x(u,v) = u
           let y(u,v) = v
           let z(u,v) = sin(u**2. + v**2.)*)
           
           // Torus example
           (*let x(u,v) = sin(v)
           let y(u,v) = (2.+cos(v))*sin(u)
           let z(u,v) = (2.+cos(v))*cos(u)*)
           
           // Sphere example
           let x(u,v) = cos(u)*sin(v)
           let y(_u,v) = -cos(v)
           let z(u,v) = sin(-u)*sin(v)

           let resolution  = 0.05 * System.Math.PI
           let uLowerLimit = 0.0
           let uUpperLimit = 2.*System.Math.PI
           let vLowerLimit = 0.0
           let vUpperLimit = 2.*System.Math.PI


           let points0 = 
               seq{for u in uLowerLimit..resolution..uUpperLimit do 
                     for v in vLowerLimit..resolution..vUpperLimit -> Point3D(x(u, v), y(u, v), z(u, v))}
           let points1 = 
               seq{for u in uLowerLimit..resolution..uUpperLimit do 
                     for v in vLowerLimit..resolution..vUpperLimit -> Point3D(x(u, v + resolution), y(u, v + resolution), z(u, v + resolution))}
           let points2 = 
               seq{for u in uLowerLimit..resolution..uUpperLimit do 
                     for v in vLowerLimit..resolution..vUpperLimit -> Point3D(x(u + resolution, v + resolution), y(u + resolution, v + resolution), z(u + resolution, v + resolution))}
           let points3 = 
               seq{for u in uLowerLimit..resolution..uUpperLimit do 
                     for v in vLowerLimit..resolution..vUpperLimit -> Point3D(x(u + resolution, v), y(u + resolution, v), z(u + resolution, v))}
           
           
           let points1 = Seq.zip3 points0 points1 points2
           let points2 = Seq.zip3 points2 points3 points0  
           
           let meshGeometry = MeshGeometry3D() 
           
           do  Seq.iter (fun n -> 
               let p1,p2,p3 = n 
               // Front
               meshGeometry.Positions.Add(p3)
               meshGeometry.Positions.Add(p2)
               meshGeometry.Positions.Add(p1)               
               // back
               meshGeometry.Positions.Add(p1)
               meshGeometry.Positions.Add(p2)
               meshGeometry.Positions.Add(p3) 
               ) points1

           do  Seq.iter (fun n -> 
               let p1,p2,p3 = n 
               // Front
               meshGeometry.Positions.Add(p3)
               meshGeometry.Positions.Add(p2)
               meshGeometry.Positions.Add(p1)               
               // Back
               meshGeometry.Positions.Add(p1)
               meshGeometry.Positions.Add(p2)
               meshGeometry.Positions.Add(p3)
               ) points2
               
           let model3D = GeometryModel3D(meshGeometry, material)

           model3D

