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
using System.Reflection;
using MixERP.Net.Core.Modules.CRM.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.FrontEnd.Controls;

namespace MixERP.Net.Core.Modules.CRM.Setup
{
    public partial class OpportunityStages : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (Scrud scrud = new Scrud())
            {
                scrud.KeyColumn = "opportunity_stage_id";

                scrud.TableSchema = "crm";
                scrud.Table = "opportunity_stages";
                scrud.ViewSchema = "crm";
                scrud.View = "opportunity_stages";

                scrud.Text = Titles.OpportunityStages;
                scrud.ResourceAssembly = Assembly.GetAssembly(typeof (OpportunityStages));

                this.ScrudPlaceholder.Controls.Add(scrud);
            }
        }
    }
}