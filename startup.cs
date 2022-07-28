using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeCompany.Models;


public class Startup {
    public IConfiguration configRoot {
        get;
    }
    public Startup(IConfiguration configuration) {
        configRoot = configuration;
    }
    public void ConfigureServices(IServiceCollection services) {
        services.AddRazorPages();
    }
    public void Configure(WebApplication app, IWebHostEnvironment env) {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();


        app.UseAuthorization();

        app.MapRazorPages();

        // // Swagger Endpoint
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
        });

        // Healtcheck endpoint
        app.MapHealthChecks("/healthz");

        // Get Employees
        app.MapGet("/employees", async (EmployeeDb db) => await db.Employees.ToListAsync());

        // Create Employees
        app.MapPost("/employees", async (EmployeeDb db, Employee employee) =>
        {
            await db.Employees.AddAsync(employee);
            await db.SaveChangesAsync();
            return Results.Created($"/employee/{employee.Id}", employee);
        });

        // Get Employee by id
        app.MapGet("/employee/{id}", async (EmployeeDb db, int id) => await db.Employees.FindAsync(id));

        // Update Employee
        app.MapPut("/employee/{id}", async (EmployeeDb db, Employee updateemployee, int id) =>
        {
            var employee = await db.Employees.FindAsync(id);
            if (employee is null) return Results.NotFound();
            employee.Name = updateemployee.Name;
            employee.Lastname = updateemployee.Lastname;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete Employee
        app.MapDelete("/employee/{id}", async (EmployeeDb db, int id) =>
        {
        var employee = await db.Employees.FindAsync(id);
        if (employee is null)
        {
            return Results.NotFound();
        }
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.Ok();
        });

        app.Run();

    }
}