// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

#r "./bin/Debug/Data_Base_Lab.dll"

open System.Data
open System.Data.Common
open System.Text
open System
open System.Text
open System.IO
open System.Windows.Markup
open DataLab.DataCommon
open DataLab.DataQuery
open DataLab.Connection
open DataLab


(**)

let p = Provider("System.Data.SqlClient")
DataCommon.getAllProviders.Rows
DataCommon.allProviderStrings

let connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MyFolders\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Data Base Lab\LABdb.mdf;Integrated Security=True;Connect Timeout=30"
let d = Connection.toDatabase  p

Connection.test d connString
d.State.ToString()
d.Close()
try d.Open() finally ()

type Record = {a:int; b:string}

let data = seq{for i in 0..10 -> {a = i; b = i.ToString()} } |> Seq.toArray

