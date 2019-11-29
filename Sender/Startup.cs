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
                    var host = cfg.Host(new Uri("rabbitmq://192.168.139.102"), hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    // Настройка топологии для Send
                    //cfg.Send<IFileReceivedEvent>(x =>
                    //{
                    //    x.UseRoutingKeyFormatter(context =>
                    //      {
                    //          var msg = context.Message as FileReceivedEvent;
                    //          return msg?.Message == "ASD" ? "routingKey" : "routingKey2";
                    //      });
                    //});
                    //cfg.Message<IFileReceivedEvent>(x => x.SetEntityName("TestMessage"));

                    // Настройка топологии для Publish
                    cfg.Publish<IFileReceivedEvent>(x =>
                    {
                        //x.ExchangeType = "direct";
                    });

                    // Регистрация потребителя (если нужно)
                    //cfg.ReceiveEndpoint(host, "Shop", ep => { });
                }));

                //configurator.AddRequestClient<IFileReceivedEvent>();
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