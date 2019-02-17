namespace XMindAPI.Core
{
    public interface ISheetComponent : IWorkbookComponent
    {
        ISheet OwnedSheet {get; set;}
    }
}