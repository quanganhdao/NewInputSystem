using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionReference doubleTab;
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference defendAction;
    [SerializeField] InputActionReference crouchAction;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] private UnityEvent onDoubleTab;
    [SerializeField] private GameObject objectToToggle;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doubleTab.action.WasPerformedThisFrame())
        {
            onDoubleTab.Invoke();
            Debug.Log("Double tab action performed");

        }
        if (doubleTab.action.WasReleasedThisFrame())
        {
            Debug.Log("Double tab action released");
        }
        if (doubleTab.action.WasCompletedThisFrame())
        {
            Debug.Log("Double tab action completed");
        }
        if (doubleTab.action.WasPressedThisFrame())
        {

            Debug.Log("Double tab action pressed");
        }

        if (attackAction.action.WasPerformedThisFrame())
        {
            Debug.Log("Attack action performed");
        }
        if (defendAction.action.WasPerformedThisFrame())
        {
            Debug.Log("Defend action performed");
        }

        Debug.Log("Move action value: " + moveAction.action.ReadValue<Vector2>());
    }
    public void ToggleObject()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
