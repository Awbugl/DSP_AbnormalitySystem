using System;
using System.Collections.Generic;

namespace DSP_AbnormalitySystem
{
    public static partial class AbnormalitySystem
    {
        internal delegate void Awards(int[] value);

        internal static readonly Dictionary<EffectType, Awards> AwardsMap = new Dictionary<EffectType, Awards>
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

        // TODO: Implement all awards

        private static void AddItemAwards(int[] value)
        {
            for (var i = 0; i < value.Length; i += 2) GameMain.history.GainTechAwards(value[i], value[i + 1]);
        }

        private static void AddVeinAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void AddEntityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void AddTechHashAwards(int[] value)
        {
            for (var i = 0; i < value.Length; i += 2)
            {
                var techState = GameMain.history.techStates[value[i]];

                if (techState.unlocked) continue;

                techState.hashUploaded += Math.Min(techState.hashNeeded - techState.hashUploaded, value[i + 1] * techState.hashNeeded / 100);
                
                GameMain.history.techStates[value[i]] = techState;
            }
        }

        private static void TriggerItemAbnormalityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void TriggerVeinAbnormalityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void TriggerEntityAbnormalityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void TriggerPlanetAbnormalityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void TriggerStarAbnormalityAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void PlanetEffectAwards(int[] value)
        {
            throw new NotImplementedException();
        }

        private static void StarEffectAwards(int[] value)
        {
            throw new NotImplementedException();
        }
    }
}
