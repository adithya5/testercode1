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
using MixERP.Net.Entities;
using MixERP.Net.Entities.Transactions;

namespace MixERP.Net.Core.Modules.Sales.Data.Reports
{
    public static class SalesByMonth
    {
        public static IEnumerable<DbGetSalesByOfficesResult> GetSalesByOffice(int officeId)
        {
            return Factory.Get<DbGetSalesByOfficesResult>("SELECT * FROM transactions.get_sales_by_offices(@0, 1000)", officeId);
        }

        public static IEnumerable<DbGetSalesByOfficesResult> GetSalesByOffice()
        {
            return Factory.Get<DbGetSalesByOfficesResult>("SELECT * FROM transactions.get_sales_by_offices(1000)");
        }
    }
}