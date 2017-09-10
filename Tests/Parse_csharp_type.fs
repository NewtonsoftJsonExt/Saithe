namespace Tests.Parse_csharp_type
open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open CSharpTypes
[<Serializable>]
[<CLIMutable>]
type PValueContainer={ V:ProductId; }


type ``Parse type``() = 

    [<Fact>]
    member this.Newtonsoft()=
        let data = @"{""V"":""ProductId/1""}"
        let result = JsonConvert.DeserializeObject<PValueContainer>(data);
        let expected = { V = ProductId(1L) }
        Assert.Equal(expected, result)

    [<Fact>]
    member this.Newtonsoft_serialize()=
        let expected = @"{""V"":""ProductId/1""}"
        let result = JsonConvert.SerializeObject({ V = ProductId(1L)})
        Assert.Equal(expected, result)

    [<Fact>]
    member this.Newtonsoft_deserialize_invalid_data()=
        let data = @"{""V"":""1""}"
        Assert.Throws<JsonSerializationException>( fun ()->
           JsonConvert.DeserializeObject<PValueContainer>(data) |> ignore
        ) |> ignore

