using Moov2.Orchard.RelatedContent.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Core.Containers.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Moov2.Orchard.RelatedContent.Settings
{
    public class RelatedContentSettingsEvents : ContentDefinitionEditorEventsBase
    {
        #region Dependencies
        private readonly IContentDefinitionManager _contentDefinitionManager;
        #endregion

        #region Constructor
        public RelatedContentSettingsEvents(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }
        #endregion

        #region Events Overrides
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "RelatedContentPart")
                yield break;

            var model = definition.Settings.GetModel<RelatedContentTypeSettings>();

            var viewModel = new RelatedContentTypePartSettingsViewModel
            {
                CollectionDisplayShape = model.CollectionDisplayShape,
                RestrictedItemContentTypes = _contentDefinitionManager.ParseContentTypeDefinitions(model.RestrictedItemContentTypes).Select(x => x.Name).ToList(),
                RestrictItemContentTypes = model.RestrictItemContentTypes,
                AvailableItemContentTypes = GetRelatableTypes()
            };

            yield return DefinitionTemplate(viewModel);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "RelatedContentPart")
                yield break;

            var viewModel = new RelatedContentTypePartSettingsViewModel
            {
                AvailableItemContentTypes = _contentDefinitionManager.ListTypeDefinitions().ToList()
            };
            updateModel.TryUpdateModel(viewModel, "RelatedContentTypePartSettingsViewModel", null, new[] { "AvailableItemContentTypes" });
            builder.WithSetting("RelatedContentTypeSettings.CollectionDisplayShape", viewModel.CollectionDisplayShape);
            builder.WithSetting("RelatedContentTypeSettings.RestrictedItemContentTypes", viewModel.RestrictedItemContentTypes != null ? string.Join(",", viewModel.RestrictedItemContentTypes) : "");
            builder.WithSetting("RelatedContentTypeSettings.RestrictItemContentTypes", viewModel.RestrictItemContentTypes.ToString());
            yield return DefinitionTemplate(viewModel);
        }
        #endregion

        #region Helpers
        private IList<ContentTypeDefinition> GetRelatableTypes()
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(x => !x.Settings.ContainsKey("Stereotype") || x.Settings["Stereotype"] == null).ToList();
        }
        #endregion
    }
}