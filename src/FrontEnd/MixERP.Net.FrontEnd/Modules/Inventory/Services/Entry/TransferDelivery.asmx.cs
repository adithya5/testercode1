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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using MixER.Net.ApplicationState.Cache;
using MixERP.Net.Common;
using MixERP.Net.Common.Extensions;
using MixERP.Net.Core.Modules.Inventory.Data.Domains;
using MixERP.Net.Core.Modules.Inventory.Data.Transactions;
using MixERP.Net.Entities;
using MixERP.Net.Entities.Models.Transactions;
using MixERP.Net.Framework;
using MixERP.Net.i18n.Resources;
using Serilog;

namespace MixERP.Net.Core.Modules.Inventory.Services.Entry
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class TransferDelivery : WebService
    {
        [WebMethod]
        public long Save(DateTime valueDate, string referenceNumber, string statementReference, long requestId, int sourceStoreId, int shipperId, string data)
        {
            try
            {
                Collection<StockAdjustmentDetail> models = GetModels(data);

                if (requestId <= 0)
                {
                    throw new MixERPException(Warnings.AccessIsDenied);
                }


                if (models == null || models.Count.Equals(0))
                {
                    throw new MixERPException(Warnings.GridViewEmpty);
                }

                if (sourceStoreId <= 0)
                {
                    throw new MixERPException(Warnings.InvalidStore);
                }

                if (shipperId <= 0)
                {
                    throw new MixERPException(Warnings.InvalidShippingCompany);
                }

                int officeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
                int userId = AppUsers.GetCurrent().View.UserId.ToInt();
                long loginId = AppUsers.GetCurrent().View.LoginId.ToLong();

                return StockTransferDelivery.Add(AppUsers.GetCurrentUserDB(), officeId, userId, loginId, requestId, valueDate,
                    referenceNumber, statementReference, shipperId, sourceStoreId, models);
            }
            catch (Exception ex)
            {
                Log.Warning("Could not save inventory transfer entry. {Exception}", ex);
                throw;
            }
        }

        private static Collection<StockAdjustmentDetail> GetModels(string json)
        {
            Collection<StockAdjustmentDetail> models = new Collection<StockAdjustmentDetail>();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic result = jss.Deserialize<dynamic>(json);

            foreach (var item in result)
            {
                StockAdjustmentDetail detail = new StockAdjustmentDetail();
                detail.TransferTypeEnum = TransactionTypeEnum.Debit;

                detail.StoreName = Conversion.TryCastString(item[0]);
                detail.ItemCode = Conversion.TryCastString(item[1]);
                detail.ItemName = Conversion.TryCastString(item[2]);
                detail.UnitName = Conversion.TryCastString(item[3]);
                detail.Quantity = Conversion.TryCastInteger(item[4]);

                models.Add(detail);
            }

            return models;
        }


        [WebMethod]
        public IEnumerable<StockTransferDeliveryModel> GetModel(long requestId)
        {
            if (requestId <= 0)
            {
                throw new MixERPException(Warnings.AccessIsDenied);
            }

            return StockTransferDelivery.GetModel(AppUsers.GetCurrentUserDB(), requestId);
        }
    }
}