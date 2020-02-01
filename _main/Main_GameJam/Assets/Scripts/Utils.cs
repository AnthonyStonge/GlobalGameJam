using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public static CinemachineVirtualCamera VirtualCamera;
    private static CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Start()
    {
        VirtualCamera = GameObject.FindWithTag("CamController").GetComponent<CinemachineVirtualCamera>();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(shake(0.3f, 1.2f, 0.5f));
        }
    }

    // Start is called before the first frame update
    public static IEnumerator shake(float ShakeAmplitude2, float ShakeFrequency2,float ShakeDuration2)
    {
        while (ShakeDuration2 > 0f)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude2;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency2;
            ShakeDuration2 -= Time.deltaTime;
            yield return null;
        }
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }
}
