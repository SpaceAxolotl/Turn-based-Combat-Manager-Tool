[System.Serializable]
public class Stat
{
    public string name;
    public int value;

    public Stat(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}