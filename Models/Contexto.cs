using BuscaPublicaTJeMG.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
#nullable disable



namespace BuscaPublicaTJeMG.Models
{
    public partial class Contexto : DbContext
    {
        public Contexto()
        {
        }

        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }

        public virtual DbSet<Movimentacao> Movimentacao { get; set; }
        public virtual DbSet<Participante> Participante { get; set; }
        public virtual DbSet<Processo> Processo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               
                optionsBuilder.UseSqlServer("Data Source=localhost\\sql2019;Initial Catalog=ProcessosTJE;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.MovimentacaoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ParticipanteConfiguration());

            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
