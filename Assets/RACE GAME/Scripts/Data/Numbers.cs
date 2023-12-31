
public static class Numbers
{
    public static readonly string[] CachedNums = new string[10000];

    public static void GenerateNums()
    {
        for (int i = 0; i < CachedNums.Length; i++)
        {
            CachedNums[i] = $"{i}";
        }
    }
}
