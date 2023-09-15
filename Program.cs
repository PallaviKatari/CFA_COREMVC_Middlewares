using DFA_COREMVC.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure the Sql Server Database ConnectionStrings
builder.Services.AddDbContext<EmployeeContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("mvcConnection")));

var app = builder.Build();

//set up the Middlewares and endpoints for our ASP.NET Core MapGet() - GET Requests/ Map() - GET/PUT/POST/DELETE
//app.MapGet("/", () => "Welcome to Core MVC:" + System.Diagnostics.Process.GetCurrentProcess().ProcessName);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//Setting the Default Files - access index.html,index.htm,default.html,default.htm by DEFAULT
//app.UseDefaultFiles();

// Enable directory browsing on the current path
//app.UseDirectoryBrowser();

//Execute files in wwwroot https://localhost:7126/assets/images/img2.jpg
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

//Example 1 - Using Run - The output comes from the first middleware component.
//The reason is when we register a middleware component using the Run() extension method,
//then that component becomes a terminal component means it will not call the next
//middleware component in the request processing pipeline.

//Add one More Middleware Component to the Application Request Processing Pipeline.

//Configuring New Middleware Component using Run Extension Method
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Getting Response from First Middleware");
//});
//////Configuring New Middleware Component using Run Extension Method
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Getting Response from Second Middleware");
//});

//Example 2 - //Configuring New Middleware Components using Use Extension Method
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Getting Response from First Middleware");
//    await next();
//});
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("\nGetting Response from Second Middleware");
//});

//Example 3 - ASP.NET Core Request Processing Pipeline
//Configuring Middleware Component using Use and Run Extension Method
//First Middleware Component Registered using Use Extension Method
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware1: Incoming Request\n");
    //Calling the Next Middleware Component
    await next();
    await context.Response.WriteAsync("Middleware1: Outgoing Response\n");
});
//Second Middleware Component Registered using Use Extension Method
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware2: Incoming Request\n");
    //Calling the Next Middleware Component
    await next();
    await context.Response.WriteAsync("Middleware2: Outgoing Response\n");
});
//Third Middleware Component Registered using Run Extension Method
app.Run(async (context) =>
{
    await context.Response.WriteAsync("Middleware3: Incoming Request handled and response generated\n");
    //Terminal Middleware Component i.e. cannot call the Next Component
});

////Adding Another Middleware Component to the Request Processing Pipeline
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Request Handled and Response Generated");
//});

app.Run();
