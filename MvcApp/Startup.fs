namespace MvcApp
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
type Startup private () = 
    
    new(configuration : IConfiguration) as this = 
        Startup()
        then this.Configuration <- configuration
    
    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services : IServiceCollection) = 
        // Add framework services.
        services.AddMvc() |> ignore
        services.AddSwaggerGen(fun o -> 
            o.SwaggerDoc("v1", OpenApiInfo(Version = "v1", Title = "Current version"))
        ) |> ignore
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app : IApplicationBuilder, env : IWebHostEnvironment) = 
        app.UseSwagger(fun o -> ()) |> ignore
        app.UseSwaggerUI(fun o -> o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")) |> ignore
        app.UseMvc() |> ignore
    
    member val Configuration : IConfiguration = null with get, set