using ECommerceApplication.Domain.Models;
using ECommerceApplication.Infrastructure.Configuration;
using ECommerceApplication.Infrastructure.Persistence.DataContext;
using ECommerceApplication.Infrastructure.Persistence.Repository;
using ECommerceWebAPIApplication.Service;
using Serilog;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                                               .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddInfrastructure();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //registering custom user service
    builder.Services.AddScoped<CustomerService>();
    //registering dapper context
    builder.Services.AddScoped<DapperDataContext>();
    //registering customer repository
    builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();

    builder.Services.AddScoped(provider =>
    new DbName { ECommerceDB = "ECommerceDB" });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application failed to start up.");

}
finally
{
    Log.CloseAndFlush();
}
