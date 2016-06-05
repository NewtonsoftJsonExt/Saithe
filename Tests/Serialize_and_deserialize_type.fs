namespace Tests
open NUnit.Framework
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel

[<TypeConverter(typeof<ValueTypeConverter<ValueType>>)>]
type ValueType={ Value:string }

[<TypeConverter(typeof<ValueTypeConverter<IntValueType>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<IntValueType>>)>]
type IntValueType={ Value:int }


[<TypeConverter(typeof<ValueTypeConverter<CSharpyValueType>>)>]
type CSharpyValueType(value:string)=
    member this.Value with get() = value

[<TypeConverter(typeof<ValueTypeConverter<CSharpyIntValueType>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<CSharpyIntValueType>>)>]
type CSharpyIntValueType(value:int)=
    member this.Value with get() = value

[<Serializable>]
[<CLIMutable>]
type ValueContainer={ Value:ValueType; IntValue:IntValueType }

[<Serializable>]
type CSharpyValueContainer(value:CSharpyValueType, intValue: CSharpyIntValueType)=
    member val Value = value with get, set
    member val IntValue = intValue with get, set

[<TestFixture>]
type ``Serialize and deserialize type``() = 

    [<Test>]
    member this.Newtonsoft()=
        let data = @"{""Value"":""Ctr"",""IntValue"":1}"
        let result = JsonConvert.DeserializeObject<ValueContainer>(data);
        let expected = { Value = { Value = "Ctr" }; IntValue={Value=1}}
        Assert.AreEqual(expected, result)

    [<Test>]
    member this.Newtonsoft_serialize()=
        let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
        let result = JsonConvert.SerializeObject({ Value = { Value = "Mgr" }; IntValue={Value=1}})
        Assert.AreEqual(expected, result)

    [<Test>]
    member this.CSharp_Newtonsoft()=
        let data = @"{""Value"":""Ctr"",""IntValue"":1}"
        let result = JsonConvert.DeserializeObject<CSharpyValueContainer>(data);
        Assert.AreEqual("Ctr", result.Value.Value)
        Assert.AreEqual(1, result.IntValue.Value)

    [<Test>]
    member this.CSharp_Newtonsoft_serialize()=
        let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
        let result = JsonConvert.SerializeObject(CSharpyValueContainer(CSharpyValueType("Mgr"), CSharpyIntValueType(1)))
        Assert.AreEqual(expected, result)
