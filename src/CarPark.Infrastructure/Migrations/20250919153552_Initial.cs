using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSpace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    IsOccupied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Registration = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Charge = table.Column<double>(type: "double precision", nullable: true),
                    ParkingSpaceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingSession_ParkingSpace_ParkingSpaceId",
                        column: x => x.ParkingSpaceId,
                        principalTable: "ParkingSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingSession_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ParkingSpace",
                columns: new[] { "Id", "IsOccupied", "Number" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), true, 1 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), true, 2 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), true, 3 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), true, 4 },
                    { new Guid("00000000-0000-0000-0000-000000000005"), true, 5 },
                    { new Guid("00000000-0000-0000-0000-000000000006"), true, 6 },
                    { new Guid("00000000-0000-0000-0000-000000000007"), true, 7 },
                    { new Guid("00000000-0000-0000-0000-000000000008"), true, 8 },
                    { new Guid("00000000-0000-0000-0000-000000000009"), true, 9 },
                    { new Guid("00000000-0000-0000-0000-000000000010"), true, 10 },
                    { new Guid("00000000-0000-0000-0000-000000000011"), true, 11 },
                    { new Guid("00000000-0000-0000-0000-000000000012"), true, 12 },
                    { new Guid("00000000-0000-0000-0000-000000000013"), true, 13 },
                    { new Guid("00000000-0000-0000-0000-000000000014"), true, 14 },
                    { new Guid("00000000-0000-0000-0000-000000000015"), true, 15 },
                    { new Guid("00000000-0000-0000-0000-000000000016"), true, 16 },
                    { new Guid("00000000-0000-0000-0000-000000000017"), true, 17 },
                    { new Guid("00000000-0000-0000-0000-000000000018"), true, 18 },
                    { new Guid("00000000-0000-0000-0000-000000000019"), true, 19 },
                    { new Guid("00000000-0000-0000-0000-000000000020"), true, 20 },
                    { new Guid("00000000-0000-0000-0000-000000000021"), true, 21 },
                    { new Guid("00000000-0000-0000-0000-000000000022"), true, 22 },
                    { new Guid("00000000-0000-0000-0000-000000000023"), true, 23 },
                    { new Guid("00000000-0000-0000-0000-000000000024"), true, 24 },
                    { new Guid("00000000-0000-0000-0000-000000000025"), true, 25 },
                    { new Guid("00000000-0000-0000-0000-000000000026"), false, 26 },
                    { new Guid("00000000-0000-0000-0000-000000000027"), false, 27 },
                    { new Guid("00000000-0000-0000-0000-000000000028"), false, 28 },
                    { new Guid("00000000-0000-0000-0000-000000000029"), false, 29 },
                    { new Guid("00000000-0000-0000-0000-000000000030"), false, 30 },
                    { new Guid("00000000-0000-0000-0000-000000000031"), false, 31 },
                    { new Guid("00000000-0000-0000-0000-000000000032"), false, 32 },
                    { new Guid("00000000-0000-0000-0000-000000000033"), false, 33 },
                    { new Guid("00000000-0000-0000-0000-000000000034"), false, 34 },
                    { new Guid("00000000-0000-0000-0000-000000000035"), false, 35 },
                    { new Guid("00000000-0000-0000-0000-000000000036"), false, 36 },
                    { new Guid("00000000-0000-0000-0000-000000000037"), false, 37 },
                    { new Guid("00000000-0000-0000-0000-000000000038"), false, 38 },
                    { new Guid("00000000-0000-0000-0000-000000000039"), false, 39 },
                    { new Guid("00000000-0000-0000-0000-000000000040"), false, 40 },
                    { new Guid("00000000-0000-0000-0000-000000000041"), false, 41 },
                    { new Guid("00000000-0000-0000-0000-000000000042"), false, 42 },
                    { new Guid("00000000-0000-0000-0000-000000000043"), false, 43 },
                    { new Guid("00000000-0000-0000-0000-000000000044"), false, 44 },
                    { new Guid("00000000-0000-0000-0000-000000000045"), false, 45 },
                    { new Guid("00000000-0000-0000-0000-000000000046"), false, 46 },
                    { new Guid("00000000-0000-0000-0000-000000000047"), false, 47 },
                    { new Guid("00000000-0000-0000-0000-000000000048"), false, 48 },
                    { new Guid("00000000-0000-0000-0000-000000000049"), false, 49 },
                    { new Guid("00000000-0000-0000-0000-000000000050"), false, 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_ParkingSpaceId",
                table: "ParkingSession",
                column: "ParkingSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_VehicleId",
                table: "ParkingSession",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSession");

            migrationBuilder.DropTable(
                name: "ParkingSpace");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
