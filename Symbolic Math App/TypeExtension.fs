module TypeExtension

open System
open System.Windows          
open System.Windows.Controls 

type StackPanel with
  /// Helper function to compose a GUI
  member sp.add x = sp.Children.Add x |> ignore
