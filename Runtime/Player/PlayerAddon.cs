using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phobebase.Player 
{
    [RequireComponent(typeof(Player))]
    public abstract class PlayerAddon : MonoBehaviour
    {
        private PlayerBase m_playerbase;
        protected T Player<T>() {
            return (T) m_playerbase;
        }

        public Type Init(PlayerBase player)
        {
            if (m_playerbase == null) m_playerbase = player;
            return this.GetType();
        }
    }
}