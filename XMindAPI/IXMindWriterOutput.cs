namespace XMindAPI
{
    public interface IXMindWriterOutput
    {
        string OutputName { get; set;}
        XMindOutputType Type {get; set;}
    }
}