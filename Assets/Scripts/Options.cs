using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioFlowField myAudioFlowField;
    public NoiseFlowField myNoiseFlowField;
    [SerializeField] GameObject myPanel;
    [SerializeField] Slider incrementSlider;
    private bool panelState = true;


    [HideInInspector] public InputField OffsetXInputField;
    [HideInInspector] public InputField OffsetYInputField;
    [HideInInspector] public InputField OffsetZInputField;

    private InputField OffsetSpeedXInputField;
    private InputField OffsetSpeedYInputField;
    private InputField OffsetSpeedZInputField;

    private InputField MinParticleSpeed;
    private InputField MaxParticleSpeed;
    
    private void Start()
    {

        incrementSlider.value = myNoiseFlowField.increment;
        incrementSlider.onValueChanged.AddListener(delegate { ValueChangedCheck(); });
        SetOffsetSpeedXYZInputFields();
        SetOffsetXYZInputFields();
        SetParticleSpeeds();
    }

    private void SetParticleSpeeds()
    {
        MinParticleSpeed = GameObject.Find("MinSpeedInputField").GetComponent<InputField>();
        MinParticleSpeed.text = myAudioFlowField.moveSpeedMinMax.x.ToString();
        MinParticleSpeed.onEndEdit.AddListener(delegate { ChangeParticleSpeedMin(MinParticleSpeed); });

        MaxParticleSpeed = GameObject.Find("MaxSpeedInputField").GetComponent<InputField>();
        MaxParticleSpeed.text = myAudioFlowField.moveSpeedMinMax.y.ToString();
        MaxParticleSpeed.onEndEdit.AddListener(delegate { ChangeParticleSpeedMax(MaxParticleSpeed); });
    }

    public void ChangeParticleSpeedMax(InputField maxParticleSpeed)
    {
        myAudioFlowField.moveSpeedMinMax.y = float.Parse(maxParticleSpeed.text);
    }

    public void ChangeParticleSpeedMin(InputField minParticleSpeed)
    {
        myAudioFlowField.moveSpeedMinMax.x = float.Parse(minParticleSpeed.text);
    }

    private void SetOffsetSpeedXYZInputFields()
    {
        OffsetSpeedXInputField = GameObject.Find("OffsetSpeedXInputField").GetComponent<InputField>();
        OffsetSpeedXInputField.text = myNoiseFlowField.offsetSpeed.x.ToString();
        OffsetSpeedXInputField.onEndEdit.AddListener(delegate { ChangeOffsetSpeedX(OffsetSpeedXInputField); });

        OffsetSpeedYInputField = GameObject.Find("OffsetSpeedYInputField").GetComponent<InputField>();
        OffsetSpeedYInputField.text = myNoiseFlowField.offsetSpeed.y.ToString();
        OffsetSpeedYInputField.onEndEdit.AddListener(delegate { ChangeOffsetSpeedY(OffsetSpeedYInputField); });

        OffsetSpeedZInputField = GameObject.Find("OffsetSpeedZInputField").GetComponent<InputField>();
        OffsetSpeedZInputField.text = myNoiseFlowField.offsetSpeed.z.ToString();
        OffsetSpeedZInputField.onEndEdit.AddListener(delegate { ChangeOffsetSpeedZ(OffsetSpeedZInputField); });

    }

    public void ChangeOffsetSpeedZ(InputField offsetSpeedZInputField)
    {
        myNoiseFlowField.offsetSpeed.z = float.Parse(offsetSpeedZInputField.text);
    }

    public void ChangeOffsetSpeedY(InputField offsetSpeedYInputField)
    {
        myNoiseFlowField.offsetSpeed.y = float.Parse(offsetSpeedYInputField.text);
    }

    public void ChangeOffsetSpeedX(InputField offsetSpeedXInputField)
    {
        myNoiseFlowField.offsetSpeed.x = float.Parse(offsetSpeedXInputField.text);
    }

    private void SetOffsetXYZInputFields()
    {
        OffsetXInputField = GameObject.Find("OffsetXInputField").GetComponent<InputField>();
        OffsetXInputField.text = 0.ToString();
        OffsetXInputField.onEndEdit.AddListener(delegate { ChangeOffsetX(OffsetXInputField); });

        OffsetYInputField = GameObject.Find("OffsetYInputField").GetComponent<InputField>();
        OffsetYInputField.text = 0.ToString();
        OffsetYInputField.onEndEdit.AddListener(delegate { ChangeOffsetY(OffsetYInputField); });

        OffsetZInputField = GameObject.Find("OffsetZInputField").GetComponent<InputField>();
        OffsetZInputField.text = 0.ToString();
        OffsetZInputField.onEndEdit.AddListener(delegate { ChangeOffsetZ(OffsetZInputField); });
    }

    public void ChangeOffsetZ(InputField offsetZInputField)
    {
        myNoiseFlowField.offset.z = float.Parse(offsetZInputField.text);
    }

    public void ChangeOffsetY(InputField offsetYInputField)
    {
        myNoiseFlowField.offset.y = float.Parse(offsetYInputField.text);
    }

    public void ChangeOffsetX(InputField offsetXInputField)
    {
        myNoiseFlowField.offset.x = float.Parse(offsetXInputField.text);
        //Debug.Log(OffsetXInputField.text);
    }

    public void ShowOptions()
    {
        panelState = !panelState;
        myPanel.gameObject.SetActive(panelState);
    }
    
    public void ValueChangedCheck()
    {
        myNoiseFlowField.increment = incrementSlider.value;
        GameObject.Find("NoisePre").GetComponent<TMPro.TMP_Text>().text = incrementSlider.value.ToString();

    }
    
    public void FullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
    
    public void UseSpeed(bool value)
    {
        myAudioFlowField.useSpeed = value;
    }

    public void UseScale(bool value)
    {
        myAudioFlowField.useScale = value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
