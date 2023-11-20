
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Diagnostics;
using PruebaEncriptacion;



static void Main()
{
    //leerArchivo();

    //crearArchivoMasCorto();

    String randomPw = randomPassword(); //Contraseña random del archivo

    //Contraseña encriptada
    String randomPwEncriptada = encripta(randomPw);

    try
    {
        String ruta = @"C:\Users\enriv\source\repos\PruebaEncriptacion\PruebaEncriptacion\passwordsPrueba.txt"; // Ruta del archivo a leer
        int division = 0;
        int numeroLineas = 500; //El numero de lineas que tiene el passwordsPrueba
        division = numeroLineas / 2;

        // Realizamos la división del total del número de líneas y los pasamos a variables
        division = numeroLineas / 5;
        int parte2 = division * 2;
        int parte3 = division * 3;
        int parte4 = division * 4;
        int parte5 = division * 5;

        //NOTA: LAS CONTRASEÑAS ME LAS CREA BIEN PERO PASA ALGO CON LOS HILOS QUE NO FUNCIONA

        Thread hilo1 = new Thread(() => fuerzaBruta(0, division, ruta, randomPwEncriptada));
        hilo1.Start();

        Thread hilo2 = new Thread(() => fuerzaBruta(division, parte2, ruta, randomPwEncriptada));
        hilo2.Start();

        Thread hilo3 = new Thread(() => fuerzaBruta(parte2, parte3, ruta, randomPwEncriptada));
        hilo3.Start();

        Thread hilo4 = new Thread(() => fuerzaBruta(parte3, parte4, ruta, randomPwEncriptada));
        hilo4.Start();

        Thread hilo5 = new Thread(() => fuerzaBruta(parte4, parte5, ruta, randomPwEncriptada));
        hilo5.Start();
    
    } catch (Exception ex)
    {
        Console.WriteLine("Error en el código "+ex.ToString());
    }

    
}



/**
 * Explora una sección específica del documento mientras codifica y realiza comparaciones con una cadena proporcionada.
 * 
 */
static void fuerzaBruta(int start, int finish, string path, string pwEncriptado)
{
    Stopwatch sw = new Stopwatch();
    sw.Start();
    using (StreamReader sr2 = new StreamReader(path))
    {
        int numeroLineaActual = 0;
        string newPw = pwEncriptado;
        string linea2;
        string newPw2;
        while ((linea2 = sr2.ReadLine()) != null) // recorremos el archivo línea a línea de nuevo
        {
            // Si la línea actual se encuentra dentro del rango especificado, procedemos a realizar la codificación y la comparación.
            if (Metodos.inRange(numeroLineaActual, start, finish))
            {
                byte[] byteLinea2 = Encoding.UTF8.GetBytes(linea2);
                using (SHA256 sha256 = SHA256.Create())
                {
                    //Codificamos la línea actual
                    byte[] hasBytes2 = sha256.ComputeHash(byteLinea2);
                    StringBuilder sb2 = new StringBuilder();
                    for (int i = 0; i < hasBytes2.Length; i++)
                    {
                        sb2.Append(hasBytes2[i].ToString("x2"));
                    }
                    newPw2 = sb2.ToString();
                    //Comparamos la línea actual codificada con la contraseña dada
                    if (newPw == newPw2)
                    {
                        sw.Stop();
                        Console.WriteLine("La contraseña: ");
                        Console.WriteLine(newPw2);
                        Console.WriteLine("y la contraseña: ");
                        Console.WriteLine(newPw);
                        Console.WriteLine("coinciden, el hackeo ha sido realizado con éxito");
                        Console.WriteLine(sw.Elapsed.ToString());
                        break;
                    }
                }
            }
            numeroLineaActual++;
        }
    }
}

/**
 * Encripta la contraseña que le pasamos como parámetro
 * @param contrasena la contraseña que hemos elegido para encriptar
 * 
 */
static string encripta(String contrasena)
{
    var resultado = String.Empty;
    var convert = SHA256.Create();

    var hashValue = convert.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
    foreach (byte b in hashValue)
    {
        resultado += $"{b:X2}";
    }

    return resultado;
}


/**
 * Leer el archivo con el diccionario de contraseñas.
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
static String randomPassword()
{
    List<string> listaPasswords = new List<string>(); //To add a list of passwords

    Random random = new Random();
    int randomNumber = random.Next(1, 501); //The lines of passwordPrueba
                                            //We can change in another moment 101 by listaPasswords.Count+1 (the size of the list)

    String line;

    String randomPw;//To add a variabble for the random password
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

        //In spanish :XD: tenemos que encriptar esta contraseña y luego intentar acceder a ella a la fuerza bruta
        //Después con varios hilos intentar hacer lo mismo de acceder a ella a la fuerza bruta.


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




