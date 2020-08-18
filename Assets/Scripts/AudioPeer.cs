using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    [SerializeField]
    int frequency;
    public float[] samples = new float[512];
    public float[] frequencyBand = new float[8];
    public float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];
    AudioSource audioSource;

    float[] freqBandHeight = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];

    public float amplitude, amplitudeBuffer;
    float amplitudeHeight;

    [SerializeField] AudioClip[] clips;
    
    private bool playback = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Debug.Log("TEST: " + FindFile.fileLoc);

        if (!FindFile.useSampleBtn)
            StartCoroutine(GetAudioClip());
        else
            PlaySample();

    }

    private void PlaySample()
    {
        int myNum = int.Parse(FindFile.fileLoc);
        audioSource.clip = clips[myNum];
        audioSource.Play();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for(int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if(currentAmplitude > amplitudeHeight)
        {
            amplitudeHeight = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHeight;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHeight;
    }

    void CreateAudioBands()
    {
        for(int i = 0; i < 8; i++)
        {
            if(frequencyBand[i] > freqBandHeight[i])
            {
                freqBandHeight[i] = frequencyBand[i];
            }
            audioBand[i] = frequencyBand[i] / freqBandHeight[i];
            audioBandBuffer[i] = bandBuffer[i] / freqBandHeight[i];
        }
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (frequencyBand[g] > bandBuffer[g])
            {
                bandBuffer[g] = frequencyBand[g];
                bufferDecrease[g] = 0.005f;
            }
            if (frequencyBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        /*
         *  22050 / 512 = 43hz per sample
         *  
         *  20 - 60hz
         *  60 - 250hz
         *  250 - 500hz
         *  2000 - 4000hz
         *  4000 - 6000hz
         *  6000 - 20000hz
         *  
         *  0 - 2 = 86hz
         *  1 - 4 = 172hz - 87-258
         *  2 - 8 = 344hz - 259-602
         *  3 - 16 = 688hz - 603-1290
         *  4 - 32 = 1376hz - 1291-2666
         *  5 - 64 = 2752hz - 2667-5418
         *  6 - 128 = 5504hz - 5419-10922
         *  7 - 256 = 11008hz - 10923-21930
         *  510
         */
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            frequencyBand[i] = average * 10;
        }
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(FindFile.fileLoc, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                playback = true;
                //audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

}
