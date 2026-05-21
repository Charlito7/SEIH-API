using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Transcrer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "seih_transfer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false),

                IdHospitalFrom = table.Column<Guid>(type: "char(36)", nullable: false),
                IdHospitalTo = table.Column<Guid>(type: "char(36)", nullable: false),

                EncryptedPayload = table.Column<byte[]>(type: "longblob", nullable: false),
                EncryptedSessionKey = table.Column<byte[]>(type: "longblob", nullable: false),
                IV = table.Column<byte[]>(type: "varbinary(32)", nullable: false),

                Signature = table.Column<string>(type: "varchar(512)", nullable: false),
                PayloadHash = table.Column<string>(type: "varchar(128)", nullable: false),
                PayloadSize = table.Column<long>(type: "bigint", nullable: false),

                PayloadType = table.Column<string>(type: "varchar(50)", nullable: false),
                SchemaVersion = table.Column<string>(type: "varchar(50)", nullable: false),

                KeyVersion = table.Column<int>(type: "int", nullable: false),
                Nonce = table.Column<string>(type: "varchar(128)", nullable: false),

                SignedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),

                IdConsent = table.Column<Guid>(type: "char(36)", nullable: false),
                ConsentHash = table.Column<string>(type: "varchar(256)", nullable: false),
                ConsentExpiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),

                PatientReference = table.Column<string>(type: "varchar(255)", nullable: false),

                Status = table.Column<string>(type: "varchar(50)", nullable: false, defaultValue: "CREATED"),

                Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                CreatedBy = table.Column<string>(type: "varchar(100)", nullable: true),
                LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                LastModifiedBy = table.Column<string>(type: "varchar(100)", nullable: true),
                LastDeleted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                LastDeletedBy = table.Column<string>(type: "varchar(100)", nullable: true),
                IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_seih_transfer", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "seih_transfer");
    }

    
    
    }
}
