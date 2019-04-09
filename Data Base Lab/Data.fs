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
        [ for i in 0..(getAllProviders.Rows.Count - 1) -> 
            string (getAllProviders.Rows.Item i).ItemArray.[2] ]    
    let getAllOldbProviders =
        let reader = System.Data.OleDb.OleDbEnumerator.GetEnumerator(Type.GetTypeFromProgID("MSDAENUM"))
        let ds = new DataSet()
        let dt = new DataTable()
        do dt.Load(reader)
           ds.Tables.Add(dt)
           ds.AcceptChanges()  // MUST Accept Changes
        ds  
    let connectionState (x : DbConnection) =         
        match x.State  with
        | ConnectionState.Open -> "open"
        | ConnectionState.Closed -> "closed"
        | ConnectionState.Connecting -> "connecting"
        | ConnectionState.Executing -> "executing"
        | ConnectionState.Fetching -> "fetching"
        | ConnectionState.Broken -> "broken"
        | _ -> ""
    let testConnection (x : DbConnection) connString = 
        do x.ConnectionString <- connString
        try x.Open(); "good"  with ex ->  "Failed to connect to DB " + x.Database + " with Error " + ex.Message


  
type NonQueryResult = {numberOfRowAffected:int;connection:DbConnection}
type Provider(input) = 
    let providerString = 
        match List.contains input DataCommon.allProviderStrings with
        | true -> Some (input)
        | false -> None
    member this.option = providerString


module Connection =        
    let dBConnection (providerName : Provider) =
        match providerName.option with
        | Some provider -> DbProviderFactories.GetFactory(provider).CreateConnection()
        | None -> DbProviderFactories.GetFactory(DataCommon.allProviderStrings.Head).CreateConnection() 
          //\ This assumes at least one provider exists on the machine, otherwise it will raise an error. But, then what's the point?
            
            

 module DataQuery =   

    let executeNonQuery (connection : DbConnection) sql =        
        connection.Open()
        let cmd = connection.CreateCommand()
        do  cmd.Connection <- connection
        do  cmd.CommandText <- sql
        do  cmd.CommandType <- CommandType.Text
        let numberOfRowAffected = cmd.ExecuteNonQuery()
        do  connection.Close() 
        {numberOfRowAffected = numberOfRowAffected; connection = connection}
                    


    
//*)
//(*
//internal - Used to specify that a member is visible inside an assembly but not outside it.
type internal DataCommon() =
    
    let mutable connDB : DbConnection = null 
    let mutable connectionString : string = ""
    let mutable providerName : string = ""
    let mutable errorString : string = ""
    let mutable sbSql : StringBuilder = null
    let mutable numberOfRowAffected : int = -1 

    // Close private DB connection "connDB"
    
    let closeConnection() = match connDB with
                            | null -> ignore()
                            | x    -> do x.Close()
                                      do connDB <- null
    
    // Open private connection 
    
    let openDBConnection() =
        do errorString <- ""
        if connectionString <> "" && providerName <> ""  then
            try
                let factory = DbProviderFactories.GetFactory(providerName)
                do connDB <- factory.CreateConnection()
                do connDB.ConnectionString <- connectionString 
                do connDB.Open()
                true
            with
                | ex -> errorString <- ex.Message
                        false
        else errorString <- "Connection String AND/OR Provider Name are blank. "
             false

    // Execute None Query - see  numberOfRowAffected
                             
    let executeNonQuery() = 
        do errorString <- ""
        if openDBConnection() then
            try
                let mutable cmd = connDB.CreateCommand()
                do cmd.Connection <- connDB
                do cmd.CommandText <- sbSql.ToString()
                do cmd.CommandType <- CommandType.Text
                do numberOfRowAffected <- cmd.ExecuteNonQuery()
                do closeConnection() 
                true 
            with 
                | ex -> do errorString <- ex.Message
                        do closeConnection() 
                        false
        else  do numberOfRowAffected <- -1
              false

    // Test DB Connection 

    let testConnection() =  if openDBConnection() then
                               closeConnection()
                               true
                            else false 
    // Get Data Set

    let getDataSet() = 
        if openDBConnection() then
            try 
                let factory = DbProviderFactories.GetFactory(providerName)
                let mutable ds: DataSet = new DataSet() 
                let mutable dbAdapter = factory.CreateDataAdapter()
                do dbAdapter.FillLoadOption <- LoadOption.PreserveChanges 
                 
                let cmd = connDB.CreateCommand()
                do cmd.CommandText <- sbSql.ToString()
                do cmd.CommandType <- CommandType.Text // Procedure -> CALL procName (@param1,...,@paramN)
                
                do dbAdapter.SelectCommand <- cmd
                do numberOfRowAffected <- dbAdapter.Fill(ds)

                do closeConnection()
                ds
            with 
                | ex -> do errorString <- ex.Message
                        do closeConnection() 
                        null
        else  do numberOfRowAffected <- -1
              null
              
    // Get All Providers installed on your PC
                                             
    let getAllProviders () = DbProviderFactories.GetFactoryClasses()

    // Get All OLE DB providers

    let getAllOldbProviders () =
                let mutable ds = new DataSet()
                do errorString <- ""
                let reader : System.Data.OleDb.OleDbDataReader  = System.Data.OleDb.OleDbEnumerator.GetEnumerator(Type.GetTypeFromProgID("MSDAENUM"))
                let mutable dt : DataTable = new DataTable()
                do dt.Load(reader)
                do dt.TableName <- "MSDAENUM"
                do ds.Tables.Add(dt)
                do ds.AcceptChanges()  // MUST Accept Changes
                ds
//*)

                                                                                                     
    // MEMBERS

    member x.ConnectionString  with get() = connectionString  and set(v) = connectionString <- v
    member x.ProviderName with get() = providerName and set(v) = providerName <-v

    member x.SbSql  with get() = sbSql  and set(v) = sbSql <- v 

    member x.CloseConnection() = closeConnection()   // just for interrupt execution

    member x.TestConnection() = testConnection()
    member x.TestConnection(currentProviderName, currentConnectionString) = 
                            if isNull currentProviderName then providerName <- ""
                                else providerName <- currentProviderName.ToString()
                            do connectionString <- currentConnectionString
                            testConnection()

    member x.ExecuteNonQuery(currentProviderName, currentConnectionString, currentSbSQL) = 
                            do providerName <- currentProviderName 
                            do connectionString <- currentConnectionString
                            do sbSql <- currentSbSQL
                            executeNonQuery()
    
    member x.ExecuteNonQuery(currentSbSQL) = 
                            do sbSql <- currentSbSQL
                            executeNonQuery()
    
    member x.ExecuteNonQuery() = executeNonQuery() 


    member x.GetDataSet(currentProviderName, currentConnectionString, currentSbSQL) = 
                            if isNull currentProviderName then providerName <- ""
                                                          else providerName <- currentProviderName.ToString()
                            do connectionString <- currentConnectionString
                            do sbSql <- currentSbSQL
                            getDataSet()
    
    member x.GetDataSet(currentSbSQL) = do sbSql <- currentSbSQL
                                        getDataSet()

    member x.GetDataSet() = getDataSet()


    member x.IntNumberOfRow  with get() = numberOfRowAffected
    member x.StrError with get() = errorString and set(v) = errorString <- v

    member x.GetAllProviders()  = getAllProviders()
    member x.GetAllOldbProviders() = getAllOldbProviders()
//*)
type ShowData() as this =
    inherit Window()

    let mySr = new StreamReader(Assembly.Load("FsharpObjXAMLLibrary").GetManifestResourceStream("ShowData.xaml"))   // XAML - MUST be Embedded Resource 
    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML
    do this.Title <- "Data Grid - Show"

    do this.Width <- 400.0
    do this.Height <- 300.0
    
    let mutable gridResult : Grid = this.Content?gridResult   // Find txtHello name in runWindow and Cast to TextBlock
    
    
    
    let mutable scaleXY : ScaleTransform = this.Content?scaleXY
    
    let initGrid(ds: DataSet) =    let tabMain = new TabControl()
                                   do gridResult.Children.Add(tabMain) |> ignore

                                   for dt in ds.Tables do
                                      let mutable ti = new TabItem()
                                      do ti.Header <- dt.TableName
                                      let mutable dg = new DataGrid()
                                      do dg.AutoGenerateColumns <- true
                                      do dg.ItemsSource <- dt.DefaultView
                                      do ti.Content <- dg
                                      do tabMain.Items.Add(ti) |> ignore

    let initGridError(strErr: String) =
                                    let tabMain = new TabControl()
                                    do gridResult.Children.Add(tabMain) |> ignore
                                    let mutable ti = new TabItem()
                                    do ti.Header <- "ERROR(S)"
                                    let mutable tb = new TextBox()
                                    do tb.Text <- strErr
                                    do tb.TextWrapping <- TextWrapping.Wrap
                                    do ti.Content <- tb
                                    do tabMain.Items.Add(ti) |> ignore
  
    let changedScale(v) = do scaleXY.ScaleX <- v / 100.0
                          do scaleXY.ScaleY <- v / 100.0
    
    member x.InitGrid(ds : DataSet) =  initGrid(ds)
    member x.InitGridError(strError : String) =  initGridError(strError)
    member x.ChangedScale(v) = changedScale(v)

type DataLab() as this =
    inherit UserControl()

 // XAML file properties -> "EmbeddedResource"

    let mySr = new StreamReader(Assembly.Load("FsharpObjXAMLLibrary").GetManifestResourceStream("DataCommonFsharp.xaml"))   // XAML - MUST be Embedded Resource 
    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML

// OR  Change XAML file properties to "Resource" and uncomment below (comment out above)

// // XAML file properties -> Resource 
//    let resource = new Uri("/FsharpObjXAMLLibrary;component/DataCommonFsharp.xaml",System.UriKind.Relative)
//    do  <- Application.LoadComponent(resource) :?> UserControl   // Cast to UserControl 

    let mutable txtSQL : TextBox = this.Content?txtSQL
    let mutable txtConnectionString : TextBox = this.Content?txtConnectionString
    let mutable comboProvider : ComboBox = this.Content?comboProvider
    let mutable btnOleDbHelper : Button = this.Content?btnOleDbHelper
    let mutable btnRUN : Button = this.Content?btnRUN
    let mutable chkSpell : CheckBox = this.Content?chkSpell
    let mutable btnTest : Button = this.Content?btnTest
    
    let mutable scaleXY : ScaleTransform = this.Content?scaleXY
    let mutable slider : Slider = this.Content?slider
    
    let mutable sh : ShowData = new ShowData() 

    let dc = new DataCommon() 

    do btnOleDbHelper.ToolTip <- "Show OleDb data providers which installed on YOUR PC."
    do comboProvider.ToolTip <- "Show ALL data providers which installed on YOUR PC."

    // Init combo box providers
     
    do comboProvider.DisplayMemberPath <- "Name"
    do comboProvider.SelectedValuePath <- "InvariantName"
    do comboProvider.ItemsSource <- (dc.GetAllProviders()).DefaultView;

    let runDS() = dc.GetDataSet(comboProvider.SelectedValue, txtConnectionString.Text, new StringBuilder(txtSQL.Text) )

    let showData(ds:DataSet) = async {
                              do sh <- new ShowData()  
                              do sh.ChangedScale(slider.Value)
                              do sh.Owner <- Application.Current.MainWindow  // window be on top of all other windows 
                              do sh.Show() 
                              if dc.StrError = "" then do sh.InitGrid(ds)
                                                  else do sh.InitGridError(dc.StrError) 
                            }

    let comboSelected() = let s = comboProvider.SelectedValue.ToString()
                          if s.IndexOf("OleDb") >= 0 then do btnOleDbHelper.IsEnabled <- true
                                                     else do btnOleDbHelper.IsEnabled <- false

    let testConnection() = async {
                                   do sh <- new ShowData()
                                   do sh.ChangedScale(slider.Value)
                                   do sh.Owner <- Application.Current.MainWindow  // window be on top of all other windows 
                                   do sh.Show() 
                                   if dc.TestConnection(comboProvider.SelectedValue, txtConnectionString.Text) 
                                       then do sh.InitGridError("Connection String - OK")
                                       else do sh.InitGridError(dc.StrError)
                                   ignore()
                             }
    let changedScale() = do scaleXY.ScaleX <- slider.Value / 100.0
                         do scaleXY.ScaleY <- slider.Value / 100.0
                         do sh.ChangedScale(slider.Value)

    do slider.ValueChanged.Add(fun _ -> changedScale()) 

    do this.Unloaded.Add(fun _ -> dc.CloseConnection())

    do comboProvider.SelectionChanged.Add(fun _ ->  comboSelected())

    do chkSpell.Checked.Add(fun _ -> do txtSQL.SpellCheck.IsEnabled <- true )
    do chkSpell.Unchecked.Add(fun _ -> do txtSQL.SpellCheck.IsEnabled <- false )

    do btnTest.Click.Add(fun _ ->  Async.StartImmediate(testConnection())) 
    do btnRUN.Click.Add(fun _ ->  Async.StartImmediate(showData(runDS()))) 
    do btnOleDbHelper.Click.Add(fun _ -> Async.StartImmediate(showData(dc.GetAllOldbProviders())))
