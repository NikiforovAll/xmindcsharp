using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace XMindAPI.Configuration
{
    /// <summary>
    /// Reads XMindConfiguration from xmindsettings.json file (or memory). It is possible to consume <see cref="IConfiguration"> as singleton via <seealso cref="XMindConfigCollection">
    /// </summary>
    internal sealed class XMindConfigurationCache
    {
        public IConfiguration XMindConfigCollection { get; private set; }

        public const string ManifestLabel = "output:definition:manifest";
        public const string MetaLabel = "output:definition:meta";
        public const string ContentLabel = "output:definition:content";

        public static XMindConfigurationCache Configuration { get { return lazy.Value; } }
        private static readonly Lazy<XMindConfigurationCache> lazy =
            new Lazy<XMindConfigurationCache>(
                    () => new XMindConfigurationCache()
                        .LoadConfigurationFile()
                    );

        private XMindConfigurationCache()
        {
        }

        /// <summary>
        /// Allows to get file locations from config xmindsettings.json file
        /// </summary>
        /// <returns>Mapping between filename to locatoin. [filename => location]</returns>
        internal Dictionary<string, string> GetOutputFilesLocations()
        {
            var basePath = XMindConfigCollection["output:base"];
            var sectionGroup = XMindConfigCollection.GetSection("output:files").GetChildren();
            var result = sectionGroup
                .ToDictionary(
                    s => s.GetChildren()
                        .FirstOrDefault(kvp => kvp.Key == "name").Value,
                    s => s.GetChildren().
                        FirstOrDefault(kvp => kvp.Key == "location").Value
                );
            return result;
        }

        /// <summary>
        /// Entry point for configuration for working with main files. 
        /// </summary>
        /// <returns>Mapping between manifest config token (label) to filename. [label => filename]</returns>
        internal Dictionary<string, string> GetOutputFilesDefinitions()
        {
            return new Dictionary<string, string>
            {
                [ManifestLabel] = Configuration.XMindConfigCollection[ManifestLabel],
                [MetaLabel] = Configuration.XMindConfigCollection[MetaLabel],
                [ContentLabel] = Configuration.XMindConfigCollection[ContentLabel]
            };
        }
        private XMindConfigurationCache LoadConfigurationFile()
        {
            try
            {
                var builder = new ConfigurationBuilder();
                var settingsFileName = "xmindsettings.json";
                var assemblyPathRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var settingsPath = Path.Combine(assemblyPathRoot, settingsFileName);
                if (File.Exists(settingsPath))
                {
                    builder.SetBasePath(assemblyPathRoot)
                        // .AddInMemoryCollection(
                        //     new Dictionary<string, string>
                        //     {
                        //         [ManifestLabel] = "output:definition:manifest",
                        //         [MetaLabel] = "output:definition:meta",
                        //         [ContentLabel] = "output:definition:content"
                        //     }
                        // )
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