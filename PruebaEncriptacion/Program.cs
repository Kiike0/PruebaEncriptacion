
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Diagnostics;


//leerArchivo();

//crearArchivoMasCorto();

string ruta = @"C:\Users\enriv\source\repos\PruebaEncriptacion\PruebaEncriptacion\passwordsPrueba.txt"; // Ruta del archivo a leer

// Lee las contraseñas del archivo y las almacena en una lista
List<string> listaPasswords = LeerPasswords(ruta);

// Contraseña aleatoria del archivo
string randomPw = randomPassword();

Console.Write(randomPw+"\n");

// Encripta la contraseña aleatoria
string pwEncriptado = EncriptarPassword(randomPw);

// Intenta adivinar la contraseña mediante fuerza bruta con hilos
FuerzaBruta(listaPasswords, pwEncriptado);



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
 */

static void FuerzaBruta(List<string> listaPasswords, string pwEncriptado)
{
    Console.WriteLine("Iniciando fuerza bruta...");

    const int numeroHilos = 10;
    int contraseñasPorHilo = listaPasswords.Count / numeroHilos;

    // Lista para almacenar los hilos
    List<Thread> hilos = new List<Thread>();

    for (int i = 0; i < numeroHilos; i++)
    {
        int inicio = i * contraseñasPorHilo;
        int fin = (i == numeroHilos - 1) ? listaPasswords.Count : (i + 1) * contraseñasPorHilo;

        // Crea un hilo para la fuerza bruta en el rango asignado
        Thread hilo = new Thread(() => RealizarFuerzaBruta(listaPasswords, pwEncriptado, inicio, fin));
        hilos.Add(hilo);

        // Inicia el hilo
        hilo.Start();
    }

    // Espera a que todos los hilos terminen
    foreach (Thread hilo in hilos)
    {
        hilo.Join();
    }
}

static void RealizarFuerzaBruta(List<string> listaPasswords, string pwEncriptado, int inicio, int fin)
{
    for (int i = inicio; i < fin; i++)
    {
        string pwActual = listaPasswords[i];

        // Encripta la contraseña actual para comparar con la contraseña encriptada objetivo
        string pwActualEncriptado = EncriptarPassword(pwActual);

        if (pwEncriptado == pwActualEncriptado)
        {
            Console.WriteLine($"Contraseña encontrada: {pwActual}");
            Console.WriteLine($"Hackeo realizado con éxito");
            return;
        }
    }
}
/**
 * Lee el archivo con el diccionario de contraseñas.
 */
static void leerArchivo()
{
    String line;
    try
    {
        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader("C:\\Users\\enriv\\source\\repos\\PruebaEncriptacion\\PruebaEncriptacion\\passwords.txt");
        //Read the first line of text
        line = sr.ReadLine();
        //Continue to read until you reach end of file
        while (line != null)
        {
            //write the line to console window
            Console.WriteLine(line);
            //Read the next line
            line = sr.ReadLine();
        }
        //close the file
        sr.Close();
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
 * Crea un archivo más corto por ejemplo de las 500 primeras contraseñas
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
        while ((line = sr.ReadLine()) != null && lineCount < 500)
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
 */
static string randomPassword()
{
    List<string> listaPasswords = new List<string>(); //To add a list of passwords

    Random random = new Random();
    int randomNumber = random.Next(1, 501); //The lines of passwordPrueba
                                            //We can change in another moment 101 by listaPasswords.Count+1 (the size of the list)

    string line;

    string randomPw;//To add a variabble for the random password
    try
    {
        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader("C:\\Users\\enriv\\source\\repos\\PruebaEncriptacion\\PruebaEncriptacion\\passwordsPrueba.txt");
        //Read the first line of text
        line = sr.ReadLine();
        //Continue to read until you reach end of file
        while (line != null)
        {
            //To add the passwords of the txt to the list
            listaPasswords.Add(line);
            //Read the next line
            line = sr.ReadLine();
        }

        /** We can print all the list with this for but we dont need it
        foreach (var elemento in listaPasswords)
        {
            Console.WriteLine(elemento);
        }
         */

        //We only need a randomPassword from the list
        randomPw = listaPasswords[randomNumber];
        //Console.WriteLine(randomPw); if we want to print it
        return randomPw;

        //close the file
        sr.Close();
        Console.ReadLine();
    }
    catch (Exception e)
    {
        return "Exception: " + e.Message;
    }
    finally
    {
        Console.WriteLine("Executing finally block.");
    }

}




