using System.Threading;
using System.Threading.Tasks;
using LockPoc.Data.Repositories;

namespace LockPoc.Data.Services
{
    public class NumberService: INumberService
    {
        private readonly INumberRepository _numberRepository;

        public NumberService(INumberRepository numberRepository)
        {
            _numberRepository = numberRepository;
        }

        public async Task<ulong> GetNewSaleDocumentNumberAsync(int userId = 0)
            => await _numberRepository.CreateNewNumber(Constants.DocumentTypes.SalesDocument, userId);
        
        public async Task<ulong> GetNewInvoiceNumberAsync(int userId = 0)
            => await _numberRepository.CreateNewNumber(Constants.DocumentTypes.Invoice, userId);
    }
}