using API.Middlewares;
using Application.Validators;
using Domain.Abstraction;
using Entity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Infrastructure.Persistence.Data.OrgContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IOrgContext>(provider => provider.GetRequiredService<Infrastructure.Persistence.Data.OrgContext>());

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly); });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MapperConfig>(); });

builder.Services.AddSwaggerGen(c => { c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); });

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; });

//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(FluentValidationFilter<,>).Assembly);
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationFilter<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();