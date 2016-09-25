namespace Tests.Handle_structs
open NUnit.Framework
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel

[<TypeConverter(typeof<ValueTypeConverter<StructValueType>>)>]
type StructValueType=
    struct 
        val Value:string
    end
    with new(value:string)={ Value=value }

[<TypeConverter(typeof<ValueTypeConverter<StructIntValueType>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<StructIntValueType>>)>]
type StructIntValueType=
    struct
        val Value:int
    end
    with new(value:int)={ Value=value }

[<Serializable>]
[<CLIMutable>]
type StructValueContainer={ Value:StructValueType; IntValue:StructIntValueType }


[<TestFixture>]
type ``Serialize and deserialize struct type``() = 

    [<Test>]
    member this.Struct_Newtonsoft()=
        let data = @"{""Value"":""Ctr"",""IntValue"":1}"
        let result = JsonConvert.DeserializeObject<StructValueContainer>(data);
        Assert.AreEqual("Ctr", result.Value.Value)
        Assert.AreEqual(1, result.IntValue.Value)

    [<Test>]
    member this.Struct_Newtonsoft_serialize()=
        let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
        let result = JsonConvert.SerializeObject({Value=StructValueType("Mgr"); IntValue=StructIntValueType(1)})
        Assert.AreEqual(expected, result)
