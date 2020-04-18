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
    internal sealed class XMindConfigurationLoader
    {
        public IConfiguration? XMindConfigCollection { get; private set; }



        // TODO: consider to remove lazy instantiated dependency for static field
        // it is not possible to have control over this thing, nor to manage configuration life time
        // this should be only used internally, solution to load items temporarily
        public static XMindConfigurationLoader Configuration { get { return lazy.Value; } }
        private static readonly Lazy<XMindConfigurationLoader> lazy =
            new Lazy<XMindConfigurationLoader>(
                    () => new XMindConfigurationLoader()
                        .LoadConfigurationFile());

        private XMindConfigurationLoader()
        {
        }

        /// <summary>
        /// Allows to get file locations from config xmindsettings.json file
        /// </summary>
        /// <returns>Mapping between filename to location. [filename => location]</returns>
        internal Dictionary<string, string> GetOutputFilesLocations()
        {
            if(XMindConfigCollection is null) {
                throw new InvalidOperationException("Output files are not configured");
            }
            var basePath = XMindConfigCollection["output:base"];
            var sectionGroup = XMindConfigCollection.GetSection("output:files").GetChildren();
            var result = sectionGroup.ToDictionary(
                    s => s.GetChildren().FirstOrDefault(kvp => kvp.Key == "name").Value,
                    s => s.GetChildren().FirstOrDefault(kvp => kvp.Key == "location").Value
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
                [XMindConfiguration.ManifestLabel] = XMindConfigCollection[XMindConfiguration.ManifestLabel],
                [XMindConfiguration.MetaLabel] = XMindConfigCollection[XMindConfiguration.MetaLabel],
                [XMindConfiguration.ContentLabel] = XMindConfigCollection[XMindConfiguration.ContentLabel]
            };
        }
        private XMindConfigurationLoader LoadConfigurationFile()
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
            catch (Exception)
            {
                // TODO: remove and add proper exception handling
                throw;
            }
            return this;
        }

    }
}
