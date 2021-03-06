﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TaskManagementWeb.Models;

namespace TaskManagementWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171113090529_Migration2")]
    partial class Migration2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TaskManagementWeb.Models.Task", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActualExecutionTimeHours");

                    b.Property<DateTime?>("CompleteTimeStamp");

                    b.Property<DateTime>("CreationTimeStamp");

                    b.Property<string>("Description");

                    b.Property<string>("Implementer");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.Property<int>("PlannedExecutionTimeHours");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("ParentId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskManagementWeb.Models.Task", b =>
                {
                    b.HasOne("TaskManagementWeb.Models.Task", "Parent")
                        .WithMany("Subtasks")
                        .HasForeignKey("ParentId");
                });
#pragma warning restore 612, 618
        }
    }
}
