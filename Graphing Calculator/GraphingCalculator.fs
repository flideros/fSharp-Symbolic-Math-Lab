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
open Utilities
open Style
open ConventionalDomain
open RpnDomain
open GraphingDomain

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
    | Trace of Trace

type ViewPoint = 
    | Pt of System.Windows.Point
    | Pt3D of System.Windows.Media.Media3D.Point3D

type InputMode = 
    | Conventional of Grid
    | RPN of Grid
    | Graph of Grid
    
type Mode =    
    | Conventional
    | RPN
    | Graph

type State = 
    { rpn:RpnDomain.CalculatorState;
      conventional:ConventionalDomain.CalculatorState;
      graph:GraphingDomain.CalculatorState;
      mode : Mode;
      model : Model }

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
        { startPoint = (Point(X (Math.Pure.Quantity.Real 10.),Y (Math.Pure.Quantity.Real 20.))); 
          traceSegments = 
            [ LineSegment (Point(X (Math.Pure.Quantity.Real 50.), Y (Math.Pure.Quantity.Real 30. ))); 
              LineSegment (Point(X (Math.Pure.Quantity.Real 100.),Y (Math.Pure.Quantity.Real 200.)));
              LineSegment (Point(X (Math.Pure.Quantity.Real 178.),Y (Math.Pure.Quantity.Real 66. ))) ]}
        |> Trace 
    let graphDefault = 
        { evaluatedExpression = 
            Math.Pure.Quantity.Number.Zero 
            |> Math.Pure.Structure.Number; 
          pendingFunction = None; 
          drawing2DBounds = 
            { upperX = X (Math.Pure.Quantity.Real 150.); 
              lowerX = X (Math.Pure.Quantity.Real -150.); 
              upperY = Y (Math.Pure.Quantity.Real 150.); 
              lowerY = Y (Math.Pure.Quantity.Real -150.)}}
        |> EvaluatedState
    let mutable state = 
        { rpn = rpnDefault; 
          conventional = conventionalDefault; 
          mode = Conventional; 
          model = modelDefault; 
          graph = graphDefault }

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
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 0)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function2D_ytLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "yt = ")
        do tb.SetValue(Grid.RowProperty,1)
        tb
    let function2D_yt_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 1)
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
        let tb = FunctionTextBlock(Text = "fx = ")
        do tb.SetValue(Grid.RowProperty,0)
        tb
    let function3D_fx_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 0)
        do tb.SetValue(Grid.ColumnProperty,1)
        tb
    let function3D_fyLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "fy = ")
        do tb.SetValue(Grid.RowProperty,1)
        tb
    let function3D_fy_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 1)
        do  tb.SetValue(Grid.RowProperty,1)
            tb.SetValue(Grid.ColumnProperty,1)
        tb    
    let function3D_fzLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "fz = ")
        do tb.SetValue(Grid.RowProperty,2)
        tb
    let function3D_fz_TextBox = 
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 2)
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
    let function3D_ShapeButton_Grid =
        let grid = Grid() 
        
        let column1 = ColumnDefinition(Width = GridLength(1., GridUnitType.Star))
        let column2 = ColumnDefinition(Width = GridLength.Auto)
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        let column4 = ColumnDefinition(Width = GridLength.Auto)
        
        do  grid.ColumnDefinitions.Add(column1)
            grid.ColumnDefinitions.Add(column2)
            grid.ColumnDefinitions.Add(column3)
            grid.ColumnDefinitions.Add(column4)

        let row1 = RowDefinition(Height = GridLength(1., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        
        do  grid.RowDefinitions.Add(row1)
            grid.RowDefinitions.Add(row2)
            
            grid.SetValue(Grid.RowProperty,3)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function3D_Sphere_Button) |> ignore
            grid.Children.Add(function3D_Cone_Button) |> ignore
            grid.Children.Add(function3D_Torus_Button) |> ignore
        grid    
    let function3D_SolidMesh_Button = 
        let b = FunctionButton(Content="Solid Mesh", TabIndex = 3)
        do b.SetValue(Grid.ColumnProperty,1)
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
            
            grid.SetValue(Grid.RowProperty,4)
            grid.SetValue(Grid.ColumnSpanProperty,2)

        do  grid.Children.Add(function3D_SolidMesh_Button) |> ignore            
        grid
    
    do  // Assemble the pieces
        function3D_Grid .Children.Add(function3D_fxLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fx_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_fyLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fy_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_fzLabel_TextBlock) |> ignore
        function3D_Grid .Children.Add(function3D_fz_TextBox) |> ignore
        function3D_Grid .Children.Add(function3D_ShapeButton_Grid) |> ignore
        function3D_Grid .Children.Add(function3D_SolidMeshButton_Grid) |> ignore

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
    let option3D_uGridLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "u Grid = ")
        do tb.SetValue(Grid.RowProperty,2)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_uGrid_TextBox = 
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
    let option3D_vGridLabel_TextBlock = 
        let tb = FunctionTextBlock(Text = "v Grid = ")
        do tb.SetValue(Grid.RowProperty,5)
        do tb.SetValue(Grid.ColumnProperty,0)
        tb
    let option3D_vGrid_TextBox = 
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
        do  grid.SetValue(Grid.RowProperty,6)

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
    
    do  // Assemble the pieces
        option3D_Grid .Children.Add(option3D_uMinLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uMin_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_uMaxLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uMax_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_uGridLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_uGrid_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_vMinLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vMin_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_vMaxLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vMax_TextBox) |> ignore        
        option3D_Grid .Children.Add(option3D_vGridLabel_TextBlock) |> ignore
        option3D_Grid .Children.Add(option3D_vGrid_TextBox) |> ignore
        option3D_Grid .Children.Add(option3D_Button_Grid) |> ignore

    //-----Immediate Text Box
    let immediate =
        let im = 
            TextBox(            
                Margin = Thickness(left = 5., top = 5., right = 5., bottom = 0.),
                MaxLines = 1,
                Background = screenColor
                )
        do  im.SetValue(Grid.ColumnSpanProperty,3)
            im.SetValue(Grid.RowProperty,2)
            im.SetValue(TextBlock.TextAlignmentProperty,TextAlignment.Right)
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
            Margin = Thickness(left = 7., top = 5., right = 2., bottom = 5.),
            Height = Double.NaN
            )
    let t_Button = 
        FuncButton(
            Content = "t",
            Margin = Thickness(left = 7., top = 5., right = 2., bottom = 5.),
            Height = Double.NaN
            )
    let u_Button = 
        FuncButton(
            Content = "u",
            Margin = Thickness(left = 7., top = 5., right = 2., bottom = 5.),
            Height = Double.NaN
            )
    let v_Button = 
        FuncButton(
            Content = "v",
            Margin = Thickness(left = 7., top = 5., right = 2., bottom = 5.),
            Height = Double.NaN
            )
    let dx_Button = 
        FuncButton(
            Content = "dX",
            Margin = Thickness(left = 7., top = 5., right = 2., bottom = 5.),
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

    do // Place buttons in a grid
        List.iter ( fun x -> calcButton_Grid.Children.Add(x) |> ignore) calcButtons         
        List.iter ( fun x -> rpnButton_Grid.Children.Add(x) |> ignore) rpnButtons
        List.iter ( fun x -> memoryButton_Grid.Children.Add(x) |> ignore) memoryButtons
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
        let pen1, pen2 = Pen(color1, 0.8), Pen(color2, 0.5)
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
        let w = floor(function_Grid.ActualWidth / 2.)
        let bias = w % resolution xInterval
        let w' = w - bias 
        (x + w')
    let mapYToCanvas y =         
        let h = ceil(function_Grid.ActualHeight / 2.)
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
          InputMode.Graph memoryButton_Grid ]
    
//  ----- Getters       
    // a function that gets active model
    let getActivetModel (s:State) = 
        let convertPoint = fun point ->            
            match point with
            | (Point(X (Math.Pure.Quantity.Real x),
                     Y (Math.Pure.Quantity.Real y))) -> Pt ( System.Windows.Point(x |> mapXToCanvas,y |> mapYToCanvas))
            | (Point3D(X (Math.Pure.Quantity.Real x),
                       Y (Math.Pure.Quantity.Real y),
                       Z (Math.Pure.Quantity.Real z))) -> Pt3D ( System.Windows.Media.Media3D.Point3D(x,y,z) )
            | _ -> failwith "Not a point."
        let convertSegment = fun segment -> 
            match segment with 
            | LineSegment(Point(X (Math.Pure.Quantity.Real x),
                                Y (Math.Pure.Quantity.Real y))) -> System.Windows.Media.LineSegment( System.Windows.Point(x |> mapXToCanvas, y |> mapYToCanvas),true )
            | _ -> System.Windows.Media.LineSegment( System.Windows.Point(0.,0.),true )        
        let model = match s.model with | Trace t -> t
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
    
// ----- Setters
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
            | View.Text t -> 
                t.Visibility <- Visibility.Collapsed
                canvas_DockPanel.Visibility <- Visibility.Collapsed) viewList
        match display with
        | View.PlotCanvas p ->
            p.Visibility <- Visibility.Visible
            canvas_DockPanel.Visibility <- Visibility.Visible
        | View.Function p
        | View.Function2D p
        | View.Function3D p
        | View.Option p
        | View.Option2D p
        | View.Option3D p -> (p.Visibility <- Visibility.Visible)
        | View.Text t -> (t.Visibility <- Visibility.Visible)        
    // a function that sets the displayed text
    let setDisplayedText =         
        fun text ->
            let mode = state.mode
            match mode with
            | RPN 
            | Conventional -> screen_Text_TextBox.Text <- text 
            | Graph -> function_y_TextBox.Text <- text
    // a function that sets the pending op text
    let setMemoText = 
        fun text -> memo.Text <- text
    // a function that sets the pending op text
    let setPendingOpText = 
        fun text -> immediate.Text <- text 
    // a function that sets the active mode buttons
    let setActiveModeButtons mode =
        do  List.iter (fun x -> 
            match x with
            | InputMode.Conventional c -> (c.Visibility <- Visibility.Collapsed)
            | InputMode.RPN r -> (r.Visibility <- Visibility.Collapsed)   
            | InputMode.Graph g -> (g.Visibility <- Visibility.Collapsed)) modeButtonList
        match mode with
        | Mode.Conventional -> (memoryButton_Grid.Visibility <- Visibility.Visible)
        | Mode.RPN -> (rpnButton_Grid.Visibility <- Visibility.Visible)
        | Mode.Graph -> (memoryButton_Grid.Visibility <- Visibility.Visible)
    // a function that sets the input mode
    let setInputMode mode = 
        do state <- {state with mode = mode}
        match mode with 
        | Conventional ->
            setActiveModeButtons mode
            setActiveDisplay (View.Text screen_Text_TextBox)
            setDisplayedText "Conventional" 
        | RPN ->
            setActiveModeButtons mode
            setActiveDisplay (View.Text screen_Text_TextBox)
            setDisplayedText (RpnServices.getDisplayFromStack state.rpn)
        | Graph ->
            setActiveModeButtons mode
            setActiveDisplay (View.Function function_Grid)
            setDisplayedText (GraphServices.getDisplayFromGraphState state.graph)
    // a function that sets the active model
    let setActivetModel model =        
        do state <- {state with model = model}

//-----Create Menu
    let menu = // 
         let header1 = MenuItem(Header = "Mode")
         let header1_Item1 = MenuItem(Header = "Calculator")//, Command = viewCommand (setActiveDisplay), CommandParameter = Text screen_Text_TextBox)
         let header1_Item1_1 = MenuItem(Header = "RPN Stack", Command = modeCommand (setInputMode), CommandParameter = RPN)
         let header1_Item1_2 = MenuItem(Header = "Conventional", Command = modeCommand (setInputMode), CommandParameter = Conventional, Icon = checkedBox)
         
         let handleCheck (item1:MenuItem) (item2:MenuItem) = 
             do  item1.Icon <- checkedBox                
                 item2.Icon <- None

         let header1_Item2 = MenuItem(Header = "Graph", Command = modeCommand (setInputMode), CommandParameter = Graph)
         let header1_Item3 = MenuItem(Header = "Graph2D", Command = viewCommand (setActiveDisplay), CommandParameter = Function2D function2D_Grid)
         let header1_Item4 = MenuItem(Header = "Graph3D", Command = viewCommand (setActiveDisplay), CommandParameter = Function3D function3D_Grid)
         
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

//////////////////////////////////////////////
    //-------setup calculator logic----------
    let calculatorServices = CalculatorServices.createServices()
    let rpnServices = RpnServices.createServices()
    let graphServices = GraphServices.createGraphServices()
    let calculate = CalculatorImplementation.createCalculate calculatorServices 
    let calculateRpn = RpnImplementation.createCalculate rpnServices
    let calculateGraph = GraphingImplementation.createEvaluate graphServices
    //-------create event handlers----------
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
        let newState =  calculateGraph (input,state.graph)        
        let expressionText = 
            match newState with
            | DrawState d -> "Graph"
            | EvaluatedState e -> graphServices.getDisplayFromExpression e.evaluatedExpression           
            | ExpressionDigitAccumulatorState e -> graphServices.getDisplayFromExpression e.expression          
            | ExpressionDecimalAccumulatorState e -> graphServices.getDisplayFromExpression e.expression           
            | ExpressionErrorState e -> graphServices.getDisplayFromExpression e.lastExpression          
            | DrawErrorState d -> graphServices.getDisplayFromExpression d.lastExpression
        state <- { state with graph = newState }
        setDisplayedText expressionText          
        match newState with
        | DrawState d -> 
            do  canvas.Children.Clear()
                setActivetModel (Trace d.trace)                    
                canvas.Children.Add(( getActivetModel state )) |> ignore
                canvas.Children.Add(( placeOrginPoint (mapXToCanvas 0.) (mapYToCanvas 0.) )) |> ignore
                setActiveDisplay (PlotCanvas screen_Canvas)
                setPendingOpText (graphServices.getDisplayFromGraphState newState)
                // add grid if checked
                match canvasGridLines_CheckBox.IsChecked.HasValue = true &&
                      canvasGridLines_CheckBox.IsChecked.Value = true with
                | true -> 
                    let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
                    do  canvas.Children.Insert(0,gl) 
                        canvasGridLine_Slider.IsEnabled <- true
                | false -> ()
        | EvaluatedState ev ->
            do setActiveDisplay (Function function_Grid)
        | _ ->  setPendingOpText (graphServices.getDisplayFromGraphState newState)
    let handleGridLinesOnCheck () =
        let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
        do  canvas.Children.Insert(0,gl) 
            canvasGridLine_Slider.IsEnabled <- true
    let handleGridLinesOnUnCheck () =
        removeGridLines ()
        canvasGridLine_Slider.IsEnabled <- false
    let handleSliderValueOnValueChange () =
        removeGridLines ()
        let gl = makeGridLines canvasGridLine_Slider.Value canvasGridLine_Slider.Value
        do  canvas.Children.Insert(0,gl)
            setActiveDisplay (Function function_Grid)
            handleGraphInput Draw
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
            | MemoryRecall -> CalcInput MemoryRecall (**)
        match state.mode with
        | RPN -> handleRpnInput rpnInput
        | Conventional -> handleConventionalInput input
        | Graph -> handleGraphInput graphInput

    let x = (Math.Pure.Objects.Symbol.Variable "x") |> ExpressionInput.Symbol |> ExpressionInput

    do  //add event handler to each button
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
        function_Button  .Click.AddHandler(RoutedEventHandler(fun _ _ -> handleGraphInput(Draw)))
        
        canvasGridLines_CheckBox.Checked.AddHandler  (RoutedEventHandler(fun _ _ -> handleGridLinesOnCheck()))
        canvasGridLines_CheckBox.Unchecked.AddHandler(RoutedEventHandler(fun _ _ -> handleGridLinesOnUnCheck()))
        
        canvasGridLine_Slider.ValueChanged.AddHandler(RoutedPropertyChangedEventHandler(fun _ _ -> handleSliderValueOnValueChange ()))
