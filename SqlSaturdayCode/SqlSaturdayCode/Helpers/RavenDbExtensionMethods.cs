namespace SqlSaturdayCode.Helpers
{
    public static class RavenDbExtensionMethods
    {
        public static int ToIntId(this string ravenId)
        {
            return int.Parse(ravenId.Split('/')[1]);
        }
    }
}