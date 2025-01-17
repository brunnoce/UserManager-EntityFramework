﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TPFinalCe.Models;

#nullable disable

namespace TPFinalCe.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TPFinalCe.Models.Beneficios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Beneficios");
                });

            modelBuilder.Entity("TPFinalCe.Models.Cuota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EmisionCuota")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaVencimiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("Monto")
                        .HasColumnType("int");

                    b.Property<bool>("Pagada")
                        .HasColumnType("bit");

                    b.Property<int>("SocioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SocioId");

                    b.ToTable("Cuotas");
                });

            modelBuilder.Entity("TPFinalCe.Models.Disciplina", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Disciplinas");
                });

            modelBuilder.Entity("TPFinalCe.Models.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Estados");
                });

            modelBuilder.Entity("TPFinalCe.Models.Socio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BeneficiosId")
                        .HasColumnType("int");

                    b.Property<int>("DNI")
                        .HasColumnType("int");

                    b.Property<int?>("DisciplinaId")
                        .HasColumnType("int");

                    b.Property<int?>("EstadoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BeneficiosId");

                    b.HasIndex("DisciplinaId");

                    b.HasIndex("EstadoId");

                    b.ToTable("Socios");
                });

            modelBuilder.Entity("WebAppEFCore.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaLogout")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("TPFinalCe.Models.Cuota", b =>
                {
                    b.HasOne("TPFinalCe.Models.Socio", "Socio")
                        .WithMany("Cuotas")
                        .HasForeignKey("SocioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Socio");
                });

            modelBuilder.Entity("TPFinalCe.Models.Socio", b =>
                {
                    b.HasOne("TPFinalCe.Models.Beneficios", "Beneficios")
                        .WithMany()
                        .HasForeignKey("BeneficiosId");

                    b.HasOne("TPFinalCe.Models.Disciplina", "Disciplina")
                        .WithMany("Socios")
                        .HasForeignKey("DisciplinaId");

                    b.HasOne("TPFinalCe.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId");

                    b.Navigation("Beneficios");

                    b.Navigation("Disciplina");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("TPFinalCe.Models.Disciplina", b =>
                {
                    b.Navigation("Socios");
                });

            modelBuilder.Entity("TPFinalCe.Models.Socio", b =>
                {
                    b.Navigation("Cuotas");
                });
#pragma warning restore 612, 618
        }
    }
}
