namespace GraphingCalculator

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
open Style
open ConventionalDomain
open RpnDomain
open GraphingDomain
open System.Windows.Input

type View =
    | PlotCanvas of Grid
    | Text of CalcTextBox
    | Function of Grid
    | Function2D of Grid
    | Function3D of Grid
    | Option of Grid
    | Option2D of Grid
    | Option3D of Grid

type Model =
    | Trace of Trace list
    | Model3D of Mesh

type ViewPoint = 
    | Pt of System.Windows.Point
    | Pt3D of System.Windows.Media.Media3D.Point3D

type InputMode = 
    | Conventional of Grid
    | RPN of Grid
    | Graph of Grid  
    | Graph2DParametric of Grid
    | Graph3DParametric of Grid
    
type Mode =    
    | Conventional
    | RPN
    | Graph
    | Graph2DParametric
    | Graph3DParametric

type State = 
    { rpn:RpnDomain.CalculatorState;
      conventional:ConventionalDomain.CalculatorState;
      graph:GraphingDomain.CalculatorState;
      graph2DParametric:GraphingDomain.CalculatorState;
      graph3DParametric:GraphingDomain.CalculatorState;
      mode : Mode;
      model : Model 
      scale : (float*float) 
      quaternion : Quaternion}

type GraphingCalculator() as graphingCalculator =
    inherit UserControl()
    
    // set initial state -- DEFAULTS --
    let conventionalDefault =  
        { pendingOp = None; 
          memory = "" }
        |> ConventionalDomain.CalculatorState.ZeroState 
    let rpnDefault = 
        {stack = RpnDomain.StackContents []}
        |> RpnDomain.CalculatorState.ReadyState 
    let modelDefault = 
        [{ startPoint = (Point(X (Math.Pure.Quantity.Real 10.),Y (Math.Pure.Quantity.Real 20.))); 
          traceSegments = 
            [ LineSegment (Point(X (Math.Pure.Quantity.Real 50.), Y (Math.Pure.Quantity.Real 30. ))); 
              LineSegment (Point(X (Math.Pure.Quantity.Real 100.),Y (Math.Pure.Quantity.Real 200.)));
              LineSegment (Point(X (Math.Pure.Quantity.Real 178.),Y (Math.Pure.Quantity.Real 66. ))) ]}] |> Trace
    let graphDefault = 
        { evaluatedExpression = 
            Math.Pure.Quantity.Number.Zero 
            |> Math.Pure.Structure.Number; 
          pendingFunction = None; 
          drawingOptions = GraphingImplementation.defaultOptions
        }   
        |> EvaluatedState
    let graph2DParametricDefault = 
        let zero = 
            Math.Pure.Quantity.Number.Zero 
            |> Math.Pure.Structure.Number
        ({ evaluatedExpression = zero; 
           pendingFunction = None; 
           drawingOptions = GraphingImplementation.defaultOptions},
           graphDefault ) |> EvaluatedState2DParametric
    let graph3DParametricDefault = 
        let zero = 
            Math.Pure.Quantity.Number.Zero 
            |> Math.Pure.Structure.Number
        ({ evaluatedExpression = zero; 
           pendingFunction = None; 
           drawingOptions = GraphingImplementation.defaultOptions},
           {x=zero;
            y=zero;
            z=zero;
            activeExpression=X_} ) |> EvaluatedState3DParametric
    let scaleDefault = (1., 1.)
    let quaternionDefault = Quaternion(0., 0., 1., 0.)
    let mutable state = 
        { rpn = rpnDefault; 
          conventional = conventionalDefault; 
          mode = Conventional; 
          model = modelDefault; 
          graph = graphDefault;
          graph2DParametric = graph2DParametricDefault;
          graph3DParametric = graph3DParametricDefault;
          scale = scaleDefault;
          quaternion = quaternionDefault}

// ------Create Views---------        
    //-----Calculator--------//
    let calculator_Grid = Grid()    
    let calculator_Rectangle = 
        Rectangle(
            StrokeThickness = 2., 
            Stroke = black, 
            Fill = radialGradientBrush, 
            RadiusX = 10., 
            RadiusY = 10.
            )
    let calculator_ModelNumber_TextBlock = 
        TextBlock(
            Margin = Thickness(left=0.,top=1.5,right=15.,bottom=0.),
            HorizontalAlignment = HorizontalAlignment.Right,
            FontWeight = FontWeights.Bold,
            Text = "FL-1"
            )    
    let calculator_Layout_Grid = 
        let grid = Grid(Margin = Thickness(left=10.,top=10.,right=10.,bottom=10.),
                        Background = transparentBlack) 
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition(Width = GridLength(325.))
        let column3 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(3., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength.Auto)//GridLength(1., GridUnitType.Star))//
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
        grid
    
    do  // Assemble the pieces
        calculator_Grid.Children.Add(calculator_Rectangle)             |> ignore
        calculator_Grid.Children.Add(calculator_ModelNumber_TextBlock) |> ignore
    
    //-----Screen
    let screen_Border = 
        let gb = Border(
                    Margin = Thickness(left = 5., top = 5., right = 5., bottom = 5.),
                    BorderBrush = black,
                    BorderThickness = Thickness(1.5)
                    )        
        do gb.SetValue(Grid.ColumnSpanProperty,3)
        do gb.SetValue(Grid.RowSpanProperty,2)
        gb
    let screen_Grid = 
        let g = 
            Grid(
                Background = screenColor,
                ClipToBounds = true
                )
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
        g
    let screen_Text_TextBox = 
        let t = CalcTextBox(                    
                    Visibility = Visibility.Visible,
                    IsReadOnly = true,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                    )
        do t.SetValue(Grid.RowProperty, 1)
        t
    let canvasGridLines_CheckBox = 
        let c = CheckBox(Content = "Show Grid Lines")
        do  c.SetValue(Grid.RowProperty, 0)
        c
    let canvasGridLine_Slider =
        let s = 
            Slider(
                Margin = Thickness(left = 0., top = 20., right = 0., bottom = 0.),
                Minimum = 5.,
                Maximum = 100.,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                TickFrequency = 5.,
                IsSnapToTickEnabled = true,
                IsEnabled = false)        
        do s.SetValue(Grid.RowProperty, 0)
        s
    let canvas_DockPanel_Grid =
        let g = Grid()
        do 
            g.SetValue(DockPanel.DockProperty,Dock.Top)
            g.Children.Add(canvasGridLines_CheckBox) |> ignore
            g.Children.Add(canvasGridLine_Slider) |> ignore
        g    
    let canvas = Canvas(ClipToBounds = true)
    let canvas_DockPanel =
        let d = DockPanel()
        do d.Children.Add(canvas_DockPanel_Grid) |> ignore
        do d.Children.Add(canvas) |> ignore
        do d.Visibility <- Visibility.Collapsed
        d
    let screen_Canvas =
        let g = Grid()
        do g.SetValue(Grid.RowProperty, 1)        
        do g.Children.Add(canvas_DockPanel) |> ignore
        
        g
    let selection_Rectangle =
        let r =
            Rectangle(
                Stroke = black,
                Fill = transparentBlack,                
                Visibility = Visibility.Collapsed,
                StrokeDashOffset = 5.,
                StrokeThickness = 0.99,
                HorizontalAlignment=HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                StrokeDashArray = DoubleCollection[3.; 3.]
                )
        do r.SetValue(Grid.RowProperty, 1)
        r

    do  // Assemble the pieces   
        screen_Grid.Children.Add(screen_Text_TextBox) |> ignore
        screen_Grid.Children.Add(screen_Canvas)       |> ignore
        screen_Grid.Children.Add(selection_Rectangle) |> ignore

    //-----Function  
    let function_Grid =     
        let grid = Grid(Visibility = Visibility.Hidden)
        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)

        grid
    let function_yLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "y = ")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let function_y_TextBox = 
        let tb = FunctionTextBox(MaxLines = 25, TabIndex = 0, IsReadOnly = true)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function_Button = 
            let b = FunctionButton(Content="Graph it!", TabIndex = 2)
            do b.SetValue(Grid.ColumnProperty,1)
            b
    let function_Button_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,1)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function_Button) |> ignore
        grid
    
    do  // Assemble the pieces
        function_Grid .Children.Add(function_yLabel_TextBlock) |> ignore
        function_Grid .Children.Add(function_y_TextBox) |> ignore      
        function_Grid .Children.Add(function_Button_Grid) |> ignore
    
    //-----Function 2D Parametric
    let function2D_Grid =     
        let grid = Grid(Visibility = Visibility.Collapsed)
        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)

        grid
    let function2D_xtLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "xt = ")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let function2D_xt_TextBox = 
        let tb = FunctionTextBox(MaxLines = 3, TabIndex = 0, IsReadOnly = true)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function2D_ytLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "yt = ")
        do tb.SetValue(Grid.RowProperty,1)
        tb
    let function2D_yt_TextBox = 
        let tb = FunctionTextBox(MaxLines = 3, TabIndex = 1, IsReadOnly = true)
        do  tb.SetValue(Grid.RowProperty,1)
            tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function2D_Spiral_Button = 
        let b = FunctionButton(Content="Spiral", TabIndex = 2)
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let function2D_Ellipse_Button = 
        let b = FunctionButton(Content="Ellipse", TabIndex = 2)
        do b.SetValue(Grid.ColumnProperty,2)
        b
    let function2D_ShapeButton_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,2)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function2D_Ellipse_Button) |> ignore
            grid.Children.Add(function2D_Spiral_Button) |> ignore
        grid
    let function2D_Graph_Button = 
        let b = FunctionButton(Content="Graph it!", TabIndex = 2)
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let function2D_GraphButton_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,3)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function2D_Graph_Button) |> ignore            
        grid
    
    do  // Assemble the pieces
        function2D_Grid .Children.Add(function2D_xtLabel_TextBlock) |> ignore
        function2D_Grid .Children.Add(function2D_xt_TextBox) |> ignore
        function2D_Grid .Children.Add(function2D_ytLabel_TextBlock) |> ignore
        function2D_Grid .Children.Add(function2D_yt_TextBox) |> ignore
        function2D_Grid .Children.Add(function2D_ShapeButton_Grid) |> ignore
        function2D_Grid .Children.Add(function2D_GraphButton_Grid) |> ignore

    //-----Function 3D Parametric
    let function3D_Grid =     
        let grid = Grid(Visibility = Visibility.Collapsed)
        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength.Auto)
        let row6 = RowDefinition(Height = GridLength.Auto)
        let row7 = RowDefinition(Height = GridLength.Auto)
        let row8 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
            grid.RowDefinitions.Add(row6)
            grid.RowDefinitions.Add(row7)
            grid.RowDefinitions.Add(row8)

        grid
    let function3D_fxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "fx(u,v) = ")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let function3D_fx_TextBox = 
        let tb = FunctionTextBox(MaxLines = 15, TabIndex = 0, IsReadOnly = true, BorderThickness = Thickness(3.))
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function3D_fyLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "fy(u,v) = ")
        do tb.SetValue(Grid.RowProperty,1)
        tb
    let function3D_fy_TextBox = 
        let tb = FunctionTextBox(MaxLines = 3, TabIndex = 1, IsReadOnly = true)
        do  tb.SetValue(Grid.RowProperty,1)
            tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let function3D_fzLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "fz(u,v) = ")
        do tb.SetValue(Grid.RowProperty,2)
        tb
    let function3D_fz_TextBox = 
        let tb = FunctionTextBox(MaxLines = 3, TabIndex = 2, IsReadOnly = true)
        do  tb.SetValue(Grid.RowProperty,2)
            tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let function3D_Sphere_Button = 
        let b = FunctionButton(Content="Sphere", TabIndex = 3)
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let function3D_Cone_Button = 
        let b = FunctionButton(Content="Cone", TabIndex = 3)
        do b.SetValue(Grid.ColumnProperty,2)
        b
    let function3D_Torus_Button = 
        let b = FunctionButton(Content="Torus", TabIndex = 3)
        do b.SetValue(Grid.ColumnProperty,3)
        b
    let function3D_Helix_Button = 
        let b = FunctionButton(Content="Helix", TabIndex = 4)
        do b.SetValue(Grid.ColumnProperty,5)
        b
    let function3D_ShapeButton_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        let column4 = ColumnDefinition(Width = GridLength.Auto)
        let column5 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,4)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function3D_Sphere_Button) |> ignore
            grid.Children.Add(function3D_Cone_Button) |> ignore
            grid.Children.Add(function3D_Torus_Button) |> ignore
            grid.Children.Add(function3D_Helix_Button) |> ignore
        grid    
    let function3D_SolidMesh_Button = 
        let b = FunctionButton(Content="Solid Mesh", TabIndex = 3)
        do b.SetValue(Grid.ColumnProperty,5)
        b
    let function3D_SolidMeshButton_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,3)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function3D_SolidMesh_Button) |> ignore            
        grid
    let tRadio = 
        RadioButton(
            FontSize = 15.,
            Margin = Thickness(left = 5., top = 5., right = 5., bottom = 0.),
            GroupName = "Select Parameter", 
            Content = "t")        
    let uvRadio = 
        RadioButton(
            FontSize = 15.,
            Margin = Thickness(left = 5., top = 5., right = 5., bottom = 0.),
            GroupName = "Select Parameter", 
            Content = "u,v",
            IsChecked = Nullable true )
    let function3D_SelectParameter_RadioButtons = 
        let radioGroup = 
            StackPanel(
                Margin = Thickness(left = 5., top = 5., right = 5., bottom = 0.),
                Orientation = Orientation.Horizontal)
        let selectParameter_TextBox = 
            let tb = FunctionTextBlock(Text = "SELECT PARAMETER")
            tb            
        do  radioGroup.Children.Add(selectParameter_TextBox) |> ignore
            radioGroup.Children.Add(tRadio) |> ignore
            radioGroup.Children.Add(uvRadio) |> ignore
            radioGroup.SetValue(Grid.RowProperty,3)
            radioGroup.SetValue(Grid.ColumnSpanProperty,2)
        radioGroup
    (**)
    do  // Assemble the pieces
        function3D_Grid .Children.Add(function3D_fxLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fx_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_fyLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fy_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_fzLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fz_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_ShapeButton_Grid) |> ignore
        function3D_Grid .Children.Add(function3D_SolidMeshButton_Grid) |> ignore
        function3D_Grid .Children.Add(function3D_SelectParameter_RadioButtons) |> ignore
    
    //-----3D Viewport
    let viewport3D = 
        let viewport = Viewport3D()
        do  viewport.SetValue(Grid.ColumnProperty,2)
            viewport.SetValue(Grid.RowProperty,2)  
            viewport.Tag <- System.Windows.Point(0., 0.)
        viewport
    let model3DGroup = Model3DGroup()
    let modelVisual3D = ModelVisual3D()
    let rotateTransform3D = RotateTransform3D()
    
    let light = DirectionalLight(Color = Colors.White, Direction = Vector3D(-1., -0.5, -1.))
    let light2 = DirectionalLight(Color = Colors.White, Direction = Vector3D(1., 0.5, 1.))
    let perspective_Camera = 
        // Defines the camera used to view the 3D object. In order to view the 3D object,
        // the camera must be positioned and pointed such that the object is within view 
        // of the camera.
        let camera = PerspectiveCamera()
                // Specify where in the 3D scene the camera is.
        do  camera.Position <- new Point3D(0., 0., 1.)
                // Specify the direction that the camera is pointing.
            camera.LookDirection <- new Vector3D(0., 0., -0.5)
                // Define camera's horizontal field of view in degrees.
            camera.FieldOfView <- 80.
                //camera.FarPlaneDistance <- 20.
        camera
    
    do // Assemble the pieces            
       //model3DGroup.Children.Add(Models.surface)            

       modelVisual3D.Content <- model3DGroup
       
       viewport3D.Camera <- perspective_Camera
       viewport3D.Children.Add(modelVisual3D)
       viewport3D.ClipToBounds <- false
       viewport3D.VerticalAlignment <- VerticalAlignment.Center
       viewport3D.HorizontalAlignment <- HorizontalAlignment.Center
       viewport3D.Height <- System.Windows.SystemParameters.WorkArea.Height//400.//
       viewport3D .Width <- System.Windows.SystemParameters.WorkArea.Height//400.//
       
       screen_Canvas.Children.Add(viewport3D) |> ignore  
       
    //-----Function Options
    let option_Grid =     
        let grid = Grid(Visibility = Visibility.Collapsed)
        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)        
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength.Auto)
        let row6 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
            grid.RowDefinitions.Add(row6)

        grid    
    let option_xMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "x Min = ")
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option_xMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option_xMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "x Max = ")
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option_xMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option_yMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "y Min = ")
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option_yMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option_yMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "y Max = ")
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option_yMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let option_Save_Button = FunctionButton(Content="Save")
    let option_Reset_Button = 
        let b = FunctionButton(Content="Reset")
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let option_Button_Grid =
        let grid = Grid() 
        do  grid.SetValue(Grid.RowProperty,4)
        
        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)            
            
        do  grid.Children.Add(option_Save_Button) |> ignore
            grid.Children.Add(option_Reset_Button) |> ignore
        grid
    
    do  // Assemble the pieces
        option_Grid .Children.Add(option_xMinLabel_TextBlock) |> ignore
        option_Grid .Children.Add(option_xMin_TextBox) |> ignore
        option_Grid .Children.Add(option_xMaxLabel_TextBlock) |> ignore
        option_Grid .Children.Add(option_xMax_TextBox) |> ignore
        option_Grid .Children.Add(option_yMinLabel_TextBlock) |> ignore
        option_Grid .Children.Add(option_yMin_TextBox) |> ignore
        option_Grid .Children.Add(option_yMaxLabel_TextBlock) |> ignore
        option_Grid .Children.Add(option_yMax_TextBox) |> ignore
        option_Grid .Children.Add(option_Button_Grid) |> ignore

    //-----Function 2D Options
    let option2D_Grid =     
        let grid = Grid(Visibility = Visibility.Collapsed)
        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)        
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength.Auto)
        let row6 = RowDefinition(Height = GridLength.Auto)        
        let row7 = RowDefinition(Height = GridLength.Auto)
        let row8 = RowDefinition(Height = GridLength.Auto)
        let row9 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
            grid.RowDefinitions.Add(row6)
            grid.RowDefinitions.Add(row7)
            grid.RowDefinitions.Add(row8)
            grid.RowDefinitions.Add(row9)

        grid
    let option2D_xMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "x Min = ")
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_xMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option2D_xMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "x Max = ")
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_xMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option2D_yMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "y Min = ")
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_yMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option2D_yMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "y Max = ")
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_yMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let option2D_tMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Min = ")
        do tb.SetValue(Grid.RowProperty,4)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_tMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,4)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option2D_tMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Max = ")
        do tb.SetValue(Grid.RowProperty,5)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_tMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,5)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb 
    let option2D_tStepLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Step = ")
        do tb.SetValue(Grid.RowProperty,6)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option2D_tStep_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,6)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb 
    let option2D_Save_Button = FunctionButton(Content="Save")
    let option2D_Reset_Button = 
        let b = FunctionButton(Content="Reset")
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let option2D_Button_Grid =
        let grid = Grid() 
        do  grid.SetValue(Grid.RowProperty,7)

        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
        do  grid.Children.Add(option2D_Save_Button) |> ignore
            grid.Children.Add(option2D_Reset_Button) |> ignore
        grid
    
    do  // Assemble the pieces
        option2D_Grid .Children.Add(option2D_xMinLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_xMin_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_xMaxLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_xMax_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_yMinLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_yMin_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_yMaxLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_yMax_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_tMinLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_tMin_TextBox) |> ignore        
        option2D_Grid .Children.Add(option2D_tMaxLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_tMax_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_tStepLabel_TextBlock) |> ignore
        option2D_Grid .Children.Add(option2D_tStep_TextBox) |> ignore
        option2D_Grid .Children.Add(option2D_Button_Grid) |> ignore

    //-----Function 3D Options
    let option3D_Grid =     
        let grid = Grid(Visibility = Visibility.Collapsed)        
        do grid.SetValue(Grid.RowProperty, 1)
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            
        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)        
        let row4 = RowDefinition(Height = GridLength.Auto)
        let row5 = RowDefinition(Height = GridLength.Auto)
        let row6 = RowDefinition(Height = GridLength.Auto)        
        let row7 = RowDefinition(Height = GridLength.Auto)
        let row8 = RowDefinition(Height = GridLength.Auto)
        let row9 = RowDefinition(Height = GridLength.Auto)
        let row10 = RowDefinition(Height = GridLength.Auto)
        let row11 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
            
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
            grid.RowDefinitions.Add(row6)
            grid.RowDefinitions.Add(row7)
            grid.RowDefinitions.Add(row8)
            grid.RowDefinitions.Add(row9)
            grid.RowDefinitions.Add(row10)
            grid.RowDefinitions.Add(row11)            
        grid
    let option3D_uMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "u Min = ")
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_uMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,0)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option3D_uMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "u Max = ")
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_uMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,1)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option3D_uStepLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "u Step = ")
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_uStep_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option3D_vMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "v Min = ")
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_vMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,3)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let option3D_vMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "v Max = ")
        do tb.SetValue(Grid.RowProperty,4)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_vMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,4)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option3D_vStepLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "v Step = ")
        do tb.SetValue(Grid.RowProperty,5)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_vStep_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,5)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb 
    let option3D_Save_Button = FunctionButton(Content="Save")
    let option3D_Reset_Button = 
        let b = FunctionButton(Content="Reset")
        do b.SetValue(Grid.ColumnProperty,1)
        b
    let option3D_Button_Grid =
        let grid = Grid() 
        do  grid.SetValue(Grid.RowProperty,11)

        let column1 = ColumnDefinition(Width = GridLength.Auto)
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength.Auto)
        let row2 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
        do  grid.Children.Add(option3D_Save_Button) |> ignore
            grid.Children.Add(option3D_Reset_Button) |> ignore
        grid
    let option3D_tMinLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Min = ")
        do tb.SetValue(Grid.RowProperty,7)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_tMin_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,7)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let option3D_tMaxLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Max = ")
        do tb.SetValue(Grid.RowProperty,8)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_tMax_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,8)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb 
    let option3D_tStepLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "t Step = ")
        do tb.SetValue(Grid.RowProperty,9)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_tStep_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1)
        do tb.SetValue(Grid.RowProperty,9)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb 

    do  // Assemble the pieces
        option3D_Grid .Children.Add(option3D_uMinLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uMin_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_uMaxLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uMax_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_uStepLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uStep_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_vMinLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vMin_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_vMaxLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vMax_TextBox) |> ignore        
        option3D_Grid .Children.Add(option3D_vStepLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vStep_TextBox) |> ignore        
        option3D_Grid .Children.Add(option3D_tMinLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_tMin_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_tMaxLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_tMax_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_tStepLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_tStep_TextBox) |> ignore                
        option3D_Grid .Children.Add(option3D_Button_Grid) |> ignore

    //-----Immediate Text Box
    let immediate =
        let im = 
            TextBox(            
                Margin = Thickness(left = 5., top = 5., right = 5., bottom = 0.),
                MaxLines = 7,
                Background = screenColor
                )
        do  im.SetValue(Grid.ColumnSpanProperty,3)
            im.SetValue(Grid.RowProperty,2)
            im.SetValue(TextBlock.TextAlignmentProperty,TextAlignment.Left)
        im

    //-----Memo Text Block
    let memo =
        let im = 
            TextBlock()
        do  im.SetValue(Grid.ColumnSpanProperty,2)
            im.SetValue(TextBlock.TextAlignmentProperty,TextAlignment.Center)
            im.SetValue(TextBlock.VerticalAlignmentProperty,VerticalAlignment.Center)
        im

 // ------Create Buttons ------
    //-----Function Buttons
    let funcButton_Grid =
        let grid = Grid()
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        let column4 = ColumnDefinition()
        let column5 = ColumnDefinition()
        let column6 = ColumnDefinition()
        let column7 = ColumnDefinition()

        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)
            grid.ColumnDefinitions.Add(column6)
            grid.ColumnDefinitions.Add(column7)
            
        let row1 = RowDefinition()
        let row2 = RowDefinition()
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.SetValue(Grid.RowProperty,3)
            grid.SetValue(Grid.ColumnProperty,1)

        grid
    let sin_Button = 
        FuncButton(Content = "sin")
    let cos_Button = 
        FuncButton(Content = "cos")
    let tan_Button = 
        FuncButton(Content = "tan")
    let xSquared_Button = 
        FuncButton(Content = "x^2")
    let xPowY_Button = 
        FuncButton(Content = "x^y")
    let pi_Button = 
        FuncButton(Content = "pi")
    let e_Button = 
        FuncButton(Content = "e")
    let x_Button = 
        FuncButton(
            Content = "x",
            Margin = Thickness(left = 5., top = 5., right = 0., bottom = 5.),
            Height = Double.NaN
            )
    let t_Button = 
        FuncButton(
            Content = "t",
            Margin = Thickness(left = 5., top = 5., right = 0., bottom = 5.),
            Height = Double.NaN
            )
    let u_Button = 
        FuncButton(
            Content = "u",
            Margin = Thickness(left = 5., top = 5., right = 0., bottom = 5.),
            Height = Double.NaN
            )
    let v_Button = 
        FuncButton(
            Content = "v",
            Margin = Thickness(left = 5., top = 5., right = 0., bottom = 5.),
            Height = Double.NaN
            )
    let dx_Button = 
        FuncButton(
            Content = "dX",
            Margin = Thickness(left = 5., top = 5., right = 0., bottom = 5.),
            Height = Double.NaN
            )
    let funcButtons = 
        [
           sin_Button; 
           cos_Button; 
           tan_Button; 
           xSquared_Button; 
           xPowY_Button
           pi_Button;
           e_Button;
           x_Button;
           t_Button;
           u_Button;
           v_Button;
           dx_Button
        ]

    do  // Place buttons in a grid
        List.iter ( fun x -> funcButton_Grid.Children.Add(x) |> ignore) funcButtons //
        
    do  // Place the memo text block in the button grid
        funcButton_Grid.Children.Add(memo) |> ignore

    do  // Arrange the buttons on the grid.
        sin_Button      .SetValue(Grid.RowProperty,0); sin_Button      .SetValue(Grid.ColumnProperty,0);
        cos_Button      .SetValue(Grid.RowProperty,0); cos_Button      .SetValue(Grid.ColumnProperty,1);
        tan_Button      .SetValue(Grid.RowProperty,0); tan_Button      .SetValue(Grid.ColumnProperty,2);
        xSquared_Button .SetValue(Grid.RowProperty,0); xSquared_Button .SetValue(Grid.ColumnProperty,3);
        xPowY_Button    .SetValue(Grid.RowProperty,0); xPowY_Button    .SetValue(Grid.ColumnProperty,4);
        pi_Button       .SetValue(Grid.RowProperty,0); pi_Button       .SetValue(Grid.ColumnProperty,5);
        e_Button        .SetValue(Grid.RowProperty,0); e_Button        .SetValue(Grid.ColumnProperty,6);
        x_Button        .SetValue(Grid.RowProperty,1); x_Button        .SetValue(Grid.ColumnProperty,0);
        t_Button        .SetValue(Grid.RowProperty,1); t_Button        .SetValue(Grid.ColumnProperty,1);
        u_Button        .SetValue(Grid.RowProperty,1); u_Button        .SetValue(Grid.ColumnProperty,2);
        v_Button        .SetValue(Grid.RowProperty,1); v_Button        .SetValue(Grid.ColumnProperty,3);
        dx_Button       .SetValue(Grid.RowProperty,1); dx_Button       .SetValue(Grid.ColumnProperty,4);
        memo            .SetValue(Grid.RowProperty,1); memo            .SetValue(Grid.ColumnProperty,5);
       
    //-----Calc Buttons     
    let calcButton_Grid =
        let grid = Grid()
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        let column4 = ColumnDefinition()
        let column5 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)
                        
        let row1 = RowDefinition()
        let row2 = RowDefinition()
        let row3 = RowDefinition()
        let row4 = RowDefinition()
        let row5 = RowDefinition()
        let row6 = RowDefinition()
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            grid.RowDefinitions.Add(row3)
            grid.RowDefinitions.Add(row4)
            grid.RowDefinitions.Add(row5)
            grid.RowDefinitions.Add(row6)
            
            grid.SetValue(Grid.RowProperty,5)
            grid.SetValue(Grid.ColumnProperty,1)

        grid
    let memoryButton_Grid =
        let grid = Grid()
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        let column4 = ColumnDefinition()
        let column5 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)
                        
        let row1 = RowDefinition()
                
        do  grid.RowDefinitions.Add(row1)
                        
            grid.SetValue(Grid.RowProperty,0)
            grid.SetValue(Grid.ColumnProperty,0)
            grid.SetValue(Grid.ColumnSpanProperty,5)

        grid
    let rpnButton_Grid =
        let grid = Grid()
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        let column4 = ColumnDefinition()
        let column5 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)
                        
        let row1 = RowDefinition()
                
        do  grid.RowDefinitions.Add(row1)
                        
            grid.SetValue(Grid.RowProperty,0)
            grid.SetValue(Grid.ColumnProperty,0)
            grid.SetValue(Grid.ColumnSpanProperty,5)
        grid    
    let graphButton_Grid =
        let grid = Grid()
        
        let column1 = ColumnDefinition()
        let column2 = ColumnDefinition()
        let column3 = ColumnDefinition()
        let column4 = ColumnDefinition()
        let column5 = ColumnDefinition()
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)
            grid.ColumnDefinitions.Add(column5)
                        
        let row1 = RowDefinition()
                
        do  grid.RowDefinitions.Add(row1)
                        
            grid.SetValue(Grid.RowProperty,0)
            grid.SetValue(Grid.ColumnProperty,0)
            grid.SetValue(Grid.ColumnSpanProperty,5)
        grid    
    
    let one =           CalcButton(Name = "oneButton", Content = "1")
    let two =           CalcButton(Name = "twoButton", Content = "2")
    let three =         CalcButton(Name = "threeButton", Content = "3")
    let four =          CalcButton(Name = "fourButton", Content = "4")
    let five =          CalcButton(Name = "fiveButton", Content = "5")
    let six =           CalcButton(Name = "sixButton", Content = "6")
    let seven =         CalcButton(Name = "sevenButton", Content = "7")
    let eight =         CalcButton(Name = "eightButton", Content = "8")
    let nine =          CalcButton(Name = "nineButton", Content = "9")
    let zero =          CalcButton(Name = "zeroButton", Content = "0")
    let decimalPoint =  CalcButton(Name = "decimalPointButton", Content = ".")
    let add =           CalcButton(Name = "addButton", Content = "+")
    let subtract =      CalcButton(Name = "subtractButton", Content = "-")
    let multiply =      CalcButton(Name = "multiplyButton", Content = "*")
    let divide =        CalcButton(Name = "divideButton", Content = "/")
    let equals =        CalcButton(Name = "equalsButton", Content = "=")
    let root =          CalcButton(Name = "rootButton", Content = "\u221A")
    let changeSign =    CalcButton(Name = "signButton", Content = "\u00B1")
    let inverse =       CalcButton(Name = "inverseButton", Content = "1/x")
    let percent =       CalcButton(Name = "percentButton", Content = "%")
    let back =          CalcButton(Name = "backButton", Content = "\u2b05")
    let clear =         CalcButton(Name = "clearButton", Content = "C")
    let clearEntry =    CalcButton(Name = "clearEntryButton", Content = "CE")
    let clearMemory =   CalcButton(Name = "clearMemoryButton",Content = "MC")
    let recallMemory =  CalcButton(Name = "recallMemoryButton", Content = "MR")
    let storeMemory =   CalcButton(Name = "storeMemoryButton", Content = "MS")
    let addToMemory =   CalcButton(Name = "addToMemoryButton", Content = "M+")
    let subtractFromMemoy = CalcButton(Name = "subtractFromButton", Content = "M-")
    let openParentheses =   CalcButton(Name = "openParentheses", Content = "(")
    let closeParentheses =  CalcButton(Name = "closeParentheses", Content = ")")
    let calcButtons = [one; two; three; four; five; six; seven; eight; nine; zero; 
        decimalPoint; add; subtract; multiply; divide; openParentheses; clearEntry;
        equals; root; changeSign; inverse; percent; back; clear; closeParentheses]
    let memoryButtons = [storeMemory; clearMemory; recallMemory; addToMemory; subtractFromMemoy]
    

    //-----RPN Buttons
    let drop =          CalcButton(Name = "dropButton", Content = "Drop")
    let duplicate =     CalcButton(Name = "duplicateButton", Content = "Dup")
    let swap =          CalcButton(Name = "swapButton", Content = "Swap")
    let clearStack =    CalcButton(Name = "clearStackButton", FontSize = 10.,Content = TextBlock(Text = "Clear Stack", TextWrapping = TextWrapping.Wrap))
    let enter =         CalcButton(Name = "enter", Content = "Enter")
    let rpnButtons = [drop; duplicate; swap; clearStack; enter]    

    //-----Graph Buttons
    let blank = CalcButton(Name = "blank",Visibility = Visibility.Hidden)
    let graphButtons = [blank]

    do // Place buttons in a grid
        List.iter ( fun x -> calcButton_Grid.Children.Add(x) |> ignore) calcButtons         
        List.iter ( fun x -> rpnButton_Grid.Children.Add(x) |> ignore) rpnButtons
        List.iter ( fun x -> memoryButton_Grid.Children.Add(x) |> ignore) memoryButtons
        List.iter ( fun x -> graphButton_Grid.Children.Add(x) |> ignore) graphButtons
        calcButton_Grid.Children.Add(graphButton_Grid) |> ignore
        calcButton_Grid.Children.Add(rpnButton_Grid) |> ignore
        calcButton_Grid.Children.Add(memoryButton_Grid) |> ignore        
        
    do  // Arrange the buttons on the grid.
        one                 .SetValue(Grid.RowProperty,4); one                  .SetValue(Grid.ColumnProperty,0);
        two                 .SetValue(Grid.RowProperty,4); two                  .SetValue(Grid.ColumnProperty,1);
        three               .SetValue(Grid.RowProperty,4); three                .SetValue(Grid.ColumnProperty,2);
        four                .SetValue(Grid.RowProperty,3); four                 .SetValue(Grid.ColumnProperty,0);
        five                .SetValue(Grid.RowProperty,3); five                 .SetValue(Grid.ColumnProperty,1);
        six                 .SetValue(Grid.RowProperty,3); six                  .SetValue(Grid.ColumnProperty,2);
        seven               .SetValue(Grid.RowProperty,2); seven                .SetValue(Grid.ColumnProperty,0);
        eight               .SetValue(Grid.RowProperty,2); eight                .SetValue(Grid.ColumnProperty,1);
        nine                .SetValue(Grid.RowProperty,2); nine                 .SetValue(Grid.ColumnProperty,2);
        zero                .SetValue(Grid.RowProperty,5); zero                 .SetValue(Grid.ColumnProperty,0);
        decimalPoint        .SetValue(Grid.RowProperty,5); decimalPoint         .SetValue(Grid.ColumnProperty,1);
        add                 .SetValue(Grid.RowProperty,5); add                  .SetValue(Grid.ColumnProperty,3);
        subtract            .SetValue(Grid.RowProperty,4); subtract             .SetValue(Grid.ColumnProperty,3);
        multiply            .SetValue(Grid.RowProperty,3); multiply             .SetValue(Grid.ColumnProperty,3);
        divide              .SetValue(Grid.RowProperty,2); divide               .SetValue(Grid.ColumnProperty,3);
        equals              .SetValue(Grid.RowProperty,5); equals               .SetValue(Grid.ColumnProperty,2);
        root                .SetValue(Grid.RowProperty,2); root                 .SetValue(Grid.ColumnProperty,4);
        changeSign          .SetValue(Grid.RowProperty,1); changeSign           .SetValue(Grid.ColumnProperty,3);
        inverse             .SetValue(Grid.RowProperty,1); inverse              .SetValue(Grid.ColumnProperty,4);
        percent             .SetValue(Grid.RowProperty,3); percent              .SetValue(Grid.ColumnProperty,4);
        back                .SetValue(Grid.RowProperty,1); back                 .SetValue(Grid.ColumnProperty,2);
        clear               .SetValue(Grid.RowProperty,1); clear                .SetValue(Grid.ColumnProperty,1);        
        clearEntry          .SetValue(Grid.RowProperty,1); clearEntry           .SetValue(Grid.ColumnProperty,0);
        //-----Parentheses Buttons
        openParentheses     .SetValue(Grid.RowProperty,4); openParentheses      .SetValue(Grid.ColumnProperty,4);
        closeParentheses    .SetValue(Grid.RowProperty,5); closeParentheses     .SetValue(Grid.ColumnProperty,4);
        //-----Memory Buttons
        clearMemory         .SetValue(Grid.RowProperty,0); clearMemory          .SetValue(Grid.ColumnProperty,2);
        recallMemory        .SetValue(Grid.RowProperty,0); recallMemory         .SetValue(Grid.ColumnProperty,1);
        storeMemory         .SetValue(Grid.RowProperty,0); storeMemory          .SetValue(Grid.ColumnProperty,0);                
        addToMemory         .SetValue(Grid.RowProperty,0); addToMemory          .SetValue(Grid.ColumnProperty,3);
        subtractFromMemoy   .SetValue(Grid.RowProperty,0); subtractFromMemoy    .SetValue(Grid.ColumnProperty,4);        
        //-----RPN Buttons
        drop                .SetValue(Grid.RowProperty,0); drop                 .SetValue(Grid.ColumnProperty,3);
        duplicate           .SetValue(Grid.RowProperty,0); duplicate            .SetValue(Grid.ColumnProperty,1);
        swap                .SetValue(Grid.RowProperty,0); swap                 .SetValue(Grid.ColumnProperty,2);
        clearStack          .SetValue(Grid.RowProperty,0); clearStack           .SetValue(Grid.ColumnProperty,0);
        enter               .SetValue(Grid.RowProperty,0); enter                .SetValue(Grid.ColumnProperty,4);

        //-----Graph Buttons
        blank               .SetValue(Grid.RowProperty,0); blank                 .SetValue(Grid.ColumnProperty,0);

    //----- Gridlines
    let xInterval = 5
    let yInterval = 5
    let makeGridLines yOffset xOffset = 
        
        let lines = Image()        
        do  lines.SetValue(Panel.ZIndexProperty, -100)
    
        let gridLinesVisual = DrawingVisual() 
        let context = gridLinesVisual.RenderOpen()
        let color1 = SolidColorBrush(Colors.Blue)
        let color2 = SolidColorBrush(Colors.Green)
        let pen1, pen2 = Pen(color1, 0.5), Pen(color2, 0.2)
        do  pen1.Freeze()
            pen2.Freeze()
    
        let rows = (int)(SystemParameters.PrimaryScreenHeight)
        let columns = (int)(SystemParameters.PrimaryScreenWidth)

        let x = System.Windows.Point(0., 0.)
        let x' = System.Windows.Point(SystemParameters.PrimaryScreenWidth, 0.)
        let y = System.Windows.Point(0.,0.)        
        let y' = System.Windows.Point(0., SystemParameters.PrimaryScreenHeight)
         
        //lines
        let horozontalLines = 
            seq{for i in 0..rows -> 
                    match i % yInterval = 0 with //Interval
                    | true -> context.DrawLine(pen1,x,x')
                              x.Offset(0.,yOffset)
                              x'.Offset(0.,yOffset)
                    | false -> context.DrawLine(pen2,x,x')
                               x.Offset(0.,yOffset)
                               x'.Offset(0.,yOffset)}
        let verticalLines = 
            seq{for i in 0..columns -> 
                    match i % xInterval = 0 with //Interval
                    | true -> context.DrawLine(pen1,y,y')
                              y.Offset(xOffset,0.)
                              y'.Offset(xOffset,0.)
                    | false -> context.DrawLine(pen2,y,y')
                               y.Offset(xOffset,0.)
                               y'.Offset(xOffset,0.)}        
        do  
            Seq.iter (fun x -> x) horozontalLines
            Seq.iter (fun y -> y) verticalLines
            context.Close()

        let bitmap = 
            RenderTargetBitmap(
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight, 
                96.,
                96.,
                PixelFormats.Pbgra32)        
        do  
            bitmap.Render(gridLinesVisual)
            bitmap.Freeze()
            lines.Source <- bitmap
        lines
    let removeGridLines () =         
        do canvas.Children.RemoveAt(0)                                
    //----- Orgin Point
    let resolution (r) = (canvasGridLine_Slider.Value * float(r))         
    let mapXToCanvas x =         
        let w = floor(screen_Grid.ActualWidth / 2.)
        let bias = w % resolution xInterval
        let w' = w - bias 
        (x + w')
    let mapYToCanvas y =         
        let h = ceil(screen_Grid.ActualHeight / 2.)
        let bias = h % resolution yInterval
        let h' = h - bias
        -(y - h')
    let placeOrginPoint x y = 
        let orgin = Image()        
        do  orgin.SetValue(Panel.ZIndexProperty, 100)        
    
        let orginVisual = DrawingVisual() 
        let context = orginVisual.RenderOpen()
        let color = SolidColorBrush(Colors.Blue)        
        let color' = SolidColorBrush(Colors.Red)
        let pen = Pen(color', 7.)
        do  pen.Freeze()                
    
        let orginPoint = System.Windows.Point(x,y)
        do  context.DrawEllipse(color,pen,orginPoint,1.,1.)
            context.Close()
        let bitmap = 
            RenderTargetBitmap(
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight, 
                96., 
                96., 
                PixelFormats.Pbgra32)
        do  
            bitmap.Render(orginVisual)
            bitmap.Freeze()
            orgin.Source <- bitmap
        orgin
    //----- Apply Scale
    let applyScaleX = 
        fun x ->
            let (xScale,_yScale) = state.scale
            x * xScale 
    let applyScaleY = 
        fun y ->
            let (_xScale,yScale) = state.scale
            y * yScale 
    
// ------Create Command Functions------
    //-----Implementation of ICommand for views
    let viewCommand ( exec : (View -> unit) )=
        let event = Event<_,_>()
        { new System.Windows.Input.ICommand with
            member __.CanExecute(_) = true
            member __.Execute(arg) = exec (arg :?> View)
            [<CLIEvent>]
            member __.CanExecuteChanged = event.Publish
        }
    // ----- List of the various Views 
    let viewList = 
            [PlotCanvas screen_Canvas; 
             Text screen_Text_TextBox; 
             Function function_Grid; 
             Option option_Grid; 
             Function2D function2D_Grid; 
             Option2D option2D_Grid; 
             Function3D function3D_Grid; 
             Option3D option3D_Grid]
    
    //----- Implementation of ICommand for modes
    let modeCommand ( exec : (Mode -> unit) )=
        let event = Event<_,_>()
        { new System.Windows.Input.ICommand with
            member __.CanExecute(_) = true
            member __.Execute(arg) = exec (arg :?> Mode)
            [<CLIEvent>]
            member __.CanExecuteChanged = event.Publish
        }
    // -----List of the various Modes
    let modeButtonList = 
        [ InputMode.Conventional memoryButton_Grid; 
          InputMode.RPN rpnButton_Grid 
          InputMode.Graph graphButton_Grid ]
    
//  ----- Getters       
    // a function that gets active model
    let getActive2DModel (s:State) = 
        let convertPoint = fun point ->            
            match point with
            | (Point(X (Math.Pure.Quantity.Real x),
                     Y (Math.Pure.Quantity.Real y))) -> Pt ( System.Windows.Point(x |> applyScaleX |> mapXToCanvas, y |> applyScaleY |> mapYToCanvas))
            | (Point3D(X (Math.Pure.Quantity.Real x),
                       Y (Math.Pure.Quantity.Real y),
                       Z (Math.Pure.Quantity.Real z))) -> Pt3D ( System.Windows.Media.Media3D.Point3D(x,y,z) )
            | _ -> failwith "Not a point."
        let convertSegment = fun segment -> 
            match segment with 
            | LineSegment(Point(X (Math.Pure.Quantity.Real x),
                                Y (Math.Pure.Quantity.Real y))) -> 
                                    System.Windows.Media.LineSegment( 
                                        System.Windows.Point(
                                            x |> applyScaleX |> mapXToCanvas, 
                                            y |> applyScaleY |> mapYToCanvas),true )
            | _ -> System.Windows.Media.LineSegment( System.Windows.Point(0.,0.),true )        
        
        let models = 
            match s.model with 
            | Trace t -> t
            | Model3D _ -> []
        
        let getPathGeometry model = 
            let segments = List.map (fun segment -> convertSegment segment) model.traceSegments
            let pg = PathGeometry()
            let pf = PathFigure()
            let pt = match convertPoint model.startPoint  with | Pt x -> x | _ -> failwith "Wrong type of point."        
            let path = Path(Stroke = Brushes.Black, StrokeThickness = 2.)
            do  pf.StartPoint <- pt
                List.iter (fun s -> pf.Segments.Add(s)) segments
                pg.Figures.Add(pf)
                path.Data <- pg   
            path
        let paths = List.map (fun m -> getPathGeometry m) models
        paths        
    let getDrawOptions() =
        let real = fun n -> Math.Pure.Quantity.NumberType.Real n
        let uX = 
            let n = 
                match state.mode with
                | Graph2DParametric -> System.Double.TryParse (option2D_xMax_TextBox.Text)
                | _ -> System.Double.TryParse (option_xMax_TextBox.Text)
                
            match n with
            | true, d -> X (real d)
            | false, _ -> GraphingImplementation.defaultOptions.upperX
        let uY = 
            let n = 
                match state.mode with
                | Graph2DParametric -> System.Double.TryParse (option2D_yMax_TextBox.Text)
                | _ -> System.Double.TryParse (option_yMax_TextBox.Text)
            match n with
            | true, d -> Y (real d)
            | false, _ -> GraphingImplementation.defaultOptions.upperY
        let lX = 
            let n = 
                match state.mode with
                | Graph2DParametric -> System.Double.TryParse (option2D_xMin_TextBox.Text)
                | _ -> System.Double.TryParse (option_xMin_TextBox.Text)
            match n with
            | true, d -> X (real d)
            | false, _ -> GraphingImplementation.defaultOptions.lowerX
        let lY = 
            let n = 
                match state.mode with
                | Graph2DParametric -> System.Double.TryParse (option2D_yMin_TextBox.Text)
                | _ -> System.Double.TryParse (option_yMin_TextBox.Text)
            match n with
            | true, d -> Y (real d)
            | false, _ -> GraphingImplementation.defaultOptions.lowerY        
        let uT = 
            match option2D_Grid.IsVisible || function2D_Grid.IsVisible with 
            | true -> 
                let n = System.Double.TryParse (option2D_tMax_TextBox.Text)                
                match n with
                | true, d -> T (real d)
                | false, _ -> GraphingImplementation.defaultOptions.upperT
            | false -> 
                let n = System.Double.TryParse (option3D_tMax_TextBox.Text)
                match n with
                | true, d -> T (real d)
                | false, _ -> GraphingImplementation.defaultOptions.upperT
        let lT = 
            match option2D_Grid.IsVisible || function2D_Grid.IsVisible with 
            | true -> 
                let n = System.Double.TryParse (option2D_tMin_TextBox.Text)                
                match n with
                | true, d -> T (real d)
                | false, _ -> GraphingImplementation.defaultOptions.lowerT
            | false -> 
                let n = System.Double.TryParse (option3D_tMin_TextBox.Text)
                match n with
                | true, d -> T (real d)
                | false, _ -> GraphingImplementation.defaultOptions.lowerT
        let tStep = 
            match option2D_Grid.IsVisible || function2D_Grid.IsVisible with 
            | true -> 
                let n = System.Double.TryParse (option2D_tStep_TextBox.Text)                
                match n with
                | true, d -> Tstep (real d)
                | false, _ -> GraphingImplementation.defaultOptions.Tstep
            | false -> 
                let n = System.Double.TryParse (option3D_tStep_TextBox.Text)
                match n with
                | true, d -> Tstep (real d)
                | false, _ -> GraphingImplementation.defaultOptions.Tstep
        let uU = 
            let n = System.Double.TryParse (option3D_uMax_TextBox.Text)
            match n with
            | true, d -> U (real d)
            | false, _ -> GraphingImplementation.defaultOptions.upperU       
        let lU = 
            let n = System.Double.TryParse (option3D_uMin_TextBox.Text)
            match n with
            | true, d -> U (real d)
            | false, _ -> GraphingImplementation.defaultOptions.lowerU
        let uStep = 
            let n = System.Double.TryParse (option3D_uStep_TextBox.Text)
            match n with
            | true, d -> Ustep (real d)
            | false, _ -> GraphingImplementation.defaultOptions.uStep
        let uV = 
            let n = System.Double.TryParse (option3D_vMax_TextBox.Text)
            match n with
            | true, d -> V (real d)
            | false, _ -> GraphingImplementation.defaultOptions.upperV
        let lV = 
            let n = System.Double.TryParse (option3D_vMin_TextBox.Text)
            match n with
            | true, d -> V (real d)
            | false, _ -> GraphingImplementation.defaultOptions.lowerV
        let vStep = 
            let n = System.Double.TryParse (option3D_vStep_TextBox.Text)
            match n with
            | true, d -> Vstep (real d)
            | false, _ -> GraphingImplementation.defaultOptions.vStep        
        { upperX = uX; 
          lowerX = lX; 
          upperY = uY; 
          lowerY = lY; 
          upperT = uT; 
          lowerT = lT; 
          Tstep  = tStep
          upperU = uU; 
          lowerU = lU; 
          uStep  = uStep
          upperV = uV; 
          lowerV = lV; 
          vStep  = vStep }

// ----- Setters    
    // a function that sets the options text
    let setGraphOptionText (options:DrawingOptions) =
        let getValueFromXCoordinate c = match c with | X (Math.Pure.Quantity.Real x) -> x.ToString() | _ -> ""
        let getValueFromYCoordinate c = match c with | Y (Math.Pure.Quantity.Real y) -> y.ToString() | _ -> ""
        let getValueFromT t = match t with | T (Math.Pure.Quantity.Real t) -> t.ToString() | _ -> ""
        let getValueFromTstep ts = match ts with | Tstep (Math.Pure.Quantity.Real ts) -> ts.ToString() | _ -> ""        
        let getValueFromU u = match u with | U (Math.Pure.Quantity.Real u) -> u.ToString() | _ -> ""
        let getValueFromUstep us = match us with | Ustep (Math.Pure.Quantity.Real us) -> us.ToString() | _ -> ""
        let getValueFromV v = match v with | V (Math.Pure.Quantity.Real v) -> v.ToString() | _ -> ""
        let getValueFromVstep vs = match vs with | Vstep (Math.Pure.Quantity.Real vs) -> vs.ToString() | _ -> ""
        do
            option_xMin_TextBox.Text <- getValueFromXCoordinate options.lowerX
            option_xMax_TextBox.Text <- getValueFromXCoordinate options.upperX
            option_yMin_TextBox.Text <- getValueFromYCoordinate options.lowerY
            option_yMax_TextBox.Text <- getValueFromYCoordinate options.upperY
            option2D_xMin_TextBox.Text <- getValueFromXCoordinate options.lowerX
            option2D_xMax_TextBox.Text <- getValueFromXCoordinate options.upperX
            option2D_yMin_TextBox.Text <- getValueFromYCoordinate options.lowerY
            option2D_yMax_TextBox.Text <- getValueFromYCoordinate options.upperY
            option2D_tMax_TextBox.Text <- getValueFromT options.upperT
            option2D_tMin_TextBox.Text <- getValueFromT options.lowerT
            option2D_tStep_TextBox.Text <- getValueFromTstep options.Tstep
            option3D_tMax_TextBox.Text <- getValueFromT options.upperT
            option3D_tMin_TextBox.Text <- getValueFromT options.lowerT
            option3D_tStep_TextBox.Text <- getValueFromTstep options.Tstep
            option3D_uMax_TextBox.Text <- getValueFromU options.upperU
            option3D_uMin_TextBox.Text <- getValueFromU options.lowerU
            option3D_uStep_TextBox.Text <- getValueFromUstep options.uStep
            option3D_vMax_TextBox.Text <- getValueFromV options.upperV
            option3D_vMin_TextBox.Text <- getValueFromV options.lowerV
            option3D_vStep_TextBox.Text <- getValueFromVstep options.vStep
    // a function that sets active display
    let setActiveDisplay display =
        do  List.iter (fun x -> 
            match x with
            | View.PlotCanvas p
            | View.Function p
            | View.Function2D p
            | View.Function3D p
            | View.Option p
            | View.Option2D p
            | View.Option3D p -> 
                p.Visibility <- Visibility.Collapsed
                canvas_DockPanel.Visibility <- Visibility.Collapsed
                viewport3D.Visibility <- Visibility.Collapsed
            | View.Text t -> 
                t.Visibility <- Visibility.Collapsed
                canvas_DockPanel.Visibility <- Visibility.Collapsed) viewList
        match display with
        | View.PlotCanvas p ->            
            p.Visibility <- Visibility.Visible            
            match state.mode = Graph3DParametric with
            | false -> canvas_DockPanel.Visibility <- Visibility.Visible
            | true -> viewport3D.Visibility <- Visibility.Visible
        | View.Function p -> 
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph)
            (p.Visibility <- Visibility.Visible)
        | View.Function2D p ->
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph2DParametric)
            (p.Visibility <- Visibility.Visible)
        | View.Function3D p ->
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph3DParametric)
            (p.Visibility <- Visibility.Visible)
        | View.Option p ->
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph)
            (p.Visibility <- Visibility.Visible)
        | View.Option2D p ->
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph2DParametric)
            (p.Visibility <- Visibility.Visible)
        | View.Option3D p -> 
            setGraphOptionText (GraphServices.getDrawingOptionsFromState state.graph3DParametric)
            (p.Visibility <- Visibility.Visible)
        | View.Text t -> (t.Visibility <- Visibility.Visible)        
    // a function that sets the displayed text
    let setDisplayedText =         
        fun text ->
            let mode = state.mode
            match mode with
            | RPN 
            | Conventional -> screen_Text_TextBox.Text <- text 
            | Graph -> function_y_TextBox.Text <- text
            | Graph2DParametric -> 
                let x,y = function2D_xt_TextBox.MaxLines, function2D_yt_TextBox.MaxLines
                match x = 3, y = 3 with
                | true,true -> 
                    do function2D_xt_TextBox.Text <- text
                       function2D_yt_TextBox.Text <- text
                | true,false -> 
                    do function2D_yt_TextBox.Text <- text
                | false,true -> 
                    do function2D_xt_TextBox.Text <- text
                | false,false -> 
                    do function2D_xt_TextBox.Text <- text
                       function2D_yt_TextBox.Text <- text
            | Graph3DParametric ->                 
                let xText, yText, zText =
                    function3D_fx_TextBox.Text,
                    function3D_fy_TextBox.Text,
                    function3D_fz_TextBox.Text
                let dim3DData = GraphServices.getDim3DStateDataFromState state.graph3DParametric 
                match xText = "" ||  yText = "" || zText = "" with
                | true -> 
                    do function3D_fx_TextBox.Text <- dim3DData.x.ToString()
                       function3D_fy_TextBox.Text <- dim3DData.y.ToString()
                       function3D_fz_TextBox.Text <- dim3DData.x.ToString()
                | false ->               
                    match dim3DData.activeExpression with
                    | X_ -> do function3D_fx_TextBox.Text <- text
                    | Y_ -> do function3D_fy_TextBox.Text <- text
                    | Z_ -> do function3D_fz_TextBox.Text <- text
    // a function that sets the pending op text
    let setMemoText = 
        fun text -> memo.Text <- text
    // a function that sets the pending op text
    let setPendingOpText = 
        fun text -> 
            immediate.Text <- text 
            immediate.ScrollToEnd()
    // a function that sets the active mode buttons
    let setActiveModeButtons mode =
        do  List.iter (fun x -> 
            match x with
            | InputMode.Conventional c -> (c.Visibility <- Visibility.Collapsed)
            | InputMode.RPN r -> (r.Visibility <- Visibility.Collapsed)   
            | InputMode.Graph g -> (g.Visibility <- Visibility.Collapsed)
            | InputMode.Graph2DParametric g -> (g.Visibility <- Visibility.Collapsed)
            | InputMode.Graph3DParametric g -> (g.Visibility <- Visibility.Collapsed)) modeButtonList

        match mode with
        | Conventional -> 
            memoryButton_Grid.Visibility <- Visibility.Visible
            memoryButton_Grid.IsHitTestVisible <- true 
        | RPN -> 
            rpnButton_Grid.Visibility <- Visibility.Visible
        | Graph -> 
            graphButton_Grid.IsHitTestVisible <- false
        | Graph2DParametric -> 
            graphButton_Grid.IsHitTestVisible <- false
        | Graph3DParametric -> 
            graphButton_Grid.IsHitTestVisible <- false
    // a function that sets the input mode
    let setInputMode mode = 
        do state <- {state with mode = mode}
        match mode with 
        | Conventional ->
            setActiveModeButtons mode
            setActiveDisplay (View.Text screen_Text_TextBox)
            setDisplayedText "Conventional" 
            x_Button.IsHitTestVisible <- false
            t_Button.IsHitTestVisible <- false
            u_Button.IsHitTestVisible <- false
            v_Button.IsHitTestVisible <- false
            x_Button.Background <- Style.linearGradientBrush_2
            t_Button.Background <- Style.linearGradientBrush_2
            u_Button.Background <- Style.linearGradientBrush_2
            v_Button.Background <- Style.linearGradientBrush_2
        | RPN ->
            setActiveModeButtons mode
            setActiveDisplay (View.Text screen_Text_TextBox)
            setDisplayedText (RpnServices.getDisplayFromStack state.rpn)
            x_Button.IsHitTestVisible <- false
            t_Button.IsHitTestVisible <- false
            u_Button.IsHitTestVisible <- false
            v_Button.IsHitTestVisible <- false
            x_Button.Background <- Style.linearGradientBrush_2
            t_Button.Background <- Style.linearGradientBrush_2
            u_Button.Background <- Style.linearGradientBrush_2
            v_Button.Background <- Style.linearGradientBrush_2
        | Graph ->
            setActiveModeButtons mode
            setActiveDisplay (View.Function function_Grid)
            setDisplayedText (GraphServices.getDisplayFromGraphState state.graph)
            x_Button.IsHitTestVisible <- true
            t_Button.IsHitTestVisible <- false
            u_Button.IsHitTestVisible <- false
            v_Button.IsHitTestVisible <- false
            x_Button.Background <- Style.linearGradientBrush_1
            t_Button.Background <- Style.linearGradientBrush_2
            u_Button.Background <- Style.linearGradientBrush_2
            v_Button.Background <- Style.linearGradientBrush_2
        | Graph2DParametric ->
            setActiveModeButtons mode
            setActiveDisplay (View.Function2D function2D_Grid)
            setDisplayedText (GraphServices.getDisplayFromGraphState state.graph2DParametric)
            x_Button.IsHitTestVisible <- false
            t_Button.IsHitTestVisible <- true
            u_Button.IsHitTestVisible <- false
            v_Button.IsHitTestVisible <- false
            x_Button.Background <- Style.linearGradientBrush_2
            t_Button.Background <- Style.linearGradientBrush_1
            u_Button.Background <- Style.linearGradientBrush_2
            v_Button.Background <- Style.linearGradientBrush_2
        | Graph3DParametric ->
            setActiveModeButtons mode
            setActiveDisplay (View.Function3D function3D_Grid)
            setDisplayedText (GraphServices.getDisplayFromGraphState state.graph3DParametric)
            x_Button.IsHitTestVisible <- false
            match tRadio.IsChecked.Value with
            | false -> 
                do  t_Button.IsHitTestVisible <- false
                    u_Button.IsHitTestVisible <- true
                    v_Button.IsHitTestVisible <- true
                    x_Button.Background <- Style.linearGradientBrush_2
                    t_Button.Background <- Style.linearGradientBrush_2
                    u_Button.Background <- Style.linearGradientBrush_1
                    v_Button.Background <- Style.linearGradientBrush_1
                    function3D_fxLabel_TextBlock.Text <- "fx(u,v) = "
                    function3D_fyLabel_TextBlock.Text <- "fy(u,v) = "
                    function3D_fzLabel_TextBlock.Text <- "fz(u,v) = "
                    function3D_SolidMesh_Button.Content <- "Build Surface"
            | true -> 
                do  t_Button.IsHitTestVisible <- true
                    u_Button.IsHitTestVisible <- false
                    v_Button.IsHitTestVisible <- false
                    x_Button.Background <- Style.linearGradientBrush_2
                    t_Button.Background <- Style.linearGradientBrush_1
                    u_Button.Background <- Style.linearGradientBrush_2
                    v_Button.Background <- Style.linearGradientBrush_2
                    function3D_fxLabel_TextBlock.Text <- "fx(t) = "
                    function3D_fyLabel_TextBlock.Text <- "fy(t) = "
                    function3D_fzLabel_TextBlock.Text <- "fz(t) = "
                    function3D_SolidMesh_Button.Content <- "Build Curve"
    // a function that sets the active model
    let setActivetModel model =        
        do state <- {state with model = model}
    // a function that sets the model scale
    let setModelScale x y = do state <- {state with scale = (x,y)}
    // a function that sets the model rotation
    let setQuaternion quaternion = do state <- {state with quaternion = quaternion}
    
//-----Create Menu
    let menu = // 
         let header1 = MenuItem(Header = "Mode")
         let header1_Item1 = MenuItem(Header = "Calculator")
         let header1_Item1_1 = MenuItem(Header = "RPN Stack", Command = modeCommand (setInputMode), CommandParameter = RPN)
         let header1_Item1_2 = MenuItem(Header = "Conventional", Command = modeCommand (setInputMode), CommandParameter = Conventional, Icon = checkedBox)
         
         let handleCheck (item1:MenuItem) (item2:MenuItem) = 
             do  item1.Icon <- checkedBox                
                 item2.Icon <- None

         let header1_Item2 = MenuItem(Header = "Graph", Command = modeCommand (setInputMode), CommandParameter = Graph)
         let header1_Item3 = MenuItem(Header = "Graph2D", Command = modeCommand (setInputMode), CommandParameter = Graph2DParametric)
         let header1_Item4 = MenuItem(Header = "Graph3D", Command = modeCommand (setInputMode), CommandParameter = Graph3DParametric)
         
         let header1_Item5 = MenuItem(Header = "Plot Canvas", Command = viewCommand (setActiveDisplay), CommandParameter = PlotCanvas screen_Canvas)
         
         do  header1_Item1.Items.Add(header1_Item1_1) |> ignore
             header1_Item1.Items.Add(header1_Item1_2) |> ignore
             header1.Items.Add(header1_Item1) |> ignore
             header1.Items.Add(header1_Item2) |> ignore
             header1.Items.Add(header1_Item3) |> ignore
             header1.Items.Add(header1_Item4) |> ignore
             header1.Items.Add(header1_Item5) |> ignore

         let header2 = MenuItem(Header = "Options")
         let header2_Item1 = MenuItem(Header = "Graph Options", Command = viewCommand (setActiveDisplay), CommandParameter = Option option_Grid)
         let header2_Item2 = MenuItem(Header = "Graph2D Options", Command = viewCommand (setActiveDisplay), CommandParameter = Option2D option2D_Grid)
         let header2_Item3 = MenuItem(Header = "Graph3D Options", Command = viewCommand (setActiveDisplay), CommandParameter = Option3D option3D_Grid)
         
         do  header2.Items.Add(header2_Item1) |> ignore
             header2.Items.Add(header2_Item2) |> ignore
             header2.Items.Add(header2_Item3) |> ignore
         
         let m = Menu()
         do  m.SetValue(Grid.RowProperty,0)
             m.Items.Add(header1) |> ignore
             m.Items.Add(header2) |> ignore
             header1_Item1_1.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleCheck header1_Item1_1 header1_Item1_2))            
             header1_Item1_2.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleCheck header1_Item1_2 header1_Item1_1))
         m
   
    do  // Assemble the pieces        
        screen_Grid.Children.Add(menu) |> ignore
        screen_Grid.Children.Add(function_Grid) |> ignore
        screen_Grid.Children.Add(function2D_Grid) |> ignore
        screen_Grid.Children.Add(function3D_Grid) |> ignore
        screen_Grid.Children.Add(option_Grid) |> ignore
        screen_Grid.Children.Add(option2D_Grid) |> ignore
        screen_Grid.Children.Add(option3D_Grid) |> ignore
        screen_Border.Child <- screen_Grid
        
        calculator_Layout_Grid.Children.Add(screen_Border) |> ignore
        calculator_Layout_Grid.Children.Add(immediate) |> ignore
        calculator_Layout_Grid.Children.Add(funcButton_Grid) |> ignore
        calculator_Layout_Grid.Children.Add(calcButton_Grid) |> ignore

        calculator_Grid.Children.Add(calculator_Layout_Grid) |> ignore
        
        graphingCalculator.Content <- calculator_Grid

        setInputMode state.mode

//////////////////////////////////////////////
    //-------setup calculator logic----------
    let calculatorServices = CalculatorServices.createServices()
    let rpnServices = RpnServices.createServices()
    let graphServices = GraphServices.createGraphServices()
    let calculate = CalculatorImplementation.createCalculate calculatorServices 
    let calculateRpn = RpnImplementation.createCalculate rpnServices
    let calculateGraph = GraphingImplementation.createEvaluate graphServices
    
    //-------create event handlers----------
        // Input
    let handleConventionalInput input =         
        let newState =  calculate(input,state.conventional)
        state <- { state with conventional = newState}
        setDisplayedText (calculatorServices.getDisplayFromState newState)
        setPendingOpText (calculatorServices.getPendingOpFromState newState)
        setMemoText (calculatorServices.getMemoFromState newState)
    let handleRpnInput input = 
        let newState =  calculateRpn(input,state.rpn)
        state <- { state with rpn = newState }
        setDisplayedText (rpnServices.getDisplayFromStack newState)
        setPendingOpText (rpnServices.getDisplayFromRpnState newState)        
    let handleStackOperation stackOp =
        match stackOp with
        | Push
        | Pop
        | Drop
        | Duplicate
        | ClearStack            
        | Swap ->                   
            let newState =  
                match state.rpn with
                | ReadyState {stack = stk} -> 
                    let opResult = rpnServices.doStackOperation (stackOp, None, stk)
                    match opResult with
                    | Success s -> ReadyState {stack = s}
                    | Failure f -> ErrorState {error = f}
                | DigitAccumulatorState {digits = acc; stack = stk}
                | DecimalAccumulatorState {digits = acc; stack = stk} -> 
                    let numb = rpnServices.getNumberFromAccumulator {digits = acc; stack = stk}
                    let opResult = rpnServices.doStackOperation (stackOp, Some (numb), stk)
                    match opResult with
                    | Success s -> ReadyState {stack = s}
                    | Failure f -> ErrorState {error = f}
                | ErrorState {error = e} -> ErrorState {error = e}           
            state <- { state with rpn = newState}
            setDisplayedText (rpnServices.getDisplayFromStack newState) 
            setPendingOpText ""
    let handleGraphInput input =
        let newState = 
            match state.mode with
            | Graph -> calculateGraph (input, state.graph)
            | Graph2DParametric -> calculateGraph (input, state.graph2DParametric)
            | Graph3DParametric -> calculateGraph (input, state.graph3DParametric)
            | _ -> ExpressionErrorState {lastExpression = Math.Pure.Structure.Number (Math.Pure.Quantity.Number.Zero); error = InputError }

        let expressionText = 
            match newState with
            | DrawState _d -> "Graph"
            | EvaluatedState e -> graphServices.getDisplayFromExpression e.evaluatedExpression           
            | ParentheticalState p -> graphServices.getDisplayFromExpression p.evaluatedExpression
            | ExpressionDigitAccumulatorState e -> graphServices.getDisplayFromExpression e.expression          
            | ExpressionDecimalAccumulatorState e -> graphServices.getDisplayFromExpression e.expression           
            | ExpressionErrorState e -> graphServices.getDisplayFromExpression e.lastExpression          
            | DrawErrorState d -> graphServices.getDisplayFromExpression d.lastExpression        
            // Parametric 2D
            | DrawState2DParametric (_d,_s) -> "Graph 2D Parametric"
            | EvaluatedState2DParametric (e,_s) -> graphServices.getDisplayFromExpression e.evaluatedExpression           
            | ParentheticalState2DParametric (p,_s) -> graphServices.getDisplayFromExpression p.evaluatedExpression
            | ExpressionDigitAccumulatorState2DParametric (e,_s) -> graphServices.getDisplayFromExpression e.expression          
            | ExpressionDecimalAccumulatorState2DParametric (e,_s) -> graphServices.getDisplayFromExpression e.expression           
            // Parametric 3D
            | DrawState3DParametric (_d,_s) -> "Graph 3D Parametric"
            | EvaluatedState3DParametric (e,_s) -> graphServices.getDisplayFromExpression e.evaluatedExpression           
            | ParentheticalState3DParametric (p,_s) -> graphServices.getDisplayFromExpression p.evaluatedExpression
            | ExpressionDigitAccumulatorState3DParametric (e,_s) -> graphServices.getDisplayFromExpression e.expression          
            | ExpressionDecimalAccumulatorState3DParametric (e,_s) -> graphServices.getDisplayFromExpression e.expression           
            
        state <- match state.mode with
                 | Graph -> { state with graph = newState }
                 | Graph2DParametric -> { state with graph2DParametric = newState } 
                 | Graph3DParametric -> { state with graph3DParametric = newState}
                 | _ -> state
        
        setDisplayedText expressionText
        
        match newState with
        | DrawState d -> 
            do  state <- { state with graph = newState }
                canvas.Children.Clear()
                setActivetModel (Trace d.trace)                    
            let models = getActive2DModel state    
            do  List.iter (
                    fun (model : System.Windows.Shapes.Path) -> 
                         model.RenderTransform <- ScaleTransform(scaleX = (fst state.scale), scaleY = (snd state.scale), centerX = mapXToCanvas 0., centerY = mapYToCanvas 0.)
                         model.StrokeThickness <- 2. / ((fst state.scale + snd state.scale) / 2.)
                         canvas.Children.Add(model) |> ignore
                         canvas.Children.Add(( placeOrginPoint (mapXToCanvas 0.) (mapYToCanvas 0.) )) |> ignore) models
                setActiveDisplay (PlotCanvas screen_Canvas)
                setPendingOpText "--->>> graph" 
                // add grid if checked
                match canvasGridLines_CheckBox.IsChecked.HasValue = true &&
                      canvasGridLines_CheckBox.IsChecked.Value = true with
                | true -> 
                    let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
                    do  canvas.Children.Insert(0,gl) 
                        canvasGridLine_Slider.IsEnabled <- true
                | false -> ()
        | EvaluatedState ev ->
            do state <- { state with graph = newState }
               setActiveDisplay (Function function_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction ev.pendingFunction) + " --->>> Evaluated. What's next")
        | ParentheticalState p ->
            do state <- { state with graph = newState }
               setActiveDisplay (Function function_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction p.pendingFunction) + " --->>> Open Parentheses")
        | ExpressionDigitAccumulatorState _ed ->
            do state <- { state with graph = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")
        | ExpressionDecimalAccumulatorState _ed ->
            do state <- { state with graph = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")
        // Parametric 2D
        | DrawState2DParametric (d,_s) -> 
            do state <- { state with graph2DParametric = newState }
               canvas.Children.Clear()
               setActivetModel (Trace d.trace)                    
            let models = getActive2DModel state    
            do  List.iter (
                    fun( model : System.Windows.Shapes.Path) -> 
                         model.RenderTransform <- ScaleTransform(scaleX = (fst state.scale), scaleY = (snd state.scale), centerX = mapXToCanvas 0., centerY = mapYToCanvas 0.)
                         model.StrokeThickness <- 2. / ((fst state.scale + snd state.scale) / 2.)
                         canvas.Children.Add(model) |> ignore
                         canvas.Children.Add(( placeOrginPoint (mapXToCanvas 0.) (mapYToCanvas 0.) )) |> ignore) models
            setActiveDisplay (PlotCanvas screen_Canvas)
            setPendingOpText "--->>> graph 2D Parametric" 
            // add grid if checked
            match canvasGridLines_CheckBox.IsChecked.HasValue = true &&
                  canvasGridLines_CheckBox.IsChecked.Value = true with
            | true -> 
                let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
                do  canvas.Children.Insert(0,gl) 
                    canvasGridLine_Slider.IsEnabled <- true
            | false -> ()
        | EvaluatedState2DParametric (ev,_s) -> 
            do state <- { state with graph2DParametric = newState }
               setActiveDisplay (Function2D function2D_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction ev.pendingFunction) + " --->>> Evaluated. What's next")
        | ParentheticalState2DParametric (p,_s) ->
            do state <- { state with graph2DParametric = newState }
               setActiveDisplay (Function2D function2D_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction p.pendingFunction) + " --->>> Open Parentheses")
        | ExpressionDigitAccumulatorState2DParametric (e,_s) ->
            do state <- { state with graph2DParametric = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")
        | ExpressionDecimalAccumulatorState2DParametric (e,_s) ->
            do state <- { state with graph2DParametric = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")         
        // Parametric 3D
        | DrawState3DParametric (sd,_d3) ->             
            match sd.mesh with
            | None -> ()
            | Some m -> 
                do state <- { state with graph3DParametric = newState }
                   model3DGroup.Children.Clear()
                   model3DGroup.Children.Add(light)
                   //model3DGroup.Children.Add(light2)
                   setActivetModel (Model3D m)   
                match state.model with
                | Trace _ -> ()
                | Model3D (Surface m) -> 
                   do
                   model3DGroup.Children.Add(m)
                   setActiveDisplay (PlotCanvas screen_Canvas)
                   setPendingOpText "--->>> graph 3D Parametric surface" 
                | Model3D (Curve m) -> 
                    do
                    model3DGroup.Children.Add(m)
                    setActiveDisplay (PlotCanvas screen_Canvas)
                    setPendingOpText "--->>> graph 3D Parametric curve"         
        | EvaluatedState3DParametric (sd,d3) -> 
            let newState = graphServices.setActivate3DStateExpression (newState,d3.activeExpression)
            do state <- { state with graph3DParametric = newState }
               setActiveDisplay (Function3D function3D_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction sd.pendingFunction) + " --->>> Evaluated. What's next")
        | ParentheticalState3DParametric (sd,_d3) ->
            do state <- { state with graph3DParametric = newState }
               setActiveDisplay (Function3D function3D_Grid)
               setPendingOpText ((graphServices.getDisplayFromPendingFunction sd.pendingFunction) + " --->>> Open Parentheses")
        | ExpressionDigitAccumulatorState3DParametric (_sd,_d3) ->
            do state <- { state with graph3DParametric = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")
        | ExpressionDecimalAccumulatorState3DParametric (_sd,_d3) ->
            do state <- { state with graph3DParametric = newState }
               setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " --->>>")        
        | _ -> setPendingOpText ((graphServices.getDisplayFromGraphState newState) + " ---<<<")
        // Gridlines
    let handleGridLinesOnCheck () =
        let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
        do  canvas.Children.Insert(0,gl) 
            canvasGridLine_Slider.IsEnabled <- true
    let handleGridLinesOnUnCheck () =
        removeGridLines ()
        canvasGridLine_Slider.IsEnabled <- false
        // Scale 
    let handleSliderValueOnValueChange () =
        removeGridLines ()
        let xScale, yScale = (canvasGridLine_Slider.Value,canvasGridLine_Slider.Value)
        let gl = makeGridLines xScale yScale
        do  setModelScale (xScale/5.) (yScale/5.)
            canvas.Children.Insert(0,gl)
            setActiveDisplay (Function function_Grid)
            handleGraphInput Draw
        // 2D Parametric
    let handleTextBoxXtPreviewMouseDown () =
        match function2D_xt_TextBox.MaxLines = 3 with
        | true ->
            do state <- {state with graph2DParametric = graphServices.toggle2DParametricState state.graph2DParametric}
               function2D_yt_TextBox.MaxLines <- 3
               function2D_xt_TextBox.MaxLines <- 15
               function2D_xt_TextBox.BorderThickness <- Thickness(3.)
               function2D_yt_TextBox.BorderThickness <- Thickness(1.)
        | false -> ()
    let handleTextBoxYtPreviewMouseDown () =
        match function2D_yt_TextBox.MaxLines = 3 with
        | true -> 
            do state <- {state with graph2DParametric = graphServices.toggle2DParametricState state.graph2DParametric}
               function2D_xt_TextBox.MaxLines <- 3
               function2D_yt_TextBox.MaxLines <- 15
               function2D_xt_TextBox.BorderThickness <- Thickness(1.)
               function2D_yt_TextBox.BorderThickness <- Thickness(3.)
        | false -> ()
    let handleFunction2DButtons (input) =        
        match  input = Draw2DParametric  with 
        | false -> 
            handleTextBoxXtPreviewMouseDown () 
            handleGraphInput(input)
            handleTextBoxYtPreviewMouseDown ()
            setDisplayedText (graphServices.getDisplayFromGraphState state.graph2DParametric)
        | true -> 
            handleTextBoxXtPreviewMouseDown () 
            handleGraphInput(input)
        // 3D Rotation and Zoom
    let handleViewport3D_MouseMove (e :Input.MouseEventArgs) =                                 
        match e.LeftButton = MouseButtonState.Pressed with
        | true ->             
            let point = e.MouseDevice.GetPosition(viewport3D) 
            let delta = ((viewport3D.Tag :?> System.Windows.Point) - point) / 2.
            let mouse = Vector3D(delta.X, -delta.Y, 0.)
            let axis = Vector3D.CrossProduct(mouse, Vector3D(0., 0., 1.))            
            match axis.Length < 0.000001 with 
            | true -> 
                do  setQuaternion (Quaternion(new Vector3D(0., 0., 1.), 0.))
                    immediate.Text <- state.quaternion.ToString()  + " check5"
            | false -> 
                let rotationDelta = Quaternion(axis, axis.Length/perspective_Camera.Position.Z) // divide by Camera.Position.Z to slow down rotation
                let newQ = rotationDelta * state.quaternion 
                do  setQuaternion (newQ)
                    rotateTransform3D.Rotation <- QuaternionRotation3D(newQ)
                    model3DGroup.Transform <- rotateTransform3D
                    immediate.Text <- state.quaternion.ToString()  + " check6"
        | false ->  immediate.Text <- state.quaternion.ToString()  + " check 99" 
    let handleViewport3D_MouseLeftButtonDown (e :Input.MouseButtonEventArgs) = 
        let point = e.MouseDevice.GetPosition(viewport3D)        
        do  viewport3D.Tag <- point
            immediate.Text <- viewport3D.Tag.ToString() + " check3"
    let handleViewport3D_MouseLeftButtonUp (e :Input.MouseButtonEventArgs) = 
        let point = e.MouseDevice.GetPosition(viewport3D)        
        let delta = ((viewport3D.Tag :?> System.Windows.Point) - point) / 2.
        let mouse = Vector3D(delta.X, -delta.Y, 0.)
        let axis = Vector3D.CrossProduct(mouse, Vector3D(0., 0., 1.))                           
        match axis.Length < 0.00001 with 
        | true ->             
            do  setQuaternion (Quaternion(new Vector3D(0., 0., 1.), 0.))
                viewport3D.ReleaseMouseCapture()
                immediate.Text <- viewport3D.Tag.ToString() + " check4" 
        | false -> 
            let rotationDelta = Quaternion(axis, axis.Length/perspective_Camera.Position.Z) // divide by Camera.Position.Z to slow down rotation
            let newQ = rotationDelta * state.quaternion 
            do  setQuaternion (newQ)                    
                rotateTransform3D.Rotation <- QuaternionRotation3D(newQ)
                model3DGroup.Transform <- rotateTransform3D
                viewport3D.ReleaseMouseCapture()
                viewport3D.Tag <- System.Windows.Point(0., 0.)
                immediate.Text <- viewport3D.Tag.ToString() + " check1" 
    let handleViewport3D_MouseWheel (e :Input.MouseWheelEventArgs) =
        let z = perspective_Camera.Position.Z - (float e.Delta/250.)
        match z < 1. with
        | false ->
            do  perspective_Camera.Position <- new Point3D( perspective_Camera.Position.X,
                                                            perspective_Camera.Position.Y,
                                                            perspective_Camera.Position.Z - (float e.Delta/250.))
        | true -> ()
    let handleViewport3D_MouseRightButtonDown () = 
        do  perspective_Camera.Position <- new Point3D( perspective_Camera.Position.X, perspective_Camera.Position.Y, 10.)
            setQuaternion (Quaternion(new Vector3D(0., 0., 1.), 0.))
            rotateTransform3D.Rotation <- QuaternionRotation3D(state.quaternion)
            model3DGroup.Transform <- rotateTransform3D
    //  3D Parametric
    let handleParameterRadioButtons_Checked () =         
        setInputMode Graph3DParametric
    let handleTextBoxFxPreviewMouseDown () =
        match function3D_fx_TextBox.MaxLines = 3 with
        | true ->
            do state <- {state with graph3DParametric = graphServices.setActivate3DStateExpression (state.graph3DParametric,X_)}
               function3D_fx_TextBox.MaxLines <- 15
               function3D_fy_TextBox.MaxLines <- 3
               function3D_fz_TextBox.MaxLines <- 3               
               function3D_fx_TextBox.BorderThickness <- Thickness(3.)
               function3D_fy_TextBox.BorderThickness <- Thickness(1.)
               function3D_fz_TextBox.BorderThickness <- Thickness(1.)
        | false -> ()
    let handleTextBoxFyPreviewMouseDown () =
        match function3D_fy_TextBox.MaxLines = 3 with
        | true -> 
            do state <- {state with graph3DParametric = graphServices.setActivate3DStateExpression (state.graph3DParametric,Y_)}
               function3D_fx_TextBox.MaxLines <- 3
               function3D_fy_TextBox.MaxLines <- 15
               function3D_fz_TextBox.MaxLines <- 3               
               function3D_fx_TextBox.BorderThickness <- Thickness(1.)
               function3D_fy_TextBox.BorderThickness <- Thickness(3.)
               function3D_fz_TextBox.BorderThickness <- Thickness(1.)
        | false -> ()
    let handleTextBoxFzPreviewMouseDown () =
        match function3D_fz_TextBox.MaxLines = 3 with
        | true -> 
            do state <- {state with graph3DParametric = graphServices.setActivate3DStateExpression (state.graph3DParametric,Z_)}
               function3D_fx_TextBox.MaxLines <- 3
               function3D_fy_TextBox.MaxLines <- 3
               function3D_fz_TextBox.MaxLines <- 15
               function3D_fx_TextBox.BorderThickness <- Thickness(1.)
               function3D_fy_TextBox.BorderThickness <- Thickness(1.)
               function3D_fz_TextBox.BorderThickness <- Thickness(3.)
        | false -> ()
    let handleFunction3DButtons (input) =        
        match  input = Draw3DParametric  with 
        | false ->            
            handleTextBoxFxPreviewMouseDown ()
            handleGraphInput(input)
            handleTextBoxFyPreviewMouseDown ()
            setDisplayedText (graphServices.getDisplayFromGraphState state.graph3DParametric)
            handleTextBoxFzPreviewMouseDown ()
            setDisplayedText (graphServices.getDisplayFromGraphState state.graph3DParametric)            
            handleGraphInput(input)
            match input with 
            | HelixExample -> 
                match tRadio.IsChecked.Value with
                | true -> ()
                | false -> 
                    do tRadio.IsChecked <- Nullable true
                       uvRadio.IsChecked <- Nullable false
                       handleParameterRadioButtons_Checked ()
            | SphereExample -> 
                match tRadio.IsChecked.Value with
                | true -> 
                    do tRadio.IsChecked <- Nullable false
                    uvRadio.IsChecked <- Nullable true
                    handleParameterRadioButtons_Checked ()
                | false -> ()
            | ConeExample -> 
                match tRadio.IsChecked.Value with
                | true -> 
                    do tRadio.IsChecked <- Nullable false
                    uvRadio.IsChecked <- Nullable true
                    handleParameterRadioButtons_Checked ()
                | false -> ()
            | TorusExample -> 
                match tRadio.IsChecked.Value with
                | true -> 
                    do tRadio.IsChecked <- Nullable false
                    uvRadio.IsChecked <- Nullable true
                    handleParameterRadioButtons_Checked ()
                | false -> ()(**)
            | _ -> ()
        | true ->  
            handleTextBoxFxPreviewMouseDown ()
            handleGraphInput(input)
            handleGraphInput(GraphingDomain.CalculatorInput.CalcInput Back)
            handleGraphInput(input)
            handleViewport3D_MouseRightButtonDown ()
            

    // a function that sets active handler based on the active input mode display
    let handleInput input =  
        let rpnInput = 
            match input with
            | Zero -> Input Zero 
            | Digit d -> Input (Digit d)
            | DecimalSeparator -> Input DecimalSeparator
            | ConventionalDomain.MathOp op -> Op (MathOp op)
            | Equals -> Enter
            | Clear -> Input Clear
            | ClearEntry -> Input ClearEntry
            | Back -> Input Back
            | MemoryStore -> Input MemoryStore
            | MemoryClear -> Input MemoryClear
            | MemoryRecall -> Input MemoryRecall (**)
        let graphInput = 
            match input with
            | Zero -> CalcInput Zero 
            | Digit d -> CalcInput (Digit d)
            | DecimalSeparator -> CalcInput DecimalSeparator
            | ConventionalDomain.MathOp _ -> CalcInput input
            | Equals -> CalcInput Equals
            | Clear -> CalcInput Clear
            | ClearEntry -> CalcInput ClearEntry
            | Back -> CalcInput Back
            | MemoryStore -> CalcInput MemoryStore
            | MemoryClear -> CalcInput MemoryClear
            | MemoryRecall -> CalcInput MemoryRecall 
        match state.mode with
        | RPN -> handleRpnInput rpnInput
        | Conventional -> handleConventionalInput input
        | Graph -> handleGraphInput graphInput
        | Graph2DParametric -> handleGraphInput graphInput 
        | Graph3DParametric -> handleGraphInput graphInput (**)

    // create objects
    let x = (Math.Pure.Objects.Symbol.Variable "x") |> ExpressionInput.Symbol |> ExpressionInput
    let t = (Math.Pure.Objects.Symbol.Variable "t") |> ExpressionInput.Symbol |> ExpressionInput
    let u = (Math.Pure.Objects.Symbol.Variable "u") |> ExpressionInput.Symbol |> ExpressionInput
    let v = (Math.Pure.Objects.Symbol.Variable "v") |> ExpressionInput.Symbol |> ExpressionInput
    let sin = (Math.Pure.Objects.Function.Sin) |> ExpressionInput.Function |> ExpressionInput
    let cos = (Math.Pure.Objects.Function.Cos) |> ExpressionInput.Function |> ExpressionInput
    let tan = (Math.Pure.Objects.Function.Tan) |> ExpressionInput.Function |> ExpressionInput
    let pi = (Math.Pure.Objects.Symbol.Constant Math.Pure.Objects.Pi) |> ExpressionInput.Symbol |> ExpressionInput
    let e = (Math.Pure.Objects.Symbol.Constant Math.Pure.Objects.E) |> ExpressionInput.Symbol |> ExpressionInput
    
    do  //add event handler to each button click
        one              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit One)))
        two              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Two))) 
        three            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Three)))
        four             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Four))) 
        five             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Five)))
        six              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Six)))
        seven            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Seven)))
        eight            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Eight)))
        nine             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Digit Nine)))
        zero             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Zero)))
        decimalPoint     .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (DecimalSeparator)))
        add              .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Add))) 
        subtract         .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Subtract)))
        multiply         .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Multiply)))
        divide           .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Divide)))
        equals           .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Equals)))
        clear            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Clear)))
        clearEntry       .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (ClearEntry)))
        clearMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryClear)))
        storeMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryStore)))
        recallMemory     .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (MemoryRecall)))
        changeSign       .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp ChangeSign))) 
        back             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (Back))) 
        addToMemory      .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp MemoryAdd))) 
        subtractFromMemoy.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp MemorySubtract))) 
        inverse          .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Inverse)))  
        percent          .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Percent))) 
        root             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleInput (CalculatorInput.MathOp Root))) 
        swap             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleStackOperation (Swap)))
        clearStack       .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleStackOperation (ClearStack))) 
        drop             .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleStackOperation (Drop)))
        enter            .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleStackOperation (Push)))
        duplicate        .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleStackOperation (Duplicate)))
        x_Button         .Click.AddHandler(RoutedEventHandler(fun _ _ -> x |> handleGraphInput ))        
        t_Button         .Click.AddHandler(RoutedEventHandler(fun _ _ -> t |> handleGraphInput ))
        u_Button         .Click.AddHandler(RoutedEventHandler(fun _ _ -> u |> handleGraphInput ))
        v_Button         .Click.AddHandler(RoutedEventHandler(fun _ _ -> v |> handleGraphInput ))
        sin_Button       .Click.AddHandler(RoutedEventHandler(fun _ _ -> sin |> handleGraphInput ))
        cos_Button       .Click.AddHandler(RoutedEventHandler(fun _ _ -> cos |> handleGraphInput ))
        tan_Button       .Click.AddHandler(RoutedEventHandler(fun _ _ -> tan |> handleGraphInput ))
        pi_Button        .Click.AddHandler(RoutedEventHandler(fun _ _ -> pi |> handleGraphInput ))
        e_Button         .Click.AddHandler(RoutedEventHandler(fun _ _ -> e |> handleGraphInput ))
        function_Button  .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(Draw)))
        openParentheses  .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(OpenParentheses)))
        closeParentheses .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(CloseParentheses)))
        option_Save_Button .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionSave (getDrawOptions()))))
        option_Reset_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionReset)))
        option2D_Save_Button .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionSave (getDrawOptions()))))
        option2D_Reset_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionReset)))
        option3D_Save_Button .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionSave (getDrawOptions()))))
        option3D_Reset_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(GraphOptionReset)))
        xSquared_Button  .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(ExpressionSquared)))
        xPowY_Button     .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(ExpressionToThePowerOf)))
        function2D_Graph_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction2DButtons(Draw2DParametric)))
        function2D_Spiral_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction2DButtons(SpiralExample)))
        function2D_Ellipse_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction2DButtons(EllipseExample)))
        function3D_SolidMesh_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction3DButtons(Draw3DParametric)))
        function3D_Helix_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction3DButtons(HelixExample)))
        function3D_Sphere_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction3DButtons(SphereExample)))
        function3D_Cone_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction3DButtons(ConeExample)))
        function3D_Torus_Button.Click.AddHandler(RoutedEventHandler(fun _ _ -> handleFunction3DButtons(TorusExample)))

        // Other events
        canvasGridLines_CheckBox.Checked.AddHandler  (RoutedEventHandler(fun _ _ -> handleGridLinesOnCheck()))
        canvasGridLines_CheckBox.Unchecked.AddHandler(RoutedEventHandler(fun _ _ -> handleGridLinesOnUnCheck()))
        
        canvasGridLine_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleSliderValueOnValueChange ()))

        function2D_yt_TextBox.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleTextBoxYtPreviewMouseDown ()))
        function2D_xt_TextBox.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleTextBoxXtPreviewMouseDown ()))

        function3D_fx_TextBox.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleTextBoxFxPreviewMouseDown ()))
        function3D_fy_TextBox.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleTextBoxFyPreviewMouseDown ()))
        function3D_fz_TextBox.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ _ -> handleTextBoxFzPreviewMouseDown ()))        

        viewport3D.PreviewMouseDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleViewport3D_MouseLeftButtonDown (e)))
        viewport3D.PreviewMouseUp.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleViewport3D_MouseLeftButtonUp (e)))
        viewport3D.PreviewMouseMove.AddHandler(Input.MouseEventHandler(fun _ e -> handleViewport3D_MouseMove (e)))
        viewport3D.PreviewMouseWheel.AddHandler(Input.MouseWheelEventHandler(fun _ e -> handleViewport3D_MouseWheel (e)))
        viewport3D.PreviewMouseRightButtonDown.AddHandler(Input.MouseButtonEventHandler(fun _ e -> handleViewport3D_MouseRightButtonDown ()))

        tRadio.Checked.AddHandler(RoutedEventHandler(fun _ _ -> handleParameterRadioButtons_Checked()))
        uvRadio.Checked.AddHandler(RoutedEventHandler(fun _ _ -> handleParameterRadioButtons_Checked()))