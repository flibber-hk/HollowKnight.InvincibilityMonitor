using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhenFocusingTHK : InvincibilityCondition
    {
        public static readonly HashSet<string> DreamFocusStates = new HashSet<string>()
        {
            "Focus Start D",
            "Focus D",
            "Focus Cancel D",
            "Keep Focus",
            "Dream Focus End"
        };

        protected override bool ConditionActive => DreamFocusStates.Contains(HeroController.instance.spellControl.ActiveStateName);

    }
}
