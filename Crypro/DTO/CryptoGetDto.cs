namespace Crypro.DTO
{
    public class CryptoGetDto
    {
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
    }
    public class CryptoCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
    }
    public class CryptoUpdateDto
    {
        public string CryptoId { get; set; } = string.Empty;    
        public double Value { get; set; }
    }
}
