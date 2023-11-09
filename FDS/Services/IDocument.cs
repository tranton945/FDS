using FDS.Data;

namespace FDS.Services
{
    public interface IDocument
    {
        public Task<List<Document>> GetAll();
        public Task<Document> GetById(int id);
        public Task<Document> Add(Document doc);
        public Task<bool> Update(Document doc, int id);
        public Task<bool> Delete(int id);
        public Task<List<Document>> Search(string searchString);
        public Task<List<Document>> GetDocumentsByAuthor(string authorName);
        public Task<List<Document>> GetDocumentsByDate();
    }
}
