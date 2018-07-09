using BundleManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLineArgs : MonoBehaviour
{
    private static List<string> cmdLineArgs = null;
    private static List<string> CmdLineArgs
    {
        get
        {
            if (cmdLineArgs == null)
            {
#if UNITY_EDITOR
                cmdLineArgs = instance.testCmdLineArgs;
#else
                string[] args = System.Environment.GetCommandLineArgs();
                if (args.Length >= 2)
                {
                    cmdLineArgs = new List<string>(args);
                    cmdLineArgs.RemoveAt(0);
                }
                else
                    cmdLineArgs = new List<string>();
#endif
            }
            return cmdLineArgs;
        }
    }
    private static int ArgsCount { get { return CmdLineArgs.Count; } }

#if UNITY_EDITOR
    [SerializeField] private List<string> testCmdLineArgs = null;
    private static CommandLineArgs instance;
    private void Awake()
    {
        instance = this;
    }
#endif

    public static string GetArgument(int index)
    {
        List<string> l = CmdLineArgs;
        if (l != null && l.Count > index)
            return l[index];
        else
            return "";
    }

    [SerializeField] BundledSceneLoader bundleSceneLoader;

    private void Start()
    {
        bool load = false;
        int args = ArgsCount;
        for (int i = 0; i < args; i++)
        {
            if (i >= args) break;
            switch (GetArgument(i))
            {
                case "load":
                    if (load)
                        continue;
                    else
                        load = true;
                    string scene = GetArgument(++i);
                    if (scene != "")
                        bundleSceneLoader.LoadSceneFromBundle(scene);
                    break;
            }
        }
    }
}
