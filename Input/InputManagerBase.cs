using System;
using UnityEngine;

namespace Codeaphobic 
{
    public class InputManagerBase : Singleton<InputManagerBase>
    {
        protected static GameInput inputActions;

        protected virtual Awake()
        {
            base.Awake();

            if (inputActions == null) inputActions = new GameInput();
            DontDestroyOnLoad(this.gameObject);
        }

        #region Control Interfaces

        public GameInput InputAction
        {
            get { return inputAction; }
        }

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

        public void SetClickAndReleaseCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
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

        public void UnsetClickAndReleaseCallback(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.canceled -= action;
        }

        #endregion
    }
}