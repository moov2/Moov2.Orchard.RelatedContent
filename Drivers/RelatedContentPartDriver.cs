using Moov2.Orchard.RelatedContent.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.DisplayManagement;
using System.Collections.Generic;
using System.Linq;

namespace Moov2.Orchard.RelatedContent.Drivers
{
    public class RelatedContentPartDriver : ContentPartDriver<RelatedContentPart>
    {
        #region Constants
        private const string TemplateName = "Parts.RelatedContent.Edit";

        private const string DefaultDisplayType = "Summary";
        #endregion

        #region Dependencies
        private readonly IContentManager _contentManager;
        #endregion

        #region Constructor
        public RelatedContentPartDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        #endregion

        #region Driver Methods
        #region Properties
        protected override string Prefix => "RelatedContent";
        #endregion

        #region Display
        protected override DriverResult Display(RelatedContentPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_RelatedContent_List", () => RenderShape(part, shapeHelper));
        }
        #endregion

        #region Editor
        protected override DriverResult Editor(RelatedContentPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_RelatedContent_Edit", () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(RelatedContentPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
        #endregion
        #endregion

        #region Helpers
        private dynamic RenderShape(RelatedContentPart part, dynamic shapeHelper)
        {
            var items = new List<ContentItem>();
            foreach (var dto in part.RelatedContentDtos)
            {
                var item = part.RelatedItems.FirstOrDefault(x => x.Id == dto.ContentItemId);
                if (item != null)
                    items.Add(item);
            }
            dynamic list = null;
            var itemShapes = items.Select(x => _contentManager.BuildDisplay(x, DefaultDisplayType));
            if (!string.IsNullOrWhiteSpace(part.CollectionDisplayShape))
            {
                list = ((IShapeFactory)shapeHelper).Create(part.CollectionDisplayShape, Arguments.From(new Dictionary<string, object> { { "Items", itemShapes } }));
            }
            else
            {
                list = shapeHelper.List(Items: itemShapes);
            }
            return shapeHelper.Parts_RelatedContent_List(
                List: list,
                Count: items.Count
            );
        }
        #endregion
    }
}