using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020052.DataLayers
{
    public interface ISimpleQueryDAL<T> where T : class
    {
        /// <summary>
        /// Lấy toàn bộ dữ liệu của 1 bảng nào đó
        /// </summary>
        /// <returns></returns>
        List<T> List();
    }
}
