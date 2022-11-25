namespace _ApiProject1_.Entities
{
    public class Adress
    {
        public int Id { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
