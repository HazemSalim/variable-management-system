using Microsoft.EntityFrameworkCore;
using VariableManagementSystem.Data;
using VariableManagementSystem.Models;

namespace VariableManagementSystem.Repositories
{
    public class VariableRepository : IVariableRepository
    {
        private readonly AppDbContext _context;

        public VariableRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retrieve all variables
        public async Task<IEnumerable<Variable>> GetAllAsync()
        {
            return await _context.Variables.ToListAsync(); // Gets all records from the database
        }

        // Retrieve a variable by its ID
        public async Task<Variable> GetByIdAsync(Guid id)
        {
            return await _context.Variables.FindAsync(id); // Finds the variable with the given ID
        }

        // Retrieve a variable by its identifier
        public async Task<Variable> GetByIdentifierAsync(string identifier)
        {
            return await _context.Variables.FirstOrDefaultAsync(v => v.Identifier == identifier); // Finds the first match based on identifier
        }

        // Add a new variable to the database
        public async Task AddAsync(Variable variable)
        {
            _context.Variables.Add(variable); // Adds the new variable
            await _context.SaveChangesAsync(); // Saves the changes to the database
        }

        // Update an existing variable
        public async Task UpdateAsync(Variable variable)
        {
            _context.Variables.Update(variable); // Updates the variable in the context
            await _context.SaveChangesAsync(); // Saves the changes to the database
        }

        // Delete a variable by its ID
        public async Task DeleteAsync(Guid id)
        {
            var variable = await GetByIdAsync(id); // Retrieve the variable to be deleted
            if (variable != null)
            {
                _context.Variables.Remove(variable); // Removes the variable from the context
                await _context.SaveChangesAsync(); // Saves the changes to the database
            }
        }
    }
}
