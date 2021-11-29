using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cmCam;
    private float shakeTimer;
    private float shakeTimeTotal;
    private void Awake()
    {
        Instance = this;
        cmCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmMCP = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmMCP.m_AmplitudeGain = intensity;

        shakeTimer = time;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
        }
        shakeTimer -= Time.deltaTime;
        if(shakeTimer <= 0f) {
            CinemachineBasicMultiChannelPerlin cmMCP = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cmMCP.m_AmplitudeGain = 0f;
        }
    }
}
