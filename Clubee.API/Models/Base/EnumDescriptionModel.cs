namespace Clubee.API.Models.Base
{
    public class EnumDescriptionModel
    {
        public readonly int Id;
        public readonly string Description;

        public EnumDescriptionModel(int id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
}
