using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using System.Linq;

namespace EditorUtils {

[FilePath("UserSettings/FpsCapperSettings.asset",
          FilePathAttribute.Location.ProjectFolder)]
public sealed class FpsCapperSettings : ScriptableSingleton<FpsCapperSettings>
{
    public int targetFrameRate = 60;
    public void Save() => Save(true);
    void OnDisable() => Save();
}

sealed class FpsCapperSettingsProvider : SettingsProvider
{
    public FpsCapperSettingsProvider()
      : base("Project/FPS Capper", SettingsScope.Project) {}

    static readonly string _helpText =
      "To enable this feature, the VSync switch in the Game View " +
      "needs to be turned off.";

    public override void OnGUI(string search)
    {
        var settings = FpsCapperSettings.instance;
        var fps = settings.targetFrameRate;

        EditorGUI.BeginChangeCheck();

        fps = EditorGUILayout.DelayedIntField("Target Frame Rate", fps);

        if (EditorGUI.EndChangeCheck())
        {
            settings.targetFrameRate = fps;
            settings.Save();
        }

        EditorGUILayout.HelpBox(_helpText, MessageType.None);
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider()
      => new FpsCapperSettingsProvider();
}

[UnityEditor.InitializeOnLoad]
sealed class FpsCapperSystem
{
    static FpsCapperSystem()
      => InsertPlayerLoopSystem();

    static bool IsCapturing
      => Time.captureDeltaTime != 0;

    static int TargetFps
      => FpsCapperSettings.instance.targetFrameRate;

    static void UpdateTargetFrameRate()
      => Application.targetFrameRate = IsCapturing ? -1 : TargetFps;

    static void InsertPlayerLoopSystem()
    {
        var system = new PlayerLoopSystem()
          { type = typeof(FpsCapperSystem),
            updateDelegate = UpdateTargetFrameRate };

        var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        for (var i = 0; i < playerLoop.subSystemList.Length; i++)
        {
            ref var phase = ref playerLoop.subSystemList[i];
            if (phase.type == typeof(UnityEngine.PlayerLoop.EarlyUpdate))
            {
                phase.subSystemList
                  = phase.subSystemList.Concat(new[]{system}).ToArray();
                break;
            }
        }

        PlayerLoop.SetPlayerLoop(playerLoop);
    }
}

} // namespace EditorUtils
