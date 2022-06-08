using System.Threading.Tasks;

namespace LockPoc.Data.Services
{
    public interface INumberService
    {
        Task<ulong> GetNewSaleDocumentNumberAsync();
    }
}