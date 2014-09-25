﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MixERP.Net.WebControls.ScrudFactory
{
    [ToolboxData("<{0}:ScrudItemSelector runat=server></{0}:ScrudItemSelector>")]
    public partial class ScrudItemSelector : CompositeControl
    {
        public Panel container;
        public DropDownList filterDropDownList;
        public TextBox filterTextBox;
        public Button goButton;
        public GridView searchGridView;
        private bool disposed;

        public sealed override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        protected override void CreateChildControls()
        {
            this.container = new Panel();
            this.AddJavaScript();
            this.LoadItemSelector(this.container);
            this.Initialize();
            this.Controls.Add(this.container);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.container != null)
                    {
                        this.container.Dispose();
                        this.container = null;
                    }

                    if (this.filterDropDownList != null)
                    {
                        this.filterDropDownList.DataBound -= this.FilterDropDownList_DataBound;
                        this.filterDropDownList.Dispose();
                        this.filterDropDownList = null;
                    }

                    if (this.filterTextBox != null)
                    {
                        this.filterTextBox.Dispose();
                        this.filterTextBox = null;
                    }

                    if (this.searchGridView != null)
                    {
                        this.searchGridView.RowDataBound -= this.SearchGridView_RowDataBound;
                        this.searchGridView.Dispose();
                        this.searchGridView = null;
                    }

                    if (this.goButton != null)
                    {
                        this.goButton.Click -= this.GoButton_Click;
                        this.goButton.Dispose();
                        this.goButton = null;
                    }
                }

                this.disposed = true;
            }
        }

        protected override void RecreateChildControls()
        {
            this.EnsureChildControls();
        }

        protected override void Render(HtmlTextWriter w)
        {
            this.container.RenderControl(w);
        }
    }
}