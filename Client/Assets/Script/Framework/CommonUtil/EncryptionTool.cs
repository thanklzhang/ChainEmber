using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Security.Cryptography;

public class EncryptionTool
{

    public static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }


    ///// <summary>
    ///// 取Hash字符串
    ///// </summary>
    ///// <param name="sourceText">原文</param>
    ///// <param name="toUpper">返回大写字符串</param>
    ///// <returns>Hash字符串</returns>
    //public static string ComputeHashByMD5(string sourceText, bool toUpper = true)
    //{
    //    if (sourceText == null)
    //        return null;

    //    StringBuilder result = new StringBuilder();
    //    using (MD5 md5 = new MD5CryptoServiceProvider())
    //    {
    //        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceText));
    //        if (toUpper)
    //            for (int i = 0; i < data.Length; i++)
    //                result.Append(data[i].ToString("X2"));
    //        else
    //            for (int i = 0; i < data.Length; i++)
    //                result.Append(data[i].ToString("x2"));
    //    }

    //    return result.ToString();
    //}

    ///// <summary>
    ///// 取Hash字符串
    ///// </summary>
    ///// <param name="data">待加密数据字节结合</param>
    ///// <param name="toUpper"></param>
    ///// <returns></returns>
    //public static string ComputeHashByMD5(byte[] data, bool toUpper = true)
    //{
    //    if (data == null)
    //        return string.Empty;
    //    StringBuilder result = new StringBuilder();
    //    using (MD5 md5 = new MD5CryptoServiceProvider())
    //    {
    //        if (toUpper)
    //            for (int i = 0; i < data.Length; i++)
    //                result.Append(data[i].ToString("X2"));
    //        else
    //            for (int i = 0; i < data.Length; i++)
    //                result.Append(data[i].ToString("x2"));
    //    }

    //    return result.ToString();

    //}
}
