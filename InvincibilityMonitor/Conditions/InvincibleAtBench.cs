using Modding;
using Vasi;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleAtBench : InvincibilityCondition
    {
        private bool _spiderBench;
        private bool _regularBench;
        protected override bool ConditionActive => _spiderBench || _regularBench;

        protected override void Hook()
        {
            ModHooks.HeroUpdateHook += OnUpdate;
            Hooks.OnFsmEnable += PatchSpiderBench;
        }

        private void OnUpdate()
        {
            if (PlayerData.instance.GetBool(nameof(PlayerData.atBench)))
            {
                _regularBench = true;
            }
            else if (!HeroController.instance.controlReqlinquished)
            {
                _regularBench = false;
                _spiderBench = false;
            }
        }

        private void PatchSpiderBench(PlayMakerFSM fsm)
        {
            if (fsm.FsmName == "Bench Control Spider" && fsm.gameObject.scene.name == "Deepnest_Spider_Town" && fsm.gameObject.name == "RestBench Spider")
            {
                fsm.GetState("Start Rest").InsertMethod(2, () => _spiderBench = true);
            }
        }
    }
}
