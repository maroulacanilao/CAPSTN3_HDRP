namespace CustomHelpers
{
    public static class StringHelpers
    {
        private static System.Collections.Generic.Dictionary<string, int> StringHashDictionary;
        public static string SplitCamelCase(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            var sb = new System.Text.StringBuilder();

            foreach (var c in source)
            {
                if (char.IsUpper(c)) sb.Append(' ');

                sb.Append(c);
            }

            return sb.ToString();
        }

        public static int ToHash(this string value)
        {
            if (StringHashDictionary == null)
            {
                StringHashDictionary = new System.Collections.Generic.Dictionary<string, int>();
            }

            if (StringHashDictionary.TryGetValue(value, out var hashID))
            {
                return hashID;
            }

            var newHashID = value.GetHashCode();
            StringHashDictionary.Add(value, newHashID);
            return newHashID;
        }

    }
}