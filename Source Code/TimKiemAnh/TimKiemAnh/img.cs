using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimKiemAnh
{
 public class img
    {
        string id;
        Vector vector;
        string tenAnh,tenThuMuc;
        double doUuTien;
        int cum;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public Vector Vector
        {
            get
            {
                return vector;
            }

            set
            {
                vector = value;
            }
        }

        public string TenAnh
        {
            get
            {
                return tenAnh;
            }

            set
            {
                tenAnh = value;
            }
        }

        public string TenThuMuc
        {
            get
            {
                return tenThuMuc;
            }

            set
            {
                tenThuMuc = value;
            }
        }

        public double DoUuTien
        {
            get
            {
                return doUuTien;
            }

            set
            {
                doUuTien = value;
            }
        }

        public int Cum
        {
            get
            {
                return cum;
            }

            set
            {
                cum = value;
            }
        }

        public img()
        {
            Vector = new Vector();
            Id = TenAnh = TenThuMuc = String.Empty;
            doUuTien = -1;
            Cum = -1;
        }
    }
}
