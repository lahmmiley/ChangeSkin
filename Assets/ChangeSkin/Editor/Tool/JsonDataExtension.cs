using LitJson;

namespace Tool
{
    public static class JsonDataExtension
    {
        public static bool ContainKey(this JsonData jsonData, string key)
        {
            return jsonData.Keys.Contains(key);
        }
    }
}
