using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1ad5baba-b6e0-4bba-8c26-1b87ebb681dd"), "Easy" },
                    { new Guid("3830149f-ec80-49fd-a483-9a7b4ac9dad8"), "Medium" },
                    { new Guid("efafab3a-cf95-4d84-b92d-4fea45c16825"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("62e43591-0a93-41fc-8d64-6f2e36d52749"), "AKL", "AuckLand", "https://media.timeout.com/images/105906099/1024/768/image.webp" },
                    { new Guid("9b42912c-52a0-4db3-94a4-ad3e29a9f339"), "HML", "Hamilton", "https://as2.ftcdn.net/v2/jpg/04/08/89/99/1000_F_408899914_F5UU2RTDtM3ZlxSKqc1PNLOJ87v2dr8i.jpg" },
                    { new Guid("9b42912c-52a0-4db3-94a4-ad3e29a9f398"), "WEL", "Wellington", "https://www.nzherald.co.nz/resizer/v2/U3PYFRGZPBGDVPPDW2BV7WTZKM.JPG?auth=9b4a0d1de59f22fbfd9e75df13c9d639fc8ae68c391bdc63857e7518b33b659a&width=1440&height=810&quality=70&focal=922%2C451&smart=false" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "difficulties",
                keyColumn: "Id",
                keyValue: new Guid("1ad5baba-b6e0-4bba-8c26-1b87ebb681dd"));

            migrationBuilder.DeleteData(
                table: "difficulties",
                keyColumn: "Id",
                keyValue: new Guid("3830149f-ec80-49fd-a483-9a7b4ac9dad8"));

            migrationBuilder.DeleteData(
                table: "difficulties",
                keyColumn: "Id",
                keyValue: new Guid("efafab3a-cf95-4d84-b92d-4fea45c16825"));

            migrationBuilder.DeleteData(
                table: "regions",
                keyColumn: "Id",
                keyValue: new Guid("62e43591-0a93-41fc-8d64-6f2e36d52749"));

            migrationBuilder.DeleteData(
                table: "regions",
                keyColumn: "Id",
                keyValue: new Guid("9b42912c-52a0-4db3-94a4-ad3e29a9f339"));

            migrationBuilder.DeleteData(
                table: "regions",
                keyColumn: "Id",
                keyValue: new Guid("9b42912c-52a0-4db3-94a4-ad3e29a9f398"));
        }
    }
}
