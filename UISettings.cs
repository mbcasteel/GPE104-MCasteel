using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettings : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Mute Toggles")]
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    [Header("Labels (optional)")]
    public TextMeshProUGUI musicValueText;
    public TextMeshProUGUI sfxValueText;

    private bool initialized = false;

    private void Start()
    {
        LoadSettings();

        // Update slider values visually
        UpdateUI();

        // Hook up listeners
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggled);
        sfxMuteToggle.onValueChanged.AddListener(OnSFXMuteToggled);

        initialized = true;
    }

    //  Volume changed (sliders)
    public void OnMusicVolumeChanged(float value)
    {
        if (!initialized) return;

        SoundManager.instance.SetMusicVolume(value);
        UpdateLabels();
        SaveSettings();
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (!initialized) return;

        SoundManager.instance.SetSFXVolume(value);
        UpdateLabels();
        SaveSettings();
    }

    //  Mute toggles
    public void OnMusicMuteToggled(bool muted)
    {
        if (!initialized) return;

        SoundManager.instance.musicSource.mute = muted;
        SaveSettings();
    }

    public void OnSFXMuteToggled(bool muted)
    {
        if (!initialized) return;

        SoundManager.instance.sfxSource.mute = muted;
        SaveSettings();
    }

    //  Save preferences between sessions
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", SoundManager.instance.musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SoundManager.instance.sfxVolume);
        PlayerPrefs.SetInt("MusicMuted", SoundManager.instance.musicSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", SoundManager.instance.sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    //  Load preferences at startup
    private void LoadSettings()
    {
        if (SoundManager.instance == null) return;

        SoundManager.instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.4f);
        SoundManager.instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        bool musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bool sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        SoundManager.instance.musicSource.mute = musicMuted;
        SoundManager.instance.sfxSource.mute = sfxMuted;
    }

    private void UpdateUI()
    {
        if (SoundManager.instance == null) return;

        musicSlider.value = SoundManager.instance.musicVolume;
        sfxSlider.value = SoundManager.instance.sfxVolume;
        musicMuteToggle.isOn = SoundManager.instance.musicSource.mute;
        sfxMuteToggle.isOn = SoundManager.instance.sfxSource.mute;

        UpdateLabels();
    }

    private void UpdateLabels()
    {
        if (musicValueText != null)
            musicValueText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();

        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(sfxSlider.value * 100).ToString();
    }
}
