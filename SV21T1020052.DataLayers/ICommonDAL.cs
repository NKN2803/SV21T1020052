﻿namespace SV21T1020052.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu chung
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T> where T : class 
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu dưới dạng phân trang (pagination)
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng dữ liệu hiển thị trên mỗi trang (bằng 0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị cần tìm (chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        List<T> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Đếm số lượng dòng dữ liệu đếm được
        /// </summary> 
        /// <param name="seachValue">Giá trị cần tìm (chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Bổ sung dữ liệu vào CSDL. Hàm trả về ID của dữ liệu được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// Xóa dữ liệu dựa vào mã (ID)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// Lấy một dòng dữ liệu dựa vào id (trả về null nếu dữ liệu không tồn tại)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// Kiểm tra một dòng dữ liệu có khóa là ID hiện có dữ liệu liên quan ở bảng nào khác hay không? 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
