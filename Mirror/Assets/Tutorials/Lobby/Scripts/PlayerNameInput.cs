using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }  //Get display name (nameInputField)

    private const string PlayerPrefsNameKey = "PlayerName"; // string playername that gets saved 

    private void Start() => SetUpInputField();   // start = void setupInputField

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }  // if playerpref name is saved get it, else return.

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey); // playerpref emtpy, get playername and save

        nameInputField.text = defaultName; //name inputfield == inputfield inhoud

        SetPlayerName(defaultName); // start void SetPlayername met de name gesaved
    }


    public TMP_Text text_Name;  
    public void SetPlayerName(string name)
    {
        if(nameInputField.text == "")   // if nameinputfield == empty
        {
            continueButton.interactable = false; // continue button false
        }
        else
        {
            continueButton.interactable = true; // true
        }
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text; //saved DisplayName = nameinputfield

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName); // save it
      
    }
}
