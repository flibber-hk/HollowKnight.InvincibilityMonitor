using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvincibilityMonitor
{
    public static class Hooks
    {
        #region PFSM onEnable
        private static event Action<PlayMakerFSM> _onFsmEnable;
        public static event Action<PlayMakerFSM> OnFsmEnable
        {
            add
            {
                if (_onFsmEnable == null) On.PlayMakerFSM.OnEnable += ModifyFsm;
                _onFsmEnable += value;
            }
            remove
            {
                _onFsmEnable -= value;
                if (_onFsmEnable == null) On.PlayMakerFSM.OnEnable -= ModifyFsm;
            }
        }
        private static void ModifyFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            foreach (Action<PlayMakerFSM> toInvoke in _onFsmEnable.GetInvocationList())
            {
                try
                {
                    toInvoke(self);
                }
                catch (Exception ex)
                {
                    InvincibilityMonitor.Instance.LogError($"Error invoking subscriber to OnFsmEnable hook:" + ex);
                }
            }
        }
        #endregion

        #region Hero Start
        private static event Action<HeroController> _onHeroStart;
        public static event Action<HeroController> OnHeroStart
        {
            add
            {
                if (_onHeroStart == null) On.HeroController.Start += ModifyHero;
                _onHeroStart += value;
                // if (HeroController.instance != null) value.Invoke(HeroController.instance);
            }
            remove
            {
                _onHeroStart -= value;
                if (_onHeroStart == null) On.HeroController.Start -= ModifyHero;
            }
        }
        private static void ModifyHero(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            foreach (Action<HeroController> toInvoke in _onHeroStart.GetInvocationList())
            {
                try
                {
                    toInvoke(self);
                }
                catch (Exception ex)
                {
                    InvincibilityMonitor.Instance.LogError($"Error invoking subscriber to OnHeroStart hook:" + ex);
                }
            }
        }
        #endregion

        #region Play Hero Anim
        private static event Action<string> _onHeroAnimPlay;
        public static event Action<string> OnHeroAnimPlay
        {
            add
            {
                // All overloads of this and related methods pass through this one
                if (_onHeroAnimPlay == null) On.tk2dSpriteAnimator.Play_tk2dSpriteAnimationClip_float_float += CheckHeroAnim;
                _onHeroAnimPlay += value;
            }
            remove
            {
                _onHeroAnimPlay -= value;
                if (_onHeroAnimPlay == null) On.tk2dSpriteAnimator.Play_tk2dSpriteAnimationClip_float_float -= CheckHeroAnim;
            }
        }
        private static void CheckHeroAnim(On.tk2dSpriteAnimator.orig_Play_tk2dSpriteAnimationClip_float_float orig, 
            tk2dSpriteAnimator self, tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
        {
            if (self.GetComponent<HeroController>() == null)
            {
                orig(self, clip, clipStartTime, overrideFps);
                return;
            }

            foreach (Action<string> toInvoke in _onHeroAnimPlay.GetInvocationList())
            {
                try
                {
                    toInvoke(clip.name);
                }
                catch (Exception ex)
                {
                    InvincibilityMonitor.Instance.LogError($"Error invoking subscriber to OnHeroAnimPlay hook:" + ex);
                }
            }

            orig(self, clip, clipStartTime, overrideFps);
        }
        #endregion
    }
}
