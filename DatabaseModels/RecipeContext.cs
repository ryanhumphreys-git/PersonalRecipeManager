using Microsoft.EntityFrameworkCore;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.DatabaseModels;

public partial class RecipeContext : DbContext
{
    public DbSet<Ingredients> Ingredients { get; set; }
    public DbSet<Tools> Tools { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<KitchenItems> KitchenItems { get; set; }
    public DbSet<KitchenType> KitchenType { get; set; }
    public DbSet<RecipeItems> RecipeItems { get; set; }
    public DbSet<Recipes> Recipes { get; set; }
    public DbSet<Entity> Entity { get; set; }

    // The following configures EF to create a SqlServer database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(@"Server=SPARK-DEV-RTH;Database=RecipeManager;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entity__3214EC073ABC51A1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ingredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC0772200186");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(50);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC07806D2A8F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Tools>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tools__3214EC07F2EB3AC9");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemType__3214EC073D328CE6");

            entity.ToTable("ItemType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<KitchenItems>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__KitchenI__6B2329058C7E0B10");

            entity.ToTable("Kitchen");

            entity.Property(e => e.AutoId).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<KitchenType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KitchenT__3214EC07692EF4AD");

            entity.ToTable("KitchenType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RecipeItems>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__RecipeIt__6B232905D307F9E5");

            entity.Property(e => e.AutoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Recipes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC0770AFB77E");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Time).HasColumnType("decimal(18, 0)");
        });
        
        modelBuilder.Entity<RecipeIngredientsDTO>().HasNoKey().ToView(null);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}