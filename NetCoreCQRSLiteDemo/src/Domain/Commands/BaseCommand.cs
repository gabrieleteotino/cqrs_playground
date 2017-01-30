using CQRSlite.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class BaseCommand : ICommand
    {
        public Guid Id { get; set; }

        #region ICommand
        public int ExpectedVersion { get; set; }
        #endregion
    }
}
