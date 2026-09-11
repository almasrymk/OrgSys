using API.Authentication;
using API.Middlewares;
using OrgSys.SharedKernel;
using Domain.Abstraction;
using FluentValidation;
using Infrastructure.Persistence.UnitOfWork;
using Accounting.Infrastructure.DependencyInjection;
using Administration.Infrastructure.DependencyInjection;
using MasterData.Infrastructure.DependencyInjection;
using MediatR;
using Organization.Infrastructure.DependencyInjection;
using Treasury.Infrastructure.DependencyInjection;
using Sales.Infrastructure.DependencyInjection;
using Inventory.Infrastructure.DependencyInjection;
using Purchasing.Infrastructure.DependencyInjection;
using Receivables.Infrastructure.DependencyInjection;
using Payables.Infrastructure.DependencyInjection;
using Reporting.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

const string AngularClientCorsPolicy = "AngularClient";

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularClientCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        };
    });

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    c.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document, null)] = new List<string>()
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 44300;
});

builder.Services.AddDbContext<Infrastructure.Persistence.Data.OrgContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrgConnection")));

builder.Services.AddScoped<IOrgContext>(provider => provider.GetRequiredService<Infrastructure.Persistence.Data.OrgContext>());

// Single Application-assembly anchor for MediatR/AutoMapper/FluentValidation scanning.
// MappingProfile and FluentValidationFilter<,> both live in Application, so the two
// AddMediatR calls this used to make were scanning the same assembly twice — collapsed here.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// IAccountingPeriodService, IReceivableAccountValidator, IPayableAccountValidator are all
// registered by AddAccountingModule() below.
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationFilter<,>));

// Extracted modules register their own MediatR/AutoMapper/FluentValidation slice here — see
// docs/modular-monolith-target-architecture.md §9. Country/City/District/Unit/Classification/
// Currency/ReferenceType/PaymentType moved out of Application into MasterData in this pass.
builder.Services.AddMasterDataModule();
builder.Services.AddOrganizationModule();
builder.Services.AddAdministrationModule();
builder.Services.AddAccountingModule();
builder.Services.AddTreasuryModule();
builder.Services.AddSalesModule();
builder.Services.AddInventoryModule();
builder.Services.AddPurchasingModule();
builder.Services.AddReceivablesModule();
builder.Services.AddPayablesModule();
builder.Services.AddReportingModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Moved ahead of auth/routing so it wraps the whole request pipeline, including exceptions
// thrown by authentication/authorization handlers — it used to run after MapControllers(),
// which meant it never actually surrounded those stages (see docs/modular-monolith-analysis.md §9).
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors(AngularClientCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();