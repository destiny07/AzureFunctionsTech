using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctionsTech.Api.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> DeserializeToObjectAsync<T>(this Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            var requestBody = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(requestBody);
        }
    }
}
