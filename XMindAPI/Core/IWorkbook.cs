using System.Collections.Generic;
using System.Threading.Tasks;

namespace XMindAPI.Core
{
    /// <summary>
    /// Provides actions on the workbook
    /// </summary>
    public interface IWorkbook: IAdaptable
    {
        /// <summary>
        /// Returns ITopic
        /// </summary>
        /// <returns></returns>
        ITopic CreateTopic();

        /// <summary>
        /// Returns Sheet
        /// </summary>
        /// <returns></returns>
        ISheet CreateSheet();
        void AddSheet(ISheet sheet);
        void AddSheet(ISheet sheet, int index);
        IEnumerable<ISheet> GetSheets();

        ISheet GetPrimarySheet();

        void RemoveSheet(ISheet sheet);

        /// <summary>
        /// Gets an element with the given identifier string.
        /// </summary>
        /// <param name="id">The identifier string of the desired element</param>
        /// <returns>The element with the given identifier string.</returns>
        /// TODO: consider to use IWorkbookComponent for return type
        object? GetElementById(string id);
        /// <summary>
        /// Finds an element with the given identifier string requested starting from the source object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        object? FindElement(string id, IAdaptable source);

        /// <summary>
        /// Get a topic element with a given id.
        /// </summary>
        /// <returns></returns>
        ITopic? FindTopic(string id);

        // IManifest GetManifest()
        // IMeta GetMate()

        /// <summary>
        /// Saves workbook based on <see cref="Configuration.XMindConfiguration"/>
        /// <seealso cref="Writers.Configuration.XMindWriterConfiguration"/>
        /// </summary>
        Task Save();
        IRelationship CreateRelationship(IRelationshipEnd rel1, IRelationshipEnd rel2);
        IRelationship CreateRelationship();
    }
}
