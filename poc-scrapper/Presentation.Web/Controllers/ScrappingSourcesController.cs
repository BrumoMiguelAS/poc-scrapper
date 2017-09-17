namespace Scrapper.Presentation.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Scrapper.Application.Dto.Scrapping.Sources;
    using Scrapper.Application.Services.Scrapping.Sources;
    using Scrapper.Presentation.Web.Extensions;
    using Scrapper.Presentation.Web.Models;

    [Route("scrapping/sources")]
    public class ScrappingSourcesController : Controller
    {
        private readonly IScrappingSourceService scrappingSourceService;

        public ScrappingSourcesController(
            IScrappingSourceService scrappingSourceService)
        {
            this.scrappingSourceService = scrappingSourceService;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddScrappingSourceRequest request)
        {
            try
            {
                if (!request.Url.IsValidUrl())
                {
                    return this.BadRequest("Invalid Url format!");
                }

                var source = new ScrappingSource()
                {
                    Name = request.Name,
                    Url = request.Url,
                };

                return this.Ok(await this.scrappingSourceService.InsertScrappingSourceAsync(source).ConfigureAwait(false));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return this.Ok(await this.scrappingSourceService.GetAllAsync().ConfigureAwait(false));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [Route("{id}/schedule")]
        [HttpPost]
        public async Task<IActionResult> Schedule(string id, [FromBody]ScheduleScrappingSourceRequest request)
        {
            try
            {
                await this.scrappingSourceService.ScheduleAsync(id).ConfigureAwait(false);
                return this.Ok();

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
