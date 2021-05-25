namespace Infrastructure.Common
{
    public class Coordinate
    {
        public string Address { get; set; } 
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"Широта: {Latitude}. Долгота: {Longitude}. {Address}";
        }
    }
}