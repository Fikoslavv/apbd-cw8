using Microsoft.AspNetCore.Mvc;

namespace apbd_cw8;

[ApiController]
[Route("[controller]")]
public class ClientTripController : ControllerBase
{
    protected IService<ClientTrip> clientTripService;
    protected IService<Client> clientService;
    protected IService<Trip> tripService;
    protected IService<Country> countryService;
    protected IService<CountryTrip> countryTripService;

    public ClientTripController(IService<ClientTrip> clientTripService, IService<Client> clientService, IService<Trip> tripService, IService<Country> countryService, IService<CountryTrip> countryTripService)
    {
        this.clientTripService = clientTripService;
        this.clientService = clientService;
        this.tripService = tripService;
        this.countryService = countryService;
        this.countryTripService = countryTripService;
    }

    [HttpGet("/api/clients/{id}/trips")]
    public async Task<IActionResult> GetClientTripsAsync(int id) => await Task.Run(() => this.GetClientTrips(id));

    protected IActionResult GetClientTrips(int id)
    {
        if (!this.clientService.Where(c => c.IdClient == id).Any()) return this.NotFound("There is no client with given id.");

        var clientTripQuerry = this.clientTripService.Where(d => d.IdClient == id);

        if (!clientTripQuerry.Any()) return this.NotFound("Client with given id has no trips.");

        var tripIds = clientTripQuerry.Select(d => d.IdTrip).ToHashSet();
        var countries = this.countryService.ToDictionary(c => c.IdCountry);
        var trips = this.tripService.Where(t => tripIds.Contains(t.IdTrip)).ToDictionary(t => t.IdTrip);

        var output = new object[clientTripQuerry.Count()];

        var clientTripEnumerator = clientTripQuerry.GetEnumerator();
        clientTripEnumerator.MoveNext();
        for (int i = 0; i < output.Length; i++, clientTripEnumerator.MoveNext())
        {
            var trip = trips[clientTripEnumerator.Current.IdTrip];

            output[i] = new
            {
                ClientTrip = clientTripEnumerator.Current,
                Trip = new
                {
                    IdTrip = trip.IdTrip,
                    Name = trip.Name,
                    Description = trip.Description,
                    DateFrom = trip.DateFrom,
                    DateTo = trip.DateTo,
                    MaxPeople = trip.MaxPeople,
                    Countries = this.countryTripService.Where(ct => ct.IdTrip == trip.IdTrip).Select(ct => countries[ct.IdCountry]).ToArray(),
                },
            };
        }

        return this.Ok(output);
    }

    [HttpPut("/api/clients/{clientId}/trips/{tripId}")]
    public async Task<IActionResult> PutClientTripAsync(int clientId, int tripId) => await Task.Run(() => this.PutClientTrip(clientId, tripId));

    protected IActionResult PutClientTrip(int clientId, int tripId)
    {
        var trip = this.tripService.Where(t => t.IdTrip == tripId).SingleOrDefault();

        if (trip is null) return this.NotFound("No trip with given id was found.");
        if (!this.clientService.Where(c => c.IdClient == clientId).Any()) return this.NotFound("No client with given id was found.");

        var clientTrip = this.clientTripService.Where(d => d.IdClient == clientId && d.IdTrip == tripId).SingleOrDefault();
        var registeredClients = this.clientTripService.Where(d => d.IdTrip == tripId).Count();

        if (clientTrip is not null) return this.Conflict("The client is already registered on this trip.");
        else if (registeredClients >= trip.MaxPeople) return this.Forbid("This trip has reached max number of clients registered.");
        else
        {
            clientTrip = new() { IdClient = clientId, IdTrip = tripId, RegisteredAt = int.Parse(DateTime.Now.ToString("yyyyMMdd")), PaymentDate = null };
            return this.clientTripService.InsertData(clientTrip) != -1 ? this.Created() : this.BadRequest("Failed to register client for the trip.");
        }
    }

    [HttpDelete("/api/clients/{clientId}/trips/{tripId}")]
    public async Task<IActionResult> DeleteClientTripAsync(int clientId, int tripId) => await Task.Run(() => this.DeleteClientTrip(clientId, tripId));

    protected IActionResult DeleteClientTrip(int clientId, int tripId) => this.clientTripService.DeleteData(new() { IdClient = clientId, IdTrip = tripId }) ? this.Ok() : this.NotFound("Failed to delete the client's reservation.");
}
