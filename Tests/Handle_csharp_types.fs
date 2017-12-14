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

[<Serializable>]
[<CLIMutable>]
type CustomerOrder = 
  { ProductDescription : string
    CustomerId : Nullable<CustomerId> 
    ProductId : Nullable<ProductId>
    OrderId : Nullable<OrderId>
  }

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

  [<Fact>]
  member this.Struct_Newtonsoft_Nullable() = 
    let data = @"{""ProductDescription"":""Description"",""CustomerId"":1,""ProductId"":""ProductId/2"",""OrderId"":3}"
    let result = JsonConvert.DeserializeObject<CustomerOrder>(data)
    Assert.Equal("Description", result.ProductDescription)
    Assert.Equal(1L, result.CustomerId.Value.Value)
    Assert.Equal(2L, result.ProductId.Value.Value)
    Assert.Equal(3L, result.OrderId.Value.Value)

  [<Fact>]
  member this.Struct_Newtonsoft_Nullable_null() = 
    let data = @"{""ProductDescription"":""Description"",""CustomerId"":null,""ProductId"":null,""OrderId"":null}"
    let result = JsonConvert.DeserializeObject<CustomerOrder>(data)
    Assert.Equal("Description", result.ProductDescription)
    Assert.Equal(false, result.CustomerId.HasValue)
    Assert.Equal(false, result.ProductId.HasValue)  
    Assert.Equal(false, result.OrderId.HasValue)  

  [<Fact>]
  member this.Struct_Newtonsoft_serialize_Nullable() = 
    let expected = @"{""ProductDescription"":""Description"",""CustomerId"":1,""ProductId"":""ProductId/2"",""OrderId"":""3""}"
    let result = 
      JsonConvert.SerializeObject({ ProductDescription = "Description"
                                    CustomerId = Nullable<CustomerId>(CustomerId(1L))
                                    ProductId= Nullable<ProductId>(ProductId(2L))
                                    OrderId=Nullable<OrderId>(OrderId(3L))
                                   })
    Assert.Equal(expected, result)


  [<Fact>]
  member this.Struct_Newtonsoft_serialize_Nullable_null() = 
    let expected = @"{""ProductDescription"":""Description"",""CustomerId"":null,""ProductId"":null,""OrderId"":null}"
    let result = 
      JsonConvert.SerializeObject({ ProductDescription = "Description"
                                    CustomerId = Nullable<CustomerId>()
                                    ProductId= Nullable<ProductId>()
                                    OrderId=Nullable<OrderId>()
                                   })
    Assert.Equal(expected, result)
