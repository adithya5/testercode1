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

using MixERP.Net.FrontEnd.Base;

/********************************************************************************
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
using System.Web.UI.HtmlControls;

namespace MixERP.Net.FrontEnd.Reports
{
    public partial class ReportMaster : MixERPWebpage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            using (HtmlGenericControl iFrame = new HtmlGenericControl())
            {
                iFrame.TagName = "iframe";
                iFrame.Attributes.Add("src", this.ResolveUrl("~/Reports/ReportViewer.aspx?Id=" + this.RouteData.Values["path"]));
                iFrame.Attributes.Add("style", "width:100%;height:100%;border:1px solid #C0C0C0;");
                this.IFramePlaceholder.Controls.Add(iFrame);
            }

            this.OverridePath = "~/Finance/Index.aspx";
        }
    }
}