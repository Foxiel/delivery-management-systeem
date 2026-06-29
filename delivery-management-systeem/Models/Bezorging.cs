using System;
using System.Collections.Generic;
using System.Text;

namespace delivery_management_systeem.Models
{
    public class Bezorging
    {
       public string Code { get; set; } = string.Empty;
       public bool IsGescand { get; set; } = false;
       public bool MistNaControle { get; set; } = false;
    }
}
