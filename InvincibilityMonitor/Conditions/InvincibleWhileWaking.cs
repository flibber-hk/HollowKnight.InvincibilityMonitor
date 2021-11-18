namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhileWaking : InvincibilityCondition
    {
        private bool Prostrated = false;

        private bool WakingAnim => Ref.HAC.Value.animator.IsPlaying("Wake Up Ground")
            || Ref.HAC.Value.animator.IsPlaying("Wake")
            || Ref.HAC.Value.animator.IsPlaying("Respawn Wake")
            || Ref.HAC.Value.animator.IsPlaying("Prostrate Rise");

        protected override bool ConditionActive => WakingAnim || Prostrated;

        protected override void Hook()
        {
            Hooks.OnHeroAnimPlay += SetProstrated;
        }

        private void SetProstrated(string clipName)
        {
            if (clipName == "Prostrate")
                Prostrated = true;
            else if (clipName == "Prostrate Rise" || clipName == "Idle" || clipName == "Run")
                Prostrated = false;
        }
    }
}
