using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using Phobebase.Common;

namespace Phobebase.Common.SaveSystem
{
    public class GameData
    {
        protected static GameData m_instance;
        protected static readonly object m_lock = new object();
        protected static List<ref SaveData> m_dataList = new List<ref SaveData>();

        // C# Singleton Pattern
        public static GameData Instance {
            get 
            { 
                if (m_instance != null)
                {
                    return m_instance;
                }

                lock (m_lock)
                {
                    if (m_instance == null)
                        m_instance = new GameData();
                }

                return m_instance;
            }
        }

        // Data List
        public static List<ref SaveData> DataList 
        {
            get { return m_dataList; }
        }

        // Saves all Data in the DataList
        public virtual void Save()
        {
            foreach(SaveData data in m_dataList)
            {
                Serialization.SaveToProtectedJsonFile($"/saves/", $"{data.GetType().ToString}.cdphbc", data);
            }
        }

        // Only Saves Data with Specified Type
        public virtual void Save(Type file)
        {
            foreach(SaveData data in m_dataList)
            {
                if (data.GetType() != fileType) continue; 
                Serialization.SaveToProtectedJsonFile($"/saves/", $"{data.GetType().ToString}.cdphbc", data);
            }
        }

        // Loads all Data Files
        // Idk how this is but its definitely slower then if u knew what u where saving 
        // Would recommend overriding this function to manually set each class 
        // but if your lazy this should work
        public virtual void Load()
        {
            SubClasses.GetAll<SaveData>().ToList().ForEach(x => x.Instance = Serialization.LoadProtectedJsonFile("/saves/", $"{x.GetType().ToString()}.cdphbc"));
        }

        // Loads only specified save data
        // This is alot better then above as it knows the class
        public virtual void Load<T>() where T : SaveData
        {
            T.Instance = Serialization.LoadProtectedJsonFile("/saves/", $"{T.GetType().ToString()}.cdphbc");
        }
    }
}