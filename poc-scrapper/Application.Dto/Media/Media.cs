namespace Scrapper.Application.Dto.Media
{
    using System.Collections.Generic;

    public class Media
    {
        public Media()
        {
            this.Properties = new List<MediaProperties>();
        }

        public string Id { get; set; }

        public string OriginalUrl { get; set; }

        public string PhysicalPath { get; set; }

        public string Name { get; set; }

        public MediaStatus Status { get; set; }

        public MediaType Type { get; set; }

        public List<MediaProperties> Properties { get; set; }
    }
}
