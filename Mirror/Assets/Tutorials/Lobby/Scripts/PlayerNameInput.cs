using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }


    public TMP_Text text_Name;  
    public void SetPlayerName(string name)
    {
        if(nameInputField.text == "")
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
      
    }
}
