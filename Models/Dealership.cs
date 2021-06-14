using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixAnalyzer.Models
{
    public class Dealership
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int DealershipID {get;set;}

        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName ="nvarchar(100)")]
        public string Name {get;set;}
    }
}