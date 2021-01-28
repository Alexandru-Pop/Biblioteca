using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Biblioteca
{

    //Construim un validator pentru campurile care trebuiesc neaparat sa fie completate
    public class StringNotEmpty : ValidationRule
    {
        //Mostenim din clasa ValidationRule
        //Suprascriem metoda Validate ce returneaza un ValidationResult
        //Astfel verificam daca utilizatorul a introdus un string valid in cele 2 capuri la care vom aplica prezenta validare (Age, BookFormat)
        //Nu este permisa introducerea unei inregistrari nule
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString == "")
                return new ValidationResult(false, "Eroare! Acest string nu poate fi gol!");
            return new ValidationResult(true, null);
        }
    }

    //Construim un validator pentru lungimea minima a string-ului introdus intr-un camp
    public class StringMinLengthValidator : ValidationRule
    {
        //Vom permite adaugarea datelor exclusiv daca operatorul a introdus un string de minim 4 caractere
        //Aceasta validarea se va efectua doar pentru campurile FirstName, LastName, University, BookTitle, Author, PublishingYear
        //Am specificat anterioara deoarece campul Varsta va fi cel mai probabil format dintr-un string care va avea 2 caractere, iar campul BookFormat poate avea si 3 caractere (in cazul in care format-ul este "PDF")
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString.Length < 4)
                return new ValidationResult(false, "String-urile introduse trebuie sa aiba minim 4 caractere!");
        return new ValidationResult(true, null);
        }
    }
}
