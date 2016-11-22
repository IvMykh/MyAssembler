using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.UI.ViewModel.Infrastructure
{
    public class DelegateCommand
        : MyCommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action, Func<bool> canExecutePredicate = null)
            : base(canExecutePredicate)
        {
            _action = action;
        }

        public override void Execute(object parameter)
        {
            _action();
        }
    }
}