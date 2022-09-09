using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Common
{
    // Inherit this for GameData to recognise your class as SaveData
    [System.Serializable]
    public class SaveData<T> where T : SaveData<T>
    {
        private static T m_data;

        // C# Singleton Pattern Will call Init Function if its just been created
        public static T Data 
        {
            get 
            { 
                if (m_instance != null)
                {
                    return m_instance;
                }

                lock (m_lock)
                {
                    if (m_instance == null) 
                    {
                        m_instance = new T();
                        Init();
                    }
                }

                return m_instance;
            }
        }

        // Checks if its already in the list if not adds a reference of itself
        // To a list in GameData
        private void Init()
        {
            if (!GameData.Instance.DataList.Contains(m_data))
            {
                GameData.Instance.DataList.Add(ref m_data);
            }
        }
    }
}