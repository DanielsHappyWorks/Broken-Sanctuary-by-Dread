using System;

namespace GDLibrary
{
    //See http://stackoverflow.com/questions/6884653/how-to-make-a-generic-type-cast-function
    public class LanguageUtility
    {
        //performs a cast to a generic type, T, from a type, U - See GenericList::Find()
        public static T ConvertValue<T, U>(U value) where U : IConvertible
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        //performs a cast of any object to a generic type, T - See GenericList::Find()
        public static T ConvertValue<T>(object value)
        {
            return (T)value;  //bug - 4/12/16 //(T)Convert.ChangeType(value, typeof(T));
        }
    }
}
