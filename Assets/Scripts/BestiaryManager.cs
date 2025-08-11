using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CapturableEntry
{
    public string capturableName; // The name of the capturable creature
    public TextMeshProUGUI capturableCountText; // Reference to the TextMeshProUGUI element for this capturable
    [HideInInspector] public int captureCount; // Hidden in Inspector, used for internal counting
}

public class BestiaryManager : MonoBehaviour
{
    public static BestiaryManager Instance;
    // A List of Capturable Entries
    [SerializeField] private List<CapturableEntry> capturableList = new List<CapturableEntry>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadBestiary();
    }

    // Method to add to capturable count
    public void AddCapturableCount(string capturableName)
    {
        // Find the capturable in the list
        CapturableEntry capturable = capturableList.Find(c => c.capturableName == capturableName);

        // If it exists, increment the count
        if (capturable != null && capturable.captureCount < 999)
        {
            capturable.captureCount++;
            UpdateUI(capturable); // Update the corresponding UI
        }
        else
        {
            Debug.LogError("Capturable not found or reached maximun quantity: " + capturableName);
        }

        // Save the updated data
        SaveBestiary();
    }

    // Update the UI for a specific capturable
    private void UpdateUI(CapturableEntry capturable)
    {
        if (capturable.capturableCountText != null)
        {
            Image capturableSR = capturable.capturableCountText.gameObject.transform.parent.transform.parent.GetChild(1).GetComponent<Image>();
            if(capturable.captureCount == 0){
                capturableSR.color = Color.black;
            }
            else if(capturable.captureCount > 0 && capturableSR.color == Color.black){
                capturableSR.color = Color.white;
            }
            capturable.capturableCountText.text = capturable.captureCount.ToString();
        }
    }

    // Save the bestiary data to PlayerPrefs
    private void SaveBestiary()
    {
        foreach (var capturable in capturableList)
        {
            PlayerPrefs.SetInt(capturable.capturableName, capturable.captureCount);
        }
        PlayerPrefs.Save();
    }

    // Load the bestiary data from PlayerPrefs
    public void LoadBestiary()
    {
        foreach (var capturable in capturableList)
        {
            // Load the saved value if it exists
            if (PlayerPrefs.HasKey(capturable.capturableName))
            {
                capturable.captureCount = PlayerPrefs.GetInt(capturable.capturableName);
                capturable.capturableCountText.gameObject.transform.parent.transform.parent.GetChild(1).GetComponent<Image>().color = Color.white;
            }
            else
            {
                // Initialize with 0 if no value is found in PlayerPrefs
                capturable.captureCount = 0;
            }

            // Update the UI after loading
            UpdateUI(capturable);
        }
    }
}
