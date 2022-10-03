namespace Saithe
open System.ComponentModel
open System.Reflection

type ValueTypeMapping<'T>() = 
  let t = typeof<'T>.GetTypeInfo()
  let properties = t.GetProperties() |> Array.toList
  
  let fields = 
    t.GetFields()
    |> Array.filter (fun f -> not f.IsStatic)
    |> Array.toList
  
  let (getValue, propertyType) = 
    match properties, fields with
    | [], [ f ] -> (f.GetValue, f.FieldType)
    | [ p ], [] -> ((fun o -> p.GetValue(o, null)), p.PropertyType)
    | _, _ -> failwithf "Should have a single instance field or property"
  
  let ctor = t.GetConstructor([| propertyType |])
  member this.PropertyType = propertyType
  member this.Parse(value : obj) = ctor.Invoke([| value |])
  member this.ToRaw(value) : obj = getValue (value)

type ValueTypeConverter<'T>() = 
  inherit TypeConverter()
  let mapping = ValueTypeMapping<'T>()

  override this.CanConvertFrom(context, sourceType) = 
    sourceType = mapping.PropertyType
  override this.ConvertFrom(context, culture, value) = mapping.Parse(value)
  override this.CanConvertTo(context, destinationType) = 
    destinationType = mapping.PropertyType && destinationType = typeof<string>
  override this.ConvertTo(context, culture, value, destinationType) = mapping.ToRaw(value)
