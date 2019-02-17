using System.Collections.Generic;

namespace XMindAPI.Core
{
    /// <summary>
    /// Provides actions on the sheet
    /// </summary>
    public interface ISheet : IIdentifiable, ITitled, IWorkbookComponent, IAdaptable
    {
        /// <summary>
        /// Returns root topic of the sheet
        /// </summary>
        /// <returns>Root topic of sheet</returns>
        ITopic GetRootTopic();

        /// <summary>
        /// Replaces root topic of the sheet
        /// </summary>
        void ReplaceRootTopic(ITopic newRootTopic);

        
        HashSet<IRelationship> GetRelationships();
        void AddRelationship(IRelationship relationship);

        void RemoveRelationship(IRelationship relationship);

    }
}