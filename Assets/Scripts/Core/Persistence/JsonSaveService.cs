using System;
using System.IO;
using UnityEngine;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Persistence
{
    public class JsonSaveService : ISaveService
    {
        private readonly string _filePath;

        public JsonSaveService(string filePath)
        {
            _filePath = filePath;
        }

        public JsonSaveService()
            : this(Path.Combine(Application.persistentDataPath, "save.json"))
        {
        }

        public void Save(CycleState state)
        {
            try
            {
                string json = JsonUtility.ToJson(state);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to save game state: {e.Message}");
            }
        }

        public CycleState Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return null;

                string json = File.ReadAllText(_filePath);
                var state = JsonUtility.FromJson<CycleState>(json);

                if (state == null || state.CycleSequence == null || state.CycleSequence.Length == 0)
                    return null;

                return state;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load game state: {e.Message}");
                return null;
            }
        }

        public void Delete()
        {
            try
            {
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to delete save file: {e.Message}");
            }
        }
    }
}
