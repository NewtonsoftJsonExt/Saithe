namespace Tests.Handle_discriminated_union

open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open System.Globalization

[<TypeConverter(typeof<ParseValueType_T1>)>]
[<JsonConverter(typeof<ParseValueType_T2>)>] 
type ParseValueType = 
  | ValueType of string
  | Empty
  
  static member Parse(str : string) = 
    match str.Split('_') |> Array.toList with
    | [] -> Empty
    | [ "P"; v ] -> ValueType v
    | _ -> raise (FormatException str)
  
  override this.ToString() = 
    match this with
    | Empty -> ""
    | ValueType value -> sprintf "P_%s" value
  interface IParsable<ParseValueType> with
    static member Parse(s:string, f:IFormatProvider) = ParseValueType.Parse(s)
    static member TryParse(s:string, f:IFormatProvider, result:byref<ParseValueType>) =
        try
            result <- ParseValueType.Parse(s)
            true
        with _ ->
            result <- Unchecked.defaultof<_>
            false
and private ParseValueType_T1 = ParseTypeConverter<ParseValueType>
and private ParseValueType_T2 = ParseTypeJsonConverter<ParseValueType>


[<Serializable>]
[<CLIMutable>]
type PValueContainer = 
  { V : ParseValueType }

type ``Parse type``() = 
  
  [<Fact>]
  member this.Newtonsoft() = 
    let data = @"{""V"":""P_Ctr""}"
    let result = JsonConvert.DeserializeObject<PValueContainer>(data)
    let expected = { V = ValueType "Ctr" }
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.Newtonsoft_serialize() = 
    let expected = @"{""V"":""P_Mgr""}"
    let result = JsonConvert.SerializeObject({ V = ValueType "Mgr" })
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.Newtonsoft_deserialize_invalid_data() = 
    let data = @"{""V"":""Ctr""}"
    Assert.Throws<FormatException>(fun () -> JsonConvert.DeserializeObject<PValueContainer>(data) |> ignore) 
    |> ignore
  
  [<Fact>]
  member this.TypeConverter_deserialize_from_invalid_data() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    Assert.Throws<FormatException>(fun () -> c.ConvertFrom("Ctr") |> ignore) |> ignore
  
  [<Fact>]
  member this.TypeConverter_deserialize_valid_data() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    let v = c.ConvertFrom("P_Mgr")
    Assert.Equal(ValueType "Mgr", v :?> ParseValueType)
  
  [<Fact>]
  member this.TypeConverter_deserialize() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    c.ConvertTo("P_1", typeof<ParseValueType>) |> ignore
  
  [<Fact>]
  member this.TypeConverter_deserialize_to_invalid_data() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    Assert.Throws<FormatException>(fun () -> c.ConvertTo("Ctr", typeof<ParseValueType>) |> ignore) |> ignore
  
  [<Fact>]
  member this.TypeConverter_convert_from_t() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    c.ConvertFrom(ValueType "1") |> ignore
  
  [<Fact>]
  member this.TypeConverter_convert_from_str() = 
    let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
    c.ConvertFrom("P_1") |> ignore
