using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimKiemAnh
{
    public class Kmeans
    {
        //danh sách ảnh
        public List<img> listItem = new List<img>();
        //danh sách cụm
        public List<img> listCum = new List<img>();
        //2 mảng kiểm tra thuật toán dừng hay chưa
        public int[] danhDau1;
        public int[] danhDau2;
        //độ bình phương sai
        public double SSE { get; set; }
        //hàm đọc file txt
        public void input(string fileName)
        {
            listItem.Clear();
            var strs = File.ReadAllLines(fileName);
            foreach (var str in strs)
            {
                //tạo 1 đối tượng img
                img anh = new img();
                Vector vt = new Vector();
                //cắt dòng đọc được trong file qua kí tự khoảng trắng,(,)
                var chuoi = str.Trim().Split(' ', '(', ')');
                anh.Id = chuoi[0];

                for (int i = 1; i < chuoi.Length - 2; i++)
                {
                    //lưu tên ảnh
                    if (chuoi[i].Contains(".jpg"))
                        anh.TenAnh = chuoi[i];
                    else
                        try
                        {
                            //lưu giá trị của 1 vector n chiều
                            vt.item.Add(Double.Parse(chuoi[i]));
                        }
                        catch (Exception ex)
                        {
                            //nếu không chuyển được qua số thì tiếp tục vòng lặp
                            continue;
                        }
                }               
                anh.Vector = vt;
                //tại vị trí chuoi.Length-1 là khoảng trắng nên chuoi.Length-2 sẽ là tên thư mục.
                anh.TenThuMuc = chuoi[chuoi.Length - 2];
                listItem.Add(anh);
            }
        }
        //khởi tạo 1 vector giá trị bằng 0 để tính tổng không bị lỗi
        public void khoiTaoVector(Vector newVT)
        {
            for (int i = 0; i < listItem[0].Vector.item.Count; i++)
            {
                newVT.item.Add(0);
            }
        }
        public void kmean()
        {

            listCum.Clear();
            Random r = new Random();
            //random các tâm cụm ví dụ trong file có 4 loại hình: ngựa , khủng long,hoa, voi
            //thì sẽ random 4 tâm thuộc 4 loại hình đó
            for (int i = 0; i < 4; i++)
            {
                //nếu trùng thì random lại ví dụ trong listCum đã có loại hình ngựa rồi
                //mà biến anh lại thuộc loại hình ngựa thì chạy vòng while đến khi nào ra loại hình ko có trong listCum
                img anh = listItem[r.Next(0, listItem.Count - 1)];
                while (listCum.Count(a => a.TenThuMuc.Equals(anh.TenThuMuc)) == 1)
                    anh = listItem[r.Next(0, listItem.Count - 1)];
                listCum.Add(anh);
            }
            //khởi tạo        
            danhDau1 = new int[listItem.Count];
            danhDau2 = new int[listItem.Count];
            for (int i = 0; i < listItem.Count; i++)
            {
                danhDau1[i] = -1;
                danhDau2[i] = -1;
            }
            while (true)
            {
                SSE = 0;
                for (int i = 0; i < listItem.Count; i++)
                {
                    int vt = 0;
                    //tính khoảng cách từ 1 item đến cụm thứ 0 (biến vt=0)
                    double min = listItem[i].Vector.tinhKC(listCum[vt].Vector);
                    for (int j = 1; j < listCum.Count; j++)
                    {
                        //tính và so sánh khoảng cách 1 item đến từng cụm nào nhỏ lấy.
                        double kc = listItem[i].Vector.tinhKC(listCum[j].Vector);
                        if (min > kc)
                        {
                            vt = j;
                            min = kc;
                        }
                    }
                    listItem[i].DoUuTien = min;
                    listItem[i].Cum = vt;
                    danhDau1[i] = vt;
                    //công thức
                    SSE += Math.Pow(min, 2);
                }
                bool check = true;
                for (int i = 0; i < danhDau1.Length; i++)
                    //nếu có khác là tiếp tục thuật toán (check=false)
                    if (danhDau1[i] != danhDau2[i])
                    {
                        danhDau2 = danhDau1;
                        check = false;
                        break;
                    }
                //nếu check=true là dừng thuật toán.
                if (check)
                    break;
                //tạo tâm mới
                for (int i = 0; i < listCum.Count; i++)
                {
                    int dem = 0;
                    Vector newVt = new Vector();                   
                    khoiTaoVector(newVt);
                    for (int j = 0; j < listItem.Count; j++)
                        if (danhDau1[j] == i)
                        {
                            dem++;
                            //tính tổng các item thuộc của từng cụm
                            newVt.tinhTong(listItem[j].Vector);
                        }
                    //sau khi có tổng chia cho biến đếm ra tâm mới
                    newVt.trungBinh(dem);
                    listCum[i].Vector = newVt;
                }
            }
        }
        //hàm ghi file
        public void ghiFile()
        {
            //sắp xếp theo độ ưu tiên nào nhỏ nhất thì càng gần tâm ( chính xác càng cao)
            listItem = listItem.OrderBy(a => a.DoUuTien).ToList();
            for (int i = 0; i < listCum.Count; i++)
            {
                //ghi ra từng file theo từng cụm tên file là tên thư mục (listCum[i].TenThuMuc)
                string s = String.Empty;
                for (int j = 0; j < listItem.Count; j++)
                    //coi phần tử nào thuộc cụm i thì ghi ra file
                    if (listItem[j].Cum == i)
                    {
                        s += listItem[j].Id + " (";
                        for (int k = 0; k < listItem[j].Vector.item.Count; k++)
                        {
                            s += listItem[j].Vector.item[k] + " ";
                        }
                        s = s.Substring(0, s.Length - 1);
                        s += ") (" + listItem[j].TenAnh + ") (" + listItem[j].TenThuMuc + ")\n";
                    }

                s = s.Substring(0, s.Length - 1);
                //tên file
                File.WriteAllText("../../" + listCum[i].TenThuMuc + ".txt", s);
            }
        }
        //hàm tìm kiếm ảnh tương tự
        public void search(string fileName, ListView lst)
        {
            //tìm ảnh có fileName truyền vào từ danh sách listItem
            img anh = listItem.Find(a => a.TenAnh.Equals(fileName));
            int vt = 0;
            //tính khoảng cách từ ảnh đó đến từng cụm coi gần cụm nào nhất (khoảng cách min)
            double min = anh.Vector.tinhKC(listCum[vt].Vector);
            for (int j = 1; j < listCum.Count; j++)
            {
                double kc = anh.Vector.tinhKC(listCum[j].Vector);
                if (min > kc)
                {
                    vt = j;
                    min = kc;
                }
            }
            //khai báo danh sách những ảnh tương tự
            List<img> listSearch = new List<img>();
            //vào file có tên listCum[vt].TenThuMuc đọc ra
            var strs = File.ReadAllLines("../../" + listCum[vt].TenThuMuc + ".txt");

            //phần này giống như hàm đọc file trên (input())
            foreach (var str in strs)
            {
                anh = new img();
                Vector vector = new Vector();
                var chuoi = str.Trim().Split(' ', '(', ')');
                anh.Id = chuoi[0];
                for (int i = 1; i < chuoi.Length - 2; i++)
                {
                    if (chuoi[i].Contains(".jpg"))
                        anh.TenAnh = chuoi[i];
                    else
                        try
                        {
                            vector.item.Add(Double.Parse(chuoi[i]));
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                }
                anh.Vector = vector;
                anh.TenThuMuc = chuoi[chuoi.Length - 2];
                listSearch.Add(anh);
            }
            lst.Items.Clear();
            lst.View = View.SmallIcon;
            //tạo danh sách ảnh
            ImageList listImg = new ImageList() { ImageSize = new Size(130, 80) };
            //gán cho listView(lst)
            lst.SmallImageList = listImg;
            int k = 0;
            foreach (img image in listSearch)
            {
                try
                {
                    listImg.Images.Add(Image.FromFile("../../Du lieu Hinh/" + image.TenThuMuc + "/" + image.TenAnh));
                }
                catch (Exception ex)
                {
                    continue;
                }
                ListViewItem item = new ListViewItem() { Text = image.TenAnh };
                item.ImageIndex = k++;
                lst.Items.Add(item);
            }
        }
    }
}
