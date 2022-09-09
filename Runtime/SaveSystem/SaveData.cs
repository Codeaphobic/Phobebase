using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.SaveSystem
{
    // Inherit this for GameData to recognise your class as SaveData
    [System.Serializable]
    public class SaveData
    {
        public static IEnumerable<Type> GetAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(SaveData)));
        }
    }
}