namespace Scrapper.Data.Gateway.Scrapping
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model.Media;
    using Scrapper.Domain.Model.Scrapping.Sources;

    public interface IScrappingSourceProcessor
    {
        Task<IEnumerable<Media>> ProcessAsync(ScrappingSource source);
    }
}
