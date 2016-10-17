using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TakagiSugeno.Model;

namespace TakagiSugeno.Migrations
{
    [DbContext(typeof(TakagiSugenoDbContext))]
    [Migration("20161017174955_SystemsUpdate")]
    partial class SystemsUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TakagiSugeno.Model.Entity.InputOutput", b =>
                {
                    b.Property<int>("InputOutputId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("TSSystemId");

                    b.Property<int>("Type");

                    b.HasKey("InputOutputId");

                    b.HasIndex("TSSystemId");

                    b.ToTable("InputsOutputs");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.Rule", b =>
                {
                    b.Property<int>("RuleId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TSSystemId");

                    b.HasKey("RuleId");

                    b.HasIndex("TSSystemId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.RuleElement", b =>
                {
                    b.Property<int>("RuleElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("InputOutputId");

                    b.Property<bool>("IsNegation");

                    b.Property<int>("NextOpartion");

                    b.Property<int>("RuleId");

                    b.Property<int>("Type");

                    b.Property<int?>("VariableId");

                    b.HasKey("RuleElementId");

                    b.HasIndex("InputOutputId");

                    b.HasIndex("RuleId");

                    b.HasIndex("VariableId");

                    b.ToTable("RuleElements");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSSystem", b =>
                {
                    b.Property<int>("TSSystemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AndMethod");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("Name");

                    b.Property<int>("OrMethod");

                    b.Property<DateTime>("PublishedDate");

                    b.HasKey("TSSystemId");

                    b.ToTable("Systems");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.Variable", b =>
                {
                    b.Property<int>("VariableId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<int>("InputOutputId");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("VariableId");

                    b.HasIndex("InputOutputId");

                    b.ToTable("Variables");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.InputOutput", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSSystem", "System")
                        .WithMany("InputsOutputs")
                        .HasForeignKey("TSSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.Rule", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSSystem", "System")
                        .WithMany("Rules")
                        .HasForeignKey("TSSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.RuleElement", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.InputOutput", "InputOutput")
                        .WithMany()
                        .HasForeignKey("InputOutputId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TakagiSugeno.Model.Entity.Rule", "Rule")
                        .WithMany("RuleElements")
                        .HasForeignKey("RuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TakagiSugeno.Model.Entity.Variable", "Variable")
                        .WithMany()
                        .HasForeignKey("VariableId");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.Variable", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.InputOutput", "InputOutput")
                        .WithMany("Variables")
                        .HasForeignKey("InputOutputId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
