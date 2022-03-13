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

        public static GlobalSettings GS = new();
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
}