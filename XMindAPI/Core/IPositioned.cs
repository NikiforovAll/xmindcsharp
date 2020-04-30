namespace XMindAPI.Core
{
    public interface IPositioned
    {
        (int x, int y) Position { get; }
    }
}
