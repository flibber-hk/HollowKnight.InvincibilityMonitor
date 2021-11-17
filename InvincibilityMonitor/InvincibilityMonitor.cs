﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using UnityEngine;

namespace InvincibilityMonitor
{
    public class InvincibilityMonitor : Mod
    {
        internal static InvincibilityMonitor Instance;

        public InvincibilityMonitor() : base(null) { Instance = this; }

        public override ModSettings GlobalSettings 
        { 
            get => GS;
            set => GS = value as GlobalSettings;
        }
        public static GlobalSettings GS = new GlobalSettings();

        public override string GetVersion()
        {
            return Vasi.VersionUtil.GetVersion<InvincibilityMonitor>();
        }

        public override void Initialize()
        {
            InvincibilityCondition.Setup();

            if (GS.DebugInfo)
            {
                On.GeoCounter.Update += GeoCounter_Update;
            }
        }

        private void GeoCounter_Update(On.GeoCounter.orig_Update orig, GeoCounter self)
        {
            orig(self);
            self.geoTextMesh.color = InvincibilityCondition.AnyConditionInvincible ? Color.red
                : InvincibilityCondition.TimerSafety ? Color.yellow
                : Color.green;

            if (Input.GetKeyDown(KeyCode.P))
            {
                Log(Conditions.InvincibleWhileDialogueActive.DialogueCount);

                Log(InvincibilityCondition.AnyConditionInvincible ? "Invincible"
                : InvincibilityCondition.TimerSafety ? "Waiting"
                : "Vulnerable");
            }
        }
    }
}