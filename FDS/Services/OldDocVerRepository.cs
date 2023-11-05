using FDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FDS.Services
{
    public class OldDocVerRepository : IOldDocVerRepository
    {
        private readonly FDSDbContext _context;

        public OldDocVerRepository(FDSDbContext context) 
        {
            _context = context;
        
        }
        public async Task<bool> Delete(int id)
        {
            var result = await _context.OldDocVers.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByDocId(int docId)
        {
            var result = await _context.OldDocVers.Where(i => i.DocId == docId).ToListAsync();
            if (result == null)
            {
                return false;
            }
            _context.RemoveRange(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OldDocVer>> getAll()
        {
            var result = await _context.OldDocVers.ToListAsync();
            return result;
        }

        public async Task<List<OldDocVer>> getAllByDocId(int docId)
        {
            var result = await _context.OldDocVers.Where(i => i.DocId == docId).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<OldDocVer> getByDocId(int docId)
        {
            var result = await _context.OldDocVers.SingleOrDefaultAsync(i => i.DocId == docId);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<OldDocVer> getById(int id)
        {
            var result = await _context.OldDocVers.SingleOrDefaultAsync(i => i.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }
    }
}
