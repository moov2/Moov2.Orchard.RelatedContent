using Moov2.Orchard.RelatedContent.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace Moov2.Orchard.RelatedContent.Handlers
{
    public class RelatedContentPartHandler : ContentHandler
    {
        public RelatedContentPartHandler(IContentManager contentManager)
        {
            OnLoading<RelatedContentPart>((ctx, part) =>
            {
                PopulateRelatedItems(contentManager, part);
            });
        }

        private static void PopulateRelatedItems(IContentManager contentManager, RelatedContentPart part)
        {
            var ids = part.RelatedContentDtos.Select(x => x.ContentItemId).ToList();
            var items = new List<ContentItem>();
            foreach (var id in ids)
            {
                var item = contentManager.Get(id, VersionOptions.Published);
                if (item != null)
                    items.Add(item);
            }
            part.RelatedItems = items;
        }
    }
}