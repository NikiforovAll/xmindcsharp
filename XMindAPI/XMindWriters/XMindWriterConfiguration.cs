using XMindAPI.Writers;
using XMindAPI.Configuration;
using System;

namespace XMindAPI.Writers.Configuration
{
    /// <summary>
    /// Controls writer configuration.
    /// </summary>
    public class XMindWriterConfiguration
    {
        internal Action<XMindWriterContext> FinalizeAction { get => _finalizeAction; private set => _finalizeAction = value; }
        private readonly XMindConfiguration _xmindConfiguration;
        private Action<XMindWriterContext> _finalizeAction; 
        internal IXMindWriter MainWriter { get => _writer; set => _writer = value; }

        private IXMindWriter _writer;

        public XMindWriterConfiguration(XMindConfiguration xMindConfiguration)
        {
            this._xmindConfiguration = xMindConfiguration;
        }

        public XMindConfiguration Writer(IXMindWriter writer)
        {
            MainWriter = writer;
            return _xmindConfiguration;
        }

        public XMindWriterConfiguration SetFinalizeAction(Action<XMindWriterContext> action)
        {
            FinalizeAction = action;
            return this;
        }

    }
}