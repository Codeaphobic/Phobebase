using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Phobebase.SaveSystem;

[CustomEditor(typeof(SaveSystemExample))]
public class SaveSystemExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Grabs all the Player Data files to help the player know whats in each save
        PlayerData[] data = GameData.LoadAllSlots<PlayerData>();
        GUILayout.Label($"Saves:");
        for (int i = 0; i < data.Length; i++)
        {
            GUILayout.Label($"        Slot {i}: Lollies = {data[i].lollies}");
        }

        GUILayout.Space(25);

        DrawDefaultInspector();

        SaveSystemExample script = (SaveSystemExample)target;

        // Binds Variables
        script.settings = GameData.Get<SettingsData>();
        script.player = GameData.Get<PlayerData>();
        script.enemy = GameData.Get<EnemyData>();

        GUILayout.Space(25);

        if (GUILayout.Button("Save"))
        {
            // Saves to current SaveSlot
            GameData.Save(script.saveSlot.ToString());
        }
        if (GUILayout.Button("Load"))
        {
            // Loads and grabs the loaded data 
            // Not nessesary if you are just altering data within the Get function
            GameData.Load(script.saveSlot.ToString());
            script.settings = GameData.Get<SettingsData>();
            script.player = GameData.Get<PlayerData>();
            script.enemy = GameData.Get<EnemyData>();
        }
    }
}
