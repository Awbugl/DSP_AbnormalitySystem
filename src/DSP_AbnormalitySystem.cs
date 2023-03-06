using System.IO;
using BepInEx;
using HarmonyLib;
using crecheng.DSPModSave;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBeInternal
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace DSP_AbnormalitySystem
{
    [BepInPlugin(MODGUID, MODNAME, VERSION)]
    public class CustomAbnormalitySystem : BaseUnityPlugin, IModCanSave
    {
        public const string MODGUID = "top.awbugl.DSP.CustomAbnormalitySystem";
        public const string MODNAME = "CustomAbnormalitySystem";
        public const string VERSION = "1.0.0";

        private void Awake()
        {
            AbnormalitySystem.Logger = Logger;
            Harmony.CreateAndPatchAll(typeof(AbnormalitySystem), MODGUID);
            Logger.LogInfo("CustomAbnormalitySystem Abnormalities:" + AbnormalitySystem.Abnormalities.Count);
        }

        public void Export(BinaryWriter w) => AbnormalitySystem.Export(w);

        public void Import(BinaryReader r) => AbnormalitySystem.Import(r);

        public void IntoOtherSave() => AbnormalitySystem.IntoOtherSave();
    }
}
