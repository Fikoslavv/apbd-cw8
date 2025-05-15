using Microsoft.AspNetCore.Mvc;

namespace apbd_cw8;

[ApiController]
[Route("/api/[controller]")]
public class ClientsController : ControllerBase
{
    protected IService<Client> clientService;

    public ClientsController(IService<Client> clientService)
    {
        this.clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientsAsync([FromQuery] IDictionary<string, string[]> filters) => await Task.Run(() => this.GetClients(filters));

    protected IActionResult GetClients(IDictionary<string, string[]> filters) => this.Ok(this.clientService.GetData(filters));

    [HttpPost]
    public async Task<IActionResult> PostClientAsync(Client client) => await Task.Run(() => this.PostClient(client));

    protected IActionResult PostClient(Client client)
    {
        if (client.Pesel.Length != 11 || !client.Pesel.Select(char.IsAsciiDigit).All(r => r)) return this.BadRequest("Pesel is not correctly formatted.");
        else if (System.Text.RegularExpressions.Regex.Matches(client.Email, @"[a-zA-Z0-9]+@[a-zA-Z0-9]+([.][a-zA-Z0-9]+)+").Count != 1) return this.BadRequest("Email is not correctly formatted.");
        else if (System.Text.RegularExpressions.Regex.Matches(client.Telephone, @"[+]{0,1}[0-9]+").Count != 1) return this.BadRequest("Telephone number is not correctly formatted.");

        // return this.clientService.InsertData(client) ? this.Ok(new { this.clientService.Where(c => c.Email == client.Email && c.FirstName == client.FirstName && c.LastName == client.LastName && c.Pesel == client.Pesel && c.Telephone == client.Telephone).LastOrDefault()?.IdClient }) : this.BadRequest("Failed to post the client.");
        return this.clientService.InsertData(client) != -1 ? this.Ok(new { client.IdClient }) : this.BadRequest("Failed to post the client.");
    }

    [HttpDelete("/api/[controller]/{id}")]
    public async Task<IActionResult> DeleteClientAsync(int id) => await Task.Run(() => this.DeleteClient(id));

    protected IActionResult DeleteClient(int id) => this.clientService.DeleteData(new() { IdClient = id }) ? this.Ok() : this.NotFound("Failed to delete the client.");
}
