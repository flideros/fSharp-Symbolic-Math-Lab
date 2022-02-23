namespace Math.Presentation.WolframEngine.Analysis

open System
open System.Numerics
open System.Windows
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media
open System.Windows.Media.Imaging
open Wolfram.NETLink
open GraphicResources
open GenericDomain
open ControlDomain

(*
This is a placeholder. 
I'm still planning and designing the application.
*)
type Analysis() as this =  
    inherit UserControl()    
    do Install() |> ignore
    
    let link = Wolfram.NETLink.MathLinkFactory.CreateKernelLink("-WSTP -linkname \"D:/Program Files/Wolfram Research/Wolfram Engine/12.3/WolframKernel.exe\"")
    do  link.WaitAndDiscardAnswer()
    let kernel = 
        let k = new Wolfram.NETLink.MathKernel(link)
        do  k.AutoCloseLink <- true
            k.CaptureGraphics <- true
            k.CaptureMessages <- true
            k.CapturePrint <- true
            k.GraphicsFormat <- "Automatic"
            //k.GraphicsHeight <- 700
            k.GraphicsResolution <- 100
            //k.GraphicsWidth <- 0
            k.HandleEvents <- true
            k.Input <- null
            k.LinkArguments <- null
            k.PageWidth <- 200
            k.ResultFormat <- Wolfram.NETLink.MathKernel.ResultFormatType.OutputForm
            k.UseFrontEnd <- true
        k

    (*Shared Values*)
    let wolframCode = SharedValue<string> "Ready"
    let wolframMessage = SharedValue<string> "Ready"
    let wolframResult = SharedValue<string> "Ready"
    let wolframLink = SharedValue<IKernelLink> link
    let wolframSettings = SharedValue<WolframResultControlSettings> {codeVisible = false; resultVisible = false; isHitTestVisible = true; isVisible = false}
    
    // Internal State
    let initialState = AnalysisDomain.Truss (AnalysisDomain.TrussAnalysisDomain.TrussState {truss = {members=[]; forces=[]; supports=[]}; mode = ControlDomain.MemberBuild})
    
    let mutable state = initialState

    (*Controls*) 
        // General purpose text for code output such as displaying ontrol state
    let label =
        let l = TextBox()
        do  l.Margin <- Thickness(Left = 1080., Top = 50., Right = 0., Bottom = 0.)
            l.FontStyle <- FontStyles.Normal
            l.FontSize <- 15.
            l.MaxWidth <- 500.
            l.TextWrapping <- TextWrapping.Wrap
            l.Text <- state.ToString()
            l.BorderBrush <- SolidColorBrush(Colors.Transparent)
            l.Opacity <- 0.5
            l.MaxLines <- 30
        l  
    
    // Wolfram Result    
    let wolframResult_Control = 
        let sb = WolframResultControl(wolframCode,wolframMessage,wolframResult,wolframLink,wolframSettings)
        do  sb.Margin <- Thickness(Left = 0., Top = 0., Right = 0., Bottom = 0.)
            sb.Visibility <- Visibility.Collapsed
            sb.SetValue(Canvas.ZIndexProperty,3) 
            sb.SetValue(Grid.ColumnProperty,3)
            sb.SetValue(Grid.HorizontalAlignmentProperty,HorizontalAlignment.Left)
        sb
            
    let truss = 
        let t = Truss(kernel,wolframResult_Control.setGraphics,wolframCode,wolframMessage,wolframResult,wolframSettings)
        do  t.SetValue(Grid.ColumnProperty,1)
            t.SetValue(Grid.ColumnSpanProperty,3)
        t

    let screen_Grid =
        let g = Grid()              
        let gs = GridSplitter()

        let column0 = ColumnDefinition(Width = GridLength.Auto)
        let column1 = ColumnDefinition()//Width = GridLength(1., GridUnitType.Star)
        let splitter2 = ColumnDefinition(Width = GridLength(1., GridUnitType.Pixel))
        let column3 = ColumnDefinition(Width = GridLength.Auto)
        
        do  g.ColumnDefinitions.Add(column0)
            g.ColumnDefinitions.Add(column1)
            g.ColumnDefinitions.Add(splitter2)
            g.ColumnDefinitions.Add(column3)

        let row1 = RowDefinition(Height = GridLength(3., GridUnitType.Star))
        let row2 = RowDefinition(Height = GridLength.Auto)
        let row3 = RowDefinition(Height = GridLength.Auto)
        
        do  g.RowDefinitions.Add(row1)
            g.RowDefinitions.Add(row2)
            g.RowDefinitions.Add(row3)
            g.Children.Add(truss) |> ignore
            g.Children.Add(wolframResult_Control) |> ignore
        
            gs.SetValue(Grid.ColumnProperty,2)
            gs.HorizontalAlignment <- HorizontalAlignment.Right
            gs.VerticalAlignment <- VerticalAlignment.Stretch
            gs.Background <- black
            gs.ShowsPreview <- true
            gs.Width <- 0.

            g.Children.Add(gs) |> ignore
        g    

    (*Initialize*)
    do  this.Content <- screen_Grid        
        wolframResult_Control.setGraphics kernel 
        
        this.Unloaded.AddHandler(RoutedEventHandler(fun _ _ -> kernel.Dispose()))