using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSpawnSettings", menuName = "ScriptableObjects/CustomerSpawnSettings")]
public class CustomerSpawnSettingData : ScriptableObject
{
    [SerializeField]
    private List<CustomerSpawnPeriod> spawnPeriodList;
    [SerializeField]
    private AnimationCurve _spawnRateCurve;

    public bool TryGetSpawnPeriod(float hour, out CustomerSpawnPeriod spawnPeriod)
    {
        foreach(CustomerSpawnPeriod period in spawnPeriodList)
        {
            if(!period.IsInPeriod(hour))
            {
                continue;
            }
            spawnPeriod = period;
            return true;
        }

        spawnPeriod = null;
        return false;
    }

    public float GetStoreLevelSpawnRate(int level)
    {
        float spawnRate = _spawnRateCurve.Evaluate(level);

        return Mathf.Max(1f, spawnRate);
    }
}
