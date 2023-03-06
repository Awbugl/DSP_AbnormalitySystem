using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using Newtonsoft.Json;
using UnityEngine;

namespace DSP_AbnormalitySystem
{
    public static partial class AbnormalitySystem
    {
        internal static Dictionary<AbnormalityType, List<Abnormality>> Abnormalities { get; }

        private static Dictionary<PlanetAbnormalitySubType, List<Abnormality>> PlanetAbnormalities { get; }

        private static Dictionary<StarAbnormalitySubType, List<Abnormality>> StarAbnormalities { get; }

        private static T GetRandomAbnormality<T>(this List<T> list) where T : Abnormality => list[Random.Next(list.Count)];

        static AbnormalitySystem()
        {
            AudioClip = AssetBundle
                       .LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "advisor_voice"))
                       .LoadAsset<AudioClip>("assets/voice/materialist.wav");

            var jsonDir = Path.Combine(Paths.ConfigPath, "CustomAbnormalitySystem", "abnormalities");
            Directory.CreateDirectory(jsonDir);

            Abnormalities = new Dictionary<AbnormalityType, List<Abnormality>>();
            PlanetAbnormalities = new Dictionary<PlanetAbnormalitySubType, List<Abnormality>>();
            StarAbnormalities = new Dictionary<StarAbnormalitySubType, List<Abnormality>>();

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var file in Directory.GetFiles(jsonDir, "*.json"))
            {
                var abnormality = JsonConvert.DeserializeObject<Abnormality>(File.ReadAllText(file));

                switch (abnormality.Type)
                {
                    case AbnormalityType.Planet:
                    {
                        var planetAbnormalitySubType = (PlanetAbnormalitySubType)abnormality.SubType;

                        if (!PlanetAbnormalities.ContainsKey(planetAbnormalitySubType))
                            PlanetAbnormalities.Add(planetAbnormalitySubType, new List<Abnormality>());

                        PlanetAbnormalities[planetAbnormalitySubType].Add(abnormality);

                        goto default;
                    }

                    case AbnormalityType.Star:
                    {
                        var starAbnormalitySubType = (StarAbnormalitySubType)abnormality.SubType;

                        if (!StarAbnormalities.ContainsKey(starAbnormalitySubType))
                            StarAbnormalities.Add(starAbnormalitySubType, new List<Abnormality>());

                        StarAbnormalities[starAbnormalitySubType].Add(abnormality);

                        goto default;
                    }

                    default:
                    {
                        if (!Abnormalities.ContainsKey(abnormality.Type)) Abnormalities.Add(abnormality.Type, new List<Abnormality>());
                        Abnormalities[abnormality.Type].Add(abnormality);

                        continue;
                    }
                }
            }
        }
    }
}
