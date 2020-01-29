using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public class EventDbContext : IdentityDbContext  //Done: what is difference between IdentityDbContext and DbContext, the former has roles?
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<TimeProposed> TimeProposeds { get; set; }
        public DbSet<ActivityTimeInvitee> ActivityTimeInvitees { get; set; }
        //public DbSet<AccountActivity> ActivityAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ActivityTimeInvitee>().HasKey(ati => new
            {
                ati.ActivityId,
                ati.InviteeId,
                ati.TimeProposedId
            });

            //One to many relationship UserName-AccountActivity
            //builder.Entity<AccountActivity>()
            //    .HasMany<Activity>(aa => aa.Activities)
            //    .WithOne(a => a.AccountActivity)
            //    .HasForeignKey(a => a.ActivityId);
                //.HasOne(a => a.AccountActivity)
                //.WithMany(aa => aa.Activities)
                //.HasForeignKey(a => a.AccountActivityId);

            //One to many relationship Activity-ActivityTimeInvitee
            builder.Entity<ActivityTimeInvitee>()
                .HasOne(ati => ati.Activity)
                .WithMany(a => a.ActivityTimeInvitees)
                .HasForeignKey(ati => ati.ActivityId);

            //One to many relationship TimeProposed-ActivityTimeInvitee
            builder.Entity<ActivityTimeInvitee>()
                .HasOne(ati => ati.TimeProposed)
                .WithMany(tp => tp.ActivityTimeInvitees)
                .HasForeignKey(ati => ati.TimeProposedId);

            //One to many relationship Invitee-ActivityTimeInvitee
            builder.Entity<ActivityTimeInvitee>()
                .HasOne(ati => ati.Invitee)
                .WithMany(i => i.ActivityTimeInvitees)
                .HasForeignKey(ati => ati.InviteeId);

            //Configure properties of Activity
            builder.Entity<Activity>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();

            builder.Entity<Activity>()
                .Property(a => a.Description)
                .HasMaxLength(300)
                .IsUnicode();

            //configure properties of Invitee
            builder.Entity<Invitee>()
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode();

            //Configure properties of TimeProposed
            builder.Entity<TimeProposed>()
                .Property(tp => tp.StartTime)
                .IsRequired();

            builder.Entity<TimeProposed>()
                .Property(tp => tp.EndTime)
                .IsRequired();

            ////Configure Properties of AccountActivity
            //builder.Entity<AccountActivity>()
            //    .Property(aa => aa.UserName)
            //    .IsRequired();

            //Add some seed-data
            builder.Entity<Activity>().HasData(new Activity { 
                ActivityId = 1, 
                Name = "NameOfActivity1", 
                Guid = Guid.NewGuid(),
                AccountName = "Kees",
                Description = "Description here" });

            builder.Entity<Invitee>().HasData(new Invitee { InviteeId = 1, Name = "Anna" });
            builder.Entity<Invitee>().HasData(new Invitee { InviteeId = 2, Name = "Bas" });
            builder.Entity<Invitee>().HasData(new Invitee { InviteeId = 3, Name = "Cor" });

            builder.Entity<TimeProposed>().HasData(new TimeProposed { TimeProposedId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1) });
            builder.Entity<TimeProposed>().HasData(new TimeProposed { TimeProposedId = 2, StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(2) });
            builder.Entity<TimeProposed>().HasData(new TimeProposed { TimeProposedId = 3, StartTime = DateTime.Now.AddDays(2), EndTime = DateTime.Now.AddDays(3) });
            builder.Entity<TimeProposed>().HasData(new TimeProposed { TimeProposedId = 4, StartTime = DateTime.Now.AddDays(3), EndTime = DateTime.Now.AddDays(4) });
            builder.Entity<TimeProposed>().HasData(new TimeProposed { TimeProposedId = 5, StartTime = DateTime.Now.AddDays(4), EndTime = DateTime.Now.AddDays(5) });

            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 1, TimeProposedId = 1 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 1, TimeProposedId = 2 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 1, TimeProposedId = 3 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 1, TimeProposedId = 4 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 1, TimeProposedId = 5 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 2, TimeProposedId = 1 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 2, TimeProposedId = 2 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 2, TimeProposedId = 3 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 2, TimeProposedId = 4 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 2, TimeProposedId = 5 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 3, TimeProposedId = 1 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 3, TimeProposedId = 2 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 3, TimeProposedId = 3 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 3, TimeProposedId = 4 });
            builder.Entity<ActivityTimeInvitee>().HasData(new ActivityTimeInvitee { ActivityId = 1, InviteeId = 3, TimeProposedId = 5 });

        }
    }
}
