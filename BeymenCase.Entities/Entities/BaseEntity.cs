namespace BeymenCase.Core.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = null!;
        public string ApplicationName { get; set; }
    }
}
