﻿using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DomainEvents;
using CleanArchitecture.Infrastructure.Logging;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StructureMap;

namespace CleanArchitecture.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void ConfigureJwt(this IServiceCollection services, byte[] key)
        {
            services.AddCors(option =>
            {
                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                var userService =
                                    context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                                var userId = int.Parse(context.Principal.Identity.Name);
                                var user = userService.GetById(userId);
                                if (user == null)
                                {
                                    // return unauthorized if user no longer exists
                                    context.Fail("Unauthorized");
                                }

                                return Task.CompletedTask;
                            }
                        };
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
            });
        }
    }
}
