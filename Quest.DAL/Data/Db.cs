using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quest.Domain.Models;


namespace Quest.DAL.Data
{
    public class Db : IdentityDbContext<ApplicationUser>
    {
        public Db(DbContextOptions<Db> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Hint> Hints { get; set; }
        public DbSet<Moderator> Moderator { get; set; }
        public DbSet<CompletedTask> CompletedTasks { get; set; }
        public DbSet<UsedTeamHint> UsedTeamHints { get; set; }
        public DbSet<TaskAttempt> TaskAttempts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamUser>()
                .HasKey(k => new { k.TeamId, k.UserId });

            modelBuilder.Entity<TeamUser>()
                .HasOne(u => u.Team)
                .WithMany(m => m.TeamUsers)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<TeamUser>()
                .HasOne(u => u.User)
                .WithMany(m => m.TeamUsers)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
