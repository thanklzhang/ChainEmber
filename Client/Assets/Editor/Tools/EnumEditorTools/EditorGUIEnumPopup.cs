using UnityEditor;
using System;
using System.Reflection;
using UnityEngine;

public class EditorGUILayout_Ex : EditorWindow
{
    public static object EnumPopup(Enum selected, params GUILayoutOption[] options)
    {
        int currSelectIndex = 0;

        FieldInfo[] fields = selected.GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
        string[] enumString = new string[fields.Length];
        int[] enumValue = new int[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            object[] objs = field.GetCustomAttributes(typeof(EnumLabelAttribute), true);
            if (objs != null && objs.Length > 0)
            {
                enumString[i] = ((EnumLabelAttribute)objs[0]).label;
            }
            else
            {
                enumString[i] = field.Name;
            }

            enumValue[i] = (int)field.GetValue(selected);
            if (selected.GetHashCode() == enumValue[i])
            {
                currSelectIndex = i;
            }
        }

        var index = EditorGUILayout.Popup(currSelectIndex, enumString, options);
        var resultValue = enumValue[index];

        return Enum.ToObject(selected.GetType(), resultValue);
    }


    //public static object EnumPopup(Enum selected, params GUILayoutOption[] options)
    //{
    //    int index = 0;
    //    var array = Enum.GetValues(selected.GetType());
    //    int length = array.Length;
    //    int currSelectIndex = 0;

    //    string[] enumString = new string[length];
    //    int[] enumValue = new int[length];
    //    for (int i = 0; i < length; i++)
    //    {
    //        //直接用这个顺序也是对的 为什么还用双层循环？？
    //        FieldInfo[] fields = selected.GetType().GetFields();
    //        foreach (FieldInfo field in fields)
    //        {
    //            var keyStr = array.GetValue(i).ToString();
    //            //Debug.LogError(" kk : " + keyStr);
    //            if (field.Name.Equals(keyStr))
    //            {
    //                //Debug.LogError(" ff : " + field.Name);
    //                object[] objs = field.GetCustomAttributes(typeof(EnumLabelAttribute), true);
    //                if (objs != null && objs.Length > 0)
    //                {
    //                    enumString[i] = ((EnumLabelAttribute)objs[0]).label;
    //                }
    //                else
    //                {
    //                    enumString[i] = field.Name;
    //                }
    //                enumValue[i] = (int)field.GetValue(selected);
    //                if (selected.GetHashCode() == enumValue[i])
    //                {
    //                    currSelectIndex = i;
    //                }
    //            }
    //        }
    //    }

    //    EditorGUILayout.BeginHorizontal();

    //    index = EditorGUILayout.Popup(currSelectIndex, enumString, options);
    //    EditorGUILayout.EndHorizontal();

    //    var value = enumValue[index];

    //    return Enum.ToObject(selected.GetType(), value);
    //}

}