namespace Saithe
open System.ComponentModel
open Newtonsoft.Json
open System.Collections.Generic
open System.Linq
open System
open System.Runtime.Serialization
open System.Reflection

type ValueTypeMapping<'T, 'V>() =
    let t = typeof<'T>
    let property = t.GetProperties()
                    |> Array.head
    let ctor = t.GetConstructor([|typeof<'V>|]);

    member this.Parse(value:'V) =
        ctor.Invoke([|value|])

    member this.ToRaw(value) : 'V=
        property.GetValue(value, null) :?> 'V

type ValueTypeConverter<'T, 'V>() = 
    inherit TypeConverter()
    let mapping = ValueTypeMapping<'T, 'V>()

    override this.CanConvertFrom(context, sourceType) = 
        if (sourceType = typeof<'V>) then true
        else base.CanConvertFrom(context, sourceType)
    
    override this.ConvertFrom(context, culture, value) = 
        if (value :? 'V) then mapping.Parse(value :?> 'V)
        else base.ConvertFrom(context, culture, value)
    
    override this.ConvertTo(context, culture, value, destinationType) = 
        if (destinationType = typeof<'V>) then mapping.ToRaw(value) :> obj
        else base.ConvertTo(context, culture, value, destinationType)


type public ValueTypeJsonConverter<'T,'V>() = 
    inherit JsonConverter()
    let mapping = ValueTypeMapping<'T, 'V>()
    let t = typeof<'T>

    override this.CanConvert(objectType) = objectType = t
    
    override this.ReadJson(reader, objectType, existingValue, serializer) = 
        if (objectType = t) then 
            let v = serializer.Deserialize<'V>(reader)
            mapping.Parse(v)
        else 
            //base.ReadJson(reader, objectType, existingValue, serializer)
            failwithf "Cant handle type %s, expects %s" (objectType.Name) (t.Name)
    
    override this.WriteJson(writer, value, serializer) = 
        if (value :? 'T) then writer.WriteValue(mapping.ToRaw(value :?> 'T))
        else
            //base.WriteJson(writer, value, serializer)
            failwithf "Cant handle type %s, expects %s" (value.GetType().Name) (t.Name)