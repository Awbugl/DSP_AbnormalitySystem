using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace DSP_AbnormalitySystem
{
    public static partial class AbnormalitySystem
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGame), "_OnCreate")]
        public static void Init(UIGame __instance) => _uiAbnormalityWindow = UIAbnormalityWindow.CreateWindow();

        [HarmonyPatch(typeof(GameScenarioLogic), "NotifyOnLandPlanet")]
        [HarmonyPostfix]
        public static void GameScenarioLogic_NotifyOnLandPlanet_Postfix(PlanetData planet)
        {
            Logger.LogInfo($"Landed on planet {planet.id}");

            if (_birthPlanetId < 0)
            {
                _birthPlanetId = planet.galaxy.birthPlanetId;
                _landedPlanets.Add(_birthPlanetId);
            }

            if (_landedPlanets.Contains(planet.id)) return;

            _landedPlanets.Add(planet.id);

            if (Devmode || Random.NextDouble() < 0.15f)
            {
                Logger.LogInfo($"Trigger abnormality on planet {planet.id}");

                PlayAudio();

                TriggerPlanetAbnormality(planet);
            }
        }

        private static void PlayAudio() => BGMController.instance.musics[BGMController.BGM].PlayOneShot(AudioClip, VFAudio.finalSoundVolume);

        private static void TriggerPlanetAbnormality(PlanetData planet)
        {
            try
            {
                var planetAbnormalitySubType
                    = planet.type == EPlanetType.Gas ? PlanetAbnormalitySubType.GasGiant : PlanetAbnormalitySubType.Terrestrial;

                var abnormality = PlanetAbnormalities[planetAbnormalitySubType].GetRandomAbnormality();

                _uiAbnormalityWindow.SetAbnormality(planet, abnormality);
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to trigger abnormality on planet {planet.id}: {e}");
            }
        }
    }
}
