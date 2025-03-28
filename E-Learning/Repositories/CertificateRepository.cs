using E_Learning.Data;
using E_Learning.Entity;

namespace E_Learning.Repositories
{
    public class CertificateRepository
    {

        private readonly ELearningDbContext _context;

        public CertificateRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public void Add(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            _context.SaveChanges();
        }

    }
}
