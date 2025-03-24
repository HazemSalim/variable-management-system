using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VariableManagementSystem.Enums;
using VariableManagementSystem.Models;
using VariableManagementSystem.Services;
using System;

namespace VariableManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariableController : ControllerBase
    {
        private readonly IVariableService _variableService;
        private readonly ILogger<VariableController> _logger;

        // Constructor Injection for both the service and logger
        public VariableController(IVariableService variableService, ILogger<VariableController> logger)
        {
            _variableService = variableService;
            _logger = logger;
        }

        // GET: api/Variable
        /// <summary>
        /// Retrieves all variables in the system.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllVariables()
        {
            var variables = await _variableService.GetAllVariablesAsync();
            return Ok(variables);
        }

        // GET: api/Variable/{id}
        /// <summary>
        /// Retrieves a variable by its unique identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVariableById(Guid id)
        {
            var variable = await _variableService.GetVariableByIdAsync(id);
            if (variable == null)
            {
                _logger.LogWarning($"Variable with id {id} not found.");
                return NotFound();
            }
            return Ok(variable);
        }

        // POST: api/Variable
        /// <summary>
        /// Creates a new variable in the system.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateVariable([FromBody] Variable variable)
        {
            // Validate variable type
            if (!Enum.IsDefined(typeof(VariableType), variable.Type))
            {
                return BadRequest("Invalid variable type.");
            }

            // Check for required fields (you could make the validation more robust based on your model)
            if (string.IsNullOrEmpty(variable.Identifier))
            {
                return BadRequest("Variable identifier is required.");
            }

            variable.Id = Guid.NewGuid();
            await _variableService.CreateVariableAsync(variable);
            _logger.LogInformation($"Variable with id {variable.Id} created successfully.");
            return CreatedAtAction(nameof(GetVariableById), new { id = variable.Id }, variable);
        }

        // PUT: api/Variable/{id}
        /// <summary>
        /// Updates the value of an existing variable.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVariable(Guid id, [FromBody] string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                return BadRequest("New value is required.");
            }

            try
            {
                await _variableService.UpdateVariableAsync(id, newValue);
                _logger.LogInformation($"Variable with id {id} updated successfully.");
                return NoContent();  // Indicates success with no content
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating variable with id {id}: {ex.Message}");
                return NotFound(new { Message = ex.Message });
            }
        }

        // DELETE: api/Variable/{id}
        /// <summary>
        /// Deletes an existing variable by its identifier.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariable(Guid id)
        {
            try
            {
                await _variableService.DeleteVariableAsync(id);
                _logger.LogInformation($"Variable with id {id} deleted successfully.");
                return Ok(new { Message = "Variable deleted successfully." });  // Return success message
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting variable with id {id}: {ex.Message}");
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
