using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BundleManagement
{
    public class BundleDatabaseBehaviour : MonoBehaviour
    {
        public BundleDatabase Database;

        [SerializeField] private string databaseFile { get {return  Configuration.Data.databaseFile; } }

        private void Awake()
        {
            Database = BundleDatabase.Load(databaseFile);
            Application.backgroundLoadingPriority = ThreadPriority.High;
        }

        private void OnApplicationQuit()
        {
            Database.Save(databaseFile);
        }

    }
}