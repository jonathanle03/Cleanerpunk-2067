using UnityEngine;

[System.Serializable]
public class DamageResistances
{
    [Header("Physical Damage Resistances")]
    [Tooltip("Multiplier for Slashing damage (1.0 = normal, 0.5 = resistant, 1.5 = vulnerable)")]
    public float slashingResistance = 1.0f;

    [Tooltip("Multiplier for Impact damage (1.0 = normal, 0.5 = resistant, 1.5 = vulnerable)")]
    public float impactResistance = 1.0f;

    [Tooltip("Multiplier for Ballistic damage (1.0 = normal, 0.5 = resistant, 1.5 = vulnerable)")]
    public float ballisticResistance = 1.0f;

    [Header("Neural Damage Resistance")]
    [Tooltip("Multiplier for Neural damage (1.0 = normal, 0.5 = resistant, 1.5 = vulnerable)")]
    public float neuralResistance = 1.0f;

    /// <summary>
    /// Get the resistance multiplier for a specific damage type
    /// </summary>
    public float GetResistance(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Slashing:
                return slashingResistance;
            case DamageType.Impact:
                return impactResistance;
            case DamageType.Ballistic:
                return ballisticResistance;
            case DamageType.Neural:
                return neuralResistance;
            default:
                return 1.0f;
        }
    }

    /// <summary>
    /// Set the resistance multiplier for a specific damage type
    /// </summary>
    public void SetResistance(DamageType damageType, float value)
    {
        switch (damageType)
        {
            case DamageType.Slashing:
                slashingResistance = value;
                break;
            case DamageType.Impact:
                impactResistance = value;
                break;
            case DamageType.Ballistic:
                ballisticResistance = value;
                break;
            case DamageType.Neural:
                neuralResistance = value;
                break;
        }
    }
}
