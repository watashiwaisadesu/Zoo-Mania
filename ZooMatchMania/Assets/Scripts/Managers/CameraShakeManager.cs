using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class CameraShakeManager : Singleton<CameraShakeManager>
    {
        [SerializeField] private CameraShakeType[] cameraShakeParameterArray;

        public Vector2 GetCameraShakeParameter(ShakeType shakeType) {
            return cameraShakeParameterArray[(int)shakeType].ShakeIntensityAndDuration;
        }

        protected override void Awake() {
            base.Awake();
        }
    }
    [HideInInspector]
    public enum ShakeType
    {
        PlayerDamaged,
        EnemyKilled,
        BondSevered
    }

    [System.Serializable]
    public class CameraShakeType
    {
        public ShakeType ShakeType;

        public Vector2 ShakeIntensityAndDuration;
    }
}
