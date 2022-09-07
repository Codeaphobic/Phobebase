using System;
using UnityEngine;

namespace Phobebase.Input
{
    public class InputManagerBase : Singleton<InputManagerBase>
    {
        // Unity Input ref
        protected static GameInput inputActions;

        protected virtual Awake()
        {
            base.Awake();

            if (inputActions == null) inputActions = new GameInput();
            DontDestroyOnLoad(this.gameObject);
        }

        #region Control Interfaces

        // public reference to the input actions
        public GameInput InputAction
        {
            get { return inputAction; }
        }

        // A Heap of Functions to Bind to actions in different ways
        // Just makes code in other classes a little cleaner

        public void SetStartedCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
        }

        public void SetPerformedCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.performed += action;
        }

        public void SetCanceledCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.canceled += action;
        }

        public void SetStartedAndCanceledCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.canceled += action;
        }

        public void UnsetStartedCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
        }

        public void UnsetPerformedCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.performed -= action;
        }

        public void UnsetCanceledCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.canceled -= action;
        }

        public void UnsetStartedAndCanceledCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.canceled -= action;
        }

        #endregion
    }
}