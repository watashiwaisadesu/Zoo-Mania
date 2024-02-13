using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace NifuDev
{
    public class CinemachineShake : Singleton<CinemachineShake>
    {

        private CinemachineVirtualCamera _cinemachineVCam;

        private float shakeTimer;
        private float shakeTimerTotal;
        private float startingIntensity;

        private void Start() {
            _cinemachineVCam = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (shakeTimer > 0f)
            {
                shakeTimer -= Time.deltaTime;

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
           _cinemachineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0f, (1f - (shakeTimer / shakeTimerTotal)));
            }
        }

        public void ShakeCamera(Vector2 cameraShakeParams)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                _cinemachineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = cameraShakeParams.x;

            startingIntensity = cameraShakeParams.x;
            shakeTimer = cameraShakeParams.y;
            shakeTimerTotal = cameraShakeParams.y;
        }
    }
}

