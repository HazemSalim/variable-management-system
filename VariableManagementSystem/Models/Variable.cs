using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VariableManagementSystem.Enums;

namespace VariableManagementSystem.Models
{
    public class Variable
    {
        [Key]
        public Guid Id { get; set; } // Unique identifier for the variable

        [Required]
        public string Identifier { get; set; } // Unique identifier for the variable (e.g., a name or code)

        [Required]
        public VariableType Type { get; set; } // The type of the variable (e.g., Boolean, Integer)

        [Required]
        public string Value { get; set; } // The value of the variable (as a string)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // The timestamp when the variable was created
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // The timestamp when the variable was last updated
    }
}
