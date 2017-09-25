namespace Moov2.Orchard.RelatedContent.Settings
{
    public class RelatedContentTypeSettings
    {
        public string RestrictedItemContentTypes { get; set; }
        public bool RestrictItemContentTypes { get; set; }

        public RelatedContentTypeSettings()
        {
            RestrictItemContentTypes = false;
        }
    }
}