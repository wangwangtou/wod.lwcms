using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace wod.imageTool.command
{
    public abstract class parameterCommandBase<T>:ICommand where T : class
    {
        public virtual bool CanExecute(object parameter)
        {
            return parameter is T;
        }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public virtual void Execute(object parameter)
        {
            Execute(parameter as T);
        }

        protected abstract void Execute(T parameter);
    }
}
