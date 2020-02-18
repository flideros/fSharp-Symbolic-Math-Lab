#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#r @"System.Windows"       
//#r @"D:\MyFolders\Desktop\SymbolicMath\Math Display\bin\Debug\Math_Display.dll"
#load "MathML.fs"
#load "TypeSetting.fs"
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

let fontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Bold"), "./#STIX Two Text Bold")
let tf = Typeface(fontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())

let tf2= Typeface("Times New Roman")

let formattedText = 
    FormattedText(
        textToFormat = "Liderosp",
        culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
        flowDirection = FlowDirection.LeftToRight,
        typeface = tf,
        emSize = 1000.,
        foreground = Brushes.Red,
        pixelsPerDip = 1.25)//dpiInfo.PixelsPerDip)

printf "Height          : %f \n" formattedText.Height
printf "Baseline        : %f \n" formattedText.Baseline
printf "Extent          : %f \n" formattedText.Extent
printf "LineHeight      : %f \n" formattedText.LineHeight
printf "OverhangAfter   : %f \n" formattedText.OverhangAfter
printf "OverhangLeading : %f \n" formattedText.OverhangLeading
printf "OverhangTrailing: %f \n" formattedText.OverhangTrailing

printf "XHeight         : %f \n" (tf.XHeight * 1000.)
printf "CapsHeight      : %f \n" (tf.CapsHeight * 1000.)

tf.FontFamily
tf.Weight


let STIX2Math_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"), "./#STIX Two Math")
let STIX2TextBold_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Bold"), "./#STIX Two Text-Bold")
let STIX2TextBoldItalic_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-BoldItalic"), "./#STIX Two Text-BoldItalic")
let STIX2TextItalic_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Italic"), "./#STIX Two Text-Italic")
let STIX2TextRegular_FontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Text-Regular"), "./#STIX Two Text-Regular")