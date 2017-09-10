namespace Tests.Parse_type
open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel

[<TypeConverter(typeof<ParseTypeConverter<ParseValueType>>)>]
type ParseValueType={ Value:string }
with
    static member Parse (str:string)= 
        match str.Split('_') |> Array.toList with
        | ["P";v] -> { Value=v }
        | _ -> failwithf "Could not parse %s" str
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

