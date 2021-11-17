using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvincibilityMonitor.Conditions
{
    public class InvincibleWhileRoarLocked : InvincibilityCondition
    {
        private static readonly HashSet<string> RoarStates = new HashSet<string>()
        {
            "Lock Start",
            "Flip",
            "Set Push Direction",
            "Check On Ground",
            "Lock Grounded",
            "Lock Air"
        };

        private PlayMakerFSM _roarFsm;
        private PlayMakerFSM roarFsm
        {
            get
            {
                if (_roarFsm != null) return _roarFsm;
                _roarFsm = HeroController.instance.gameObject.LocateMyFSM("Roar Lock");
                if (_roarFsm == null) return null;
                return _roarFsm;
            }
        }

        protected override bool ConditionActive => RoarStates.Contains(roarFsm?.ActiveStateName ?? string.Empty);
    }
}
