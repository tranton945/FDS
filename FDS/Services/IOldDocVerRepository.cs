using FDS.Data;

namespace FDS.Services
{
    public interface IOldDocVerRepository
    {
        public Task<OldDocVer> getByDocId(int docId);
        public Task<List<OldDocVer>> getAllByDocId(int docId);
        public Task<OldDocVer> getById(int id);
        public Task<List<OldDocVer>> getAll();
        public Task<bool> Delete(int id);
        public Task<bool> DeleteAllByDocId(int docId);
    }
}
