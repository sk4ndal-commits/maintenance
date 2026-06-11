using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletionNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    QrCodePayload = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TechnicianId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TechnicianName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AssignedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EntityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsMandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StoragePath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UploadedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    TechnicianId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technicians", x => x.TechnicianId);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AssignedTechnicianId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedTechnicianName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletionNotes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.WorkOrderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AssignmentHistories");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ChecklistSteps");

            migrationBuilder.DropTable(
                name: "MaintenanceDocuments");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropTable(
                name: "WorkOrders");
        }
    }
}
