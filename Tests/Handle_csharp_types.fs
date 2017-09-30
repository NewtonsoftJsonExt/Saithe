namespace Tests.Handle_csharp_types

open Xunit
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open CSharpTypes

[<Serializable>]
[<CLIMutable>]
type Customer = 
  { Name : string
    Id : CustomerId }

type ``Serialize and deserialize struct type``() = 
  
  [<Fact>]
  member this.Struct_Newtonsoft() = 
    let data = @"{""Name"":""Ctr"",""Id"":1}"
    let result = JsonConvert.DeserializeObject<Customer>(data)
    Assert.Equal("Ctr", result.Name)
    Assert.Equal(1L, result.Id.Value)
  
  [<Fact>]
  member this.Struct_Newtonsoft_serialize() = 
    let expected = @"{""Name"":""Mgr"",""Id"":1}"
    let result = 
      JsonConvert.SerializeObject({ Name = "Mgr"
                                    Id = CustomerId(1L) })
    Assert.Equal(expected, result)
