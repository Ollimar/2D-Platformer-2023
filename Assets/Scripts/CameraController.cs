using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public CinemachineVirtualCamera basic2dCamera;
    public CinemachineFramingTransposer cameraSettings;
    public CinemachineBasicMultiChannelPerlin shakeSettings;

    // Screenshake parameters
    public float duration = 0.5f;
    public float amplitude = 2f;
    public float frequency = 2f;

    // Start is called before the first frame update
    void Start()
    {
        basic2dCamera = GetComponent<CinemachineVirtualCamera>();
        cameraSettings = basic2dCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        shakeSettings = basic2dCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //Esimerkki ruudun offsetista. Vaihda sopiva arvo
        cameraSettings.m_ScreenX = 0.5f;
        shakeSettings.m_AmplitudeGain = 0f;
    }

    public void ScreenShake(float myFrequency, float myDuration, float myAmplitude)
    {
        StartCoroutine(ScreenShakeStart(myFrequency, myDuration, myAmplitude));
    }

    public IEnumerator ScreenShakeStart(float myFrequency, float myDuration, float myAmplitude)
    {
        shakeSettings.m_AmplitudeGain = myAmplitude;
        shakeSettings.m_FrequencyGain = myFrequency;
        yield return new WaitForSeconds(myDuration);
        shakeSettings.m_AmplitudeGain = 0f;
    }

    private void Update()
    {

    }

    /*
    public void FlipScreenX(bool facingRight)
    {
        if (facingRight)
        {
            cameraSettings.m_ScreenX = 0.5f;
        }

        else if (!facingRight)
        {
            cameraSettings.m_ScreenX = 0.55f;
        }

    }
    */
}
