using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Bundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}