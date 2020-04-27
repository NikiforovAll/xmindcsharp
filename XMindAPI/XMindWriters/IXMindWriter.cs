using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public interface IXMindWriter<out TConfig> where TConfig : IXMindWriterOutputConfig
    {
        Task WriteToStorage(XDocument document, string fileName);
        IXMindWriter<TConfig> SetOutput(IXMindWriterOutputConfig output);
        TConfig GetOutputConfig();
    }
}
