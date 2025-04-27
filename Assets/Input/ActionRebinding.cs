using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class ActionRebinding : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference; // Reference to the Input Action Asset
    [SerializeField] private bool useModifier; // Xác định nếu action sử dụng modifier

    private RebindingOperation rebindOperation;
    private int bindingIndex; // Index của binding đang được rebind
    private bool isRebindingComplete = true; // Trạng thái rebinding

    // Bắt đầu quá trình rebinding
    public void StartRebinding()
    {
        if (useModifier)
        {
            // Nếu sử dụng modifier, bắt đầu với modifier trước
            StartModifierRebinding();
        }
        else
        {
            // Nếu không có modifier, rebind bình thường
            StartSimpleRebinding();
        }
    }

    // Rebinding đơn giản cho các action không có modifier
    private void StartSimpleRebinding()
    {
        var action = inputActionReference.action;
        action.Disable();

        rebindOperation = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnComplete(operation => CompleteSimpleRebinding(operation))
            .Start();

        LogBindings();
    }

    // Hoàn thành rebinding đơn giản
    private void CompleteSimpleRebinding(RebindingOperation operation)
    {
        operation.Dispose();

        var action = inputActionReference.action;
        action.Enable();

        Debug.Log("Simple rebinding completed!");
        SaveBindings();
    }

    // Bắt đầu rebinding cho phím modifier
    private void StartModifierRebinding()
    {
        var action = inputActionReference.action;

        // Tìm index của composite OneModifier
        int compositeIndex = FindCompositeIndex("One Modifier");

        if (compositeIndex != -1)
        {
            // Modifier là binding đầu tiên trong composite (index + 1)
            bindingIndex = compositeIndex + 1;

            action.Disable();
            isRebindingComplete = false;

            rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => CompleteModifierRebinding())
                .Start();

            Debug.Log("Please press a modifier key (Shift, Ctrl, Alt)");
        }
        else
        {
            Debug.LogError("No OneModifier composite found in this action");
        }
    }

    // Hoàn thành rebinding cho phím modifier và chuyển sang phím chính
    private void CompleteModifierRebinding()
    {
        Debug.Log("Modifier binding rebinded to: " + inputActionReference.action.bindings[bindingIndex].path);

        // Sau khi rebind modifier, tiếp tục với phím chính
        StartMainKeyRebinding();
    }

    // Bắt đầu rebinding cho phím chính
    private void StartMainKeyRebinding()
    {
        var action = inputActionReference.action;

        // Tìm index của composite OneModifier
        int compositeIndex = FindCompositeIndex("One Modifier");

        if (compositeIndex != -1)
        {
            // Phím chính là binding thứ hai trong composite (index + 2)
            bindingIndex = compositeIndex + 2;

            rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => CompleteMainKeyRebinding())
                .Start();

            Debug.Log("Now press the main key");
        }
    }

    // Hoàn thành rebinding cho phím chính và lưu thay đổi
    private void CompleteMainKeyRebinding()
    {
        rebindOperation.Dispose();

        var action = inputActionReference.action;
        action.Enable();

        isRebindingComplete = true;
        SaveBindings();
        Debug.Log(inputActionReference.action.GetBindingDisplayString());
    }

    // Tìm index của composite trong action
    private int FindCompositeIndex(string compositeName)
    {
        var action = inputActionReference.action;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isComposite && action.bindings[i].name == compositeName)
            {
                return i;
            }
        }
        return -1;
    }

    // In ra danh sách bindings
    private void LogBindings()
    {
        for (int i = 0; i < inputActionReference.action.bindings.Count; i++)
        {
            Debug.Log($"Binding {i}: {inputActionReference.action.bindings[i].path}");
        }
    }

    // Lưu tất cả binding overrides
    private void SaveBindings()
    {
        InputActionAsset asset = inputActionReference.action.actionMap.asset;
        var rebinds = asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(asset.name, rebinds);
        Debug.Log("Bindings saved successfully");

    }

    private void OnDestroy()
    {
        rebindOperation?.Dispose();
    }
}
