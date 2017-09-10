namespace MvcApp.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open MvcApp.Models

type CustomerModificationModel={FirstName:string ; LastName:string}

[<Route("api/[controller]")>]
type CustomersController () =
    inherit Controller()

    [<HttpGet>]
    [<Produces(typeof<Customer array>)>]
    member this.Get() =
        [|{FirstName= "value1"; LastName="Second"; Version=1; Id= CustomerId.Parse "c-y_e5TfGCE0iEKwD7Fe4XUw" } 
          {FirstName= "value2"; LastName="Second"; Version=1; Id= CustomerId.Parse "c-4ChA7W4VAUWu24HqwBTciw" }
        |]

    [<HttpGet("{id}")>]
    [<Produces(typeof<Customer>)>]
    member this.Get(id:CustomerId) =
        "value"

    [<HttpPost>]
    member this.Post([<FromBody>]value:CustomerModificationModel) =
        ()

    [<HttpPut("{id}")>]
    member this.Put(id:CustomerId, [<FromBody>]value:CustomerModificationModel ) =
        ()

    [<HttpDelete("{id}")>]
    member this.Delete(id:CustomerId) =
        ()
