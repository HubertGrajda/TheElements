using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.SaveSystem
{
    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerInspector : UnityEditor.Editor
    {
        private const string SAVE_BUTTON_TEXT = "Save";
        private const string LOAD_BUTTON_TEXT = "Load";
        private const string DELETE_BUTTON_TEXT = "Delete";
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            var saveManager = (SaveManager)target;

            if (GUILayout.Button(SAVE_BUTTON_TEXT))
            {
                saveManager.Save();
            }

            if (GUILayout.Button(LOAD_BUTTON_TEXT))
            {
                saveManager.Load();
            }

            if (GUILayout.Button(DELETE_BUTTON_TEXT))
            {
                saveManager.DeleteSave();
            }
        }
    }
}