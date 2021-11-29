using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
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
            InvincibilityCondition.Setup();
        }

        /*
        private void GeoCounter_Update(On.GeoCounter.orig_Update orig, GeoCounter self)
        {
            orig(self);
            self.geoTextMesh.color = InvincibilityCondition.AnyConditionInvincible ? Color.red
                : InvincibilityCondition.TimerSafety ? Color.yellow
                : Color.green;

            if (Input.GetKeyDown(KeyCode.P))
            {
                Log(InvincibilityCondition.AnyConditionInvincible ? "Invincible"
                : InvincibilityCondition.TimerSafety ? "Waiting"
                : "Vulnerable");

                foreach (string s in InvincibilityCondition.GetCurrentlyInvincibleConditions())
                {
                    Log("INVINCIBLE DUE TO " + s);
                }
            }
        }
        */
    }
}
