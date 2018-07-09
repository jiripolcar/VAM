using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BundleManagement
{
    public class BundleDatabaseBehaviour : MonoBehaviour
    {
        public BundleDatabase Database;

        [SerializeField] private string databaseFile = "database.json";

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