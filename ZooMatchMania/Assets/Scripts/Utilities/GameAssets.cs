using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class GameAssets : Singleton<GameAssets>
    {
        public SoundAudioClip[] soundAudioClipArray;

        [System.Serializable]
        public class SoundAudioClip
        {
            public SoundManager.SoundType soundType;
            public AudioClip audioClip;
        }

        protected override void Awake() {
            base.Awake();
            SoundManager.Initialize();
        }
    }