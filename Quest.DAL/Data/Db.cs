﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
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
