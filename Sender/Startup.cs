using System;
using Common;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport.Topology.Topologies;
using MassTransit.Topology.Topologies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sender
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }

        public IConfiguration AppConfiguration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMassTransit(configurator =>
            {
                configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://192.168.0.192"), hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    cfg.Send<IFileReceivedEvent>(x =>
                    {
                        x.UseRoutingKeyFormatter(context =>
                          {
                              var msg = context.Message as FileReceivedEventEvent;
                              return msg?.Message == "ASD" ? "routingKey" : "routingKey2";
                          });
                    });
                    cfg.Message<IFileReceivedEvent>(x => x.SetEntityName("TestMessage"));
                    cfg.Publish<IFileReceivedEvent>(x => { x.ExchangeType = "direct"; });

                    cfg.ReceiveEndpoint(host, "Shop", ep => { });
                }));

                configurator.AddRequestClient<IFileReceivedEvent>();
            });

            services.AddSingleton<IHostedService, BusService>();

            //var rabbitMqOptions = new RabbitMqOptions();
            //AppConfiguration.GetSection("BillingV2Options").Bind(rabbitMqOptions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}