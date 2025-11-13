namespace ReciboDeSupermercado.Core;

public abstract class Promocion
{
    public abstract string NombreProducto { get; }
    public abstract decimal CalcularDescuento(Producto producto);
    public abstract string ObtenerDescripcion();
    public abstract string ObtenerImpresionParaRecibo(decimal descuentoAplicado);
}