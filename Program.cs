namespace apbd_cw8;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("mssql_pjwstk") ?? throw new ArgumentNullException("Connection string is undefined in the configuration !");

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IService<Client>, ClientService>(provider => new ClientService(connectionString));
        builder.Services.AddScoped<IService<Trip>, TripService>(provider => new TripService(connectionString));
        builder.Services.AddScoped<IService<ClientTrip>, ClientTripService>(provider => new ClientTripService(connectionString));
        builder.Services.AddScoped<IService<Country>, CountryService>(provider => new CountryService(connectionString));
        builder.Services.AddScoped<IService<CountryTrip>, CountryTripService>(provider => new CountryTripService(connectionString));

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
