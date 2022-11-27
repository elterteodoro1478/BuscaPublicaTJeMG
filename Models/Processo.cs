#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BuscaPublicaTJeMG.Models
{
    public partial class Processo
    {
        public Processo()
        {
            Movimentacao = new HashSet<Movimentacao>();
            Participante = new HashSet<Participante>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string NumeroProcesso { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataDistribuicao { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string ClasseJuridicial { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Assunto { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Jurisdicao { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string OrgaoJulgador { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string NumeroProcessoReferencia { get; set; }

        [InverseProperty("IdProcessoNavigation")]
        public virtual ICollection<Movimentacao> Movimentacao { get; set; }
        [InverseProperty("IdProcessoNavigation")]
        public virtual ICollection<Participante> Participante { get; set; }
    }
}