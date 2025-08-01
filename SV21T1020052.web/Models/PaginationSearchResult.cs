namespace SV21T1020052.Web.Models
{
    public abstract class PaginationSearchResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        /// <summary>
        /// Cho biết tổng số dòng dữ liệu mà chúng ta truy vấn được
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Số trang mà chúng ta tính toán được
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;

                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    c += 1;

                return c;
            }
        }
    }
}
