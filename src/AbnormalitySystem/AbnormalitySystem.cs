using DSP_AbnormalitySystem.Abnormality;
using DSP_AbnormalitySystem.UI;
using HarmonyLib;
using UnityEngine;

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

                var audio = VFAudio.Create("mecha-mining", GameMain.mainPlayer.transform, Vector3.zero);
                audio.Play();
                var audioSource = Object.Instantiate(audio.audioSource);
                audio.Stop();
                audioSource.clip = AudioClip;
                audioSource.PlayOneShot(AudioClip, audioSource.volume);

                TriggerPlanetAbnormality(planet);
            }
        }

        private static void TriggerPlanetAbnormality(PlanetData planet)
        {
            var planetAbnormalitySubType = planet.type == EPlanetType.Gas ? PlanetAbnormalitySubType.GasGiant : PlanetAbnormalitySubType.Terrestrial;

            Abnormality.Abnormality abnormality = PlanetAbnormalities[planetAbnormalitySubType].GetRandomAbnormality();

            _uiAbnormalityWindow.SetAbnormality(planet, abnormality);
        }
    }
}
