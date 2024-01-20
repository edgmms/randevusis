namespace Hyper.Core.Domain
{
    public interface ISoftDeletedEntity
    {
        bool Deleted { get; set; }
    }
}
