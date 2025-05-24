using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Reflection;
using PlotDesigner.Runtime;

namespace BattleTrigger.Editor
{
    public class BaseVarField
    {
        public void Parse(JsonData nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(JsonData nodeJsonData)
        {

        }

        public void Create()
        {
            this.OnCreate();
        }

        public virtual void OnCreate()
        {

        }

        public virtual string GetDrawContentStr()
        {
            return "";
        }

        public virtual void DrawSelectInfo()
        {

        }

        public BaseVarField Clone()
        {
            return this.OnClone();
        }

        public virtual BaseVarField OnClone()
        {
            return null;
        }

        public JsonData ToJson()
        {
            return this.OnToJson();
        }

        public virtual JsonData OnToJson()
        {
            return null;
        }

        
    }
}