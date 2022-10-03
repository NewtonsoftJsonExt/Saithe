namespace Saithe.NewtonsoftJson
open Newtonsoft.Json
open System
open Saithe

type public ValueTypeJsonConverter<'T>() = 
  inherit JsonConverter()
  let mapping = ValueTypeMapping<'T>()
  let t = typeof<'T>
  let nullableT = (typedefof<Nullable<_>>).MakeGenericType([|mapping.PropertyType|])
  override this.CanConvert(objectType) = 
    objectType = t
  override this.ReadJson(reader, objectType, existingValue, serializer) = 
    if (objectType = t) then 
      let v = serializer.Deserialize(reader, mapping.PropertyType)
      mapping.Parse(v)
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      let v = serializer.Deserialize(reader, nullableT)
      if isNull v then
        null
      else
        mapping.Parse(v)
    else failwithf "Cant handle type %s, expects %s (1)" (objectType.Name) (t.Name)
  
  override this.WriteJson(writer, value, serializer) = 
    if (value :? 'T) then writer.WriteValue(mapping.ToRaw(value :?> 'T))
    else failwithf "Cant handle type %s, expects %s (2)" (value.GetType().Name) (t.Name)

