namespace XMindAPI
{
    public class XMindConfiguration
    {
        /// <summary>
        /// Determines the output verbosity
        /// </summary>
        private XMindWriterVerbosityLevel _verbosityLevel;
        /// <summary>
        /// Configures the write that generated files  will be emitted to.
        /// </summary>
        public XMindWriterConfiguration WriteTo { get; internal set; }
        public XMindConfiguration()
        {
            WriteTo = new XMindWriterConfiguration(this);
        }

        public XMindConfiguration SetVerbosityLevel(XMindWriterVerbosityLevel level)
        {
            return this;
        }

    }

    public enum XMindWriterVerbosityLevel {Result, Information, Debug}
}