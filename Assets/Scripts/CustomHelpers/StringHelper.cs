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
        
        public static UnityEngine.Vector3 StringToVector3(string sourceVector_)
        {
            // Remove the parentheses
            if (sourceVector_.StartsWith ("(") && sourceVector_.EndsWith (")")) 
            {
                sourceVector_ = sourceVector_.Substring(1, sourceVector_.Length-2);
            }

            // split the items
            string[] sArray = sourceVector_.Split(',');

            // store as a Vector3
            UnityEngine.Vector3 result = new UnityEngine.Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
        
        public static bool TryParseVector3(string vectorString_, out UnityEngine.Vector3 result_)
        {
            // Remove the parentheses
            if (vectorString_.StartsWith ("(") && vectorString_.EndsWith (")")) 
            {
                vectorString_ = vectorString_.Substring(1, vectorString_.Length-2);
            }

            result_ = UnityEngine.Vector3.zero;
            string[] components = vectorString_.Split(',');

            if (components.Length == 3 &&
                float.TryParse(components[0], out float x) &&
                float.TryParse(components[1], out float y) &&
                float.TryParse(components[2], out float z))
            {
                result_ = new UnityEngine.Vector3(x, y, z);
                return true;
            }

            return false;
        }
        
        public static bool TryParseQuaternion(string quaternionString_, out UnityEngine.Quaternion result_)
        {
            if (quaternionString_.StartsWith ("(") && quaternionString_.EndsWith (")")) 
            {
                quaternionString_ = quaternionString_.Substring(1, quaternionString_.Length-2);
            }
            
            result_ = UnityEngine.Quaternion.identity;
            string[] components = quaternionString_.Split(',');

            if (components.Length == 4 &&
                float.TryParse(components[0], out float x) &&
                float.TryParse(components[1], out float y) &&
                float.TryParse(components[2], out float z) &&
                float.TryParse(components[3], out float w))
            {
                result_ = new UnityEngine.Quaternion(x, y, z, w);
                return true;
            }

            return false;
        }
        
        public static string SurroundWithColor(this string source_, UnityEngine.Color color_)
        {
            return $"<color=#{ColorToHex(color_)}>{source_}</color>";
        }

        public static string ColorToHex(UnityEngine.Color color)
        {
            UnityEngine.Color32 color32 = color;
            return $"{color32.r:X2}{color32.g:X2}{color32.b:X2}{color32.a:X2}";
        }
        
        public static string ReplaceEnterWithNewLine(this string source_)
        {
            return source_.Replace("\\n", "\n");
        }
        
        public static string ReplaceNewLineWithEnter(this string source_)
        {
            return source_.Replace("\n", "\\n");
        }
        
        // idk what to name this lmao
        public static string Beautify(this string source_)
        {
            return source_.Replace("\r", "");
        }
    }
}