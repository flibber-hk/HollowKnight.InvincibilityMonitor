using System.Collections;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            ModHooks.Instance.HeroUpdateHook += OnUpdate;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += PatchSpiderBench;
        }

        private void OnUpdate()
        {
            if (!HeroController.instance.controlReqlinquished) _spiderBench = false;
        }

        private void PatchSpiderBench(Scene scene, LoadSceneMode lsm)
        {
            if (scene.name == "Deepnest_Spider_Town")
            {
                GameManager.instance.StartCoroutine(PatchSpiderBenchDelay());
            }

            if (PlayerData.instance.GetBool(nameof(PlayerData.atBench)))
            {
                _regularBench = true;
            }
            else if (!HeroController.instance.controlReqlinquished)
            {
                _regularBench = false;
            }
        }

        private IEnumerator PatchSpiderBenchDelay()
        {
            yield return null;

            GameObject spiderBench = GameObject.Find("RestBench Spider");
            spiderBench.LocateMyFSM("Bench Control Spider").GetState("Start Rest").InsertMethod(2, () => _spiderBench = true);
            // spiderBench.LocateMyFSM("Bench Control Spider").GetState("Start Rest").RemoveAction<HutongGames.PlayMaker.Actions.SendEventByName>();
        }
    }
}
