using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractLibrary
{
    public abstract class AbstractTank
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string PlaceOfOrigin { get; set; }
        public float Mass { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public int Crew { get; set; }
        public AbstractTank(string name, string type, string place, float mass, float length, float height, int crew)
        {
            Name = name;
            Type = type;
            PlaceOfOrigin = place;
            Mass = mass;
            Length = length;
            Height = height;
            Crew = crew;
        }

        public virtual string GetInfo()
        {
            string info = $"Name of the tank: {Name}\nType: {Type}\nPlaceOfOrigin: {PlaceOfOrigin}\nMass: {Mass}\n" +
                $"Length: {Length}\nHeight: {Height}\nCrew: {Crew}";

            return info;
        }
    }
}