using System.Collections.Generic;

namespace WheelPicker
{
    public class MusicLibrary
    {
        public string Name { get; set; } = "";

        public List<MusicGenre> Genres { get; set; } = new();
    }

    public class MusicGenre
    {
        public string Name { get; set; } = "";

        public List<string> Subgenres { get; set; } = new();
    }
}