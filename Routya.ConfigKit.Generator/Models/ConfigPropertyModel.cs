namespace Routya.ConfigKit.Generator.Models
{
    internal class ConfigPropertyModel
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public bool IsRequired { get; set; }

        public object DefaultValue { get; set; }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}