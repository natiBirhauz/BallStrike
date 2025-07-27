using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    public InputField cheatInputField; // Reference to the standard InputField
    private GameManager gameManager; // Reference to GameManager
    public TextMeshProUGUI cheatText; // 

    // Dictionary of cheat codes and their effects, tied to ball types
    private readonly System.Collections.Generic.Dictionary<string, System.Action> cheats =
        new System.Collections.Generic.Dictionary<string, System.Action>
    {
        { "MORE POINTS", () => Instance?.AddPoints(100) }, 
        { "ENDLESS POINTS", () => Instance?.AddPoints(1000000) }, 
        { "MORE HEALTH", () => Instance?.IncreasePlayerHealth(5) },  
        { "ENDLESS HEALTH", () => Instance?.IncreasePlayerHealth(999) }, 
        
        // Add more ball types and effects here

        };

    private static GameManager Instance => Object.FindFirstObjectByType<GameManager>();

    private void Start()
    {
        gameManager = Instance;
        if (cheatInputField == null)
        {
            cheatInputField = GetComponent<InputField>();
        }

        if (cheatInputField != null)
        {
            // Process cheat when Enter is pressed
            cheatInputField.onEndEdit.AddListener(ProcessCheat);
        }
        else
        {
            Debug.LogWarning("Cheat Input Field not assigned!");
        }
    }

    private void Update()
    {
        // Show Input Field only in Options state
        if (cheatInputField != null)
        {
            cheatInputField.gameObject.SetActive(GameStateManager.Instance.CurrentState == GameState.Options);
        }
    }

    public void ProcessCheat(string input)
    {
        // Only process cheats in Options state
        if (GameStateManager.Instance.CurrentState != GameState.Options)
            return;

        input = input.Trim().ToUpper();
        if (cheats.ContainsKey(input))
        {
            cheats[input]?.Invoke();
            Debug.Log($"Cheat activated: {input}");

            cheatText.gameObject.SetActive(true); // Show cheat text
            //set to false after 2 seconds
            Invoke(nameof(HideCheatText), 2f);

            cheatInputField.text = ""; // Clear input field
        }
        else
        {
            Debug.LogWarning($"Invalid cheat code: {input}");
        }
    }

    private object HideCheatText()
    {
        if (cheatText != null)
        {
            cheatText.gameObject.SetActive(false); // Hide cheat text
        }
        return null;
    }
}