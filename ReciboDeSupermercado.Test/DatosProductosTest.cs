using System.Collections;

namespace ReciboDeSupermercado.Test;

public class DatosProductosTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new ProductosTestDatos
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m }
                },
                TotalEsperado = 0.99m
            }
        };

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m },
                    new () { Nombre = "Arroz", Precio = 2.49m }
                },
                TotalEsperado = 0.99m + 2.49m
            },
        };

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m },
                    new () { Nombre = "Arroz", Precio = 2.49m },
                    new () {Nombre = "Tubo para pasta de dientes",  Precio = 1.79m }
                },
                TotalEsperado = 0.99m + 2.49m + 1.79m
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}