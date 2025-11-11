using System.Text;

namespace ReciboDeSupermercado.Test;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<IPromocion> _promociones = new();
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    public decimal Total
    {
        get
        {
            decimal subtotal = Productos.Sum(p => p.Subtotal);
            decimal descuentos = CalcularDescuentoTotal();
            return subtotal - descuentos;
        }
    }


    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        var productoExistente = _productos.Find(p => p.Nombre == productoDescripcion);

        if (productoExistente != null)
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(new Producto(productoDescripcion, precio));
        }
    }

    public void AplicarPromocion(IPromocion promocion)
    {
        _promociones.Add(promocion);
    }

    public void AplicarDescuentoPorcentual(string nombreProducto, decimal porcentaje)
    {
        AplicarPromocion(new PromocionDescuentoPorcentual(nombreProducto, porcentaje));
    }

    public void PromocionLLeveXPagueX(string nombreProducto, int compra, int lleva)
    {
        AplicarPromocion(new PromocionLLeveXPagueX(nombreProducto, compra, lleva));
    }

    public void AplicarPromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    {
        AplicarPromocion(new PromocionPackPrecioFijo(nombreProducto, cantidad, precioFijo));
    }

    private decimal CalcularDescuentoTotal()
    {
        decimal total = 0m;

        foreach (var producto in _productos)
        {
            foreach (var promocion in _promociones)
            {
                total += promocion.CalcularDescuento(producto);
            }
        }

        return total;
    }

    public string GenerarRecibo()
    {
        var reciboImpreso = new StringBuilder();

        foreach (var producto in _productos)
        {
            reciboImpreso.AppendLine($"{producto.Nombre,-20} x{producto.Cantidad,-5} ${producto.Subtotal}");
        }

        reciboImpreso.AppendLine("".PadRight(40, '-'));
    
        // Subtotal
        decimal subtotal = Productos.Sum(p => p.Subtotal);
        reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${subtotal:F2}");
    
        // Descuentos (si hay)
        decimal descuentoTotal = CalcularDescuentoTotal();
        if (descuentoTotal > 0)
        {
            reciboImpreso.AppendLine();
            reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");
        
            foreach (var promocion in _promociones)
            {
                foreach (var producto in _productos)
                {
                    decimal descuento = promocion.CalcularDescuento(producto);
                    if (descuento > 0)
                    {
                        string descripcion = ObtenerDescripcionPromocion(promocion);
                        reciboImpreso.AppendLine($"  {descripcion,-28} -${descuento:F2}");
                    }
                }
            }
        
            reciboImpreso.AppendLine("".PadRight(40, '-'));
        }


        reciboImpreso.AppendLine("".PadRight(40, '-'));
        reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total:F2}");
        
        return reciboImpreso.ToString();
    }

    private string ObtenerDescripcionPromocion(IPromocion promocion)
    {
        return promocion switch
        {
            PromocionDescuentoPorcentual p => $"Descuento en {p.NombreProducto}",
            PromocionLLeveXPagueX p => $"Lleve X Pague X en {p.NombreProducto}",
            PromocionPackPrecioFijo p => $"Pack en {p.NombreProducto}",
            _ => "Descuento"
        };
    }
}