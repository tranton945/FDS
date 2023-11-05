using FDS.Data;

namespace FDS.Services
{
    public interface IFlight
    {
        public Task<List<Flight>> GetAll();
        public Task<Flight> GetById(int id);
        public Task<Flight> Add(Flight flight);
        public Task<bool> Update(Flight flight, int id);
        public Task<bool> Delete(int id);
    }
}
