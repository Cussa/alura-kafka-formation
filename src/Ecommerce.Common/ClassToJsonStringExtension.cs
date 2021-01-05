using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public static class ClassToJsonStringExtension
    {
        public static string ToJsonString(this object data) => JsonConvert.SerializeObject(data);
    }
}
