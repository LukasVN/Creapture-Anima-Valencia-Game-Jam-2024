using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusic;
    public Slider musicSlider;
    public TextMeshProUGUI musicValueText;
    public Slider effectsSlider;
    public TextMeshProUGUI effectsValueText;
    public AudioClip menuMusic;
    public AudioClip inGameMusic;
    public AudioSource soundEffects;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        if(PlayerPrefs.HasKey("Music")){
            backgroundMusic.volume = PlayerPrefs.GetFloat("Music");
            musicSlider.value = backgroundMusic.volume;
            musicValueText.text = (backgroundMusic.volume*100).ToString("00");
        }
        else{
            PlayerPrefs.SetFloat("Music", backgroundMusic.volume);
            musicSlider.value = backgroundMusic.volume;
            musicValueText.text = backgroundMusic.volume*100+"";
        }
        if(PlayerPrefs.HasKey("FX")){
            soundEffects.volume = PlayerPrefs.GetFloat("FX");
            effectsSlider.value = soundEffects.volume;
            effectsValueText.text = (soundEffects.volume*100).ToString("00");
        }
        else{
            PlayerPrefs.SetFloat("FX", soundEffects.volume);
            effectsSlider.value = soundEffects.volume;
            effectsValueText.text = (soundEffects.volume*100).ToString("00");
        }
    }

    public void SetInGameMusic(){
        backgroundMusic.Stop();
        backgroundMusic.clip = inGameMusic;
        backgroundMusic.Play();
    }

    public void SetMenuMusic(){
        backgroundMusic.Stop();
        backgroundMusic.clip = menuMusic;
        backgroundMusic.Play();
    }

    public void ChangeMusicVolume(){
        backgroundMusic.volume = musicSlider.value;
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        musicValueText.text = (backgroundMusic.volume*100).ToString("00");
    }

    public void ChangeEffectsVolume(){
        soundEffects.volume = effectsSlider.value;
        PlayerPrefs.SetFloat("FX", effectsSlider.value);
        effectsValueText.text = (soundEffects.volume*100).ToString("00");
    }

    public void PlaySound(AudioSource audioS, AudioClip sound){
        audioS.PlayOneShot(sound);
    }

}
