using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UKnowledge.Web.Helper
{
    public static class Helper
    {
        public static string GetErrorList(IdentityResult result)
        {
            string _errors = "";
            List<string> errors = result.Errors.Select(x => x.Description).ToList();
            foreach (string err in errors)
            {
                _errors += err + " ";
            }
            return _errors;
        }
    }
}
