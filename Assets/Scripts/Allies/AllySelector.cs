using UnityEngine;

public class AllySelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AllyLibrary allyLibrary;

    [Header("Ally Selection")]
    [SerializeField] private AllyData selectedAlly;

    public AllyData SelectedAlly => selectedAlly;

    private void Start()
    {
        if (allyLibrary == null)
        {
            Debug.LogError("AllyLibrary not assigned to AllySelector!");
            return;
        }
    }

    private void OnEnable()
    {
        if (selectedAlly != null && CurrentAllies.Instance != null)
        {
            CurrentAllies.Instance.AddAllyGameObject(gameObject);
        }
    }

    private void OnDisable()
    {
        if (CurrentAllies.Instance != null)
        {
            CurrentAllies.Instance.RemoveAllyGameObject(gameObject);
        }
    }

    public void SelectAlly(AllyData ally)
    {
        if (ally == null)
        {
            Debug.LogError("Attempted to select null ally!");
            return;
        }

        selectedAlly = ally;
        // Only register if we're not already registered
        if (CurrentAllies.Instance != null && !CurrentAllies.Instance.IsAllyActive(gameObject))
        {
            CurrentAllies.Instance.AddAllyGameObject(gameObject);
        }
        Debug.Log($"Selected ally: {selectedAlly.allyName}");
    }

    // Editor method to validate the selected index
    private void OnValidate()
    {
        if (allyLibrary != null && allyLibrary.allies != null && selectedAlly != null)
        {
            selectedAlly = allyLibrary.allies.Find(a => a.allyName == selectedAlly.allyName);
        }
    }
} 