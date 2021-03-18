using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //params verdiğinde Run() içerisine istediğin kadar parametre verebilirsin
        public static IResult Run(params IResult[] logics)
        {
            //Bütün logic'leri gez hatalı olanı yazdır.
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }
            return null;
        }

    }
}
