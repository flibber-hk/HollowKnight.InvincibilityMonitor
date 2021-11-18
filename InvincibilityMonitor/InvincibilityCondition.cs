using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Modding;
using UnityEngine;

namespace InvincibilityMonitor
{
    public abstract class InvincibilityCondition
    {
        // Return true to cause the player to become invincible from this condition
        protected abstract bool ConditionActive { get; }
        // Checks whether the condition is enabled in the dictionary
        private bool ConditionEnabled
        {
            get
            {
                string key = GetType().Name;
                if (InvincibilityMonitor.GS.EnabledConditions.TryGetValue(key, out bool val))
                    return val;
                InvincibilityMonitor.GS.EnabledConditions[key] = true;
                return true;
            }
        }
        private bool Invincible => ConditionEnabled && ConditionActive;

        // Apply any hooks required
        protected virtual void Hook() { }

        public static List<InvincibilityCondition> ActiveConditions = new List<InvincibilityCondition>();
        public static IEnumerable<string> GetCurrentlyInvincibleConditions()
        {
            foreach (InvincibilityCondition condition in ActiveConditions)
            {
                if (condition.Invincible)
                    yield return condition.GetType().Name;
            }
        }

        public static bool AnyConditionInvincible
        {
            get
            {
                try
                {
                    return ActiveConditions.Any(x => x.Invincible);
                }
                catch
                {
                    return false;
                }
            }
        }
        public static float InvincibilityDelay = 0f;
        public static bool TimerSafety => InvincibilityDelay < InvincibilityMonitor.GS.LeniencyTime;

        public static void Setup()
        {
            foreach (Type t in typeof(InvincibilityCondition).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(InvincibilityCondition)) && !x.IsAbstract && x.Namespace == "InvincibilityMonitor.Conditions"))
            {
                InvincibilityCondition condition = (InvincibilityCondition)Activator.CreateInstance(t);
                condition.Hook();
                ActiveConditions.Add(condition);
            }

            ModHooks.Instance.HeroUpdateHook += OnHeroUpdate;
            ModHooks.Instance.GetPlayerBoolHook += SetInvincibility;
        }

        private static bool SetInvincibility(string originalSet)
        {
            bool internalValue = PlayerData.instance.GetBoolInternal(originalSet);
            if (originalSet == nameof(PlayerData.isInvincible))
            {
                return internalValue || TimerSafety;
            }
            return internalValue;
        }

        private static void OnHeroUpdate()
        {
            if (AnyConditionInvincible)
            {
                InvincibilityDelay = 0;
            }
            else
            {
                InvincibilityDelay += Time.deltaTime;
            }
        }
    }
}
