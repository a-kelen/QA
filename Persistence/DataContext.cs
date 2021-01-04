using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User,IdentityRole<Guid>, Guid>
    {
        //public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions{ get; set; }
        public DbSet<Room>  Rooms{ get; set; }
        public DbSet<Like> Likes  { get; set; }
        public DbSet<UserRoom> UserRoom  { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
          
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Answer>().HasKey(x => x.Id);
            builder.Entity<Question>().HasKey(x => x.Id);
            builder.Entity<Room>().HasKey(x => x.Id);
            builder.Entity<Answer>()
                .Property(x => x.Created)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Question>()
                .Property(x => x.Created)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Room>()
                .Property(x => x.Created)
                .HasDefaultValueSql("getdate()");
            builder.Entity<User>()
                .Property(x => x.Created)
                .HasDefaultValueSql("getdate()");

            builder.Entity<User>()
                .HasMany(x => x.Rooms)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            builder.Entity<User>()
                .HasMany(x => x.Questions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Room>()
                .HasMany(x => x.Questions)
                .WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId);

            builder.Entity<User>(x => 
                x.ToTable("AspNetUsers")
            );

            //builder.Entity<Like>(x => x.HasKey(k => 
            //    new {k.QuestionId, k.UserId}
            //));
            builder.Entity<Question>().HasOne(x => x.Answer).WithOne(x => x.Question);
            builder.Entity<Like>()
                .HasOne(x => x.User)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            ///SEED
            var user = new User
            {
                Id = new Guid("497D3B94-0EFC-4A1C-9CBA-2C7F08B476F6"),
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                Lastname = "Ivanov",
                Firstname = "Ivan",
                Nickname = "Ivank",
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                SecurityStamp = "I3I3AH23LKZUJ7REI433F55OD6VPT7N4"
            };
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, "_123123Aa");
            user.PasswordHash = hashedPassword;
            builder.Entity<User>().HasData(
                    user
                );
            builder.Entity<Room>().HasData(
                    new Room { 
                        Id = new Guid("BD3236AE-B995-42A8-A63C-DE996E13312D"),
                        Name = "Default Room",
                        Description = "Description",
                        UserId = new Guid("497D3B94-0EFC-4A1C-9CBA-2C7F08B476F6")
                    },
                      new Room
                      {
                          Id = new Guid("6715BB3E-0109-4CF8-8051-9E99891B7389"),
                          Name = "RoomD",
                          Description = "Description",
                          UserId = new Guid("497D3B94-0EFC-4A1C-9CBA-2C7F08B476F6")
                      }
                );
            builder.Entity<Question>().HasData(
                    new Question { 
                        Id = new Guid("122AAAF3-EE68-4A86-A7EF-5584BFE3FAD5"),
                        Title = "First question",
                        Description = "Lorem lorem",
                        Ranking = 0,
                        RoomId = new Guid("BD3236AE-B995-42A8-A63C-DE996E13312D"),
                        UserId = new Guid("497D3B94-0EFC-4A1C-9CBA-2C7F08B476F6")
                    }
                );

        }
    }
}
