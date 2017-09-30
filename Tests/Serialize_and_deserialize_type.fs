namespace Tests.Serialize_and_deserialize_type

open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel

[<TypeConverter(typeof<ValueTypeConverter<ValueType>>)>]
type ValueType = 
  { Value : string }

[<JsonConverter(typeof<ValueTypeJsonConverter<IntValueType>>)>]
type IntValueType = 
  { Value : int }

[<TypeConverter(typeof<ValueTypeConverter<CSharpyValueType>>)>]
type CSharpyValueType(value : string) = 
  member this.Value = value

[<JsonConverter(typeof<ValueTypeJsonConverter<CSharpyIntValueType>>)>]
type CSharpyIntValueType(value : int) = 
  member this.Value = value

[<Serializable>]
[<CLIMutable>]
type ValueContainer = 
  { Value : ValueType
    IntValue : IntValueType }

[<Serializable>]
type CSharpyValueContainer(value : CSharpyValueType, intValue : CSharpyIntValueType) = 
  member val Value = value with get, set
  member val IntValue = intValue with get, set

type ``Serialize and deserialize type``() = 
  
  [<Fact>]
  member this.Newtonsoft() = 
    let data = @"{""Value"":""Ctr"",""IntValue"":1}"
    let result = JsonConvert.DeserializeObject<ValueContainer>(data)
    
    let expected = 
      { Value = { Value = "Ctr" }
        IntValue = { Value = 1 } }
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.Newtonsoft_serialize() = 
    let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
    
    let result = 
      JsonConvert.SerializeObject({ Value = { Value = "Mgr" }
                                    IntValue = { Value = 1 } })
    Assert.Equal(expected, result)
  
  [<Fact>]
  member this.CSharp_Newtonsoft() = 
    let data = @"{""Value"":""Ctr"",""IntValue"":1}"
    let result = JsonConvert.DeserializeObject<CSharpyValueContainer>(data)
    Assert.Equal("Ctr", result.Value.Value)
    Assert.Equal(1, result.IntValue.Value)
  
  [<Fact>]
  member this.CSharp_Newtonsoft_serialize() = 
    let expected = @"{""Value"":""Mgr"",""IntValue"":1}"
    let result = JsonConvert.SerializeObject(CSharpyValueContainer(CSharpyValueType("Mgr"), CSharpyIntValueType(1)))
    Assert.Equal(expected, result)
