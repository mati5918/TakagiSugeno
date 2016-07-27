using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TakagiSugeno.Model;

namespace TakagiSugeno.Migrations
{
    [DbContext(typeof(TakagiSugenoDbContext))]
    partial class TakagiSugenoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSInputOutput", b =>
                {
                    b.Property<int>("TSInputOutputId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Name");

                    b.Property<int>("TSSystemId");

                    b.Property<int>("Type");

                    b.HasKey("TSInputOutputId");

                    b.HasIndex("TSSystemId");

                    b.ToTable("InputsOutputs");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSRule", b =>
                {
                    b.Property<int>("TSRuleId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TSSystemId");

                    b.HasKey("TSRuleId");

                    b.HasIndex("TSSystemId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSRuleElement", b =>
                {
                    b.Property<int>("TSRuleElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TSInputOutputId");

                    b.Property<int>("TSRuleId");

                    b.Property<int>("TSVariableId");

                    b.Property<int>("Type");

                    b.HasKey("TSRuleElementId");

                    b.HasIndex("TSInputOutputId");

                    b.HasIndex("TSRuleId");

                    b.HasIndex("TSVariableId");

                    b.ToTable("RuleElements");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSSystem", b =>
                {
                    b.Property<int>("TSSystemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AndMethod");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name");

                    b.HasKey("TSSystemId");

                    b.ToTable("Systems");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSVariable", b =>
                {
                    b.Property<int>("TSVariableId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("Name");

                    b.Property<int>("TSInputOutputId");

                    b.Property<int>("Type");

                    b.HasKey("TSVariableId");

                    b.HasIndex("TSInputOutputId");

                    b.ToTable("Variables");
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSInputOutput", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSSystem", "System")
                        .WithMany("InputsOutputs")
                        .HasForeignKey("TSSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSRule", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSSystem", "System")
                        .WithMany("Rules")
                        .HasForeignKey("TSSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSRuleElement", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSInputOutput", "InputOutput")
                        .WithMany()
                        .HasForeignKey("TSInputOutputId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TakagiSugeno.Model.Entity.TSRule", "Rule")
                        .WithMany("RuleElements")
                        .HasForeignKey("TSRuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TakagiSugeno.Model.Entity.TSVariable", "Variable")
                        .WithMany()
                        .HasForeignKey("TSVariableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TakagiSugeno.Model.Entity.TSVariable", b =>
                {
                    b.HasOne("TakagiSugeno.Model.Entity.TSInputOutput", "InputOutput")
                        .WithMany("Variables")
                        .HasForeignKey("TSInputOutputId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
