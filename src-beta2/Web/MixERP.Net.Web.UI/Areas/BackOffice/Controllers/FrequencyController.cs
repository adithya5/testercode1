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

using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MixERP.Net.Common.Helpers;
using MixERP.Net.Entities.Core;
using MixERP.Net.UI.ScrudFactory;
using MixERP.Net.Web.UI.BackOffice.Resources;
using MixERP.Net.Web.UI.Providers;
using Config = MixERP.Net.UI.ScrudFactory.Config;

namespace MixERP.Net.Web.UI.BackOffice.Controllers
{
    [RouteArea("BackOffice", AreaPrefix = "back-office")]
    [RoutePrefix("frequency")]
    [Route("{action=index}")]

    public class FrequencyController : ScrudController
    {
        public ActionResult Index()
        {
            const string view = "~/Areas/BackOffice/Views/Frequency.cshtml";
            return View(view, this.GetConfig());
        }

        public override Config GetConfig()
        {
            Config config = ScrudProvider.GetScrudConfig();
            {
                config.KeyColumn = "frequency_setup_id";

                config.TableSchema = "core";
                config.Table = "frequency_setups";
                config.ViewSchema = "core";
                config.View = "frequency_setup_scrud_view";

                config.DisplayFields = GetDisplayFields();
                config.DisplayViews = GetDisplayViews();

                config.Text = Titles.Frequencies;
                config.ResourceAssembly = Assembly.GetAssembly(typeof(FrequencyController));

               return config;
            }
        }
        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.frequencies.frequency_id",
                ConfigurationHelper.GetDbParameter("FrequencyDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.fiscal_year.fiscal_year_code",
                ConfigurationHelper.GetDbParameter("FiscalYearDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.frequencies.frequency_id", "core.frequency_selector_view");
            ScrudHelper.AddDisplayView(displayViews, "core.fiscal_year.fiscal_year_code", "core.fiscal_year_scrud_view");
            return string.Join(",", displayViews);
        }

    }
}