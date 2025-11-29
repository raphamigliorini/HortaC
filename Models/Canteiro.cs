using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiapWebAluno.Models; // onde estão SensorUmidade, Irrigacao, Colheita

namespace Esg.Horta.Entities
{
    [Table("CANTEIRO")]
    public class Canteiro
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Column("NOME", TypeName = "VARCHAR2(255)")]
        public string Nome { get; set; } = string.Empty;

        [Column("ESPECIE_ID")]
        public long EspecieId { get; set; }

        [Column("AREA_M2", TypeName = "NUMBER(7,2)")]
        public decimal? AreaM2 { get; set; }

        [Column("META_DOACAO_KG", TypeName = "NUMBER(9,2)")]
        public decimal? MetaDoacaoKg { get; set; }

        // ==========================
        // Navegações (necessárias
        // para o WithMany no contexto)
        // ==========================
        public ICollection<SensorUmidade> SensoresUmidade { get; set; } = new List<SensorUmidade>();
        public ICollection<Irrigacao> Irrigacoes { get; set; } = new List<Irrigacao>();
        public ICollection<Colheita> Colheitas { get; set; } = new List<Colheita>();
    }
}
