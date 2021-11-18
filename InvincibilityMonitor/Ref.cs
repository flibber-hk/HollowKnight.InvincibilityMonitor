using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InvincibilityMonitor
{
    public class Cached<T> where T : UnityEngine.Object
    {
        private readonly Func<T> captureObject;
        public Cached(Func<T> captureObject)
        {
            this.captureObject = captureObject;
        }
        private T _value;
        public T Value
        {
            get
            {
                if (_value != null) return _value;
                _value = captureObject();
                if (_value == null) return null;
                return _value;
            }
        }
    }

    public static class Ref
    {
        private static HeroAnimationController captureHAC() => HeroController.instance.GetComponent<HeroAnimationController>();
        public static Cached<HeroAnimationController> HAC = new Cached<HeroAnimationController>(captureHAC);
        
        private static Transform captureDM() => GameCameras.instance.hudCamera.transform.Find("DialogueManager");
        public static Cached<Transform> DialogueManager = new Cached<Transform>(captureDM);

        public static Cached<PlayMakerFSM> BoxOpen = new Cached<PlayMakerFSM>(() => DialogueManager.Value.gameObject.LocateMyFSM("Box Open"));
        public static Cached<PlayMakerFSM> BoxOpenYN = new Cached<PlayMakerFSM>(() => DialogueManager.Value.gameObject.LocateMyFSM("Box Open YN"));
        public static Cached<PlayMakerFSM> BoxOpenDream = new Cached<PlayMakerFSM>(() => DialogueManager.Value.gameObject.LocateMyFSM("Box Open Dream"));
    }
}
