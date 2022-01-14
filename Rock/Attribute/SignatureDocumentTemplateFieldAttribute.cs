using Rock.Field.Types;
using Rock.Model;

namespace Rock.Attribute
{
    /// <summary>
    /// Field Attribute to select a <see cref="SignatureDocumentTemplate" />. Stored as the SignatureDocumentTemplate's Guid.
    /// </summary>
    public class SignatureDocumentTemplateFieldAttribute : FieldAttribute
    {
        private const string SHOW_TEMPLATES_WITH_EXTERNAL_PROVIDERS = "SHOW_TEMPLATES_WITH_EXTERNAL_PROVIDERS";


        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureDocumentTemplateFieldAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public SignatureDocumentTemplateFieldAttribute( string name ) : base( name )
        {
            FieldTypeClass = typeof( SignatureDocumentTemplateFieldType ).FullName;
        }

        /// <summary>
        /// This should normally be false. If set to true, templates that have external providers will be shown instead of
        /// ones that use Rock eSignatures.
        /// </summary>
        /// <value><c>true</c> if [show template that have external providers]; otherwise, <c>false</c>.</value>
        public bool ShowTemplatesThatHaveExternalProviders
        {
            get
            {
                return FieldConfigurationValues.GetValueOrNull( SHOW_TEMPLATES_WITH_EXTERNAL_PROVIDERS ).AsBoolean();
            }

            set
            {
                FieldConfigurationValues.AddOrReplace( SHOW_TEMPLATES_WITH_EXTERNAL_PROVIDERS, new Field.ConfigurationValue( value.ToString() ) );
            }
        }
    }
}
