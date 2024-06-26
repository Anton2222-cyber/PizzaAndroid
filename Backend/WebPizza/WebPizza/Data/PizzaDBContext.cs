﻿using Microsoft.EntityFrameworkCore;
using WebPizza.Data.Entities;

namespace WebPizza.Data
{
    public class PizzaDbContext : DbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) :
            base(options)
        { }

        public DbSet<CategoryEntity> Categories { get; set; }


        public DbSet<IngredientEntity> Ingredients { get; set; }
        public DbSet<PizzaEntity> Pizzas { get; set; }
        public DbSet<PizzaIngredientEntity> PizzaIngredients { get; set; }

        public DbSet<PizzaSizeEntity> Sizes { get; set; } = null!;
        public DbSet<PizzaSizePriceEntity> PizzaSizes { get; set; } = null!;

        public DbSet<PizzaPhotoEntity> PizzaPhotos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PizzaIngredients builder
            modelBuilder.Entity<PizzaIngredientEntity>()
                .HasKey(pi => new { pi.PizzaId, pi.IngredientId });

            modelBuilder.Entity<PizzaIngredientEntity>()
                .HasOne(rc => rc.Pizza)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(rc => rc.PizzaId)
                .IsRequired();

            modelBuilder.Entity<PizzaIngredientEntity>()
                .HasOne(rc => rc.Ingredient)
                .WithMany(c => c.Pizzas)
                .HasForeignKey(rc => rc.IngredientId)
                .IsRequired();

            // PizzaSizes builder
            modelBuilder.Entity<PizzaSizePriceEntity>()
                .HasOne(ps => ps.Pizza)
                .WithMany(p => p.Sizes)
                .HasForeignKey(ps => ps.PizzaId);

            modelBuilder.Entity<PizzaSizePriceEntity>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.PizzaSizePrices)
                .HasForeignKey(ps => ps.SizeId);
        }
    }
}
