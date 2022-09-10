using UnityEngine;
using Phobebase.SaveSystem;

public class SaveSystemExample : MonoBehaviour
{
    public enum Save
    {
        SaveSlot1 = 0,
        SaveSlot2,
        SaveSlot3
    }

    public Save saveSlot = Save.SaveSlot1;

    public SettingsData settings;
    public PlayerData player;
    public EnemyData enemy;
}
