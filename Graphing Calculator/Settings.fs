﻿namespace GraphingCalculator

type setting = 
        member this.UMin = "u Min";
        member this.UMax = "u Max";
        member this.UGrid = "u GridSections";
        member this.VMin = "v Min";
        member this.VMax = "v Max";
        member this.VGrid = "v GridSections";
        member this.XMin = "x Min";
        member this.XMax = "x Max";
        member this.YMin = "y Min";
        member this.YMax = "y Max";
        member this.XMin2D = "x Min";
        member this.XMax2D = "x Max";
        member this.YMin2D = "y Min";
        member this.YMax2D = "y Max";
        member this.Min2D = "t Min";
        member this.Max2D = "t Max";
        member this.Step2D = "t Step";
        member this.UMinReg = "uMin";
        member this.UMaxReg = "uMax";
        member this.UGridReg = "uGrid";
        member this.VMinReg = "vMin";
        member this.VMaxReg = "vMax";
        member this.VGridReg = "vGrid";
        member this.XMinReg = "xMin";
        member this.XMaxReg = "xMax";
        member this.YMinReg = "yMin";
        member this.YMaxReg = "yMax";
        member this.XMin2DReg = "xMin2D";
        member this.XMax2DReg = "xMax2D";
        member this.YMin2DReg = "yMin2D";
        member this.YMax2DReg = "yMax2D";
        member this.Min2DReg = "tMin2D";
        member this.Max2DReg = "tMax2D";
        member this.Step2DReg = "tStep2D";
        member this.UMinDefault = "-pi";
        member this.UMaxDefault = "pi";
        member this.UGridDefault = "24";
        member this.VMinDefault = "0";
        member this.VMaxDefault = "pi";
        member this.VGridDefault = "48";
        member this.XMinDefault = "-10";
        member this.XMaxDefault = "10";
        member this.YMinDefault = "-10";
        member this.YMaxDefault = "10";
        member this.XMin2DDefault = "-10";
        member this.XMax2DDefault = "10";
        member this.YMin2DDefault = "-10";
        member this.YMax2DDefault = "10";
        member this.Min2DDefault = "0";
        member this.Max2DDefault = "10pi";
        member this.Step2DDefault = "pi/16";
        member this.Fx = "x( u,v )=";
        member this.Fy = "y( u,v )=";
        member this.Fz = "z( u,v )=";
        member this.Y = "y=";
        member this.Xt = "x( t )=";
        member this.Yt = "y( t )=";
        member this.FxReg = "fx";
        member this.FyReg = "fy";
        member this.FzReg = "fz";
        member this.YReg = "y";
        member this.XtReg = "xt";
        member this.YtReg = "yt";
        member this.FxDefault = "cos(u)sin(v)";
        member this.FyDefault = "-cos(v)";
        member this.FzDefault = "sin(-u)sin(v)";
        member this.YDefault = "x*sin(x^2)+x";
        member this.XtDefault = "sin(t)*t/pi";
        member this.YtDefault = "cos(t)*t/pi";
        member this.Function = "2D";
        member this.Function2D = "2D Parametric";
        member this.Function3D = "3D Parametric";
        member this.FunctionNone = "None";
