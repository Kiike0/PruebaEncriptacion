
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Diagnostics;



//crearArchivoMasCorto();

//string ruta = @"C:\Users\enriv\source\repos\PruebaEncriptacion\PruebaEncriptacion\passwordsPrueba.txt"; // Ruta del archivo editado a leer

string ruta = @"C:\Users\enriv\source\repos\PruebaEncriptacion\PruebaEncriptacion\passwords.txt"; // Ruta del archivo completo

// Lee las contraseñas del archivo y las almacena en una lista
List<string> listaPasswords = LeerPasswords(ruta);

// Contraseña aleatoria del archivo
string randomPw = randomPassword(listaPasswords);

Console.WriteLine(randomPw);

// Encripta la contraseña aleatoria
string pwEncriptado = EncriptarPassword(randomPw);

FuerzaBruta(listaPasswords, pwEncriptado, randomPw); // Intenta adivinar la contraseña mediante fuerza bruta con hilos



/**
 * Almacena todas las contraseñas en una lista del archivo proporcionado
 * @param ruta la ruta de la lista de contraseñas
 */

static List<string> LeerPasswords(string ruta)
{
    try
    {
        // Lee todas las líneas del archivo y las devuelve como una lista
        return new List<string>(File.ReadAllLines(ruta));
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al leer el archivo de contraseñas: " + ex.Message);
        return new List<string>();
    }
}


/**
 * Encripta la contraseña que le pasamos como parámetro
 * @param password string de la cadena de la contraseña que le pasamos
 */
static string EncriptarPassword(string password)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder sb = new StringBuilder();

        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}

/**
 * Intenta acceder a las contraseñas por medio de hilos
 * @param listaPasswords la lista de contraseñas
 * @param pwEncriptado la contraseña ya encriptada
 * @param randomPw la contraseña aleatoria para compararla con la encontrada
 */

static void FuerzaBruta(List<string> listaPasswords, string pwEncriptado, string randomPw)
{
    var StartMoment = DateTime.Now; //Para saber cuánto tardamos en acceder a la contraseña
    Console.WriteLine("Iniciando fuerza bruta...");

    const int numeroHilos = 10; //Elegimos el número de hilos que queremos que se usen
    int pwPorHilo = listaPasswords.Count / numeroHilos;

    // Lista para almacenar los hilos
    List<Thread> hilos = new List<Thread>();

    for (int i = 0; i < numeroHilos; i++)
    {
        int inicio = i * pwPorHilo;
        int fin;

        if (i == numeroHilos - 1)
        {
            fin = listaPasswords.Count;
        }
        else
        {
            fin = (i + 1) * pwPorHilo;
        }

        // Crea un hilo para la fuerza bruta en el rango asignado
        Thread hilo = new Thread(() => RealizarFuerzaBruta(listaPasswords, pwEncriptado, inicio, fin, StartMoment, randomPw));
        hilos.Add(hilo);

        // Inicia el hilo
        hilo.Start();
    }

}

/**
 * Realiza por medio de fuerza bruta el acceso a la contraseña
 * @param listaPasswords la lista de contraseñas
 * @param pwEncriptado la contraseña ya encriptada
 * @param inicio indica la posición inicial en la lista de contraseñas desde la cual el hilo actual comenzará a procesar contraseñas
 * @param fin indica la posición final en la lista de contraseñas hasta la cual el hilo actual procesará contraseñas.
 * @param StartMoment la fecha desde que se inicia el método fuerzabruta
 * @param randomPw la contraseña aleatoria para compararla con la encontrada
 */
static void RealizarFuerzaBruta(List<string> listaPasswords, string pwEncriptado, int inicio, int fin, DateTime StartMoment, string randomPw)
{
    for (int i = inicio; i < fin; i++)
    {
        string pwActual = listaPasswords[i];

        // Encripta la contraseña actual para comparar con la contraseña encriptada objetivo
        string pwActualEncriptado = EncriptarPassword(pwActual);

        if (pwEncriptado == pwActualEncriptado)
        {
            Console.WriteLine($"Contraseña encontrada: {pwActual}");
            
            // Comparación adicional
            if (pwActual == randomPw)
            {
                Console.WriteLine("La contraseña encontrada coincide con la contraseña aleatoria original.");
                Console.WriteLine("Hackeo realizado con éxito");
            }
            else
            {
                Console.WriteLine("La contraseña encontrada NO coincide con la contraseña aleatoria original.");
            }

            
            Console.WriteLine("Se ha tardado " + (DateTime.Now - StartMoment));
            return;
        }
    }
}

/**
 * Crea un archivo más corto
 */
static void crearArchivoMasCorto()
{
    String line;
    try
    {
        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader("C:\\Users\\enriv\\source\\repos\\PruebaEncriptacion\\PruebaEncriptacion\\passwords.txt");
        //Pass the filepath and filename to the StreamWriter Constructor
        StreamWriter sw = new StreamWriter("C:\\Users\\enriv\\source\\repos\\PruebaEncriptacion\\PruebaEncriptacion\\passwordsPrueba.txt");

        int lineCount = 0;

        //Continue to read until you reach the 500th line
        while ((line = sr.ReadLine()) != null && lineCount < 2000)
        {
            //write the line to new file
            sw.WriteLine(line);

            //Add count to lineCount
            lineCount++;
        }
        //close the file
        sr.Close();
        sw.Close();
        Console.ReadLine();
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }
    finally
    {
        Console.WriteLine("Executing finally block.");
    }
}


/**
 * Elege una contraseña de forma aleatoria.
 * @param listaPw le pasamos una lista de contraseñas que ya leyó en anterioridad
 */
static string randomPassword(List<string> listaPw)
{
    // Genera un índice aleatorio para seleccionar una contraseña del listado
    Random random = new Random();
    int indiceAleatorio = random.Next(listaPw.Count);

    // Devuelve la contraseña aleatoria seleccionada
    return listaPw[indiceAleatorio];

}




