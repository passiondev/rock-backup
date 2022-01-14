
using Rock.Data;

namespace Rock.Model
{
    public partial class SignatureDocument
    {
        internal class SaveHook: EntitySaveHook<SignatureDocument>
        {
            protected override void PreSave()
            {
                base.PreSave();
            }

            protected override void PostSave()
            {
                base.PostSave();
            }
        }
    }
}
