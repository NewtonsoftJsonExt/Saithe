namespace Saithe

open System.ComponentModel
open System
open System.Reflection
open Newtonsoft.Json
type IParse<'T> = interface 
  abstract member Parse : string -> 'T 
end

type ParseTypeConverter<'T (* when 'T :> IParse<'T> *) >() = //when 'T : (static member parse : string -> 'T)
  inherit TypeConverter()
  let strT = typeof<string>
  let t = typeof<'T>
  let instance = Activator.CreateInstance(t)

  let parse_method = t.GetInterface("IParse`1").GetMethod("Parse")
  
  let parse s = 
    try 
      box (parse_method.Invoke(instance, [| s |]))
    with :? TargetInvocationException as e -> raise (e.GetBaseException())
  
  override this.CanConvertFrom(context, sourceType) = (strT = sourceType || sourceType = t)
  
  override this.ConvertFrom(context, culture, value) = 
    match value with
    | :? 'T as v -> box (v.ToString())
    | :? string as s -> parse s
    | _ -> // it's not
      failwithf "Expected string or %s" (value.GetType().Name)
  
  override this.CanConvertTo(context, destinationType) = (strT = destinationType || t = destinationType)
  override this.ConvertTo(context, culture, value, destinationType) = 
    if destinationType = t then box (parse value)
    else box (value.ToString())

type public ParseTypeJsonConverter<'T (* when 'T :> IParse<'T> *) >() = 
  inherit JsonConverter()
  let t = typeof<'T>
  let instance = Activator.CreateInstance(t)

  let parse_method = t.GetInterface("IParse`1").GetMethod("Parse")
  
  let parse s = 
    try 
      box (parse_method.Invoke(instance, [| s |]))
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
