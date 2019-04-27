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
//open GraphingCalculatorDomain

type GraphingCalculator() as graphingCalculator =
    inherit UserControl()


// ------Create Types---------        
    let calculator_Grid = Grid()    
    let calculator_Rectangle = 
        Rectangle(
            StrokeThickness = 2., 
            Stroke = black, 
            Fill = radialGradientBrush, 
            RadiusX = 10., 
            RadiusY = 10.
            )
    let calculator_modelNumber_TextBlock = 
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

        let row1 = RowDefinition(Height = GridLength(3.))
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
        calculator_Grid.Children.Add(calculator_modelNumber_TextBlock) |> ignore
    
    let screen_Border = 
        let gb = Border(
                    Margin = Thickness(left = 5., top = 5., right = 5., bottom = 5.),
                    BorderBrush = black,
                    BorderThickness = Thickness(1.5)
                    )        
        do gb.SetValue(Grid.RowSpanProperty,5)
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
                    Visibility = Visibility.Hidden,
                    IsReadOnly = true,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                    )
        do t.SetValue(Grid.RowProperty, 1)
        t
    let canvas = Canvas( Visibility = Visibility.Visible, 
                         ClipToBounds = true)
    let screen_Canvas =
        let g = Grid()
        do g.SetValue(Grid.RowProperty, 1)        
        do g.Children.Add(canvas) |> ignore
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
        //screen_Text_TextBox.Text <- "Testing one two, testing."         
        screen_Grid.Children.Add(screen_Text_TextBox) |> ignore
        screen_Grid.Children.Add(screen_Canvas)       |> ignore
        screen_Grid.Children.Add(selection_Rectangle) |> ignore
  
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
        let tb = FunctionTextBox(MaxLines = 1, TabIndex = 0)
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

    let option3D_Grid =     
        let grid = Grid(Visibility = Visibility.Visible)        
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

    let menu = 
        
        let itemsControlTemplate = ItemsPanelTemplate()

        let header1 = MenuItem(Header = "Graph")
        let header1_Item1 = MenuItem(Header = "Text")
        let header1_Item2 = MenuItem(Header = "Graph")
        let header1_Item3 = MenuItem(Header = "Graph2D")
        let header1_Item4 = MenuItem(Header = "Graph3D")

        do  header1.Items.Add(header1_Item1) |> ignore
            header1.Items.Add(header1_Item2) |> ignore
            header1.Items.Add(header1_Item3) |> ignore
            header1.Items.Add(header1_Item4) |> ignore

        let header2 = MenuItem(Header = "Options")
        let header2_Item1 = MenuItem(Header = "Graph")
        let header2_Item2 = MenuItem(Header = "Graph2D")
        let header2_Item3 = MenuItem(Header = "Graph3D")
        
        do  header2.Items.Add(header2_Item1) |> ignore
            header2.Items.Add(header2_Item2) |> ignore
            header2.Items.Add(header2_Item3) |> ignore

        let m = Menu()
        do  m.SetValue(Grid.RowProperty,0)
            m.Items.Add(header1) |> ignore
            m.Items.Add(header2) |> ignore
            
        m

    let immediate =
        let im = TextBox()

        im


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
        calculator_Grid.Children.Add(calculator_Layout_Grid) |> ignore
        
        
        graphingCalculator.Content <- calculator_Grid














(*
screen_Grid.MouseLeftButtonDown="OnCanvasClickStart"
screen_Grid.MouseLeftButtonUp="OnCanvasClickFinish" MouseMove="OnCanvasMouseMove"
screen_Grid.MouseRightButtonDown="OnCanvasRightClick"> 
*)
