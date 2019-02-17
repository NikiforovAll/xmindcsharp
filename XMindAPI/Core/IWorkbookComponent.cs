namespace XMindAPI.Core
{
    /// <summary>
    /// Provides object capability to return parent workbook
    /// </summary>
    public interface IWorkbookComponent
    {
        /// <summary>
        /// Gets the workbook who owns this component.
        /// </summary>
        /// <value></value>
        IWorkbook OwnedWorkbook { get; set; }
    }
}