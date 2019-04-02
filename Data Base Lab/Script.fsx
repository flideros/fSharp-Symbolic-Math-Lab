// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System.Data
open System.Data.Common
open System.Text
open System
open System.Text
open System.IO
open System.Windows.Markup


let getAllProviders = DbProviderFactories.GetFactoryClasses()

let getAllOldbProviders =
    let reader = System.Data.OleDb.OleDbEnumerator.GetEnumerator(Type.GetTypeFromProgID("MSDAENUM"))
    let ds = new DataSet()
    let dt = new DataTable()
    do dt.Load(reader)
       ds.Tables.Add(dt)
       ds.AcceptChanges()  // MUST Accept Changes
    ds

let allProviderStrings = [ for i in 0..(getAllProviders.Rows.Count - 1) -> string (getAllProviders.Rows.Item i).ItemArray.[2] ]
    


type Provider(input) = 
     let providerString = 
        match List.contains input allProviderStrings with
        | true -> Some (input)
        | false -> None
     member x.option = providerString

//
let p = Provider("System.Data.Odbc")
p.option
//

let dBConnection' connectionString (provider : Provider) =
        
        
        let this = DbProviderFactories.GetFactory(provider.option.Value).CreateConnection()
        this.ConnectionString <- connectionString
        this

                    
let testConnection (x : DbConnection) =         
        match x.State  with
        | ConnectionState.Open -> "open"
        | ConnectionState.Closed -> "closed"
        | ConnectionState.Connecting -> "connecting"
        | ConnectionState.Executing -> "executing"
        | ConnectionState.Fetching -> "fetching"
        | ConnectionState.Broken -> "broken"
        | _ -> ""

//let db = dBConnection' " " " "