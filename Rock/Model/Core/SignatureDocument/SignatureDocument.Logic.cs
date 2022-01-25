using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Model
{
    public partial class SignatureDocument
    {
        public string CalculateSignatureVerificationHash()
        {
            /*
        /// The computed SHA1 hash for the SignedDocumentText, SignedClientIP address, SignedClientUserAgent, SignedDateTime, SignedByPersonAliasId, SignatureData, and SignedName.
        /// This hash can be used to prove the authenticity of the unaltered signature document.
        /// This is only calculated once during the pre-save event when the SignedDateTime was originally null/empty but now has a value.
            */

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
    }
}
