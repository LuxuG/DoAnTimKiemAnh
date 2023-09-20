using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimKiemAnh
{
    public class Vector
    {       
        public List<double> item = new List<double>();               
        //hàm tính khoảng cách giữa 2 vector
        public double tinhKC(Vector input)
        {
            double sum = 0;
            for (int i = 0; i < item.Count; i++)
                sum += (item[i] - input.item[i]) * (item[i] - input.item[i]);
            return Math.Sqrt(sum);
        }
        //hàm tính tổng giữa 2 vector
        public void tinhTong(Vector input)
        {
            for (int i = 0; i < input.item.Count; i++)
            {
                item[i] += input.item[i];
            }            
        }
        //hàm tính trung bình
        public void trungBinh(int dem)
        {
            for (int i = 0; i < item.Count; i++)
                item[i] /= dem;
        }
    }
}
