using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaEncriptacion
{
    public static class Metodos
    {
        // Devuelve si un valor tipo int dado está entre dos números concretos
        public static bool inRange(this int valor, int valMin, int valMax)
        {
            return valor >= valMin && valor <= valMax;
        }
    }
}
