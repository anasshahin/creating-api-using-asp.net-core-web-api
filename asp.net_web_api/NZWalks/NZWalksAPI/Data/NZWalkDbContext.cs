
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalkDbContext : DbContext
    {
        /*
         We want to pass the DbContextOptions because we later on want to send our own connections through the Program.cs 
         */
        /*
         We will see the usage of this constructor later on when we create a new connection string and then inject
         the connection through the Program.cs file.
        */
        public NZWalkDbContext(DbContextOptions<NZWalkDbContext> dbContextOptions) : base(dbContextOptions)

        {

        }

        /*
         A DB set is a property of DbContext class that represents a collection of entities in the database.
        */
        public DbSet<Difficulty> Difficulies { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }
        // these three properties will create tables in database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /// seed data for difficulties
            /// Esay, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
            new Difficulty(){
            Id = Guid.Parse("2e8b8a9c-ee40-41eb-bb15-85b1fa374171"),
            Name="Esay",
            },
             new Difficulty(){
            Id = Guid.Parse("42c672bd-c31c-42d4-a998-0faea61319d7"),
            Name="Medium",
            },
              new Difficulty(){
            Id =Guid.Parse("4b1d61d0-ce3c-4494-a78b-f76498ca81fc") ,
            Name="Hard",
            },
            };
            // seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);

        }
    }

}