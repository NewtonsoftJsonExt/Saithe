namespace Tests.NewtonsoftJson.Parse_csharp_type
open Xunit
open System
open Newtonsoft.Json
open System.ComponentModel
open CSharpTypes
[<Serializable; CLIMutable>]
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
        let ex = Assert.Throws<Exception>( fun ()->
           JsonConvert.DeserializeObject<PValueContainer>(data) |> ignore
        )
        Assert.Equal ("Could not parse product id", ex.Message)

    [<Fact>]
    member this.TypeConverter_deserialize()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertTo("P_1", typeof<ParseValueType>) |> ignore

    [<Fact>]
    member this.TypeConverter_deserialize_invalid_data()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        Assert.Throws<ParseValueException>( fun ()->
          c.ConvertTo("Ctr", typeof<ParseValueType>) |> ignore
        ) |> ignore

    [<Fact>]
    member this.TypeConverter_convert_from_t()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertFrom(ParseValueType("1")) |> ignore
    [<Fact>]
    member this.TypeConverter_convert_from_str()=
        let c = TypeDescriptor.GetConverter(typeof<ParseValueType>)
        c.ConvertFrom("P_1") |> ignore
