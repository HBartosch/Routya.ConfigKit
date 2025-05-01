namespace Routya.ConfigKit.Generator.Models
{
    internal class ConfigSectionMetadata
    {
        public string SectionName { get; set; }
        public ConfigBindingMode BindingMode { get; set; }

        public ConfigSectionMetadata(string sectionName, ConfigBindingMode bindingMode)
        {
            SectionName = sectionName;
            BindingMode = bindingMode;
        }
    }
}