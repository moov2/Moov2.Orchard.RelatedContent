using Moov2.Orchard.RelatedContent.Dtos;
using Moov2.Orchard.RelatedContent.Settings;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moov2.Orchard.RelatedContent.Models
{
    public class RelatedContentPart : ContentPart
    {
        public string RelatedContentJson { get { return this.Retrieve(x => x.RelatedContentJson); } set { this.Store(x => x.RelatedContentJson, value); } }

        public IList<RelatedContentDto> RelatedContentDtos
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<IList<RelatedContentDto>>(RelatedContentJson);
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is JsonException)
                {
                    return new List<RelatedContentDto>();
                }
            }
            set
            {
                try
                {
                    RelatedContentJson = JsonConvert.SerializeObject(value);
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is JsonException)
                {
                    RelatedContentJson = null;
                }
            }
        }

        public IList<ContentItem> RelatedItems { get; set; }
        public string RelatedContentTypes
        {
            get
            {
                if (Settings.GetModel<RelatedContentTypeSettings>()?.RestrictItemContentTypes ?? false)
                    return Settings.GetModel<RelatedContentTypeSettings>().RestrictedItemContentTypes;
                return null;
            }
        }

        public ContentItem GetItemForDto(RelatedContentDto dto)
        {
            return RelatedItems?.FirstOrDefault(x => x.Id == dto.ContentItemId);
        }
    }
}