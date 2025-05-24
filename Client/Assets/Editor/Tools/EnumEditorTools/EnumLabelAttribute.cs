using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
public class EnumLabelAttribute : PropertyAttribute
{
    public string label;
    public new int[] order = new int[0];
    public EnumLabelAttribute(string label)
    {
        this.label = label;
    }

    public EnumLabelAttribute(string label, params int[] order)
    {
        this.label = label;
        this.order = order;
    }

}