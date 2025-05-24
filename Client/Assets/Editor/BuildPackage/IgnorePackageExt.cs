using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;


public class IgnorePackageExt
{
    private static readonly List<string> IgnoredAssetTypeExtension = new List<string>{
            string.Empty,
            ".manifest",
            ".meta",
            ".assetbundle",
            ".sample",
            ".unitypackage",
            ".cs",
            ".sh",
            ".js",
            ".zip",
            ".tar",
            ".tgz",
			#if UNITY_5_6 || UNITY_5_6_OR_NEWER
			#else
			".m4v",
			#endif
		};

    public static bool IsCanPackageAsset(string fileExtension)
    {
        return !IgnoredAssetTypeExtension.Contains(fileExtension);
    }
}

