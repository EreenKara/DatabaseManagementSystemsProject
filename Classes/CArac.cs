using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriTabaniUygulamasi
{
    internal class CArac
    {
        public int aracNo { get; set; }
        private int _plaka;

        public int Plaka
        {
            get { return _plaka; }
            set { _plaka = value; }
        }

        public string model { get; set; }
        public int aracKM { get; set; }
        public int musteriNo { get; set; }


    }
}
