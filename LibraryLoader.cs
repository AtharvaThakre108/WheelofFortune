using System.IO;
using System.Text.Json;

namespace WheelPicker
{
    public static class LibraryLoader
    {
        public static Library Load(string path)
        {
            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<Library>(json)!
                   ?? new Library();
        }
    }
}