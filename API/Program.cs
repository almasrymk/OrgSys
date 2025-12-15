using Domain.Abstraction;
using Entity;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Infrastructure.Persistence.Data.OrgContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IOrgContext>(provider => provider.GetRequiredService<Infrastructure.Persistence.Data.OrgContext>());

builder.Services.AddMediatR(cfg => 
{ 
    //cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Org.City.Commands.CreateCityCommand).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Org.Country.Commands.CreateCountryCommand).Assembly); 
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MapperConfig>(); });

builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
