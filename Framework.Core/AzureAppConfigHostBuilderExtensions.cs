using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Models;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace Framework.Core
{
    public static class AzureAppConfigIHostBuilderExtensions
    {
        public static IHostBuilder ConfigureAzureAppConfiguration(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(configBuilder =>
            {
                var config = configBuilder.Build();
                var enabled = config.GetValue<bool>("Azure:AppConfig:Enabled");
                if (!enabled) return;
                var useManagedIdentity = config.GetValue<bool>("Azure:AppConfig:UseManagedIdentity");
                var connectionString = config.GetValue<string>("Azure:AppConfig:ConnectionString");
                var selectorsSection = config.GetSection("Azure:AppConfig:Selectors");
                var selectors = new List<KeyValueSelector>();
                selectorsSection.Bind(selectors);
                ConfigureAzureAppConfiguration(options =>
                {
                    if (useManagedIdentity)
                    {
                        options.Connect(new Uri(connectionString), new ManagedIdentityCredential())
                            .ConfigureKeyVault(kv => { kv.SetCredential(new ManagedIdentityCredential()); });
                    }
                    else
                    {
                        options.Connect(connectionString)
                            .ConfigureKeyVault(kv => { kv.SetCredential(new DefaultAzureCredential()); });
                    }
                }, configBuilder, selectors);
            });
        }

        private static void ConfigureAzureAppConfiguration(Action<AzureAppConfigurationOptions> setupAction,
            IConfigurationBuilder configBuilder,
            IList<KeyValueSelector> selectors)
        {
            var secretsSource = configBuilder.Sources.OfType<JsonConfigurationSource>()
                .FirstOrDefault(s => s.Path == "secrets.json");
            var envVarSource = configBuilder.Sources.OfType<EnvironmentVariablesConfigurationSource>()
                .FirstOrDefault();
            var cmdLineSource = configBuilder.Sources.OfType<CommandLineConfigurationSource>()
                .FirstOrDefault();
            if (secretsSource != null) configBuilder.Sources.Remove(secretsSource);
            if (envVarSource != null) configBuilder.Sources.Remove(envVarSource);
            if (cmdLineSource != null) configBuilder.Sources.Remove(cmdLineSource);
            configBuilder.AddAzureAppConfiguration(options =>
            {
                setupAction?.Invoke(options);
                options
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
                foreach (var kv in selectors)
                {
                    if (string.IsNullOrWhiteSpace(kv.LabelFilter))
                        options.Select(kv.KeyFilter);
                    else
                        options.Select(kv.KeyFilter, kv.LabelFilter);
                }
            }, true);
            if (secretsSource != null) configBuilder.Add(secretsSource);
            if (envVarSource != null) configBuilder.Add(envVarSource);
            if (cmdLineSource != null) configBuilder.Add(cmdLineSource);
        }


        public static IConfiguration BuildConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.development.json", optional: true);
            
            var config = configBuilder.Build();

            var enabled = config.GetValue<bool>("Azure:AppConfig:Enabled");

            if (!enabled)
            {
                return config;
            }

            var useManagedIdentity = config.GetValue<bool>("Azure:AppConfig:UseManagedIdentity");
            var connectionString = config.GetValue<string>("Azure:AppConfig:ConnectionString");
            var selectorsSection = config.GetSection("Azure:AppConfig:Selectors");
            var selectors = new List<KeyValueSelector>();
            selectorsSection.Bind(selectors);
            ConfigureAzureAppConfiguration(options =>
            {
                if (useManagedIdentity)
                {
                    options.Connect(new Uri(connectionString), new ManagedIdentityCredential())
                        .ConfigureKeyVault(kv => { kv.SetCredential(new ManagedIdentityCredential()); });
                }
                else
                {
                    options.Connect(connectionString)
                        .ConfigureKeyVault(kv => { kv.SetCredential(new DefaultAzureCredential()); });
                }
            }, configBuilder, selectors);
            return configBuilder.Build();
        }
    }
}