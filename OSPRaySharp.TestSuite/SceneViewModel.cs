using OSPRay.TestSuite.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite
{
    internal abstract class SceneViewModel : INotifyPropertyChanged
    {
        public SceneViewModel(string name)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the name of the scene
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the corresponding render model
        /// </summary>
        public abstract RenderModel RenderModel { get; }

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
