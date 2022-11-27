#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BuscaPublicaTJeMG.Models
{
    public partial class Movimentacao
    {
        [Key]
        public int Id { get; set; }
        public int? IdProcesso { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataMovimentacao { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Descricao { get; set; }

        [ForeignKey("IdProcesso")]
        [InverseProperty("Movimentacao")]
        public virtual Processo IdProcessoNavigation { get; set; }
    }
}