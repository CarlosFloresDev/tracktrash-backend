using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CicloRecojoYCapacidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelacion",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReservadoKg",
                schema: "tracktrash",
                table: "asociaciones",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "tracktrash",
                table: "asociaciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_solicitudes_recojo_AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                column: "AsociacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudes_recojo_asociaciones_AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                column: "AsociacionId",
                principalSchema: "tracktrash",
                principalTable: "asociaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solicitudes_recojo_asociaciones_AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropIndex(
                name: "IX_solicitudes_recojo_AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "Latitud",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "Longitud",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "MotivoCancelacion",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "ReservadoKg",
                schema: "tracktrash",
                table: "asociaciones");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "tracktrash",
                table: "asociaciones");
        }
    }
}
