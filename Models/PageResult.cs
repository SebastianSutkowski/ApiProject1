namespace _ApiProject1_.Models
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }
        public PageResult(List<T> items, int pageSize, int pageNumber,int total)
        {
            Items = items;
            TotalItemsCount=total;
            ItemFrom = pageSize*(pageNumber-1)+1;
            ItemsTo = ItemFrom + pageSize-1;
            TotalPages = (int)Math.Ceiling(TotalItemsCount / (double)pageSize);
            
        }
    }
}
