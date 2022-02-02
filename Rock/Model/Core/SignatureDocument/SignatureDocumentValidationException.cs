using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Model
{
    internal class SignatureDocumentValidationException : ValidationException
    {
        public SignatureDocumentValidationException( string errorMessage ) : base( errorMessage )
        {
        }
    }
}
