using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject settings_Menu;
    public GameObject bestiary_menu;
    public GameObject bestiary_page_1;
    public GameObject bestiary_page_2;
    public GameObject in_Game_Hud;
    public GameObject main_Menu;
    public GameObject you_lost_Buttons;
    public Image last_Capture;
    public GameObject last_Capture_Panel;
    public GameObject you_lost_interface;
    public TextMeshProUGUI maxDistanceRecord;
    public TextMeshProUGUI webCounterText;
    public int distanceRecord;
    public AudioClip buttonSound;
    public AudioClip closeSound;
    public AudioClip resetSound;


    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        if(PlayerPrefs.HasKey("DistanceRecord")){
            distanceRecord = PlayerPrefs.GetInt("DistanceRecord");
            maxDistanceRecord.text = distanceRecord.ToString("00000") +"M";
        }
        else{
            PlayerPrefs.SetInt("DistanceRecord", 0);
        }
        MainMenu_Initialize();

    }

    public void Update(){
        if(bestiary_menu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            CloseBestiary();
        }
        else if(settings_Menu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            ManageSettingTab();
        }
    }

    public void gameStartUI(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, buttonSound);
        webCounterText.text = "x"+GameManager.Instance.webCounter;
        you_lost_interface.SetActive(false);
        you_lost_Buttons.SetActive(false);
        main_Menu.SetActive(false);
        last_Capture_Panel.SetActive(false);
        in_Game_Hud.SetActive(true);
    }

    public void youLostUI(int playerDistance){
        you_lost_interface.SetActive(true);
        you_lost_Buttons.SetActive(true);
        last_Capture_Panel.SetActive(true);
        if(playerDistance > PlayerPrefs.GetInt("DistanceRecord")){
            PlayerPrefs.SetInt("DistanceRecord", playerDistance);
            maxDistanceRecord.SetText(playerDistance.ToString("00000") +"M");
        }
    }

    public void OpenBestiary(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, buttonSound);
        bestiary_menu.SetActive(true);
    }

    public void ChangePage(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, buttonSound);
        bestiary_page_1.SetActive(!bestiary_page_1.activeSelf);
        bestiary_page_2.SetActive(!bestiary_page_2.activeSelf);
    }

    public void CloseBestiary(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, closeSound);
        bestiary_menu.SetActive(false);
        bestiary_page_1.SetActive(true);
        bestiary_page_2.SetActive(false);
    }

    public void ManageSettingTab(){
        settings_Menu.SetActive(!settings_Menu.activeSelf);
        if(settings_Menu.activeSelf){
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, buttonSound);
        }
        else{
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, closeSound);
        }
    }

    public void ResetAllData(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, resetSound);
        AudioManager.Instance.backgroundMusic.volume = 0.2f;
        AudioManager.Instance.musicSlider.value = 0.2f;
        AudioManager.Instance.musicValueText.text = (AudioManager.Instance.backgroundMusic.volume*100).ToString("00");
        AudioManager.Instance.soundEffects.volume = 0.2f;
        AudioManager.Instance.effectsSlider.value = 0.2f;
        AudioManager.Instance.effectsValueText.text = (AudioManager.Instance.soundEffects.volume*100).ToString("00");
        PlayerPrefs.DeleteAll();
        distanceRecord = 0;
        maxDistanceRecord.text = distanceRecord.ToString("00000") + "M";
        BestiaryManager.Instance.LoadBestiary();
    }

    public void MainMenu_Initialize(){
        AudioManager.Instance.SetMenuMusic();
        settings_Menu.SetActive(false);
        you_lost_interface.SetActive(false);
        in_Game_Hud.SetActive(false);
        bestiary_menu.SetActive(false);
        bestiary_page_1.SetActive(true);
        bestiary_page_2.SetActive(false);
        main_Menu.SetActive(true);
        you_lost_Buttons.SetActive(true);
    }

}
