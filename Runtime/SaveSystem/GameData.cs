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
        protected static string p_save = "default";

        // Gets the SaveData file that you want from the dictionary
        // Use this to set data within the files
        public static T Get<T>() where T : SaveData
        {
            if (!m_dataList.ContainsKey(typeof(T))) m_dataList.Add(typeof(T), Activator.CreateInstance(typeof(T)) as SaveData);

            return (T) m_dataList[typeof(T)];
        }

        // Saves all Data in the DataList
        public static void Save(string save = null)
        {
            if (save != null) p_save = save;
            if (p_save == "") p_save = "default";

            foreach (SaveData data in m_dataList.Values)
            {
                if (data.GetType().IsDefined(typeof(SaveSlot), false)) 
                    Serialization.SaveToProtectedJsonFile($"/saves/", $"{p_save}-{data.GetType().ToString()}.cdphbc", data);
                else 
                    Serialization.SaveToProtectedJsonFile($"/saves/", $"{data.GetType().ToString()}.cdphbc", data);
            }
        }

        // Only Saves Data with Specified Type
        public static void Save(Type fileType, string save = null)
        {
            if (save != null) p_save = save;
            if (p_save == "") p_save = "default";

            if (m_dataList.ContainsKey(fileType)) m_dataList.Add(fileType, Activator.CreateInstance(fileType) as SaveData);

            if (fileType.IsDefined(typeof(SaveSlot), false)) 
                Serialization.SaveToProtectedJsonFile($"/saves/", $"{p_save}-{fileType.ToString()}.cdphbc", m_dataList[fileType]);
            else
                Serialization.SaveToProtectedJsonFile($"/saves/", $"{fileType.ToString()}.cdphbc", m_dataList[fileType]);
        }

        // Loads all Data Files
        // Idk how slow this is but its definitely slower then if u knew what u where loading 
        // Would recommend overriding this function to manually set each class 
        // but if your lazy or just want the simplicity this should work
        public static void Load(string save = null)
        {
            if (save != null) p_save = save;
            SaveData.GetAll().ToList().ForEach(x => LoadType(x));
        }

        private static void LoadType(Type type)
        {
            MethodInfo method = typeof(GameData).GetMethod(nameof(GameData.LoadSingle));
            MethodInfo generic = method.MakeGenericMethod(type);
            generic.Invoke(null, new object[]{ null });
        }

        // Loads only specified save data
        // This is prolly a better then above as it knows the class
        public static void LoadSingle<T>(string save = null) where T : SaveData
        {
            if (save != null) p_save = save;
            if (p_save == "") p_save = "default";

            if (!m_dataList.ContainsKey(typeof(T))) m_dataList.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);

            T saveData;

            if (typeof(T).IsDefined(typeof(SaveSlot), false))
                saveData = Serialization.LoadProtectedJsonFile<T>("/saves/", $"{p_save}-{typeof(T).ToString()}.cdphbc");
            else
                saveData = Serialization.LoadProtectedJsonFile<T>("/saves/", $"{typeof(T).ToString()}.cdphbc");

            if (saveData != null) m_dataList[typeof(T)] = saveData;
        }

        // Loads all slots of a specific class 
        // Use is for Getting save info to help player choose a save
        public static T[] LoadAllSlots<T>()
        {
            string[] dirs = Directory.GetFiles(Application.persistentDataPath + "/saves/", $"*-{typeof(T).ToString()}.cdphbc");

            #if UNITY_EDITOR
            dirs = Directory.GetFiles(Application.persistentDataPath + "/saves/dev/", $"*-{typeof(T).ToString()}.cdphbc");
            #endif

            T[] files = new T[dirs.Length];

            for (int i = 0; i < dirs.Length; i++)
            {
                files[i] = Serialization.LoadProtectedJsonFileFullPath<T>(dirs[i]);
            }

            return files;
        }
    }
}