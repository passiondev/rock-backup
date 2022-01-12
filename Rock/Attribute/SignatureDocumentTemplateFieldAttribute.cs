using Rock.Field.Types;
using Rock.Model;

namespace Rock.Attribute
{
    /// <summary>
    /// Field Attribute to select a <see cref="SignatureDocumentTemplate" />. Stored as the SignatureDocumentTemplate's Guid.
    /// </summary>
    public class SignatureDocumentTemplateFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureDocumentTemplateFieldAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public SignatureDocumentTemplateFieldAttribute( string name ) : base( name )
        {
            FieldTypeClass = typeof( SignatureDocumentTemplateFieldType ).FullName;
        }
    }
}
