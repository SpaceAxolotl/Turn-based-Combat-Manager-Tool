using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour
{
    [Header("Ally UI")]
    [SerializeField] private TextMeshProUGUI allyNameText;
    [SerializeField] private TextMeshProUGUI allyHealthText;
    [SerializeField] private Slider allyHealthSlider;
    [SerializeField] private TextMeshProUGUI allyTypeText;

    [Header("Enemy UI")]
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private TextMeshProUGUI enemyTypeText;

    [Header("Move Selection")]
    [SerializeField] private GameObject moveSelectionPanel;
    [SerializeField] private Transform moveButtonContainer;
    [SerializeField] private GameObject moveButtonPrefab;

    [Header("Turn Manager")]
    [SerializeField] private CombatTurnManager turnManager;

    private void Start()
    {
        if (turnManager == null)
            turnManager = FindFirstObjectByType<CombatTurnManager>();

        // Initialize UI with current ally and enemy data
        UpdateAllyUI();
        UpdateEnemyUI();
    }

    public void OnAllyTurnStart()
    {
        Debug.Log("Ally turn started - showing move selection");
        // Show move selection UI
        ShowMoveSelection();
    }

    public void OnEnemyTurnStart()
    {
        Debug.Log("Enemy turn started - hiding move selection");
        // Hide move selection UI
        HideMoveSelection();
    }

    public void OnBattleEnd()
    {
        Debug.Log("Battle ended - hiding move selection");
        // Hide move selection UI
        HideMoveSelection();
    }

    private void ShowMoveSelection()
    {
        if (moveSelectionPanel == null)
        {
            Debug.LogError("Move selection panel is not assigned!");
            return;
        }

        moveSelectionPanel.SetActive(true);
        PopulateMoveButtons();
    }

    private void HideMoveSelection()
    {
        if (moveSelectionPanel != null)
        {
            moveSelectionPanel.SetActive(false);
        }
    }

    private void PopulateMoveButtons()
    {
        if (moveButtonContainer == null)
        {
            Debug.LogError("Move button container is not assigned!");
            return;
        }

        if (moveButtonPrefab == null)
        {
            Debug.LogError("Move button prefab is not assigned!");
            return;
        }

        // Clear existing buttons
        foreach (Transform child in moveButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Get moves from current ally
        var ally = CurrentAllies.Instance.ActiveAllyData;
        if (ally == null)
        {
            Debug.LogError("No active ally data found!");
            return;
        }

        Debug.Log($"Populating move buttons for ally: {ally.allyName} with {ally.moves.Count} moves");

        foreach (var move in ally.moves)
        {
            if (move == null)
            {
                Debug.LogWarning("Found null move in ally's moves list!");
                continue;
            }

            GameObject buttonObj = Instantiate(moveButtonPrefab, moveButtonContainer);
            if (buttonObj == null)
            {
                Debug.LogError("Failed to instantiate move button!");
                continue;
            }

            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            
            if (buttonText == null)
            {
                Debug.LogError("Move button prefab is missing TextMeshProUGUI component!");
                continue;
            }

            buttonText.text = $"{move.moveName}\nPower: {move.power} Acc: {move.accuracy}%";
            Debug.Log($"Created move button for: {move.moveName}");

            if (button == null)
            {
                Debug.LogError("Move button prefab is missing Button component!");
                continue;
            }

            button.onClick.AddListener(() => OnMoveButtonClicked(move));
        }
    }

    private void OnMoveButtonClicked(MoveData move)
    {
        Debug.Log($"Move button clicked: {move.moveName}");
        if (turnManager != null)
        {
            turnManager.OnMoveSelected(move);
        }
    }

    public void UpdateAllyUI()
{
    var ally = CurrentAllies.Instance.ActiveAllyData;
    if (ally == null) return;

    // Update name
    if (allyNameText != null)
        allyNameText.text = ally.allyName;

    // Ensure maxHealth stat name matches what's in AllyData (likely "maxHealth")
    int maxHealth = ally.GetStatValue("maxHealth");
    int currentHealth = ally.currentHealth;

    // Update health text
    if (allyHealthText != null)
        allyHealthText.text = $"HP: {currentHealth}/{maxHealth}";

    // Update health slider
    if (allyHealthSlider != null && maxHealth > 0)
        allyHealthSlider.value = (float)currentHealth / maxHealth;

    // Update type
    if (allyTypeText != null)
        allyTypeText.text = string.Join(", ", ally.types.ConvertAll(t => t.typeName));
}


    public void UpdateEnemyUI()
    {
        var enemy = CurrentEnemies.Instance.ActiveEnemyData;
        if (enemy == null) return;

        // Update name
        if (enemyNameText != null)
            enemyNameText.text = enemy.enemyName;

        // Update health
        if (enemyHealthText != null)
        {
            int currentHealth = (int)CurrentEnemies.Instance.GetActiveEnemyStat("Health");
            int maxHealth = (int)CurrentEnemies.Instance.GetActiveEnemyStat("Max Health");
            enemyHealthText.text = $"HP: {currentHealth}/{maxHealth}";
        }

        // Update health slider
        if (enemyHealthSlider != null)
        {
            int currentHealth = (int)CurrentEnemies.Instance.GetActiveEnemyStat("Health");
            int maxHealth = (int)CurrentEnemies.Instance.GetActiveEnemyStat("Max Health");
            enemyHealthSlider.value = (float)currentHealth / maxHealth;
        }

        // Update type
        if (enemyTypeText != null)
            enemyTypeText.text = string.Join(", ", enemy.types.ConvertAll(t => t.typeName));
    }
} 