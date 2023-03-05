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
            AbnormalitySystemPatches.Logger = Logger;
            Harmony.CreateAndPatchAll(typeof(AbnormalitySystemPatches), MODGUID);
        }

        public void Export(BinaryWriter w) => AbnormalitySystemPatches.Export(w);

        public void Import(BinaryReader r) => AbnormalitySystemPatches.Import(r);

        public void IntoOtherSave() => AbnormalitySystemPatches.IntoOtherSave();
    }
}
