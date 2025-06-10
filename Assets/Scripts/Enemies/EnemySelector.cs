using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyLibrary enemyLibrary;

    [Header("Enemy Selection")]
    [SerializeField] private EnemyData selectedEnemy;

    public EnemyData SelectedEnemy => selectedEnemy;

    private void Start()
    {
        if (enemyLibrary == null)
        {
            Debug.LogError("EnemyLibrary not assigned to EnemySelector!");
            return;
        }
    }

    private void OnEnable()
    {
        if (selectedEnemy != null && CurrentEnemies.Instance != null)
        {
            CurrentEnemies.Instance.AddEnemyGameObject(gameObject);
        }
    }

    private void OnDisable()
    {
        if (CurrentEnemies.Instance != null)
        {
            CurrentEnemies.Instance.RemoveEnemyGameObject(gameObject);
        }
    }

    public void SelectEnemy(EnemyData enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Attempted to select null enemy!");
            return;
        }

        selectedEnemy = enemy;
        // Only register if we're not already registered
        if (CurrentEnemies.Instance != null && !CurrentEnemies.Instance.IsEnemyActive(gameObject))
        {
            CurrentEnemies.Instance.AddEnemyGameObject(gameObject);
        }
        Debug.Log($"Selected enemy: {selectedEnemy.enemyName}");
    }

    // Editor method to validate the selected enemy
    private void OnValidate()
    {
        if (enemyLibrary != null && enemyLibrary.enemies != null && selectedEnemy != null)
        {
            selectedEnemy = enemyLibrary.enemies.Find(e => e.enemyName == selectedEnemy.enemyName);
        }
    }
} 