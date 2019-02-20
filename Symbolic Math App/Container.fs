namespace UI

open System
open System.Windows          
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Shapes
open TypeExtension


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
type Shapes = Rectangle | Ellipse | Polygon of PointCollection

type ShapeContainer(shapes:SharedValue<Shapes>,
                    width:SharedValue<int>,
                    height:SharedValue<int>,
                    color:SharedValue<Color>) 
                    // Other properties...
                    as this =    
    
    // -> Refactor this to suit
    inherit Label(Width=250., Height=250.)
    
    let mutable shape = Ellipse() :> Shape

    let setWidth  width  = shape.Width  <- float width
    let setHeight height = shape.Height <- float height
    let setColor  color  = shape.Fill   <- SolidColorBrush(color)
  
    // -> Refactor this 
    let makePolygonFrom points =     
        let p = Polygon()
        do p.Points <- points
           p.Stretch <- shape.Stretch
           // Other properties...
        p 
  
    let initShape () =
        this.Content <- shape
        setWidth  width.Get
        setHeight height.Get
        setColor  color.Get
    let setShape du =
        match du with
          | Shapes.Rectangle -> shape <- Rectangle()
          | Shapes.Ellipse   -> shape <- Ellipse  ()
          | Shapes.Polygon points -> shape <- makePolygonFrom points           
            
        initShape ()
    do
        // specifying cooperations with shared values and the shape
        width.Changed.Add  setWidth
        height.Changed.Add setHeight
        color.Changed.Add  setColor 
        shapes.Changed.Add setShape
        // initialization
        initShape ()
//\-------------------------------------Shape Container -------------------------------------------/\

