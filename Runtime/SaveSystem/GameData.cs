using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using Phobebase.Common;
using System.Reflection;

namespace Phobebase.SaveSystem
{
    public class GameData
    {
        protected static Dictionary<Type,SaveData> m_dataList = new Dictionary<Type, SaveData>();

        // Gets the SaveData file that you want from the dictionary
        // Use this to set data within the files
        public static T Get<T>() where T : SaveData
        {
            if (!m_dataList.ContainsKey(typeof(T))) m_dataList.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);

            return (T) m_dataList[typeof(T)];
        }

        // Saves all Data in the DataList
        public static void Save()
        {
            foreach(SaveData data in m_dataList.Values)
            {
                if (data == null) continue;
                Serialization.SaveToProtectedJsonFile($"/saves/", $"{data.GetType().ToString()}.cdphbc", data);
            }
        }

        // Only Saves Data with Specified Type
        public static void Save(Type fileType)
        {
            if (m_dataList.ContainsKey(fileType)) m_dataList.Add(fileType, Activator.CreateInstance(fileType) as SaveData);

            Serialization.SaveToProtectedJsonFile($"/saves/", $"{fileType.ToString()}.cdphbc", m_dataList[fileType]);
        }

        // Loads all Data Files
        // Idk how slow this is but its definitely slower then if u knew what u where loading 
        // Would recommend overriding this function to manually set each class 
        // but if your lazy or just want the simplicity this should work
        public static void Load()
        {
            SaveData.GetAll().ToList().ForEach(x => LoadType(x));
        }

        // DONT USE THIS ONE WHEN SAVING SINGLE FILES
        // use the one below will be alot faster
        private static void LoadType(Type type)
        {
            MethodInfo method = typeof(GameData).GetMethod(nameof(GameData.LoadSingle));
            MethodInfo generic = method.MakeGenericMethod(type);
            generic.Invoke(null, null);
        }

        // Loads only specified save data
        // This is prolly a better then above as it knows the class
        public static void LoadSingle<T>() where T : SaveData
        {
            if (!m_dataList.ContainsKey(typeof(T))) m_dataList.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);

            T saveData = Serialization.LoadProtectedJsonFile<T>("/saves/", $"{typeof(T).ToString()}.cdphbc");

            if (saveData != null) m_dataList[typeof(T)] = saveData;
        }
    }
}