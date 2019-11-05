using System.ComponentModel;

namespace Clubee.API.Contracts.Enums
{
    public enum GenreEnum
    {
        [Description("Rock")]
        Rock = 1,
        
        [Description("Sertanejo")]
        Sertanejo,

        [Description("Eletrônico")]
        Eletronico,

        [Description("Funk")]
        Funk,

        [Description("Samba")]
        Samba,

        [Description("Pagode")]
        Pagode,

        [Description("Alternativo")]
        Alternativo
    }
}
