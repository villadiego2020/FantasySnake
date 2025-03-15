using UnityEditor;
using System.IO;

public class BuildScript
{
    public static void BuildClient()
    {
        string buildPath = "Builds/Windows/Client/";
        if (!Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath + "GameClient.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    public static void BuildServer()
    {
        string buildPath = "Builds/Windows/Server/";
        if (!Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath + "GameServer.exe", BuildTarget.StandaloneWindows64, BuildOptions.EnableHeadlessMode);
    }
}
