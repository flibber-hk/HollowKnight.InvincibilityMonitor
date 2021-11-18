using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker.Actions;
using Modding;
using Vasi;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhileCollectingItem : InvincibilityCondition
    {
        private bool PickingUpItem = false;
        protected override bool ConditionActive => PickingUpItem;


        protected override void Hook()
        {
            ModHooks.Instance.HeroUpdateHook += ClearWhenHaveControl;
            Hooks.OnHeroAnimPlay += PickingUpAnimation;

            Hooks.OnFsmEnable += ModifyFsm;
        }

        private void ModifyFsm(PlayMakerFSM fsm)
        {
            if (fsm.FsmName == "Vengeful Spirit" && fsm.gameObject.name == "Vengeful Spirit")
            {
                fsm.GetState("Get").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Get Scream" && fsm.gameObject.name == "Scream Get")
            {
                fsm.GetState("Get").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Quake" && fsm.gameObject.name == "Quake Item")
            {
                fsm.GetState("Get").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Conversation Control" && fsm.gameObject.name == "Dream Nail Get")
            {
                fsm.GetState("PD Get").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Spell Control" && fsm.gameObject.name == "Knight")
            {
                fsm.GetState("SG Antic").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Ruins Shaman" && fsm.gameObject.name == "Ruins Shaman")
            {
                fsm.GetState("Yes").InsertMethod(0, () => PickingUpItem = true);
            }
            else if (fsm.FsmName == "Control" && fsm.gameObject.name == "Crystal Shaman")
            {
                fsm.GetState("Shatter").InsertMethod(0, () => PickingUpItem = true);
            }
        }

        // Cursed
        private void PickingUpAnimation(string clipName)
        {
            if (clipName.StartsWith("Collect"))
                PickingUpItem = true;
        }

        private void ClearWhenHaveControl()
        {
            if (!HeroController.instance.controlReqlinquished) PickingUpItem = false;
        }
    }
}
