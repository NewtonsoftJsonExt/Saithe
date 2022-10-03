namespace Saithe.SystemTextJson
open System
open System.Text.Json.Serialization
open Saithe
module Internal =
  let read<'T> (mapping: ValueTypeMapping<'T>) v objectType =
    let t = typeof<'T>
    if (objectType = t) then
      mapping.Parse(v) :?> 'T
    else if (Nullable.GetUnderlyingType(objectType) = t) then
      if Object.ReferenceEquals(v,null) then
        Unchecked.defaultof<'T>
      else
        mapping.Parse(v) :?> 'T
    else failwithf $"Cant handle type %s{objectType.Name}, expects %s{t.Name} (1)"
    
open Internal
type ValueTypeStringJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let t = typeof<'T>
  let mapping = ValueTypeMapping<'T>()
  override this.CanConvert(objectType) = objectType = t
  override this.Read(reader, objectType, _) : 'T = read mapping (reader.GetString()|>unbox) objectType
  override this.Write(writer, value, _) = writer.WriteStringValue(string <| mapping.ToRaw(value))
  
type ValueTypeIntJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let t = typeof<'T>
  let mapping = ValueTypeMapping<'T>()
  override this.CanConvert(objectType) = objectType = t
  override this.Read(reader, objectType, _) : 'T = read mapping (reader.GetInt32()|>unbox) objectType
  override this.Write(writer, value, _) = writer.WriteNumberValue(mapping.ToRaw(value) :?> int32)
type ValueTypeShortJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let t = typeof<'T>
  let mapping = ValueTypeMapping<'T>()
  override this.CanConvert(objectType) = objectType = t
  override this.Read(reader, objectType, _) : 'T = read mapping (reader.GetInt16()|>unbox) objectType
  override this.Write(writer, value, _) = writer.WriteNumberValue(mapping.ToRaw(value) :?> int16)

type ValueTypeLongJsonConverter<'T when 'T :> obj>() =
  inherit JsonConverter<'T>()
  let t = typeof<'T>
  let mapping = ValueTypeMapping<'T>()
  override this.CanConvert(objectType) = objectType = t
  override this.Read(reader, objectType, _) : 'T = read mapping (reader.GetInt64()|>unbox) objectType
  override this.Write(writer, value, _) = writer.WriteNumberValue(mapping.ToRaw(value) :?> int64)
