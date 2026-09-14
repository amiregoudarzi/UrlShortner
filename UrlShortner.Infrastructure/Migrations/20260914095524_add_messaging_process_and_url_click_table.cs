using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_messaging_process_and_url_click_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "message_processings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    message_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    queue_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    routing_key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attempt_count = table.Column<int>(type: "int", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    processed_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_date_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_processings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "url_clicks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    short_url_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_agent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    referrer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_url_clicks", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_processings");

            migrationBuilder.DropTable(
                name: "url_clicks");
        }
    }
}
