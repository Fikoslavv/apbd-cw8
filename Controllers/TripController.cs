using Microsoft.AspNetCore.Mvc;

namespace apbd_cw8;

[ApiController]
[Route("/api/[controller]")]
public class TripsController : ControllerBase
{
    protected IService<Trip> tripService;
    protected IService<Country> countryService;
    protected IService<CountryTrip> countryTripService;

    public TripsController(IService<Trip> tripService, IService<Country> countryService, IService<CountryTrip> countryTripService)
    {
        this.tripService = tripService;
        this.countryService = countryService;
        this.countryTripService = countryTripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync() => await Task.Run(() => this.GetTrips());

    private IActionResult GetTrips()
    {
        var output = new LinkedList<object>();
        var countries = this.countryService.ToDictionary(c => c.IdCountry);

        foreach (var trip in this.tripService)
        {
            output.AddLast
            (
                new
                {
                    IdTrip = trip.IdTrip,
                    Name = trip.Name,
                    Description = trip.Description,
                    DateFrom = trip.DateFrom,
                    DateTo = trip.DateTo,
                    MaxPeople = trip.MaxPeople,
                    Countries = this.countryTripService.Where(ct => ct.IdTrip == trip.IdTrip).Select(ct => countries[ct.IdCountry]).ToArray(),
                }
            );
        }

        return this.Ok(output);
    }

    [HttpPost]
    public async Task<IActionResult> PostTripAsync(Trip trip) => await Task.Run(() => this.PostTrip(trip));

    private IActionResult PostTrip(Trip trip) => this.tripService.InsertData(trip) ? this.Ok(new { trip.IdTrip }) : this.BadRequest("Failed to add the trip.");

    [HttpDelete("/api/[controller]/trips/{id}")]
    public async Task<IActionResult> DeleteTripAsync(int id) => await Task.Run(() => this.DeleteTrip(id));

    private IActionResult DeleteTrip(int id) => this.tripService.DeleteData(new() { IdTrip = id }) ? this.Ok() : this.BadRequest("Failed to delete the trip.");
}
