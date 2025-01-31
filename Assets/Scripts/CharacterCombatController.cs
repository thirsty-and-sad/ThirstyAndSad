using System.Collections.Generic;

public class Character
{
    public string Name;
    public int Sanity;
    public int MaxSanity;
    public List<Attack> Attacks;

    public Character(string name, int maxSanity, List<Attack> attacks)
    {
        Name = name;
        MaxSanity = maxSanity;
        Sanity = maxSanity;
        Attacks = attacks;
    }

    public void LoseSanity(int amount)
    {
        Sanity -= amount;
        if (Sanity < 0) Sanity = 0;
    }

    public bool IsInsane()
    {
        return Sanity <= 0;
    }
}
