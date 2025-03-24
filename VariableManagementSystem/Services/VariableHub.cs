using Microsoft.AspNetCore.SignalR;

namespace VariableManagementSystem.Services
{
    public class VariableHub : Hub
    {
        // Send updates to clients when a variable's value changes
        public async Task NotifyVariableChange(int variableId, string variableName, object oldValue, object newValue)
        {
            // This method will broadcast the change to all connected clients
            await Clients.All.SendAsync("ReceiveVariableUpdate", variableId, variableName, oldValue, newValue);
        }
    }
}
