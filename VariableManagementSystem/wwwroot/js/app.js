const connection = new signalR.HubConnectionBuilder()
    .withUrl("/variableHub") // URL for SignalR hub
    .build();

// Listen for variable updates
connection.on("ReceiveVariableUpdate", (variableId, variableName, oldValue, newValue) => {
    console.log(`Variable ${variableName} updated!`);
    console.log(`Old value: ${oldValue}, New value: ${newValue}`);
    document.getElementById("variables").innerHTML = `Updated Variable: ${variableName}, Old: ${oldValue}, New: ${newValue}`;
});

// Listen for variable deletions
connection.on("ReceiveVariableDeletion", (variableId) => {
    console.log(`Variable with ID ${variableId} has been deleted.`);
    document.getElementById("variables").innerHTML = `Variable with ID ${variableId} has been deleted.`;
});

// Start the connection to SignalR
connection.start()
    .catch(err => console.error("Error while starting connection: " + err));
