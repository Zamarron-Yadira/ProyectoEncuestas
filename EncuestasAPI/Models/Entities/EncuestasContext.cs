using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EncuestasAPI.Models.Entities;

public partial class EncuestasContext : DbContext
{
    public EncuestasContext()
    {
    }

    public EncuestasContext(DbContextOptions<EncuestasContext> options)
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
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Detallerespuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("detallerespuestas");

            entity.HasIndex(e => e.IdPregunta, "fkPreguntaId_idx");

            entity.HasIndex(e => e.IdRespuesta, "fkRespuestaId_idx");

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.Detallerespuestas)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkPreguntaId");

            entity.HasOne(d => d.IdRespuestaNavigation).WithMany(p => p.Detallerespuestas)
                .HasForeignKey(d => d.IdRespuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkRespuestaId");
        });

        modelBuilder.Entity<Encuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("encuestas");

            entity.HasIndex(e => e.IdUsuario, "fkUsuarioId_idx");

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Encuestas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkUsuarioId");
        });

        modelBuilder.Entity<Preguntas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("preguntas");

            entity.HasIndex(e => e.IdEncuesta, "fkEncuestaId_idx");

            entity.Property(e => e.Descripcion).HasColumnType("text");

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.Preguntas)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkEncuestaId");
        });

        modelBuilder.Entity<Respuestas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("respuestas");

            entity.HasIndex(e => e.IdEncuesta, "fkIdEncuesta_idx");

            entity.HasIndex(e => e.IdUsuarioAplicador, "fkUsuarioId_idx");

            entity.Property(e => e.FechaAplicacion).HasColumnType("datetime");
            entity.Property(e => e.NombreAlumno).HasMaxLength(150);
            entity.Property(e => e.NumControlAlumno).HasMaxLength(10);

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.Respuestas)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkEncuest");

            entity.HasOne(d => d.IdUsuarioAplicadorNavigation).WithMany(p => p.Respuestas)
                .HasForeignKey(d => d.IdUsuarioAplicador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkUsu");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.Contrasena).HasMaxLength(10);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
