using UnityEngine;

public static class DamageCalculator
{
    /// <summary>
    /// Calculate damage based on damage type, piercing flag, and stats
    /// </summary>
    public static float CalculateDamage(DamageType damageType, bool isPiercing, float cardPower, float globalMultiplier, Entity attacker, Entity target)
    {
        float baseDamage = 0f;
        bool isPhysical = (damageType == DamageType.Slashing || damageType == DamageType.Impact || damageType == DamageType.Ballistic);

        if (isPhysical) // Physical
        {
            if (isPiercing)
            {
                baseDamage = CalculatePiercingPhysicalDamage(attacker.attack);
            }
            else
            {
                baseDamage = CalculatePhysicalDamage(attacker.attack, target.defense);
            }
        }
        else // Neural
        {
            if (isPiercing)
            {
                baseDamage = CalculatePiercingNeuralDamage(attacker.force);
            }
            else
            {
                baseDamage = CalculateNeuralDamage(attacker.force, target.shield);
            }
        }

        // Multiply by card power
        baseDamage *= cardPower;

        // Apply global damage multiplier
        baseDamage *= globalMultiplier;

        // Apply resistance/vulnerability
        float resistanceMultiplier = target.damageResistances.GetResistance(damageType);
        baseDamage *= resistanceMultiplier;

        return baseDamage;
    }

    /// <summary>
    /// Physical Damage = min(ATK^1.7, ATK^0.5 * 1.28 * 5000^(ATK / 1200)^0.224965, ATK^0.5 * 6400) / DEF^0.5
    /// </summary>
    private static float CalculatePhysicalDamage(int attack, int defense)
    {
        float atk = (float)attack;
        float def = (float)defense;

        // Calculate three components
        float component1 = Mathf.Pow(atk, 1.7f);
        float component2 = Mathf.Pow(atk, 0.5f) * 1.28f * Mathf.Pow(5000f, Mathf.Pow(atk / 1200f, 0.224965f));
        float component3 = Mathf.Pow(atk, 0.5f) * 6400f;

        // Take minimum of the three
        float numerator = Mathf.Min(component1, Mathf.Min(component2, component3));

        // Divide by DEF^0.5
        float denominator = Mathf.Pow(def, 0.5f);

        return numerator / denominator;
    }

    /// <summary>
    /// Neural Damage = min(FRC^1.6, FRC^0.5 * 1.28 * 2500^(FRC / 1500)^0.153587, FRC^0.5 * 3200) / SHD^0.5
    /// </summary>
    private static float CalculateNeuralDamage(int force, int shield)
    {
        float frc = (float)force;
        float shd = (float)shield;

        // Calculate three components
        float component1 = Mathf.Pow(frc, 1.6f);
        float component2 = Mathf.Pow(frc, 0.5f) * 1.28f * Mathf.Pow(2500f, Mathf.Pow(frc / 1500f, 0.153587f));
        float component3 = Mathf.Pow(frc, 0.5f) * 3200f;

        // Take minimum of the three
        float numerator = Mathf.Min(component1, Mathf.Min(component2, component3));

        // Divide by SHD^0.5
        float denominator = Mathf.Pow(shd, 0.5f);

        return numerator / denominator;
    }

    /// <summary>
    /// Piercing Physical Damage = ATK^0.9
    /// </summary>
    private static float CalculatePiercingPhysicalDamage(int attack)
    {
        return Mathf.Pow((float)attack, 0.9f);
    }

    /// <summary>
    /// Piercing Neural Damage = FRC^0.825
    /// </summary>
    private static float CalculatePiercingNeuralDamage(int force)
    {
        return Mathf.Pow((float)force, 0.825f);
    }

    /// <summary>
    /// Apply random variance to final damage as a flat value + percentage spread
    /// </summary>
    public static float ApplyDamageVariance(float baseDamage, float flat = 5f, float pct = 0.03f)
    {
        if (baseDamage <= 0f) return baseDamage;

        float range = flat + pct * baseDamage;
        float u = Random.value - Random.value;      // triangular [-1,1]
        float finalDamage = baseDamage + u * range;

        return Mathf.Max(1f, finalDamage);
    }
}
