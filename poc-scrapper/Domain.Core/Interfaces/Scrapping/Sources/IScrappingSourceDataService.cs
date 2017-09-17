
namespace Scrapper.Domain.Core.Interfaces.Scrapping.Sources
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Scrapper.Domain.Model.Scrapping.Sources;

    public interface IScrappingSourceDataService
    {
        Task<ScrappingSource> InsertScrappingSourceAsync(ScrappingSource source);

        Task<IEnumerable<ScrappingSource>> GetAllAsync();

        Task ScheduleAsync(string id);
    }
}
