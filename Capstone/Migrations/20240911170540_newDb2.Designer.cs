﻿// <auto-generated />
using System;
using Capstone.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Capstone.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240911170540_newDb2")]
    partial class newDb2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Capstone.Models.Auth.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomeRuolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Capstone.Models.Auth.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("DataCreazione")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("PartiteGiocate")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RuoloPreferito")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("ValutazioneMedia")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Capstone.Models.Bookings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataPrenotazione")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PagamentoEffettuato")
                        .HasColumnType("bit");

                    b.Property<int>("PartitaId")
                        .HasColumnType("int");

                    b.Property<int>("StatoPrenotazione")
                        .HasColumnType("int");

                    b.Property<int>("UtenteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartitaId");

                    b.HasIndex("UtenteId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Capstone.Models.Chats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCreazione")
                        .HasColumnType("datetime2");

                    b.Property<int>("PartitaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartitaId")
                        .IsUnique();

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("Capstone.Models.Fields", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Città")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Indirizzo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeCampo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrezzoOrario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TipoCampo")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal>("ValutazioneMedia")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Capstone.Models.Matches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CampoId")
                        .HasColumnType("int");

                    b.Property<int>("CreatoreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataFine")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInizio")
                        .HasColumnType("datetime2");

                    b.Property<int>("Stato")
                        .HasColumnType("int");

                    b.Property<int>("TipoPartita")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CampoId");

                    b.HasIndex("CreatoreId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Capstone.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataInvio")
                        .HasColumnType("datetime2");

                    b.Property<int>("MittenteId")
                        .HasColumnType("int");

                    b.Property<string>("Testo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("MittenteId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Capstone.Models.Reviews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Commento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataRecensione")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FieldsId")
                        .HasColumnType("int");

                    b.Property<int>("Punteggio")
                        .HasColumnType("int");

                    b.Property<int>("TipoRecensione")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("ValutatoCampoId")
                        .HasColumnType("int");

                    b.Property<int?>("ValutatoGiocatoreId")
                        .HasColumnType("int");

                    b.Property<int>("ValutatoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FieldsId");

                    b.HasIndex("UserId");

                    b.HasIndex("ValutatoCampoId");

                    b.HasIndex("ValutatoGiocatoreId");

                    b.HasIndex("ValutatoreId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("RolesUsers", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RolesUsers");
                });

            modelBuilder.Entity("UserMatch", b =>
                {
                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MatchId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMatch");
                });

            modelBuilder.Entity("Capstone.Models.Bookings", b =>
                {
                    b.HasOne("Capstone.Models.Matches", "Partita")
                        .WithMany("Prenotazioni")
                        .HasForeignKey("PartitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Auth.Users", "Utente")
                        .WithMany("Prenotazioni")
                        .HasForeignKey("UtenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Partita");

                    b.Navigation("Utente");
                });

            modelBuilder.Entity("Capstone.Models.Chats", b =>
                {
                    b.HasOne("Capstone.Models.Matches", "Partita")
                        .WithOne("Chat")
                        .HasForeignKey("Capstone.Models.Chats", "PartitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Partita");
                });

            modelBuilder.Entity("Capstone.Models.Fields", b =>
                {
                    b.HasOne("Capstone.Models.Auth.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Capstone.Models.Matches", b =>
                {
                    b.HasOne("Capstone.Models.Fields", "Campo")
                        .WithMany("Partite")
                        .HasForeignKey("CampoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Auth.Users", "Creatore")
                        .WithMany("PartiteCreate")
                        .HasForeignKey("CreatoreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Campo");

                    b.Navigation("Creatore");
                });

            modelBuilder.Entity("Capstone.Models.Messages", b =>
                {
                    b.HasOne("Capstone.Models.Chats", "Chat")
                        .WithMany("Messaggi")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Auth.Users", "Mittente")
                        .WithMany("MessaggiInviati")
                        .HasForeignKey("MittenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Mittente");
                });

            modelBuilder.Entity("Capstone.Models.Reviews", b =>
                {
                    b.HasOne("Capstone.Models.Fields", null)
                        .WithMany("Reviews")
                        .HasForeignKey("FieldsId");

                    b.HasOne("Capstone.Models.Auth.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Fields", "ValutatoCampo")
                        .WithMany()
                        .HasForeignKey("ValutatoCampoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Capstone.Models.Auth.Users", "ValutatoGiocatore")
                        .WithMany("RecensioniRicevute")
                        .HasForeignKey("ValutatoGiocatoreId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Capstone.Models.Auth.Users", "Valutatore")
                        .WithMany("RecensioniLasciate")
                        .HasForeignKey("ValutatoreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("ValutatoCampo");

                    b.Navigation("ValutatoGiocatore");

                    b.Navigation("Valutatore");
                });

            modelBuilder.Entity("RolesUsers", b =>
                {
                    b.HasOne("Capstone.Models.Auth.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Auth.Users", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserMatch", b =>
                {
                    b.HasOne("Capstone.Models.Matches", null)
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone.Models.Auth.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Capstone.Models.Auth.Users", b =>
                {
                    b.Navigation("MessaggiInviati");

                    b.Navigation("PartiteCreate");

                    b.Navigation("Prenotazioni");

                    b.Navigation("RecensioniLasciate");

                    b.Navigation("RecensioniRicevute");
                });

            modelBuilder.Entity("Capstone.Models.Chats", b =>
                {
                    b.Navigation("Messaggi");
                });

            modelBuilder.Entity("Capstone.Models.Fields", b =>
                {
                    b.Navigation("Partite");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Capstone.Models.Matches", b =>
                {
                    b.Navigation("Chat")
                        .IsRequired();

                    b.Navigation("Prenotazioni");
                });
#pragma warning restore 612, 618
        }
    }
}
