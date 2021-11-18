using Modding;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleInCutscene : InvincibilityCondition
    {
        private bool _inCutscene = false;
        protected override bool ConditionActive => _inCutscene;

        protected override void Hook()
        {
            On.CinematicPlayer.TriggerStartVideo += StartedCutscene;
            ModHooks.Instance.HeroUpdateHook += ClearWhenInControl;
        }

        private void ClearWhenInControl()
        {
            if (!HeroController.instance.controlReqlinquished) _inCutscene = false;
        }

        private void StartedCutscene(On.CinematicPlayer.orig_TriggerStartVideo orig, CinematicPlayer self)
        {
            orig(self);
            _inCutscene = true;
        }
    }
}
