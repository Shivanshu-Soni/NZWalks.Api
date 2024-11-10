using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Models;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Difficulty> difficulties { get; set; }
        public DbSet<Region> regions { get; set; }
        public DbSet<Walk> walks { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //seed data for difficulties
            var difficulties = new List<Difficulty>(){
                new Difficulty() {
                    Id = Guid.Parse("1ad5baba-b6e0-4bba-8c26-1b87ebb681dd"),
                    Name = "Easy"
                    },
                    new Difficulty() {
                        Id= Guid.Parse("efafab3a-cf95-4d84-b92d-4fea45c16825") ,
                        Name = "Hard"
                    },
                    new Difficulty(){
                        Id= Guid.Parse("3830149f-ec80-49fd-a483-9a7b4ac9dad8"),
                        Name= "Medium"
                    }
            };

            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            // seed data for Regions

            var regions = new List<Region>(){
                new Region() {
                    Id= Guid.Parse("62e43591-0a93-41fc-8d64-6f2e36d52749"),
                    Name= "AuckLand",
                    Code= "AKL",
                    RegionImageUrl = "https://media.timeout.com/images/105906099/1024/768/image.webp"
                },
                 new Region() {
                    Id= Guid.Parse("9b42912c-52a0-4db3-94a4-ad3e29a9f398"),
                    Name= "Wellington",
                    Code= "WEL",
                    RegionImageUrl = "https://www.nzherald.co.nz/resizer/v2/U3PYFRGZPBGDVPPDW2BV7WTZKM.JPG?auth=9b4a0d1de59f22fbfd9e75df13c9d639fc8ae68c391bdc63857e7518b33b659a&width=1440&height=810&quality=70&focal=922%2C451&smart=false"
                },
                 new Region() {
                    Id= Guid.Parse("9b42912c-52a0-4db3-94a4-ad3e29a9f339"),
                    Name= "Hamilton",
                    Code="HML",
                    RegionImageUrl= "https://as2.ftcdn.net/v2/jpg/04/08/89/99/1000_F_408899914_F5UU2RTDtM3ZlxSKqc1PNLOJ87v2dr8i.jpg"
                }


            };

            modelBuilder.Entity<Region>().HasData(regions);

        }

    }
}