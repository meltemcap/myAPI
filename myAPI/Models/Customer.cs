namespace myAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CustomerStatus Status { get; set; }

        public enum CustomerStatus
        {
            Active ,
            Inactive 
        }
    }
}
