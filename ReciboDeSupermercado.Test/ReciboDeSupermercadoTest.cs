using FluentAssertions;

namespace ReciboDeSupermercado.Test;

public class ReciboDeSupermercadoTest
{
    [Fact]
    public void DeberiaCrearUnReciboVacioConTotalCero()
    {
        var recibo = new Recibo();
        
        Assert.Equal(0m, recibo.Total);
    }
    

    [Theory]
    [ClassData(typeof(DatosProductosTest))]
    public void Si_AgregarProductosAlReciboElTotal_Debe_SerLaSumaDeTodosLosPrecios(ProductosTestDatos productosTestDatos)
    {
        var recibo = new Recibo();

        foreach (var producto in productosTestDatos.Productos)
        {
            recibo.AgregarProducto(producto.Nombre, producto.Precio);
        }
        
        recibo.Total.Should().Be(productosTestDatos.TotalEsperado);
    }
}

public class Recibo
{
    private decimal _total;
    public decimal Total => _total;

    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        _total += precio;
    }
}