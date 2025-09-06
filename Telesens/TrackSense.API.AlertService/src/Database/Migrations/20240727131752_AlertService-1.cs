using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimeScaleDB.Migrations
{
    /// <inheritdoc />
    public partial class AlertService1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alert_rules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    critical_value = table.Column<float>(type: "real", nullable: false),
                    @operator = table.Column<int>(name: "operator", type: "integer", nullable: false),
                    value_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    imei = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alert_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gps = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    alert_rule_id = table.Column<int>(type: "integer", nullable: false),
                    imei = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_alert_events_alert_rules_alert_rule_id",
                        column: x => x.alert_rule_id,
                        principalTable: "alert_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "alert_rules_notification_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alert_rule_id = table.Column<int>(type: "integer", nullable: false),
                    notification_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_rules_notification_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_alert_rules_notification_groups_alert_rules_alert_rule_id",
                        column: x => x.alert_rule_id,
                        principalTable: "alert_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_alert_rules_notification_groups_notification_groups_notific~",
                        column: x => x.notification_group_id,
                        principalTable: "notification_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "emails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email_value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notification_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emails", x => x.id);
                    table.ForeignKey(
                        name: "FK_emails_notification_groups_notification_group_id",
                        column: x => x.notification_group_id,
                        principalTable: "notification_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "telegram_chats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    telegram_chat_id = table.Column<string>(type: "text", nullable: false),
                    notification_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_chats", x => x.id);
                    table.ForeignKey(
                        name: "FK_telegram_chats_notification_groups_notification_group_id",
                        column: x => x.notification_group_id,
                        principalTable: "notification_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alert_events_alert_rule_id_imei",
                table: "alert_events",
                columns: new[] { "alert_rule_id", "imei" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alert_rules_notification_groups_alert_rule_id",
                table: "alert_rules_notification_groups",
                column: "alert_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_rules_notification_groups_notification_group_id",
                table: "alert_rules_notification_groups",
                column: "notification_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_notification_group_id",
                table: "emails",
                column: "notification_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_telegram_chats_notification_group_id",
                table: "telegram_chats",
                column: "notification_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_events");

            migrationBuilder.DropTable(
                name: "alert_rules_notification_groups");

            migrationBuilder.DropTable(
                name: "emails");

            migrationBuilder.DropTable(
                name: "telegram_chats");

            migrationBuilder.DropTable(
                name: "alert_rules");

            migrationBuilder.DropTable(
                name: "notification_groups");
        }
    }
}
