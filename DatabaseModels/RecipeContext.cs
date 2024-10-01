using Microsoft.EntityFrameworkCore;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.DatabaseModels;

public partial class RecipeContext : DbContext
{
    public DbSet<Ingredients> Ingredients { get; set; }
    public DbSet<ToolsAndEquipment> ToolsAndEquipment { get; set; }
    public DbSet<KitchenIngredients> KitchenIngredients { get; set; }
    public DbSet<KitchenToolsAndEquipment> KitchenToolsAndEquipment { get; set; }
    public DbSet<KitchenType> KitchenType { get; set; }
    public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
    public DbSet<RecipeToolsAndEquipment> RecipeToolsAndEquipment { get; set; }
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

        modelBuilder.Entity<ToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC07806D2A8F");

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

        modelBuilder.Entity<KitchenIngredients>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__KitchenI__6B2329058B1AE66A");

            entity.ToTable("KitchenIngredients");

            entity.Property(e => e.AutoId).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<KitchenToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__KitchenT__6B232905AA4950B5");

            entity.ToTable("KitchenToolsAndEquipment");

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

        modelBuilder.Entity<RecipeIngredients>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__RecipeIn__6B232905C9FA5D74");

            entity.Property(e => e.AutoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<RecipeToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__RecipeTo__6B232905A760B805");

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