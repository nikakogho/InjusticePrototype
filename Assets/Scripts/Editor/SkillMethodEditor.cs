using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SkillMethod))]
public class SkillMethodDrawer : PropertyDrawer
{
    string[] GetOptions(MonoScript script)
    {
        var type = script.GetClass();
        
        var methodInfos = type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        List<string> methodNames = new List<string>();

        foreach (var info in methodInfos)
        {
            var parameters = info.GetParameters();

            if (parameters.Length != 1) continue;

            if (parameters[0].ParameterType != typeof(Player)) continue;

            methodNames.Add(info.Name);
        }

        return methodNames.ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var scriptProperty = property.FindPropertyRelative("script");

        var _scriptProperty = property.FindPropertyRelative("oldScript");
        
        MonoScript oldScript = scriptProperty.objectReferenceValue as MonoScript;

        EditorGUI.BeginProperty(new Rect(position.x, position.y, position.width * 2, position.height), GUIContent.none, property);

        Rect scriptPosition = new Rect(position.x, position.y, position.width / 2, position.height);

        _scriptProperty.objectReferenceValue = EditorGUI.ObjectField(scriptPosition, GUIContent.none, _scriptProperty.objectReferenceValue, typeof(MonoScript), false);

        MonoScript script = _scriptProperty.objectReferenceValue as MonoScript;

        var indexProp = property.FindPropertyRelative("methodIndex");

        #region Change Of Script

        if (script != oldScript)
        {
            var methodArrayProperty = property.FindPropertyRelative("possibleMethods");

            methodArrayProperty.ClearArray();

            indexProp.intValue = 0;

            if (script == null)
            {

            }
            else
            {
                string[] options = GetOptions(script);
                
                for(int i = 0; i < options.Length; i++)
                {
                    methodArrayProperty.InsertArrayElementAtIndex(i);
                    methodArrayProperty.GetArrayElementAtIndex(i).stringValue = options[i];
                }
            }

            scriptProperty.objectReferenceValue = script;
            oldScript = script;
        }

        #endregion

        if(scriptProperty.objectReferenceValue == null)
        {
            //something?
        }
        else
        {
            Rect maskPosition = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
            indexProp.intValue = EditorGUI.Popup(maskPosition, indexProp.intValue, GetOptions(script));
        }

        EditorGUI.EndProperty();
    }
}