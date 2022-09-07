using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Phobebase.Player
{
    public class PlayerBase : MonoBehaviour
    {
        #region Initialisation

        protected Dictionary<Type, PlayerAddon> m_addons = new Dictionary<Type, PlayerAddon>();

        protected virtual void Awake()
        {
            foreach (PlayerAddon addon in GetComponents<PlayerAddon>())
            {
                Debug.Log("Addon Init");
                Type type = addon.Init(this);
                if (!m_addons.ContainsKey(type)) m_addons.Add(type, addon);
            }
        }

        #endregion
    }
}