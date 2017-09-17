namespace Scrapper.Domain.Model.Media
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public class Media
    {
        public Media()
        {
            this.Properties = new List<MediaProperties>();
        }

        public ObjectId Id { get; set; }

        public string OriginalUrl { get; set; }

        public string PhysicalPath { get; set; }

        public string Name { get; set; }

        public MediaStatus Status { get; set; }

        public MediaType Type { get; set; }

        public List<MediaProperties> Properties { get; set; }
    }
}
