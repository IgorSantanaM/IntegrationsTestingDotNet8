namespace RentalMotorcycle.Domain.Core.Models
{
    public abstract record Event<TId> : Message<TId> where TId : notnull
    {
        public DateTime TimeStamp { get; set; }

        protected Event(TId aggregateId) : base(aggregateId)
        {
            TimeStamp = DateTime.UtcNow;
        }
    }
}
