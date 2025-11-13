using System.Globalization;
using System.Text;

namespace ReciboDeSupermercado.Core;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<Promocion> _promociones = new();
    private decimal _subtotal;
    private decimal _descuentoTotal;

    public Recibo()
    {
        _subtotal = 0m;
        _descuentoTotal = 0m;
    }

    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    public decimal Total
    {
        get
        {
            decimal subtotal = Productos.Sum(p => p.Subtotal);
            decimal descuentos = CalcularDescuentoTotal();
            return _subtotal - _descuentoTotal;
        }
    }
    
    
    public void AgregarProducto(Producto producto)
    {
        
        var productoExistente = _productos.Find(p => p.Nombre == producto.Nombre);

        if (productoExistente != null)
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(producto);
        }
    }

    public void AplicarPromocion(Promocion promocion)
    {
        _promociones.Add(promocion);
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
        var impresionDescuentos = new StringBuilder();

        foreach (var producto in _productos)
        {
            reciboImpreso.AppendLine(producto.ObtenerImpresionParaRecibo());
            
            _subtotal += producto.Subtotal;
            
            foreach (var promocion in _promociones)
            {
                var descuentoAplicado = promocion.CalcularDescuento(producto);
                if(descuentoAplicado>0)
                {
                    impresionDescuentos.AppendLine(promocion.ObtenerImpresionParaRecibo(descuentoAplicado));
                }

                _descuentoTotal += descuentoAplicado;
            }
        }

        reciboImpreso.AppendLine("".PadRight(40, '-'));
        
        reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${_subtotal.ToString("F2", CultureInfo.InvariantCulture)}");

        if (impresionDescuentos.ToString() != "")
        {
            reciboImpreso.AppendLine();
            reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");

            reciboImpreso.Append(impresionDescuentos);
            reciboImpreso.AppendLine("".PadRight(40, '-'));
        }
        

        reciboImpreso.AppendLine("".PadRight(40, '-'));
        reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total.ToString("F2", CultureInfo.InvariantCulture)}");
        
        return reciboImpreso.ToString();
    }

    
}