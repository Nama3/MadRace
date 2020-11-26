using System;
using System.IO;
using EncryptionTool;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SavedDataType : byte
{
    PlayerData,
    SettingsData
}

#region Data

[Serializable]
public struct PlayerData
{
    public string playerName;
    
    public uint coinsAmount;

    public byte levelIndex;
}

[Serializable]
public struct SettingsData
{
    public bool mute;
}

#endregion /Data

public class SavedDataServices : MonoBehaviour
{
    public const string GAME_NAME = "CarpenterRun";

    public static readonly string PlayerDataFileName = $"{GAME_NAME}PlayerData";
    public static readonly string SettingsDataFileName = $"{GAME_NAME}SettingsData";

    private static bool m_init;

    private static string m_playerDataPath, m_settingsDataPath;

    private static bool m_savePlayer, m_saveSettings;

    private static PlayerData m_playerData;
    private static SettingsData m_settingsData;

    public static PlayerData PlayerData
    {
        get
        {
            if (!m_init) InitManagedPlayerData(true);
            return m_playerData;
        }
    }
    
    #region Static Access

    #region PlayerData

    public static string PlayerName
    {
        get
        {
            if (string.IsNullOrEmpty(PlayerData.playerName))
                PlayerName = "Player";
            return PlayerData.playerName;
        }
        set
        {
            if (PlayerData.playerName == value) return;

            m_playerData.playerName = value;

            m_savePlayer = true;
            SaveToLocal(SavedDataType.PlayerData);
        }
    }

    public static uint CoinsAmount
    {
        get => PlayerData.coinsAmount;
        set
        {
            if (PlayerData.coinsAmount == value) return;

            m_playerData.coinsAmount = value;

            m_savePlayer = true;
            SaveToLocal(SavedDataType.PlayerData);
        }
    }

    public static byte LevelIndex
    {
        get => PlayerData.levelIndex;
        set
        {
            if (PlayerData.levelIndex == value) return;

            m_playerData.levelIndex = value;

            m_savePlayer = true;
            SaveToLocal(SavedDataType.PlayerData);
        }
    }

    #endregion /PlayerData

    #region SettingsData

    public static bool Mute
    {
        get => m_settingsData.mute;
        set
        {
            if (m_settingsData.mute == value) return;

            m_saveSettings = true;
            m_settingsData.mute = value;
        }
    }

    #endregion /SettingsData

    #endregion /Static Access

    #region Utilities

    #region IO

    private static string GetPath(SavedDataType dataType)
    {
        switch (dataType)
        {
            case SavedDataType.PlayerData:
                if (string.IsNullOrEmpty(m_playerDataPath))
                    m_playerDataPath = $"{Path.Combine(Application.persistentDataPath, PlayerDataFileName)}.save";
                return m_playerDataPath;
            case SavedDataType.SettingsData:
                if (string.IsNullOrEmpty(m_settingsDataPath))
                    m_settingsDataPath = $"{Path.Combine(Application.persistentDataPath, SettingsDataFileName)}.save";
                return m_settingsDataPath;
            default:
                return null;
        }
    }

    private static void CreateFile(SavedDataType dataType)
    {
        switch (dataType)
        {
            case SavedDataType.PlayerData:
                File.WriteAllText(GetPath(SavedDataType.PlayerData), Encryption.Encrypt(DataToJson(SavedDataType.PlayerData)));
                break;
            case SavedDataType.SettingsData:
                File.WriteAllText(GetPath(SavedDataType.SettingsData), DataToJson(SavedDataType.SettingsData));
                break;
        }
    }

    #endregion /IO

    #region Json

    private static string DataToJson(SavedDataType dataType)
    {
        switch (dataType)
        {
            case SavedDataType.PlayerData:
                return JsonUtility.ToJson(m_playerData);
            case SavedDataType.SettingsData:
                return JsonUtility.ToJson(m_settingsData);
            default:
                return null;
        }
    }

    #endregion /Json

    #endregion /Utilities

    #region Init

    public static void Init(EncryptionInitializer encryptionInitializer) => Encryption.Init(encryptionInitializer);

    private static void InitManagedPlayerData(bool checkIfNull)
    {
        m_init = true;
        if (checkIfNull)
        {
            // Load Managed Data if Null
            return;
        }

        // Always Load Managed Data
    }

    #endregion /Init

    #region LoadModel & Save

    private static bool UpdatePlayerData(PlayerData playerData)
    {
        var localKept = false;

        // Compare and update each fields and choose whether to keel local or not

        return localKept;
    }

    public static void LoadEverythingFromLocal()
    {
        LoadFromLocal(SavedDataType.PlayerData);
        LoadFromLocal(SavedDataType.SettingsData);
    }

    public static void LoadFromLocal(SavedDataType dataType)
    {
        var path = GetPath(dataType);

        if (!File.Exists(path))
        {
            if (dataType == SavedDataType.PlayerData) InitManagedPlayerData(false);

            CreateFile(dataType);
            return;
        }

        try
        {
            switch (dataType)
            {
                case SavedDataType.PlayerData:
                    m_playerData = JsonUtility.FromJson<PlayerData>(Encryption.Decrypt(File.ReadAllText(path)));
                    InitManagedPlayerData(true);
                    break;
                case SavedDataType.SettingsData:
                    m_settingsData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(path));
                    break;
            }
        }
        catch
        {
            if (dataType == SavedDataType.PlayerData)
                InitManagedPlayerData(true);
            SaveToLocal(dataType);
        }

#if UNITY_EDITOR
        Debug.Log(path);
        Debug.Log(DataToJson(dataType));
#endif
    }

    public static void SavePlayerToLocal() => SaveToLocal(SavedDataType.PlayerData);

    public static void SavesSettingsToLocal() => SaveToLocal(SavedDataType.SettingsData);

    public static void SaveToLocal(SavedDataType dataType)
    {
        switch (dataType)
        {
            case SavedDataType.PlayerData:
                if (!m_savePlayer) return;
                m_savePlayer = false;
                break;
            case SavedDataType.SettingsData:
                if (!m_saveSettings) return;
                m_saveSettings = false;
                break;
        }

        CreateFile(dataType);
    }

    #endregion /Load & Save

    #region Reset

    public static void ResetData(SavedDataType dataType)
    {
        switch (dataType)
        {
            case SavedDataType.PlayerData:
                ResetPlayerData();
                break;
            case SavedDataType.SettingsData:
                ResetSettingsData();
                break;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Saved Data Services/Reset Player Data", false, 11)]
#endif
    public static void ResetPlayerData()
    {
        m_playerData = new PlayerData();
        InitManagedPlayerData(false);
        var path = GetPath(SavedDataType.PlayerData);
        if (!File.Exists(path)) return;
        File.Delete(path);
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Saved Data Services/Reset Settings Data", false, 12)]
#endif
    public static void ResetSettingsData()
    {
        m_settingsData = new SettingsData();
        var path = GetPath(SavedDataType.SettingsData);
        if (!File.Exists(path)) return;
        File.Delete(path);
    }

    #endregion /Reset
}