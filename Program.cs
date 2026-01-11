using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:7023", "http://localhost:7022");

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// HttpClient
builder.Services.AddHttpClient("MyAPIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7023/");
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; 
    });
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var addresses = app.Urls;
Console.WriteLine("Kestrel will listen on: " + string.Join(", ", addresses));


app.Run();
