using System.Collections.Generic;
using UnityEngine;

namespace BundleManagement
{
    [System.Serializable]
    public class BundleDatabase
    {
        public List<BundleData> bundleData;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }

        public void Save(string fileName)
        {
            JsonReader.SerializeAndWrite(this, fileName);
        }

        public static BundleDatabase Load(string fileName)
        {
            return JsonReader.ReadAndDeserialize<BundleDatabase>(fileName);
        }

        /*public string FindSceneBundle(string sceneName)
        {
            foreach (BundleData bd in bundleData)
            {
                foreach (string s in bd.sceneNames)
                {
                    if (sceneName == s)
                        return bd.dataBundleName;
                }
            }
            return null;
        }*/



    }
}