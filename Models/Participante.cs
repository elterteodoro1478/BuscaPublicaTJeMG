#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace BuscaPublicaTJeMG.Models
{
    public partial class Participante
    {
        [Key]
        public int Id { get; set; }
        public int? IdProcesso { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Nome { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Documento { get; set; }
        [StringLength(50)]
        [Unicode(false)]

        public string TipoDocumento { get; set; }

        public string Polo { get; set; }

        [ForeignKey("IdProcesso")]
        [InverseProperty("Participante")]
        public virtual Processo IdProcessoNavigation { get; set; }
    }
}