namespace Diglett.Core.Domain
{
    public interface ITimestamped
    {
        DateTime CreatedOnUtc { get; set; }
        DateTime UpdatedOnUtc { get; set; }
    }
}