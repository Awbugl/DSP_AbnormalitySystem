namespace DSP_AbnormalitySystem
{
    public enum AbnormalityType
    {
        None, // Should not be used
        Item,
        Vein,
        Entity,
        Planet,
        Star,
        Galaxy,
        Player
    }
    
    public enum PlanetAbnormalitySubType
    {
        None, // Should not be used
        Terrestrial,
        GasGiant
    }
    
    public enum StarAbnormalitySubType
    {
        None, // Should not be used
        NormalStar,
        BlackHole,
        NeutronStar,
    }
}
