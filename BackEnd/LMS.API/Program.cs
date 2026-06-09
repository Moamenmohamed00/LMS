using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
}); 
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LMSDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddIdentity<ApplicationUser,IdentityRole<Guid>>()
    .AddEntityFrameworkStores<LMSDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSwaggerGen(options=>{
    options.SwaggerDoc("v1",new OpenApiInfo{
        Title="LMS API",
        Version="v1"
    });
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{
        Type=SecuritySchemeType.Http,
        Description="Enter JWT token",
        Scheme="Bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    }
                });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();

