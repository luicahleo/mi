using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIDAS.Classes
{
    public class ItemDesplegable
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _nombre;

        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        
        
    }
}