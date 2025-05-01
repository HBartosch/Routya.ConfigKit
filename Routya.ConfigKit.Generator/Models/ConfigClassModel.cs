using System.Collections.Generic;

namespace Routya.ConfigKit.Generator.Models
{
    internal class ConfigClassModel
    {
        public ConfigClassModel(
            string? @namespace, 
            string className, 
            string sectionName, 
            ConfigBindingMode bindingMode, 
            List<ConfigPropertyModel> properties)
        {
            Namespace = @namespace;
            ClassName = className;
            SectionName = sectionName;
            BindingMode = bindingMode;
            Properties = properties;
        }

        public string? Namespace { get; set; }

        public string ClassName { get; set; }

        public string SectionName { get; set; }

        public ConfigBindingMode BindingMode { get; set; } = ConfigBindingMode.Singleton;

        public List<ConfigPropertyModel> Properties { get; set; } = new List<ConfigPropertyModel>();
    }
}