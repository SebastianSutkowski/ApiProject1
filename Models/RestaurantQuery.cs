namespace _ApiProject1_.Models
{
    public class RestaurantQuery
    {

        public string SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrberBy { get; set; }
        public Direction SortDirection { get; set; }
    }
}
