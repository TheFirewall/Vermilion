using MiNET.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermilion.Modules
{
    public interface IModule
    {
        /// <summary>
        ///     This function will be called on plugin initialization.
        /// </summary>
        void OnEnable(PluginContext context, Vermilion heart);

        /// <summary>
        ///     This function will be called when the plugin will be disabled.s
        /// </summary>
        void OnDisable();
    }
}
