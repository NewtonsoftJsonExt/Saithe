namespace Saithe
open System.ComponentModel
open System
open System.Reflection

type ParseTypeConverter<'T>() = //when 'T : (static member parse : string -> 'T)
    inherit TypeConverter()
    let strT = typeof<string>
    let t = typeof<'T>
    let parse = t.GetTypeInfo().GetMethod("Parse")

    override this.CanConvertFrom(context, sourceType) = 
        (strT = sourceType)
    override this.ConvertFrom(context, culture, value) = 
        try
            box (parse.Invoke(null,[|value :?> string|]))
        with
            | :? TargetInvocationException as e -> raise (e.GetBaseException())

    override this.CanConvertTo(context, destinationType) =
        (strT = destinationType)
    override this.ConvertTo(context, culture, value, destinationType) = 
        box (value.ToString())
       