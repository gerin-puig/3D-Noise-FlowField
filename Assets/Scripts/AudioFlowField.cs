using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]
public class AudioFlowField : MonoBehaviour
{
    NoiseFlowField noiseFlowField;
    public AudioPeer audioPeer;
    [Header("Speed")]
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;

    [Header("Scale")]
    public bool useScale;
    public Vector2 scaleMinMax;

    [Header("Material")]
    public Material material;
    private Material[] audioMaterial;

    public bool useColour1;
    public string colourName1;
    public Gradient gradient1;
    private Color[] colour1;
    [Range(0f,1f)]
    public float colourThreshold1;
    public float colourMultiplier1;

    public bool useColour2;
    public string colourName2;
    public Gradient gradient2;
    private Color[] colour2;
    [Range(0f, 1f)]
    public float colourThreshold2;
    public float colourMultiplier2;

    // Start is called before the first frame update
    void Start()
    {
        noiseFlowField = GetComponent<NoiseFlowField>();
        audioMaterial = new Material[8];
        colour1 = new Color[8];
        colour2 = new Color[8];
        for(int i = 0; i < 8; i++)
        {
            colour1[i] = gradient1.Evaluate((1f / 8f) * i);
            colour2[i] = gradient2.Evaluate((1f / 8f) * i);
            audioMaterial[i] = new Material(material);
        }

        int countBand = 0;
        for(int i = 0; i< noiseFlowField.amountOfParticles; i++)
        {
            int band = countBand % 8;
            noiseFlowField.particleMeshRenderer[i].material = audioMaterial[band];
            noiseFlowField.particles[i].audioBand = band;
            countBand++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useSpeed)
        {
            noiseFlowField.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audioPeer.amplitudeBuffer); 
            noiseFlowField.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer.amplitudeBuffer); 
        }
        else
        {
            noiseFlowField.particleMoveSpeed = 25f;
            noiseFlowField.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer.amplitudeBuffer);
        }
        for(int i = 0; i < noiseFlowField.amountOfParticles; i++)
        {
            if (useScale)
            {
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer.audioBandBuffer[noiseFlowField.particles[i].audioBand]);
                noiseFlowField.particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                noiseFlowField.particles[i].transform.localScale = new Vector3(1, 1, 1);
            }
        }
        for(int i = 0; i < 8; i++)
        {
            if (useColour1)
            {
                if(audioPeer.audioBandBuffer[i] > colourThreshold1)
                {
                    audioMaterial[i].SetColor(colourName1, colour1[i] * audioPeer.audioBandBuffer[i] * colourMultiplier1);
                }
                else
                {
                    audioMaterial[i].SetColor(colourName1, colour1[i] * 0f);
                }
            }
            if (useColour2)
            {
                if(audioPeer.audioBand[i] > colourThreshold2)
                {
                    audioMaterial[i].SetColor(colourName1, colour1[i] * audioPeer.audioBand[i] * colourMultiplier2);
                }
                else
                {
                    audioMaterial[i].SetColor(colourName2, colour2[i] * 0f);
                }
            }
        }
    }
}
