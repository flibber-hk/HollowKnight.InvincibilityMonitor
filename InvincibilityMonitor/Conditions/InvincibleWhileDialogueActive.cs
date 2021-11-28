using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using UnityEngine;
using Vasi;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhileDialogueActive : InvincibilityCondition
    {
        public bool ShownDialogue = false;
        protected override bool ConditionActive => ShownDialogue || AnyDialogueActive;

        protected override void Hook()
        {
            On.InvAnimateUpAndDown.AnimateUp += OnShowDialogue;
            ModHooks.HeroUpdateHook += HiddenDialogue;
            Hooks.OnFsmEnable += EditStagControl;
            Hooks.OnFsmEnable += EditShopRegion;
            Hooks.OnHeroAnimPlay += DialogueAnim;
        }

        private void EditShopRegion(PlayMakerFSM fsm)
        {
            if (fsm.FsmName == "Shop Region")
            {
                fsm.GetState("Take Control").AddMethod(() => ShownDialogue = true);
            }
        }

        public bool AnyDialogueActive => ((Ref.BoxOpen.Value.ActiveStateName == "Box Up")
            || (Ref.BoxOpenYN.Value.ActiveStateName == "Box Up")
            || (Ref.BoxOpenDream.Value.ActiveStateName == "Box Up")) && HeroController.instance.controlReqlinquished;

        private void DialogueAnim(string clipName)
        {
            if (clipName == "TurnToBG")
                ShownDialogue = true;
        }
        private void HiddenDialogue()
        {
            if (!HeroController.instance.controlReqlinquished)
                ShownDialogue = false;
        }
        private void EditStagControl(PlayMakerFSM fsm)
        {
            if (fsm.FsmName == "Stag Control")
            {
                fsm.GetState("Take Control").AddMethod(() => ShownDialogue = true);
            }
        }
        private void OnShowDialogue(On.InvAnimateUpAndDown.orig_AnimateUp orig, InvAnimateUpAndDown self)
        {
            orig(self);
            if (InventoryObject(self.transform)) return;
            if (self.gameObject.scene.name.StartsWith("Grimm_Nightmare")) return;
            ShownDialogue = true;
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
