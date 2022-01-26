using System.ComponentModel.DataAnnotations.Schema;

using Rock.Data;
using Rock.Lava;

namespace Rock.Model
{
    public partial class SignatureDocument
    {
        /// <summary>
        /// Calculates the signature verification hash.
        /// </summary>
        /// <returns>System.String.</returns>
        public string CalculateSignatureVerificationHash()
        {
            var hashObject = new
            {
                SignedDocumentText = this.SignedDocumentText,
                SignedClientIp = this.SignedClientIp,
                SignedClientUserAgent = this.SignedClientUserAgent,
                SignedDateTime = this.SignedDateTime,
                SignedByPersonAliasId = this.SignedByPersonAliasId,
                SignatureData = this.SignatureData,
                SignedName = this.SignedName
            };

            var hashObjectJson = hashObject.ToJson();

            var hashResult = Rock.Security.Encryption.GetSHA1Hash( hashObjectJson );

            return hashResult;
        }

        /// <summary>
        /// The data that was collected during a drawn signature type.
        /// This is an img data url. Example:
        /// <code>data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAngAAABkCAYAAAAVH...</code>
        /// This is stored as <seealso cref="SignatureDataEncrypted"/>.
        /// </summary>
        /// <value>The signature data.</value>
        [NotMapped]
        [HideFromReporting]
        [LavaHidden]
        public virtual string SignatureData
        {
            /*
            1/25/2022 MDP

            SignatureData is then base64 DataURL of the drawn signature. For example, the 'src' value of

            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAngAAABkCAYAAAAVH...' />

            So, 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAngAAABkCAYAAAAVH...' is what SignatureData would be.

            Note that this is PII Data (https://en.wikipedia.org/wiki/Personal_data), so it needs to be stored
            in the database encrypted in the SignatureDataEncrypted field.
            */

            get
            {
                return Rock.Security.Encryption.DecryptString( this.SignatureDataEncrypted );
            }

            set
            {
                this.SignatureDataEncrypted = Rock.Security.Encryption.EncryptString( this.SignatureData );
            }
        }
    }
}
