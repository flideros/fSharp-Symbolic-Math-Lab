namespace Math.Presentation

open System.Windows       
open System.Windows.Controls  
open System.Windows.Shapes  
open System.Windows.Media

type FontMetrics = 
    {ascender:float; 
     descender:float; 
     capital:float; 
     height:float;
     xHeight:float;     
    }

type Font = 
    {emSquare : float<MathML.em>
     metrics : FontMetrics
     size : float<MathML.px>
    }

