using FDS.Data;

namespace FDS.Services
{
    public interface IDocTypeRepository
    {
        public Task<List<DocType>> GetAll();
        public Task<DocType> GetById(int id);
        public Task<DocType> Add(DocType type);
        public Task<bool> Update(DocType type, int id);
        public Task<bool> Delete(int id);
        //public Task<List<Document>> Search(string searchString);
        //public Task<List<Document>> GetDocumentsByAuthor(string authorName);
        //public Task<List<Document>> GetDocumentsByDate();
    }
}
