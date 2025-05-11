using System;
using System.Collections.Generic;
using Domain.AppProgrammingInt.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppProgrammingInt.DataBase.Context;

public partial class contextAppProgrammingInt : DbContext
{
    public contextAppProgrammingInt(DbContextOptions<contextAppProgrammingInt> options)
        : base(options)
    {
    }

    public virtual DbSet<ApCliente> ApClientes { get; set; }

    public virtual DbSet<ApCuenta> ApCuenta { get; set; }

    public virtual DbSet<ApMovimiento> ApMovimientos { get; set; }

    public virtual DbSet<ApPersona> ApPersonas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApCliente>(entity =>
        {
            entity.HasKey(e => e.ClIdCliente);

            entity.ToTable("ap_cliente");

            entity.HasIndex(e => e.ClIdPersona, "IX_ap_id_persona_unique").IsUnique();

            entity.Property(e => e.ClIdCliente).HasColumnName("cl_id_cliente");
            entity.Property(e => e.ClContraseña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cl_contraseña");
            entity.Property(e => e.ClEstado)
                .HasDefaultValue(true)
                .HasColumnName("cl_estado");
            entity.Property(e => e.ClIdPersona).HasColumnName("cl_id_persona");

            entity.HasOne(d => d.ClIdPersonaNavigation).WithOne(p => p.ApCliente)
                .HasForeignKey<ApCliente>(d => d.ClIdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ap_cliente_ap_persona");
        });

        modelBuilder.Entity<ApCuenta>(entity =>
        {
            entity.HasKey(e => e.CuIdCuenta);

            entity.ToTable("ap_cuenta");

            entity.Property(e => e.CuIdCuenta).HasColumnName("cu_id_cuenta");
            entity.Property(e => e.CuEstado)
                .HasDefaultValue(true)
                .HasColumnName("cu_estado");
            entity.Property(e => e.CuIdCliente).HasColumnName("cu_id_cliente");
            entity.Property(e => e.CuNumeroCuenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cu_numero_cuenta");
            entity.Property(e => e.CuSaldoInicial).HasColumnName("cu_saldo_inicial");
            entity.Property(e => e.CuTipoCuenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cu_tipo_cuenta");

            entity.HasOne(d => d.CuIdClienteNavigation).WithMany(p => p.ApCuenta)
                .HasForeignKey(d => d.CuIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ap_cuenta_ap_cliente");
        });

        modelBuilder.Entity<ApMovimiento>(entity =>
        {
            entity.HasKey(e => e.MoIdMovimientos);

            entity.ToTable("ap_movimientos");

            entity.Property(e => e.MoIdMovimientos).HasColumnName("mo_id_movimientos");
            entity.Property(e => e.MoFecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("mo_fecha");
            entity.Property(e => e.MoIdCuenta).HasColumnName("mo_id_cuenta");
            entity.Property(e => e.MoSaldo).HasColumnName("mo_saldo");
            entity.Property(e => e.MoTipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mo_tipo_movimiento");
            entity.Property(e => e.MoValor).HasColumnName("mo_valor");

            entity.HasOne(d => d.MoIdCuentaNavigation).WithMany(p => p.ApMovimientos)
                .HasForeignKey(d => d.MoIdCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ap_movimientos_ap_cuenta");
        });

        modelBuilder.Entity<ApPersona>(entity =>
        {
            entity.HasKey(e => e.PsIdPersona);

            entity.ToTable("ap_persona");

            entity.Property(e => e.PsIdPersona).HasColumnName("ps_id_persona");
            entity.Property(e => e.PsDireccion)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ps_direccion");
            entity.Property(e => e.PsEdad).HasColumnName("ps_edad");
            entity.Property(e => e.PsGenero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ps_genero");
            entity.Property(e => e.PsIdentificacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ps_identificacion");
            entity.Property(e => e.PsNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ps_nombre");
            entity.Property(e => e.PsTelefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ps_telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
