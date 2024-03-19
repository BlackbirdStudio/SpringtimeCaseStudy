using CaseStudy.Configuration;
using CaseStudy.EFCore;
using CaseStudy.Middlewares;
using CaseStudy.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var configuration = builder.Configuration.Get<SpringTimeConfiguration>()!;
builder.Services.AddSingleton(configuration);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.Redis.ConnectionString;
    options.InstanceName = configuration.Redis.InstanceName;
});
var redis = (IServiceProvider provider) => ConnectionMultiplexer.Connect(configuration.Redis.ConnectionString);
builder.Services.AddTransient<IConnectionMultiplexer, ConnectionMultiplexer>(redis);
builder.Services.AddDbContext<CaseStudyContext>();
builder.Services.AddTransient<Repository>();
builder.Services.AddResponseCaching();
builder.Services.AddAutoMapper(typeof(CaseStudyProfile));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SpringtimeApi",
        Version = "v1",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    c.CustomSchemaIds(type => type.FullName);
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var config = builder.Configuration;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = configuration.Jwt.Issuer,
        ValidAudience = configuration.Jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Jwt.Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RedisCachingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();
