using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;

namespace InvincibilityMonitor
{
    public class GlobalSettings
    {
        public float LeniencyTime = 0.2f;
        // public bool DebugInfo = false;

        public Dictionary<string, bool> EnabledConditions = new Dictionary<string, bool>();
    }
}
