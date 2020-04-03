#r @"PresentationCore"
#r @"PresentationFramework"
#r @"WindowsBase"
#r @"System.Xaml"
#r @"UIAutomationTypes"
#r @"System.Windows"       
//#r @"D:\MyFolders\Desktop\SymbolicMath\Math Display\bin\Debug\Math_Display.dll"
#load "MathML.fs"
#load "OperatorDictionary.fs"
#load "MathematicalAlphanumericSymbols.fs"
#load "MathematicalStandardizedVariants.fs"
#load "Text.fs"
#load "TypeSetting.fs"
open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

let fontFamily = FontFamily(System.Uri("file:///" + __SOURCE_DIRECTORY__ + "\\#STIX2Math"), "./#STIX Two Math")
let tf = Typeface(fontFamily,System.Windows.FontStyle(),System.Windows.FontWeight(),System.Windows.FontStretch())

let tf2 = Typeface((char 0x221b).ToString())

let formattedText = 
    FormattedText(
        textToFormat = "W",//(Math.Presentation.TypeSetting.getOperatorString MathML.OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix),//(char 0x23b3).ToString(),//"H",//
        culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
        flowDirection = FlowDirection.LeftToRight,
        typeface = tf,
        emSize = 1000.,
        foreground = Brushes.Red,
        pixelsPerDip = 1.25)//dpiInfo.PixelsPerDip)

let glyph = Math.Presentation.TypeSetting.getGlyph ((Math.Presentation.TypeSetting.getOperatorString MathML.OperatorDictionary.mathematicalRightFlattenedParenthesisPostfix)) {emSquare = 1000.<MathML.em>;typeFace = tf;size = 300.<MathML.px>}


printf "Glyph Height    : %f \n" glyph.path.Data.Bounds.Height
printf "Height          : %f \n" formattedText.Height
printf "Baseline        : %f  %f \n" formattedText.Baseline (1000. - formattedText.Baseline)
printf "Extent          : %f \n" formattedText.Extent
printf "LineHeight      : %f \n" formattedText.LineHeight
printf "OverhangAfter   : %f \n" formattedText.OverhangAfter
printf "OverhangLeading : %f \n" formattedText.OverhangLeading
printf "OverhangTrailing: %f \n" formattedText.OverhangTrailing



printf "XHeight         : %f \n" (tf.XHeight * 1000.)
printf "CapsHeight      : %f \n" (tf.CapsHeight * 1000.)

tf.FontFamily
tf

fontFamily.Source

(new string([|((char)(System.Convert.ToInt32("0x2AB0",16)));((char)(System.Convert.ToInt32("0x338",16)))|]))


let testList   = [1.;2.;3.;4.;5.]
let testListoo = [0.1;0.01;0.001;0.0001;0.00001]

List.scan (fun acc x -> acc + x) 0. testList



(char 0x221b).ToString()