using Microsoft.AspNetCore.SignalR;
using Serilog;
using VariableManagementSystem.Models;
using VariableManagementSystem.Repositories;
using System;

namespace VariableManagementSystem.Services
{
    public class VariableService : IVariableService
    {
        private readonly IVariableRepository _repository;
        private readonly IHubContext<VariableHub> _hubContext;

        // Constructor to inject repository and SignalR hub context
        public VariableService(IVariableRepository repository, IHubContext<VariableHub> hubContext)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        // Fetch all variables asynchronously
        public async Task<IEnumerable<Variable>> GetAllVariablesAsync()
        {
            try
            {
                var variables = await _repository.GetAllAsync();
                Log.Information("Successfully retrieved all variables.");
                return variables;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching all variables.");
                throw new ApplicationException("Error fetching variables", ex);
            }
        }

        // Fetch a variable by its ID asynchronously
        public async Task<Variable> GetVariableByIdAsync(Guid id)
        {
            try
            {
                var variable = await _repository.GetByIdAsync(id);
                if (variable == null)
                {
                    Log.Warning("Variable with ID {VariableId} not found.", id);
                    throw new KeyNotFoundException($"Variable with ID {id} not found.");
                }
                Log.Information("Successfully retrieved variable with ID {VariableId}.", id);
                return variable;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching variable with ID {VariableId}.", id);
                throw new ApplicationException("Error fetching variable by ID", ex);
            }
        }

        // Fetch a variable by its identifier asynchronously
        public async Task<Variable> GetVariableByIdentifierAsync(string identifier)
        {
            try
            {
                var variable = await _repository.GetByIdentifierAsync(identifier);
                if (variable == null)
                {
                    Log.Warning("Variable with identifier {VariableIdentifier} not found.", identifier);
                    throw new KeyNotFoundException($"Variable with identifier {identifier} not found.");
                }
                Log.Information("Successfully retrieved variable with identifier {VariableIdentifier}.", identifier);
                return variable;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching variable with identifier {VariableIdentifier}.", identifier);
                throw new ApplicationException("Error fetching variable by identifier", ex);
            }
        }

        // Create a new variable and notify clients of the new variable
        public async Task<Variable> CreateVariableAsync(Variable variable)
        {
            if (variable == null)
            {
                Log.Error("Variable cannot be null.");
                throw new ArgumentNullException(nameof(variable), "Variable cannot be null.");
            }

            try
            {
                // Add the variable to the database
                await _repository.AddAsync(variable);

                // Log the creation event
                Log.Information("Variable created: {VariableName}", variable.Identifier);

                // Notify all connected clients about the new variable
                await _hubContext.Clients.All.SendAsync("ReceiveVariableUpdate", variable.Id, variable.Identifier, null, variable.Value);

                return variable;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating variable {VariableName}.", variable.Identifier);
                throw new ApplicationException("Error creating variable", ex);
            }
        }

        // Update an existing variable and notify clients of the update
        public async Task UpdateVariableAsync(Guid id, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                Log.Error("New value cannot be null or empty.");
                throw new ArgumentException("New value cannot be null or empty.", nameof(newValue));
            }

            try
            {
                // Fetch the variable from the repository
                var variable = await _repository.GetByIdAsync(id);
                if (variable == null)
                {
                    Log.Warning("Variable with ID {VariableId} not found.", id);
                    throw new KeyNotFoundException($"Variable with ID {id} not found.");
                }

                var oldValue = variable.Value;

                // Update the variable value and timestamp
                variable.Value = newValue;
                variable.UpdatedAt = DateTime.UtcNow;

                // Log the update event
                Log.Information("Variable {VariableName} updated from {OldValue} to {NewValue}.", variable.Identifier, oldValue, newValue);

                // Persist the change in the database
                await _repository.UpdateAsync(variable);

                // Notify all connected clients about the updated variable
                await _hubContext.Clients.All.SendAsync("ReceiveVariableUpdate", variable.Id, variable.Identifier, oldValue, newValue);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating variable with ID {VariableId}.", id);
                throw new ApplicationException("Error updating variable", ex);
            }
        }

        // Delete an existing variable and notify clients of the deletion
        public async Task DeleteVariableAsync(Guid id)
        {
            try
            {
                // Fetch the variable from the repository
                var variable = await _repository.GetByIdAsync(id);
                if (variable == null)
                {
                    Log.Warning("Variable with ID {VariableId} not found for deletion.", id);
                    throw new KeyNotFoundException($"Variable with ID {id} not found for deletion.");
                }

                // Delete the variable
                await _repository.DeleteAsync(id);

                // Log the deletion event
                Log.Information("Variable {VariableName} (ID: {VariableId}) deleted.", variable.Identifier, id);

                // Notify all connected clients about the deletion
                await _hubContext.Clients.All.SendAsync("ReceiveVariableDeletion", id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting variable with ID {VariableId}.", id);
                throw new ApplicationException("Error deleting variable", ex);
            }
        }
    }
}
