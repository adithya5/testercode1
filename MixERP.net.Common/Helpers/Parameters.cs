﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. 
If a copy of the MPL was not distributed  with this file, You can obtain one at 
http://mozilla.org/MPL/2.0/.
***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MixERP.Net.Common.Helpers
{
    public static class Parameters
    {
        public static string PartyName()
        {
            return GetParameter("PartyName");
        }
        
        private static string GetParameter(string key)
        {
            return Pes.Utility.Helpers.ConfigurationHelper.GetSectionKey("MixERPParameters", key);
        }
    }
}
