using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
using DSP_AbnormalitySystem.Abnormality;
using DSP_AbnormalitySystem.UI;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable Unity.UnknownResource

namespace DSP_AbnormalitySystem
{
    public static class AbnormalitySystemPatches
    {
        private static HashSet<int> _landedPlanets = new HashSet<int>();
        private static int _birthPlanetId = -1;
        private static AudioClip AudioClip;
        private static UIAbnormalityWindow UIAbnormalityWindow;
        private static readonly DotNet35Random Random = new DotNet35Random();

        internal static ManualLogSource Logger;

        private static readonly bool _devmode = true;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGame), "_OnCreate")]
        public static void Init(UIGame __instance)
        {
            UIAbnormalityWindow = UIAbnormalityWindow.CreateWindow();

            AudioClip ConvertBytesToClip(byte[] rawData)
            {
                var samples = new float[rawData.Length / 2];
                float rescaleFactor = 32767;

                for (var i = 0; i < rawData.Length; i += 2)
                {
                    var st = BitConverter.ToInt16(rawData, i);
                    var ft = st / rescaleFactor;
                    samples[i / 2] = ft;
                }

                var audioClip = AudioClip.Create("audioClip", samples.Length, 1, 44100, false);
                audioClip.SetData(samples, 0);

                return audioClip;
            }

            var memoryStream = new MemoryStream();
            Assembly.GetExecutingAssembly().GetManifestResourceStream("DSP_AbnormalitySystem.dependencies.abnormality_materialist.wav")
                   ?.CopyTo(memoryStream);

            AudioClip = ConvertBytesToClip(memoryStream.ToArray());
        }

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

            if (_devmode || Random.NextDouble() < 0.15f)
            {
                Logger.LogInfo($"Trigger abnormality on planet {planet.id}");

                // doesn't work, weird
                var audioSource = AccessTools.FieldRefAccess<VFAudio>(typeof(PlayerAudio), "driftAudio")(GameMain.mainPlayer.audio).audioSource;
                audioSource.PlayOneShot(AudioClip);

                // generate by Copilot
                var abnormality = new PlanetAbnormality()
                                  {
                                      Name = "Materialist",
                                      Description = "This planet is materialist",
                                      Type = AbnormalityType.Planet,
                                      Effects = new Effect[]
                                                {
                                                    new Effect()
                                                    {
                                                        Name = "Good.",
                                                        Description = "Get a water.",
                                                        Type = EffectType.AddItem,
                                                        Value = new int[] { 1000, 1 }
                                                    },
                                                    new Effect()
                                                    {
                                                        Name = "Good.",
                                                        Description = "Get a water.",
                                                        Type = EffectType.AddItem,
                                                        Value = new int[] { 1000, 1 }
                                                    },
                                                    new Effect()
                                                    {
                                                        Name = "Good.",
                                                        Description = "Get a water.",
                                                        Type = EffectType.AddItem,
                                                        Value = new int[] { 1000, 1 }
                                                    }
                                                },
                                      Translations = new StringProto[]
                                                     {
                                                         new StringProto() { Name = "Materialist", ENUS = "Materialist", ZHCN = "物质主义者" },
                                                         new StringProto()
                                                         {
                                                             Name = "This planet is materialist",
                                                             ENUS = "This planet is materialist",
                                                             ZHCN = "这个星球是物质主义者"
                                                         },
                                                         new StringProto() { Name = "Good.", ENUS = "Good.", ZHCN = "好的" },
                                                         new StringProto() { Name = "Get a water.", ENUS = "Get a water.", ZHCN = "获得一瓶水" }
                                                     }
                                  };

                TriggerPlanetAbnormality(planet, abnormality);
            }
        }

        private static void TriggerPlanetAbnormality(PlanetData planet, Abnormality.Abnormality abnormality)
            => UIAbnormalityWindow.SetAbnormality(planet, abnormality);

        internal static void Export(BinaryWriter w)
        {
            w.Write(_landedPlanets.Count);

            foreach (var planetId in _landedPlanets) w.Write(planetId);
        }

        internal static void Import(BinaryReader r)
        {
            ReInitAll();

            var landedPlanetscount = r.ReadInt32();

            for (var j = 0; j < landedPlanetscount; j++) _landedPlanets.Add(r.ReadInt32());
        }

        internal static void IntoOtherSave() => ReInitAll();

        private static void ReInitAll()
        {
            _landedPlanets = new HashSet<int>();
            _birthPlanetId = -1;
        }
    }
}
