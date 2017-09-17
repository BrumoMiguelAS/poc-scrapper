namespace Scrapper.Application.Services.Scrapping.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Scrapper.Application.Dto.Scrapping.Sources;
    using Scrapper.Domain.Core.Interfaces.Scrapping.Sources;

    public class ScrappingSourceService : IScrappingSourceService
    {
        private readonly IScrappingSourceDataService scrappingSourceService;

        public ScrappingSourceService(
            IScrappingSourceDataService scrappingSourceService)
        {
            this.scrappingSourceService = scrappingSourceService;
        }

        public async Task<IEnumerable<ScrappingSource>> GetAllAsync()
        {
            var sources = await this.scrappingSourceService.GetAllAsync().ConfigureAwait(false);
            return sources.Select(ScrappingMapper.ScrappingSourceToDto);
        }

        public async Task<ScrappingSource> InsertScrappingSourceAsync(ScrappingSource source)
        {
            if (source == null)
            {
                throw new ArgumentException();
            }

            var sourceToInsert = ScrappingMapper.ScrappingSourceToModel(source);
            var insertedSource = await this.scrappingSourceService.InsertScrappingSourceAsync(sourceToInsert).ConfigureAwait(false);

            return ScrappingMapper.ScrappingSourceToDto(insertedSource);
        }

        public async Task ScheduleAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException();
            }

            await this.scrappingSourceService.ScheduleAsync(id).ConfigureAwait(false);
        }
    }
}
