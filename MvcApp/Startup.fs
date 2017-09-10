namespace MvcApp

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Swashbuckle
open Swashbuckle.AspNetCore.Swagger
open MvcApp.Models
type Startup private () = 
    
    new(configuration : IConfiguration) as this = 
        Startup()
        then this.Configuration <- configuration
    
    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services : IServiceCollection) = 
        // Add framework services.
        services.AddMvc() |> ignore
        let i = Info()
        i.Version <-"v1"
        i.Title <-"Current version"
        services.AddSwaggerGen(fun o -> 
            o.SwaggerDoc("v1", i)
            o.MapType(typeof<CustomerId>, fun ()-> 
                let s = Schema()
                s.Type <- "string"
                s.Default <- CustomerId.Default.ToString()
                s
            )
        ) |> ignore
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app : IApplicationBuilder, env : IHostingEnvironment) = 
        app.UseSwagger(fun o -> ()) |> ignore
        app.UseSwaggerUI(fun o -> o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")) |> ignore
        app.UseMvc() |> ignore
    
    member val Configuration : IConfiguration = null with get, set