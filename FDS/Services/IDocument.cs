using FDS.Data;

namespace FDS.Services
{
    public interface IDocument
    {
        public Task<List<Document>> GetAll();
        public Task<Document> GetById(int id);
        public Task<Document> Add(Document doc, IFormFile file, IFormFile signature);
        public Task<bool> Update(Document doc, int id, IFormFile file, IFormFile signature);
        public Task<bool> Delete(int id);
        public Task<List<Document>> Search(string searchString);
        public Task<List<Document>> GetDocumentsByAuthor(string authorName);
        public Task<List<Document>> GetDocumentsByDate();
        public Task<List<Document>> GetDocumentsByFilghtId(int flightId);

        public Task<Document> DowloadDocumentById(int id);
        public Task<List<Document>> DowloadDocumentByFlightId(int id);
    }
}
