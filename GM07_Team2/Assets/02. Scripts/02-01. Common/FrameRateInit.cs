using UnityEngine;

public static class FrameRateInit
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
    }
}
