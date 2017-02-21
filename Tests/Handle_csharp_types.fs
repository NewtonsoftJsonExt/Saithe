namespace Tests.Handle_csharp_types
open NUnit.Framework
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
open CSharpTypes
[<Serializable>]
[<CLIMutable>]
type Customer={ Name:string; Id:CustomerId }


[<TestFixture>]
type ``Serialize and deserialize struct type``() = 

    [<Test>]
    member this.Struct_Newtonsoft()=
        let data = @"{""Name"":""Ctr"",""Id"":1}"
        let result = JsonConvert.DeserializeObject<Customer>(data);
        Assert.AreEqual("Ctr", result.Name)
        Assert.AreEqual(1, result.Id.Value)

    [<Test>]
    member this.Struct_Newtonsoft_serialize()=
        let expected = @"{""Name"":""Mgr"",""Id"":1}"
        let result = JsonConvert.SerializeObject({Name="Mgr"; Id=CustomerId(1L)})
        Assert.AreEqual(expected, result)
