﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public abstract class DefaultGoogleService<T, U, Z>
    {
        public abstract void CreateService();
        protected abstract T ResponseParser(Z response);
        public abstract U GetData();
        protected abstract void SetData(U itemList);
        public abstract void InsertToDb(U data);
    }
}
