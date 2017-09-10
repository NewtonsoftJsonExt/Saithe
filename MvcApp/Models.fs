module MvcApp.Models

open Saithe
open System
open System.ComponentModel

// from https://madskristensen.net/post/A-shorter-and-URL-friendly-GUID
let toStr (x : Guid) = 
    let enc = Convert.ToBase64String(x.ToByteArray())
    let enc = enc.Replace("/", "_").Replace("+", "-")
    enc.Substring(0, 22)

let fromStr (encoded : string) = 
    let encoded = encoded.Replace("_", "/").Replace("-", "+")
    let buffer = Convert.FromBase64String(encoded + "==")
    if buffer.Length = 16 then Some(Guid buffer)
    else None

let tryParseId (prefix : string) (str : string) = 
    if str.StartsWith(prefix) then fromStr (str.Substring prefix.Length)
    else None

let parseId prefix str = 
    let maybeParsed = tryParseId prefix str
    match maybeParsed with
    | Some g -> g
    | None -> raise (FormatException str)

[<TypeConverter(typeof<ParseTypeConverter<CustomerId>>)>]
type CustomerId = 
    { Value : Guid }
    static member Default : CustomerId = { Value=Guid.Empty }
    static member Parse(str : string) : CustomerId = { Value = parseId "c-" str }
    override this.ToString() = sprintf "c-%s" (toStr this.Value)

[<TypeConverter(typeof<ParseTypeConverter<ProductId>>)>]
type ProductId = 
    { Value : Guid }
    static member Default : ProductId = { Value=Guid.Empty }
    static member Parse(str : string) : ProductId = { Value = parseId "p-" str }
    override this.ToString() = sprintf "p-%s" (toStr this.Value)

[<TypeConverter(typeof<ParseTypeConverter<OrderId>>)>]
type OrderId = 
    { Value : Guid }
    static member Default : OrderId = { Value=Guid.Empty }
    static member Parse(str : string) : OrderId = { Value = parseId "o-" str }
    override this.ToString() = sprintf "o-%s" (toStr this.Value)


type Customer = {Id:CustomerId; FirstName:string ; LastName:string; Version:int}

type Product = {Id:ProductId; Cost:decimal; Name: string; Version: int}

type Order = {Id:OrderId; Customer:Customer; OrderDate:DateTime; Products: Product list; Version: int}
