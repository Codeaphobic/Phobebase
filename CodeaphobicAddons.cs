using System;
using System.Theading;
using UnityEngine;

namespace Codeaphobic
{
    public class Singleton : MonoBehaviour 
    {
        public static Singleton instance { get; protected set; }

        private virtual void Awake() {
            if (Singleton.instance != null && instance != this)
            {
                Destory(this);
            }
            else 
            {
                instance = this;
            }
        }
    }
}