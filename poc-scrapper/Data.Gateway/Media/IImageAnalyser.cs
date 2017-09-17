namespace Scrapper.Data.Gateway.Media
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model.Media;

    public interface IImageAnalyser
    {
        Task<IEnumerable<Media>> AnalyseAsync(IEnumerable<Media> mediaList);
    }
}
