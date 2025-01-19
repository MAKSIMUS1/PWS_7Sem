using Lab3_StudentAPI.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3_StudentAPI.Services;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddControllers();

builder.Services.AddDbContext<StudentsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRouting();

builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    return new UrlHelper(actionContext);
});


builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true; 
})
.AddXmlSerializerFormatters(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapFallbackToController("HandleFallback", "Fallback");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
