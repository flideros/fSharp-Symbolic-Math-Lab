// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module App  
   
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
open Style
open Control

// Create Types
let browser = Browser("https://www.bing.com/")
let browserBorder = BorderStyle();
let comboBox = ComboBox()
let listView = ListView()
let dockPanel = DockPanelStyle()

// Compose Types
browserBorder.Child <- browser

listView.Items.Add("aaaa") |> ignore
listView.Items.Add("bbbb") |> ignore

comboBox.Items.Add (listView) |> ignore

dockPanel.Children.Add (comboBox) |> ignore
dockPanel.Children.Add (browserBorder) |> ignore


// Make a window and add content
let window = Window()
window.Title <- "F# is fun!"
window.Content <- dockPanel //browserBorder
//window.Show()

  

[<STAThread>] 
[<EntryPoint>]
do (new Application()).Run(window) |> ignore

