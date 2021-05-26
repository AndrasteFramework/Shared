using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andraste.Shared.Lifecycle
{
    #nullable enable
    public class ManagerContainer : IManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly Dictionary<Type, IManager> Managers;

        public ManagerContainer()
        {
            Managers = new Dictionary<Type, IManager>();
        }

        public bool Enabled
        {
            get => true;
            set => throw new InvalidOperationException("Cannot disable ManagerContainer");
        }

        public bool Loaded { get; private set; }

        public void Load()
        {
            foreach (var manager in Managers.Values.Where(manager => !manager.Loaded))
            {
                try
                {
                    manager.Load();
                    manager.Enabled = true;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Exception when Loading or Enabling a Manager");
                }
            }

            Loaded = true;
        }

        public void Unload()
        {
            foreach (var manager in Managers.Values.Where(manager => manager.Loaded))
            {
                try
                {
                    manager.Enabled = false;
                    manager.Unload();
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Exception when Unloading or Disabling a Manager");
                }
            }

            Managers.Clear();
            Loaded = false;
        }

        public void RegisterManager(IManager instance)
        {
            Managers.Add(instance.GetType(), instance);
        }

        public void RegisterManagerAndLoad(IManager instance)
        {
            RegisterManager(instance);
            if (instance.Loaded)
            {
                return;
            }

            try
            {
                instance.Load();
                instance.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Exception when Loading or Enabling a Manager");
            }
        }

        public IManager? GetManager(Type type)
        {
            return Managers.ContainsKey(type) ? Managers[type] : null;
        }
    }
    #nullable restore
}
