namespace XMindAPI.Core
{
    internal class Core
    {
        private static IIdFactory _factoryInstance = new IdFactory(); 
        public static IIdFactory GetFactory()
        {
            return _factoryInstance;
        }
    }
}