using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace XMindAPI
{
    /// <summary>
    /// Reads XMindConfiguration from xmindsettings.json file (or memory). It is possible to consume <see cref="IConfiguration"> as singleton via <seealso cref="XMindConfigCollection">
    /// </summary>
    internal sealed class XMindConfigurationCache
    {
        public IConfiguration XMindConfigCollection { get; private set; }
        public static XMindConfigurationCache Configuration {get { return lazy.Value;}}
        private static readonly Lazy<XMindConfigurationCache> lazy =
            new Lazy<XMindConfigurationCache>(
                    () => new XMindConfigurationCache()
                        .LoadConfigurationFile()
                    );

        private XMindConfigurationCache()
        {
        }
        
        private XMindConfigurationCache LoadConfigurationFile()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["namespace"] = "urn:xmind:xmap:xmlns:meta:2.0"
                        }
                    );
                var settingsFileName = "xmindsettings.json";
                var assemblyPathRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var settingsPath = Path.Combine(assemblyPathRoot, settingsFileName);
                if (File.Exists(settingsPath))
                {
                    builder.SetBasePath(assemblyPathRoot)
                        .AddJsonFile(path: settingsFileName, optional: true, reloadOnChange: true);
                }
                XMindConfigCollection = builder.Build();
            }
            catch (System.Exception)
            {
                // TODO: remove and add proper exception handling
                throw;
            }
            return this;
        }
    }
}