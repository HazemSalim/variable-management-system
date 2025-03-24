using VariableManagementSystem.Models;

namespace VariableManagementSystem.Services
{
    public interface IVariableService
    {
        // Retrieve all variables
        Task<IEnumerable<Variable>> GetAllVariablesAsync();

        // Retrieve a variable by its unique ID
        Task<Variable> GetVariableByIdAsync(Guid id);

        // Retrieve a variable by its unique identifier
        Task<Variable> GetVariableByIdentifierAsync(string identifier);

        // Create a new variable
        Task<Variable> CreateVariableAsync(Variable variable);

        // Update a variable's value by its ID
        Task UpdateVariableAsync(Guid id, string newValue);

        // Delete a variable by its ID
        Task DeleteVariableAsync(Guid id);
    }
}
