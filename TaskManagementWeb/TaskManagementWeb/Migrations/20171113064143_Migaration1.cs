using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskManagementWeb.Migrations
{
    public partial class Migaration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_ParentID",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "Tasks",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ParentID",
                table: "Tasks",
                newName: "IX_Tasks_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_ParentId",
                table: "Tasks",
                column: "ParentId",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_ParentId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Tasks",
                newName: "ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ParentId",
                table: "Tasks",
                newName: "IX_Tasks_ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_ParentID",
                table: "Tasks",
                column: "ParentID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
