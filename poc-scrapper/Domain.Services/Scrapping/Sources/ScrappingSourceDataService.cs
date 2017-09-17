namespace Scrapper.Domain.Services.Scrapping.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Hangfire;
    using MongoDB.Bson;
    using Scrapper.Data.Gateway.Scrapping;
    using Scrapper.Data.Repository.Interfaces;
    using Scrapper.Domain.Core.Interfaces.Scrapping.Sources;
    using Scrapper.Domain.Model.Scrapping.Sources;

    public class ScrappingSourceDataService : IScrappingSourceDataService
    {
        private readonly IScrappingSourceRepository scrappingRepository;

        public ScrappingSourceDataService(
            IScrappingSourceRepository scrappingRepository)
        {
            this.scrappingRepository = scrappingRepository;
        }

        public async Task<IEnumerable<ScrappingSource>> GetAllAsync()
        {
            var exisitingSources = await this.scrappingRepository.FindAllAsync().ConfigureAwait(false);
            if (exisitingSources == null)
            {
                return Enumerable.Empty<ScrappingSource>();
            }

            return exisitingSources;
        }

        public async Task<ScrappingSource> InsertScrappingSourceAsync(ScrappingSource source)
        {
            if(source == null)
            {
                throw new ArgumentException();
            }

            var exisitingSource = await this.scrappingRepository.FindAsync(s => s.Url == source.Url).ConfigureAwait(false);
            if (exisitingSource != null)
            {
                throw new ArgumentException("Another source with the same url already exists!");
            }

            return await this.scrappingRepository.AddAsync(source).ConfigureAwait(false);
        }

        public async Task ScheduleAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid scrapping source id!");
            }

            var source = await this.scrappingRepository.FindAsync(s => s.Id == ObjectId.Parse(id)).ConfigureAwait(false);
            if (source == null)
            {
                throw new ArgumentException("Scrapping source for the given id was not found!");
            }

            BackgroundJob.Enqueue<IScrappingSourceProcessor>(t => t.ProcessAsync(source));
        }
    }
}
