namespace SV21T1020052.Web.Models
{
    /// <summary>
    /// Lưu giữ các thông tin đầu vào để sử dụng cho chức năng tìm kiếm và hiển thị dữ liệu đưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số dòng hiển thị mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Chuỗi chứa giá trị cần tìm kiếm
        /// </summary>
        public string SearchValue { get; set; } = "";
    }
}
