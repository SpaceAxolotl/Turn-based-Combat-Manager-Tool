using UnityEngine;
using UnityEngine.UI;

public class BattleStartButton : MonoBehaviour
{
    [SerializeField] private BattleStarter battleStarter;

    private void Start()
    {
        // Find BattleStarter if not assigned
        if (battleStarter == null)
        {
            battleStarter = FindFirstObjectByType<BattleStarter>();
        }

        // Get the Button component and add the click listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("BattleStartButton script requires a Button component!");
        }
    }

    private void OnButtonClick()
    {
        if (battleStarter != null)
        {
            battleStarter.StartBattle();
        }
        else
        {
            Debug.LogError("BattleStarter reference is missing!");
        }
    }
} 