using System;
using System.Collections.Generic;
using System.Text;

namespace delivery_management_systeem.Models;

public class Product
{
    public int BestellingId { get; set; } = 0;
    public string EAN { get; set; } = string.Empty;

    public int LeverancierId { get; set; }

    public int ProductLocatieId { get; set; }

    public string Naam { get; set; } = string.Empty;

    public string Beschrijving { get; set; } = string.Empty;

    public decimal Prijs { get; set; }

    public double Gewicht { get; set; }

    public string Garantie { get; set; } = string.Empty;

    public int HuidigeVoorraad { get; set; }

    public int MinimumVoorraad { get; set; }

    public string VoorraadStatus { get; set; } = string.Empty;

    public bool IsGescand { get; set; } = false;
    public bool MistNaControle { get; set; } = false;
}
