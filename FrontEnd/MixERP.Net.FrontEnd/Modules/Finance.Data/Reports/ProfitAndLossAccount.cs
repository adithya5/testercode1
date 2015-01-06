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
using System.Data;
using MixERP.Net.Common;
using MixERP.Net.Common.Helpers;
using MixERP.Net.DBFactory;
using Npgsql;

namespace MixERP.Net.Core.Modules.Finance.Data.Reports
{
    public static class ProfitAndLossAccount
    {
        public static DataTable GetPLAccount(DateTime from, DateTime to, int userId, int officeId, bool compact, decimal factor)
        {
            const string sql = "SELECT core.get_profit_and_loss_statement(@From::date, @To::date, @UserId, @OfficeId, @Factor, @Compact);";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OfficeId", officeId);
                command.Parameters.AddWithValue("@Factor", factor);
                command.Parameters.AddWithValue("@Compact", compact);

                return JSONHelper.JsonToDataTable(Conversion.TryCastString(DbOperation.GetScalarValue(command)));
            }
        }
    }
}