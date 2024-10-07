namespace BeymenCase.Core.Entities
{
    public class ConfigurationRecord : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
