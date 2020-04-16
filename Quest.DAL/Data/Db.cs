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

        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Hint> Hints { get; set; }
        public DbSet<TaskAttemptTeam> TaskAttemptTeams { get; set; }
        public DbSet<TeamHint> UsedTeamHints { get; set; }
        public DbSet<TaskAttempt> TaskAttempts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamUser>()
                .HasKey(k => new { k.TeamId, k.UserId });

            modelBuilder.Entity<TeamUser>()
                .HasOne(u => u.Team)
                .WithMany(m => m.Members)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<TeamUser>()
                .HasOne(u => u.User)
                .WithMany(m => m.JoinedTeams)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
            
        modelBuilder.Entity<TaskAttemptTeam>()
                .HasKey(k => new { k.TeamId, k.TaskId });

            modelBuilder.Entity<TaskAttemptTeam>()
                .HasOne(u => u.Team)
                .WithMany(m => m.TaskAttempts)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<TaskAttemptTeam>()
                .HasOne(u => u.Task)
                .WithMany(m => m.TaskAttempts)
                .HasForeignKey(u => u.TaskId)
                .IsRequired();


            modelBuilder.Entity<TeamHint>()
                .HasKey(k => new { k.TeamId, k.HintId });

            modelBuilder.Entity<TeamHint>()
                .HasOne(u => u.Team)
                .WithMany(m => m.UsedHints)
                .HasForeignKey(u => u.TeamId)
                .IsRequired();

            modelBuilder.Entity<TeamHint>()
                .HasOne(u => u.Hint)
                .WithMany(m => m.UsedTeamHints)
                .HasForeignKey(u => u.HintId)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .HasOne(x => x.Quest)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.QuestId)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .HasOne(x => x.Captain)
                .WithMany(x => x.OwnedTeams)
                .HasForeignKey(x => x.CaptainUserId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
