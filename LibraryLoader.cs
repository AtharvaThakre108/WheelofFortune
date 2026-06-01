using System.IO;
using System.Text.Json;
using System.Windows;

namespace WheelPicker
{
    public static class LibraryLoader
    {
        public static ItemLibrary Load(string path)
        {
            string json = File.ReadAllText(path);

            var library = JsonSerializer.Deserialize<ItemLibrary>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (library == null)
            {
                MessageBox.Show("Library is NULL");
                return new ItemLibrary();
            }

            return library;
        }
    }
}