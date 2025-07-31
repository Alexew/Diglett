namespace Diglett.Core.Web.Modelling
{
    public abstract class ModelBase { }

    public abstract class EntityModelBase : ModelBase
    {
        public virtual int Id { get; set; }
    }
}
