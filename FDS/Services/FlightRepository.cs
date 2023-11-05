using FDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FDS.Services
{
    public class FlightRepository : IFlight
    {
        private readonly FDSDbContext _context;

        public FlightRepository(FDSDbContext context) 
        { 
            _context = context;
        }
        public async Task<Flight> Add(Flight flight)
        {
            var newFlight = new Flight
            {
                FlightNo = flight.FlightNo,
                Date = flight.Date,
                PointOfLoading = flight.PointOfLoading,
                PointOfUnLoading = flight.PointOfUnLoading,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime
            };

            _context.AddAsync(newFlight);
            await _context.SaveChangesAsync();
            return newFlight;
                   
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Flights
                                        .Include(a => a.Documents)
                                        .FirstOrDefaultAsync(f => f.Id == id);
            if (result == null)
            {
                return false;
            }
            // Xóa tất cả các tài liệu liên kết
            _context.Documents.RemoveRange(result.Documents);
            _context.Flights.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Flight>> GetAll()
        {
            var result = await _context.Flights.ToListAsync();
            return result;
        }

        public async Task<Flight> GetById(int id)
        {
            var result = await _context.Flights
                                        .Include(a => a.Documents)
                                        .FirstOrDefaultAsync(f => f.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> Update(Flight flight, int id)
        {
            var result = await _context.Flights.SingleOrDefaultAsync(f => f.Id == id);
            if(result == null)
            {
                return false;
            }
            result.FlightNo = flight.FlightNo;
            result.Date = flight.Date;
            result.PointOfLoading = flight.PointOfLoading;
            result.PointOfUnLoading = flight.PointOfUnLoading;
            result.DepartureTime = flight.DepartureTime;
            result.ArrivalTime = flight.ArrivalTime;

            _context.Update(result);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
