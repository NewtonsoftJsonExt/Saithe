namespace Saithe.NewtonsoftJson

open Newtonsoft.Json
open System
open System.Reflection

type public ParseTypeJsonConverter<'T>() = 
  inherit JsonConverter()
  let t = typeof<'T>

  let parse_method = t.GetTypeInfo().GetMethod("Parse")
  
  let parse s = 
    try 
      box (parse_method.Invoke(null, [| s |]))
    with :? TargetInvocationException as e -> raise (e.GetBaseException())
  
  override this.CanConvert(objectType) = objectType = t
  
  override this.ReadJson(reader, objectType, existingValue, serializer) = 
    if (objectType = t) then 
      let v = serializer.Deserialize(reader, typeof<string>)
      parse (v)
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      let v = serializer.Deserialize(reader, typeof<string>)
      if isNull v then
        null
      else
        parse v
    else failwithf "Cant handle type %s, expects %s (3)" (objectType.Name) (t.Name)
  
  override this.WriteJson(writer, value, serializer) = 
    if (value :? 'T) then writer.WriteValue((value :?> 'T).ToString())
    else failwithf "Cant handle type %s, expects %s (4)" (value.GetType().Name) (t.Name)

