using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using PresentationRobot.Activities.Design.Designers;
using PresentationRobot.Activities.Design.Properties;

namespace PresentationRobot.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(DisplayMessage), categoryAttribute);
            builder.AddCustomAttributes(typeof(DisplayMessage), new DesignerAttribute(typeof(DisplayMessageDesigner)));
            builder.AddCustomAttributes(typeof(DisplayMessage), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
