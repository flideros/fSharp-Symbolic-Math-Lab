namespace ControlLibrary

open System
open System.Windows          
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Shapes


//\/----------------------------------- Shared Value Container ------------------------------------\/
/// This container is used by some controls to share a variable. If the value is changed, 
/// it fires changed event. Controls should have this instead of their own internal data.
type SharedValue<'a when 'a : equality>(value:'a) =
    let mutable _value = value
    let changed = Event<'a>()
    member _sv.Get       = _value
    member _sv.Set value =
        let old = _value 
        _value <- value
        if old <> _value then _value |> changed.Trigger
    member _sv.Changed = changed.Publish
//\------------------------------------- Shared Value Container ------------------------------------/\


//\/-------------------------------------Shape Container -------------------------------------------\/
/// Shape container control which reacts when properties of a shape change.
[<RequireQualifiedAccess>]
type Shapes = Rectangle | Ellipse | CustomShape of Shape | Polygon of PointCollection | CustomPolygon of PointCollection * Polygon

type ShapeContainer(shapes:SharedValue<Shapes>,
                    width:SharedValue<float>,
                    height:SharedValue<float>,
                    color:SharedValue<Color>)                     
                    // Other shared properties...
                    as this =    
    
    // -> Refactor this to suit
    inherit Border()//Width=250., Height=250.)

    let mutable shape = Ellipse() :> Shape

    let setWidth  width  = shape.Width  <- width
    let setHeight height = shape.Height <- height
    let setColor  color  = shape.Fill   <- SolidColorBrush(color)
         
    let makePolygonFrom points options =     
        let p = 
            match options with 
            | None -> Polygon()
            | Some poly -> poly
        do p.Points <- points
           p.Stretch <- shape.Stretch
        p 
  
    let initShape () =
        this.Child <- shape
        setWidth  width.Get
        setHeight height.Get
        setColor  color.Get
        
    let setShape s =
        match s with
          | Shapes.Rectangle -> shape <- Rectangle()
          | Shapes.Ellipse   -> shape <- Ellipse  ()
          | Shapes.CustomShape e -> shape <- e 
          | Shapes.Polygon points -> shape <- makePolygonFrom points None
          | Shapes.CustomPolygon (points,p) -> 
            shape <- makePolygonFrom points (Some p)
        initShape ()

    do  // specifying cooperations with shared values and the shape
        width.Changed.Add  setWidth
        height.Changed.Add setHeight
        color.Changed.Add  setColor 
        shapes.Changed.Add setShape
        // initialization
        setShape shapes.Get

//\-------------------------------------Shape Container -------------------------------------------/\

