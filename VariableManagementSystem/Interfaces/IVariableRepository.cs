using VariableManagementSystem.Models;

namespace VariableManagementSystem.Repositories
{
    public interface IVariableRepository
    {
        // Get all variables
        Task<IEnumerable<Variable>> GetAllAsync();

        // Get a variable by its unique ID
        Task<Variable> GetByIdAsync(Guid id);

        // Get a variable by its unique identifier
        Task<Variable> GetByIdentifierAsync(string identifier);

        // Add a new variable to the database
        Task AddAsync(Variable variable);

        // Update an existing variable in the database
        Task UpdateAsync(Variable variable);

        // Delete a variable by its ID
        Task DeleteAsync(Guid id);
    }
}
