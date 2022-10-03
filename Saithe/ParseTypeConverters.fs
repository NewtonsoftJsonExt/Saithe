namespace Saithe
open System.ComponentModel
open System.Reflection

type ParseTypeConverter<'T (*when 'T :> IParsable<'T>*) >() = //when 'T : (static member parse : string -> 'T)
  inherit TypeConverter()
  let strT = typeof<string>
  let t = typeof<'T>
  let parse_method = t.GetTypeInfo().GetMethod("Parse")
  
  let parse s = 
    try 
      box (parse_method.Invoke(null, [| s |]))
    with :? TargetInvocationException as e -> raise (e.GetBaseException())
  
  override this.CanConvertFrom(context, sourceType) = (strT = sourceType || sourceType = t)
  
  override this.ConvertFrom(context, culture, value) = 
    match value with
    | :? 'T as v -> box (v.ToString())
    | :? string as s -> parse s
    | _ -> // it's not
      failwithf $"Expected string or %s{value.GetType().Name}"
  
  override this.CanConvertTo(context, destinationType) = (strT = destinationType || t = destinationType)
  override this.ConvertTo(context, culture, value, destinationType) = 
    if destinationType = t then box (parse value)
    else box (value.ToString())

