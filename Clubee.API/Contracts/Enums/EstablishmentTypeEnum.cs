using System.ComponentModel;

namespace Clubee.API.Contracts.Enums
{
    public enum EstablishmentTypeEnum
    {
        [Description("Bar")]
        Bar = 1,

        [Description("Pub")]
        Pub,

        [Description("Balada")]
        Balada,

        [Description("Casa de shows")]
        CasaShows
    }
}
