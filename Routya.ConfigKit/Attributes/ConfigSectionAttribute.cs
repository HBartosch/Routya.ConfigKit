using System;

namespace Routya.ConfigKit
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigSectionAttribute : Attribute
    {
        public string SectionName { get; }
        public ConfigBindingMode Mode { get; }

        public ConfigSectionAttribute(string sectionName, ConfigBindingMode mode = ConfigBindingMode.Singleton)
        {
            SectionName = sectionName;
            Mode = mode;
        } 
    }
}