using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.SaveSystem
{
   [CustomEditor(typeof(SaveHandler))]
   public class SaveHandlerInspector : UnityEditor.Editor
   {
      private const string GENERATE_BUTTON_TEXT = "Generate GUID";
      
      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();
         
         var saveHandler = (SaveHandler)target;

         if (GUILayout.Button(GENERATE_BUTTON_TEXT))
         {
            saveHandler.GenerateId();
            EditorUtility.SetDirty(saveHandler);
            serializedObject.ApplyModifiedProperties();
         }
      }
   }
}