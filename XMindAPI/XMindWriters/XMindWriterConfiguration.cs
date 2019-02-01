using XMindAPI.Writers;
using XMindAPI.Configuration;
using XMindAPI.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace XMindAPI.Writers.Configuration
{
    /// <summary>
    /// Controls writer configuration.
    /// </summary>
    public class XMindWriterConfiguration
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        internal Action<List<XMindWriterContext>> FinalizeAction { get => _finalizeAction; private set => _finalizeAction = value; }
        private readonly XMindConfiguration _xmindConfiguration;
        private Action<List<XMindWriterContext>> _finalizeAction; 
        
        // internal IXMindWriter MainWriter { get => _writer; set => _writer = value; }
        private List<IXMindWriter> _writers;

        private List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> _criteria;

        public XMindWriterConfiguration(XMindConfiguration xMindConfiguration)
        {
            this._xmindConfiguration = xMindConfiguration;
        }

        public XMindConfiguration Writer(IXMindWriter writer)
        {
            _writers = new List<IXMindWriter>{writer};
            return _xmindConfiguration;
        }

        public XMindConfiguration Writers(List<IXMindWriter> writers)
        {
            _writers = writers;
            return _xmindConfiguration;
        }

        public XMindConfiguration SetWriterBinding(List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> criteria)
        {
            _criteria = criteria;
            return _xmindConfiguration;
        }

        public XMindWriterConfiguration SetFinalizeAction(Action<List<XMindWriterContext>> action)
        {
            FinalizeAction = action;
            return this;
        }

        internal IXMindWriter ResolveWriter(XMindWriterContext context)
        {
            if(_criteria == null){
                if(_writers != null && _writers.Any()){
                    Logger.WarnFormat("XMindConfiguration.ResolveWriter: default writer is assigned");
                    return _writers[0];
                }else
                {
                    throw new InvalidOperationException("XMindConfiguration.ResolveWriter: Writer is not specified");
                }
            } 
            var writerFound = _criteria.Select(w => w.Invoke(context, _writers)).Where(w => w != null).FirstOrDefault();
            return writerFound;
        }

    }
}