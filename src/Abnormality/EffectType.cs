using System;

namespace DSP_AbnormalitySystem
{
    [Flags]
    public enum EffectType
    {
        None = 0,
        AddItem = 1,
        AddVein = 2,
        AddEntity = 4,
        AddTechHash = 8,
        TriggerItemAbnormality = 16,
        TriggerVeinAbnormality = 32,
        TriggerEntityAbnormality = 64,
        TriggerPlanetAbnormality = 128,
        TriggerStarAbnormality = 256,
        PlanetEffect = 512,
        StarEffect = 1024,
    }
}
