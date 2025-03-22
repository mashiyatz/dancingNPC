using System.Collections;
using UnityEngine;

public class VisualizeAudioSpectrum : MonoBehaviour
{
    public Camera backgroundCam;
    public static float[] spectrum;
    private int spectrumSize;

    private AudioClip microphoneClip;

    public float soundDeltaThreshold;
    public float soundPickupTimeInterval;

    private float previousAudioValue;
    private float audioValue;
    private float timeOfTransition;
    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;
    private Coroutine colorCoroutine;


    void Start()
    {
        spectrumSize = 64;
        spectrum = new float[spectrumSize];
        MicrophoneToAudioClip();

        timeOfTransition = Time.time;
    }

    IEnumerator ChangeBackgroundColor(Color newColor)
    {
        float t = 0;
        Color startColor = backgroundCam.backgroundColor;
        while (t < soundPickupTimeInterval) {
            t += Time.deltaTime;
            backgroundCam.backgroundColor = Color.Lerp(startColor, newColor, t/soundPickupTimeInterval);
            yield return null;
        }
    }

    void Update()
    {
        GetAudioDataFromMic();
        // AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        previousAudioValue = audioValue;
        audioValue = (float)System.Math.Tanh(100 * GetLoudness()); // how to best normalize volume
        // Debug.Log(audioValue);

        if (Time.time - timeOfTransition > soundPickupTimeInterval) // set interval to 1/(bpm / 60)?
        {
            if (Mathf.Abs(audioValue - previousAudioValue) > soundDeltaThreshold)
            {
                if (colorCoroutine != null) StopCoroutine(colorCoroutine);
                colorCoroutine = StartCoroutine(ChangeBackgroundColor(Color.Lerp(colorA, colorB, audioValue)));
                timeOfTransition = Time.time;
            }
            
        }
    }

    void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    void GetAudioDataFromMic()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);
        int startPosition = clipPosition - spectrumSize;
        if (startPosition < 0) return;
        microphoneClip.GetData(spectrum, startPosition);
    }

    float GetLoudness()
    {
        float totalLoudness = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            totalLoudness += Mathf.Abs(spectrum[i]);
        }

        return totalLoudness / spectrumSize;
    }
}
