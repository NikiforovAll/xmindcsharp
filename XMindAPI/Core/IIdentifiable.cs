namespace XMindAPI.Core
{
    /// <summary>
    /// Provides object Id
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Provides access to Id of object
        /// </summary>
        /// <returns></returns>
        string GetId();
    }
}