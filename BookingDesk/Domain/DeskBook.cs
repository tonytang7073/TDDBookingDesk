namespace BookingDesk.Domain
{
    public class DeskBook : BookingBase
    {
        public Guid Id { get; set; }
        public Guid DeskId { get; set; }

        public virtual Desk Desk {get; set;}
    }
}
