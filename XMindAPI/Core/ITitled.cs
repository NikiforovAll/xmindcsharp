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
        /// <returns></returns>
        string GetTitle();

        /// <summary>
        /// Object title
        /// </summary>
        /// <param name="value"></param>
        void SetTitle(string value);



        /// <summary>
        /// Returns whether object has a valid text
        /// </summary>
        /// <returns>true if object has a valid title text, false otherwise</returns>
        bool HasTitle();
    }
}