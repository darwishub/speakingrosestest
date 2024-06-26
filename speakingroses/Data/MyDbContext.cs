using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using speakingrosestest.Models;

namespace speakingrosestest.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<_Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<_Task>(entity =>
        {
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.DueDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Priority).HasDefaultValueSql("((1))");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.TaskId).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
