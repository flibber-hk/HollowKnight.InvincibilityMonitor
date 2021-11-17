using GlobalEnums;

namespace InvincibilityMonitor.Conditions
{
    public class TransitionInvincibility : InvincibilityCondition
    {
        protected override bool ConditionActive => HeroController.instance.transitionState != HeroTransitionState.WAITING_TO_TRANSITION;
    }
}
