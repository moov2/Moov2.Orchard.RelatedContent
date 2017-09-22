using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Moov2.Orchard.RelatedContent
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("RelatedContentPart", builder => builder
                .Attachable()
                .WithDescription("Adds a manageable related content items list to a content type."));

            return 1;
        }
    }
}