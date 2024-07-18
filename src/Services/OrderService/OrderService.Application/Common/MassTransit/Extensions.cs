﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace OrderService.Application.Common.MassTransit;

public static class Extentions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["RabbitMq:Host"]!), host =>
                {
                    host.Username(configuration["RabbitMq:Username"]);
                    host.Password(configuration["RabbitMq:Password"]);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
