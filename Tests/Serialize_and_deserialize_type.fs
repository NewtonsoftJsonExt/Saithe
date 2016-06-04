namespace Tests
open NUnit.Framework
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel

[<TypeConverter(typeof<ValueTypeConverter<ValueType,string>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<ValueType>>)>]
[<CLIMutable>]
type ValueType={ Value:string }

[<TypeConverter(typeof<ValueTypeConverter<CSharpyValueType,string>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<CSharpyValueType>>)>]
type CSharpyValueType(value:string)=
    member this.Value with get() = value

[<Serializable>]
[<CLIMutable>]
type ValueContainer={ Value:ValueType }

[<Serializable>]
type CSharpyValueContainer(value:CSharpyValueType)=
    member val Value = value with get, set

[<TestFixture>]
type ``Serialize and deserialize type``() = 

    [<Test>]
    member this.Newtonsoft()=
        let data = @"{""Value"":""Ctr""}"
        let result = JsonConvert.DeserializeObject<ValueContainer>(data);
        let expected = { Value = { Value = "Ctr" }}
        Assert.AreEqual(expected, result)

    [<Test>]
    member this.Newtonsoft_serialize()=
        let expected = @"{""Value"":""Mgr""}"
        let result = JsonConvert.SerializeObject({ Value = { Value = "Mgr" }})
        Assert.AreEqual(expected, result)

    [<Test>]
    member this.CSharp_Newtonsoft()=
        let data = @"{""Value"":""Ctr""}"
        let result = JsonConvert.DeserializeObject<CSharpyValueContainer>(data);
        Assert.AreEqual("Ctr", result.Value.Value)

    [<Test>]
    member this.CSharp_Newtonsoft_serialize()=
        let expected = @"{""Value"":""Mgr""}"
        let result = JsonConvert.SerializeObject(CSharpyValueContainer(CSharpyValueType("Mgr")))
        Assert.AreEqual(expected, result)
