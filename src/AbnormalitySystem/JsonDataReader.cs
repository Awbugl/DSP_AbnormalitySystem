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
        internal static Dictionary<Abnormality.AbnormalityType, List<Abnormality.Abnormality>> Abnormalities { get; }

        private static Dictionary<Abnormality.PlanetAbnormalitySubType, List<Abnormality.PlanetAbnormality>> PlanetAbnormalities { get; }

        private static Dictionary<Abnormality.StarAbnormalitySubType, List<Abnormality.StarAbnormality>> StarAbnormalities { get; }

        private static T GetRandomAbnormality<T>(this List<T> list) where T : Abnormality.Abnormality => list[Random.Next(list.Count)];

        static AbnormalitySystem()
        {
            AudioClip = AssetBundle
                       .LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "advisor_voice"))
                       .LoadAsset<AudioClip>("assets/voice/materialist.wav");

            var jsonDir = Path.Combine(Paths.ConfigPath, "CustomAbnormalitySystem", "abnormalities");
            Directory.CreateDirectory(jsonDir);

            Abnormalities = new Dictionary<Abnormality.AbnormalityType, List<Abnormality.Abnormality>>();
            PlanetAbnormalities = new Dictionary<Abnormality.PlanetAbnormalitySubType, List<Abnormality.PlanetAbnormality>>();
            StarAbnormalities = new Dictionary<Abnormality.StarAbnormalitySubType, List<Abnormality.StarAbnormality>>();

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var file in Directory.GetFiles(jsonDir, "*.json"))
            {
                var abnormality = JsonConvert.DeserializeObject<Abnormality.Abnormality>(File.ReadAllText(file));

                switch (abnormality.Type)
                {
                    case Abnormality.AbnormalityType.Planet:
                    {
                        var planetAbnormality = abnormality as Abnormality.PlanetAbnormality;

                        if (planetAbnormality == null) throw new InvalidDataException("Planet abnormality is null");

                        var planetAbnormalitySubType = (Abnormality.PlanetAbnormalitySubType)planetAbnormality.SubType;

                        if (!PlanetAbnormalities.ContainsKey(planetAbnormalitySubType))
                            PlanetAbnormalities.Add(planetAbnormalitySubType, new List<Abnormality.PlanetAbnormality>());

                        PlanetAbnormalities[planetAbnormalitySubType].Add(planetAbnormality);

                        break;
                    }

                    case Abnormality.AbnormalityType.Star:
                    {
                        var starAbnormality = abnormality as Abnormality.StarAbnormality;

                        if (starAbnormality == null) throw new InvalidDataException("Star abnormality is null");

                        var starAbnormalitySubType = (Abnormality.StarAbnormalitySubType)starAbnormality.SubType;

                        if (!StarAbnormalities.ContainsKey(starAbnormalitySubType))
                            StarAbnormalities.Add(starAbnormalitySubType, new List<Abnormality.StarAbnormality>());

                        StarAbnormalities[starAbnormalitySubType].Add(starAbnormality);

                        break;
                    }

                    default:
                    {
                        if (!Abnormalities.ContainsKey(abnormality.Type)) Abnormalities.Add(abnormality.Type, new List<Abnormality.Abnormality>());

                        break;
                    }
                }

                Abnormalities[abnormality.Type].Add(abnormality);
            }
        }
    }
}
