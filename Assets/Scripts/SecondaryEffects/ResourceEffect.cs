[System.Serializable]
public class ResourceEffect
{
    public ResourceDefinition resource; // e.g. Health, Mana
    public int amountPerTick;           // Negative = damage, Positive = regen
}