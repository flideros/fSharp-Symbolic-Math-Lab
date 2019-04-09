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

//let db = dBConnection' " " " "
[<Literal>]
let connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + "D:\MyFolderks\MyDocuments\Visual Studio 2017\Projects\Symbolic Math\Data Base Lab\LABdb.mdf" + ";Integrated Security=True;Connect Timeout=30"
let d = (dBConnection p)
d.Close()
try d.Open() finally ()
testConnection d connString

