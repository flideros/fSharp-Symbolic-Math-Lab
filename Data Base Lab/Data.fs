namespace DataLab

open System.Data
open System.Data.Common
open System.Text
open System
open System.IO
open System.Windows.Markup
open System.Reflection
open System.Windows
open System.Windows.Controls
open System.Windows.Media
open Utilities

//(*
module DataCommon = 
    let getAllProviders = DbProviderFactories.GetFactoryClasses()
    let allProviderStrings =         
        List.rev[ for i in 0..(getAllProviders.Rows.Count - 1) 
            -> string (getAllProviders.Rows.Item i).ItemArray.[2] ]    
    let getAllOldbProviders =
        let reader = System.Data.OleDb.OleDbEnumerator.GetEnumerator(Type.GetTypeFromProgID("MSDAENUM"))
        let ds = new DataSet()
        let dt = new DataTable()
        do dt.Load(reader)
           ds.Tables.Add(dt)
           ds.AcceptChanges()  // MUST Accept Changes
        ds

type NonQueryResult = {numberOfRowAffected:int;error:string}

type QueryResult = {data:DataSet; numberOfRowAffected:int;error:string}

type Provider(input) = 
    let providerString = 
        match List.contains input DataCommon.allProviderStrings with
        | true -> Some (input)
        | false -> None
    member this.option = providerString

module Connection =        
    let toDatabase (providerName : Provider) =
        match providerName.option with
        | Some provider -> DbProviderFactories.GetFactory(provider).CreateConnection()
        | None -> DbProviderFactories.GetFactory(DataCommon.allProviderStrings.Head).CreateConnection() 
          //\ This assumes at least one provider exists on the machine, otherwise it will raise an error. But, then what's the point?
    let connectionState (x : DbConnection) =         
        match x.State  with
        | ConnectionState.Open -> "open"
        | ConnectionState.Closed -> "closed"
        | ConnectionState.Connecting -> "connecting"
        | ConnectionState.Executing -> "executing"
        | ConnectionState.Fetching -> "fetching"
        | ConnectionState.Broken -> "broken"
        | _ -> ""
    let test (x : DbConnection) connString =         
        try do x.ConnectionString <- connString
            x.Open(); x.Close();"good"  with ex ->  "Failed to connect to DB " + x.Database + " with Error " + ex.Message 

module DataQuery =  
    let executeNonQuery (connection : DbConnection) sql =        
        async { 
                connection.Open()
                try 
                    let cmd = connection.CreateCommand()
                    do  cmd.Connection <- connection
                    do  cmd.CommandText <- sql
                    do  cmd.CommandType <- CommandType.Text
                    let numberOfRowAffected = cmd.ExecuteNonQuery()
                    do  connection.Close()
                    {numberOfRowAffected = numberOfRowAffected;error="Done."} |> ignore
                with 
                    | ex -> 
                        do  connection.Close()
                        {numberOfRowAffected = -1;error = ex.Message} |> ignore
               } 

    // Get Data Set
    let getDataSet (p:Provider) (connection : DbConnection) sql  = 
        let factory = DbProviderFactories.GetFactory(p.option.Value)
        let ds: DataSet = new DataSet() 
        let dbAdapter = factory.CreateDataAdapter()
        let cmd = connection.CreateCommand()
        match Connection.test connection connection.ConnectionString with
        | "good" -> 
            try                
                do  dbAdapter.FillLoadOption <- LoadOption.PreserveChanges
                    cmd.CommandText <- sql.ToString()
                    cmd.CommandType <- CommandType.Text // Procedure -> CALL procName (@param1,...,@paramN)                   
                    dbAdapter.SelectCommand <- cmd
                let rows = dbAdapter.Fill(ds)  
                do  connection.Close()
                {data = ds; numberOfRowAffected = rows;error=""}
            with 
                | ex ->                    
                    connection.Close()
                    {data = ds; numberOfRowAffected = -1; error=ex.Message}
        | e -> {data = ds; numberOfRowAffected = -1; error = e}
 
type ShowData() as this =
    inherit Window()
    
    do this.Title <- "Data Grid - Show"
       this.Width <- 400.0
       this.Height <- 300.0

    let dg = new DataGrid()
    let ti = new TabItem()
    let tb = new TextBox()
    let tabMain = new TabControl()
    let mutable gridResult : Grid = Grid()       
    
    let initGrid(ds: DataSet) =    
        do  gridResult.Children.Add(tabMain) |> ignore
        for dt in ds.Tables do                                      
            ti.Header <- dt.TableName                                      
            dg.AutoGenerateColumns <- true
            dg.ItemsSource <- dt.DefaultView
            ti.Content <- dg
            tabMain.Items.Add(ti) |> ignore
            this.Content <- gridResult

    let initGridError(strErr: String) =                                    
        do  gridResult.Children.Add(tabMain) |> ignore                                    
            ti.Header <- "ERROR(S)"                                    
            tb.Text <- strErr
            tb.TextWrapping <- TextWrapping.Wrap
            ti.Content <- tb
            tabMain.Items.Add(ti) |> ignore
            this.Content <- gridResult
    
    member x.InitGrid(ds : DataSet) =  initGrid(ds)
    member x.InitGridError(strError : String) =  initGridError(strError)


type State = { connectionString : string
               provider : Provider
               connection : DbConnection
               error : string }

type DataLab() as this =
    inherit UserControl()

    // Set attributes on this.
    do  this.Name <- "DataLab"
        
    // Create controls.
    let grid_Main = 
        new Grid( Name = "gridMain", 
                  RenderTransformOrigin = Point(0.,0.),
                  MinWidth = 600.,
                  MinHeight = 400.
                )
    let textBox_SQL = 
        new TextBox( Name = "txtSQL",
                     TextWrapping = TextWrapping.Wrap, 
                     Text = "Enter SQL Here.",
                     Margin = Thickness(Left = 10., Top = 63., Right = 10., Bottom = 27.),
                     IsManipulationEnabled = true,
                     IsInactiveSelectionHighlightEnabled = true,
                     AcceptsReturn = true
                    )
    let textBox_ConnectionString = 
        new TextBox( Name = "txtConnectionString",
                     Height = 23., 
                     Margin = Thickness(Left = 10., Top = 8., Right = 136., Bottom = 0.),
                     TextWrapping = TextWrapping.Wrap,
                     Text = "Connection String", 
                     VerticalAlignment = VerticalAlignment.Top                     
                    )
    let comboBox_Provider = 
        new ComboBox( Name="comboProvider",
                      Margin = Thickness(Left = 10., Top = 36., Right = 370., Bottom = 0.), 
                      VerticalAlignment=VerticalAlignment.Top,
                      ToolTip = "Show ALL data providers installed on this PC.",
                      DisplayMemberPath = "Name",
                      SelectedValuePath = "InvariantName",
                      ItemsSource = (DataCommon.getAllProviders).DefaultView,
                      SelectedValue = "System.Data.SqlClient"
                      )
    let button_OleDbHelper = 
        new Button( Name = "btnOleDbHelper",
                    Content = "OleDb Helper",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = Thickness(Left = 0., Top = 36., Right = 10., Bottom = 0.),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 121.,
                    IsCancel = true,
                    IsEnabled = false,
                    ToolTip = "Show OleDb data providers installed on this PC."
                   )
    let button_RUN = 
        new Button( Name = "btnRUN",
                    Content = "RUN SQL",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = Thickness(Left = 0., Top = 36., Right = 136., Bottom = 0.),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 121.,
                    IsCancel = true                    
                   )
    let checkBox_Spell = 
        new CheckBox( Name = "chkSpell", 
                      Content = "Spell Check",
                      HorizontalAlignment = HorizontalAlignment.Right,
                      Margin = Thickness(Left = 0., Top = 39., Right = 271., Bottom = 0.),
                      VerticalAlignment = VerticalAlignment.Top,
                      Width = 94.
                    )
    let button_Test = 
        new Button( Name = "btnTest",
                    Content = "Test",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = Thickness(Left = 0., Top = 10., Right = 10., Bottom = 0.),//
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 121.,
                    IsCancel = true,
                    ToolTip = "Test Connection String."
                   )
     
    // Initialize connection parameters.
    let provider = 
        let pList = DataCommon.allProviderStrings
        Provider(pList.Head)
    let connection =  Connection.toDatabase provider
            
    // Initial state variable
    let mutable state = 
        { connectionString = ""
          provider = provider
          connection = connection
          error = ""
          }

    // Functions
    let showData(ds:DataSet) = async {
        let showData = new ShowData()        
        do showData.Owner <- Application.Current.MainWindow  // window be on top of all other windows         
        do showData.Show()
        match state.error = "" with
        | true -> do showData.InitGrid(ds)
        | false -> do showData.InitGridError(state.error) 
      }
    
    let runSQL() = 
        match textBox_SQL.Text.Contains("SELECT") with 
        | true -> showData( (DataQuery.getDataSet state.provider state.connection textBox_SQL.Text).data ) 
        | false -> DataQuery.executeNonQuery state.connection textBox_SQL.Text
        
    /// Enables OleDbHelper_button when the OleDb provider is selected from combo box and 
    /// disables OleDbHelper_button when another provider is selected. 
    let comboSelected() = 
        let s = comboBox_Provider.SelectedValue.ToString()
        let newState = {state with provider = Provider(s); connection = Connection.toDatabase (Provider(s))}        
        do  state <- newState
            match s.IndexOf("System.Data.OleDb") >= 0 with
            | true -> do button_OleDbHelper.IsEnabled <- true
            | false -> do button_OleDbHelper.IsEnabled <- false   
    
    let testConnection() = async {
        let showData = new ShowData()       
        do  showData.Show()
            showData.Owner <- Application.Current.MainWindow  
        do state <- {state with connectionString = textBox_ConnectionString.Text}
        match Connection.test state.connection state.connectionString with
        | "good" -> do showData.InitGridError("Connection String - OK")
        | error -> do showData.InitGridError(error)
        ignore()}
    
    // Compose controls
    do  grid_Main.Children.Add(textBox_SQL)              |> ignore
        grid_Main.Children.Add(textBox_ConnectionString) |> ignore
        grid_Main.Children.Add(comboBox_Provider)        |> ignore
        grid_Main.Children.Add(button_OleDbHelper)       |> ignore
        grid_Main.Children.Add(button_RUN)               |> ignore
        grid_Main.Children.Add(checkBox_Spell)           |> ignore
        grid_Main.Children.Add(button_Test)              |> ignore        
        this.Content <- grid_Main

    do this.Unloaded.Add(fun _ -> state.connection.Close())    
       comboBox_Provider.SelectionChanged.Add(fun _ ->  comboSelected())    
       checkBox_Spell.Checked.Add(fun _ -> do textBox_SQL.SpellCheck.IsEnabled <- true )
       checkBox_Spell.Unchecked.Add(fun _ -> do textBox_SQL.SpellCheck.IsEnabled <- false )    
       button_Test.Click.Add(fun _ ->  Async.StartImmediate (testConnection())) 
       button_RUN.Click.Add(fun _ ->  Async.StartImmediate (runSQL()) )
       button_OleDbHelper.Click.Add(fun _ -> Async.StartImmediate(showData(DataCommon.getAllOldbProviders)))
