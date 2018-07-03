using System;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FeatureToggle
{
    /// <summary>
    /// Rough implementation of utilising a yaml file as a configuration file
    /// </summary>
    public static class ConfigurationManager
    {
        private static readonly Lazy<Deserializer> Deserializer = new Lazy<Deserializer>(() =>
        {
            return new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        });

        /// <summary>
        /// Read yaml config file using <see cref="YamlDotNet.Serialization.Deserializer"/>
        /// </summary>
        /// <returns></returns>
        private static Configuration ReadConfig()
        {
            using (var fs = new FileStream("../../config.yml", FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    return Deserializer.Value.Deserialize<Configuration>(sr.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Read yaml config file using <see cref="YamlDotNet.RepresentationModel.YamlStream"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object ReadConfig(string key)
        {
            using (var fs = new FileStream("../../config.yml", FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var ys = new YamlStream();
                    ys.Load(sr);

                    var mapping = (YamlMappingNode)ys.Documents.First().RootNode;

                    return mapping.Children[new YamlScalarNode(key)];
                }
            }
        }

        public static Configuration Configuration => ReadConfig();
        public static bool EnableMultiplier => Boolean.TryParse(ReadConfig("enable-multiplier").ToString(), out var result);
    }
}
