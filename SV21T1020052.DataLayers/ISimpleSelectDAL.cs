using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020052.DataLayers
{
    public interface ISimpleSelectDAL<T> where T : class
    {
        /// <summary>
        /// select toàn bộ dữ liệu trong bảng
        /// </summary>
        /// <returns></returns>
        List<T> List();
    }
}
