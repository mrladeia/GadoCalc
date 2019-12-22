using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GadoCalc.Models
{
    public class Parameters
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Frete { get; set; }
        public double Comissao { get; set; }
    }
}
