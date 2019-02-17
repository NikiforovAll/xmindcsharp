namespace XMindAPI.Core
{
    public interface IRelationship : IIdentifiable, ITitled, ISheetComponent
    {
        IRelationshipEnd End1 { get; set; }
        IRelationshipEnd End2 { get; set; }

        ISheet GetParent();

    }
}