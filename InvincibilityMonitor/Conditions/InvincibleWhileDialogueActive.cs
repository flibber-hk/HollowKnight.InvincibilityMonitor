using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhileDialogueActive : InvincibilityCondition
    {
        public static int DialogueCount = 0;
        protected override bool ConditionActive => DialogueCount > 0;

        protected override void Hook()
        {
            On.InvAnimateUpAndDown.AnimateUp += InvAnimateUpAndDown_AnimateUp;
            On.InvAnimateUpAndDown.AnimateDown += InvAnimateUpAndDown_AnimateDown;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            DialogueCount = 0;
        }

        private void InvAnimateUpAndDown_AnimateDown(On.InvAnimateUpAndDown.orig_AnimateDown orig, InvAnimateUpAndDown self)
        {
            orig(self);
            if (InventoryObject(self.transform)) return;
            DialogueCount = Math.Max(DialogueCount - 1, 0);
        }

        private void InvAnimateUpAndDown_AnimateUp(On.InvAnimateUpAndDown.orig_AnimateUp orig, InvAnimateUpAndDown self)
        {
            orig(self);
            if (InventoryObject(self.transform)) return;
            DialogueCount++;
        }

        private bool InventoryObject(Transform t)
        {
            if (t.name == "Inventory") return true;
            while (t.parent != null)
            {
                t = t.parent;
                if (t.name == "Inventory") return true;
            }
            return false;
        }
    }
}
