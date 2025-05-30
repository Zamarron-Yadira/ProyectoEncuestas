using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EncuestasAPI.Models.Entities;

public partial class WebsitosEncuestasContext : DbContext
{
    public WebsitosEncuestasContext()
    {
    }

    public WebsitosEncuestasContext(DbContextOptions<WebsitosEncuestasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Detallerespuestas> Detallerespuestas { get; set; }

    public virtual DbSet<Encuestas> Encuestas { get; set; }

    public virtual DbSet<Preguntas> Preguntas { get; set; }

    public virtual DbSet<Respuestas> Respuestas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Detallerespuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("detallerespuestas");

            entity.HasIndex(e => e.IdPregunta, "fkIdPregunta");

            entity.HasIndex(e => e.IdRespuesta, "fkIdRespuesta");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdPregunta).HasColumnType("int(11)");
            entity.Property(e => e.IdRespuesta).HasColumnType("int(11)");
            entity.Property(e => e.ValorEvaluacion).HasColumnType("int(11)");

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.Detallerespuestas)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkIdPregunta");

            entity.HasOne(d => d.IdRespuestaNavigation).WithMany(p => p.Detallerespuestas)
                .HasForeignKey(d => d.IdRespuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkIdRespuesta");
        });

        modelBuilder.Entity<Encuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("encuestas");

            entity.HasIndex(e => e.IdUsuario, "fkIdusuario");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Encuestas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkIdusuario");
        });

        modelBuilder.Entity<Preguntas>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PRIMARY");

            entity.ToTable("preguntas");

            entity.HasIndex(e => e.IdEncuesta, "fkIdencuesta");

            entity.Property(e => e.IdPregunta).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.IdEncuesta).HasColumnType("int(11)");
            entity.Property(e => e.NumeroPregunta).HasColumnType("int(11)");

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.Preguntas)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkIdencuesta");
        });

        modelBuilder.Entity<Respuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("respuestas");

            entity.HasIndex(e => e.IdEncuesta, "fkEncuestaId");

            entity.HasIndex(e => e.IdUsuarioAplicador, "fkUsuarioAplicador");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FechaAplicacion).HasColumnType("datetime");
            entity.Property(e => e.IdEncuesta).HasColumnType("int(11)");
            entity.Property(e => e.IdUsuarioAplicador).HasColumnType("int(11)");
            entity.Property(e => e.NombreAlumno).HasMaxLength(150);
            entity.Property(e => e.NumControlAlumno).HasMaxLength(10);

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.Respuestas)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkEncuestaId");

            entity.HasOne(d => d.IdUsuarioAplicadorNavigation).WithMany(p => p.Respuestas)
                .HasForeignKey(d => d.IdUsuarioAplicador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkUsuarioAplicador");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contrasena).HasMaxLength(10);
            entity.Property(e => e.EsAdmin).HasColumnType("int(11)");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
