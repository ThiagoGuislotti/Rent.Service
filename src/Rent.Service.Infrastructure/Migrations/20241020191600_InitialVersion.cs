using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_persons",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cnpj = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp", nullable: false),
                    driver_license_number = table.Column<string>(type: "text", nullable: false),
                    driver_license_category = table.Column<byte>(type: "smallint", nullable: false),
                    with_driver_license_image = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_persons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycle_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    motorcycle_id = table.Column<string>(type: "text", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycle_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    license_plate = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    delivery_person_id = table.Column<string>(type: "text", nullable: false),
                    motorcycle_id = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    expected_end_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    plan = table.Column<byte>(type: "smallint", nullable: false),
                    daily_value = table.Column<int>(type: "integer", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_delivery_persons_delivery_person_id",
                        column: x => x.delivery_person_id,
                        principalTable: "delivery_persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentals_motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_delivery_persons_cnpj",
                table: "delivery_persons",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_persons_driver_license_number",
                table: "delivery_persons",
                column: "driver_license_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_motorcycles_license_plate",
                table: "motorcycles",
                column: "license_plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rentals_delivery_person_id",
                table: "rentals",
                column: "delivery_person_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_motorcycle_id",
                table: "rentals",
                column: "motorcycle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motorcycle_events");

            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "delivery_persons");

            migrationBuilder.DropTable(
                name: "motorcycles");
        }
    }
}
