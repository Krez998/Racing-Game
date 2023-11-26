
public static class Numbers
{
    public static readonly string[] GeneratedNumsStr = new string[10_000];

    public static void GenerateNums()
    {
        for (int i = 0; i < GeneratedNumsStr.Length; i++)
        {
            GeneratedNumsStr[i] = $"{i}";
        }
    }
}
