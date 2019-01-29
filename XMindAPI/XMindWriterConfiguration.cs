namespace XMindAPI
{
    /// <summary>
    /// Controls writer configuration.
    /// </summary>
    public class XMindWriterConfiguration
    {
        private readonly XMindConfiguration _xmindConfiguration;
        private IXMindWriter _writer;

        public XMindWriterConfiguration(XMindConfiguration xMindConfiguration)
        {
            this._xmindConfiguration = xMindConfiguration;
        }

        public XMindConfiguration Writer(IXMindWriter writer)
        {
            _writer = writer;
            return _xmindConfiguration;
        }

    }
}