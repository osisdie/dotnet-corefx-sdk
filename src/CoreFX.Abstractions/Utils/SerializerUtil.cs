using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreFX.Abstractions.Utils
{
    public static class SerializerUtil
    {
        public static JsonSerializerSettings DefaultJsonSetting = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static JsonSerializerSettings DefaultNotifyJsonSetting = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
        };

        public static string ToJson(this object src, JsonSerializerSettings setting = null)
        {
            if (src != null)
            {
                return JsonConvert.SerializeObject(src, setting ?? SerializerUtil.DefaultJsonSetting);
            }

            return string.Empty;
        }
    }
}
