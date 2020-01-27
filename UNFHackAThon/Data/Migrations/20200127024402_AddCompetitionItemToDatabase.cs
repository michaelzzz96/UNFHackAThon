using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UNFHackAThon.Data.Migrations
{
    public partial class AddCompetitionItemToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CompetitionDate = table.Column<DateTime>(nullable: false),
                    CompetitionTime = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CompetitionId = table.Column<int>(nullable: false),
                    SubCompetitionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionItem_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompetitionItem_SubCompetition_SubCompetitionId",
                        column: x => x.SubCompetitionId,
                        principalTable: "SubCompetition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionItem_CompetitionId",
                table: "CompetitionItem",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionItem_SubCompetitionId",
                table: "CompetitionItem",
                column: "SubCompetitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionItem");
        }
    }
}
