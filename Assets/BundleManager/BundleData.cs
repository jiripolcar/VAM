using System.Collections.Generic;

namespace BundleManagement
{
    [System.Serializable]
    public class BundleData
    {
        public string sceneName;
        public List<string> dependencies;
        public string sceneBundleName;


        /*public string dataBundleName;
        public string sceneBundleName;
        public List<string> sceneNames;*/
    }
}