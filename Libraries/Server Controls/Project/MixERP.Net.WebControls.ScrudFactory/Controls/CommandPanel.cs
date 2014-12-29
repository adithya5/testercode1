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

using MixERP.Net.WebControls.ScrudFactory.Resources;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MixERP.Net.WebControls.ScrudFactory.Controls
{
    internal sealed class CommandPanel : IDisposable
    {
        private Panel commandPanel;
        private bool disposed;

        public event EventHandler DeleteButtonClick;

        public event EventHandler EditButtonClick;

        public string AddButtonIconCssClass { get; set; }

        public string AllButtonIconCssClass { get; set; }

        public string ButtonCssClass { get; set; }

        public string CompactButtonIconCssClass { get; set; }

        public string CssClass { get; set; }

        public Button DeleteButton { get; private set; }

        public string DeleteButtonIconCssClass { get; set; }

        public Button EditButton { get; private set; }

        public string EditButtonIconCssClass { get; set; }

        public string PrintButtonIconCssClass { get; set; }

        public string SelectButtonIconCssClass { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public Panel GetCommandPanel(string controlSuffix)
        {
            this.commandPanel = new Panel();
            this.commandPanel.Attributes.Add("role", "toolbar");
            this.commandPanel.CssClass = this.CssClass;
            this.AddSelectButton(this.commandPanel);
            this.AddShowCompactButton(this.commandPanel);
            this.AddShowAllButton(this.commandPanel);
            this.AddAddButton(this.commandPanel);
            this.AddEditButtonHidden(this.commandPanel, controlSuffix);
            this.AddEditButtonVisible(this.commandPanel, controlSuffix);
            this.AddDeleteButtonHidden(this.commandPanel, controlSuffix);
            this.AddDeleteButtonVisible(this.commandPanel, controlSuffix);
            this.AddPrintButton(this.commandPanel);
            return this.commandPanel;
        }

        private void AddAddButton(Panel p)
        {
            HtmlButton addButton = this.GetInputButton("ALT + A", "return(scrudAddNew());", Titles.AddNew, this.ButtonCssClass, this.AddButtonIconCssClass);
            p.Controls.Add(addButton);
        }

        private void AddDeleteButtonHidden(Panel p, string controlSuffix)
        {
            this.DeleteButton = this.GetButton("CTRL + D", "return(scrudConfirmAction());", Titles.DeleteSelected);
            this.DeleteButton.ID = "DeleteButton" + controlSuffix;
            this.DeleteButton.CssClass = "hidden";
            this.DeleteButton.CausesValidation = false;
            this.DeleteButton.Click += this.OnDeleteButtonClick;
            p.Controls.Add(this.DeleteButton);
        }

        private void AddDeleteButtonVisible(Panel p, string controlSuffix)
        {
            HtmlButton deleteButton = this.GetInputButton("CTRL + E", "$('#DeleteButton" + controlSuffix + "').click();return false;", Titles.DeleteSelected, this.ButtonCssClass, this.DeleteButtonIconCssClass);
            p.Controls.Add(deleteButton);
        }

        private void AddEditButtonHidden(Panel p, string controlSuffix)
        {
            this.EditButton = this.GetButton("CTRL + E", "return(scrudConfirmAction());", Titles.EditSelected);
            this.EditButton.Attributes.Add("role", "edit");
            this.EditButton.ID = "EditButton" + controlSuffix;
            this.EditButton.CssClass = "hidden";
            this.EditButton.Click += this.OnEditButtonClick;
            p.Controls.Add(this.EditButton);
        }

        private void AddEditButtonVisible(Panel p, string controlSuffix)
        {
            HtmlButton editButton = this.GetInputButton("CTRL + E", "$('#EditButton" + controlSuffix + "').click();return false;", Titles.EditSelected, this.ButtonCssClass, this.EditButtonIconCssClass);
            p.Controls.Add(editButton);
        }

        private void AddPrintButton(Panel p)
        {
            HtmlButton printButton = this.GetInputButton("CTRL + P", "scrudPrintGridView();", Titles.Print, this.ButtonCssClass, this.PrintButtonIconCssClass);
            p.Controls.Add(printButton);
        }

        private void AddSelectButton(Panel p)
        {
            if (this.IsModal())
            {
                HtmlButton addSelectButton = this.GetInputButton("RETURN", "scrudSelectAndClose();", Titles.Select, this.ButtonCssClass, this.SelectButtonIconCssClass);
                p.Controls.Add(addSelectButton);
            }
        }

        private void AddShowAllButton(Panel p)
        {
            HtmlButton showAllButton = this.GetInputButton("CTRL + S", "scrudShowAll();", Titles.ShowAll, this.ButtonCssClass, this.AllButtonIconCssClass);
            p.Controls.Add(showAllButton);
        }

        private void AddShowCompactButton(Panel p)
        {
            HtmlButton showCompactButton = this.GetInputButton("ALT + C", "scrudShowCompact();", Titles.ShowCompact, this.ButtonCssClass, this.CompactButtonIconCssClass);
            p.Controls.Add(showCompactButton);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.commandPanel != null)
                    {
                        this.commandPanel.Dispose();
                        this.commandPanel = null;
                    }

                    if (this.EditButton != null)
                    {
                        this.EditButton.Click -= this.OnEditButtonClick;
                        this.EditButtonClick = null;
                        this.EditButton.Dispose();
                        this.EditButton = null;
                    }

                    if (this.DeleteButton != null)
                    {
                        this.DeleteButton.Click -= this.OnDeleteButtonClick;
                        this.DeleteButtonClick = null;
                        this.DeleteButton.Dispose();
                        this.DeleteButton = null;
                    }
                }

                this.disposed = true;
            }
        }

        private Button GetButton(string toolTip, string onClientClick, string text)
        {
            using (Button button = new Button())
            {
                button.CssClass = this.ButtonCssClass;
                button.ToolTip = toolTip;
                button.OnClientClick = onClientClick;
                button.Text = text;
                return button;
            }
        }

        private HtmlButton GetInputButton(string title, string onclick, string value, string cssClass, string iconCssClass)
        {
            using (HtmlButton inputButton = new HtmlButton())
            {
                inputButton.Attributes.Add("class", cssClass);
                inputButton.Attributes.Add("type", "button");
                inputButton.Attributes.Add("title", title);
                inputButton.Attributes.Add("onclick", onclick);

                if (!string.IsNullOrWhiteSpace(iconCssClass))
                {
                    inputButton.InnerHtml = @"<i class='" + iconCssClass + "'></i> " + value;
                }
                else
                {
                    inputButton.InnerHtml = value;
                }

                return inputButton;
            }
        }

        private bool IsModal()
        {
            Page page = HttpContext.Current.CurrentHandler as Page;

            if (page != null)
            {
                string modal = page.Request.QueryString["modal"];
                if (modal != null)
                {
                    if (modal.Equals("1"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (this.DeleteButtonClick != null)
            {
                this.DeleteButtonClick(this, e);
            }
        }

        private void OnEditButtonClick(object sender, EventArgs e)
        {
            if (this.EditButtonClick != null)
            {
                this.EditButtonClick(this, e);
            }
        }
    }
}