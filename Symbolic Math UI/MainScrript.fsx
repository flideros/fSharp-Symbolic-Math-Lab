#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"

#load "Operator.fs"
#load "TypeExtension.fs"
#load "Container.fs"
//#load "Control.fs"

open System
open System.Windows          
open System.Windows.Controls  
open System.Windows.Media
open TypeExtension
open UI
open Operator
//open Control
open System.Windows.Shapes
//////////////////////////////////////////////////////////////////////////////
type Record = {a:int; b:string}

let data = seq{ for i in 0..10 -> {a = i; b = i.ToString()} } |> Seq.toArray

let dataGrid = DataGrid(ItemsSource = data)

do  dataGrid.AutoGeneratingColumn .AddHandler(fun _ _ -> dataGrid.SetValue(DataGrid.ColumnWidthProperty, DataGridLength(150.) ) )
   
let window = new Window(Title="Manually Populate TreeView",
                        Content = dataGrid)//tree)//)

[<STAThread()>]
do 
    let app =  Application() in
    app.Run(window) |> ignore



(menu.Items.Item(0) :?> MenuItem).Items.Count //<-

///////////////////////////////////////////////////
let command exec =
    let event = Event<_,_>()
    { new System.Windows.Input.ICommand with
        member __.CanExecute(_) = true
        member __.Execute(arg) = exec arg
        [<CLIEvent>]
        member __.CanExecuteChanged = event.Publish
    }

let helloCommand =
    command (fun _ -> MessageBox.Show("Hello") |> ignore)

let menu = 
    
    let header1 = MenuItem(Header = "Graph")
    let header1_Item1 = MenuItem(Header = "Text", Command = helloCommand )
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


(menu.Items.Item(0) :?> MenuItem).Items.Count //<-

((menu.Items.Item(0) :?> MenuItem).Items.Item(0) :?> MenuItem).Header   //<-

menu.Items.CurrentItem :?> MenuItem

//menu.


type Animal = {spices:string;breed:string}





let addSubItems items (branch:TreeViewItem) = items |> Seq.iter (fun item -> 
    item |> branch.Items.Add |> ignore)

let tree = new TreeView()

tree.Items.Add
   (let animalBranch = new TreeViewItem(Header="Animal")

    animalBranch.Items.Add
       (let branch = new TreeViewItem(Header="Dog")
        let dogs = ["Poodle";"Irish Setter";"German Shepherd"]
        addSubItems dogs branch
        branch) |>ignore

    animalBranch.Items.Add
       (let branch = new TreeViewItem(Header="Cat")
        branch.Items.Add(new TreeViewItem(Header="Alley Cat")) |>ignore
        branch.Items.Add(new Button(Content="Noodles")) |>ignore
        branch.Items.Add("Siamese") |>ignore
        branch) |> ignore

    animalBranch.Items.Add
       (let branch = new TreeViewItem(Header="Primate")
        let primates = ["Chimpanzee";"Bonobo";"Human"]
        addSubItems primates branch
        branch) |> ignore
    animalBranch) |> ignore
      
tree.Items.Add
   (let branch = new TreeViewItem(Header="Mineral")
    let minerals = ["Calcium";"Zinc";"Iron"]
    addSubItems minerals branch
    branch) |> ignore

tree.Items.Add
   (let branch = new TreeViewItem(Header="Vegetable")
    let vegetables = ["Carrot";"Asparagus";"Broccoli"]
    addSubItems vegetables branch
    branch) |> ignore

let template = new HierarchicalDataTemplate(typeof<Animal>)

let oo = tree.Items.[0] :?> System.Windows.Controls.TreeViewItem

oo.Header




