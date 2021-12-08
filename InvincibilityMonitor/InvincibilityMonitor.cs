using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using MonoMod.ModInterop;
using UnityEngine;

namespace InvincibilityMonitor
{
    public class InvincibilityMonitor : Mod, IGlobalSettings<GlobalSettings>
    {
        internal static InvincibilityMonitor Instance;

        public InvincibilityMonitor() : base(null) { Instance = this; }

        public static GlobalSettings GS;
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        public override string GetVersion()
        {
            return Vasi.VersionUtil.GetVersion<InvincibilityMonitor>();
        }

        public override void Initialize()
        {
            // Doesn't change anything
            DebugMod.CreateSimpleInfoPanel("InvincibilityMonitor.Info", 220);

            DebugMod.AddInfoToSimplePanel("InvincibilityMonitor.Info", "Invincibility State", GetInvincibilityState);
            DebugMod.AddInfoToSimplePanel("InvincibilityMonitor.Info", null, null);

            InvincibilityCondition.Setup();
        }

        public string GetInvincibilityState()
        {
            return InvincibilityCondition.AnyConditionInvincible ? "Invincible"
                : InvincibilityCondition.TimerSafety ? "Waiting"
                : "Vulnerable";
        }
    }

    internal static class DebugMod
    {
        [ModImportName("DebugMod")]
        private static class DebugImport
        {
            public static Func<bool, string> GetStringForBool = null;
            public static Action<string, bool> CreateCustomInfoPanel = null;
            public static Action<string, float, float, float, string, Func<string>> AddInfoToPanel = null;
            public static Action<string, float> CreateSimpleInfoPanel = null;
            public static Action<string, string, Func<string>> AddInfoToSimplePanel = null;
        }
        static DebugMod()
        {
            typeof(DebugImport).ModInterop();
        }

        public static string GetStringForBool(bool b) => DebugImport.GetStringForBool?.Invoke(b) ?? "";

        public static void CreateCustomInfoPanel(string Name, bool ShowSprite)
            => DebugImport.CreateCustomInfoPanel?.Invoke(Name, ShowSprite);

        public static void AddInfoToPanel(string Name, float xLabel, float xInfo, float y, string label, Func<string> textFunc)
            => DebugImport.AddInfoToPanel?.Invoke(Name, xLabel, xInfo, y, label, textFunc);

        public static void CreateSimpleInfoPanel(string Name, float sep)
            => DebugImport.CreateSimpleInfoPanel?.Invoke(Name, sep);

        public static void AddInfoToSimplePanel(string Name, string label, Func<string> textFunc)
            => DebugImport.AddInfoToSimplePanel?.Invoke(Name, label, textFunc);
    }
}