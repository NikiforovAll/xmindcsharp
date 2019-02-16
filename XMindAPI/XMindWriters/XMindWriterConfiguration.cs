// using XMindAPI.Writers;
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
        private readonly XMindConfiguration _xMindConfiguration;
        private Action<List<XMindWriterContext>> _finalizeAction; 
        
        // internal IXMindWriter MainWriter { get => _writer; set => _writer = value; }
        private List<IXMindWriter> _writers;

        private List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> _criteria;

        public XMindWriterConfiguration(XMindConfiguration xMindConfiguration)
        {
            this._xMindConfiguration = xMindConfiguration;
        }

        public XMindConfiguration Writer(IXMindWriter writer)
        {
            _writers = new List<IXMindWriter>{writer};
            return _xMindConfiguration;
        }

        public XMindConfiguration Writers(List<IXMindWriter> writers)
        {
            _writers = writers;
            foreach (var writer in writers)
            {
                //Accept resolver
            }
            return _xMindConfiguration;
        }

        public XMindConfiguration SetWriterBinding(List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> criteria)
        {
            _criteria = criteria;
            return _xMindConfiguration;
        }

        public XMindConfiguration SetFinalizeAction(Action<List<XMindWriterContext>> action)
        {
            FinalizeAction = action;
            return _xMindConfiguration;
        }

        internal List<IXMindWriter> ResolveWriters(XMindWriterContext context)
        {
            if(_writers == null || !_writers.Any())
            {
                throw new InvalidOperationException("XMindConfiguration.ResolveWriter: Writer is not specified");
            }
            if(_criteria == null){
                    Logger.WarnFormat("XMindConfiguration.ResolveWriter: default writer is assigned");
                    return _writers.Take(1).ToList();
            } 
            var writersFound = _criteria.Select(w => w.Invoke(context, _writers)).Where(w => w != null);
            Logger.Debug($"For context.FileName: {context.FileName} ResolveWriters.writersFound: {writersFound.Count()}");
            return writersFound.ToList();
        }

        internal void AddResolver(Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter> criteria)
        {
            _criteria.Add(criteria);
        }
    }
}