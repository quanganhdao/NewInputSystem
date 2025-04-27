using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionRebinding : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionAsset; // Reference to the Input Action Asset
    // Start is called before the first frame update
    public void StartRebinding()
    {
        inputActionAsset.action.Disable(); // Disable the action before rebinding
        inputActionAsset.action.PerformInteractiveRebinding().OnComplete((op) => { RebindCompleted(); op.Dispose(); inputActionAsset.action.Enable(); }).Start();

    }

    public void RebindCompleted()
    {
        // Handle the completion of the rebinding process here
        Debug.Log("Rebinding completed!");
        Save();
    }
    private void Save()
    {
        InputActionAsset asset = inputActionAsset.action.actionMap.asset;
        var rebinds = asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(asset.name, rebinds);
    }
}
