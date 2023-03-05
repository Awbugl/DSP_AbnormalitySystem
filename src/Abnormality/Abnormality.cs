using System.Collections.Generic;

namespace DSP_AbnormalitySystem.Abnormality
{
    public abstract class Abnormality
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbnormalityType Type { get; set; }
        public Effect[] Effects { get; set; }
        public StringProto[] Translations { get; set; }
    }

    public class Effect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EffectType Type { get; set; }
        // public string Icon { get; set; }
        public int[] Value { get; set; }
    }
}