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
    [SerializeField] InputActionReference combinedKeyAction;
    [SerializeField] private InputActionReference ultimatePowerAction; // Reference to the combined key action
    [SerializeField] private UnityEvent onDoubleTab;
    [SerializeField] private GameObject objectToToggle;


    // Start is called before the first frame update
    void Start()
    {
    }
    void Awake()
    {
        inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(inputActions.name));

        ultimatePowerAction.action.started += ctx => Debug.Log("Ultimate power action started");
        ultimatePowerAction.action.performed += ctx => Debug.Log("Attack action performed");
        Debug.Log("Ultimate action value: " + ultimatePowerAction.action.controls);
        for (int i = 0; i < ultimatePowerAction.action.controls.Count; i++)
        {
            Debug.Log("Control " + i + ": " + ultimatePowerAction.action.controls[i].name);
        }
        for (int i = 0; i < moveAction.action.controls.Count; i++)
        {
            Debug.Log("Control " + i + ": " + moveAction.action.controls[i].name);
        }



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
