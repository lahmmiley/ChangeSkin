using LitJson;

namespace Psd2UGUI
{
    public static class JsonDataExtension
    {
        public static bool ContainKey(this JsonData jsonData, string key)
        {
            return jsonData.Keys.Contains(key);
        }
    }
}
