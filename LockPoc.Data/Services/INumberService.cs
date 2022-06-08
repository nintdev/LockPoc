using System.Threading;
using System.Threading.Tasks;

namespace LockPoc.Data.Services
{
    public interface INumberService
    {
        Task<ulong> GetNewSaleDocumentNumberAsync(int userId = 0);

        Task<ulong> GetNewInvoiceNumberAsync(int userId = 0);
    }
}