using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombatTurnManager turnManager;
    [SerializeField] private BattleUI battleUI;

    private void Start()
    {
        if (turnManager == null)
            turnManager = FindFirstObjectByType<CombatTurnManager>();
        if (battleUI == null)
            battleUI = FindFirstObjectByType<BattleUI>();

        // Initialize with active characters
        var activeAlly = CurrentAllies.Instance.ActiveAllyData;
        var activeEnemy = CurrentEnemies.Instance.ActiveEnemyData;

        if (activeAlly != null && activeEnemy != null)
        {
            // Update UI to show the active characters
            if (battleUI != null)
            {
                activeAlly.InitializeCurrentHealthFromMax();
                activeEnemy.InitializeCurrentHealthFromMax();
                battleUI.UpdateAllyUI();
                battleUI.UpdateEnemyUI();
            }
        }
        else
        {
            Debug.LogError("No active ally or enemy found in CurrentAllies/CurrentEnemies!");
        }
    }

    public void StartBattle()
    {
        Debug.Log("Starting battle...");
        if (battleUI != null)
        {
            Debug.Log("Showing move selection UI...");
            battleUI.OnAllyTurnStart();
        }
        else
        {
            Debug.LogError("BattleUI reference is missing!");
        }
        turnManager.StartBattle();
    }
} 