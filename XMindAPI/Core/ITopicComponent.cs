namespace XMindAPI.Core
{
    public interface ITopicComponent : ISheetComponent
    {
        ITopic Parent { get; }
    }
}
