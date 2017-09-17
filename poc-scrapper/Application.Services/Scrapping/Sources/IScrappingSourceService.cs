namespace Scrapper.Application.Services.Scrapping.Sources
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Scrapper.Application.Dto.Scrapping.Sources;

    public interface IScrappingSourceService
    {
        Task<ScrappingSource> InsertScrappingSourceAsync(ScrappingSource source);

        Task<IEnumerable<ScrappingSource>> GetAllAsync();

        Task ScheduleAsync(string id);
    }
}
