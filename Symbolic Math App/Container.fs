module Container

/// This container is used by some controls to share a variable.
/// If the value is changed, it fires changed event.
/// Controls should have this instead of their own internal data
type SharedValue<'a when 'a : equality>(value:'a) =
  let mutable _value = value
  let changed = Event<'a>()
  member o.Get       = _value
  member o.Set value =
    let old = _value 
    _value <- value
    if old <> _value then _value |> changed.Trigger
  member o.Changed = changed.Publish
type share<'a when 'a : equality> = SharedValue<'a>

