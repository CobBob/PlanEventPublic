﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlanEvent.Data;

namespace PlanEvent.Migrations
{
    [DbContext(typeof(EventDbContext))]
    partial class EventDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PlanEvent.Data.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<Guid>("Guid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.HasKey("ActivityId");

                    b.ToTable("Activities");

                    b.HasData(
                        new
                        {
                            ActivityId = 1,
                            AccountName = "Kees",
                            Description = "Description here",
                            Guid = new Guid("639e5c81-05af-471e-9609-915c9067a53a"),
                            Name = "NameOfActivity1"
                        });
                });

            modelBuilder.Entity("PlanEvent.Data.ActivityTimeInvitee", b =>
                {
                    b.Property<int>("ActivityId");

                    b.Property<int>("InviteeId");

                    b.Property<int>("TimeProposedId");

                    b.Property<int>("Availability");

                    b.HasKey("ActivityId", "InviteeId", "TimeProposedId");

                    b.HasIndex("InviteeId");

                    b.HasIndex("TimeProposedId");

                    b.ToTable("ActivityTimeInvitees");

                    b.HasData(
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 1,
                            TimeProposedId = 1,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 1,
                            TimeProposedId = 2,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 1,
                            TimeProposedId = 3,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 1,
                            TimeProposedId = 4,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 1,
                            TimeProposedId = 5,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 2,
                            TimeProposedId = 1,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 2,
                            TimeProposedId = 2,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 2,
                            TimeProposedId = 3,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 2,
                            TimeProposedId = 4,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 2,
                            TimeProposedId = 5,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 3,
                            TimeProposedId = 1,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 3,
                            TimeProposedId = 2,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 3,
                            TimeProposedId = 3,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 3,
                            TimeProposedId = 4,
                            Availability = 0
                        },
                        new
                        {
                            ActivityId = 1,
                            InviteeId = 3,
                            TimeProposedId = 5,
                            Availability = 0
                        });
                });

            modelBuilder.Entity("PlanEvent.Data.Invitee", b =>
                {
                    b.Property<int>("InviteeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(true);

                    b.HasKey("InviteeId");

                    b.ToTable("Invitees");

                    b.HasData(
                        new
                        {
                            InviteeId = 1,
                            Name = "Anna"
                        },
                        new
                        {
                            InviteeId = 2,
                            Name = "Bas"
                        },
                        new
                        {
                            InviteeId = 3,
                            Name = "Cor"
                        });
                });

            modelBuilder.Entity("PlanEvent.Data.TimeProposed", b =>
                {
                    b.Property<int>("TimeProposedId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("TimeProposedId");

                    b.ToTable("TimeProposeds");

                    b.HasData(
                        new
                        {
                            TimeProposedId = 1,
                            EndTime = new DateTime(2019, 11, 27, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(5361),
                            StartTime = new DateTime(2019, 11, 26, 11, 39, 18, 369, DateTimeKind.Local).AddTicks(6817)
                        },
                        new
                        {
                            TimeProposedId = 2,
                            EndTime = new DateTime(2019, 11, 28, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6246),
                            StartTime = new DateTime(2019, 11, 27, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6233)
                        },
                        new
                        {
                            TimeProposedId = 3,
                            EndTime = new DateTime(2019, 11, 29, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6274),
                            StartTime = new DateTime(2019, 11, 28, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6271)
                        },
                        new
                        {
                            TimeProposedId = 4,
                            EndTime = new DateTime(2019, 11, 30, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6291),
                            StartTime = new DateTime(2019, 11, 29, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6288)
                        },
                        new
                        {
                            TimeProposedId = 5,
                            EndTime = new DateTime(2019, 12, 1, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6306),
                            StartTime = new DateTime(2019, 11, 30, 11, 39, 18, 371, DateTimeKind.Local).AddTicks(6304)
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlanEvent.Data.ActivityTimeInvitee", b =>
                {
                    b.HasOne("PlanEvent.Data.Activity", "Activity")
                        .WithMany("ActivityTimeInvitees")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlanEvent.Data.Invitee", "Invitee")
                        .WithMany("ActivityTimeInvitees")
                        .HasForeignKey("InviteeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlanEvent.Data.TimeProposed", "TimeProposed")
                        .WithMany("ActivityTimeInvitees")
                        .HasForeignKey("TimeProposedId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
