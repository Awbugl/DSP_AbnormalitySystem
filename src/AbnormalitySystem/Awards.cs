using System.Collections.Generic;
using DSP_AbnormalitySystem.Abnormality;

namespace DSP_AbnormalitySystem
{
    public static partial class AbnormalitySystem
    {
        internal delegate void Awards(int[] value);

        internal static Dictionary<EffectType, Awards> AwardsMap = new Dictionary<EffectType, Awards>()
                                                                   {
                                                                       { EffectType.AddItem, AddItemAwards },
                                                                       { EffectType.AddVein, AddVeinAwards },
                                                                       { EffectType.AddEntity, AddEntityAwards },
                                                                       { EffectType.AddTechHash, AddTechHashAwards },
                                                                       { EffectType.TriggerItemAbnormality, TriggerItemAbnormalityAwards },
                                                                       { EffectType.TriggerVeinAbnormality, TriggerVeinAbnormalityAwards },
                                                                       { EffectType.TriggerEntityAbnormality, TriggerEntityAbnormalityAwards },
                                                                       { EffectType.TriggerPlanetAbnormality, TriggerPlanetAbnormalityAwards },
                                                                       { EffectType.TriggerStarAbnormality, TriggerStarAbnormalityAwards },
                                                                       { EffectType.PlanetEffect, PlanetEffectAwards },
                                                                       { EffectType.StarEffect, StarEffectAwards },
                                                                   };

        private static void AddItemAwards(int[] value)
        {
            for (var i = 0; i < value.Length; i += 2) GameMain.history.GainTechAwards(value[i], value[i + 1]);
        }

        private static void AddVeinAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void AddEntityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void AddTechHashAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void TriggerItemAbnormalityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void TriggerVeinAbnormalityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void TriggerEntityAbnormalityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void TriggerPlanetAbnormalityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void TriggerStarAbnormalityAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void PlanetEffectAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }

        private static void StarEffectAwards(int[] value)
        {
            throw new System.NotImplementedException();
        }
    }
}
