// using XMindAPI.Writers;
using XMindAPI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using XMindAPI.Models;
using XMindAPI.Infrastructure.Logging;

namespace XMindAPI.Writers.Configuration
{
    /// <summary>
    /// Controls writer configuration.
    /// </summary>
    public class XMindWriterConfiguration
    {
        internal Action<List<XMindWriterContext>, XMindWorkBook>? FinalizeAction { get; private set; }
        private readonly XMindConfiguration _xMindConfiguration;

        private List<IXMindWriter<IXMindWriterOutputConfig>>? _writers;

        private List<Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>>>? _criteria;

        public XMindWriterConfiguration(XMindConfiguration xMindConfiguration)
        {
            _xMindConfiguration = xMindConfiguration;
            // _criteria = new List<Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>>>();
            // _writers = new List<IXMindWriter<IXMindWriterOutputConfig>>();
        }

        public XMindConfiguration Writer(IXMindWriter<IXMindWriterOutputConfig> writer)
        {
            _writers = new List<IXMindWriter<IXMindWriterOutputConfig>> { writer };
            return _xMindConfiguration;
        }

        public XMindConfiguration Writers(List<IXMindWriter<IXMindWriterOutputConfig>> writers)
        {
            _writers = writers;
            // foreach (var writer in writers)
            // {
            //     //Accept resolver
            // }
            return _xMindConfiguration;
        }

        public XMindConfiguration SetWriterBinding(List<Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>>> criteria)
        {
            _criteria = criteria;
            return _xMindConfiguration;
        }

        public XMindConfiguration SetFinalizeAction(Action<List<XMindWriterContext>, XMindWorkBook> action)
        {
            FinalizeAction = action;
            return _xMindConfiguration;
        }

        internal IList<IXMindWriter<IXMindWriterOutputConfig>> ResolveWriters(XMindWriterContext context)
        {
            if (_writers == null || !_writers.Any())
            {
                throw new InvalidOperationException(
                    "XMindConfiguration.ResolveWriter: Writer is not specified");
            }
            if (_criteria == null)
            {
                Logger.Log.Warning(
                    "XMindConfiguration.ResolveWriter: default writer is assigned");
                return _writers.Take(1).ToList();
            }
            var writersFound = _criteria.Select(w => w.Invoke(context, _writers)).Where(w => w != null);
            Logger.Log.DebugTrace(
                $"For context.FileName: {context.FileName} ResolveWriters.writersFound: {writersFound.Count()}");
            return writersFound.ToList();
        }

        internal void AddResolver(Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>> criteria)
        {
            _criteria?.Add(criteria);
        }
    }
}
