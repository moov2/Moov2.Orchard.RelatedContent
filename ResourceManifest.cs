using Orchard.UI.Resources;

namespace Moov2.Orchard.RelatedContent
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add()
                .DefineScript("RelatedContentAdmin").SetUrl("relatedcontent.admin.min.js", "relatedcontent.admin.js").SetDependencies("JQuery", "ContentPIcker");

            builder.Add()
                .DefineStyle("RelatedContentAdmin").SetUrl("relatedcontent.admin.min.css", "relatedcontent.admin.css");
        }
    }
}