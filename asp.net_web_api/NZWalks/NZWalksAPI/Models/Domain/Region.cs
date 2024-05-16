namespace NZWalksAPI.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        /*
         A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required. 
        Such an identifier has a very low probability of being duplicated.
         */
        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }

        // RegionImageUrl is accept null value
        // Name Code Id is not accept null value
    }
}
