using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ludo.Infrastructure.Configurations;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Ludo.Infrastructure.Converters;
using Ludo.Infrastructure.Middlewares;
using Serilog;
using Serilog.Events;

namespace Ludo.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddCorsConfiguration()
        {
            var corsConfiguration = builder.Configuration.GetRequiredSection(nameof(CorsConfiguration)).Get<CorsConfiguration>();
            if (corsConfiguration == null)
            {
                throw new ApplicationException("The CORS configuration needs to be set!");
            }
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policyBuilder =>
                    {
                        policyBuilder.WithOrigins(corsConfiguration.Origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            return builder;
        }
        public WebApplicationBuilder UseLogger()
        {
            builder.Host.UseSerilog((_, logger) =>
            {
                logger
                    .MinimumLevel.Is(LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.WithThreadId()
                    .WriteTo.Console();
            });
            return builder;
        }
        public WebApplicationBuilder AddApi()
        {
            builder.Services.AddControllers(options => { options.Conventions.Add(new MultipartFormConvention()); });
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            });
            builder.Services.AddEndpointsApiExplorer()
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
                });
            return builder;
        }
        public WebApplicationBuilder ConfigureAuthentication()
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    var jwtConfiguration = builder.Configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();
                    if (jwtConfiguration == null)
                    {
                        throw new ApplicationException("The JWT configuration needs to be set!");
                    }
                    var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key);
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = jwtConfiguration.Audience,
                        ValidIssuer = jwtConfiguration.Issuer,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                }).Services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder().AddDefaultPolicy().Build();
                });
            return builder;
        }
        public WebApplicationBuilder AddAuthorizationWithSwagger(string application)
        {
            builder.Services
                .AddSwaggerGen()
                .AddOpenApi(options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddSchemaTransformer((schema, context, cancellationToken) =>
                    {
                        var jsonTypeInfo = context.JsonTypeInfo;
                        if (schema.Properties is null)
                        {
                            return Task.CompletedTask;
                        }
                        foreach (var prop in jsonTypeInfo.Properties)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                            if ((prop.Get is not null && prop.IsGetNullable) ||
                                (prop.Set is not null && prop.IsSetNullable) ||
                                (prop.PropertyType.IsGenericType &&
                                 prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                continue;
                            }
                            schema.Required ??= new HashSet<string>();
                            schema.Required.Add(prop.Name);
                        }
                        return Task.CompletedTask;
                    });
                });
            return builder;
        }
    }
    private static AuthorizationPolicyBuilder AddDefaultPolicy(this AuthorizationPolicyBuilder policy) =>
        policy.RequireClaim(ClaimTypes.NameIdentifier)
            .RequireClaim(ClaimTypes.Name)
            .RequireClaim(ClaimTypes.Email);
}