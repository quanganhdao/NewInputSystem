using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] InputActionReference doubleTab;
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference defendAction;
    [SerializeField] InputActionReference crouchAction;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] private UnityEvent onDoubleTab;
    [SerializeField] private GameObject objectToToggle;
    [SerializeField] private InputActionReference combinedKeyAction;


    // Start is called before the first frame update
    void Start()
    {
    }
    void Awake()
    {
        inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(inputActions.name));

    }

    // Update is called once per frame
    void Update()
    {
        if (doubleTab.action.WasPerformedThisFrame())
        {
            onDoubleTab.Invoke();
            Debug.Log("Double tab action performed");

        }

        if (attackAction.action.WasPerformedThisFrame())
        {
            Debug.Log("Attack action performed");
        }
        if (defendAction.action.WasPerformedThisFrame())
        {
            Debug.Log("Defend action performed");
        }
        if (combinedKeyAction.action.WasPerformedThisFrame())
        {
            Debug.Log("Combined key action performed");
        }

        // Debug.Log("Move action value: " + moveAction.action.ReadValue<Vector2>());
    }
    public void ToggleObject()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
