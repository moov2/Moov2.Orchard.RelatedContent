using Moov2.Orchard.RelatedContent.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Moov2.Orchard.RelatedContent.Drivers
{
    public class RelatedContentPartDriver : ContentPartDriver<RelatedContentPart>
    {
        #region Constants
        private const string TemplateName = "Parts.RelatedContent.Edit";
        #endregion

        #region Driver Methods

        #region Properties
        protected override string Prefix => "RelatedContent";
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
    }
}