using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System;
using UnityEngine.UI;

public class FindFile : MonoBehaviour
{
    public static string fileLoc;
    public static bool useSampleBtn;
    private TMPro.TMP_Text searchOutput;
    private Button startButton;
    private TMPro.TMP_Dropdown musicSelect;

    string[] words;

    private void Start()
    {
        useSampleBtn = true;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Playable", ".wav"));
        FileBrowser.SetDefaultFilter(".wav");

        searchOutput = GameObject.Find("TMPSearchOutput").GetComponent<TMPro.TMP_Text>();
        startButton = GameObject.Find("StartBtn").GetComponent<Button>();
        musicSelect = GameObject.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();

        musicSelect.onValueChanged.AddListener(delegate { DropDownValueChanged(musicSelect); });
    }

    public void DropDownValueChanged(TMPro.TMP_Dropdown musicSelect)
    {
        //Debug.Log(musicSelect.value);
        fileLoc = musicSelect.value.ToString();
        if(musicSelect.value != 0)
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }

    public void FindFilePath()
    {
        FileBrowser.ShowLoadDialog((fpath) => { fileLoc = fpath; Debug.Log("Path: " + fileLoc); FileSearchOutput(fpath); startButton.interactable = true; }, () => { Debug.Log("Canceled"); }, false, null, "Select Folder", "Select");
    }

    public void UseSearchBtn()
    {
        useSampleBtn = false;
        if(searchOutput.text != "File: N/A")
        {
            startButton.interactable = true;
        }
        else if(searchOutput.text == "File: N/A")
        {
            startButton.interactable = false;
        }
    }

    public void UseSampleBtn()
    {
        useSampleBtn = true;
    }



    void FileSearchOutput(string path)
    {
        words = path.Split('\\');
        int val = words.Length;
        searchOutput.text = "File: " + words[val-1];
        //Debug.Log(words[val-1]);
    }

}
