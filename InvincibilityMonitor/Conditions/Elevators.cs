using System;
using System.Collections;
using System.Linq;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;

namespace InvincibilityMonitor.Conditions
{
    public enum ElevatorType
    {
        SmallCityLift,
        LargeCityLift,
        NonCityLift
    }
    public static class ElevatorTypeCalculator
    {
        public static ElevatorType GetElevatorType()
        {
            if (GameManager.instance.sceneName.StartsWith("Crossroads_49") || GameManager.instance.sceneName.StartsWith("Ruins2_10"))
                return ElevatorType.LargeCityLift;
            else if (GameManager.instance.sceneName.Contains("Ruins"))
                return ElevatorType.SmallCityLift;
            else
                return ElevatorType.NonCityLift;
        }
    }

    public class NonCityElevators : InvincibilityCondition
    {
        protected override bool ConditionActive => HeroController.instance.transform.parent != null
            && ElevatorTypeCalculator.GetElevatorType() == ElevatorType.NonCityLift;
    }

    public class SmallCityElevators : InvincibilityCondition
    {
        protected override bool ConditionActive => HeroController.instance.transform.parent != null 
            && ElevatorTypeCalculator.GetElevatorType() == ElevatorType.SmallCityLift;
    }

    public class LargeCityElevators : InvincibilityCondition
    {
        private class LeverRangeMonitor : MonoBehaviour
        {
            public static bool InElevatorRange = false;
            public static bool InElevatorScene = false;

            void OnTriggerEnter2D(Collider2D col)
            {
                if (col.name == "Hero Check")
                    InElevatorRange = true;
            }
            void OnTriggerStay2D(Collider2D col)
            {
                if (col.name == "Hero Check")
                    InElevatorRange = true;
            }
            void OnTriggerExit2D(Collider2D col)
            {
                if (col.name == "Hero Check")
                    InElevatorRange = false;
            }

            void OnDestroy()
            {
                InElevatorScene = false;
                InElevatorRange = false;
            }
        }

        protected override void Hook()
        {
            On.HeroController.Start += AddLeverRangeMonitor;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += PatchHeroCheck;
        }

        private void AddLeverRangeMonitor(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            self.gameObject.AddComponent<LeverRangeMonitor>();
        }

        private void PatchHeroCheck(Scene oldScene, Scene scene)
        {
            try
            {
                if (!scene.name.StartsWith("Crossroads_49") && !scene.name.StartsWith("Ruins2_10"))
                {
                    LeverRangeMonitor.InElevatorRange = false;
                    LeverRangeMonitor.InElevatorScene = false;
                }
                else
                {
                    LeverRangeMonitor.InElevatorScene = true;
                }
                if (!oldScene.name.StartsWith("Crossroads_49") && !oldScene.name.StartsWith("Ruins2_10"))
                    LeverRangeMonitor.InElevatorRange = false;
            }
            catch { }
        }

        protected override bool ConditionActive => LeverRangeMonitor.InElevatorRange && LeverRangeMonitor.InElevatorScene;
    }


}
