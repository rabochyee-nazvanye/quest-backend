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
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Hint> Hints { get; set; }
        public DbSet<AppUserQuest> AppUserQuests { get; set; }
        public DbSet<TaskAttemptTeam> TaskAttemptTeams { get; set; }
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
            
            
            modelBuilder.Entity<AppUserQuest>()
                .HasKey(k => new { k.QuestId, k.UserId });

            modelBuilder.Entity<AppUserQuest>()
                .HasOne(u => u.User)
                .WithMany(m => m.AppUserQuests)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<AppUserQuest>()
                .HasOne(u => u.Quest)
                .WithMany(m => m.AppUserQuests)
                .HasForeignKey(u => u.QuestId)
                .IsRequired();
            
            
            modelBuilder.Entity<TaskAttemptTeam>()
                .HasKey(k => new { k.TeamId, k.TaskId });

            modelBuilder.Entity<TaskAttemptTeam>()
                .HasOne(u => u.Team)
                .WithMany(m => m.TaskAttemptTeams)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<TaskAttemptTeam>()
                .HasOne(u => u.Task)
                .WithMany(m => m.TaskAttemptTeams)
                .HasForeignKey(u => u.TaskId)
                .IsRequired();


            modelBuilder.Entity<UsedTeamHint>()
                .HasKey(k => new { k.TeamId, k.HintId });

            modelBuilder.Entity<UsedTeamHint>()
                .HasOne(u => u.Team)
                .WithMany(m => m.UsedTeamHints)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<UsedTeamHint>()
                .HasOne(u => u.Hint)
                .WithMany(m => m.UsedTeamHints)
                .HasForeignKey(u => u.HintId)
                .IsRequired();
            
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
