using System.Collections.Generic;

namespace DSP_AbnormalitySystem
{
    public class Abnormality
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbnormalityType Type { get; set; }
        public int SubType { get; set; }
        public Effect[] Effects { get; set; }
        public StringProto[] Translations { get; set; }
    }

    public class Effect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EffectType Type { get; set; }
        public Dictionary<EffectType, int[]> Value { get; set; }
    }
}
