﻿namespace Tests.Parse_fs_type
open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open System.Globalization

[<TypeConverter(typeof<ParseValueType_T1>)>]
type ParseValueType={ Value:string }
with
    static member Parse (str:string)= 
        match str.Split('_') |> Array.toList with
        | ["P";v] -> { Value=v }
        | _ -> raise (FormatException str)
    override this.ToString()=
        sprintf "P_%s" this.Value
    interface IParsable<ParseValueType> with
        static member Parse(s:string, f:IFormatProvider) = ParseValueType.Parse(s)
        static member TryParse(s:string, f:IFormatProvider, result:byref<ParseValueType>) =
            try
                result <- ParseValueType.Parse(s)
                true
            with _ ->
                result <- Unchecked.defaultof<_>
                false
and internal ParseValueType_T1 = ParseTypeConverter<ParseValueType>

[<Serializable>]
[<CLIMutable>]
type PValueContainer={ V:ParseValueType; }

type ``Parse type``() = 

    [<Fact>]
    member this.Newtonsoft()=
        let data = @"{""V"":""P_Ctr""}"
        let result = JsonConvert.DeserializeObject<PValueContainer>(data);
        let expected = { V = { Value = "Ctr" } }
        Assert.Equal(expected, result)

    [<Fact>]
    member this.Newtonsoft_serialize()=
        let expected = @"{""V"":""P_Mgr""}"
        let result = JsonConvert.SerializeObject({ V = { Value = "Mgr" }})
        Assert.Equal(expected, result)

    [<Fact>]
    member this.Newtonsoft_deserialize_invalid_data()=
        let data = @"{""V"":""Ctr""}"
        Assert.Throws<JsonSerializationException>( fun ()->
           JsonConvert.DeserializeObject<PValueContainer>(data) |> ignore
        ) |> ignore


    [<Fact>]
    member this.TypeConverter_deserialize_from_invalid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)

        Assert.Throws<FormatException>( fun ()->
           c.ConvertFrom("Ctr") |> ignore
        ) |> ignore

    [<Fact>]
    member this.TypeConverter_deserialize_valid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        let v = c.ConvertFrom("P_Mgr")
        Assert.Equal({ Value = "Mgr" } ,v :?> ParseValueType)

    [<Fact>]
    member this.TypeConverter_deserialize()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertTo("P_1", typeof<ParseValueType>) |> ignore

    [<Fact>]
    member this.TypeConverter_deserialize_to_invalid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        Assert.Throws<FormatException>( fun ()->
          c.ConvertTo("Ctr", typeof<ParseValueType>) |> ignore
        ) |> ignore

    [<Fact>]
    member this.TypeConverter_convert_from_t()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertFrom({ Value = "1" }) |> ignore
    [<Fact>]
    member this.TypeConverter_convert_from_str()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertFrom("P_1") |> ignore
