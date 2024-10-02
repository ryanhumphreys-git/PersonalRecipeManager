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
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(50);
        });

        modelBuilder.Entity<ToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ToolsAnd__3214EC07EE62D0B3");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<KitchenIngredients>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__KitchenI__6B2329058B1AE66A");

            entity.ToTable("KitchenIngredients");

            entity.Property(e => e.AutoId).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.KitchenIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("FK_KitchenIngredients_Ingredients");

            entity.HasOne(d => d.Kitchen).WithMany(p => p.KitchenIngredients)
                .HasForeignKey(d => d.KitchenId)
                .HasConstraintName("FK_KitchenIngredients_KitchenType");
        });

        modelBuilder.Entity<KitchenToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__KitchenT__6B232905AA4950B5");

            entity.ToTable("KitchenToolsAndEquipment");

            entity.Property(e => e.AutoId).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Kitchen).WithMany(p => p.KitchenToolsAndEquipments)
                .HasForeignKey(d => d.KitchenId)
                .HasConstraintName("FK_KitchenToolsAndEquipment_KitchenType");

            entity.HasOne(d => d.ToolsAndEquipment).WithMany(p => p.KitchenToolsAndEquipments)
                .HasForeignKey(d => d.ToolsAndEquipmentId)
                .HasConstraintName("FK_KitchenToolsAndEquipment_ToolsAndEquipment");
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
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(50);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("FK_RecipeIngredients_Ingredients");

            entity.HasOne(d => d.Recipes).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK_RecipeIngredients_Recipes");
        });

        modelBuilder.Entity<RecipeToolsAndEquipment>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__RecipeTo__6B232905A760B805");

            entity.Property(e => e.AutoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeToolsAndEquipments)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK_RecipeToolsAndEquipment_Recipes");

            entity.HasOne(d => d.ToolsAndEquipment).WithMany(p => p.RecipeToolsAndEquipments)
                .HasForeignKey(d => d.ToolsAndEquipmentId)
                .HasConstraintName("FK_RecipeToolsAndEquipment_ToolsAndEquipment");
        });

        modelBuilder.Entity<Recipes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipes__3214EC07AB5C1B61");

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