using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AF.Web.Framework
{
    public  class BaseAfValidator<T> : AbstractValidator<T> where T : class
    {
        public BaseAfValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        private void PostInitialize()
        {

        }
    }
}
