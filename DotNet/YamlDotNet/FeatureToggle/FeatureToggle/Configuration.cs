using YamlDotNet.Serialization;

namespace FeatureToggle
{
    public class Configuration
    {
        [YamlMember(Alias = "enable-multiplier", ApplyNamingConventions = false)]
        public bool EnableMultiplier { get; set; }
    }
}
