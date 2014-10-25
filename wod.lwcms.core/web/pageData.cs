using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace wod.lwcms.web
{
    public enum pageType
    {
        category, article, common, index
    }

    public class pageData : wod.lwcms.models.ISeoObject
    {
        public pageData(objectPool _op)
        {
            this.op = _op;
        }

        public wod.lwcms.models.wodsite ws
        {
            get
            {
                return op.getObject<wod.lwcms.models.wodsite>("ws");
            }
        }

        public List<wod.lwcms.models.category> allCats
        {
            get
            {
                return op.getObject<List<wod.lwcms.models.category>>("allCats");
            }
        }

        public wod.lwcms.models.category cat
        {
            get
            {
                return op.getObject<wod.lwcms.models.category>("cat");
            }
        }

        public wod.lwcms.models.article art
        {
            get
            {
                return op.getObject<wod.lwcms.models.article>("art");
            }
        }

        public string pageTransferName
        {
            get
            {
                return op.getObject<string>("pageTransferName");
            }
        }
        public pageType pageType { get; set; }

        public objectPool op { get; private set; }
        
        public string description
        {
            get
            {
                wod.lwcms.models.ISeoObject content,site;
                getSeoObj(out content, out site);
                return content == null || string.IsNullOrEmpty(content.description) ? site.description : content.description;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private void getSeoObj(out wod.lwcms.models.ISeoObject content, out wod.lwcms.models.ISeoObject site)
        {
            content = null;
            site = ws;
            switch (pageType)
            {
                case pageType.category:
                    content = cat;
                    break;
                case pageType.article:
                    content = art;
                    break;
            }
        }

        public string keywords
        {
            get
            {
                wod.lwcms.models.ISeoObject content, site;
                getSeoObj(out content, out site);
                return content == null || string.IsNullOrEmpty(content.keywords) ? site.keywords : content.keywords;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string seotitle
        {
            get
            {
                wod.lwcms.models.ISeoObject content, site;
                getSeoObj(out content, out site);
                return content == null || string.IsNullOrEmpty( content.seotitle) ? site.seotitle : content.seotitle + " - " + site.seotitle;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<partView> getPartViews(partView.viewPos viewPos)
        {
            return op.getObject<List<partView>>(viewPos.ToString());
        }
    }
}
