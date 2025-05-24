using System;
using System.Reflection;

public static class EnumExtends
{
    public static string GetLabel(this Enum e)
    {
        var str = "";
        FieldInfo[] fields = e.GetType().GetFields();
        var value = e.GetHashCode();

        foreach (FieldInfo field in fields)
        {
            object[] objs = field.GetCustomAttributes(typeof(EnumLabelAttribute), true);
            if (objs != null && objs.Length > 0)
            {
                var lable = ((EnumLabelAttribute)objs[0]).label;
                var currValue = (int)field.GetValue(e);
                if (currValue == value)
                {
                    str = lable;
                    break;
                }
            }
        }
        return str;
    }
}