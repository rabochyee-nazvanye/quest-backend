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
        public DbSet<SoloInfiniteQuest> SingleInfiniteQuests { get; set; }
        public DbSet<TeamScheduledQuest> TeamScheduledQuests { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        
        public DbSet<Hint> Hints { get; set; }
        public DbSet<ParticipantHint> UsedParticipantHints { get; set; }
        public DbSet<TaskAttempt> TaskAttempts { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //
            // Inheritance explicit declaration for TPH
            //
            modelBuilder.Entity<SoloPlayer>().HasBaseType<Participant>();
            modelBuilder.Entity<Team>().HasBaseType<Participant>();
            
            modelBuilder.Entity<SoloInfiniteQuest>().HasBaseType<QuestEntity>();
            modelBuilder.Entity<TeamScheduledQuest>().HasBaseType<QuestEntity>();
            
            //
            // One-To-Many, Many-To-Many relationships
            //
            
            // Task <> Participant (many-to-many)
            modelBuilder.Entity<TaskAttempt>()
                .HasKey(k => new { k.TaskId, k.ParticipantId });
            
            modelBuilder.Entity<TaskAttempt>()
                .HasOne(t => t.Participant)
                .WithMany(p => p.TaskAttempts)
                .HasForeignKey(t => t.ParticipantId)
                .IsRequired();
            
            modelBuilder.Entity<TaskAttempt>()
                .HasOne(t => t.TaskEntity)
                .WithMany(p => p.TaskAttempts)
                .HasForeignKey(t => t.TaskId)
                .IsRequired();
            
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
            
            modelBuilder.Entity<ParticipantHint>()
                .HasKey(k => new { k.ParticipantId, k.HintId });

            modelBuilder.Entity<ParticipantHint>()
                .HasOne(u => u.Participant)
                .WithMany(m => m.UsedHints)
                .HasForeignKey(u => u.ParticipantId)
                .IsRequired();

            modelBuilder.Entity<ParticipantHint>()
                .HasOne(u => u.Hint)
                .WithMany(m => m.UsedTeamHints)
                .HasForeignKey(u => u.HintId)
                .IsRequired();

            modelBuilder.Entity<Participant>()
                .HasOne(x => x.Quest)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.QuestId)
                .IsRequired();

            modelBuilder.Entity<Participant>()
                .HasOne(x => x.Principal)
                .WithMany(x => x.OwnedParticipants)
                .HasForeignKey(x => x.PrincipalUserId)
                .IsRequired();
            
            modelBuilder.Entity<Participant>()
                .HasOne(x => x.Moderator)
                .WithMany(x => x.ModeratedParticipants)
                .HasForeignKey(x => x.ModeratorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
