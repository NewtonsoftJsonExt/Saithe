namespace Tests.Parse_fs_type
open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open System.Globalization
exception ParseValueException of string

[<TypeConverter(typeof<ParseTypeConverter<ParseValueType>>)>]
type ParseValueType={ Value:string }
with
    static member Parse (str:string)= 
        match str.Split('_') |> Array.toList with
        | ["P";v] -> { Value=v }
        | _ -> raise (ParseValueException str)
    override this.ToString()=
        sprintf "P_%s" this.Value

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
    member this.TypeConverter_deserialize_invalid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)

        Assert.Throws<ParseValueException>( fun ()->
           c.ConvertFrom("Ctr") |> ignore
        ) |> ignore

    [<Fact>]
    member this.TypeConverter_deserialize_valid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        let v = c.ConvertFrom("P_Mgr")
        Assert.Equal({ Value = "Mgr" } ,v :?> ParseValueType)
