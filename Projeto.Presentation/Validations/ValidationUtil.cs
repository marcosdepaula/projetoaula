using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Validations
{
    public class ValidationUtil
    {
        //método para retornar um JSON contendo
        //as mensagens de validação dos campos
        //baseado no ModelState
        public static Hashtable GetErrors(ModelStateDictionary modelState)
        {
            var erros = new Hashtable();

            //percorrer o modelstate
            foreach(var state in modelState)
            {
                //verificar se o campo possui erros de validação
                if(state.Value.Errors.Count > 0)
                {
                    //armazenar os erros de validação do campo no Hashtable
                    erros[state.Key] = state.Value.Errors
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
                }
            }

            return erros;
        }
    }
}
