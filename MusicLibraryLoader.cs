using System.IO;
using System.Text.Json;

namespace WheelPicker
{
    public static class MusicLibraryLoader
    {
        public static MusicLibrary Load(string path)
        {
            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<MusicLibrary>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new MusicLibrary();
        }
    }
}