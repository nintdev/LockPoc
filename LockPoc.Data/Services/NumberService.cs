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

        public async Task<ulong> GetNewSaleDocumentNumberAsync()
            => await _numberRepository.GetNewNumber(Constants.SalesDocument);
        
        public async Task<ulong> GetNewInvoiceNumberAsync()
            => await _numberRepository.GetNewNumber(Constants.Invoice);
    }
}