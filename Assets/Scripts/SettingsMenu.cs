using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audiomixer;

    private void Start()
    {
        Screen.fullScreen = true;
    }

    public void SetVolume (float volume)
    {
        audiomixer.SetFloat("volume", volume);
    }

    public void SetFullscreen (Toggle toggle)
    {
        toggle.isOn = true;
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void SetWindowd(Toggle toggle)
    {
        toggle.isOn = true;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}
