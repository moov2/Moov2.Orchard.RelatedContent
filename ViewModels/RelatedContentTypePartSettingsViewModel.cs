using Orchard.ContentManagement.MetaData.Models;
using System.Collections.Generic;

namespace Moov2.Orchard.RelatedContent.ViewModels
{
    public class RelatedContentTypePartSettingsViewModel
    {
        public IList<ContentTypeDefinition> AvailableItemContentTypes { get; set; }
        public bool RestrictItemContentTypes { get; set; }
        public IList<string> RestrictedItemContentTypes { get; set; }
    }
}