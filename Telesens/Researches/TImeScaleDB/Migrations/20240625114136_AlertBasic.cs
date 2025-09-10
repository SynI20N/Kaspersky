using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TImeScaleDB.Migrations
{
    /// <inheritdoc />
    public partial class AlertBasic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CriticalValue = table.Column<float>(type: "real", nullable: false),
                    Operator = table.Column<int>(type: "integer", nullable: false),
                    ValueName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.UniqueID);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    UTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GPS = table.Column<string>(type: "text", nullable: true),
                    sensor1 = table.Column<float>(type: "real", nullable: false),
                    sensor2 = table.Column<float>(type: "real", nullable: false),
                    sensor3 = table.Column<float>(type: "real", nullable: false),
                    g_sensor = table.Column<float>(type: "real", nullable: false),
                    volt = table.Column<float>(type: "real", nullable: false),
                    temp = table.Column<int>(type: "integer", nullable: false),
                    hum = table.Column<byte>(type: "smallint", nullable: false),
                    barometer = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.UTC);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GPS = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AlertRuleId = table.Column<int>(type: "integer", nullable: false),
                    IMEI = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.UniqueID);
                    table.ForeignKey(
                        name: "FK_Events_Rules_AlertRuleId",
                        column: x => x.AlertRuleId,
                        principalTable: "Rules",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AlertRuleId",
                table: "Events",
                column: "AlertRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Rules");
        }
    }
}
