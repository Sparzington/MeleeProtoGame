//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Inputs/FightControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @FightControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @FightControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""FightControls"",
    ""maps"": [
        {
            ""name"": ""PlayerStance"",
            ""id"": ""f8c04c9a-5958-488d-b81a-07ce765f0b50"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""c668116e-8503-47a6-828c-ef686f6b9943"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""63aecb9e-5528-45f6-81a0-5f890d7fcfbe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e1bbe94e-0eda-4fc3-8b3d-efd09ee0d31b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f786015-51f3-4928-9dcf-c51f78af55d8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3671a4b-c197-41e0-86ae-fd4b188fb017"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04da0420-ed42-47d9-9008-6f9665b73e98"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerStance
        m_PlayerStance = asset.FindActionMap("PlayerStance", throwIfNotFound: true);
        m_PlayerStance_LightAttack = m_PlayerStance.FindAction("LightAttack", throwIfNotFound: true);
        m_PlayerStance_HeavyAttack = m_PlayerStance.FindAction("HeavyAttack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerStance
    private readonly InputActionMap m_PlayerStance;
    private List<IPlayerStanceActions> m_PlayerStanceActionsCallbackInterfaces = new List<IPlayerStanceActions>();
    private readonly InputAction m_PlayerStance_LightAttack;
    private readonly InputAction m_PlayerStance_HeavyAttack;
    public struct PlayerStanceActions
    {
        private @FightControls m_Wrapper;
        public PlayerStanceActions(@FightControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_PlayerStance_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_PlayerStance_HeavyAttack;
        public InputActionMap Get() { return m_Wrapper.m_PlayerStance; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerStanceActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerStanceActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerStanceActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerStanceActionsCallbackInterfaces.Add(instance);
            @LightAttack.started += instance.OnLightAttack;
            @LightAttack.performed += instance.OnLightAttack;
            @LightAttack.canceled += instance.OnLightAttack;
            @HeavyAttack.started += instance.OnHeavyAttack;
            @HeavyAttack.performed += instance.OnHeavyAttack;
            @HeavyAttack.canceled += instance.OnHeavyAttack;
        }

        private void UnregisterCallbacks(IPlayerStanceActions instance)
        {
            @LightAttack.started -= instance.OnLightAttack;
            @LightAttack.performed -= instance.OnLightAttack;
            @LightAttack.canceled -= instance.OnLightAttack;
            @HeavyAttack.started -= instance.OnHeavyAttack;
            @HeavyAttack.performed -= instance.OnHeavyAttack;
            @HeavyAttack.canceled -= instance.OnHeavyAttack;
        }

        public void RemoveCallbacks(IPlayerStanceActions instance)
        {
            if (m_Wrapper.m_PlayerStanceActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerStanceActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerStanceActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerStanceActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerStanceActions @PlayerStance => new PlayerStanceActions(this);
    public interface IPlayerStanceActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
    }
}