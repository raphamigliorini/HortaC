using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esg.Horta.Entities
{
    [Table("ESPECIE")]
    public class Especie
    {
        [Key]
        [Column("ID")]
        // O Oracle vai gerar via SEQ_ESPECIE (config no DbContext)
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MinLength(1, ErrorMessage = "Nome não pode ser vazio.")]
        [StringLength(60)]
        [Column("NOME", TypeName = "VARCHAR2(60)")]
        public string Nome { get; set; } = string.Empty;
    }
}
