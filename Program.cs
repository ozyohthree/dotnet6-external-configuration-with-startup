using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeCompany.Models;

var builder = WebApplication.CreateBuilder(args);

// Set conection String
var connectionString = builder.Configuration.GetConnectionString("Employees") ?? "Data Source=Employee.db";


// Swagger config
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<EmployeeDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConnection")));


// Uncomment this line to use the inmemory DB
//builder.Services.AddDbContext<EmployeeDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Employee API",
         Description = "A dummy employee API",
         Version = "v1" });
});

// Add Healtchecks
builder.Services.AddHealthChecks();

// Add custom json properties file
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});

// Add services to the container.
builder.Services.AddRazorPages();


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method