namespace XMindAPI.Core
{
    /// <summary>
    /// Provides access to object title
    /// </summary>
    public interface ITitled
    {
        /// <summary>
        /// Object title
        /// </summary>
        /// <value></value>
        string Title { get; set; }

        /// <summary>
        /// Returns whether object has a valid text
        /// </summary>
        /// <returns>true if object has a valid title text, false otherwise</returns>
        bool HasTitle();
    }
}