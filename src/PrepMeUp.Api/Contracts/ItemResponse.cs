namespace PrepMeUp.Api.Contracts;

// Describe what API promises to return, when a client sends GET / items (= the endpoint that ItemController.GetItems() handles)
// Promises to return a json array of of { "id": ..., "name": ... } objects — one per row currently in the Items table
public record ItemResponse(Guid Id, string Name);