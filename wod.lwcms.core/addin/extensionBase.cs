using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.addin
{
    public abstract class extensionBase
    {
        public abstract string name {get;}

        protected string getPath(string path)
        {
            return wodEnvironment.GetDataPath("extensions/" + name + "/" + path);
        }

        public delegate void AddPartViewDelegate(commands.commandsParameter cp, web.partView view, web.partView.viewPos pos);

        protected void AddPartView(commands.commandsParameter cp, web.partView view, web.partView.viewPos pos)
        {
            List<web.partView> partView = getPartView(cp, pos.ToString());
            web.partView pview = partView.Find(v => v.name == view.name);
            if (pview == null)
            {
            }
            else
            {
                partView.Remove(pview);
            }
            partView.Add(view);
        }

        private List<web.partView> getPartView(commands.commandsParameter cp, string name)
        {
            List<web.partView> partView = cp.GetObject(name) as List<web.partView>;
            if (partView == null)
            {
                partView = new List<web.partView>();
                cp.AddObject(name, partView);
            }
            return partView;
        }
    }
}
