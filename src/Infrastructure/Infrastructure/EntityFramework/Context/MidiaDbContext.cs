﻿using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infrastructure.EntityFramework.Context;
public class MidiaDbContext : DbContext, IMidiaDbContext
{
    public IDbConnection Connection => base.Database.GetDbConnection();
    private readonly IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public DbSet<Midia> Midias { get; set; }

    public MidiaDbContext(DbContextOptions<MidiaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //These configurations are useful while debuging the code. DON'T USE IN PRODUCTION
        optionsBuilder
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging();
    }

    public async Task<bool> CommitAsync()
    {
        return await base.SaveChangesAsync() > 0;
    }
}