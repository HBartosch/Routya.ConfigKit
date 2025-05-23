﻿using Routya.ConfigKit.Generator.Models;
using System.Text;

namespace Routya.ConfigKit.Generator.Emitters
{
    internal static class ConfigBindingEmitter
    {
        internal static string Generate(ConfigClassModel model)
        {
            var sb = new StringBuilder();

            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(model.Namespace))
            {
                sb.AppendLine($"namespace {model.Namespace}");
                sb.AppendLine("{");
            }

            sb.AppendLine($"    public static class {model.ClassName}ConfigExtensions");
            sb.AppendLine("    {");
            sb.AppendLine($"        public static IServiceCollection Add{model.ClassName}(this IServiceCollection services, IConfiguration configuration)");
            sb.AppendLine("        {");

            // AddSingleton / Both
            if (model.BindingMode == ConfigBindingMode.Singleton || model.BindingMode == ConfigBindingMode.Both)
            {
                sb.AppendLine($"            var options = new {model.ClassName}()");
                sb.AppendLine("            {");
                foreach (var property in model.Properties)
                {
                    sb.AppendLine($"                {property.Name} = configuration.GetValue<{property.TypeName}>(\"{model.SectionName}:{property.Name}\"),");
                }
                sb.AppendLine("            };");
                sb.AppendLine();
                sb.AppendLine("            var validationContext = new ValidationContext(options);");
                sb.AppendLine("            Validator.ValidateObject(options, validationContext, validateAllProperties: true);");
                sb.AppendLine();
                sb.AppendLine("            services.AddSingleton(options);");
                sb.AppendLine();
            }

            // IOptions / Both
            if (model.BindingMode == ConfigBindingMode.IOptions || model.BindingMode == ConfigBindingMode.Both)
            {
                sb.AppendLine($"            services.AddOptions<{model.ClassName}>()");
                sb.AppendLine($"                    .Bind(configuration.GetSection(\"{model.SectionName}\"))");
                sb.AppendLine("                    .ValidateDataAnnotations()");
                sb.AppendLine("                    .ValidateOnStart();");
                sb.AppendLine();
            }

            sb.AppendLine("            return services;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");

            if (!string.IsNullOrWhiteSpace(model.Namespace))
            {
                sb.AppendLine("}");
            }

            return sb.ToString();
        }

    }
}